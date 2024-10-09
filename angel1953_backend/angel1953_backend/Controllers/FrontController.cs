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
    //[Authorize]
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
        [HttpPost("AnsQnestion")]
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
            var result = _frontservice.GetOneRecord("Teacher1",recoveryId);
            var msg = new { Status = 200, Message = result};
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