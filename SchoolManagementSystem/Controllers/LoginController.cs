using Newtonsoft.Json;
using SchoolManagementSystem.Common;
using SchoolManagementSystem.DAL;
using SchoolManagementSystem.ViewModel;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Globalization;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SchoolManagementSystem.Controllers
{
    public class LoginController : Controller
    {
        SchoolManagementContext db = new SchoolManagementContext();
        List<LoginViewModel> lstLogin = new List<LoginViewModel>();

        // GET: Login
        public ActionResult Index()
         {
         
           if (Request.Cookies["SuperAdminCookieSMA"] != null)
            {
                return RedirectToAction("Index", "SuperAdmin");
            }
            else if (Request.Cookies["AdminCookieSMA"] != null)
            {
                return RedirectToAction("Index", "Admin");
            }
            else if (Request.Cookies["StaffCookieSMA"] != null)
            {
                return RedirectToAction("Index", "Staff");
            }
            else
            {
                ViewBag.AppName = ConfigurationManager.AppSettings["First_Word_AppName"] + " " + ConfigurationManager.AppSettings["Second_Word_AppName"];
                return View();
            }
        }

        [HttpPost]
        public ActionResult Index(string txtEmail, string txtPassword)
        {
            try
            {
                //--Get Plan-Expiration
                DateTime Plan_ExpiryDate = DateTime.ParseExact(ConfigurationManager.AppSettings["Plan_ExpiryDate"], "d/M/yyyy", CultureInfo.InvariantCulture);
                //--Get Current India-DateTime
                DateTime Current_DateTime = DateTime.UtcNow.AddMinutes(330).Date;

                //--if plan has validity
                if (Current_DateTime <= Plan_ExpiryDate)
                {
                    JWTClass objJWTClass = new JWTClass();
                    string str = EDClass.Encrypt(txtPassword);
                    ViewBag.ReturnVal = null;
                    SqlParameter[] queryParams = new SqlParameter[] {
                    new SqlParameter("id", "0"),
                    new SqlParameter("email", txtEmail),
                    new SqlParameter("password", EDClass.Encrypt(txtPassword)),
                    new SqlParameter("mode", "1")
                    };

                    lstLogin = db.Database.SqlQuery<LoginViewModel>("exec sp_Login @id,@email,@password,@mode", queryParams).ToList();
                    if (lstLogin.Count > 0)
                    {
                        //Remove SuperAdmin login cookie
                        HttpCookie myCookieSuperAdminn = new HttpCookie("SuperAdminCookieSMA");
                        myCookieSuperAdminn.Expires = DateTime.Now.AddDays(-1);
                        Response.Cookies.Add(myCookieSuperAdminn);

                        //Remove Admin login cookie
                        HttpCookie myCookieAdminn = new HttpCookie("AdminCookieSMA");
                        myCookieAdminn.Expires = DateTime.Now.AddDays(-1);
                        Response.Cookies.Add(myCookieAdminn);
                              
                        //Remove Staff login cookie
                        HttpCookie myCookieStaff = new HttpCookie("StaffCookieSMA");
                        myCookieStaff.Expires = DateTime.Now.AddDays(-1);
                        Response.Cookies.Add(myCookieStaff);

                        //--if SuperAdmin login
                        if (lstLogin[0].UserTypeId == 1)
                        {
                            //--Generate and get the JWT Token
                            string _JWT_User_Token = objJWTClass.Create_JWT(lstLogin[0].Id, "SuperAdmin");

                            //Create SuperAdmin login cookie
                            HttpCookie myCookie = new HttpCookie("SuperAdminCookieSMA");
                            myCookie["UserToken"] = _JWT_User_Token;
                            myCookie.Expires = DateTime.Now.AddDays(1);
                            Response.Cookies.Add(myCookie);

                            return RedirectToAction("Index", "SuperAdmin");
                                                        
                        }
                        //--if Admin login
                        else if (lstLogin[0].UserTypeId == 2)
                        {
                            //--Generate and get the JWT Token
                            string _JWT_User_Token = objJWTClass.Create_JWT(lstLogin[0].Id, "Admin");

                            //Create Admin login cookie
                            HttpCookie myCookie = new HttpCookie("AdminCookieSMA");
                            myCookie["UserToken"] = _JWT_User_Token;
                            myCookie.Expires = DateTime.Now.AddDays(1);
                            Response.Cookies.Add(myCookie);

                            return RedirectToAction("Index", "Admin");

                            
                        }
                        //--if Staff login
                        else if (lstLogin[0].UserTypeId == 3)
                        {
                            //--Generate and get the JWT Token
                            string _JWT_User_Token = objJWTClass.Create_JWT(lstLogin[0].Id, "Staff");

                            //Create Staff login cookie
                            HttpCookie myCookie = new HttpCookie("StaffCookieSMA");
                            myCookie["UserToken"] = _JWT_User_Token;
                            myCookie.Expires = DateTime.Now.AddDays(1);
                            Response.Cookies.Add(myCookie);

                            return RedirectToAction("Index", "Staff");
                                                        
                        }
                        else
                        {
                            Session["SessionLoginCheck"] = "0";
                            ModelState.Clear();
                            return RedirectToAction("Index", "Login");
                        }
                    }
                    else
                    {
                        Session["SessionLoginCheck"] = "0";
                        ModelState.Clear();
                        return RedirectToAction("Index", "Login");
                    }
                }
                else
                {
                    Session["SessionLoginCheck"] = "-1";
                    ModelState.Clear();
                    return RedirectToAction("Index", "Login");
                }
            }
            catch (Exception ex)
            {
                Session["SessionLoginCheck"] = "0";
                ModelState.Clear();
                return RedirectToAction("Index", "Login");
            }
        }

        public JsonResult LogoutUser()
        {
    //Remove SuperAdmin login cookie
                        HttpCookie myCookieSuperAdminn = new HttpCookie("SuperAdminCookieSMA");
                        myCookieSuperAdminn.Expires = DateTime.Now.AddDays(-1);
                        Response.Cookies.Add(myCookieSuperAdminn);

            //Remove Admin login cookie
            HttpCookie myCookieAdminn = new HttpCookie("AdminCookieSMA");
            myCookieAdminn.Expires = DateTime.Now.AddDays(-1);
            Response.Cookies.Add(myCookieAdminn);

            //Remove staff login cookie
            HttpCookie myCookieStaff = new HttpCookie("StaffCookieSMA");
            myCookieStaff.Expires = DateTime.Now.AddDays(-1);
            Response.Cookies.Add(myCookieStaff);

            //Remove Sidebar cookie detail
            HttpCookie myCookieSidebar = new HttpCookie("SidebarCookieSMA");
            myCookieSidebar.Expires = DateTime.Now.AddDays(-1);
            Response.Cookies.Add(myCookieSidebar);

            //--Return
            return Json("1", JsonRequestBehavior.AllowGet);
        }

        public ActionResult ForgotPassword()
        {
            if (Request.Cookies["SuperAdminCookieSMA"] != null)
            {
                return RedirectToAction("Dashboard", "SuperAdmin");
            }
            else if (Request.Cookies["AdminCookieSMA"] != null)
            {
                return RedirectToAction("Dashboard", "Admin");
            }
            else if (Request.Cookies["StaffCookieSMA"] != null)
            {
                return RedirectToAction("ManageStudent", "Staff");
            }
            else
            {
                return View();
            }
        }

        //--Forgot-Password Admin--    // This API will send the Forgot-Password Link to the Admin Email Address
        [HttpPost]                          // POST API with JSON Parameter
        public JsonResult ForgotPassword(string email)
        {
            try
            {
                //--Create object of Resopnse class
                JsonResponseViewModel objResponse = null;
                //--Check if model state is valid or not
                if (String.IsNullOrEmpty(email))
                {
                    //--Create Invalid Return-Resonse--
                    objResponse = new JsonResponseViewModel() { status = -101, message = "Invalid Request!" };

                    //--Return response as bad request
                    return Json(objResponse, JsonRequestBehavior.AllowGet);
                }

                //UserLogin_VM objUser = (from ul in db.UserLogin
                //                      where ul.Email == email
                //                      select new UserLogin_VM { Id = ul.Id, LoginStatus = ul.LoginStatus, Email = ul.Email, Username = ul.Username, UserTypeId = ul.UserTypeId }
                //                         ).FirstOrDefault();
                LoginViewModel objUser = null;

                SqlParameter[] queryParams = new SqlParameter[] {
                new SqlParameter("id", "0"),
                new SqlParameter("email", email),
                new SqlParameter("password", ""),
                new SqlParameter("mode", "2")
                };

                lstLogin = db.Database.SqlQuery<LoginViewModel>("exec sp_Login @id,@email,@password,@mode", queryParams).ToList();
                if (lstLogin.Count > 0)
                {
                    objUser = lstLogin.First();
                }

                if (objUser != null)
                {
                    //--Double Check by checking its (Username) //--With Case Sensitive Check--
                    objUser = (objUser.Email == email) ? objUser : null;
                }

                //--Check if Username Not-Exists--
                if (objUser == null)
                {
                    //--Create "Username not exists" Response--
                    objResponse = new JsonResponseViewModel() { status = -1, message = "Sorry, Email does not exist!" };

                    //--Return response as OK with "Username not exists"
                    return Json(objResponse, JsonRequestBehavior.AllowGet);
                }
                else if (objUser != null && objUser.LoginStatus == 0)
                {
                    //--Create "Username not exists" Response--
                    objResponse = new JsonResponseViewModel() { status = -1, message = "Your account is inactive. Contact your admin for activation." };

                    //--Return response as OK with "Username not exists"
                    return Json(objResponse, JsonRequestBehavior.AllowGet);
                }
                else
                {


                    #region Send Forgot-Password Email to the Admin

                    ResetPasswordToken_VM resetPasswordToken_VM = new ResetPasswordToken_VM
                    {
                        UserId = objUser.Id,
                        ValidTill_UTCDateTime = DateTime.UtcNow.AddHours(1)
                    };
                    string serializedToken = JsonConvert.SerializeObject(resetPasswordToken_VM).ToString();
                    //--Encrypt the Admin-ID
                    string Encrypted_Reset_Token = EDClass.Encrypt(serializedToken);
                    //--Encode Encrypted-Admin-ID according to the URL Encoding
                    //string url_encoded_AdminID = HttpContext.Current.Server.UrlEncode(Encrypted_Admin_Id);
                    string url_encoded_token = HttpUtility.UrlEncode(Encrypted_Reset_Token);

                    string URL = ConfigurationManager.AppSettings["SiteURL"] + "/Login/ResetPassword?token=" + url_encoded_token;
                    string Receiver_Name = ""; //"Admin / Student / Staff";
                    string Receiver_Email = objUser.Email;
                    string Subject = "Forgot Password";
                    string Message = @"<table>
                                    <tr>
                                        <td style='padding-bottom:20px;'>
                                            <h2>Reset Password</h2>
                                            Please click on the below link to reset your password.
                                        </td>
                                    </tr>
                                    <tr>
                                        <td style='text-align:center;'>
                                            <a href='" + URL + @"' style='padding:10px 20px;background-color:green;color:white;text-decoration:none;'>Click Here</a>
                                        </td>
                                    </tr>
                                </table>";



                    // TODOC: ResetPassword email link Comment This testing code and enable sending email
                    //------------------------------------- TESTING -------------------------------
                    //--Create response as Successfully Sent Email
                    objResponse = new JsonResponseViewModel() { status = 1, message = "Forgot-Password email has been successfully sent to your email address, please check!", data = new { token = URL } };

                    //sending response as OK
                    return Json(objResponse, JsonRequestBehavior.AllowGet);
                    //------------------------------------- TESTING ------------------------------- Returns back here


                    SendEmail objEmail = new SendEmail();
                     objEmail.Send(Receiver_Name, Subject, Receiver_Email, Message, "");
                    #endregion

                    //--Create response as Successfully Sent Email
                    objResponse = new JsonResponseViewModel() { status = 1, message = "Forgot-Password email has been successfully sent to your email address, please check!" };

                    //sending response as OK
                    return Json(objResponse, JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception ex)
            {
                //#region Insert Erorr-Info
                //var IsInserted = ErrorsLogClass.SaveError("InsertUser", ex.ToString());
                //#endregion

                //--Create response as Error
                JsonResponseViewModel objResponse = new JsonResponseViewModel() { status = -100, message = "Internal Server Error!" };
                //sending response as error
                //return Json(objResponse);
                return Json(objResponse, JsonRequestBehavior.AllowGet);
            }
        }

        public ActionResult ResetPassword(string token)
        {
            try
            {
                if (String.IsNullOrEmpty(token))
                {
                    ViewBag.ErrorMessage = "Invalid Link!";
                    return View();
                }
                ResetPasswordViewModel resetPasswordViewModel = new ResetPasswordViewModel();
                DateTime UTC_DateTime = DateTime.UtcNow;
                //DateTime UTC_DateTime = DateTime.UtcNow.AddHours(1);   // For Testing Token Expiry, uncomment this.
                string decryptedToken = EDClass.Decrypt(token);
                ResetPasswordToken_VM resetPasswordToken_VM = JsonConvert.DeserializeObject<ResetPasswordToken_VM>(decryptedToken);
                bool is_Valid = true;

                var dateTime_diff = resetPasswordToken_VM.ValidTill_UTCDateTime - UTC_DateTime;
                //validate date by having same date and hours difference.
                // Total Hours will be negative if exceeding validTill-DateTime-Hours.
                if (Convert.ToInt32(dateTime_diff.TotalDays) == 0 && dateTime_diff.TotalHours >= 0)
                {
                    // token valid
                    is_Valid = true;
                }
                else
                {
                    is_Valid = false;
                    ViewBag.ErrorMessage = "Link has been expired!";
                }
                ViewBag.IsTokenValid = is_Valid;
                resetPasswordViewModel.Token = token;
                return View(resetPasswordViewModel);
            }
            catch (Exception ex)
            {
                ViewBag.ErrorMessage = "Internal Server Error!";
                return View();
            }
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public ActionResult ResetPassword(ResetPasswordViewModel resetPasswordViewModel)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    ViewBag.IsValid = false;
                    ViewBag.ErrorMessage("All fields are required!");
                    return View(resetPasswordViewModel);
                }

                #region Decrypt Reset-Password Token & Check Token validity

                DateTime UTC_DateTime = DateTime.UtcNow;
                //DateTime UTC_DateTime = DateTime.UtcNow.AddHours(1);   // For Testing Token Expiry, uncomment this.
                string decryptedToken = EDClass.Decrypt(resetPasswordViewModel.Token);
                ResetPasswordToken_VM resetPasswordToken_VM = JsonConvert.DeserializeObject<ResetPasswordToken_VM>(decryptedToken);
                bool is_Valid = true;

                var dateTime_diff = resetPasswordToken_VM.ValidTill_UTCDateTime - UTC_DateTime;
                //validate date by having same date and hours difference.
                // Difference of Total Hours will be negative if exceeding validTill-DateTime-Hours.
                if (Convert.ToInt32(dateTime_diff.TotalDays) == 0 && dateTime_diff.TotalHours >= 0)
                {
                    // token valid
                    is_Valid = true;
                }
                else
                {
                    is_Valid = false;
                    ViewBag.ErrorMessage = "Link has been expired!";
                    ViewBag.IsTokenValid = is_Valid;
                    return View(resetPasswordViewModel);
                }
                #endregion

                Int64 userId = resetPasswordToken_VM.UserId;

                #region LinQ code commented
                //UserLogin userLogin = db.UserLogin.FirstOrDefault(u => u.Id == userId);

                //if (userLogin == null)
                //{
                //    ViewBag.IsValid = false;
                //    ViewBag.ErrorMessage("Invalid User!");
                //    return View(resetPasswordViewModel);
                //}

                //userLogin.Password = EDClass.Encrypt(resetPasswordViewModel.Password);
                //db.SaveChanges();
                #endregion

                SqlParameter[] queryParams = new SqlParameter[] {
                new SqlParameter("id", userId),
                new SqlParameter("email", ""),
                new SqlParameter("password", EDClass.Encrypt(resetPasswordViewModel.Password)),
                new SqlParameter("mode", "3")
                };

                ResponseViewModel resetResponse = db.Database.SqlQuery<ResponseViewModel>("exec sp_Login @id,@email,@password,@mode", queryParams).FirstOrDefault();

                if (resetResponse.ret == 2)
                {
                    ViewBag.IsResetSuccessful = true;
                    return View();
                }
                else
                {
                    ViewBag.IsValid = false;
                    ViewBag.ErrorMessage(resetResponse.responseMessage);
                    return View(resetPasswordViewModel);
                }

            }
            catch (Exception ex)
            {
                ViewBag.ErrorMessage = "Internal Server Error!";
                return View();
            }
        }

    }
}