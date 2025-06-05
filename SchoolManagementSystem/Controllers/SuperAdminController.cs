using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SchoolManagementSystem.Controllers
{
    public class SuperAdminController : Controller
    {
        public JsonResult GetSuperAdminCookieDetail()
        {
            string _Token = "";
            HttpCookie myCookie = Request.Cookies["SuperAdminCookieSMA"];

            if (myCookie != null)
            {
                _Token = myCookie["UserToken"].ToString();
            }
            return Json(_Token, JsonRequestBehavior.AllowGet);
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
        public JsonResult GetSidebarCookieDetail()
        {
            string _SelectedLink = "";
            HttpCookie myCookie = Request.Cookies["SidebarCookieSMA"];

            if (myCookie != null)
            {
                _SelectedLink = myCookie["SelectedLink"].ToString();
            }
            return Json(_SelectedLink, JsonRequestBehavior.AllowGet);
        }
        public JsonResult GetCurrentDate()
        {
            #region Get Current Date
            DateTime dt = DateTime.UtcNow.AddMinutes(330);

            string _CurrentDate = "";

            string dy = dt.ToString("dd");
            string mn = dt.ToString("MM");
            string yyyy = dt.ToString("yyyy");

            _CurrentDate = yyyy + "-" + mn + "-" + dy;

            return Json(_CurrentDate, JsonRequestBehavior.AllowGet);
            #endregion
        }
        public bool ValidateSuperAdmin()
        {
            bool _isValid = false;
            HttpCookie myCookie = Request.Cookies["SuperAdminCookieSMA"];

            if (myCookie != null)
            {
                _isValid = true;
            }

            return _isValid;
        }

        // GET: SuperAdmin
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult ManageSchool()
        {
            return View();
        }

        public ActionResult ManageProfile()
        {
            bool _ValidationStatus = ValidateSuperAdmin();
            if (_ValidationStatus == true)
            {
                SetSidebarCookieInfo("manageProfile");
                ViewBag.UsertypeId = 1;
                return View();
            }
            else
            {
                return RedirectToAction("Index", "Login");
            }
        }

    }
}