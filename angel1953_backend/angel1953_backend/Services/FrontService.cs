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
        public string AddVideoClick(int Id,string account)
        {
            return _frontrepository.addVideoClick(Id,account);
        }
        #endregion

        #region 取得書籍資訊
        public List<Book> GetBookList()
        {
            return _frontrepository.getBookList();
        }

        #endregion
        #region 取得素養網路爬蟲資料
        public List<ExternalLinks> GetExtLink()
        {
            return _frontrepository.getExtLink();
        }
        #endregion
        #region 取得紅綠燈圖表
        public ChartDto GetStateChart()
        {
            return _frontrepository.getStateChart();
        }
        #endregion
        #region 取得本月霸凌貼文折線圖
        public ChartDto GetBullyingPostChart()
        {
            return _frontrepository.getBullyingPostChart();
        }

        #endregion
        #region 計算關鍵字總數
        public Dictionary<string, int> GetKeywordSum()
        {
            return _frontrepository.GetKeywordSum();
        }
        #endregion
        #region 新增霸凌通報
        public string AddScase(string account,Scase Case)
        {
            return _frontrepository.addScase(account,Case);

        }
        #endregion
        #region 顯示霸凌通報紀錄
        public List<Scase> ShowScase(string account)
        {
            return _frontrepository.showScase(account);
        }
        #endregion
        #region 顯示單筆霸凌通報詳細記錄
        public Scase ShowOneScase(int id,string account)
        {
            return _frontrepository.showOneScase(id,account);

        }
        #endregion
        #region 取得通報紀錄縮圖

        public byte[] GetScaseImg(int id)
        {
            return _frontrepository.getScaseImg(id);
        }
        #endregion
    }
}