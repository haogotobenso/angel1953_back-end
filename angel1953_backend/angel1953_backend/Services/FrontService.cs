using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using angel1953_backend.Dtos;
using angel1953_backend.Models;
using angel1953_backend.Repository;
using System.Security.Cryptography;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.AspNetCore.Mvc.RazorPages.Infrastructure;
using OfficeOpenXml.Utils.TypeConversion;

namespace angel1953_backend.Services
{
    public class FrontService
    {
        private readonly FrontRepository _frontrepository;

        public FrontService(FrontRepository frontRepository)
        {
            _frontrepository = frontRepository;
        }
        
        #region 取得題目
        public List<QuestionDto> GetQuestion()
        {
            var QuestionList = _frontrepository.getQuestion();
            return QuestionList ;

        }
        #endregion

        #region 對答案

        public string AnsQuestion(List<AnswerDto> ans,string account)
        {
            var result = _frontrepository.ansQuestion(ans,account);
            return result;
        }

        #endregion

        #region 顯示使用者作答紀錄
        public List<Recovery> UserRecovery(string account)
        {
            List<Recovery> Record = _frontrepository.userRecovery(account);
            return Record;
        }
        #endregion

        #region 作答紀錄單筆詳細

        public List<RecoveryDetailDto> GetOneRecord(string user, int Rid)
        {  
            var OneRecord = _frontrepository.getOneRecord(user,Rid);
            return OneRecord;
        }

        #endregion

        #region 取得影片清單
        public List<VideoLinksDto> GetVideoList()
        {
            List<VideoLinksDto> VideoList = _frontrepository.getVideoList();
            return VideoList;
        }


        #endregion

        #region 取得影片縮圖

        public byte[] GetVideoImg(int id)
        {
            return _frontrepository.getVideoImg(id);
        }
        #endregion

        #region 增加影片瀏覽次數
        public string AddVideoClick(int Id)
        {
            return _frontrepository.addVideoClick(Id);
        }
        #endregion

        #region 取得書籍資訊
        public List<Book> GetBookList()
        {
            return _frontrepository.getBookList();
        }

        #endregion

    }
}