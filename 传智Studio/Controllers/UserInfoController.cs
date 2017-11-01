using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Caching;
using System.Web.Mvc;
using 传智Studio.BLL;
using 传智Studio.Common;
using 传智Studio.Model;

namespace 传智Studio.Controllers
{
    public class UserInfoController :BaseController
    {
        UserInfoService userInfoService { get; set; }
        //
        // GET: /UserInfo/

        //密码修改页
        public ActionResult EditPwdPage()
        {
            return View();

        }
        //用户信息页
        public ActionResult UserInfoPage()
        {
            string UserAccount = Session["UserAccount"].ToString();
           var UserInfo=userInfoService.LoadEntities(u =>u.UserAccount==UserAccount);
           ViewData.Model =UserInfo;
            return View();
        }
        #region 完成用户信息的修改
        [HttpPost]
        public ActionResult EditUserInfo()
        {
            string UserAccount = Session["UserAccount"].ToString();
            var userInfo = userInfoService.LoadEntities(u => u.UserAccount == UserAccount).FirstOrDefault();
            string UserName = Request["UserName"];
            string PhoneNumber = Request["PhoneNumber"];
            string Professional = Request["Professional"];
            string Grade = Request["Grade"];
            string Department = Request["Department"];
            string Campus = Request["Campus"];
            string sex = Request["sex"];
            userInfo.UserName = UserName;
            userInfo.PhoneNumber = PhoneNumber;
            userInfo.Professional = Professional;
            userInfo.Grade = Grade;
            userInfo.Department = Department;
            userInfo.Campus = Campus;
            userInfo.Sex = sex;
            if(!string.IsNullOrEmpty(UserName))
            {
                Session["UserName"] = UserName;
            }
            else
            {
                Session["UserName"] = UserAccount;
            }
            userInfoService.EditEntity(userInfo);
            return Content("ok");
        } 
        #endregion
        #region 完成用户密码的修改
        [HttpPost]
        public ActionResult EditPwd()
        {
            //接收用户的信息
            string CurrentPwd = Request["CurrentPwd"];
            string EditPwd = Request["EditPwd"];
            string EditPwdAgain = Request["EditPwdAgain"];
            //校验数据
            if (!string.IsNullOrEmpty(CurrentPwd) && !string.IsNullOrEmpty(EditPwd) && !string.IsNullOrEmpty(EditPwdAgain))
            {
                if (EditPwd == EditPwdAgain)
                {
                    if (!(EditPwd.Length < 6))
                    {
                        string Pwd = CreateMd5.GetMd5String(CreateMd5.GetMd5String(CurrentPwd));
                        string userAccount = Session["UserAccount"].ToString();
                        var userInfo = userInfoService.LoadEntities(u => u.UserAccount == userAccount && u.UserPwd == Pwd).FirstOrDefault();
                        if (userInfo != null)
                        {
                            Pwd = CreateMd5.GetMd5String(CreateMd5.GetMd5String(EditPwdAgain));
                            userInfo.UserPwd = Pwd;
                            userInfoService.EditEntity(userInfo);
                            Session["UserAccount"] = null;
                            Session["UserName"] = null;
                            return Content("ok:密码修改成功!");
                        }
                        else
                        {
                            return Content("no:输入的当前密码错误!");
                        }
                    }
                    else
                    {
                        return Content("no:密码长度不能小于6个字符!");
                    }
                }
                else
                {
                    return Content("no:输入的新密码不一致!");
                }

            }
            else
            {
                return Content("no:信息不能为空!");
            }
        } 
        #endregion
        #region 完成忘记密码的修改
        [HttpPost]
        public ActionResult ForgetPwd()
        {
            string Email = Session["UserAccount"].ToString();
            string EditPwd = Request["EditPwd"];
            string EditPwdAgain = Request["EditPwdAgain"];
            string EmailValidata = Request["EmailValidata"];
            if (HttpContext.Cache[Email] != null)
            {
                if (HttpContext.Cache[Email].ToString() == EmailValidata)
                {
                    if (EditPwd == EditPwdAgain)
                    {
                        if (EditPwd.Length >= 6)
                        {
                            var userInfo = userInfoService.LoadEntities(u => u.UserAccount == Email).FirstOrDefault();
                            EditPwd = CreateMd5.GetMd5String(CreateMd5.GetMd5String(EditPwdAgain));
                            userInfo.UserPwd = EditPwd;
                            userInfoService.EditEntity(userInfo);
                            //完成后设置为过期
                            HttpContext.Cache.Insert(Email, "", null, DateTime.Now.AddSeconds(0.1), TimeSpan.Zero, System.Web.Caching.CacheItemPriority.Normal, null);
                            Session["UserAccount"] = null;
                            Session["UserName"] = null;
                            return Content("ok:修改成功！");
                        }
                        else
                        {
                            return Content("no:密码长度不能小于6个字符!");
                        }
                    }
                    else
                    {
                        return Content("no:输入密码不一致!");
                    }
                }
                else
                {
                    return Content("no:验证码错误!");
                }
            }
            else
            {
                return Content("no:验证码已经过期，请重新发送!");
            }
        }
        #endregion
        #region 加载用户信息
        public ActionResult UserInfoLoding()
        {
            if (Session["UserName"] != null)
            {
                return Content("ok:" + Session["UserName"].ToString());
            }
            else
            {
                return Content("no:没有登陆");
            }
        }
        #endregion
        #region 完成用户的登出
        public ActionResult LogOut()
        {
            Session["UserAccount"] = null;
            Session["UserName"] = null;
            return Redirect("/Home/Login");
        }
        #endregion
        #region 完成邮件的发送
        public ActionResult SendEmail()
        {
            string Email = Session["UserAccount"].ToString();
            string UserName=Session["UserName"].ToString();
            Random r=new Random();
             //生成随机验证码
            string validata = r.Next(10000, 99999).ToString();
            //设置限制时间5分钟
            
            HttpContext.Cache.Insert(Email, validata, null, DateTime.Now.AddMinutes(5), TimeSpan.Zero, System.Web.Caching.CacheItemPriority.Normal, null);//键，值，依赖，绝对过期时间，相对过期时间，优先级，
            string EmailContent = UserName + "你好!\n\n你在" + DateTime.Now + "在传智工作后台管理系统进行忘记密码的操作。\n\n注意:邮箱账号就是你的登陆账号。\n\n以下号码是系统随机生成的验证码,请使用该验证码进行后续的忘记密码的操作，注意验证码有效时间为5分钟。\n\n" + validata + "\n\n感谢使用！";
            if(SendEmailInfo.SendEmail(Email, EmailContent))
            {
                return Content("ok:邮件发送成功！");
            }
            else
            {
                return Content("no:邮件发送失败！请重试！");
            }
        }

        #endregion
        

    }
}
