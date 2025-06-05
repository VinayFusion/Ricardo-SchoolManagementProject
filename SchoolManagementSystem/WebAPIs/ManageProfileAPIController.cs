using SchoolManagementSystem.Common;
using SchoolManagementSystem.DAL;
using SchoolManagementSystem.Models;
using SchoolManagementSystem.ViewModel;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Security.Claims;
using System.Web;
using System.Web.Http;

namespace SchoolManagementSystem.WebAPIs
{
    public class ProfileAPIController : ApiController
    {

        private SchoolManagementContext db = new SchoolManagementContext();


        //--Get Admin Profile Detail by Token-- 
        [Authorize(Roles = "Admin")]
        [Route("GetAdminProfile")]
        [HttpGet]
        public HttpResponseMessage GetAdminProfileInfo()
        {
            try
            {
                //--Get User Identity
                var identity = User.Identity as ClaimsIdentity;

                //--Check if user is authorized user or not
                if (identity != null)
                {
                    IEnumerable<Claim> claims = identity.Claims;
                    string _LoginID = claims.Where(p => p.Type == "loginid").FirstOrDefault()?.Value;
                    Int64 _LoginID_Exact = 0;

                    if (_LoginID != "" && _LoginID != null)
                    {
                        _LoginID_Exact = Convert.ToInt64(_LoginID);
                    }

                    AdminViewModel lstAdmin = new AdminViewModel();

                    //--Get Staff-Profile-Detail by Login-ID
                    SqlParameter[] queryParams_Staff = new SqlParameter[] {
                    new SqlParameter("id", _LoginID_Exact),
                    new SqlParameter("mode", "2")
                    };
                    lstAdmin = db.Database.SqlQuery<AdminViewModel>("exec sp_ManageProfile_StaffAdmin @id,@mode", queryParams_Staff).FirstOrDefault();

                    if (lstAdmin != null)
                    {
                        //--Create response
                        var objResponse = new
                        {
                            status = 1,
                            message = "Success",
                            data = new
                            { Admin = lstAdmin }
                        };

                        //sending response as OK
                        return Request.CreateResponse(HttpStatusCode.OK, objResponse);
                    }
                    else
                    {
                        //--Create response as Un-Authorized
                        var objResponse = new { status = -101, message = "Authorization has been denied for this request!", data = "" };
                        //sending response as Un-Authorized
                        return Request.CreateResponse(HttpStatusCode.Unauthorized, objResponse);
                    }
                }
                else
                {
                    //--Create response as Un-Authorized
                    var objResponse = new { status = -101, message = "Authorization has been denied for this request!", data = "" };
                    //sending response as Un-Authorized
                    return Request.CreateResponse(HttpStatusCode.Unauthorized, objResponse);
                }
            }
            catch (Exception ex)
            {
                //--Create response as Error
                var objResponse = new { status = -100, message = "Internal Server Error!", data = "" };
                //sending response as error
                return Request.CreateResponse(HttpStatusCode.InternalServerError, objResponse);
            }
        }

        // --Get SuperAdmin , Admin And staff Detail by Id-- 

