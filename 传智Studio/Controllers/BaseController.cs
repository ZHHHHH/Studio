using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace 传智Studio.Controllers
{
    public class BaseController : Controller
    {
        //
        // GET: /Base/
        protected override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            base.OnActionExecuting(filterContext);
            if (Session["UserAccount"] == null)
            {
                filterContext.Result = Redirect("/Home/Login");
                return;
            }
        }


    }
}
