using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using 传智Studio.BLL;
using 传智Studio.Common;
using 传智Studio.Model;

namespace 传智Studio.Controllers
{
    public class AdminController : BaseController
    {
        BulletinService bulletinService { get; set; }
        ProjectService projectService { get; set; }
        VoteService voteService { get; set; }
        RecruitService recruitService { get; set; }
        UserInfoService userInfoService { get; set; }
        //
        // GET: /Admin/

        //后台系统的主页
        public ActionResult Index()
        {
            //拿到各种公共的信息
            ShowInforMation(6, "bulletinService");
            ShowInforMation(6, "projectService");
            ShowInforMation(6, "voteService");
            ShowInforMation(6, "recruitService");
            return View();
        }
        //公告页
        public ActionResult BulletinPage()
        {
            ShowInforMation(10, "bulletinService");
            return View();
        }
        //招聘页
        public ActionResult RecruitPage()
        {
            ShowInforMation(10, "recruitService");
            return View();
        }
        //投票页
        public ActionResult VotePage()
        {
            ShowInforMation(10, "voteService");
            return View();
        }
        //项目页
        public ActionResult ProjectPage()
        {
            ShowInforMation(10, "projectService");
            return View();
        }
        //功能开发中Page
        public ActionResult FunctionPage()
        {
            return View();
        }
       
        #region 获取最新的公共信息
        public void ShowInforMation(int pageSize,string str)//pageSize:根据传过来的数字决定一次显示多少条数据
        {
            //页码
            int pageIndex;
            if (!int.TryParse(Request["pageIndex"], out pageIndex))
            {
                pageIndex = 1;
            }
            //拿到总页数
            int totalCount;
            if (str == "bulletinService")
            {
                //根据分页拿到数据
                var bulletinList = bulletinService.LoadPageEntities<DateTime>(pageIndex, pageSize, out totalCount, b => true, b => b.Time);
                ViewData.Model = bulletinList;
                ViewData["bulletinList"] = bulletinList;
                ViewData["pageIndex"] = pageIndex;
                ViewData["totalCountBulletin"] = totalCount;
            }
            else if(str=="projectService")
            {
                //根据分页拿到数据
                var projectList = projectService.LoadPageEntities<DateTime>(pageIndex, pageSize, out totalCount, p => true, p => p.Time);
                ViewData.Model = projectList;
                ViewData["projectList"] = projectList;
                ViewData["pageIndex"] = pageIndex;
                ViewData["totalCountProject"] = totalCount;
            }
            else if(str=="voteService")
            {
                //根据分页拿到数据
                var voteList = voteService.LoadPageEntities<DateTime>(pageIndex, pageSize, out totalCount, v => true, v => v.Time);
                ViewData.Model = voteList;
                ViewData["voteList"] = voteList;
                ViewData["pageIndex"] = pageIndex;
                ViewData["totalCountVote"] = totalCount;
            }
            else
            {
                //根据分页拿到数据
                var recruitList = recruitService.LoadPageEntities<DateTime>(pageIndex, pageSize, out totalCount, r => true, r => r.Time);
                ViewData.Model = recruitList;
                ViewData["recruitList"] = recruitList;
                ViewData["pageIndex"] = pageIndex;
                ViewData["totalCountRecruit"] = totalCount;
            }
        } 
        #endregion

       //完成各类信息的发布
        #region 完成公告的发布
        [ValidateInput(false)][HttpPost]
        public ActionResult EditBulletin()
        {
            //接受用户输入的数据
            string title = Request["title"];
            string content = Request["content"];
            //校验数据
            if (!string.IsNullOrEmpty(title)&&title.Length<=20)
            {
                //写入数据库
                if (!string.IsNullOrEmpty(content))
                {
                    Bulletin bulletin = new Bulletin();
                    bulletin.Title = title;
                    bulletin.Content = content;
                    bulletin.Name = Session["UserName"].ToString();
                    bulletin.Time = DateTime.Now;
                    if (bulletinService.AddEntity(bulletin))
                    {
                        //完成公告详细页面的创建
                        CreateHtmlPage.CreateHtml(bulletin, "公告", "公告信息列表", "BulletinPage");
                        return Content("ok:提交成功!");
                    }
                    else
                    {
                        return Content("no:提交失败!");
                    }
                }
                else
                {
                    return Content("no:内容不能为空!");
                }
            }
            else 
            {
                return Content("no:标题不能为空并且长度不能大于20个字符!");
            }
        }
        #endregion

        #region 完成招聘信息的发布
        [ValidateInput(false)][HttpPost]
        public ActionResult EditRecruit()
        { 
            //接收用户的输入数据
            string title = Request["Title"];
            string company = Request["Company"];
            string job = Request["Job"];
            string content = Request["content"];
            //校验数据
            if(!string.IsNullOrEmpty(title)&&title.Length<=20)
            {
                if(!string.IsNullOrEmpty(company)&&company.Length<=8)
                {
                    if(!string.IsNullOrEmpty(job)&&job.Length<=10)
                    {
                        if(!string.IsNullOrEmpty(content))
                        {
                            Recruit recruit = new Recruit();
                            recruit.Title = title;
                            recruit.Company = company;
                            recruit.Job = job;
                            recruit.Content = content;
                            recruit.Time = DateTime.Now;
                            recruit.Name = Session["UserName"].ToString();
                            recruitService.AddEntity(recruit);
                            CreateHtmlPage.CreateHtml(recruit, "招聘", "招聘信息列表", "RecruitPage");
                            return Content("ok:提交成功!");

                        }
                        else
                        {
                            return Content("no:招聘详情不能为空!");
                        }
                    }
                    else
                    {
                        return Content("no:招聘岗位不能为空并且长度不能大于10个字符!");
                    }
                }
                else
                {
                    return Content("no:公司名称不能为空并且长度不能大于8个字符!");
                }
            }
            else
            {
                return Content("no:标题不能为空并且长度不能大于20个字符!");
            }
        }
        #endregion

