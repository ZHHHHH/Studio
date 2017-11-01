using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Web;
using System.Web.Mvc;
using 传智Studio.BLL;
using 传智Studio.Common;
using 传智Studio.Model;

namespace 传智Studio.Controllers
{
    public class HomeController : Controller
    {
        UserInfoService userInfoService { get; set; }
        //
        // GET: /Index/
        //主页
        public ActionResult Index()
        {
            return View();
        }
        //登陆页面
        public ActionResult Login()
        {
            return View();
        }
        //404页面
        public ActionResult NotFindPage()
        {
            return View();
        }
        //出错页面
        public ActionResult ErrorPage()
        {
            return View();
        }

        #region 完成用户登录
        [HttpPost]
        public ActionResult UserLogin()
        {
            string Username = Request["Username"];
            string Password = Request["Password"];
            if (!string.IsNullOrEmpty(Username) && !string.IsNullOrEmpty(Password))
            {
                string pwd = CreateMd5.GetMd5String(CreateMd5.GetMd5String(Password));
                var userInfo = userInfoService.LoadEntities(u => u.UserAccount == Username && u.UserPwd == pwd).FirstOrDefault();
                if (userInfo != null)
                {
                   Session["UserAccount"] = userInfo.UserAccount;
                    //用户自定义的昵称
                    if (!string.IsNullOrEmpty(userInfo.UserName))
                    {
                        Session["UserName"] = userInfo.UserName;
                    }
                    else
                    {
                        Session["UserName"] = Username;
                    }
                    return Content("ok:登陆成功!");
                }
                else
                {
                    return Content("no:用户账号或密码有误!");
                }
            }
            else
            {
                return Content("no:用户账号或密码有误!");
            }

        } 
        #endregion
        #region 显示验证码
        public ActionResult ShowValidateCode()
        {
            Common.ValidateCode vliateCode = new Common.ValidateCode();
            string code = vliateCode.CreateValidateCode(4);//产生验证码
            Session["validateCode"] = code;
            byte[] buffer = vliateCode.CreateValidateGraphic(code);//将验证码画到画布上.
            return File(buffer, "image/jpeg");
        }
        #endregion
        #region 完成用户注册
        [HttpPost]
        public ActionResult UserRegister()
        {
            string Email = Request["Email"];
            string Validata = Request["Validata"];
            //接收用户的激活码并进行加密处理
            string ValidataNumber = CreateMd5.GetMd5String(CreateMd5.GetMd5String(Request["ValidataNumber"]));
            //获取内部生产的激活码
            string Number = ValidateCode.ValidataGenerate();
            if (!string.IsNullOrEmpty(Email) && !string.IsNullOrEmpty(Validata) && !string.IsNullOrEmpty(ValidataNumber))
            {
                //判断数据库是否已经有相同的用户账号
                var userinfo = userInfoService.LoadEntities(u => u.UserAccount == Email).FirstOrDefault();
                if (userinfo == null)
                {
                    //验证码的校验
                    if (Validata == Session["validateCode"].ToString())
                    {
                        //激活码的校验
                        if (ValidataNumber == Number)
                        {
                            Random r = new Random();
                            //生成随机密码
                            string pwd = r.Next(100000, 999999).ToString();
                            string EmailContent = "你好！\n\n你在" + DateTime.Now + "在传智工作室后台管理系统进行邮箱注册。\n\n注意:邮箱账号就是你的登陆账号。\n\n以下号码是系统随机生成的密码,请使用该密码进行网站的登陆。\n\n" + pwd + "\n\n如需修改密码可以登陆网站后在用户设置中进行密码的修改。\n\n感谢使用！";
                            //完成邮件的发送
                            if (SendEmailInfo.SendEmail(Email, EmailContent))
                            {
                                //完成用户信息写入数据库
                                UserInfo userInfo = new UserInfo();
                                userInfo.UserAccount = Email;
                                userInfo.UserPwd = CreateMd5.GetMd5String(CreateMd5.GetMd5String(pwd));
                                userInfo.RegTime = DateTime.Now;
                                userInfoService.AddEntity(userInfo);
                                return Content("ok:邮件已发送，请注意查收!");
                            }
                            else
                            {
                                return Content("no:邮件发送失败，请保证邮箱格式的正确性");
                            }
                        }
                        else
                        {
                            return Content("no:激活码错误!(只允许内部人员的注册)");
                        }
                    }
                    else
                    {
                        return Content("no:验证码有误!");
                    }
                }
                else
                {
                    return Content("no:该邮箱已经被注册!");
                }
            }
            else
            {
                return Content("no:输入信息有误!");
            }
        } 
        #endregion
        
    }
}
