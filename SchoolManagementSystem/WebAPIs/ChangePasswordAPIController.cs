using SchoolManagementSystem.Common;
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
    public class ChangePasswordAPIController : ApiController
    {
        private SchoolManagementContext db = new SchoolManagementContext();

        //--Change Password-- 
        [Authorize(Roles = "SuperAdmin,Admin,Staff")]
        [Route("UpdatePassword")]
        [HttpPost]
        public HttpResponseMessage UpdateUserPassword()
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
                        string _OldPassword = HttpRequest.Params["oldPassword"];
                        string _NewPassword = HttpRequest.Params["newPassword"];
                        string _ConfirmNewPassword = HttpRequest.Params["confirmNewPassword"];

                        //  Validate Current Password.
                        if (!_UserLogin.Password.Equals(EDClass.Encrypt(_OldPassword)))
                        {
                            //--Create response as Error
                            var objResponseError = new { status = -1, message = "Incorrect Old Password!", data = "" };
                            //sending response as error
                            return Request.CreateResponse(HttpStatusCode.OK, objResponseError);
                        }

                        // Confirm Password
                        if (!_NewPassword.Equals(_ConfirmNewPassword))
                        {
                            //--Create response as Error
                            var objResponseError = new { status = -1, message = "New Password doesn't matched!", data = "" };
                            //sending response as error
                            return Request.CreateResponse(HttpStatusCode.OK, objResponseError);
                        }

                        SqlParameter[] queryParams = new SqlParameter[] {
                            new SqlParameter("id", _UserLogin.Id),
                            new SqlParameter("email", ""),
                            new SqlParameter("password", EDClass.Encrypt(_NewPassword)),    
                            new SqlParameter("mode", "3")
                            };

                        ResponseViewModel resetResponse = db.Database.SqlQuery<ResponseViewModel>("exec sp_Login @id,@email,@password,@mode", queryParams).FirstOrDefault();


                        //--Create response
                        var objResponse = new
                        {
                            status = resetResponse.ret,
                            message = resetResponse.responseMessage,
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