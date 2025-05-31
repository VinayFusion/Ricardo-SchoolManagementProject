using SchoolManagementSystem.DAL;
using SchoolManagementSystem.Models;
using SchoolManagementSystem.ViewModel;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Security.Claims;
using System.Web;
using System.Web.Http;

namespace SchoolManagementSystem.WebAPIs
{
    public class ManageSessionFeeAPIController : ApiController
    {
        private SchoolManagementContext db = new SchoolManagementContext();

        private List<T> Call_ManageSessionFee_SP<T>(SQL_ParametersViewModel_VM Params)
        {
            if (String.IsNullOrEmpty(Params.SessionName))
                Params.SessionName = "";
            List<T> lstSessionFee = new List<T>();
            SqlParameter[] queryParams = new SqlParameter[] {
                    new SqlParameter("id", Params.Id),
                    new SqlParameter("classId", Params.ClassId),
                    new SqlParameter("sessionId", Params.SessionId),
                    new SqlParameter("sessionName", Params.SessionName),
                    new SqlParameter("mode", Params.Mode)
                    };
            //
            lstSessionFee = db.Database.SqlQuery<T>("exec sp_ManageSessionFee @id,@classId,@sessionId,@sessionName,@mode", queryParams).ToList();
            return lstSessionFee;
        }

        [Authorize(Roles = "Admin,Staff")]
        [Route("SessionBindByClass4SessionFee")]
        [HttpGet]
        public HttpResponseMessage SessionBindByClass4SessionFee(int classId)
        {
            try
            {
                //--Get User Identity
                var identity = User.Identity as ClaimsIdentity;

                //--Check if user is authorized user or not
                if (identity != null)
                {
                    List<SessionFeeViewModel> lstSessionFee = new List<SessionFeeViewModel>();
                    Int64 _Id = classId;

                    SQL_ParametersViewModel_VM sessionFeeSQLParameter_VM = new SQL_ParametersViewModel_VM()
                    {
                        Id = _Id,
                        Mode = 3,
                    };
                    lstSessionFee = Call_ManageSessionFee_SP<SessionFeeViewModel>(sessionFeeSQLParameter_VM);

                    //--Create response
                    var objResponse = new
                    {
                        status = 1,
                        message = "Success",
                        data = new
                        { SessionFee = lstSessionFee }
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
        [Route("GetFilterAllSessionFee")]
        [HttpGet]
        public HttpResponseMessage GetFilterAllSessionFee(int classid, string sessionName)
        {
            try
            {
                //--Get User Identity
                var identity = User.Identity as ClaimsIdentity;

                //--Check if user is authorized user or not
                if (identity != null)
                {
                    var _mode = 0;
                    List<SessionFeeViewModel> lstSessionFee = new List<SessionFeeViewModel>();
                    if (classid != 0 && sessionName != "")
                    {
                        _mode = 4;
                    }
                    else
                    {
                        _mode = classid != 0 ? 5 : sessionName != "" && sessionName != null ? 6 : 1;
                    }

                    SQL_ParametersViewModel_VM sessionFeeSQLParameter_VM = new SQL_ParametersViewModel_VM()
                    {
                        Id = 0,
                        ClassId = classid,
                        SessionName = sessionName,
                        Mode = _mode,
                    };
                    lstSessionFee = Call_ManageSessionFee_SP<SessionFeeViewModel>(sessionFeeSQLParameter_VM);

                    //--Create response
                    var objResponse = new
                    {
                        status = 1,
                        message = "Success",
                        data = new
                        { SessionFee = lstSessionFee }
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
        [Route("GetDetailOfSessionFee")]
        [HttpGet]
        public HttpResponseMessage GetDetailOfSessionFee(int sessionfeeId)
        {
            try
            {
                //--Get User Identity
                var identity = User.Identity as ClaimsIdentity;

                //--Check if user is authorized user or not
                if (identity != null)
                {
                    List<SessionFeeViewModel> lstSessionFee = new List<SessionFeeViewModel>();
                    SQL_ParametersViewModel_VM sessionFeeSQLParameter_VM = new SQL_ParametersViewModel_VM()
                    {
                        Id = sessionfeeId,
                        Mode = 2,
                    };
                    lstSessionFee = Call_ManageSessionFee_SP<SessionFeeViewModel>(sessionFeeSQLParameter_VM);

                    //--Create response
                    var objResponse = new
                    {
                        status = 1,
                        message = "Success",
                        data = new
                        { SessionFee = lstSessionFee }
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

        //--Add New Session Fee-- 
        [Authorize(Roles = "Admin,Staff")]
        [Route("InsertUpdateSessionFee")]
        [HttpPost]
        public HttpResponseMessage InsertUpdateSessionFeeData()
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
                        Int64 classId = Convert.ToInt64(HttpRequest.Params["ClassId"]);
                        Int64 sessionId = Convert.ToInt64(HttpRequest.Params["SessionId"]);
                        string feeTypeId = HttpRequest.Params["FeeTypeId"];
                        string feeAmount = HttpRequest.Params["FeeAmount"];
                        string remark = HttpRequest.Params["Remark"];
                        int _Mode = Convert.ToInt32(HttpRequest.Params["mode"]);

                        //--Insert New Session Detail
                        SqlParameter[] queryParams_Session = new SqlParameter[] {
                        new SqlParameter("id", _Id),
                        new SqlParameter("classId", classId),
                        new SqlParameter("sessionId", sessionId),
                        new SqlParameter("feeTypeId", feeTypeId),
                        new SqlParameter("feeAmount", feeAmount),
                        new SqlParameter("remark", remark),
                        new SqlParameter("submittedByLoginId", _UserLogin.Id),
                        new SqlParameter("mode", _Mode)
                        };
                        _resp = db.Database.SqlQuery<ResponseViewModel>("exec sp_InsertUpdateSessionFee @id,@classId,@sessionId,@feeTypeId,@feeAmount,@remark,@submittedByLoginId,@mode", queryParams_Session).FirstOrDefault();

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

    }
}