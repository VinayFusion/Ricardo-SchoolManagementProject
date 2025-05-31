using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SchoolManagementSystem.Controllers
{
    public class StaffController : Controller
    {
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

        public JsonResult GetStaffCookieDetail()
        {
            string _Token = "";
            HttpCookie myCookie = Request.Cookies["StaffCookieSMA"];

            if (myCookie != null)
            {
                _Token = myCookie["UserToken"].ToString();
            }
            return Json(_Token, JsonRequestBehavior.AllowGet);
        }

        public bool ValidateStaff()
        {
            bool _isValid = false;
            HttpCookie myCookie = Request.Cookies["StaffCookieSMA"];

            if (myCookie != null)
            {
                _isValid = true;
            }

            return _isValid;
        }
        //---------------------------------------------------------------------------------------------------------------------------------

        public ActionResult Index()
        {
            bool _ValidateStatus = ValidateStaff();

            if (_ValidateStatus == true)
            {
                SetSidebarCookieInfo("manageDashboard");
                return View();
            }
            else
            {
                return RedirectToAction("Index", "Login");
            }
        }


        public ActionResult ManageClass()
        {
            bool _ValidateStatus = ValidateStaff();

            if (_ValidateStatus == true)
            {
                SetSidebarCookieInfo("manageClass");
                return View();
            }
            else
            {
                return RedirectToAction("Index", "Login");
            }
        }

        public ActionResult ManageSession()
        {
            bool _ValidateStatus = ValidateStaff();

            if (_ValidateStatus == true)
            {
                SetSidebarCookieInfo("manageSession");
                return View();
            }
            else
            {
                return RedirectToAction("Index", "Login");
            }
        }
        public ActionResult ManageSection()
        {
            bool _ValidateStatus = ValidateStaff();

            if (_ValidateStatus == true)
            {
                SetSidebarCookieInfo("manageSection");
                return View();
            }
            else
            {
                return RedirectToAction("Index", "Login");
            }
        }

        public ActionResult ManageStudent()
        {
            bool _ValidateStatus = ValidateStaff();

            if (_ValidateStatus == true)
            {
                SetSidebarCookieInfo("manageStudent");
                return View();
            }
            else
            {
                return RedirectToAction("Index", "Login");
            }
        }
        public ActionResult StudentDetail()
        {
            bool _ValidateStatus = ValidateStaff();

            if (_ValidateStatus == true)
            {
                SetSidebarCookieInfo("studentDetail");
                return View();
            }
            else
            {
                return RedirectToAction("Index", "Login");
            }
        }

        public ActionResult ManagePayFee()
        {
            bool _ValidateStatus = ValidateStaff();

            if (_ValidateStatus == true)
            {
                SetSidebarCookieInfo("managePayFee");
                return View();
            }
            else
            {
                return RedirectToAction("Index", "Login");
            }
        }


        public ActionResult PendingFee()
        {
            bool _ValidateStatus = ValidateStaff();

            if (_ValidateStatus == true)
            {
                SetSidebarCookieInfo("pendingFee");
                return View();
            }
            else
            {
                return RedirectToAction("Index", "Login");
            }
        }

        public ActionResult ExpiredCourseFee()
        {
            bool _ValidateStatus = ValidateStaff();

            if (_ValidateStatus == true)
            {
                SetSidebarCookieInfo("expiredCourseFee");
                return View();
            }
            else
            {
                return RedirectToAction("Index", "Login");
            }
        }

        public ActionResult CompletedCourseFee()
        {
            bool _ValidateStatus = ValidateStaff();

            if (_ValidateStatus == true)
            {
                SetSidebarCookieInfo("completedCourseFee");
                return View();
            }
            else
            {
                return RedirectToAction("Index", "Login");
            }
        }

        public ActionResult ManageRevenue()
        {
            bool _ValidateStatus = ValidateStaff();

            if (_ValidateStatus == true)
            {
                SetSidebarCookieInfo("manageRevenue");
                return View();
            }
            else
            {
                return RedirectToAction("Index", "Login");
            }
        }

        public ActionResult ChangePassword()
        {
            bool _ValidateStatus = ValidateStaff();

            if (_ValidateStatus == true)
            {
                SetSidebarCookieInfo("changePassword");
                return View();
            }
            else
            {
                return RedirectToAction("Index", "Login");
            }
        }

        public ActionResult ManageExpenditure()
        {
            bool _ValidateStatus = ValidateStaff();

            if (_ValidateStatus == true)
            {
                SetSidebarCookieInfo("manageExpenditure");
                return View();
            }
            else
            {
                return RedirectToAction("Index", "Login");
            }
        }

        public ActionResult ManageEnquiry()
        {
            bool _ValidateStatus = ValidateStaff();

            if (_ValidateStatus == true)
            {
                SetSidebarCookieInfo("manageEnquiry");
                return View();
            }
            else
            {
                return RedirectToAction("Index", "Login");
            }
        }

        public ActionResult ManageExam()
        {
            bool _ValidationStatus = ValidateStaff();
            if (_ValidationStatus == true)
            {
                SetSidebarCookieInfo("manageExam");
                return View();
            }
            else
            {
                return RedirectToAction("Index", "Login");
            }
        }

        public ActionResult ManageProfile()
        {
            bool _ValidationStatus = ValidateStaff();
            if (_ValidationStatus == true)
            {
                SetSidebarCookieInfo("manageProfile");
                ViewBag.UsertypeId = 2;
                return View();
            }
            else
            {
                return RedirectToAction("Index", "Login");
            }

        }

    }
}