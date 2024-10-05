import csv
import asyncio
from playwright.async_api import async_playwright
import re
from datetime import datetime, timedelta
import math
from typing import List, Dict, Any, Optional
import logging
import os

# 設置日誌
logging.basicConfig(
    level=logging.INFO,
    format='%(asctime)s - %(levelname)s - %(message)s'
)
logger = logging.getLogger(__name__)

class FacebookScraper:
        

    def __init__(self, email: str, password: str, url):
        if url is None:
            self.target_url = 'https://mbasic.facebook.com/'
        else:
            self.target_url = f'https://mbasic.facebook.com{url}'
        self.email = email
        self.password = password
        self.message_list: List[Dict[str, Any]] = []
        self.post_list: List[Dict[str, Any]] = []
        # 等待時間配置（毫秒）
        self.wait_times = {
            'login': 15000,
            'navigation': 15000,
            'reaction': 50,
            'between_pages': 2000,
            'between_reaction': 500
        }

    async def retry_operation(self, max_attempts: int, operation, *args):
        """重試機制的包裝器"""
        for attempt in range(max_attempts):
            try:
                return await operation(*args)
            except Exception as e:
                if attempt == max_attempts - 1:
                    raise e
                await asyncio.sleep(1)

    @staticmethod
    def parse_post_time(post_time: str) -> str:
        """解析貼文時間"""
        if "昨天" in post_time:
            time = datetime.now() - timedelta(days=1)
        elif "小時" in post_time:
            hours_match = re.search(r"(\d+) 小時", post_time)
            if hours_match:
                hours = int(hours_match.group(1))
                time = datetime.now() - timedelta(hours=hours)
        elif "分鐘" in post_time:
            minutes_match = re.search(r"(\d+) 分鐘", post_time)
            if minutes_match:
                minutes = int(minutes_match.group(1))
                time = datetime.now() - timedelta(minutes=minutes)
        elif "剛剛" in post_time:
            time = datetime.now()
        else:
            date_match = re.search(r"(\d+月\d+日)", post_time)
            if date_match:
                return date_match.group(1)
            return post_time
        return time.strftime("%m月%d日")

    @staticmethod
    def get_id(url: str) -> str:
        """從URL中提取ID"""
        id_match = re.search(r'(?<=\?|&)id=(\d+)', url)
        if id_match:
            return id_match.group(1)
        id_news_match = re.search(r'/([^/?]+)', url)
        return id_news_match.group(1) if id_news_match else "N/A"

    @staticmethod
    def deal_with_count(count: str) -> float:
        """處理計數字串"""
        count = float(count.replace(",", ""))
        if count > 50:
            logger.info(f'Total Reply = {count}, catch top 50')
            count = 50
        return count

    @staticmethod
    def save_to_csv(status: str, data: List[Dict[str, Any]]) -> None:
        """將數據保存到CSV文件"""
        if not data:
            logger.warning(f"No data to save for status: {status}")
            return
        
        directory = 'scrapeResp'
        if not os.path.exists(directory):
            os.makedirs(directory)

        current_time = datetime.now().strftime("%Y%m%d_%H%M%S")
        csv_name = f'{status}_{current_time}.csv'
        fieldnames = list(data[0].keys())
        
        file_path = os.path.join(directory, csv_name)

        try:
            with open(file_path, mode='w', newline='', encoding='utf-8') as file:
                writer = csv.DictWriter(file, fieldnames=fieldnames)
                writer.writeheader()
                writer.writerows(data)
            logger.info(f"Successfully saved data to {csv_name}")
        except Exception as e:
            logger.error(f"Error saving to CSV: {e}")

    async def wait_and_verify(self, page, timeout: int, message: str):
        """等待並驗證頁面狀態"""
        logger.info(f"Waiting {timeout}ms for {message}")
        await page.wait_for_timeout(timeout)
        await page.wait_for_load_state('networkidle')

    async def scrape_reactions(self, index: int, page, like_type_element, count: str, reaction_type: str) -> List[Dict[str, Any]]:
        """爬取反應數據"""
        await self.retry_operation(3, like_type_element.click)
        await page.wait_for_load_state('networkidle')
        reaction_list = []
        count = self.deal_with_count(count)
        scrape_times = math.ceil(count / 10)
        
        logger.info(f"Scraping {scrape_times} pages for {reaction_type} reactions")

        for i in range(scrape_times):
            await page.wait_for_selector('li', state='visible')
            who_like = await self.retry_operation(3, page.query_selector_all, 'li')
            logger.info(f"Found {len(who_like)} items on page {i+1}")

            for who in who_like:
                try:
                    if await self._process_next_button(who, page):
                        continue
                    
                    who_data = await self._extract_who_data(who, index)
                    if who_data:
                        reaction_list.append(who_data)
                        # 在每個反應處理之間添加等待
                        await page.wait_for_timeout(self.wait_times['reaction'])
                except Exception as e:
                    logger.error(f"Error processing reaction: {e}")

        logger.info(f"Scraped {len(reaction_list)} {reaction_type} reactions in total")
        return reaction_list

    async def _process_next_button(self, who, page) -> bool:
        """處理'下一頁'按鈕"""
        next_check = await self.retry_operation(3, who.query_selector, 'span')
        if next_check:
            next_button = await self.retry_operation(3, who.query_selector, 'a')
            await next_button.click()
            await page.wait_for_load_state('networkidle')
            await page.wait_for_timeout(self.wait_times['between_reaction'])
            return True
        return False

    async def _extract_who_data(self, who, index: int) -> Optional[Dict[str, Any]]:
        """提取用戶數據"""
        who_element = await self.retry_operation(3, who.query_selector, 'header a')
        if not who_element:
            return None

        who_name = await self.retry_operation(3, who_element.inner_text)
        who_url = await self.retry_operation(3, who_element.get_attribute, 'href')
        who_id = self.get_id(who_url)
        
        return {
            'post': index,
            'name': who_name,
            'id': who_id,
            'url': who_url
        }

    async def _process_likes(self, post_footer_element, context, mainpage, index: int):
        """處理貼文的讚和反應"""
        post_like_element = await self.retry_operation(3, post_footer_element.query_selector, 'a')
        if not post_like_element:
            return

        post_like_url = await self.retry_operation(3, post_like_element.get_attribute, 'href')
        likepage = await context.new_page()
        
        try:
            await likepage.goto('https://mbasic.facebook.com' + post_like_url)
            await likepage.wait_for_load_state('networkidle')
            await mainpage.wait_for_timeout(self.wait_times['between_pages'])

            storyid = self._extract_story_id(post_like_url)
            if not storyid:
                logger.error('Could not extract story ID')
                return

            await self._process_story_reactions(likepage, storyid, index)
        finally:
            await likepage.close()

    def _extract_story_id(self, url: str) -> Optional[str]:
        """從URL中提取貼文ID"""
        if "story_fbid" in url:
            match = re.search(r'story_fbid=([^/&]+)', url)
        elif "groups" in url:
            match = re.search(r'permalink/(\d+)/', url)
        else:
            logger.error('Unsupported URL format')
            return None
        
        return match.group(1) if match else None

    async def _process_story_reactions(self, likepage, storyid: str, index: int):
        """處理貼文的反應"""
        story_like_element = await self.retry_operation(3, likepage.query_selector, f'#sentence_{storyid} > a')
        if not story_like_element:
            return

        story_like_url = await self.retry_operation(3, story_like_element.get_attribute, 'href')
        if not story_like_url:
            return

        await likepage.goto('https://mbasic.facebook.com' + story_like_url)
        await likepage.wait_for_load_state('networkidle')
        
        like_type_elements = await self.retry_operation(3, likepage.query_selector_all, 'a[role="button"]')
        
        for i in range(len(like_type_elements)):
            await self._process_single_reaction_type(likepage, i, index)

    async def _process_single_reaction_type(self, likepage, i: int, index: int):
        """處理單個反應類型"""
        like_type_element = await self.retry_operation(3, likepage.query_selector, f'a[role="button"]:nth-of-type({i + 1})')
        await likepage.wait_for_timeout(self.wait_times['reaction'])
        
        type_element = await self.retry_operation(3, like_type_element.query_selector, 'img')
        if not type_element:
            return

        reaction_type = await self.retry_operation(3, type_element.get_attribute, 'alt')
        count_element = await self.retry_operation(3, like_type_element.query_selector, 'span')
        
        if count_element and reaction_type in ["讚", "哈"]:
            count = await self.retry_operation(3, count_element.inner_text)
            logger.info(f"Processing {reaction_type} reactions")
            reaction_list = await self.scrape_reactions(index, likepage, like_type_element, count, reaction_type)
            self.message_list.extend(reaction_list)
            logger.info(f"Processed {len(reaction_list)} {reaction_type} reactions")

    async def scrape(self):
        """主要爬蟲函數"""
        async with async_playwright() as p:
            browser = await p.chromium.launch(headless=False)
            context = await browser.new_context()
            mainpage = await context.new_page()

            try:
                await self._login(mainpage)
                await self._navigate_to_target(mainpage)
                await self._process_articles(mainpage, context)
            except Exception as e:
                logger.error(f"Error during scraping: {e}")
            finally:
                await browser.close()

            self.save_to_csv('Post', self.post_list)
            self.save_to_csv('Reply', self.message_list)

    async def _login(self, page):
        """登入Facebook"""
        await page.goto('https://www.facebook.com/')
        await page.fill('input[name="email"]', self.email)
        await page.fill('input[name="pass"]', self.password)
        await page.click('button[name="login"]')
        await self.wait_and_verify(page, self.wait_times['login'], "login completion")

    async def _navigate_to_target(self, page):
        """導航到目標頁面"""
        await page.goto(self.target_url)
        await self.wait_and_verify(page, self.wait_times['navigation'], "navigation to target page")

    async def _process_articles(self, mainpage, context):
        """處理文章"""
        personal = await mainpage.query_selector('a:has-text("動態時報")')
        if personal != None:
            personal_url = await self.retry_operation(3, personal.get_attribute, 'href')
            await mainpage.goto('https://mbasic.facebook.com' + personal_url)
            await self.wait_and_verify(mainpage, self.wait_times['navigation'], "navigation to target page")


        articles = await mainpage.query_selector_all('section article')
        if not articles:
            logger.warning("No articles found")
            return

        for index, article in enumerate(articles):
            try:
                post_data = await self._process_single_article(index, article, context, mainpage)
                if post_data:
                    self.post_list.append(post_data)
                # 在每篇文章處理之間添加等待
                await mainpage.wait_for_timeout(self.wait_times['between_pages'])
            except Exception as e:
                logger.error(f"Error processing article {index}: {e}")

    async def _process_single_article(self, index: int, article, context, mainpage) -> Optional[Dict[str, Any]]:
        """處理單篇文章"""
        logger.info(f"Processing article {index}")
        post_dict = {'post': index}

        # 提取發文者信息
        poster_element = await self.retry_operation(3, article.query_selector, 'header h3 span strong a')
        if poster_element:
            poster_name = await self.retry_operation(3, poster_element.inner_text)
            poster_url = await self.retry_operation(3, poster_element.get_attribute, 'href')
            poster_id = self.get_id(poster_url)
            post_dict.update({
                'name': poster_name,
                'id': poster_id,
                'url': poster_url
            })
            logger.info(f"Extracted poster info: {poster_name}")

        # 提取貼文內容
        post_content_element = await self.retry_operation(3, article.query_selector, 'div[data-ft]')
        if post_content_element:
            post_content = await self.retry_operation(3, post_content_element.inner_text)
            post_dict['content'] = post_content
            logger.info("Extracted post content")

        # 提取貼文時間和反應
        post_footer_elements = await self.retry_operation(3, article.query_selector_all, 'footer > div')
        if post_footer_elements:
            for i, post_footer_element in enumerate(post_footer_elements):
                if i == 0:
                    # 處理時間
                    post_time_element = await self.retry_operation(3, post_footer_element.query_selector, 'abbr')
                    if post_time_element:
                        post_time = await self.retry_operation(3, post_time_element.inner_text)
                        post_dict['time'] = self.parse_post_time(post_time)
                        logger.info(f"Extracted post time: {post_dict['time']}")
                else:
                    # 處理讚和反應
                    post_like_element = await self.retry_operation(3, post_footer_element.query_selector, 'a')
                    if post_like_element:
                        post_url = await self._process_likes_for_article(post_like_element, context, mainpage, index)
                        post_dict['post_url'] = post_url

        return post_dict if post_dict else None

    async def _process_likes_for_article(self, post_like_element, context, mainpage, index: int):
        """處理文章的讚和反應"""
        post_like_url = await self.retry_operation(3, post_like_element.get_attribute, 'href')
        likepage = await context.new_page()
        
        try:
            await likepage.goto('https://mbasic.facebook.com' + post_like_url)
            await likepage.wait_for_load_state('networkidle')
            await mainpage.wait_for_timeout(self.wait_times['between_pages'])

            # 提取貼文ID
            storyid = None
            if "story_fbid" in post_like_url:
                storyid = re.search(r'story_fbid=([^/&]+)', post_like_url)
            elif "groups" in post_like_url:
                storyid = re.search(r'permalink/(\d+)/', post_like_url)
            
            if not storyid:
                logger.error('Could not extract story ID')
                return
            
            storyid = storyid.group(1)
            story_like_element = await self.retry_operation(3, likepage.query_selector, f'#sentence_{storyid} > a')
            if story_like_element:
                story_like_url = await self.retry_operation(3, story_like_element.get_attribute, 'href')
                if story_like_url:
                    await likepage.goto('https://mbasic.facebook.com' + story_like_url)
                    await likepage.wait_for_load_state('networkidle')
                    
                    # 處理不同類型的反應
                    like_type_elements = await self.retry_operation(3, likepage.query_selector_all, 'a[role="button"]')
                    for i in range(len(like_type_elements)):
                        await self._process_reaction_type(likepage, i, index)

        except Exception as e:
            logger.error(f"Error processing likes for article {index}: {e}")
        finally:
            await likepage.close()
        return post_like_url

    async def _process_reaction_type(self, likepage, i: int, index: int):
        """處理單個反應類型"""
        try:
            like_type_element = await self.retry_operation(3, likepage.query_selector, f'a[role="button"]:nth-of-type({i + 1})')
            await likepage.wait_for_timeout(self.wait_times['reaction'])
            
            type_element = await self.retry_operation(3, like_type_element.query_selector, 'img')
            if not type_element:
                return

            reaction_type = await self.retry_operation(3, type_element.get_attribute, 'alt')
            count_element = await self.retry_operation(3, like_type_element.query_selector, 'span')

            if count_element and reaction_type in ["讚", "哈"]:
                count = await self.retry_operation(3, count_element.inner_text)
                logger.info(f"Processing {reaction_type} reactions")
                reaction_list = await self.scrape_reactions(index, likepage, like_type_element, count, reaction_type)
                self.message_list.extend(reaction_list)
                logger.info(f"Processed {len(reaction_list)} {reaction_type} reactions")

        except Exception as e:
            logger.error(f"Error processing reaction type {i} for article {index}: {e}")

def Scraper(url = None):
    """主程序入口"""
    try:
        scraperResp = FacebookScraper('hao104078.2@gmail.com', '!QAZ@WSX3edc', url)
        asyncio.run(scraperResp.scrape())
    except Exception as e:
        logger.error(f"Main program error: {e}")

if __name__ == "__main__":
    Scraper()

