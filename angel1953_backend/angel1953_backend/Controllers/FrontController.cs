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
            var user = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            string result = _frontservice.AddVideoClick(VideoId,user);
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


        #region 取得紅綠燈圖表
        [AllowAnonymous]
        [HttpGet("GetStateChart")]
        public IActionResult GetStateChart()
        {
            
            var result = _frontservice.GetStateChart();
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
        [AllowAnonymous]
        [HttpGet("GetBullyingPostChart")]
        public IActionResult GetBullyingPostChart()
        {
            
            var result = _frontservice.GetBullyingPostChart();
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

        #region 計算關鍵字總數
        [AllowAnonymous]
        [HttpGet("GetKeywordSum")]
        public IActionResult GetKeywordSum()
        {
            var keywordSum = _frontservice.GetKeywordSum();

            var response = new { Status = 200, Message = keywordSum };
            var jsonresponse = JsonConvert.SerializeObject(response);

            return Content(jsonresponse, "application/json");
        }

        #endregion

        #region 新增霸凌通報
        [HttpPost("AddScase")]
        public IActionResult AddScase([FromForm]Scase Case,[FromForm] IFormFile? CaseImg)
        {
            var user = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            var account = user;
            if(CaseImg !=null)
            {
                using (var memoryStream = new MemoryStream())
                {
                    CaseImg.CopyTo(memoryStream);
                    Case.SCimg = memoryStream.ToArray(); // 將圖片儲存為 byte[]
                }

            }
            var msg = _frontservice.AddScase(account,Case);
            var response = new { Status = 200, Message = msg  };
            var jsonresponse = JsonConvert.SerializeObject(response);
            return Content(jsonresponse, "application/json");

        }
        #endregion
        #region 查看歷史通報紀錄
        [HttpGet("ShowScase")]
        public IActionResult ShowScase()
        {
            var user = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            var Case  =_frontservice.ShowScase(user);
            var response = new { Status = 200, Message = Case };
            var jsonresponse = JsonConvert.SerializeObject(response);
            return Content(jsonresponse, "application/json");

        }
        #endregion
        #region 查看一筆通報詳細記錄
        [HttpGet("ShowOneScase")]
        public IActionResult ShowOneScase([FromQuery]int scaseId)
        {
            var user = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            var Case  =_frontservice.ShowOneScase(scaseId,user);
            var response = new { Status = 200, Message = Case };
            var jsonresponse = JsonConvert.SerializeObject(response);
            return Content(jsonresponse, "application/json");
        }
        #endregion
        #region 取得霸凌通報圖片
        [HttpGet("{ScaseId}/GetScaseImg")]
        public IActionResult GetScaseImg(int ScaseId)
        {
            var thumbnail = _frontservice.GetScaseImg(ScaseId);
            if (thumbnail == null) return NotFound();
            return File(thumbnail, "image/jpeg");
        }
        #endregion        

        
    }
}