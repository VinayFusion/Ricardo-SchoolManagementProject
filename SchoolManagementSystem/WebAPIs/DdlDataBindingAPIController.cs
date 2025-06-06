using SchoolManagementSystem.DAL;
using SchoolManagementSystem.ViewModel;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http;

namespace SchoolManagementSystem.WebAPIs
{
    public class DdlDataBindingAPIController : ApiController
    {
        private SchoolManagementContext db = new SchoolManagementContext();
        //---------------------------------------------------------------------------------------------------------
        private List<T> Call_FilterDdlClassSession_SP<T>(SQL_ParametersViewModel_VM Params)
        {
            if (String.IsNullOrEmpty(Params.SessionName))
                Params.SessionName = "";

            List<T> resultlst = new List<T>();
            SqlParameter[] queryParams = new SqlParameter[] {
                    new SqlParameter("classId", Params.ClassId),
                    new SqlParameter("sessionId", Params.SessionId),
                    new SqlParameter("sessionName", Params.SessionName),
                    new SqlParameter("mode", Params.Mode)
                    };
            //
            resultlst = db.Database.SqlQuery<T>("exec sp_FilterDdlClassSession @classId,@sessionId,@sessionName,@mode", queryParams).ToList();
            return resultlst;
        }
        [Authorize(Roles = "Admin,Staff")]
        [Route("GetDdlClassDataForFilter")]
        [HttpGet]
        public HttpResponseMessage GetDdlClassDataForFilter(string sessionName)
        {
            try
            {
                //--Get User Identity
                var identity = User.Identity as ClaimsIdentity;

                //--Check if user is authorized user or not
                if (identity != null && sessionName != "0" && sessionName != "")
                {
                    List<SessionFeeViewModel> lstddlData = new List<SessionFeeViewModel>();

                    SQL_ParametersViewModel_VM StudentSQLParameter_VM = new SQL_ParametersViewModel_VM()
                    {
                        SessionName = sessionName,
                        Mode = 1,
                    };
                    lstddlData = Call_FilterDdlClassSession_SP<SessionFeeViewModel>(StudentSQLParameter_VM).ToList();

                    //--Create response
                    var objResponse = new
                    {
                        status = 1,
                        message = "Success",
                        data = new
                        { ddlDataList = lstddlData }
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

        [Authorize(Roles = "Admin,Staff")]
        [Route("GetDdlSessionDataForFilter")]
        [HttpGet]
        public HttpResponseMessage GetDdlSessionDataForFilter()
        {
            try
            {
                //--Get User Identity
                var identity = User.Identity as ClaimsIdentity;

                //--Check if user is authorized user or not
                if (identity != null)
                {
                    List<SessionFeeViewModel> lstddlData = new List<SessionFeeViewModel>();

                    SQL_ParametersViewModel_VM StudentSQLParameter_VM = new SQL_ParametersViewModel_VM()
                    {
                        Mode = 2,
                    };
                    lstddlData = Call_FilterDdlClassSession_SP<SessionFeeViewModel>(StudentSQLParameter_VM).ToList();

                    //--Create response
                    var objResponse = new
                    {
                        status = 1,
                        message = "Success",
                        data = new
                        { ddlDataList = lstddlData }
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

        [Authorize(Roles = "Admin,Staff")]
        [Route("GetDdlClassAllData")]
        [HttpGet]
        public HttpResponseMessage GetDdlClassAllData()
        {
            try
            {
                //--Get User Identity
                var identity = User.Identity as ClaimsIdentity;

                //--Check if user is authorized user or not
                if (identity != null)
                {
                    List<SessionFeeViewModel> lstddlData = new List<SessionFeeViewModel>();

                    SQL_ParametersViewModel_VM StudentSQLParameter_VM = new SQL_ParametersViewModel_VM()
                    {
                        Mode = 4,
                    };
                    lstddlData = Call_FilterDdlClassSession_SP<SessionFeeViewModel>(StudentSQLParameter_VM).ToList();

                    //--Create response
                    var objResponse = new
                    {
                        status = 1,
                        message = "Success",
                        data = new
                        { ddlDataList = lstddlData }
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

        [Authorize(Roles = "Admin,Staff")]
        [Route("GetMultipleDdlDataBinding")]
        [HttpGet]
        public HttpResponseMessage GetMultipleDdlDataBinding(string nameOfDataBind)
        {
            try
            {
                //--Get User Identity
                var identity = User.Identity as ClaimsIdentity;

                //--Check if user is authorized user or not
                if (identity != null)
                {
                    List<ClassViewModel> lstddl = new List<ClassViewModel>();

                    SqlParameter[] queryParams_ddl = new SqlParameter[] {
                    new SqlParameter("id", "0"),
                    new SqlParameter("mode", "1"),
                    new SqlParameter("NameOfDataBind", nameOfDataBind),
                    new SqlParameter("sessionName", "")
                    };
                    lstddl = db.Database.SqlQuery<ClassViewModel>("exec sp_ddlDataBinding @id,@mode,@NameOfDataBind,@sessionName", queryParams_ddl).ToList();

                    //--Create response
                    var objResponse = new
                    {
                        status = 1,
                        message = "Success",
                        data = new
                        { DdlValue = lstddl }
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

        //--------------------------------------------------------------------------------------------------------
    
    }
}
