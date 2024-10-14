using angel1953_backend.Dtos;
using angel1953_backend.Models;
using angel1953_backend.Repository;
using System.Security.Cryptography;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.AspNetCore.Mvc.RazorPages.Infrastructure;

namespace angel1953_backend.Services
{
    public class MemberServices
    {
        private readonly MemberRepository _memberRepository;
        private readonly MailService _mailService;
        private readonly IConfiguration _configuration;

        public MemberServices(MemberRepository memberRepository, MailService mailService, IConfiguration configuration)
        {
            _memberRepository = memberRepository;
            _mailService = mailService;
            _configuration = configuration;
        }

        #region 註冊
        public string Register(RegisterDto register)
        {
            Member member = new Member();
            member.Account = register.Account;
            member.Password = register.Password;
            member.Name = register.Name;
            member.Email = register.Email;
            member.SchoolId = register.SchoolId;
            // if(register.MidSchoolId == null) //高中
            // {
            //     member.SchoolId = register.SchoolId;
            //     member.MidSchoolId = null;
            // }
            // else if(register.SchoolId == null) //國中
            // {
            //     member.SchoolId = null;
            //     member.MidSchoolId = register.MidSchoolId;
            // }
            //member.SchoolId = _memberRepository.getSchoolId(register.School);
            member.IsTeacher = register.IsTeacher;
            member.FBurl = register.FBurl;
            if (register.IsTeacher==1) //老師
            {
                member.TeacherImg = register.TeacherImg;
                member.ClassId = _memberRepository.getClassId(register.Class);
                member.StudentId = null;
            }
            else if(register.IsTeacher==2) //學校輔導人員
            {
                member.TeacherImg = register.TeacherImg;
                member.ClassId = null;
                member.StudentId = null;
            }
            else //學生
            {
                member.TeacherImg = null;
                member.ClassId = _memberRepository.getClassId(register.Class);
                member.StudentId = register.StudentId;
            
            }
            member.Password = HashPassword(member.Password);
            member.AuthCode = _mailService.AuthCode();
            _memberRepository.register(member);
            return member.AuthCode;
        }
        #endregion

        #region 帳號檢查
        public bool CheckAccount(string account)
        {
            if (_memberRepository.checkAccount(account))
            {
                return true;
            }
            return false;
        }
        #endregion

        #region Hash密碼
        public string HashPassword(string password)
        {
            string saltkey = "iwanttogotobensohahapy";
            string saltAndPassword = String.Concat(password, saltkey);
            SHA256 sha256 = SHA256.Create();
            var Hashed = sha256.ComputeHash(Encoding.UTF8.GetBytes(saltAndPassword));
            string HashPassword = Convert.ToBase64String(Hashed);
            return HashPassword;
        }
        #endregion

        #region Email認證
        public bool EmailValidate(string Account, string AuthCode)
        {
            return _memberRepository.emailValidate(Account, AuthCode);
        }
        #endregion

        #region 取得一筆資料
        public Member GetMemberByAccount(string account)
        {
            Member oneMember = _memberRepository.getMemberByAccount(account);
            return oneMember;
        }
        #endregion

        #region 登入
        public string Login(string account, string password)
        {
            Member LoginMember = GetMemberByAccount(account);
            if (LoginMember != null)
            {
                if (LoginMember.AuthCode == null)
                {
                    if (checkPassword(password, LoginMember.Password))
                    {
                        return "";
                    }
                    else
                    {
                        return "帳號或密碼輸入錯誤";
                    }
                }
                else
                {
                    return "目前帳號尚未通過驗證";
                }
            }
            else
            {
                return "目前無此帳號，請去註冊";
            }

        }

        #endregion

        #region 確認密碼
        public bool checkPassword(string Password, string DBPassword)
        {
            if (HashPassword(Password) == DBPassword)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        #endregion

        #region 變更密碼
        public string ChangePassword(string account,ChangePasswordDto changePassword)
        {
            Member member = GetMemberByAccount(account);
            if(checkPassword(changePassword.OldPassword,member.Password))
            {
                if(changePassword.NewPassword==changePassword.NewPasswordCheck)
                { 
                    if(_memberRepository.changePassword(account,HashPassword(changePassword.NewPassword)))
                    {
                        return "";
                    }
                    else
                    {
                        return "密碼變更時發生錯誤";
                    }
                }
                else
                {
                    return "新密碼驗證錯誤";
                }
                
            }
            else
            {
                return "密碼錯誤";
            }

        }

        #endregion

        #region 取得角色
        public string GetRole(string account)
        {
            Member member = GetMemberByAccount(account);
            if (member.IsTeacher == 1)
            {
                return "Teacher";
            } 
            else if (member.IsTeacher == 2)
            {
                return "Counselor";
            }
            return "Student";
        }
        #endregion

        #region JWT生成
        public string GenerateToken(string Account)
        {
            Member member = GetMemberByAccount(Account);
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JWT:KEY"]));
            var jwt = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new Claim[] {
                    new Claim(JwtRegisteredClaimNames.NameId, member.Account),
                    new Claim(ClaimTypes.Name, member.Name),
                    new Claim(ClaimTypes.Role,GetRole(member.Account)),
                }),
                Issuer = _configuration["JWT:Issuer"],
                Audience = _configuration["JWT:Audience"],
                Expires = DateTime.UtcNow.AddHours(1),
                SigningCredentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256)
            };
            var handler = new JwtSecurityTokenHandler();
            var token = handler.CreateToken(jwt);
            return handler.WriteToken(token);
        }

        #endregion

        // #region 取得國中名單
        // public List<MidSchool> GetMidSchoolList()
        // {
        //     List<MidSchool> midSchool = _memberRepository.getMidSchoolList();
        //     return midSchool;
        // }
        // #endregion

        #region 取得學校名單
        public List<School> GetSchoolList(bool IsHigh)
        {
            List<School> School = _memberRepository.getSchoolList(IsHigh);
            return School;
        }
        #endregion

        #region 取得目前使用者資料
        public UserInfoDetailDto GetAccountInfo(string account)
        {
            return _memberRepository.getAccountInfo(account);
        }
        #endregion
    }
}
