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
    public class MemberController : ControllerBase
    {
        private readonly MemberServices _memberService;
        private readonly MailService _mailService;
        private readonly IConfiguration _configuration;

        public MemberController (MemberServices memberService, MailService mailService, IConfiguration configuration)
        {
            _memberService = memberService;
            _mailService = mailService;
            _configuration = configuration;
        }

        #region 國中名單
        [HttpGet("GetMidSchoolList")]
        public IActionResult GetMidSchoolList()
        {
            List<School> midSchool = _memberService.GetSchoolList(false);
            var msg = new { Status = 200, Message = midSchool };
            var jsonmsg = JsonConvert.SerializeObject(msg);
            return Content(jsonmsg, "application/json");

        }
        #endregion

        #region  高中名單
        [HttpGet("GetSchoolList")]
        public IActionResult GetSchoolList()
        {
            List<School> School = _memberService.GetSchoolList(true);
            var msg = new { Status = 200, Message = School };
            var jsonmsg = JsonConvert.SerializeObject(msg);
            return Content(jsonmsg, "application/json");

        }

        #endregion

        #region 註冊
        [HttpPost("Register")]
        public IActionResult Register([FromForm] RegisterDto member, [FromForm] IFormFile? teacherPhoto)
        {
            if((member.IsTeacher == 1 || member.IsTeacher == 2) && teacherPhoto !=null)
            {
                using (var memoryStream = new MemoryStream())
                {
                    teacherPhoto.CopyTo(memoryStream);
                    member.TeacherImg = memoryStream.ToArray(); // 將圖片儲存為 byte[]
                }
            }
            if(member.IsTeacher == 0 && member.FBurl == null)
            {
                var msg = new { Status = 400, Message = "學生須輸入Facebook連結" };
                var jsonmsg = JsonConvert.SerializeObject(msg);
                   return Content(jsonmsg, "application/json");
            }
            
            if (member.Password == member.PasswordCheck)
            {
                if (_memberService.CheckAccount(member.Account))
                {
                    member.AuthCode=_memberService.Register(member);
                    string TempMail = System.IO.File.ReadAllText("../angel1953_backend/MailBody/MailBody.html");
                    string ValidateUrl = $"{Request.Scheme}://{Request.Host}/api/Member/EmailValidate?Account={member.Account}&AuthCode={member.AuthCode}";
                    string mailBody = _mailService.GetMailBody(TempMail, member.Account, ValidateUrl);
                    _mailService.SendMail(mailBody, member.Email);
                    var msg = new { Status = 200, Message = "帳號註冊成功，請去收信" };
                    var jsonmsg = JsonConvert.SerializeObject(msg);
                    return Content(jsonmsg, "application/json");
                }
                else
                {
                    var msg = new { Status = 400, Message = "帳號已註冊" };
                    var jsonmsg = JsonConvert.SerializeObject(msg);
                    return Content(jsonmsg, "application/json");
                }
            }
            else 
            {
                var success = new { Status = 400, Message = "兩次密碼輸入不一致" };
                var jsonsuccess = JsonConvert.SerializeObject(success);
                return Content(jsonsuccess, "application/json");
                
            }
            if{
                       }
        #endregion

        #region Email認證
        [HttpGet("EmailValidate")]
        public IActionResult EmailValidate([FromQuery] string Account, string AuthCode)
        {
            if ( _memberService.EmailValidate(Account, AuthCode))
            {
                var success = new { Status = 200, Message = "驗證成功" };
                var jsonsuccess = JsonConvert.SerializeObject(success);
                return Content(jsonsuccess, "application/json");
            }
            else
            {
                var fail = new { Status = 400, Message = "驗證失敗" };
                var jsonfail = JsonConvert.SerializeObject(fail);
                return Content(jsonfail, "application/json");
            }
        }
        #endregion

        #region 登入
        [HttpPost("Login")]
        public IActionResult Login([FromBody]LoginDto login)
        {
            var result = _memberService.Login(login.Account,login.Password);
            if(result == "")
            {
                string token = _memberService.GenerateToken(login.Account);
                var success = new { Status = 200, Message = "驗證成功" , Token = token};
                var jsonsuccess = JsonConvert.SerializeObject(success);
                return Content(jsonsuccess, "application/json");
            }
            else
            {
                var fail = new { Status = 400, Message = result };
                var jsonfail = JsonConvert.SerializeObject(fail);
                return Content(jsonfail, "application/json");
            }

        }
        #endregion

        #region 修改密碼
        [Authorize]
        [HttpPost("ChangePassword")]
        public IActionResult ChangePassword([FromBody]ChangePasswordDto changePassword)
        {
            var user = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            string failMsg;
            if(user != null)
            {
                var result = _memberService.ChangePassword(user,changePassword);
                if(result == "")
                {
                    var success = new { Status = 200, Message = "修改成功"};
                    var jsonsuccess = JsonConvert.SerializeObject(success);
                    return Content(jsonsuccess, "application/json");
                }
                else
                {
                    failMsg = result;
                }
            }
            else
            {
                failMsg = "帳號狀態錯誤";
            }
            var fail = new { Status = 400, Message = failMsg, User = user};
            var jsonfail = JsonConvert.SerializeObject(fail);
            return Content(jsonfail, "application/json");
        }
        #endregion

        #region 取得使用者帳號資訊
        [HttpGet("GetAccountInfo")]
        public IActionResult GetAccountInfo()
        {
            var user = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            var UserInfo = _memberService.GetAccountInfo(user);
            object msg;
            if(UserInfo!=null)
            {
                msg = new { Status = 200, Message = UserInfo};
            }
            else
            {
                msg = new { Status = 400, Message = "尋找用戶資料時發生錯誤"};
            }
            var jsonsuccess = JsonConvert.SerializeObject(msg);
            return Content(jsonsuccess, "application/json");
        }
        #endregion

        
        #region 取得帳號測試
        [Authorize]
        [HttpGet("trytrysee")]
        public IActionResult TryTrySee() 
        {
            var userName = User.Identity?.Name;
            var role = User.FindFirst(ClaimTypes.Role)?.Value;
            
            if (userName != null && role != null)
            {
                return Ok(new { status = 200, message = $"User: {userName}, Role: {role}" });
            }
            else
            {
                return Ok(new { status = 200, message = "Unknown User, Role: No Role" });
            }
            
        }
        #endregion
    }
}
