using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using angel1953_backend.Dtos;
using angel1953_backend.Services;
using Newtonsoft.Json;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using System.IdentityModel.Tokens.Jwt;
using angel1953_backend.Models;
using Microsoft.Identity.Client;

namespace angel1953_backend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles = "Teacher,Admin")]
    public class BackController : ControllerBase
    {
        private readonly BackService _backService;
        public BackController(BackService backService)
        {
            _backService = backService;
        }

        #region 顯示所有書籍
        [HttpGet("GetBooks")]
        public IActionResult GetBooks()
        {
            var book = _backService.GetBooks();
            var msginfo = new { Status = 200, Message = book };
            var jsonmsg = JsonConvert.SerializeObject(msginfo);
            return Content(jsonmsg, "application/json");
        }
        #endregion
        #region 新增書籍
        [HttpPost("UploadBooks")]
        public IActionResult UploadBooks(IFormFile bookexcel)
        {
            string msg = "";
            if (bookexcel == null || bookexcel.Length == 0)
            {
                msg = "請上傳有效的檔案";
            }

            var fileExtension = Path.GetExtension(bookexcel.FileName);
            if (fileExtension != ".xls" && fileExtension != ".xlsx")
            {
                msg = "只支援 Excel 格式的檔案";
            }

            // 保存檔案到伺服器上，或者讀取檔案內容
            using (var stream = new MemoryStream())
            {
                bookexcel.CopyTo(stream);
                // 解析 CSV 或 Excel
                var result = _backService.BookProcessExcel(stream);
                
                if (result)
                {
                    msg = "資料已成功更新";
                    var msginfo = new { Status = 200, Message = msg };
                    var jsonmsg = JsonConvert.SerializeObject(msginfo);
                    return Content(jsonmsg, "application/json");
                }
                else
                {
                    msg = "資料更新失敗";
                }
            }
            var failmsginfo = new { Status = 400, Message = msg };
            var failjsonmsg = JsonConvert.SerializeObject(failmsginfo);
            return Content(failjsonmsg, "application/json");

        }
        #endregion

        #region 刪除一筆書籍
        [HttpDelete("DeleteOneBook")]
        public IActionResult DeleteOneBook([FromQuery] int Bookid)
        {
            var msg = _backService.DeleteOneBook(Bookid);
            var msginfo = new { Status = 200, Message = msg };
            var jsonmsg = JsonConvert.SerializeObject(msginfo);
            return Content(jsonmsg, "application/json");
        }

        #endregion

        #region 新增一筆影片
        [HttpPost("AddOneVideo")]
        public IActionResult AddOneVideo([FromForm] VideoLink Video, [FromForm] IFormFile? VideoPhoto)
        {
            using (var memoryStream = new MemoryStream())
            {
                VideoPhoto.CopyTo(memoryStream);
                Video.VideoImg = memoryStream.ToArray(); // 將圖片儲存為 byte[]
            }
            var result = _backService.AddOneVideo(Video);
            var msginfo = new { Status = 200, Message = result };
            var jsonmsg = JsonConvert.SerializeObject(msginfo);
            return Content(jsonmsg, "application/json");

        }
        #endregion
        #region 讀取所有影片資訊
        [HttpGet("GetAllVideo")]
        public IActionResult GetAllVideo()
        {
            List<VideoLinksDto> result = _backService.GetAllVideo();
            var msginfo = new { Status = 200, Message = result };
            var jsonmsg = JsonConvert.SerializeObject(msginfo);
            return Content(jsonmsg, "application/json");
        }
        #endregion

        #region 取得影片圖片
        [HttpGet("{VideoId}/GetVideoImg")]
        public IActionResult GetVideoImg(int VideoId)
        {
            var thumbnail = _backService.GetVideoImg(VideoId);
            if (thumbnail == null) return NotFound();
            return File(thumbnail, "image/jpeg");
        }
        #endregion

        #region 刪除一筆影片資訊
        [HttpDelete("DeleteOneVideo")]
        public IActionResult DeleteOneVideo([FromQuery] int videoid)
        {
            string result = _backService.DeleteOneVideo(videoid);
            var msginfo = new { Status = 200, Message = result };
            var jsonmsg = JsonConvert.SerializeObject(msginfo);
            return Content(jsonmsg, "application/json");
        }

        #endregion

        #region 取得題目
        [HttpGet("GetQuestionList")]
        public IActionResult GetQuestion()
        {
            var QuestionList = _backService.GetQuestionList();
            var msg = new { Status = 200, Message = QuestionList };
            var jsonmsg = JsonConvert.SerializeObject(msg);
            return Content(jsonmsg, "application/json");
        } 
        #endregion

        #region 取得帳號測試
        [HttpGet("trytrysee")]
        public IActionResult TryTrySee() 
        {
            var userName = User.Identity?.Name;
            var role = User.FindFirst(ClaimTypes.Role)?.Value;
            var user = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (userName != null && role != null)
            {
                return Ok(new { status = 200, message = $"User: {userName}, Role: {role}, account: {user}" });
            }
            else
            {
                return Ok(new { status = 200, message = "Unknown User, Role: No Role" });
            }
            
        }
        #endregion
    }
}