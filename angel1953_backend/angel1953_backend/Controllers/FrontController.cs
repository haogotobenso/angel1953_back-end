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
    [Authorize]
    public class FrontController : ControllerBase
    {
        private readonly FrontService _frontservice ;

        public FrontController (FrontService frontService)
        {
            _frontservice = frontService;
        }

        #region 取得題目
        [HttpGet("GetQuestion")]
        public IActionResult GetQuestion()
        {
            var QuestionList = _frontservice.GetQuestion();
            var msg = new { Status = 200, Message = QuestionList };
            var jsonmsg = JsonConvert.SerializeObject(msg);
            return Content(jsonmsg, "application/json");
        } 
        #endregion

        #region 使用者回答題目
        [HttpPost("AnsQuestion")]
        public IActionResult AnsQuestion([FromBody]List<AnswerDto> answer)
        {
            var user = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            var result = _frontservice.AnsQuestion(answer,user);

            var msg = new { Status = 200, Message = result};
            var jsonmsg = JsonConvert.SerializeObject(msg);
            return Content(jsonmsg, "application/json");

        }

        #endregion

        #region 使用者作答紀錄
        [HttpGet("AnsRecord")]
        public IActionResult AnsRecord()
        {
            var user = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            var result = _frontservice.UserRecovery(user);
            var msg = new { Status = 200, Message = result};
            var jsonmsg = JsonConvert.SerializeObject(msg);
            return Content(jsonmsg, "application/json");
        }
        #endregion

        #region 作答紀錄詳細
        [HttpPost("GetOneRecord")]
        public IActionResult GetOneRecord([FromBody] int recoveryId)
        {
            var user = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            var result = _frontservice.GetOneRecord(user,recoveryId);
            var msg = new { Status = 200, Message = result};
            var jsonmsg = JsonConvert.SerializeObject(msg);
            return Content(jsonmsg, "application/json");
        }
        #endregion

        #region 取得影片清單
        [HttpGet("GetVideoList")]
        public IActionResult GetVideoList()
        {
            var result = _frontservice.GetVideoList();
            var msg = new { Status = 200, Message = result};
            var jsonmsg = JsonConvert.SerializeObject(msg);
            return Content(jsonmsg, "application/json");
        }

        #endregion

        #region 取得影片圖片
        [HttpGet("{VideoId}/GetVideoImg")]
        public IActionResult GetVideoImg(int VideoId)
        {
            var thumbnail = _frontservice.GetVideoImg(VideoId);
            if (thumbnail == null) return NotFound();
            return File(thumbnail, "image/jpeg");
        }
        #endregion

        #region 增加影片瀏覽次數
        [HttpGet("AddVideoClick")]
        public IActionResult AddVideoClick(int VideoId)
        {
            string result = _frontservice.AddVideoClick(VideoId);
            var msg = new { Status = 200, Message = result};
            var jsonmsg = JsonConvert.SerializeObject(msg);
            return Content(jsonmsg, "application/json");
        }
        #endregion

        #region 取得書籍資訊
        [HttpGet("GetBookList")]
        public IActionResult GetBookList()
        {
            var result = _frontservice.GetBookList();
            var msg = new { Status = 200, Message = result};
            var jsonmsg = JsonConvert.SerializeObject(msg);
            return Content(jsonmsg, "application/json");
        }
        #endregion

        #region 取得素養網路爬蟲資訊
        [HttpGet("GetExtLink")]
        public IActionResult GetExtLink()
        {
            var result = _frontservice.GetExtLink();
            var msg = new { Status = 200, Message = result };
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