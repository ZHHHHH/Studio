using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mail;
namespace 传智Studio.Common
{
    public class SendEmailInfo
    {
        //#region 发送邮件
        //public static bool SendEmail(string Email, string EmailContent)
        //{
        //    try
        //    {
        //        MailMessage mailMsg = new MailMessage();//两个类，别混了，要引入System.Net这个Assembly
        //        mailMsg.From = new MailAddress("1015300310@qq.com", "传智工作室");//源邮件地址 
        //        mailMsg.To.Add(new MailAddress(Email));//目的邮件地址。可以有多个收件人
        //        mailMsg.Subject = "传智工作室后台管理系统邮箱注册";//发送邮件的标题 
        //        mailMsg.Body = EmailContent;//发送邮件的内容 
        //        SmtpClient client = new SmtpClient("smtp.qq.com");//smtp.163.com，smtp.qq.com
        //        client.Port = 25;
        //        client.EnableSsl = true;//SSL加密传输
        //        client.Credentials = new NetworkCredential("1015300310@qq.com", "jbwmfeaqdktybfai");
        //        //jbwmfeaqdktybfai,13650004286@163.com
        //        client.Send(mailMsg);
        //        return true;
        //    }
        //    catch(Exception e)
        //    {
        //        HttpContext.Current.Response.Write(e.Message+"%");
        //        return false;
        //    }
        //}
        //#endregion
        #region 发送邮件
        public static bool SendEmail(string Email, string EmailContent)
        {
            MailMessage _mailMessage = new MailMessage();
            _mailMessage.Body = EmailContent;
            _mailMessage.From = "\"传智工作室\"<1015300310@qq.com>"; //发件人
            _mailMessage.Subject = "传智工作室后台管理系统邮箱注册";//题目
            _mailMessage.To = Email;//收件人
            //_mailMessage.BodyEncoding = System.Text.Encoding.UTF8;
            //_mailMessage.BodyFormat = MailFormat.Html;
            _mailMessage.Fields.Add("http://schemas.microsoft.com/cdo/configuration/smtpauthenticate", 1);
            _mailMessage.Fields.Add("http://schemas.microsoft.com/cdo/configuration/sendusername", "1015300310@qq.com");
            _mailMessage.Fields.Add("http://schemas.microsoft.com/cdo/configuration/sendpassword", "jbwmfeaqdktybfai");
            _mailMessage.Fields.Add("http://schemas.microsoft.com/cdo/configuration/smtpserverport", 465);
            _mailMessage.Fields.Add("http://schemas.microsoft.com/cdo/configuration/smtpusessl", "true");
            SmtpMail.SmtpServer = "smtp.qq.com";
            try
            {
                SmtpMail.Send(_mailMessage);
                return true;
            }
            catch (Exception ex)
            {
                HttpContext.Current.Response.Write(ex.Message);
                return false;
            }
        }
        #endregion
    }
}
