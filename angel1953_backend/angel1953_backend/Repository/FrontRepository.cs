using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using angel1953_backend.Models;
using angel1953_backend.Dtos;
using System.Reflection.Metadata.Ecma335;

namespace angel1953_backend.Repository
{
    public class FrontRepository
    {
        private angel1953Context _context;
        public FrontRepository(angel1953Context angel1953context) 
        {
            _context = angel1953context;
        }

        #region 顯示題目
        public List<QuestionDto> getQuestion()
        {
            
            try
            {
                var questions = _context.Question.ToList();
                var random = new Random();

                var randomizedQuestions = questions.Select(q => new QuestionDto {
                    QuestionId = q.QuestionId,
                    Question = q.Question1,
                    Options = new List<string> { q.Option1, q.Option2, q.Option3, q.Answer }.OrderBy(x => random.Next()).ToList()
                }).ToList();
                return randomizedQuestions;

            }
            catch (Exception ex)
            {
                throw new Exception (ex.ToString());
            }
        }
        #endregion

        #region 對答案
        public string ansQuestion(List<AnswerDto> ans, string account)
        {
            try
            {
                var recovery = new Recovery
                {
                    Account = account,
                    Time = DateTime.Now,
                    Correct = 0
                };
                _context.Recovery.Add(recovery);
                _context.SaveChanges(); // 保存 Recovery 記錄，讓其獲取生成的 RecoveryId

                int correctCount = 0; // 記錄答對的數量

                // 遍歷每個作答記錄
                foreach (var answer in ans)
                {
                    // 從資料庫獲取題目
                    var question = _context.Question.FirstOrDefault(q => q.QuestionId == answer.QuestionId);

                    if (question == null)
                    {
                        return "無效的問題 ID";
                    }

                    // 比對使用者答案是否正確
                    if (answer.UserAnswer == question.Answer)
                    {
                        correctCount++; // 答對題數加1
                    }
                    else
                    {
                        // 記錄錯誤的答案到 RecoveryRecord 表中
                        var recoveryRecord = new RecoveryRecord
                        {
                            RecoveryId = recovery.RecoveryId, // 關聯的 RecoveryId
                            QuestionId = question.QuestionId, // 錯誤的問題 ID
                            UserAnswer = answer.UserAnswer
                        };
                        _context.RecoveryRecord.Add(recoveryRecord);
                    }
                }

                // 更新 Recovery 的答對題數
                recovery.Correct = correctCount;
                _context.SaveChanges(); // 保存所有變更
                var Account = account;
                return $"{Account}，已對答完成，正確題數{correctCount.ToString()}";
            }
            catch(Exception ex)
            {
                throw new Exception(ex.ToString());
            }
        }

        #endregion
        #region 使用者作答紀錄
        public List<Recovery> userRecovery(string account)
        {
            try
            {
                var recoveryRecords = _context.Recovery
                .Where(r => r.Account == account)
                .ToList(); 
                return recoveryRecords;

            }
            catch (Exception ex)
            {
                throw new Exception(ex.ToString());
            }
        }
        #endregion


