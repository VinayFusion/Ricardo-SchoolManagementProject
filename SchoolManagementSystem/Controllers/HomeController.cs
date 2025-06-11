using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading;
using System.Web;
using System.Web.Mvc;

namespace SchoolManagementSystem.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            return View();  
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }

        [HttpPost]
        public ActionResult ChangeLanguage(string language)
        {
            Thread.CurrentThread.CurrentCulture = CultureInfo.CreateSpecificCulture(language);
            Thread.CurrentThread.CurrentUICulture = new CultureInfo(language);

            Session["CurrentCulture"] = language;

            // Optional: Store culture in cookie for persistence
            HttpCookie cultureCookie = new HttpCookie("Culture", language)
            {
                Expires = DateTime.Now.AddYears(1)
            };
            Response.Cookies.Add(cultureCookie);

            return Redirect(Request.UrlReferrer.ToString());
        }



    }
}