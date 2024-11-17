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
using Microsoft.VisualBasic;

namespace angel1953_backend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles = "Teacher,Counselor")]
    public class BackController : ControllerBase
    {
        private readonly BackService _backService;
        private readonly PythonService _pythonService;
        public BackController(BackService backService,PythonService pythonService)
        {
            _backService = backService;
            _pythonService = pythonService;
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
        public IActionResult UploadBooks([FromForm]Book bookupdate)
        {
            string msg = "";
            object msginfo;
            if(bookupdate !=null)
            {
                msg = _backService.UploadBooks(bookupdate);
                msginfo = new { Status = 200, Message = msg };
            }
            else
            {
                msg = "請輸入遇上傳書籍";
                msginfo = new { Status = 400, Message = msg };
            }
            var failjsonmsg = JsonConvert.SerializeObject(msginfo);
            return Content(failjsonmsg, "application/json");

        }
        #endregion

        #region 取得書籍上傳定型檔
        [HttpGet("GetBookExcel")]
        public IActionResult GetBookExcel()
        {
            var bookexcel = _backService.GetBookExcel();
            return File(bookexcel,"application/vnd.openxmlformats-officedocument.spreadsheetml.sheet","BookExcelEx.xlsx");
            
        }

        #endregion

        #region 刪除一筆書籍
        [HttpDelete("DeleteOneBook")]
        public IActionResult DeleteOneBook([FromQuery] int Bookid)
        {
            bool result = _backService.DeleteOneBook(Bookid);
            object msg ;
            if(result)
            {
                msg = new { Status = 200, Message = "刪除成功" };
            }
            else
            {
                msg = new { Status = 400, Message = "刪除失敗" };
            }
            var jsonmsg = JsonConvert.SerializeObject(msg);
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
            bool result = _backService.DeleteOneVideo(videoid);
            object msg ;
            if(result)
            {
                msg = new { Status = 200, Message = "刪除成功" };
            }
            else
            {
                msg = new { Status = 400, Message = "刪除失敗" };
            }
            var jsonmsg = JsonConvert.SerializeObject(msg);
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

        #region FB爬蟲連結新增
        [HttpPost("AddFBLink")]
        public IActionResult AddFBLink([FromBody] CrawlerLink FBLink)
        {
            var user = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            FBLink.Account = user;
            string result = _backService.AddFBLink(FBLink);
            var msg = new { Status = 200, Message = result };
            var jsonmsg = JsonConvert.SerializeObject(msg);
            return Content(jsonmsg, "application/json");

        }
        #endregion

        #region FB爬蟲連結清單顯示
        [HttpGet("GetFBLinkList")]
        public IActionResult GetFBLinkList()
        {
            var result = _backService.GetFBLinkList();
            var msg = new { Status = 200, Message = result };
            var jsonmsg = JsonConvert.SerializeObject(msg);
            return Content(jsonmsg, "application/json");
        }

        #endregion

        #region FB爬蟲連結刪除
        [HttpDelete("DeleteFBLink")]
        public IActionResult DeleteFBLink([FromQuery] int LinkId)
        {
            bool result = _backService.DeleteFBLink(LinkId);
            object msg ;
            if(result)
            {
                msg = new { Status = 200, Message = "刪除成功" };
            }
            else
            {
                msg = new { Status = 400, Message = "刪除失敗" };
            }
            var jsonmsg = JsonConvert.SerializeObject(msg);
            return Content(jsonmsg, "application/json");
        }

        #endregion

        #region 顯示素養網路爬蟲資訊
        [HttpGet("GetExtLink")]
        public IActionResult GetExtLink()
        {
            var result = _backService.GetExtLink();
            var msg = new { Status = 200, Message = result };
            var jsonmsg = JsonConvert.SerializeObject(msg);
            return Content(jsonmsg, "application/json");
        }
        #endregion

        #region 更新素養網路爬蟲資訊
        [HttpGet("UpdateExtLink")]
        public IActionResult UpdateExtLink()
        {
            var result = _pythonService.UpdateExtLinkAPI();
            var msg = new { Status = 200, Message = result };
            var jsonmsg = JsonConvert.SerializeObject(msg);
            return Content(jsonmsg, "application/json");
        }
        #endregion

        #region 事件檢視
        [HttpGet("ShowCase")]
        public IActionResult ShowCase()
        {
            var AllCase = _backService.ShowCase();
            var msg = new { Status = 200, Message = AllCase };
            var jsonmsg = JsonConvert.SerializeObject(msg);
            return Content(jsonmsg, "application/json");
        } 
        #endregion

        #region 事件檢視詳細
        [HttpGet("ShowCaseDetail")]
        public IActionResult ShowCaseDetail(int BPId)
        {
            var OneCase = _backService.ShowCaseDetail(BPId);
            var msg = new { Status = 200, Message = OneCase };
            var jsonmsg = JsonConvert.SerializeObject(msg);
            return Content(jsonmsg, "application/json");

        }

        #endregion

        #region 使用者資訊
        [HttpGet("UserInfo")]
        public IActionResult UserInfo()
        {
            var user = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            var AllUser = _backService.UserInfo(user);
            var msg = new { Status = 200, Message = AllUser };
            var jsonmsg = JsonConvert.SerializeObject(msg);
            return Content(jsonmsg, "application/json");

        }

        #endregion

        #region 使用者資訊詳細
        [HttpGet("UserInfoDetail")]
        public IActionResult UserInfoDetail([FromQuery]string Account)
        {
            var user = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            var OneUser = _backService.GetOneUserInfo(user,Account);
            var msg = new { Status = 200, Message = OneUser };
            var jsonmsg = JsonConvert.SerializeObject(msg);
            return Content(jsonmsg, "application/json");
        }

        #endregion

        #region 取得紅綠燈圖表
        [HttpGet("GetStateChart")]
        public IActionResult GetStateChart()
        {
            var user = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            var result = _backService.GetStateChart(user);
            if(result != null)
            {
                var response = new { Status = 200, Message = result };
                var jsonresponse = JsonConvert.SerializeObject(response);
                return Content(jsonresponse, "application/json");
            }
            else
            {
                var response = new { Status = 400, Messaeg = result };
                var jsonresponse = JsonConvert.SerializeObject(response);
                return Content(jsonresponse, "application/json");
            }

        }
        #endregion
        #region 取得本月霸凌貼文總數折線圖
        [HttpGet("GetBullyingPostChart")]
        public IActionResult GetBullyingPostChart()
        {
            var user = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            var result = _backService.GetBullyingPostChart(user);
            if(result != null)
            {
                var response = new { Status = 200, Message = result };
                var jsonresponse = JsonConvert.SerializeObject(response);
                return Content(jsonresponse, "application/json");
            }
            else
            {
                var response = new { Status = 400, Messaeg = result };
                var jsonresponse = JsonConvert.SerializeObject(response);
                return Content(jsonresponse, "application/json");
            }

        }
        #endregion

        #region 學生Todo老師狀態變更
        [HttpGet("UserTodoChage")]
        public IActionResult UserTodoChage([FromQuery]string account)
        {
            var user = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            object msg;
            if(_backService.UserTodoChage(user,account))
            {
                msg = new { Status = 200, Message = "狀態變更成功" };
            }
            else
            {
                msg = new { Status = 400, Message = "狀態變更時發生錯誤" };
            }
            var jsonmsg = JsonConvert.SerializeObject(msg);
            return Content(jsonmsg, "application/json");
        }
        #endregion
        #region 查看學生通報資訊
        [HttpGet("ShowScase")]
        public IActionResult ShowScase()
        {
            var user = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            var Case  =_backService.ShowScase(user);
            var response = new { Status = 200, Message = Case };
            var jsonresponse = JsonConvert.SerializeObject(response);
            return Content(jsonresponse, "application/json");

        }
        #endregion
        #region 查看單筆學生通報詳細
        [HttpGet("ShowOneScase")]
        public IActionResult ShowOneScase([FromQuery]int scaseId)
        {
            var user = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            var Case  =_backService.ShowOneScase(scaseId,user);
            var response = new { Status = 200, Message = Case };
            var jsonresponse = JsonConvert.SerializeObject(response);
            return Content(jsonresponse, "application/json");
        }
        #endregion
        #region 查看通報圖片
        [HttpGet("{ScaseId}/GetScaseImg")]
        public IActionResult GetScaseImg(int ScaseId)
        {
            var thumbnail = _backService.GetScaseImg(ScaseId);
            if (thumbnail == null) return NotFound();
            return File(thumbnail, "image/jpeg");
        }
        #endregion
        #region 處理狀態變更
        [HttpPost("ChageOneScase")]
        public IActionResult ChangOneScase(int ScaseId)
        {
            string msg = "";
            object response;
            var user = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if(_backService.ChageOneScase(ScaseId,user))
            {
                 response = new { Status = 200, Message = "狀態變更成功" };
            }
            else
            {
                response = new { Status = 400, Message = "狀態變更時出現異常" };
            }
            var jsonresponse = JsonConvert.SerializeObject(response);
            return Content(jsonresponse, "application/json");
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