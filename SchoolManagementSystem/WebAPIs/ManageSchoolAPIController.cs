using SchoolManagementSystem.Common;
using SchoolManagementSystem.DAL;
using SchoolManagementSystem.Models;
using SchoolManagementSystem.ViewModel;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Security.Claims;
using System.Web;
using System.Web.Http;

namespace SchoolManagementSystem.WebAPIs
{
    public class ManageSchoolAPIController : ApiController
    {
        private SchoolManagementContext db = new SchoolManagementContext();

        //--Get All Schools-List-- 
        [Authorize(Roles = "SuperAdmin")]
        [Route("GetAllSchool")]
        [HttpGet]
        public HttpResponseMessage GetAllSchoolData()
        {
            try
            {
                //--Get User Identity
                var identity = User.Identity as ClaimsIdentity;

                //--Check if user is authorized user or not
                if (identity != null)
                {
                    List<SchoolViewModel> lstSchool = new List<SchoolViewModel>();

                    //--Get All School-List
                    SqlParameter[] queryParams_School = new SqlParameter[] {
                    new SqlParameter("id", "0"),
                    new SqlParameter("mode", "1")
                    };
                    lstSchool = db.Database.SqlQuery<SchoolViewModel>("exec sp_ManageSchool @id,@mode", queryParams_School).ToList();

                    //--Create response
                    var objResponse = new
                    {
                        status = 1,
                        message = "Success",
                        data = new
                        { school = lstSchool }
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
            catch (Exception ex)
            {
                //--Create response as Error
                var objResponse = new { status = -100, message = "Internal Server Error!", data = "" };
                //sending response as error
                return Request.CreateResponse(HttpStatusCode.InternalServerError, objResponse);
            }
        }

        //--Add New School-- 
        [Authorize(Roles = "SuperAdmin")]
        [Route("InsertUpdateSchool")]
        [HttpPost]
        public HttpResponseMessage InsertUpdateSchoolData()
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

                    ResponseViewModel _resp = new ResponseViewModel();

                    if (_LoginID != "" && _LoginID != null)
                    {
                        _LoginID_Exact = Convert.ToInt64(_LoginID);
                    }

                    //--Get User-Login Detail
                    UserLogin _UserLogin = db.UserLogin.Where(ul => ul.Id == _LoginID_Exact).FirstOrDefault();

                    if (_UserLogin != null)
                    {
                        //--Create object of HttpRequest
                        var HttpRequest = HttpContext.Current.Request;

                        //--Get all parameter's value of Form-Data (by Key-Name)
                        Int64 _Id = Convert.ToInt64(HttpRequest.Params["Id"]);
                        string _SchoolName = HttpRequest.Params["schoolName"];
                        Int64 _SchoolTypeId = Convert.ToInt64(HttpRequest.Params["schoolTypeId"]);
                        string _Email = HttpRequest.Params["email"];
                        string _MobileNumber = HttpRequest.Params["mobile"];
                        string _CountryCodeOnly = HttpRequest.Params["countryMobilePhoneCodeOnly"];
                        string _MobileNumberOnly = HttpRequest.Params["mobileNumberOnly"];
                        string _Password = HttpRequest.Params["password"];
                        string _Pincode = HttpRequest.Params["pincode"];
                        string _Address = HttpRequest.Params["address"];
                        int _Status = Convert.ToInt32(HttpRequest.Params["status"]); // Used as isActive
                        int _Mode = Convert.ToInt32(HttpRequest.Params["mode"]);


                        string _ImageName = Guid.NewGuid().ToString().Replace("-", "") + ".gif";

                        //--Insert New School Detail
                        SqlParameter[] queryParams_School = new SqlParameter[] {
                        new SqlParameter("@id", _Id),
                        new SqlParameter("@schoolName", _SchoolName),
                        new SqlParameter("@schoolTypeId", _SchoolTypeId),
                        new SqlParameter("@email", _Email),
                        new SqlParameter("@mobile", _MobileNumber),
                        new SqlParameter("@countryMobilePhoneCode_only", _CountryCodeOnly),
                        new SqlParameter("@mobileNumber_only", _MobileNumberOnly),
                        new SqlParameter("@password", EDClass.Encrypt(_Password)),
                        new SqlParameter("@pincode", _Pincode),
                        new SqlParameter("@address", _Address),
                        new SqlParameter("@isActive", _Status),
                        new SqlParameter("@profileImage", _ImageName),
                        new SqlParameter("@submittedByLoginId", _UserLogin.Id),
                        new SqlParameter("@mode", _Mode)
                    };

                        _resp = db.Database
                            .SqlQuery<ResponseViewModel>(
                                "EXEC sp_InsertUpdateSchool @id,@schoolName,@schoolTypeId,@email,@mobile,@countryMobilePhoneCode_only,@mobileNumber_only,@password,@pincode,@address,@isActive,@profileImage,@submittedByLoginId,@mode",
                                queryParams_School
                            )
                            .FirstOrDefault();


                        //--If School successfully Inserted/Updated
                        if (_resp.ret == 1 || _resp.ret == 2)
                        {
                            //--Check if File uploaded
                            // if (_File != null)
                            //{
                            #region Save Profile-Image to the Application Folder

                            string _FirstCharacter_StudentName = _SchoolName.Substring(0, 1).ToUpper();

                            AvtarClass objAvtar = new AvtarClass();
                            Font font = new Font(FontFamily.GenericSerif, 45, FontStyle.Bold);
                            Color fontcolor = ColorTranslator.FromHtml("#FFF");
                            Color bgcolor = ColorTranslator.FromHtml("#83B869");

                            var _img = objAvtar.GenerateAvtarImage(_FirstCharacter_StudentName, font, fontcolor, bgcolor);
                            _img.Save(HttpContext.Current.Server.MapPath("/Content/SchoolImages/" + _ImageName));

                            #endregion

                            //--During Update-Mode only (Remove the Previous Image from Application Folder)
                            if (_resp.ret == 2)
                            {
                                #region Remove Previous Profile-Image from the Application Folder
                                var filePath = HttpContext.Current.Server.MapPath("/Content/SchoolImages/" + _resp.PreviousProfileImage);
                                if (System.IO.File.Exists(filePath))
                                {
                                    System.IO.File.Delete(filePath);
                                }
                                #endregion
                            }
                        }
                        // ✅ Send Email if New School Created
                        if (_resp.ret == 1)
                        {
                            SendEmail sendEmail = new SendEmail();
                            sendEmail.SendSchoolAccountCreatedEmail(_SchoolName, _Email, _Password);
                        }
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

        //--Get School Detail by Id-- 
        [Authorize(Roles = "SuperAdmin")]
        [Route("GetSchoolById")]
        [HttpGet]
        public HttpResponseMessage GetSchoolDetailById(Int64 SchoolID)
        {
            try
            {
                //--Get User Identity
                var identity = User.Identity as ClaimsIdentity;

                //--Check if user is authorized user or not
                if (identity != null)
                {
                    SchoolViewModel lstSchool = new SchoolViewModel();

                    //--Get School-Detail by School-ID
                    SqlParameter[] queryParams_School = new SqlParameter[] {
                    new SqlParameter("id", SchoolID),
                    new SqlParameter("mode", "2")
                    };
                    lstSchool = db.Database.SqlQuery<SchoolViewModel>("exec sp_ManageSchool @id,@mode", queryParams_School).FirstOrDefault();

                    if (lstSchool != null)
                    {
                        lstSchool.Password = EDClass.Decrypt(lstSchool.Password);
                    }

                    //--Create response
                    var objResponse = new
                    {
                        status = 1,
                        message = "Success",
                        data = new
                        { School = lstSchool }
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
            catch (Exception ex)
            {
                //--Create response as Error
                var objResponse = new { status = -100, message = "Internal Server Error!", data = "" };
                //sending response as error
                return Request.CreateResponse(HttpStatusCode.InternalServerError, objResponse);
            }
        }

        //--Get School Profile Detail by Token-- 
        [Authorize(Roles = "School")]
        [Route("GetSchoolProfile")]
        [HttpGet]
        public HttpResponseMessage GetSchoolProfileInfo()
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

                    SchoolViewModel lstSchool = new SchoolViewModel();

                    //--Get School-Profile-Detail by Login-ID
                    SqlParameter[] queryParams_School = new SqlParameter[] {
                    new SqlParameter("id", _LoginID_Exact),
                    new SqlParameter("mode", "5")
                    };
                    lstSchool = db.Database.SqlQuery<SchoolViewModel>("exec sp_ManageSchool @id,@mode", queryParams_School).FirstOrDefault();

                    if (lstSchool != null)
                    {
                        //--Create response
                        var objResponse = new
                        {
                            status = 1,
                            message = "Success",
                            data = new
                            { School = lstSchool }
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

        //-Delete School by School-Id-- 
        [Authorize(Roles = "SuperAdmin")]
        [Route("DeleteSchoolById")]
        [HttpGet]
        public HttpResponseMessage DeleteSchool(Int64 SchoolID)
        {
            try
            {
                //--Get User Identity
                var identity = User.Identity as ClaimsIdentity;

                //--Check if user is authorized user or not
                if (identity != null)
                {
                    ResponseViewModel _resp = new ResponseViewModel();

                    //--Delete School by School-ID
                    SqlParameter[] queryParams_School = new SqlParameter[] {
                    new SqlParameter("id", SchoolID),
                    new SqlParameter("mode", "3")
                    };
                    _resp = db.Database.SqlQuery<ResponseViewModel>("exec sp_ManageSchool @id,@mode", queryParams_School).FirstOrDefault();

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
                    var objResponse = new { status = -101, message = "Authorization has been denied for this request!" };
                    //sending response as Un-Authorized
                    return Request.CreateResponse(HttpStatusCode.Unauthorized, objResponse);
                }
            }
            catch (Exception ex)
            {
                //--Create response as Error
                var objResponse = new { status = -100, message = "Internal Server Error!" };
                //sending response as error
                return Request.CreateResponse(HttpStatusCode.InternalServerError, objResponse);
            }
        }

        //-Change School Status by School-Id-- 
        [Authorize(Roles = "SuperAdmin")]
        [Route("ChangeSchoolStatusById")]
        [HttpGet]
        public HttpResponseMessage ChangeSchoolStatus(Int64 SchoolID)
        {
            try
            {
                //--Get User Identity
                var identity = User.Identity as ClaimsIdentity;

                //--Check if user is authorized user or not
                if (identity != null)
                {
                    ResponseViewModel _resp = new ResponseViewModel();

                    //--Delete School by School-ID
                    SqlParameter[] queryParams_School = new SqlParameter[] {
                    new SqlParameter("id", SchoolID),
                    new SqlParameter("mode", "4")
                    };
                    _resp = db.Database.SqlQuery<ResponseViewModel>("exec sp_ManageSchool @id,@mode", queryParams_School).FirstOrDefault();

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
                    var objResponse = new { status = -101, message = "Authorization has been denied for this request!" };
                    //sending response as Un-Authorized
                    return Request.CreateResponse(HttpStatusCode.Unauthorized, objResponse);
                }
            }
            catch (Exception ex)
            {
                //--Create response as Error
                var objResponse = new { status = -100, message = "Internal Server Error!" };
                //sending response as error
                return Request.CreateResponse(HttpStatusCode.InternalServerError, objResponse);
            }
        }

        //--Get SuperAdmin Profile Detail by Token-- 
        [Authorize(Roles = "SuperAdmin")]
        [Route("GetSuperAdminProfile")]
        [HttpGet]
        public HttpResponseMessage GetSuperAdminProfileInfo()
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

                    SuperAdminViewModel lstSuperAdmin = new SuperAdminViewModel();

                    //--Get Staff-Profile-Detail by Login-ID
                    SqlParameter[] queryParams_SuperAdmin = new SqlParameter[] {
                    new SqlParameter("id", _LoginID_Exact),
                    new SqlParameter("mode", "5")
                    };
                    lstSuperAdmin = db.Database.SqlQuery<SuperAdminViewModel>("exec sp_ManageSchool @id,@mode", queryParams_SuperAdmin).FirstOrDefault();

                    if (lstSuperAdmin != null)
                    {
                        //--Create response
                        var objResponse = new
                        {
                            status = 1,
                            message = "Success",
                            data = new
                            { superadmin = lstSuperAdmin }
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
