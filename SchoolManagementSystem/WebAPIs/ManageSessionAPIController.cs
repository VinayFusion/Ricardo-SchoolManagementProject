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
    public class ManageSessionAPIController : ApiController
    {
        private SchoolManagementContext db = new SchoolManagementContext();

        private List<T> Call_ManageSession_SP<T>(SQL_ParametersViewModel_VM Params)
        {
            if (String.IsNullOrEmpty(Params.SessionName))
                Params.SessionName = "";

            List<T> resultlst = new List<T>();
            SqlParameter[] queryParams = new SqlParameter[] {
                    new SqlParameter("id", Params.Id),
                    new SqlParameter("sessionName", Params.SessionName),
                    new SqlParameter("mode", Params.Mode)
                    };
            //
            resultlst = db.Database.SqlQuery<T>("exec sp_ManageSession @id,@sessionName,@mode", queryParams).ToList();
            return resultlst;
        }

     
        [Authorize(Roles = "Admin,Staff")]
        [Route("GetAllSessions")]
        [HttpGet]
        public HttpResponseMessage GetAllSessionData(string sessionName = "")
        {
            try
            {
                //--Get User Identity
                var identity = User.Identity as ClaimsIdentity;

                //--Check if user is authorized user or not
                if (identity != null)
                {
                    List<SessionViewModel> lstSession = new List<SessionViewModel>();
                    var _mode = sessionName == "" || sessionName == null ? 1 : 5;

                    SQL_ParametersViewModel_VM sessionSQLParameter_VM = new SQL_ParametersViewModel_VM()
                    {
                        SessionName = sessionName,
                        Mode = _mode,
                    };
                    lstSession = Call_ManageSession_SP<SessionViewModel>(sessionSQLParameter_VM);


                    //--Create response
                    var objResponse = new
                    {
                        status = 1,
                        message = "Success",
                        data = new
                        { sessions = lstSession }
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

        //--Add New Session-- 
        [Authorize(Roles = "Admin,Staff")]
        [Route("InsertUpdateSession")]
        [HttpPost]
        public HttpResponseMessage InsertUpdateSessionData()
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
                        Int64 _ClassId = Convert.ToInt64(HttpRequest.Params["ClassId"]);
                        string _SessionName = HttpRequest.Params["sessionName"];
                        string _StartYear = HttpRequest.Params["startYear"];
                        string _EndYear = HttpRequest.Params["endYear"];
                        string _Status = HttpRequest.Params["status"];
                        int _Mode = Convert.ToInt32(HttpRequest.Params["mode"]);

                        //--Insert New Session Detail
                        SqlParameter[] queryParams_Session = new SqlParameter[] {
                        new SqlParameter("id", _Id),
                        new SqlParameter("classId", _ClassId),
                        new SqlParameter("sessionName", _SessionName),
                        new SqlParameter("startYear", _StartYear),
                        new SqlParameter("endYear", _EndYear),
                        new SqlParameter("status", _Status),
                        new SqlParameter("submittedByLoginId", _UserLogin.Id),
                        new SqlParameter("mode", _Mode)
                        };
                        _resp = db.Database.SqlQuery<ResponseViewModel>("exec sp_InsertUpdateSession @id,@classId,@sessionName,@startYear,@endYear,@status,@submittedByLoginId,@mode", queryParams_Session).FirstOrDefault();

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

        //--Get Session Detail by Id-- 
        [Authorize(Roles = "Admin,Staff")]
        [Route("GetSessionById")]
        [HttpGet]
        public HttpResponseMessage GetSessionDetailById(Int64 sessionID)
        {
            try
            {
                //--Get User Identity
                var identity = User.Identity as ClaimsIdentity;

                //--Check if user is authorized user or not
                if (identity != null)
                {
                    SessionViewModel lstSession = new SessionViewModel();

                    //--Get Session-Detail by Session-ID
                    SQL_ParametersViewModel_VM sessionSQLParameter_VM = new SQL_ParametersViewModel_VM()
                    {
                        Id = sessionID,
                        Mode = 2,
                    };
                    lstSession = Call_ManageSession_SP<SessionViewModel>(sessionSQLParameter_VM).FirstOrDefault();
                    //SqlParameter[] queryParams_Session = new SqlParameter[] {
                    //new SqlParameter("id", sessionID),
                    //new SqlParameter("mode", "2")
                    //};
                    //session = db.Database.SqlQuery<SessionViewModel>("exec sp_ManageSession @id,@mode", queryParams_Session).FirstOrDefault();


                    //--Create response
                    var objResponse = new
                    {
                        status = 1,
                        message = "Success",
                        data = new
                        { session = lstSession }
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

        //-Delete Session by Session-Id-- 
        [Authorize(Roles = "Admin")]
        [Route("DeleteSessionById")]
        [HttpGet]
        public HttpResponseMessage DeleteSession(Int64 sessionID)
        {
            try
            {
                //--Get User Identity
                var identity = User.Identity as ClaimsIdentity;

                //--Check if user is authorized user or not
                if (identity != null)
                {
                    ResponseViewModel _resp = new ResponseViewModel();

                    //--Delete Session by Session-ID
                    SQL_ParametersViewModel_VM sessionSQLParameter_VM = new SQL_ParametersViewModel_VM()
                    {
                        Id = sessionID,
                        Mode = 3,
                    };
                    _resp = Call_ManageSession_SP<ResponseViewModel>(sessionSQLParameter_VM).FirstOrDefault();
                    //SqlParameter[] queryParams_Session = new SqlParameter[] {
                    //new SqlParameter("id", sessionID),
                    //new SqlParameter("mode", "3")
                    //};
                    //_resp = db.Database.SqlQuery<ResponseViewModel>("exec sp_ManageSession @id,@mode", queryParams_Session).FirstOrDefault();

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

        //-Change Session Status by Session-Id-- 
        [Authorize(Roles = "Admin,Staff")]
        [Route("ChangeSessionStatusById")]
        [HttpGet]
        public HttpResponseMessage ChangeSessionStatus(Int64 sessionID)
        {
            try
            {
                //--Get User Identity
                var identity = User.Identity as ClaimsIdentity;

                //--Check if user is authorized user or not
                if (identity != null)
                {
                    ResponseViewModel _resp = new ResponseViewModel();

                    //--Delete Session by Session-ID
                    SQL_ParametersViewModel_VM sessionSQLParameter_VM = new SQL_ParametersViewModel_VM()
                    {
                        Id = sessionID,
                        Mode = 4,
                    };
                    _resp = Call_ManageSession_SP<ResponseViewModel>(sessionSQLParameter_VM).FirstOrDefault();
                    //SqlParameter[] queryParams_Session = new SqlParameter[] {
                    //new SqlParameter("id", sessionID),
                    //new SqlParameter("mode", "4")
                    //};
                    //_resp = db.Database.SqlQuery<ResponseViewModel>("exec sp_ManageSession @id,@mode", queryParams_Session).FirstOrDefault();

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


    }
}