        [Authorize(Roles = "SuperAdmin,Admin,Staff")]
        [Route("GetProfileDataById")]
        [HttpGet]
        public HttpResponseMessage GetProfileDataById()
        {
            try
            {
                //--Get User Identity
                var identity = User.Identity as ClaimsIdentity;

                //--Check if user is authorized user or not
                if (identity != null)
                {

                    IEnumerable<Claim> claims = identity.Claims;
                    string _LoginID = claims.Where(p => p.Type == "loginid").FirstOrDefault()?.Value;
                    Int64 _LoginID_Exact = 0;

                    //AdminResponse_ViewModel _resp = new AdminResponse_ViewModel();

                    if (_LoginID != "" && _LoginID != null)
                    {
                        _LoginID_Exact = Convert.ToInt64(_LoginID);
                    }

                    //--Get User-Login Detail
                    UserLogin _UserLogin = db.UserLogin.Where(ul => ul.Id == _LoginID_Exact).FirstOrDefault();
                    var _AdminLogin = "";//db.Admin.Where(a => a.LoginId == _LoginID_Exact).FirstOrDefault();
                    var _SuperAdminLogin = "";
                    var _StaffLogin = db.Staff.Where(a => a.LoginId == _LoginID_Exact).FirstOrDefault();

                    //------------- for superadmin ---------------------
                    if (_SuperAdminLogin != null && _UserLogin.UserTypeId == 1)
                    {
                        SuperAdminViewModel lstSuperAdmin = new SuperAdminViewModel();

                        //--Get Detail by username
                        SqlParameter[] queryParams_Branch = new SqlParameter[] {
                    new SqlParameter("loginid", _LoginID_Exact),
                    new SqlParameter("mode", "4")
                    };
                        lstSuperAdmin = db.Database.SqlQuery<SuperAdminViewModel>("exec sp_ManageProfile_StaffAdmin @loginid,@mode", queryParams_Branch).FirstOrDefault();



                        //--Create response
                        var objResponse = new
                        {
                            status = 1,
                            message = "Success",
                            data = new
                            { SuperAdmin = lstSuperAdmin }
                        };
                        //sending response as OK

                        return Request.CreateResponse(HttpStatusCode.OK, objResponse);
                    }
                    //------------- for admin ---------------------
                    else if (_AdminLogin != null && _UserLogin.UserTypeId == 2)
                    {
                        AdminViewModel lstAdmin = new AdminViewModel();

                        //--Get Detail by username
                        SqlParameter[] queryParams_Branch = new SqlParameter[] {
                    new SqlParameter("loginid", _LoginID_Exact),
                    new SqlParameter("mode", "1")
                    };
                        lstAdmin = db.Database.SqlQuery<AdminViewModel>("exec sp_ManageProfile_StaffAdmin @loginid,@mode", queryParams_Branch).FirstOrDefault();



                        //--Create response
                        var objResponse = new
                        {
                            status = 1,
                            message = "Success",
                            data = new
                            { Admin = lstAdmin }
                        };
                        //sending response as OK

                        return Request.CreateResponse(HttpStatusCode.OK, objResponse);
                    }
                    //------------- for Staff ---------------------
                    else if (_StaffLogin != null && _UserLogin.UserTypeId == 3)
                    {
                        StaffViewModel lstStaff = new StaffViewModel();

                        //--Get Detail by username
                        SqlParameter[] queryParams_Branch = new SqlParameter[] {
                    new SqlParameter("loginid", _LoginID_Exact),
                    new SqlParameter("mode", "3")
                    };
                        lstStaff = db.Database.SqlQuery<StaffViewModel>("exec sp_ManageProfile_StaffAdmin @loginid,@mode", queryParams_Branch).FirstOrDefault();



                        //--Create response
                        var objResponse = new
                        {
                            status = 1,
                            message = "Success",
                            data = new
                            { Staff = lstStaff }
                        };
                        //sending response as OK
                        return Request.CreateResponse(HttpStatusCode.OK, objResponse);
                    }
                    else
                    {
                        //--Create response as Un-Authorized
                        var objResponse = new { status = -101, message = "Authorization has been denied for this request!", data = "" };
                        //sending response as Un-Authorized
                        return Request.CreateResponse(HttpStatusCode.Unauthorized, objResponse);
                    }

                }
                else
                {
                    //--Create response as Un-Authorized
                    var objResponse = new { status = -101, message = "Authorization has been denied for this request!", data = "" };
                    //sending response as Un-Authorized
                    return Request.CreateResponse(HttpStatusCode.Unauthorized, objResponse);
                }
            }

            catch (Exception ex)
            {
                //--Create response as Error
                var objResponse = new { status = -100, message = "Internal Server Error!", data = "" };
                //sending response as error
                return Request.CreateResponse(HttpStatusCode.InternalServerError, objResponse);
            }
        }

        //--Insert Update Admin Data and Staff Data -- 
        [Authorize(Roles = "SuperAdmin,Admin,Staff")]
        [Route("InsertUpdateProfileData")]
        [HttpPost]

