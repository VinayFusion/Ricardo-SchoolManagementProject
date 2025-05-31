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
    public class ManageClassAPIController : ApiController
    {
        private SchoolManagementContext db = new SchoolManagementContext();

        [Authorize(Roles = "Admin,Staff")]
        [Route("GetAllClass")]
        [HttpGet]
        public HttpResponseMessage GetAllClassData()
        {
            try
            {
                //--Get User Identity
                var identity = User.Identity as ClaimsIdentity;

                //--Check if user is authorized user or not
                if (identity != null)
                {
                    List<ClassViewModel> lstClass = new List<ClassViewModel>();

                    //--Get All Class-List
                    SqlParameter[] queryParams_Class = new SqlParameter[] {
                    new SqlParameter("id", "0"),
                    new SqlParameter("mode", "1")
                    };
                    lstClass = db.Database.SqlQuery<ClassViewModel>("exec sp_ManageClassDetail @id,@mode", queryParams_Class).ToList();

                    //--Create response
                    var objResponse = new
                    {
                        status = 1,
                        message = "Success",
                        data = new
                        { Class = lstClass}
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

        //--Add New Class-- 
        [Authorize(Roles = "Admin,Staff")]
        [Route("InsertUpdateClass")]
        [HttpPost]
        public HttpResponseMessage InsertUpdateClassData()
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
                        Int64 _Id = Convert.ToInt64(HttpRequest.Params["CId"]);
                        Int64 _SessionId = Convert.ToInt64(HttpRequest.Params["SessionId"]);
                        string _ClassName = HttpRequest.Params["ClassName"];
                        int _Mode = Convert.ToInt32(HttpRequest.Params["mode"]);

                        //--Insert New Class Detail
                        SqlParameter[] queryParams_Class = new SqlParameter[] {
                        new SqlParameter("id", _Id),
                        new SqlParameter("sessionId", _SessionId),
                        new SqlParameter("className", _ClassName),
                        new SqlParameter("submittedByLoginId", _UserLogin.Id),
                        new SqlParameter("mode", _Mode)
                        };
                        _resp = db.Database.SqlQuery<ResponseViewModel>("exec sp_InsertUpdateClassDetail @id, @sessionId,@className,@submittedByLoginId,@mode", queryParams_Class).FirstOrDefault();

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

        //--Get Class Detail by Id-- 
        [Authorize(Roles = "Admin,Staff")]
        [Route("GetClassById")]
        [HttpGet]
        public HttpResponseMessage GetClassDetailById(Int64 ClassID)
        {
            try
            {
                //--Get User Identity
                var identity = User.Identity as ClaimsIdentity;

                //--Check if user is authorized user or not
                if (identity != null)
                {
                    ClassViewModel Class = new ClassViewModel();

                    //--Get Class-Detail by Class-ID
                    SqlParameter[] queryParams_Class = new SqlParameter[] {
                    new SqlParameter("id", ClassID),
                    new SqlParameter("mode", "2")
                    };
                    Class = db.Database.SqlQuery<ClassViewModel>("exec sp_ManageClassDetail @id,@mode", queryParams_Class).FirstOrDefault();


                    //--Create response
                    var objResponse = new
                    {
                        status = 1,
                        message = "Success",
                        data = new
                        { Class = Class }
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

        //-Delete Class by Class-Id-- 
        [Authorize(Roles = "Admin")]
        [Route("DeleteClassById")]
        [HttpGet]
        public HttpResponseMessage DeleteClass(Int64 ClassID)
        {
            try
            {
                //--Get User Identity
                var identity = User.Identity as ClaimsIdentity;

                //--Check if user is authorized user or not
                if (identity != null)
                {
                    ResponseViewModel _resp = new ResponseViewModel();

                    //--Delete Class by Class-ID
                    SqlParameter[] queryParams_Class = new SqlParameter[] {
                    new SqlParameter("id", ClassID),
                    new SqlParameter("mode", "3")
                    };
                    _resp = db.Database.SqlQuery<ResponseViewModel>("exec sp_ManageClassDetail @id,@mode", queryParams_Class).FirstOrDefault();

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