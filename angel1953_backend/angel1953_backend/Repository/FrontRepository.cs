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
    }
}