        #region Edit By Dheeraj

        //public HttpResponseMessage InsertUpdateProfileData()
        //{
        //    try
        //    {
        //        //--Get User Identity
        //        var identity = User.Identity as ClaimsIdentity;

        //        //--Check if user is authorized user or not
        //        if (identity != null)
        //        {
        //            IEnumerable<Claim> claims = identity.Claims;
        //            string _LoginID = claims.Where(p => p.Type == "loginid").FirstOrDefault()?.Value;
        //            Int64 _LoginID_Exact = 0;

        //            if (_LoginID != "" && _LoginID != null)
        //            {
        //                _LoginID_Exact = Convert.ToInt64(_LoginID);
        //            }

        //            //--Get User-Login Detail
        //            UserLogin _UserLogin = db.UserLogin.Where(ul => ul.Id == _LoginID_Exact).FirstOrDefault();
        //            var _AdminLogin = db.Admin.Where(a => a.LoginId == _LoginID_Exact).FirstOrDefault();
        //            var _StaffLogin = db.Staff.Where(a => a.LoginId == _LoginID_Exact).FirstOrDefault();
        //            if (_UserLogin != null && (_AdminLogin != null || _StaffLogin != null))
        //            {
        //                var _status = 0;
        //                var _message = "";
        //                //--Create object of HttpRequest
        //                var HttpRequest = HttpContext.Current.Request;
        //                Int64 _Id = _LoginID_Exact;         
        //                string _FirstName = HttpRequest.Params["firstName"];
        //                string _LastName = HttpRequest.Params["lastName"];
        //                string _Email = HttpRequest.Params["email"];
        //                string _MobileNumber = HttpRequest.Params["mobile"];
        //                string _CountryMobilePhoneCode_Only = HttpRequest.Params["countryMobilePhoneCodeOnly"];
        //                string _MobileNumber_Only = HttpRequest.Params["mobileNumberOnly"];
        //                //string _username = HttpRequest.Params["username"];
        //                string _Pincode = HttpRequest.Params["pincode"];
        //                string _Address = HttpRequest.Params["address"];
        //                int _IsActive = Convert.ToInt32(HttpRequest.Params["isActive"]);
        //                Int64 _submittedByLoginId = _UserLogin.Id;
        //                int _mode = 0;
        //                if (_UserLogin.UserTypeId == 1)
        //                {
        //                    _mode = 1;
        //                }
        //                else if (_UserLogin.UserTypeId == 2)
        //                {
        //                    _mode = 2;
        //                }
        //                //--Insert All Detail
        //                SqlParameter[] queryParams_Staff = new SqlParameter[] {
        //                new SqlParameter("loginid", _Id),
        //                new SqlParameter("firstName", _FirstName),
        //                new SqlParameter("lastName", _LastName),
        //                new SqlParameter("email", _Email),
        //                new SqlParameter("mobile", _MobileNumber),
        //                new SqlParameter("countryMobilePhoneCode_only", _CountryMobilePhoneCode_Only),
        //                new SqlParameter("mobileNumber_only", _MobileNumber_Only),
        //                new SqlParameter("pincode", _Pincode),
        //                new SqlParameter("address", _Address),
        //                new SqlParameter("isActive", _IsActive),
        //                new SqlParameter("submittedByLoginId", _submittedByLoginId),
        //                new SqlParameter("mode", _mode)
        //                };

        //                //---------------------------------------------------------------------Admin Start--------------------------------------------------------------------//
        //                if (_AdminLogin != null && _UserLogin.UserTypeId == 1)
        //                {
        //                    var _resp = new AdminResponse_ViewModel();

