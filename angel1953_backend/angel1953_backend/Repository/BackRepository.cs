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
        public string uploadBook(Book NewBook)
        {
            try
            {
                // // 刪除舊資料
                // _context.Book.RemoveRange(_context.Book);
                // _context.SaveChanges();

                // 插入新資料
                _context.Book.AddRange(NewBook);
                _context.SaveChanges();
                return "上傳成功";
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

        #region 取得所有事件檢視資訊
        public List<ShowCaseDto> showCase()
        {
            try
            {
                var query = from bp in _context.BullyingerPost
                            join b in _context.Bullyinger on bp.BullyingerId equals b.BullyingerId
                            select new ShowCaseDto
                            {
                                BPId = bp.BPId,
                                PostTime = bp.PostTime,
                                FirstDate = b.FirstDate,
                                Posturl = bp.Posturl,
                                Bullyinger = b.Bullyinger1,
                                FBurl = b.FBurl
                            };

                return query.ToList(); // 明確調用 ToList() 方法


            }
            catch(Exception ex)
            {
                throw new Exception(ex.ToString());
            }

        }
        #endregion
        #region 取得一筆事件檢視資訊
        public ShowCaseDetailDto showCaseDetail(int bpid)
        {
            try
            {
                var detail =  (from bp in _context.BullyingerPost
                            join b in _context.Bullyinger on bp.BullyingerId equals b.BullyingerId
                            where bp.BPId == bpid
                            select new ShowCaseDetailDto
                            {
                                BPId = bp.BPId,
                                Bullyinger = b.Bullyinger1,
                                PostTime = bp.PostTime,
                                FirstDate = b.FirstDate,
                                PostInfo = bp.PostInfo,
                                Posturl = bp.Posturl,
                                KeyWord = bp.KeyWord
                            }).FirstOrDefault();
                return detail;

            }
            catch(Exception ex)
            {
                throw new Exception(ex.ToString());
            }
        }
        #endregion

        #region 取得使用者資訊清單
        public List<UserInfoDto> userInfo (string TorCAccount)
        {
            try
            {
                Member BackUser = _context.Member.Where(u=>u.Account == TorCAccount).SingleOrDefault();
                if(BackUser.IsTeacher == 1)
                {
                    // 查詢Member表，並篩選出符合SchoolId和ClassId的記錄
                    var query = from member in _context.Member
                    where member.SchoolId == BackUser.SchoolId && member.ClassId == BackUser.ClassId && member.IsTeacher == 0
                    join bullyinger in _context.Bullyinger
                    on new { member.Account, member.FBurl } equals new { bullyinger.Account, bullyinger.FBurl } into memberBullyinger
                    from bully in memberBullyinger.DefaultIfEmpty() // 左連接，允許找不到對應的bullyinger資料
                    select new UserInfoDto
                    {
                        Account = member.Account,
                        Name = member.Name,
                        School = member.School != null ? member.School.School1 : string.Empty, // 如果School是null, 預設為空字串
                        Class = member.Class != null ? member.Class.Class1 : string.Empty,   // 如果Class是null, 預設為空字串
                        State = bully == null ? "安全" : bully.State == 1 ? "警示" : bully.State == 2 ? "危險" : "未知",
                        Todo0 = _context.Todo.Any(t => t.Account == member.Account && t.TodoThing == 0 && t.State == false) ? false : true,
                        Todo1 = _context.Todo.Any(t => t.Account == member.Account && t.TodoThing == 1 && t.State == false) ? false : true,
                        Todo2 = _context.Todo.Any(t => t.Account == member.Account && t.TodoThing == 2 && t.State == false) ? false : true
                    };

                    return query.ToList();

                }
                else if(BackUser.IsTeacher == 2)
                {
                    // 查詢Member表，並篩選出符合SchoolId和ClassId的記錄
                    var query = from member in _context.Member
                    where member.SchoolId == BackUser.SchoolId && member.IsTeacher == 0
                    join bullyinger in _context.Bullyinger
                    on new { member.Account, member.FBurl } equals new { bullyinger.Account, bullyinger.FBurl } into memberBullyinger
                    from bully in memberBullyinger.DefaultIfEmpty() // 左連接，允許找不到對應的bullyinger資料
                    select new UserInfoDto
                    {
                        Account = member.Account,
                        Name = member.Name,
                        School = member.School != null ? member.School.School1 : string.Empty, // 如果School是null, 預設為空字串
                        Class = member.Class != null ? member.Class.Class1 : string.Empty,   // 如果Class是null, 預設為空字串
                        State = bully == null ? "安全" : bully.State == 1 ? "警示" : bully.State == 2 ? "危險" : "未知",
                        Todo0 = _context.Todo.Any(t => t.Account == member.Account && t.TodoThing == 0 && t.State == false) ? false : true,
                        Todo1 = _context.Todo.Any(t => t.Account == member.Account && t.TodoThing == 1 && t.State == false) ? false : true,
                        Todo2 = _context.Todo.Any(t => t.Account == member.Account && t.TodoThing == 2 && t.State == false) ? false : true
                    };
                    return query.ToList();
                }
                List<UserInfoDto> EmptyDto = new List<UserInfoDto>();
                return EmptyDto;

            }
            catch (Exception ex)
            {
                throw new Exception(ex.ToString());
            }

        }
        #endregion
        #region 取得一筆使用者資訊
        public UserInfoDetailDto getOneUserInfo(string user,string account)
        {
            try
            {
                Member BackUser = _context.Member.Where(u=>u.Account == user).SingleOrDefault();
                var query = from member in _context.Member
                where member.SchoolId == BackUser.SchoolId && member.IsTeacher == 0 && member.Account == account
                join bullyinger in _context.Bullyinger
                on new { member.Account, member.FBurl } equals new { bullyinger.Account, bullyinger.FBurl } into memberBullyinger
                from bully in memberBullyinger.DefaultIfEmpty() // 左連接，允許找不到對應的bullyinger資料
                select new UserInfoDetailDto
                {
                    Name = member.Name,
                    Account = member.Account,
                    Email = member.Email,
                    School = member.School != null ? member.School.School1 : string.Empty, // 如果School是null, 預設為空字串
                    Class = member.Class != null ? member.Class.Class1 : string.Empty,   // 如果Class是null, 預設為空字串
                    StudentId = member.StudentId,
                    FBurl = member.FBurl,
                    State = bully == null ? "安全" : bully.State == 1 ? "警示" : bully.State == 2 ? "危險" : "未知",
                    BullyingerPoint = bully == null ? 0 : bully.BullyingerPoint,
                    BullyingerPost = bully == null ? 0 : _context.BullyingerPost.Count(p => p.BullyingerId == bully.BullyingerId),
                    Todo = (from todo in _context.Todo 
                            where todo.Account == member.Account
                            select new TodoBackDto
                            {
                                TodoName = todo.TodoThing  == 0 ? "影片觀賞" : todo.TodoThing == 1 ? "素養題目" : todo.TodoThing == 2 ? "學校人員處理" : "未知",
                                TodoState = todo.State == false ? "未完成" : todo.State == true ? "已完成" :"未知"
                            }).ToList()
                };
                return query.FirstOrDefault();
                

            }
            catch(Exception ex)
            {
                throw new Exception(ex.ToString());
            }
        }
        #endregion

        #region 學生todo老師處理狀態變更
        public bool userTodoChage(string user,string account)
        {
            try
            {
                Member BackUser = _context.Member.Where(u=>u.Account == user).SingleOrDefault();
                var studentTodo2 = _context.Todo.Where(t=>t.Account == account && t.TodoThing ==2 && t.State == false).FirstOrDefault();
                if(studentTodo2 !=null)
                {
                    studentTodo2.State = true;
                    _context.SaveChanges();
                    return true;
                }
                else
                {
                    return false;
                }

            }
            catch(Exception ex)
            {
                throw new Exception(ex.ToString());
            }

        }
        #endregion

        #region 取得紅綠燈圖表
        public ChartDto getStateChart(string user)
        {
            var data = new ChartDto();
            try
            {
                Member BackUser = _context.Member.Where(u=>u.Account == user).SingleOrDefault();
                var query = from member in _context.Member
                where member.SchoolId == BackUser.SchoolId && member.IsTeacher == 0
                join bullyinger in _context.Bullyinger
                on new { member.Account, member.FBurl } equals new { bullyinger.Account, bullyinger.FBurl } into memberBullyinger
                from bully in memberBullyinger.DefaultIfEmpty() // 左連接，允許找不到對應的bullyinger資料
                let state = bully == null ? "安全" : bully.State == 1 ? "警示" : bully.State == 2 ? "危險" : "未知"
                group state by state into stateGroup
                select new 
                {
                    State = stateGroup.Key,       // 狀態名稱（安全、警示、危險）
                    Count = stateGroup.Count()    // 該狀態的個數
                };
                var State = new StateShow
                {
                    Green = query.FirstOrDefault(x => x.State == "安全")?.Count ?? 0,
                    Yellow = query.FirstOrDefault(x => x.State == "警示")?.Count ?? 0,
                    Red = query.FirstOrDefault(x => x.State == "危險")?.Count ?? 0
                };
                data.Labels = ["安全","警示","危險"];
                data.Datasets = new[]
                {
                    new ChartDatasetDto
                    {
                        Label = "本校社群紅綠燈統計圓餅圖",
                        Data = [State.Green,State.Yellow,State.Red],
                        BorderWidth = 1
                    }
                };
                return data;
                
            }
            catch(Exception ex)
            {
                throw new Exception(ex.ToString());

            }
        }
        #endregion

        #region 取得本月霸凌貼文折線圖
        public ChartDto getBullyingPostChart(string user)
        {
            var data = new ChartDto();
            // 取得當前時間和當月的第一天與最後一天
            var now = DateTime.Now;
            var firstDayOfMonth = new DateTime(now.Year, now.Month, 1);
            var lastDayOfMonth = firstDayOfMonth.AddMonths(1).AddDays(-1);
            var allDatesInMonth = Enumerable.Range(1, DateTime.DaysInMonth(now.Year, now.Month))
                                  .Select(day => new DateTime(now.Year, now.Month, day).ToString("yyyy-MM-dd"))
                                  .ToArray();
            try
            {
                // 查詢並按每天分組計算貼文次數
                var query = from bp in _context.BullyingerPost
                where bp.PostTime.HasValue && bp.PostTime.Value >= firstDayOfMonth && bp.PostTime.Value <= lastDayOfMonth
                group bp by bp.PostTime.Value.Date into postGroup
                select new
                {
                    Date = postGroup.Key,    // 分組的日期
                    PostCount = postGroup.Count()   // 該日期的貼文數量
                };
                var result = query.ToList();
                var chartData = new
                {
                    Dates = result.Select(r => r.Date.ToString("yyyy-MM-dd")).ToArray(),  // 將 DateTime 轉為字串陣列
                    PostCounts = result.Select(r => r.PostCount).ToArray()                // 將貼文數量轉為整數陣列
                };
                data.Labels = allDatesInMonth;
                data.Datasets = new[]
                {
                    new ChartDatasetDto
                    {
                        Label = "本月每日霸凌數量折線圖",
                        Data = allDatesInMonth.Select(date => result.FirstOrDefault(r => r.Date.ToString("yyyy-MM-dd") == date)?.PostCount ?? 0).ToArray(),
                        BorderWidth = 1
                    }
                };
                return data;

            }
            catch(Exception ex)
            {
                throw new Exception(ex.ToString());
            }

        }
        #endregion
    }
}       