using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using angel1953_backend.Models;
using angel1953_backend.Dtos;
using System.Reflection.Metadata.Ecma335;
using System.Linq.Expressions;

namespace angel1953_backend.Repository
{
    public class BackRepository
    {
        private angel1953Context _context;
        public BackRepository(angel1953Context angel1953context) 
        {
            _context = angel1953context;
        } 
        #region 上傳書籍
        public void UpdateBook(List<Book> NewBook)
        {
            try
            {
                // // 刪除舊資料
                // _context.Book.RemoveRange(_context.Book);
                // _context.SaveChanges();

                // 插入新資料
                _context.Book.AddRange(NewBook);
                _context.SaveChanges();
            }
            catch(Exception ex)
            {
                throw new Exception(ex.ToString());
            }
        }
        #endregion
        #region 取得所有書籍
        public List<Book> getBooks()
        {
            try
            {
                List<Book> booklist = _context.Book.ToList();
                return booklist;
            }
            catch(Exception ex)
            {
                throw new Exception(ex.ToString());
            }
        }
        #endregion
        #region 刪除一筆書籍
        public bool deleteOneBook(int bookid)
        {
            try
            {
                Book book = _context.Book.Where(b=>b.BookId == bookid).SingleOrDefault();
                if(book == null)
                {
                    return false;
                }
                else
                {
                    _context.Book.Remove(book);
                    _context.SaveChanges();
                    return true;
                }
                
            }
            catch(Exception ex)
            {
                throw new Exception(ex.ToString());
            }
        }
        #endregion
        #region 新增一筆影片
        public string addOneVideo(VideoLink video)
        {
            try
            {
                _context.VideoLink.Add(video);
                _context.SaveChanges();
                return "影片資訊新增完成";
            }
            catch(Exception ex)
            {
                throw new Exception(ex.ToString());
            }
        }
        #endregion

        #region 取得所有影片
        public List<VideoLinksDto> getAllVideos()
        {
            try
            {
                List<VideoLinksDto> videos = _context.VideoLink.Select(v => new VideoLinksDto
                {
                    VideoId = v.VideoId,
                    VideoName = v.VideoName,
                    ImgInnerUrl = $"/api/Back/{v.VideoId}/GetVideoImg",
                    VideoLink1 = v.VideoLink1,
                    LinkClick = v.LinkClick
                })
                .ToList();
                return videos;
            }
            catch(Exception ex)
            {
                throw new Exception(ex.ToString());
            }
        }

        #endregion 

        #region 取得影片圖片
        public byte[] getVideoImg(int id)
        {
            try
            {
               
                  byte[] img = _context.VideoLink.Where(v => v.VideoId == id).Select(v => v.VideoImg).FirstOrDefault();
                  return img;
            }
            catch(Exception ex)
            {
                throw new Exception(ex.ToString());
            }
        }
        #endregion

        #region 刪除一筆影片
        public bool deleteOneVideo(int Videoid)
        {
            try
            {
                VideoLink video = _context.VideoLink.Where(v=>v.VideoId == Videoid).SingleOrDefault();
                if(video==null)
                {
                    return false;
                }
                else
                {
                    _context.VideoLink.Remove(video);
                    _context.SaveChanges();
                    return true;
                }
                
            }
            catch(Exception ex)
            {
                throw new Exception(ex.ToString());
            }
        }
        #endregion

        #region 取得題目資訊
        public List<Question> getQuestionList()
        {
             try
            {
                List<Question> questions = _context.Question.ToList();
                return questions;

            }
            catch (Exception ex)
            {
                throw new Exception (ex.ToString());
            }
        }
        #endregion

        #region 新增FB爬蟲連結
        public string addFBLink(CrawlerLink FBLink)
        {
            try
            {
                _context.CrawlerLink.Add(FBLink);
                _context.SaveChanges();
                return "新增Facebook爬蟲連結成功";

            }
            catch(Exception ex)
            {
                throw new Exception(ex.ToString());
            }
        }
        #endregion

        #region 取得FB爬蟲連結表
        public List<CrawlerLink> getFBLinkList()
        {
            try
            {
                List<CrawlerLink> LinkList = _context.CrawlerLink.ToList();
                return LinkList;
            }
            catch(Exception ex)
            {
                throw new Exception(ex.ToString());
            }
        }
        #endregion 

        #region 刪除一筆FB爬蟲連結
        public bool deleteFBLink(int FBLink)
        {
            try
            {
                CrawlerLink Link = _context.CrawlerLink.Where(l=>l.LinkId ==FBLink).FirstOrDefault();
                
                if(Link==null)
                {
                    return false;
                }
                else
                {
                    _context.Remove(Link);
                    _context.SaveChanges();
                    return true;
                }
            }
            catch(Exception ex)
            {
                throw new Exception(ex.ToString());
            }

        }
        #endregion

        #region 取得素養網路爬蟲資料
        public List<ExternalLinks> getExtLink()
        {
            try
            {
                List<ExternalLinks> LinkList = _context.ExternalLinks.ToList();
                return LinkList;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.ToString());
            }
        }
        #endregion
        
    }
}