        //                    _resp = db.Database.SqlQuery<AdminResponse_ViewModel>("exec sp_InsertUpdateProfile_StaffAdmin @loginid,@firstName,@lastName,@email,@mobile,@countryMobilePhoneCode_only,@mobileNumber_only,@pincode,@address,@isActive,@submittedByLoginId,@mode", queryParams_Staff).FirstOrDefault();
        //                    _status = _resp.ret;
        //                    _message = _resp.responseMessage;
        //                }
        //                //------------------------------------------------------------------------Admin End--------------------------------------------------------------------//
        //                //------------------------------------------------------------------------staff  start--------------------------------------------------------------------//
        //                else if (_StaffLogin != null && _UserLogin.UserTypeId == 2)
        //                {
        //                    var _resp = new StaffResponse_ViewModel();

        //                    _resp = db.Database.SqlQuery<StaffResponse_ViewModel>("exec sp_InsertUpdateProfile_StaffAdmin @loginid,@firstName,@lastName,@email,@mobile,@countryMobilePhoneCode_only,@mobileNumber_only,@pincode,@address,@isActive,@submittedByLoginId,@mode", queryParams_Staff).FirstOrDefault();

        //                    _status = _resp.ret;
        //                    _message = _resp.responseMessage;

        //                }
        //                //------------------------------------------------------------------------staff  End--------------------------------------------------------------------//
        //                //--Create response
        //                var objResponse = new
        //                {
        //                    status = _status,
        //                    message = _message,
        //                };
        //                //sending response as OK
        //                return Request.CreateResponse(HttpStatusCode.OK, objResponse);

        //            }
        //            else
        //            {
        //                //--Create response as Un-Authorized
        //                var objResponse = new { status = -101, message = "Authorization has been denied for this request!", data = "" };
        //                //sending response as Un-Authorized
        //                return Request.CreateResponse(HttpStatusCode.Unauthorized, objResponse);
        //            }
        //        }

        //        else
        //        {
        //            //--Create response as Un-Authorized
        //            var objResponse = new { status = -101, message = "Authorization has been denied for this request!", data = "" };
        //            //sending response as Un-Authorized
        //            return Request.CreateResponse(HttpStatusCode.Unauthorized, objResponse);
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        //--Create response as Error
        //        var objResponse = new { status = -100, message = "Internal Server Error!", data = "" };
        //        //sending response as error
        //        return Request.CreateResponse(HttpStatusCode.InternalServerError, objResponse);
        //    }
        //}
        #endregion

