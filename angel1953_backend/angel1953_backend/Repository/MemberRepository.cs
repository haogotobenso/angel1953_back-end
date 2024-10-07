﻿using Microsoft.EntityFrameworkCore;
using angel1953_backend.Models;
using angel1953_backend.Dtos;
using System.Reflection.Metadata.Ecma335;
namespace angel1953_backend.Repository
{
    public class MemberRepository
    {

        private angel1953Context _context;
        public MemberRepository(angel1953Context angel1953context) 
        {
            _context = angel1953context;
        }
        #region 帳號重複查詢
        public bool checkAccount(string Account) 
        {
            try 
            {
                var account = (from a in _context.Member
                               where a.Account == Account
                               select a).SingleOrDefault();
                if (account == null)
                {
                    return true;
                }
                else
                {
                    return false;
                }

            }
            catch (Exception ex)
            {
                throw new Exception(ex.ToString());
            }
          
        }
        #endregion
        #region 帳號註冊
        public void register(Member member) 
        {
            try 
            {
                _context.Add(member);
                _context.SaveChanges();
            }
            catch (Exception ex) 
            {
                throw new Exception(ex.ToString());
            }
        }
        #endregion
        #region 取得班級Id
        public int getClassId(string classname)
        {
            try
            {
                int classid = sreachClass(classname);
                if (classid == -1)
                {
                    classid = addClass(classname);
                }
                return classid;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.ToString());
            }

        }
        #endregion
        #region 新增班級
        public int addClass(string classname)
        {
            try
            {
                Class newClass = new Class
                {
                    Class1 = classname
                };

                _context.Class.Add(newClass);
                _context.SaveChanges();
                return newClass.ClassId; 
            }
            catch (Exception ex)
            {
                throw new Exception(ex.ToString());
            }
        }
        #endregion
        #region 查詢班級
        public int sreachClass(string classname)
        {
            try
            {
                int classid = (from c in _context.Class
                                where c.Class1 == classname
                               select c.ClassId).SingleOrDefault();
                return classid == 0 ? -1 : classid;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.ToString());
            }
        }
        #endregion
        #region email驗證
        public bool emailValidate(string account, string authCode) 
        {
            try
            {
                var check = _context.Member.SingleOrDefault(a => a.Account == account && a.AuthCode == authCode);
                if (check != null)
                {
                    check.AuthCode = null;
                    _context.SaveChanges();
                    return true;
                }
                else 
                {
                    return false;
                }
            }
            catch (Exception ex) 
            {
                throw new Exception(ex.ToString());
            }
        }
        #endregion

        #region 取得一筆會員資料
        public Member getMemberByAccount(string account)
        {
            Member oneMember = new Member();
            try
            {
                oneMember = _context.Member.SingleOrDefault(a=>a.Account == account);
                return oneMember;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.ToString());
            }
        }
        #endregion

        #region  變更密碼
        public bool changePassword(string account, string newpassword)
        {
            var result = _context.Member.SingleOrDefault(a => a.Account == account);
            if (result != null)
            {
                result.Password = newpassword;
                try
                {
                    _context.SaveChanges();
                }
                catch (DbUpdateConcurrencyException)
                {
                    throw;
                }
                return true;
            }
            else
            {
                return false;
            }
        }
        #endregion

        #region 取得國中名單
        public List<MidSchool> getMidSchoolList()
        {
            
            try
            {
                List<MidSchool> midSchools = _context.MidSchool.ToList();
                return midSchools; 

            }
            catch (Exception ex)
            {
                throw new Exception(ex.ToString());
            }
        }
        #endregion

        #region 取得高中名單
        public List<School> getSchoolList()
        {
            
            try
            {
                List<School> Schools = _context.School.ToList();
                return Schools; 

            }
            catch (Exception ex)
            {
                throw new Exception(ex.ToString());
            }
        }
        #endregion

        // #region 取得學校Id
        // public int getSchoolId(string school) 
        // {
        //     try 
        //     {
        //         int schoolid = sreachSchool(school);
        //         if (schoolid == -1) 
        //         {
        //             schoolid=addSchool(school);
        //         }
        //         return schoolid;
        //     }catch (Exception ex) 
        //     {
        //         throw new Exception(ex.ToString());
        //     }

        // }
        // #endregion
        // #region 新增學校
        // public int addSchool(string school) 
        // {
        //     try 
        //     {
        //         School newSchool = new School();
        //         newSchool.School1 = school;
        //         _context.School.Add(newSchool);
        //         _context.SaveChanges();
        //         return newSchool.SchoolId;

        //     }
        //     catch (Exception ex) 
        //     {
        //         throw new Exception(ex.ToString());
        //     }
        // }

        // #endregion
        // #region 查詢學校
        // public int sreachSchool(string school) 
        // {
        //     try
        //     {
        //         int schoolid = _context.School
        //                       .Where(s => s.School1 == school)
        //                       .Select(s => s.SchoolId)
        //                       .SingleOrDefault();

        //         return schoolid == 0 ? -1 : schoolid;
        //     }
        //     catch (Exception ex)
        //     {
        //         throw new Exception(ex.ToString());
        //     }
        // }
        // #endregion
    }
}