        #region 完成项目信息的发布
        [ValidateInput(false)][HttpPost]
        public ActionResult EditProject()
        { 
            //接收用户输入的信息
            string projectName = Request["ProjectName"];
            string progress = Request["Progress"];
            string content = Request["content"];
            //数据校验
            if(!string.IsNullOrEmpty(projectName)&&projectName.Length<=20)
            {
                if(!string.IsNullOrEmpty(progress))
                {
                    if(!string.IsNullOrEmpty(content))
                    {
                        Project project = new Project();
                        project.ProjectName = projectName;
                        project.Progress =progress;
                        project.Content = content;
                        project.Name = Session["UserName"].ToString();
                        project.Time = DateTime.Now;
                        projectService.AddEntity(project);
                        CreateHtmlPage.CreateHtml(project, "项目信息", "项目信息列表", "ProjectPage");
                        return Content("ok:提交成功!");
                    }
                    else
                    {
                        return Content("no:项目详情不能为空!");
                    }
                }
                else
                {
                    return Content("no:开发进度不能为空!");
                }
            }
            else 
            {
                return Content("no:项目名称不能为空并且长度不能大于20个字符!");
            }
        }
        #endregion

        #region 完成投票的发布
        [ValidateInput(false)][HttpPost]
        public ActionResult EditVote()
        { 
            //接收用户的数据
            string title = Request["Title"];
            string content = Request["content"];
            int number = Convert.ToInt32(Request["number"]);
            if(number==0)
            {
                return Content("no:投票选项不能为空!");
            }
            string[] str = new string[number];
            //将用户添加的答案循环取出放进数组中
            for (int i = 0; i <number; i++)
            {
                str[i] = Request["Option" + (i + 1)];
                if(string.IsNullOrEmpty(str[i]))
                {
                    return Content("no:投票选项不能为空!");
                }
            }
            //进行数据的校验
            if(!string.IsNullOrEmpty(title)&&title.Length<=20)
            {
                if(!string.IsNullOrEmpty(content))
                {
                    if(str.Length>=1)
                    {
                        Vote vote = new Vote();
                        vote.Time = DateTime.Now;
                        vote.Title = title;
                        vote.Content = content;
                        vote.Name = Session["UserName"].ToString();                   
                        for (int i = 0; i < str.Length; i++)
                        {
                            vote.Answer += str[i] + "∫";
                        }
                        voteService.AddEntity(vote);
                        CreateHtmlPage.CreateHtml(vote, "投票", "投票列表", "VotePage");
                        return Content("ok:提交成功!");
                    }
                    else
                    {
                        return Content("no:投票选项不能为空!");
                    }
                }
                else
                {
                    return Content("no:投票内容不能为空!");
                }
            }
            else
            {
                return Content("no:投票主题不能为空并且长度不能大于20个字符!");
            }
        }
        #endregion
       //完成投票模块
        #region 完成投票
        public ActionResult VotePress()
        {
            string url=Request.UrlReferrer.ToString();
            //接收信息
            string answer = Request["answer"];
            string name = Request["Name"];
            DateTime time =Convert.ToDateTime(Request["Time"]);
            //根据投票时间查出这条信息
            var voteInfo = voteService.LoadEntities(v => v.Time.Year == time.Year&&v.Time.Month==time.Month&&v.Time.Day==time.Day&&v.Time.Hour==time.Hour&&v.Time.Minute==time.Minute&&v.Time.Second==time.Second).FirstOrDefault();
            voteInfo.Voters += name + ":" + answer + "∫";
            //把用户投票的答案写入数据库
            voteService.EditEntity(voteInfo);
            return Redirect(url);

        }
        #endregion

        #region 展示投票的结果信息
        public ActionResult ShowAnswer()
        {
            DateTime time = Convert.ToDateTime(Request["Time"]);
            var voteInfo = voteService.LoadEntities(v => v.Time.Year == time.Year && v.Time.Month == time.Month && v.Time.Day == time.Day && v.Time.Hour == time.Hour && v.Time.Minute == time.Minute && v.Time.Second == time.Second).FirstOrDefault();
            return Content(voteInfo.Voters);
        }
        #endregion

        #region 判断用户是否已经投票
        public ActionResult VoteorNot()
        {
            DateTime time = Convert.ToDateTime(Request["Time"]);
            //查询出这条投票信息
            var voteInfo = voteService.LoadEntities(v => v.Time.Year == time.Year && v.Time.Month == time.Month && v.Time.Day == time.Day && v.Time.Hour == time.Hour && v.Time.Minute == time.Minute && v.Time.Second == time.Second).FirstOrDefault();
                string str1 = Session["UserAccount"].ToString();
                string str2 = Session["UserName"].ToString();
                //根据投票信息查询是否保持用户的账号或者昵称
                if (!string.IsNullOrEmpty(voteInfo.Voters))
                {
                    bool b1 = voteInfo.Voters.Contains(str1);
                    bool b2 = voteInfo.Voters.Contains(str2);
                    if (b1 || b2)
                    {
                        return Content("ok");
                    }
                    else
                    {
                        return Content("no");
                    }
                }
                else
                {
                    return Content("no");
                }
            
        }
        #endregion
      

    }
}