        public HttpResponseMessage InsertUpdateProfileData()
        {
            try
            {
                //--Get User Identity
                var identity = User.Identity as ClaimsIdentity;

                //--Check if user is authorized user or not
                if (identity != null)
                {
                    IEnumerable<Claim> claims = identity.Claims;
                    string _LoginID = claims.Where(p => p.Type == "loginid").FirstOrDefault()?.Value;
                    Int64 _LoginID_Exact = 0;

                    if (_LoginID != "" && _LoginID != null)
                    {
                        _LoginID_Exact = Convert.ToInt64(_LoginID);
                    }

                    //--Get User-Login Detail
                    UserLogin _UserLogin = db.UserLogin.Where(ul => ul.Id == _LoginID_Exact).FirstOrDefault();
                    Int64 Staff_Admin_Id = 0;
                    //For SuperAdmin
                    if (_UserLogin.UserTypeId == 1)
                    {
                        var _SuperAdminLogin = db.SuperAdmin.Where(a => a.LoginId == _LoginID_Exact).FirstOrDefault();
                        Staff_Admin_Id = _SuperAdminLogin.Id;
                    }
                    //For Admin 
                    else if (_UserLogin.UserTypeId == 2)
                    {
                        var _AdminLogin = db.Admin.Where(a => a.LoginId == _LoginID_Exact).FirstOrDefault();
                        Staff_Admin_Id = _AdminLogin.Id;
                    }
                    //For Staff
                    else if (_UserLogin.UserTypeId == 3)
                    {
                        var _StaffLogin = db.Staff.Where(a => a.LoginId == _LoginID_Exact).FirstOrDefault();
                        Staff_Admin_Id = _StaffLogin.Id;
                    }
                    if (_UserLogin != null && Staff_Admin_Id != 0)
                    {
                        //--Create object of HttpRequest
                        var HttpRequest = HttpContext.Current.Request;
                        Int64 _Id = Staff_Admin_Id;
                        string _FirstName = HttpRequest.Params["firstName"];
                        string _LastName = HttpRequest.Params["lastName"];
                        string _Email = HttpRequest.Params["email"];
                        string _MobileNumber = HttpRequest.Params["mobile"];
                        string _CountryMobilePhoneCode_Only = HttpRequest.Params["countryMobilePhoneCodeOnly"];
                        string _MobileNumber_Only = HttpRequest.Params["mobileNumberOnly"];
                        //string _username = HttpRequest.Params["username"];
                        string _Pincode = HttpRequest.Params["pincode"];
                        string _Address = HttpRequest.Params["address"];
                        int _IsActive = Convert.ToInt32(HttpRequest.Params["isActive"]);
                        Int64 _submittedByLoginId = _UserLogin.Id;
                        int _mode = 0;
                        if (_UserLogin.UserTypeId == 1)
                        {
                            _mode = 3;
                        }
                        else if (_UserLogin.UserTypeId == 2)
                        {
                            _mode = 1;
                        }
                        else if (_UserLogin.UserTypeId == 3)
                        {
                            _mode = 2;
                        }
                        var _resp = new ResponseViewModel();
                        //--Insert All Detail
                        SqlParameter[] queryParams_Staff = new SqlParameter[] {
                        new SqlParameter("id", _Id),
                        new SqlParameter("firstName", _FirstName),
                        new SqlParameter("lastName", _LastName),
                        new SqlParameter("email", _Email),
                        new SqlParameter("mobile", _MobileNumber),
                        new SqlParameter("countryMobilePhoneCode_only", _CountryMobilePhoneCode_Only),
                        new SqlParameter("mobileNumber_only", _MobileNumber_Only),
                        new SqlParameter("pincode", _Pincode),
                        new SqlParameter("address", _Address),
                        new SqlParameter("isActive", _IsActive),
                        new SqlParameter("submittedByLoginId", _submittedByLoginId),
                        new SqlParameter("mode", _mode)
                        };

                        _resp = db.Database.SqlQuery<ResponseViewModel>("exec sp_InsertUpdateProfile_StaffAdmin @id,@firstName,@lastName,@email,@mobile,@countryMobilePhoneCode_only,@mobileNumber_only,@pincode,@address,@isActive,@submittedByLoginId,@mode", queryParams_Staff).FirstOrDefault();

                        //--Create response
                        var objResponse = new
                        {
                            status = _resp.ret,
                            message = _resp.responseMessage,
                        };
                        //sending response as OK
                        return Request.CreateResponse(HttpStatusCode.OK, objResponse);

                    }
                    else
                    {
                        //--Create response as Un-Authorized
                        var objResponse = new { status = -101, message = "Authorization has been denied for this request!", data = "" };
                        //sending response as Un-Authorized
                        return Request.CreateResponse(HttpStatusCode.Unauthorized, objResponse);
                    }
                }

                else
                {
                    //--Create response as Un-Authorized
                    var objResponse = new { status = -101, message = "Authorization has been denied for this request!", data = "" };
                    //sending response as Un-Authorized
                    return Request.CreateResponse(HttpStatusCode.Unauthorized, objResponse);
                }
            }
            catch (Exception ex)
            {
                //--Create response as Error
                var objResponse = new { status = -100, message = "Internal Server Error!", data = "" };
                //sending response as error
                return Request.CreateResponse(HttpStatusCode.InternalServerError, objResponse);
            }
        }