        #region 作答紀錄單筆詳細
        public List<RecoveryDetailDto> getOneRecord(string user, int Rid)
        {
            try
            {
                var recovery = _context.Recovery.FirstOrDefault(r => r.RecoveryId == Rid && r.Account ==user);
                if(recovery == null)
                {
                    List<RecoveryDetailDto> Empty = new List<RecoveryDetailDto>();
                    return Empty;
                }
                // 首先獲取所有問題
                var allQuestions = _context.Question.ToList();
                // 然後獲取用戶的作答記錄
                var userAnswers = _context.RecoveryRecord
                    .Where(rr => rr.RecoveryId == Rid && rr.Recovery.Account == user)
                    .ToDictionary(rr => rr.QuestionId, rr => rr.UserAnswer);

                // 為每個問題創建 DTO，無論是否有作答記錄
                var recoveryDetails = allQuestions.Select(q => new RecoveryDetailDto
                {
                    Question = q.Question1 ?? "",
                    CorrectAnswer = q.Answer ?? "",
                    UserAnswer = userAnswers.ContainsKey(q.QuestionId) ? userAnswers[q.QuestionId] : q.Answer,
                    Options = new List<RecoveryDetailDto.OptionDetail>
                    {
                        new RecoveryDetailDto.OptionDetail
                        {
                            Text = q.Option1 ?? "",
                            IsCorrect = (q.Option1 ?? "") == (q.Answer ?? ""),
                            IsUserAnswer = userAnswers.ContainsKey(q.QuestionId)? q.Option1 == userAnswers[q.QuestionId] : q.Option1 == q.Answer
                        },
                        new RecoveryDetailDto.OptionDetail
                        {
                            Text = q.Option2 ?? "",
                            IsCorrect = (q.Option2 ?? "") == (q.Answer ?? ""),
                            IsUserAnswer = userAnswers.ContainsKey(q.QuestionId)? q.Option2 == userAnswers[q.QuestionId] : q.Option2 == q.Answer
                        },
                        new RecoveryDetailDto.OptionDetail
                        {
                            Text = q.Option3 ?? "",
                            IsCorrect = (q.Option3 ?? "") == (q.Answer ?? ""),
                            IsUserAnswer = userAnswers.ContainsKey(q.QuestionId)? q.Option3 == userAnswers[q.QuestionId] : q.Option3 == q.Answer
                        },
                        new RecoveryDetailDto.OptionDetail
                        {
                            Text = q.Answer ?? "",
                            IsCorrect = true,
                            IsUserAnswer = userAnswers.ContainsKey(q.QuestionId) 
                            ? q.Answer == userAnswers[q.QuestionId]  // 這裡改正了 Answer 的判斷
                            : true  // 如果沒有作答記錄，正確答案選項應該被標記為選中
                        }
                    }
                }).ToList();
                    return recoveryDetails;
            }
            catch(Exception ex)
            {
                throw new Exception(ex.ToString());
            }

        }
        #endregion

        #region 取得影片清單
        public List<VideoLinksDto> getVideoList()
        {
            try
            {
                List<VideoLinksDto> videos = _context.VideoLink.Select(v => new VideoLinksDto
                {
                    VideoId = v.VideoId,
                    VideoName = v.VideoName,
                    ImgInnerUrl = $"/api/Front/{v.VideoId}/GetVideoImg",
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

        #region 增加影片瀏覽次數
        public string addVideoClick(int videoId)
        {
            try
            {
                VideoLink video = _context.VideoLink.Where(v=>v.VideoId == videoId).FirstOrDefault();
                if(video == null)
                {
                    return"找不到該筆影片資訊";
                }
                video.LinkClick += 1;
                _context.SaveChanges();
                return "已增加一點閱數";

            }
            catch(Exception ex)
            {
                throw new Exception(ex.ToString());
            }

        }
        #endregion

        #region 取得書籍資訊
        public List<Book> getBookList()
        {
            try
            {
                List<Book> book = _context.Book.ToList();
                return book;

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
        #region 取得紅綠燈圖表
        public ChartDto getStateChart()
        {
            var data = new ChartDto();
            try
            {
               
                var query = from member in _context.Member
                where member.IsTeacher == 0
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
        public ChartDto getBullyingPostChart()
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
        #region 計算關鍵字總數
        public Dictionary<string, int> GetKeywordSum()
        {
            try
            {
                // 從資料庫取得所有bullyingerpost資料的keyword欄位
                var keywordList = _context.BullyingerPost.Select(b => b.KeyWord).ToList();

                // 建立一個字典來儲存每個字詞出現的次數
                Dictionary<string, int> keywordCount = new Dictionary<string, int>();

                foreach (var keywords in keywordList)
                {
                    // 將每筆 keyword 分割（假設用逗號和空格作為分隔符）
                    var words = keywords.Split(new char[] { ',', ' ' }, StringSplitOptions.RemoveEmptyEntries);

                    foreach (var word in words)
                    {
                        // 統計每個字詞出現的次數
                        if (keywordCount.ContainsKey(word))
                        {
                            keywordCount[word]++;
                        }
                        else
                        {
                            keywordCount[word] = 1;
                        }
                    }
                }

                // 回傳字典
                return keywordCount;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.ToString());
            }
        }



        #endregion
    }
}