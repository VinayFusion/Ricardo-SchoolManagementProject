using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Newtonsoft.Json.Linq;

namespace SchoolManagementSystem.Controllers
{
    public class AdminController : Controller
    {
        public JsonResult GetAdminCookieDetail()
        {
            string _Token = "";
            HttpCookie myCookie = Request.Cookies["AdminCookieSMA"];

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

        //public JsonResult SetPayFeeFormCookie()
        //{
        //    //Remove Pay-Fee-Form cookie detail
        //    HttpCookie myCookie = new HttpCookie("PayFeeFormCookieSMA");
        //    myCookie.Expires = DateTime.Now.AddDays(-1);
        //    Response.Cookies.Add(myCookie);

        //    HttpCookie myCookie_PayFee = new HttpCookie("PayFeeFormCookieSMA");
        //    myCookie_PayFee["DataVal"] = "0";
        //    myCookie_PayFee.Expires = DateTime.Now.AddDays(1);
        //    Response.Cookies.Add(myCookie_PayFee);

        //    return Json("1", JsonRequestBehavior.AllowGet);
        //}

        //public JsonResult GetPayFeeFormCookieDetail()
        //{
        //    string _val = "";
        //    HttpCookie myCookie = Request.Cookies["PayFeeFormCookieSMA"];

        //    if (myCookie != null)
        //    {
        //        _val = myCookie["DataVal"].ToString();
        //    }

        //    //Remove Pay-Fee-Form cookie detail
        //    HttpCookie myCookie2 = new HttpCookie("PayFeeFormCookieSMA");
        //    myCookie2.Expires = DateTime.Now.AddDays(-1);
        //    Response.Cookies.Add(myCookie2);

        //    return Json(_val, JsonRequestBehavior.AllowGet);
        //}

        //public JsonResult SetReJoinCourseCookie(Int64 payFeeID)
        //{
        //    //Remove Re-Join Course cookie detail
        //    HttpCookie myCookie = new HttpCookie("PayFeeFormCookieSMA");
        //    myCookie.Expires = DateTime.Now.AddDays(-1);
        //    Response.Cookies.Add(myCookie);

        //    HttpCookie myCookie_PayFee = new HttpCookie("PayFeeFormCookieSMA");
        //    myCookie_PayFee["DataVal"] = payFeeID.ToString(); // for Re-Join Course
        //    myCookie_PayFee.Expires = DateTime.Now.AddDays(1);
        //    Response.Cookies.Add(myCookie_PayFee);

        //    return Json("1", JsonRequestBehavior.AllowGet);
        //}

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

        public bool ValidateAdmin()
        {
            bool _isValid = false;
            HttpCookie myCookie = Request.Cookies["AdminCookieSMA"];

            if (myCookie != null)
            {
                _isValid = true;
            }

            return _isValid;
        }

        // GET: Admin    --------------------------------------------------------------------------
        public ActionResult Index()
        {
            bool _ValidateStatus = ValidateAdmin();

            if (_ValidateStatus == true)
            {
                SetSidebarCookieInfo("manageDashboardAdmin");
                return View();
            }
            else
            {
                return RedirectToAction("Index", "Login");
            }
        }
        public ActionResult ManageStaff()
        {
            bool _ValidateStatus = ValidateAdmin();

            if (_ValidateStatus == true)
            {
                SetSidebarCookieInfo("manageStaffAdmin");
                return View();
            }
            else
            {
                return RedirectToAction("Index", "Login");
            }
        }

        public ActionResult ManageClass()
        {
            bool _ValidateStatus = ValidateAdmin();

            if (_ValidateStatus == true)
            {
                SetSidebarCookieInfo("manageClassAdmin");
                return View();
            }
            else
            {
                return RedirectToAction("Index", "Login");
            }
        }
        public ActionResult ManageSession()
        {
            bool _ValidateStatus = ValidateAdmin();

            if (_ValidateStatus == true)
            {
                SetSidebarCookieInfo("manageSessionAdmin");
                return View();
            }
            else
            {
                return RedirectToAction("Index", "Login");
            }
        }
        public ActionResult ManageSessionFee()
        {
            bool _ValidateStatus = ValidateAdmin();

            if (_ValidateStatus == true)
            {
                SetSidebarCookieInfo("manageSessionFeeAdmin");
                return View();
            }
            else
            {
                return RedirectToAction("Index", "Login");
            }
        }

        public ActionResult ManageSection()
        {
            bool _ValidateStatus = ValidateAdmin();

            if (_ValidateStatus == true)
            {
                SetSidebarCookieInfo("manageSectionAdmin");
                return View();
            }
            else
            {
                return RedirectToAction("Index", "Login");
            }
        }

        public ActionResult ManageStudent()
        {
            bool _ValidateStatus = ValidateAdmin();

            if (_ValidateStatus == true)
            {
                SetSidebarCookieInfo("manageStudentAdmin");
                return View();
            }
            else
            {
                return RedirectToAction("Index", "Login");
            }
        }
        public ActionResult StudentDetail()
        {
            bool _ValidateStatus = ValidateAdmin();

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
            bool _ValidateStatus = ValidateAdmin();

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
            bool _ValidateStatus = ValidateAdmin();

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
            bool _ValidateStatus = ValidateAdmin();

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
            bool _ValidateStatus = ValidateAdmin();

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
            bool _ValidateStatus = ValidateAdmin();

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
            bool _ValidateStatus = ValidateAdmin();

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
            bool _ValidateStatus = ValidateAdmin();

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
            bool _ValidateStatus = ValidateAdmin();

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
            bool _ValidationStatus = ValidateAdmin();
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
            bool _ValidationStatus = ValidateAdmin();
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

        public ActionResult ConnectDevice()
        {
            bool _ValidateStatus = ValidateAdmin();

            if (_ValidateStatus == true)
            {
                SetSidebarCookieInfo("manageDeviceConfigurationAdmin");
                return View();
            }
            else
            {
                return RedirectToAction("Index", "Login");
            }
        }
    }
}