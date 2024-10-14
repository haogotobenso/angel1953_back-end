from playwright.sync_api import sync_playwright
from datetime import datetime
import re


def scrape_with_playwright():
    with sync_playwright() as p:

        browser = p.chromium.launch(headless=True) 
        page = browser.new_page()

        page.goto('https://eliteracy.edu.tw/Search.aspx?q=網路霸凌')


        focus_news_section = page.locator('h2:has-text("焦點新聞")')
        if focus_news_section:
            news_links = page.locator('h2:has-text("焦點新聞") ~ a')
            count = min(news_links.count(), 10)
            for i in range(count):
                title = news_links.nth(i).text_content().strip()
                link = news_links.nth(i).get_attribute('href')
                print(f'{i + 1}. 新聞標題: {title}')
                print(f'{i + 1}. 新聞連結: https://eliteracy.edu.tw/%7Blink%7D')

                news_page = browser.new_page()
                news_page.goto(f'https://eliteracy.edu.tw/%7Blink%7D')
                date_info = news_page.locator('p[style="clear:both; color:gray"]').text_content()

                date_match = re.search(r'\d{4}年\d{1,2}月\d{1,2}日', date_info)
                if date_match:
                    date_str = date_match.group(0)
                    date_obj = datetime.strptime(date_str, '%Y年%m月%d日')
                    print(f'   發佈日期: {date_obj}')
                else:
                    print('   無法提取日期')

                news_page.close()
                print()

        browser.close()

scrape_with_playwright()