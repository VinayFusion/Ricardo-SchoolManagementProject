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
    public class ManageStudentAPIController : ApiController
    {
        private SchoolManagementContext db = new SchoolManagementContext();

        private List<T> Call_ManageStudent_SP<T>(SQL_ParametersViewModel_VM Params)
        {
            if (String.IsNullOrEmpty(Params.SessionName))
                Params.SessionName = "";

            List<T> resultlst = new List<T>();
            SqlParameter[] queryParams = new SqlParameter[] {
                    new SqlParameter("id", Params.Id),
                    new SqlParameter("classId", Params.ClassId),
                    new SqlParameter("sessionId", Params.SessionId),
                    new SqlParameter("sessionName", Params.SessionName),
                    new SqlParameter("mode", Params.Mode)
                    };
            //
            resultlst = db.Database.SqlQuery<T>("exec sp_ManageStudent @id,@classId,@sessionId,@sessionName,@mode", queryParams).ToList();
            return resultlst;
        }

        //----this API Also use in PayFee ScriptFile and student file
        [Authorize(Roles = "Admin,Staff")]
        [Route("SessionBindByClass4Student")]
        [HttpGet]
        public HttpResponseMessage SessionBindByClass4Student(Int64 classId)
        {
            try
            {
                //--Get User Identity
                var identity = User.Identity as ClaimsIdentity;

                //--Check if user is authorized user or not
                if (identity != null)
                {
                    List<SectionViewModel> lstSession = new List<SectionViewModel>();
                    Int64 _Id = classId;
                    SQL_ParametersViewModel_VM StudentSQLParameter_VM = new SQL_ParametersViewModel_VM()
                    {
                        Id = _Id,
                        Mode = 5,
                    };
                    lstSession = Call_ManageStudent_SP<SectionViewModel>(StudentSQLParameter_VM).ToList();

                    //--Create response
                    var objResponse = new
                    {
                        status = 1,
                        message = "Success",
                        data = new
                        { Session = lstSession }
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

        [Authorize(Roles = "Admin,Staff")]
        [Route("GetAllDdlData4StudentProfile")]
        [HttpGet]
        public HttpResponseMessage GetAllDdlData4StudentProfile(Int64 studentID)
        {
            try
            {
                //--Get User Identity
                var identity = User.Identity as ClaimsIdentity;

                //--Check if user is authorized user or not
                if (identity != null)
                {
                    List<StudentViewModel> lstddl = new List<StudentViewModel>();

                    SQL_ParametersViewModel_VM StudentSQLParameter_VM = new SQL_ParametersViewModel_VM()
                    {
                        Id = studentID,
                        Mode = 7,
                    };
                    lstddl = Call_ManageStudent_SP<StudentViewModel>(StudentSQLParameter_VM).ToList();

                    //--Create response
                    var objResponse = new
                    {
                        status = 1,
                        message = "Success",
                        data = new
                        { Student = lstddl }
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

        [Authorize(Roles = "Admin,Staff")]
        [Route("SectionBindBySession4Student")]
        [HttpGet]
        public HttpResponseMessage SectionBindBySession4Student(Int64 sessionId)
        {
            try
            {
                //--Get User Identity
                var identity = User.Identity as ClaimsIdentity;

                //--Check if user is authorized user or not
                if (identity != null)
                {
                    List<SectionViewModel> lstSection = new List<SectionViewModel>();
                    
                    Int64 _Id = sessionId;

                    SQL_ParametersViewModel_VM StudentSQLParameter_VM = new SQL_ParametersViewModel_VM()
                    {
                        Id = _Id,
                        Mode = 6,
                    };
                    lstSection = Call_ManageStudent_SP<SectionViewModel>(StudentSQLParameter_VM).ToList();

                    //--Create response
                    var objResponse = new
                    {
                        status = 1,
                        message = "Success",
                        data = new
                        { Section = lstSection }
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

        //--Add New Student-- 
        [Authorize(Roles = "Admin,Staff")]
        [Route("InsertUpdateStudent")]
        [HttpPost]
        public HttpResponseMessage InsertUpdateStudentData()
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
                        Int64 _SessionId = Convert.ToInt64(HttpRequest.Params["sessionId"]);
                        Int64 _SectionId = Convert.ToInt64(HttpRequest.Params["sectionId"]);
                        string _FirstName = HttpRequest.Params["firstName"];
                        string _LastName = HttpRequest.Params["lastName"];
                        string _DOB = HttpRequest.Params["DOB"];
                        string _gender = HttpRequest.Params["gender"];
                        string _blood = HttpRequest.Params["blood"];
                        string _fatherName = HttpRequest.Params["fatherName"];
                        string _motherName = HttpRequest.Params["motherName"];
                        string _Email = HttpRequest.Params["email"];
                        string _MobileNumber = HttpRequest.Params["mobile"];
                        string _CountryMobilePhoneCode_Only = HttpRequest.Params["countryMobilePhoneCodeOnly"];
                        string _MobileNumber_Only = HttpRequest.Params["mobileNumberOnly"];
                        string _userName = HttpRequest.Params["userName"];
                        string _Password = HttpRequest.Params["password"];
                        string _Pincode = HttpRequest.Params["pincode"];
                        string _Address = HttpRequest.Params["address"];
                        int _HasTakenTransportService = Convert.ToInt32(HttpRequest.Params["hasTakenTransportService"]);
                        int _TransportAmount = Convert.ToInt32(HttpRequest.Params["transportAmount"]);
                        int _IsActive = Convert.ToInt32(HttpRequest.Params["isActive"]);
                        int _Mode = Convert.ToInt32(HttpRequest.Params["mode"]);

                        string _ImageName = Guid.NewGuid().ToString().Replace("-", "") + ".gif";

                        //--Insert New Student Detail
                        SqlParameter[] queryParams_Student = new SqlParameter[] {
                        new SqlParameter("id", _Id),
                        new SqlParameter("SessionId", _SessionId),
                        new SqlParameter("SectionId", _SectionId),
                        new SqlParameter("firstName", _FirstName),
                        new SqlParameter("lastName", _LastName),
                        new SqlParameter("DOB", _DOB),
                        new SqlParameter("gender", _gender),
                        new SqlParameter("blood", _blood),
                        new SqlParameter("fatherName", _fatherName),
                        new SqlParameter("motherName", _motherName),
                        new SqlParameter("email", _Email),
                        new SqlParameter("mobile", _MobileNumber),
                        new SqlParameter("countryMobilePhoneCode_only", _CountryMobilePhoneCode_Only),
                        new SqlParameter("mobileNumber_only", _MobileNumber_Only),
                        new SqlParameter("userName", _userName),
                        new SqlParameter("password", EDClass.Encrypt(_Password)),
                        new SqlParameter("pincode", _Pincode),
                        new SqlParameter("address", _Address),
                        new SqlParameter("hasTakenTransportService", _HasTakenTransportService),
                        new SqlParameter("transportAmount", _TransportAmount),
                        new SqlParameter("isActive", _IsActive),
                        new SqlParameter("profileImage", _ImageName),
                        new SqlParameter("submittedByLoginId", _UserLogin.Id),
                        new SqlParameter("mode", _Mode)
                        };
                        _resp = db.Database.SqlQuery<ResponseViewModel>("exec sp_InsertUpdateStudent @id,@SessionId,@SectionId,@firstName,@lastName,@DOB,@gender,@blood,@fatherName,@motherName,@email,@mobile,@countryMobilePhoneCode_only,@mobileNumber_only,@userName,@password,@pincode,@address,@hasTakenTransportService,@transportAmount,@isActive,@profileImage,@submittedByLoginId,@mode", queryParams_Student).FirstOrDefault();

                        //--If student successfully Inserted/Updated
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
                            _img.Save(HttpContext.Current.Server.MapPath("/Content/StudentImages/" + _ImageName));

                            #endregion

                            //--During Update-Mode only (Remove the Previous Image from Application Folder)
                            if (_resp.ret == 2)
                            {
                                #region Remove Previous Profile-Image from the Application Folder
                                var filePath = HttpContext.Current.Server.MapPath("/Content/StudentImages/" + _resp.PreviousProfileImage);
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
                var objResponse = new { status = -100, message = "Internal Server Error!", data = "" };
                //sending response as error
                return Request.CreateResponse(HttpStatusCode.InternalServerError, objResponse);
            }
        }
               
        [Authorize(Roles = "Admin,Staff")]
        [Route("GetFilterStudentList")]
        [HttpGet]
        public HttpResponseMessage GetFilterStudentList(int classid, string sessionName)
        {
            try
            {
                //--Get User Identity
                var identity = User.Identity as ClaimsIdentity;

                //--Check if user is authorized user or not
                if (identity != null)
                {
                    List<StudentViewModel> lstStudent = new List<StudentViewModel>();
                    var _mode = 0;
                    if (classid != 0 && sessionName != "")
                    {
                        _mode = 8;
                    }
                    else
                    {
                        _mode = classid != 0 ? 9 : sessionName != "" && sessionName != null ? 10 : 1;
                    }
                    SQL_ParametersViewModel_VM StudentSQLParameter_VM = new SQL_ParametersViewModel_VM()
                    {
                        ClassId = classid,
                        SessionName = sessionName,
                        Mode = _mode,
                    };
                    lstStudent = Call_ManageStudent_SP<StudentViewModel>(StudentSQLParameter_VM).ToList();
                    //--Create response
                    var objResponse = new
                    {
                        status = 1,
                        message = "Success",
                        data = new
                        { student = lstStudent }
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

        //--Get Student Detail by Id-- 
        [Authorize(Roles = "Admin,Staff")]
        [Route("GetStudentById")]
        [HttpGet]
        public HttpResponseMessage GetStudentDetailById(Int64 studentID)
        {
            try
            {
                //--Get User Identity
                var identity = User.Identity as ClaimsIdentity;

                //--Check if user is authorized user or not
                if (identity != null)
                {
                    StudentViewModel lstStudent = new StudentViewModel();

                    //--Get Student-Detail by Student-ID
                    SQL_ParametersViewModel_VM StudentSQLParameter_VM = new SQL_ParametersViewModel_VM()
                    {
                        Id = studentID,
                        Mode = 2,
                    };
                    lstStudent = Call_ManageStudent_SP<StudentViewModel>(StudentSQLParameter_VM).FirstOrDefault();
                    if (lstStudent != null)
                    {
                    lstStudent.Password = EDClass.Decrypt(lstStudent.Password);
                    }
                    #region Student-Fee Detail and joining detail
                    //if (lstStudent != null)
                    //{
                    //    lstStudent.Password = EDClass.Decrypt(lstStudent.Password);

                    //    #region Get Student-Fee Detail by Student-Id

                    //    List<Student_PayFeeDetail_VM> lstFeeDetail = new List<Student_PayFeeDetail_VM>();

                    //    SqlParameter[] queryParams_PayFee = new SqlParameter[] {
                    //    new SqlParameter("id", "0"),
                    //    new SqlParameter("from_date", ""),
                    //    new SqlParameter("to_date", ""),
                    //    new SqlParameter("studentId", studentID),
                    //    new SqlParameter("courseId", "0"),
                    //    new SqlParameter("mode", "3")
                    //    };
                    //    lstFeeDetail = db.Database.SqlQuery<Student_PayFeeDetail_VM>("exec sp_ManagePayFee @id,@from_date,@to_date,@studentId,@courseId,@mode", queryParams_PayFee).ToList();

                    //    #endregion

                    //    #region Get Student Joining-Time

                    //    string _JoiningTime = "";

                    //    DateTime zeroTime = new DateTime(1, 1, 1);
                    //    DateTime _Current_DateTime = DateTime.UtcNow.AddMinutes(330); //--Indian Standard DateTime

                    //    var _TimeSpan_Diff = _Current_DateTime - lstStudent.CreatedOn.AddMinutes(330);
                    //    //var dateSpan = DateTimeSpan.CompareDates(compareTo, now);

                    //    // Because we start at year 1 for the Gregorian
                    //    // calendar, we must subtract a year here.
                    //    int years = (zeroTime + _TimeSpan_Diff).Year - 1;

                    //    //var month = ((_Current_DateTime.Year - lstStudent.CreatedOn.Year) * 12) + _Current_DateTime.Month - lstStudent.CreatedOn.Month;
                    //    // month = Math.Abs(month);
                    //    // Divide by this constant to convert days to months.
                    //    const double daysToMonths = 30.4368499;
                    //    var month = Convert.ToInt32(Math.Floor(_TimeSpan_Diff.Days / daysToMonths));

                    //    var weeks = (_Current_DateTime - lstStudent.CreatedOn).TotalDays / 7;

                    //    //--Check Years
                    //    if (years >= 1)
                    //    {
                    //        if (years <= 1)
                    //        {
                    //            _JoiningTime = "Registered 1 year ago";
                    //        }
                    //        else
                    //        {
                    //            _JoiningTime = "Registered " + years + " years ago";
                    //        }
                    //    }
                    //    //--Check Months
                    //    else if (month >= 1)
                    //    {
                    //        if (month <= 1)
                    //        {
                    //            _JoiningTime = "Registered 1 month ago";
                    //        }
                    //        else
                    //        {
                    //            _JoiningTime = "Registered " + month + " months ago";
                    //        }
                    //    }
                    //    //--Check Weeks
                    //    else if (weeks >= 1)
                    //    {
                    //        if (weeks <= 1)
                    //        {
                    //            _JoiningTime = "Registered 1 week ago";
                    //        }
                    //        else
                    //        {
                    //            _JoiningTime = "Registered " + weeks.ToString().Substring(0, 1) + " weeks ago";
                    //        }
                    //    }
                    //    //--Check Day
                    //    else if (_TimeSpan_Diff.Days >= 1)
                    //    {
                    //        if (_TimeSpan_Diff.Days <= 1)
                    //        {
                    //            _JoiningTime = "Registered 1 day ago";
                    //        }
                    //        else
                    //        {
                    //            _JoiningTime = "Registered " + _TimeSpan_Diff.Days + " days ago";
                    //        }
                    //    }
                    //    //--Check Hours
                    //    else if (_TimeSpan_Diff.Hours >= 1)
                    //    {
                    //        if (_TimeSpan_Diff.Hours <= 1)
                    //        {
                    //            _JoiningTime = "Registered 1 hour ago";
                    //        }
                    //        else
                    //        {
                    //            _JoiningTime = "Registered " + _TimeSpan_Diff.Hours + " hours ago";
                    //        }
                    //    }
                    //    //--Check Minutes
                    //    else if (_TimeSpan_Diff.Minutes >= 1)
                    //    {
                    //        if (_TimeSpan_Diff.Minutes <= 1)
                    //        {
                    //            _JoiningTime = "Registered 1 minute ago";
                    //        }
                    //        else
                    //        {
                    //            _JoiningTime = "Registered " + _TimeSpan_Diff.Minutes + " minutes ago";
                    //        }
                    //    }
                    //    //--Check Second
                    //    else if (_TimeSpan_Diff.Seconds >= 1)
                    //    {
                    //        if (_TimeSpan_Diff.Seconds <= 1)
                    //        {
                    //            _JoiningTime = "Registered 1 second ago";
                    //        }
                    //        else
                    //        {
                    //            _JoiningTime = "Registered " + _TimeSpan_Diff.Seconds + " seconds ago";
                    //        }
                    //    }
                    //    #endregion

                    //    lstStudent.JoiningTime = _JoiningTime;
                    //    lstStudent.FeeData = lstFeeDetail;
                    //}
                    #endregion

                    //--Create response
                    var objResponse = new
                    {
                        status = 1,
                        message = "Success",
                        data = new
                        { student = lstStudent }
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

        //-Delete Student by Student-Id-- 
        [Authorize(Roles = "Admin,Staff")]
        [Route("DeleteStudentById")]
        [HttpGet]
        public HttpResponseMessage DeleteStudent(Int64 studentID)
        {
            try
            {
                //--Get User Identity
                var identity = User.Identity as ClaimsIdentity;

                //--Check if user is authorized user or not
                if (identity != null)
                {
                    ResponseViewModel _resp = new ResponseViewModel();

                    //--Delete Student by Student-ID
                    SQL_ParametersViewModel_VM StudentSQLParameter_VM = new SQL_ParametersViewModel_VM()
                    {
                        Id = studentID,
                        Mode = 3,
                    };
                    _resp = Call_ManageStudent_SP<ResponseViewModel>(StudentSQLParameter_VM).FirstOrDefault();

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

        //--Insert Update Student Profile Image -- 
        [Authorize(Roles = "Admin,Staff")]
        [Route("StudentProfileImageSet")]
        [HttpPost]
        public HttpResponseMessage StudentProfileImageSet()
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
                    if (_UserLogin.UserTypeId == 1 || _UserLogin.UserTypeId == 2)
                    {
                        var _resp = new ResponseViewModel();
                        //--Create object of HttpRequest
                        var HttpRequest = HttpContext.Current.Request;
                        var _id = Convert.ToInt64(HttpRequest.Params["id"]);
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

                        if (_TypeProfileImage != null && _TypeProfileImage != "" && _id != 0 && _FirstName != null && _FirstName != "")
                        {


                        //-------------------------------- Defined the path For save image ---------------------------//
                            if (_TypeProfileImage == "NewImage")
                            {
                                StoreImagePath = Path.Combine(HttpContext.Current.Server.MapPath("/Content/StudentImages/") + InputFileName);
                                ImagePath = ("/Content/StudentImages/") + InputFileName;
                                _ImageName = InputFileName;
                            }
                            else if (_TypeProfileImage == "ResetImage")
                            {
                                StoreImagePath = (HttpContext.Current.Server.MapPath("/Content/StudentImages/" + _ImageName));
                                ImagePath = ("/Content/StudentImages/") + _ImageName;
                            }

                            //--------------------------------------- Image name only save in data base --------------------------------//

                            SqlParameter[] queryParams_Admin = new SqlParameter[] {
                        new SqlParameter("id", _id),
                        new SqlParameter("profileImage", _ImageName),
                        new SqlParameter("mode", 3)
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
                                if (_UserLogin.UserTypeId == 1 || _UserLogin.UserTypeId == 2)
                                {
                                    filePath = HttpContext.Current.Server.MapPath("/Content/StudentImages/" + _resp.PreviousProfileImage);
                                }


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
                                status = 1,
                                message = _resp.responseMessage,
                                data = new
                                { imagePath = ImagePath}
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