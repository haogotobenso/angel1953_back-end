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
using OfficeOpenXml;

namespace angel1953_backend.Services
{
    public class BackService
    {
        private readonly BackRepository _backRepository;
        public BackService(BackRepository backRepository)
        {
            _backRepository = backRepository;
        }
        #region 顯示書籍
        public List<Book> GetBooks()
        {
            List<Book> booklist = _backRepository.getBooks();
            return booklist;
        }

        #endregion

        #region 上傳書本檔案
        public bool BookProcessExcel(Stream fileStream)
        {
            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
            using (var package = new ExcelPackage(fileStream))
            {
                var worksheet = package.Workbook.Worksheets[0]; // 選取第一個工作表
                var rowCount = worksheet.Dimension.Rows;
                List<Book> BookList = new List<Book>();
                for (int row = 2; row <= rowCount; row++) // 假設第一列是標題
                {
                    Book book = new Book
                    {
                        BookName = worksheet.Cells[row, 1].Value.ToString(),
                        Author = worksheet.Cells[row, 2].Value.ToString(),
                        PublicDate = DateTime.Parse(worksheet.Cells[row, 3].Value.ToString())
                    };
                    BookList.Add(book);
                    
                    
                }
                // 更新資料庫邏輯
                _backRepository.UpdateBook(BookList);
            }
            return true;
        }
        #endregion

        #region 下載書籍上傳定型檔
        public byte[] GetBookExcel()
        {
            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;

            using (var package = new ExcelPackage())
            {
                // 建立新的工作表
                var worksheet = package.Workbook.Worksheets.Add("Sheet1");

                // 填入資料
                worksheet.Cells[1, 1].Value = "書名";
                worksheet.Cells[1, 2].Value = "作者";
                worksheet.Cells[1, 3].Value = "出版日期";

                // 生成 Excel 並回傳 byte[]
                return package.GetAsByteArray();
            }
        }
        #endregion

        #region 刪除一筆書籍
        public bool DeleteOneBook(int BookId)
        {

            return _backRepository.deleteOneBook(BookId);
        }
        #endregion

        #region 新增一筆影片
        public string AddOneVideo(VideoLink video)
        {
            return _backRepository.addOneVideo(video);
        }
        #endregion

        #region 取得影片
        public List<VideoLinksDto> GetAllVideo()
        {
            return _backRepository.getAllVideos();
        }
        #endregion

        #region 取得影片縮圖

        public byte[] GetVideoImg(int id)
        {
            return _backRepository.getVideoImg(id);
        }
        #endregion

        #region 刪除一筆影片
        public bool DeleteOneVideo(int VideoId)
        {
            return _backRepository.deleteOneVideo(VideoId);
        }
        #endregion

        #region 取得題目資訊
        public List<Question> GetQuestionList()
        {
            return _backRepository.getQuestionList();
        }

        #endregion

        #region 新增FB爬蟲連結
        public string AddFBLink(CrawlerLink FBLink)
        {
            return _backRepository.addFBLink(FBLink);
        }
        #endregion

        #region 取得FB爬蟲連結表
        public List<CrawlerLink> GetFBLinkList()
        {
            return _backRepository.getFBLinkList();
        }
        #endregion 
        #region 刪除一筆FB爬蟲連結
        public bool DeleteFBLink(int LinkId)
        {
            return _backRepository.deleteFBLink(LinkId);
        }
        #endregion

        #region 取得素養網路爬蟲資料
        public List<ExternalLinks> GetExtLink()
        {
            return _backRepository.getExtLink();
        }
        #endregion

        #region 取得所有事件檢視資訊
        public List<ShowCaseDto> ShowCase()
        {
            return _backRepository.showCase();
        }
        #endregion

        #region 取得一筆資訊顯示詳細
        public ShowCaseDetailDto ShowCaseDetail(int BPId)
        {
            return _backRepository.showCaseDetail(BPId);
        }
        #endregion
        
        #region 取得使用者資訊清單
        public List<UserInfoDto> UserInfo(string TorCAccount)
        {
            return _backRepository.userInfo(TorCAccount);
        }
        #endregion

        #region 取得一筆詳細使用者資訊
        public UserInfoDetailDto GetOneUserInfo(string user,string Account)
        {
            return _backRepository.getOneUserInfo(user,Account);
        }
        #endregion
        
    }
}