        //--Insert Update SuperAdmin Profile Image , Admin Profile Image and Staff Profile Image -- 
        [Authorize(Roles = "SuperAdmin,Admin,Staff")]
        [Route("ProfileImageSet")]
        [HttpPost]
        public HttpResponseMessage ProfileImageSet()
        {
            try
            {
                //--Get User Identity
                var identity = User.Identity as ClaimsIdentity;

                //--Check if user is authorized user or not
                if (identity != null)
                {
                    IEnumerable<Claim> claims = identity.Claims;
                    string _LoginID = claims.Where(p => p.Type == "loginid").FirstOrDefault()?.Value;
                    Int64 _LoginID_Exact = 0;



                    if (_LoginID != "" && _LoginID != null)
                    {
                        _LoginID_Exact = Convert.ToInt64(_LoginID);
                    }


                    UserLogin _UserLogin = db.UserLogin.Where(ul => ul.Id == _LoginID_Exact).FirstOrDefault();
                    if (_UserLogin.UserTypeId == 1 || _UserLogin.UserTypeId == 2 || _UserLogin.UserTypeId == 3)
                    {
                        var _resp = new ResponseViewModel();
                        //--Create object of HttpRequest
                        var HttpRequest = HttpContext.Current.Request;
                        var _TypeProfileImage = HttpRequest.Params["TypeProfileImage"];
                        var _FirstName = HttpRequest.Params["_FirstName"];
                        string _ImageName = _TypeProfileImage == "NewImage" ? HttpRequest.Files.AllKeys[0] : Guid.NewGuid().ToString().Replace("-", "") + ".gif";
                        HttpPostedFileBase file = null;
                        string ImagePath = "";
                        string StoreImagePath = "";
                        Image resetImage = null;
                        var InputFileName = "";


                        //-------------------------------------------------------Image create  start------------------------------------------------------------//
                        if (_TypeProfileImage == "NewImage")
                        {
                            int numUploadImage = HttpRequest.Files.AllKeys.Length;
                            if (numUploadImage > 0)
                            {
                                #region Save uploaded Profile-Image to the Application Folder 
                                file = new HttpPostedFileWrapper(HttpRequest.Files[_ImageName]);
                                //Checking file is available to save.  
                                if (file != null)
                                {
                                    InputFileName = Path.GetFileName(file.FileName);
                                    InputFileName = Guid.NewGuid().ToString().Replace("-", "") + '_' + InputFileName;
                                }
                                #endregion
                            }
                        }
                        else if (_TypeProfileImage == "ResetImage")
                        {
                            #region Save FirstCharacter of Name Profile-Image to the Application Folder
                            string _FirstCharacter_AdminName = _FirstName.Substring(0, 1).ToUpper();
                            AvtarClass objAvtar = new AvtarClass();
                            Font font = new Font(FontFamily.GenericSerif, 45, FontStyle.Bold);
                            Color fontcolor = ColorTranslator.FromHtml("#FFF");
                            Color bgcolor = ColorTranslator.FromHtml("#83B869");
                            resetImage = objAvtar.GenerateAvtarImage(_FirstCharacter_AdminName, font, fontcolor, bgcolor);

                            #endregion
                        }
                        //------------------------------------------------------------------Image Create End-----------------------------------------------------//

                        //---------------------------- Defined mode admin or staff and also Defined the path For save image --------------------------------------//
                        var _mode = 0;
                        if (_UserLogin.UserTypeId == 1)
                        {
                            _mode = 4;
                            if (_TypeProfileImage == "NewImage")
                            {
                                StoreImagePath = Path.Combine(HttpContext.Current.Server.MapPath("/Content/SuperAdminImages/") + InputFileName);
                                ImagePath = ("/Content/SuperAdminImages/") + InputFileName;
                                _ImageName = InputFileName;
                            }
                            else if (_TypeProfileImage == "ResetImage")
                            {
                                StoreImagePath = (HttpContext.Current.Server.MapPath("/Content/SuperAdminImages/" + _ImageName));
                                ImagePath = ("/Content/SuperAdminImages/") + _ImageName;
                            }

                        }
                        else if (_UserLogin.UserTypeId == 2)
                        {
                            _mode = 1;
                            if (_TypeProfileImage == "NewImage")
                            {
                                StoreImagePath = Path.Combine(HttpContext.Current.Server.MapPath("/Content/AdminImages/") + InputFileName);
                                ImagePath = ("/Content/AdminImages/") + InputFileName;
                                _ImageName = InputFileName;
                            }
                            else if (_TypeProfileImage == "ResetImage")
                            {
                                StoreImagePath = (HttpContext.Current.Server.MapPath("/Content/AdminImages/" + _ImageName));
                                ImagePath = ("/Content/AdminImages/") + _ImageName;
                            }

                        }
                        else if (_UserLogin.UserTypeId == 3)
                        {
                            _mode = 2;
                            if (_TypeProfileImage == "NewImage")
                            {
                                StoreImagePath = Path.Combine(HttpContext.Current.Server.MapPath("/Content/StaffImages/") + InputFileName);
                                ImagePath = ("/Content/StaffImages/") + InputFileName;
                                _ImageName = InputFileName;
                            }
                            else if (_TypeProfileImage == "ResetImage")
                            {
                                StoreImagePath = (HttpContext.Current.Server.MapPath("/Content/StaffImages/" + _ImageName));
                                ImagePath = ("/Content/StaffImages/") + _ImageName;
                            }
                        }
                        //------------------------------------ Image name only save in data base ----------------------------------------------//
                        Int64 _Id = _LoginID_Exact;
                        SqlParameter[] queryParams_Admin = new SqlParameter[] {
                      new SqlParameter("id", _Id),
                      new SqlParameter("profileImage", _ImageName),
                      new SqlParameter("mode", _mode)
                      };
                        _resp = db.Database.SqlQuery<ResponseViewModel>("exec sp_InsertUpdateProfileImage @id,@profileImage,@mode", queryParams_Admin).FirstOrDefault();

                        //-----------------------------------------------Image save in defined path ------------------------------------------------------//
                        if (_TypeProfileImage == "NewImage")
                        {
                            file.SaveAs(StoreImagePath);
                        }
                        else if (_TypeProfileImage == "ResetImage")
                        {
                            resetImage.Save(StoreImagePath);
                        }

                        //--During Update-Mode only (Remove the Previous Image from Application Folder)
                        if (_resp.ret == 2)
                        {
                            #region Remove Previous Profile-Image from the Application Folder
                            var filePath = "";
                            if (_UserLogin.UserTypeId == 1)
                            {
                                filePath = HttpContext.Current.Server.MapPath("/Content/SuperAdminImages/" + _resp.PreviousProfileImage);
                            }
                            else if (_UserLogin.UserTypeId == 2)
                            {
                                filePath = HttpContext.Current.Server.MapPath("/Content/AdminImages/" + _resp.PreviousProfileImage);
                            }
                            else if (_UserLogin.UserTypeId == 3)
                            {
                                filePath = HttpContext.Current.Server.MapPath("/Content/StaffImages/" + _resp.PreviousProfileImage);
                            }


                            if (System.IO.File.Exists(filePath))
                            {
                                System.IO.File.Delete(filePath);
                            }
                            #endregion
                        }
                        //--Create response
                        var objResponse = new
                        {
                            status = 1,
                            message = _resp.responseMessage,
                            data = new
                            { imagePath = ImagePath, usertype = _UserLogin.UserTypeId }
                        };
                        //sending response as OK
                        return Request.CreateResponse(HttpStatusCode.OK, objResponse);
                    }
                    else
                    {
                        //--Create response as Un-Authorized
                        var objResponse = new { status = -101, message = "Authorization has been denied for this request!", data = "" };
                        //sending response as Un-Authorized
                        return Request.CreateResponse(HttpStatusCode.Unauthorized, objResponse);
                    }
                }
                else
                {
                    //--Create response as Un-Authorized
                    var objResponse = new { status = -101, message = "Authorization has been denied for this request!", data = "" };
                    //sending response as Un-Authorized
                    return Request.CreateResponse(HttpStatusCode.Unauthorized, objResponse);
                }

            }
            catch (Exception ex)
            {
                //--Create response as Error
                var objResponse = new { status = -100, message = "Internal Server Error!", data = "" };
                //sending response as error
                return Request.CreateResponse(HttpStatusCode.InternalServerError, objResponse);
            }
        }

    }
}