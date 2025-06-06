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
    public class StaffAPIController : ApiController
    {
        private SchoolManagementContext db = new SchoolManagementContext();

        //--Get All Staffs-List-- 
        [Authorize(Roles = "Admin,Staff")]
        [Route("GetAllStaff")]
        [HttpGet]
        public HttpResponseMessage GetAllStaffData()
        {
            try
            {
                //--Get User Identity
                var identity = User.Identity as ClaimsIdentity;

                //--Check if user is authorized user or not
                if (identity != null)
                {
                    List<StaffViewModel> lstStaff = new List<StaffViewModel>();

                    //--Get All Staff-List
                    SqlParameter[] queryParams_Staff = new SqlParameter[] {
                    new SqlParameter("id", "0"),
                    new SqlParameter("mode", "1")
                    };
                    lstStaff = db.Database.SqlQuery<StaffViewModel>("exec sp_ManageStaff @id,@mode", queryParams_Staff).ToList();

                    //--Create response
                    var objResponse = new
                    {
                        status = 1,
                        message = "Success",
                        data = new
                        { staff = lstStaff }
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
                var objResponse = new { status = -100, message = "Internal Server Error!", data = "", errorMessage = ex.Message.ToString() };
                //sending response as error
                return Request.CreateResponse(HttpStatusCode.InternalServerError, objResponse);
            }
        }

        //--Add New Staff-- 
        [Authorize(Roles = "Admin")]
        [Route("InsertUpdateStaff")]
        [HttpPost]
        public HttpResponseMessage InsertUpdateStaffData()
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
                        //Int64 _BranchId = Convert.ToInt64(HttpRequest.Params["branchId"]);
                        string _FirstName = HttpRequest.Params["firstName"];
                        string _LastName = HttpRequest.Params["lastName"];
                        string _Email = HttpRequest.Params["email"];
                        string _MobileNumber = HttpRequest.Params["mobile"];
                        string _CountryMobilePhoneCode_Only = HttpRequest.Params["countryMobilePhoneCodeOnly"];
                        string _MobileNumber_Only = HttpRequest.Params["mobileNumberOnly"];
                        string _Password = HttpRequest.Params["password"];
                        string _Pincode = HttpRequest.Params["pincode"];
                        string _Address = HttpRequest.Params["address"];
                        int _IsActive = Convert.ToInt32(HttpRequest.Params["isActive"]);
                        string _Gender = HttpRequest.Params["gender"];
                        Int64 _StaffTypeId = Convert.ToInt64(HttpRequest.Params["staffTypeId"]);
                        decimal _WorkExperience = Convert.ToDecimal(HttpRequest.Params["workExperience"]);
                        decimal _Salary = Convert.ToDecimal(HttpRequest.Params["salary"]);
                        string _JoiningDate = HttpRequest.Params["joiningDate"];
                        int _Mode = Convert.ToInt32(HttpRequest.Params["mode"]);


                        string _ImageName = Guid.NewGuid().ToString().Replace("-", "") + ".gif";

                        //--Insert New Staff Detail
                        SqlParameter[] queryParams_Staff = new SqlParameter[] {
                        new SqlParameter("id", _Id),
                        new SqlParameter("firstName", _FirstName),
                        new SqlParameter("lastName", _LastName),
                        new SqlParameter("email", _Email),
                        new SqlParameter("mobile", _MobileNumber),
                        new SqlParameter("countryMobilePhoneCode_only", _CountryMobilePhoneCode_Only),
                        new SqlParameter("mobileNumber_only", _MobileNumber_Only),
                        new SqlParameter("password", EDClass.Encrypt(_Password)),
                        new SqlParameter("pincode", _Pincode),
                        new SqlParameter("address", _Address),
                        new SqlParameter("isActive", _IsActive),
                        new SqlParameter("profileImage", _ImageName),
                        new SqlParameter("gender", _Gender),
                        new SqlParameter("staffTypeId", _StaffTypeId),
                        new SqlParameter("workExperience", _WorkExperience),
                        new SqlParameter("salary", _Salary),
                        new SqlParameter("joiningDate", _JoiningDate),
                        new SqlParameter("submittedByLoginId", _UserLogin.Id),
                        new SqlParameter("mode", _Mode)
                        };
                        _resp = db.Database.SqlQuery<ResponseViewModel>("exec sp_InsertUpdateStaff @id,@firstName,@lastName,@email,@mobile,@countryMobilePhoneCode_only,@mobileNumber_only,@password,@pincode,@address,@isActive,@profileImage,@submittedByLoginId,@gender,@staffTypeId,@workExperience,@salary,@joiningDate,@mode", queryParams_Staff).FirstOrDefault();

                        //--If staff successfully Inserted/Updated
                        if (_resp.ret == 1 || _resp.ret == 2)
                        {
                            //--Check if File uploaded
                            // if (_File != null)
                            //{
                            #region Save Profile-Image to the Application Folder

                            string _FirstCharacter_StudentName = _FirstName.Substring(0, 1).ToUpper();

                            AvtarClass objAvtar = new AvtarClass();
                            Font font = new Font(FontFamily.GenericSerif, 45, FontStyle.Bold);
                            Color fontcolor = ColorTranslator.FromHtml("#FFF");
                            Color bgcolor = ColorTranslator.FromHtml("#83B869");

                            var _img = objAvtar.GenerateAvtarImage(_FirstCharacter_StudentName, font, fontcolor, bgcolor);
                            _img.Save(HttpContext.Current.Server.MapPath("/Content/StaffImages/" + _ImageName));

                            #endregion

                            //--During Update-Mode only (Remove the Previous Image from Application Folder)
                            if (_resp.ret == 2)
                            {
                                #region Remove Previous Profile-Image from the Application Folder
                                var filePath = HttpContext.Current.Server.MapPath("/Content/StaffImages/" + _resp.PreviousProfileImage);
                                if (System.IO.File.Exists(filePath))
                                {
                                    System.IO.File.Delete(filePath);
                                }
                                #endregion
                            }
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
                var objResponse = new { status = -100, message = "Internal Server Error!", data = "", errorMessage = ex.Message.ToString() };
                //sending response as error
                return Request.CreateResponse(HttpStatusCode.InternalServerError, objResponse);
            }
        }

        //--Get Staff Detail by Id-- 
        [Authorize(Roles = "Admin,Staff")]
        [Route("GetStaffById")]
        [HttpGet]
        public HttpResponseMessage GetStaffDetailById(Int64 staffID)
        {
            try
            {
                //--Get User Identity
                var identity = User.Identity as ClaimsIdentity;

                //--Check if user is authorized user or not
                if (identity != null)
                {
                    StaffViewModel lstStaff = new StaffViewModel();

                    //--Get Staff-Detail by Staff-ID
                    SqlParameter[] queryParams_Staff = new SqlParameter[] {
                    new SqlParameter("id", staffID),
                    new SqlParameter("mode", "2")
                    };
                    lstStaff = db.Database.SqlQuery<StaffViewModel>("exec sp_ManageStaff @id,@mode", queryParams_Staff).FirstOrDefault();

                    if (lstStaff != null)
                    {
                        lstStaff.Password = EDClass.Decrypt(lstStaff.Password);
                    }

                    //--Create response
                    var objResponse = new
                    {
                        status = 1,
                        message = "Success",
                        data = new
                        { staff = lstStaff }
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
                var objResponse = new { status = -100, message = "Internal Server Error!", data = "", errorMessage = ex.Message.ToString() };
                //sending response as error
                return Request.CreateResponse(HttpStatusCode.InternalServerError, objResponse);
            }
        }

        //--Get Staff Profile Detail by Token-- 
        [Authorize(Roles = "Staff")]
        [Route("GetStaffProfile")]
        [HttpGet]
        public HttpResponseMessage GetStaffProfileInfo()
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

                    StaffViewModel lstStaff = new StaffViewModel();

                    //--Get Staff-Profile-Detail by Login-ID
                    SqlParameter[] queryParams_Staff = new SqlParameter[] {
                    new SqlParameter("id", _LoginID_Exact),
                    new SqlParameter("mode", "5")
                    };
                    lstStaff = db.Database.SqlQuery<StaffViewModel>("exec sp_ManageStaff @id,@mode", queryParams_Staff).FirstOrDefault();

                    if (lstStaff != null)
                    {
                        //--Create response
                        var objResponse = new
                        {
                            status = 1,
                            message = "Success",
                            data = new
                            { staff = lstStaff }
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
                var objResponse = new { status = -100, message = "Internal Server Error!", data = "", errorMessage = ex.Message.ToString() };
                //sending response as error
                return Request.CreateResponse(HttpStatusCode.InternalServerError, objResponse);
            }
        }

        //-Delete Staff by Staff-Id-- 
        [Authorize(Roles = "Admin")]
        [Route("DeleteStaffById")]
        [HttpGet]
        public HttpResponseMessage DeleteStaff(Int64 staffID)
        {
            try
            {
                //--Get User Identity
                var identity = User.Identity as ClaimsIdentity;

                //--Check if user is authorized user or not
                if (identity != null)
                {
                    ResponseViewModel _resp = new ResponseViewModel();

                    //--Delete Staff by Staff-ID
                    SqlParameter[] queryParams_Staff = new SqlParameter[] {
                    new SqlParameter("id", staffID),
                    new SqlParameter("mode", "3")
                    };
                    _resp = db.Database.SqlQuery<ResponseViewModel>("exec sp_ManageStaff @id,@mode", queryParams_Staff).FirstOrDefault();

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
                var objResponse = new { status = -100, message = "Internal Server Error!", errorMessage = ex.Message.ToString() };
                //sending response as error
                return Request.CreateResponse(HttpStatusCode.InternalServerError, objResponse);
            }
        }

        //-Change Staff Status by Staff-Id-- 
        [Authorize(Roles = "Admin")]
        [Route("ChangeStaffStatusById")]
        [HttpGet]
        public HttpResponseMessage ChangeStaffStatus(Int64 staffID)
        {
            try
            {
                //--Get User Identity
                var identity = User.Identity as ClaimsIdentity;

                //--Check if user is authorized user or not
                if (identity != null)
                {
                    ResponseViewModel _resp = new ResponseViewModel();

                    //--Delete Staff by Staff-ID
                    SqlParameter[] queryParams_Staff = new SqlParameter[] {
                    new SqlParameter("id", staffID),
                    new SqlParameter("mode", "4")
                    };
                    _resp = db.Database.SqlQuery<ResponseViewModel>("exec sp_ManageStaff @id,@mode", queryParams_Staff).FirstOrDefault();

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
                var objResponse = new { status = -100, message = "Internal Server Error!", errorMessage = ex.Message.ToString() };
                //sending response as error
                return Request.CreateResponse(HttpStatusCode.InternalServerError, objResponse);
            }
        }

        //-Get All Active StaffType-FieldTypeValues -- 
        [Authorize(Roles = "Admin")]
        [Route("GetAllActiveStaffTypes")]
        [HttpGet]
        public HttpResponseMessage GetStaffFieldTypeValues()
        {
            try
            {
                //--Get User Identity
                var identity = User.Identity as ClaimsIdentity;

                //--Check if user is authorized user or not
                if (identity != null)
                {
                    List<FieldTypeValueViewModel> lstStaffTypeValues = new List<FieldTypeValueViewModel>();

                    //--Delete Staff by Staff-ID
                    SqlParameter[] queryParams_Staff = new SqlParameter[] {
                    new SqlParameter("id", "0"),
                    new SqlParameter("fieldTypeId", "0"),
                    new SqlParameter("fieldTypeName", "Staff"),
                    new SqlParameter("fieldTypeValue", ""),
                    new SqlParameter("mode", "3")
                    };
                    lstStaffTypeValues = db.Database.SqlQuery<FieldTypeValueViewModel>("exec sp_ManageFieldTypeValues @id,@fieldTypeId,@fieldTypeName,@fieldTypeValue,@mode", queryParams_Staff).ToList();

                    //--Create response
                    var objResponse = new
                    {
                        status = 1,
                        message = "Success",
                        data = new
                        { staffTypeValues = lstStaffTypeValues }
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
                var objResponse = new { status = -100, message = "Internal Server Error!", errorMessage = ex.Message.ToString() };
                //sending response as error
                return Request.CreateResponse(HttpStatusCode.InternalServerError, objResponse);
            }
        }

    }
}