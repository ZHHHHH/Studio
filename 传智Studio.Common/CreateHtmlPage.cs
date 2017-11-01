using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using 传智Studio.Model;

namespace 传智Studio.Common
{
    public class CreateHtmlPage
    {
        static Bulletin bulletin=null;
        static Recruit recruit=null;
        static Vote vote=null;
        static Project project=null;
        public static void CreateHtml(Object entity,string mark,string marklist,string link)
        {
            //拿到实体数据
            if(link=="BulletinPage")
            {
                bulletin = (Bulletin)entity;
            }
            else if(link=="RecruitPage")
            {
                recruit = (Recruit)entity;
            }
            else if (link == "VotePage")
            {
                vote = (Vote)entity;
            }
            else
            {
                project = (Project)entity;
            }
            string fullDir;
            //读取模板文件
            string template = HttpContext.Current.Request.MapPath("/Html/Template.html");
            string fileContent = File.ReadAllText(template);
            //根据实体数据替换模板占位符
            if (bulletin != null)
            {
                //替换模板占位符
                fileContent = fileContent.Replace("$title", bulletin.Title).Replace("$Name", bulletin.Name).Replace("$Time", bulletin.Time.ToString()).Replace("$Content", bulletin.Content).Replace("$mark", mark).Replace("$link", link).Replace("$list", marklist);
                //构建存储的文件路径
                string dir = "/Html/BulletinInformation/" + bulletin.Time.Year + "/" + bulletin.Time.Month + "/" + bulletin.Time.Day + "/";
                //创建实体文件
                Directory.CreateDirectory(Path.GetDirectoryName(HttpContext.Current.Request.MapPath(dir)));
                 fullDir = dir + bulletin.Id + ".html";
                //用完要赋值为null，因为后面又来写文件了这个是有值的，就总是会执行这段代码
                 bulletin = null;
            }
            else if(vote!=null)
            {
                //读取模板文件
                template = HttpContext.Current.Request.MapPath("/Html/VoteTemplate.html");
                fileContent = File.ReadAllText(template);
                //对投票选项分割
                 string[] str = vote.Answer.Split('∫');
                StringBuilder sb = new StringBuilder();
                for (int i = 0; i < str.Length-1; i++)
                {
                    sb.AppendFormat("<input type='submit' value='{0}' class='answer' name='answer' />&nbsp;&nbsp;&nbsp;", str[i]);
                }
                //替换模板占位符
                fileContent = fileContent.Replace("$title", vote.Title).Replace("$Name", vote.Name).Replace("$Time", vote.Time.ToString()).Replace("$Content", vote.Content).Replace("$mark", mark).Replace("$link", link).Replace("$list", marklist).Replace("$Answer",sb.ToString());
                //构建存储的文件路径
                string dir = "/Html/VoteInformation/" + vote.Time.Year + "/" + vote.Time.Month + "/" + vote.Time.Day + "/";
                //创建实体文件
                Directory.CreateDirectory(Path.GetDirectoryName(HttpContext.Current.Request.MapPath(dir)));
                 fullDir = dir + vote.Id + ".html";
                 //用完要赋值为null，因为后面又来写文件了这个是有值的，就总是会执行这段代码
                 vote = null;
            }
            else if(recruit!=null)
            {
                //替换模板占位符
                fileContent = fileContent.Replace("$title", recruit.Title).Replace("$Name", recruit.Name).Replace("$Time", recruit.Time.ToString()).Replace("$Content", recruit.Content).Replace("$mark", mark).Replace("$link", link).Replace("$list", marklist);
                //构建存储的文件路径
                string dir = "/Html/RecruitInformation/" + recruit.Time.Year + "/" + recruit.Time.Month + "/" + recruit.Time.Day + "/";
                //创建实体文件
                Directory.CreateDirectory(Path.GetDirectoryName(HttpContext.Current.Request.MapPath(dir)));
                 fullDir = dir + recruit.Id + ".html";
                 //用完要赋值为null，因为后面又来写文件了这个是有值的，就总是会执行这段代码
                 recruit = null;
            }
            else
            {
                //替换模板占位符
                fileContent = fileContent.Replace("$title", project.ProjectName).Replace("$Name", project.Name).Replace("$Time", project.Time.ToString()).Replace("$Content", project.Content).Replace("$mark", mark).Replace("$link", link).Replace("$list", marklist);
                //构建存储的文件路径
                string dir = "/Html/ProjectInformation/" + project.Time.Year + "/" + project.Time.Month + "/" + project.Time.Day + "/";
                //创建实体文件
                Directory.CreateDirectory(Path.GetDirectoryName(HttpContext.Current.Request.MapPath(dir)));
                 fullDir = dir + project.Id + ".html";
                 //用完要赋值为null，因为后面又来写文件了这个是有值的，就总是会执行这段代码
                 project = null;
            }
    
            //写入内容
            File.WriteAllText(HttpContext.Current.Request.MapPath(fullDir), fileContent, System.Text.Encoding.UTF8);
        }
    }
}
