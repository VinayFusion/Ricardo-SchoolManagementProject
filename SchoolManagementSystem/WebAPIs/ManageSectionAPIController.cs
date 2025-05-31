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
    public class ManageSectionAPIController : ApiController
    {
        private SchoolManagementContext db = new SchoolManagementContext();

        private List<T> Call_ManageClassSection_SP<T>(SQL_ParametersViewModel_VM Params)
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
            resultlst = db.Database.SqlQuery<T>("exec sp_ManageClassSection @id,@sessionName,@mode", queryParams).ToList();
            return resultlst;
        }

        [Authorize(Roles = "Admin,Staff")]
        [Route("SessionBindByClass")]
        [HttpGet]
        public HttpResponseMessage SessionBindByClass(int classId,int IsAddFormRequest)
        {
            try
            {
                //--Get User Identity
                var identity = User.Identity as ClaimsIdentity;

                //--Check if user is authorized user or not
                if (identity != null)
                {
                    List<SectionViewModel> lstSection = new List<SectionViewModel>();
                    Int64 _Id = classId;
                    int _mode = -1;
                    if (IsAddFormRequest <= 3)
                    {
                        _mode = 3;
                    }
                    else if (IsAddFormRequest ==4 )
                    {
                        _mode = IsAddFormRequest;
                    }
                    SQL_ParametersViewModel_VM sessionSQLParameter_VM = new SQL_ParametersViewModel_VM()
                    {
                        Id = _Id,
                        Mode = _mode,
                    };
                    lstSection = Call_ManageClassSection_SP<SectionViewModel>(sessionSQLParameter_VM);

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

        [Authorize(Roles = "Admin,Staff")]
        [Route("GetAllSectionList")]
        [HttpGet]
        public HttpResponseMessage GetAllSectionList(string sessionName = "")
        {
            try
            {
                //--Get User Identity
                var identity = User.Identity as ClaimsIdentity;

                //--Check if user is authorized user or not
                if (identity != null)
                {
                    List<SectionViewModel> lstSection = new List<SectionViewModel>();
                    var _mode = sessionName == "" || sessionName == null ? 1 : 5;
                    SQL_ParametersViewModel_VM sessionSQLParameter_VM = new SQL_ParametersViewModel_VM()
                    {
                        SessionName = sessionName,
                        Mode = _mode,
                    };
                    lstSection = Call_ManageClassSection_SP<SectionViewModel>(sessionSQLParameter_VM);

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
                
        [Authorize(Roles = "Admin,Staff")]
        [Route("InsertUpdateSection")]
        [HttpPost]
        public HttpResponseMessage InsertUpdateSection()
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
                        string _Id = HttpRequest.Params["Id"];
                        Int64 _SessionId = Convert.ToInt64(HttpRequest.Params["SessionId"]);
                        string _SectionId = HttpRequest.Params["SectionId"];
                        int _Mode = Convert.ToInt32(HttpRequest.Params["mode"]);

                        //--Insert New Class Detail
                        SqlParameter[] queryParams_Class = new SqlParameter[] {
                        new SqlParameter("id", _Id),
                        new SqlParameter("sessionId", _SessionId),
                        new SqlParameter("SectionId", _SectionId),
                        new SqlParameter("submittedByLoginId", _UserLogin.Id),
                        new SqlParameter("mode", _Mode)
                        };
                        _resp = db.Database.SqlQuery<ResponseViewModel>("exec sp_InsertUpdateClassSection @id, @sessionId,@SectionId,@submittedByLoginId,@mode", queryParams_Class).FirstOrDefault();

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
        [Route("GetSectionById")]
        [HttpGet]
        public HttpResponseMessage GetSectionDetailById(Int64 sID)
        {
            try
            {
                //--Get User Identity
                var identity = User.Identity as ClaimsIdentity;

                //--Check if user is authorized user or not
                if (identity != null)
                {
                    SectionViewModel _Section = new SectionViewModel();

                    //--Get Section-Detail by Section-ID
                    SQL_ParametersViewModel_VM sessionSQLParameter_VM = new SQL_ParametersViewModel_VM()
                    {
                        Id = sID,
                        Mode = 2,
                    };
                    _Section = Call_ManageClassSection_SP<SectionViewModel>(sessionSQLParameter_VM).FirstOrDefault();

                    //--Create response
                    var objResponse = new
                    {
                        status = 1,
                        message = "Success",
                        data = new
                        { Section = _Section }
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

    }
}