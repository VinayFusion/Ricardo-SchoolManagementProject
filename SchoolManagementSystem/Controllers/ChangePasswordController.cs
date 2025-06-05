using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SchoolManagementSystem.Controllers
{
    public class ChangePasswordController : Controller
    {
        // GET: ChangePassword
        public ActionResult Index()
        {
            bool HasChangePasswordAccess = true;
            if (Request.Cookies["SuperAdminCookieSMA"] != null)
            {
                ViewBag.LayoutPath = "~/Views/Shared/_SuperAdminLayout.cshtml";
            }
            else if (Request.Cookies["AdminCookieSMA"] != null)
            {
                ViewBag.LayoutPath = "~/Views/Shared/_AdminLayout.cshtml";
            }
            else if (Request.Cookies["StaffCookieSMA"] != null)
            {
                ViewBag.LayoutPath = "~/Views/Shared/_StaffLayout.cshtml";
            }
            else
            {
                HasChangePasswordAccess = false;
            }

            if (!HasChangePasswordAccess)
            {
                return RedirectToAction("Index", "Login");
            }

            SetSidebarCookieInfo("changePassword");
            return View();
        }


        public void SetSidebarCookieInfo(string _val)
        {
            //Remove Sidebar cookie detail
            HttpCookie myCookie = new HttpCookie("SidebarCookieSMA");
            myCookie.Expires = DateTime.Now.AddDays(-1);
            Response.Cookies.Add(myCookie);

            HttpCookie myCookie_Sidebar = new HttpCookie("SidebarCookieSMA");
            myCookie_Sidebar["SelectedLink"] = _val;
            myCookie_Sidebar.Expires = DateTime.Now.AddDays(7);
            Response.Cookies.Add(myCookie_Sidebar);
        }
    }
}