using SchoolManagementSystem.DAL;
using SchoolManagementSystem.ViewModel;
using SchoolManagementSystem.Models;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;

using System.Linq;
using System.Net;
using System.Net.Http;
using System.Security.Claims;
using System.Web;
using System.Web.Http;
using System.Security.Cryptography;
using SchoolManagementSystem.Common;

namespace SchoolManagementSystem.WebAPIs
{
    public class ManagePayFeeAPIController : ApiController
    {
        private SchoolManagementContext db = new SchoolManagementContext();
        SendEmail objSend = new SendEmail();
        PrintClass objPrint = new PrintClass();
        private List<T> Call_ManagePayFee_SP<T>(SQL_ParametersViewModel_VM Params)
        {

            List<T> lstPayFee = new List<T>();
            //if (String.IsNullOrEmpty(Params.SessionName))
            //    Params.SessionName = "";
            SqlParameter[] queryParams = new SqlParameter[] {
                    new SqlParameter("id", Params.Id),
                    new SqlParameter("classId", Params.ClassId),
                    new SqlParameter("sessionId", Params.SessionId),
                    new SqlParameter("sessionName", Params.SessionName),
                    new SqlParameter("mode", Params.Mode)
                    };
            //
            lstPayFee = db.Database.SqlQuery<T>("exec sp_ManagePayFee @id,@classId,@sessionId,@sessionName,@mode", queryParams).ToList();
            return lstPayFee;
        }
        private List<T> Call_sp_InsertUpdatePayFee_SP<T>(SQL_ParametersInsertManagePayFee Params)
        {

            List<T> lstPayFee = new List<T>();
            SqlParameter[] queryParams = new SqlParameter[] {
                //-----------------------ManagePayFee Table ------------------------
                        new SqlParameter("id", Params.Id),
                        new SqlParameter("studentId", Params.StudentId),
                        new SqlParameter("paidOn", Params.PaidOn),
                        new SqlParameter("totalFeeTypeAmount", Params.TotalFeeTypeAmount),
                        new SqlParameter("totalMonths", Params.TotalMonths),
                        new SqlParameter("totalFine", Params.TotalFine),
                        new SqlParameter("totalDiscount", Params.TotalDiscount),
                        new SqlParameter("totalReceiptAmount", Params.TotalReceiptAmount),
                        new SqlParameter("totalPaid", Params.TotalPaid),
                        new SqlParameter("pendingAmount", Params.PendingAmount),
                        new SqlParameter("pendingDate", Params.PendingDate),
                        new SqlParameter("remark", Params.Remark),
                        new SqlParameter("paymentMethodId", Params.PaymentMethodId),
                        new SqlParameter("referenceNumber", Params.ReferenceNumber),
                        new SqlParameter("currentDateVal", Params.CurrentDateVal),
                        new SqlParameter("submittedByLoginId", Params.SubmittedByLoginId),
                        //---------ReceiptFeeType Table ----------------------------
                        new SqlParameter("feeTypeId", Params.FeeTypeId),
                        new SqlParameter("feeTypeName", Params.FeeTypeName),
                        new SqlParameter("feeTypeAmount", Params.FeeTypeAmount),
                        new SqlParameter("monthlyMonthName", Params.MonthlyMonthName),
                        new SqlParameter("transportMonthName", Params.TransportMonthName),
                        new SqlParameter("activityMonthName", Params.ActivityMonthName),
                        new SqlParameter("monthlyFine", Params.MonthlyFine),
                        new SqlParameter("transportFine", Params.TransportFine),
                        new SqlParameter("activityFine", Params.ActivityFine),
                        //--------------------------------------------------------------
                        new SqlParameter("mode", Params.Mode),
                        };
            //
            lstPayFee = db.Database.SqlQuery<T>("exec sp_InsertUpdatePayFee @id,@studentId,@paidOn,@totalFeeTypeAmount,@totalMonths,@totalFine,@totalDiscount,@totalReceiptAmount,@totalPaid,@pendingAmount,@pendingDate,@remark,@paymentMethodId,@referenceNumber,@currentDateVal,@submittedByLoginId,@feeTypeId,@feeTypeName,@feeTypeAmount,@monthlyMonthName,@transportMonthName,@activityMonthName,@monthlyFine,@transportFine,@activityFine,@mode ", queryParams).ToList();
            return lstPayFee;
        }

        [Authorize(Roles = "Admin,Staff")]
        [Route("GetAllFeeTypes")]
        [HttpGet]
        public HttpResponseMessage GetAllFeeTypes()
        {
            try
            {
                //--Get User Identity
                var identity = User.Identity as ClaimsIdentity;

                //--Check if user is authorized user or not
                if (identity != null)
                {
                    List<ClassViewModel> lstFeeType = new List<ClassViewModel>();
                    SQL_ParametersViewModel_VM StudentSQLParameter_VM = new SQL_ParametersViewModel_VM()
                    {
                        Mode = 6,
                    };
                    lstFeeType = Call_ManagePayFee_SP<ClassViewModel>(StudentSQLParameter_VM).ToList();

                    //--Create response
                    var objResponse = new
                    {
                        status = 1,
                        message = "Success",
                        data = new
                        { FeeTypeData = lstFeeType }
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

        //---Get all List of payFee
        [Authorize(Roles = "Admin,Staff")]
        [Route("GetFilterPayFeeData")]
        [HttpGet]
        public HttpResponseMessage GetFilterPayFeeData(int classid, string sessionName)
        {
            try
            {
                //--Get User Identity
                var identity = User.Identity as ClaimsIdentity;

                //--Check if user is authorized user or not
                if (identity != null)
                {
                    List<PayFeeViewModel> lstPayFee = new List<PayFeeViewModel>();
                    var _mode = 0;
                    if (classid != 0 && sessionName != "")
                    {
                        _mode = 4;
                    }
                    else
                    {
                        _mode = classid != 0 ? 3 : sessionName != "" && sessionName != null ? 2 : 1;
                    }
                    SQL_ParametersViewModel_VM PayFeeSQLParameter_VM = new SQL_ParametersViewModel_VM()
                    {
                        ClassId = classid,
                        SessionName = sessionName,
                        Mode = _mode,
                    };
                    lstPayFee = Call_ManagePayFee_SP<PayFeeViewModel>(PayFeeSQLParameter_VM).ToList();
                    //--Create response
                    var objResponse = new
                    {
                        status = 1,
                        message = "Success",
                        data = new
                        { PayFeedata = lstPayFee }
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

        //--Get Student Detail by Id-- 
        [Authorize(Roles = "Admin,Staff")]
        [Route("GetStudentDetailByID")]
        [HttpGet]
        public HttpResponseMessage GetStudentDetailByID(Int64 studentID)
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
                        Mode = 7,
                    };
                    lstStudent = Call_ManagePayFee_SP<StudentViewModel>(StudentSQLParameter_VM).FirstOrDefault();

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
                var objResponse = new { status = -100, message = "Internal Server Error!", data = "", errorMessage = ex.Message.ToString() };
                //sending response as error
                return Request.CreateResponse(HttpStatusCode.InternalServerError, objResponse);
            }
        }

        //--Get Pending Annual amount by studentId-- 
        [Authorize(Roles = "Admin,Staff")]
        [Route("GetPaidMonths")]
        [HttpGet]
        public HttpResponseMessage GetPaidMonths(Int64 studentID)
        {
            try
            {
                //--Get User Identity
                var identity = User.Identity as ClaimsIdentity;

                //--Check if user is authorized user or not
                if (identity != null)
                {
                    List<PayFeeViewModel> result = new List<PayFeeViewModel>();

                    //--Get Pending annual Amount-Detail by Student-ID
                    SQL_ParametersViewModel_VM PayFeeSQLParameter_VM = new SQL_ParametersViewModel_VM()
                    {
                        Id = studentID,
                        Mode = 10,
                    };
                    result = Call_ManagePayFee_SP<PayFeeViewModel>(PayFeeSQLParameter_VM).ToList();
                    if (result.Count == 0)
                    {
                        result = new List<PayFeeViewModel>() {
                            new PayFeeViewModel()
                            {
                                FeeTypeId = 0,
                                FeeTypeName = "",
                                MonthName = "",
                            }
                        };
                    }
                    //--Create response
                    var objResponse = new
                    {
                        status = 1,
                        message = "Success",
                        data = new
                        { payfee = result }
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

        //--Get Pending Annual amount by studentId-- 
        [Authorize(Roles = "Admin,Staff")]
        [Route("GetAnnualPendingAmount")]
        [HttpGet]
        public HttpResponseMessage GetAnnualPendingAmount(Int64 studentID)
        {
            try
            {
                //--Get User Identity
                var identity = User.Identity as ClaimsIdentity;

                //--Check if user is authorized user or not
                if (identity != null)
                {
                    PayFeeViewModel result = new PayFeeViewModel();

                    //--Get Pending annual Amount-Detail by Student-ID
                    SQL_ParametersViewModel_VM PayFeeSQLParameter_VM = new SQL_ParametersViewModel_VM()
                    {
                        Id = studentID,
                        Mode = 9,
                    };
                    result = Call_ManagePayFee_SP<PayFeeViewModel>(PayFeeSQLParameter_VM).FirstOrDefault();
                    if (result == null)
                    {
                        result = new PayFeeViewModel();
                        result.Is_Paid = 1;
                        result.PendingAmount = 0;
                    }
                    //--Create response
                    var objResponse = new
                    {
                        status = 1,
                        message = "Success",
                        data = new
                        { payfee = result }
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
        [Route("GetPayFeeById")]
        [HttpGet]
        public HttpResponseMessage GetPayFeeDetailById(Int64 payFeeID)
        {
            try
            {
                //--Get User Identity
                var identity = User.Identity as ClaimsIdentity;

                //--Check if user is authorized user or not
                if (identity != null)
                {
                    PayFeeViewModel lstPayFee = new PayFeeViewModel();

                    //--Get PayFee-Detail by PayFee-ID
                    SQL_ParametersViewModel_VM PayFeeSQLParameter_VM = new SQL_ParametersViewModel_VM()
                    {
                        Id = payFeeID,
                        Mode = 5,
                    };
                    lstPayFee = Call_ManagePayFee_SP<PayFeeViewModel>(PayFeeSQLParameter_VM).FirstOrDefault();

                    //--Create response
                    var objResponse = new
                    {
                        status = 1,
                        message = "Success",
                        data = new
                        { PayFeedata = lstPayFee }
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

        //--Add New payFee-- 
        [Authorize(Roles = "Admin,Staff")]
        [Route("InsertUpdateManageFee")]
        [HttpPost]
        public HttpResponseMessage InsertUpdateManagePayFee()
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
                        Int64 _id = 0; //Convert.ToInt64(HttpRequest.Params["_Id"]);
                        Int64 _studentId = Convert.ToInt64(HttpRequest.Params["_studentId"]);
                        string _paidOn = (HttpRequest.Params["_paidOn"]);
                        decimal _totalFeeTypeAmount = Convert.ToDecimal(HttpRequest.Params["_totalFeeTypeAmount"]);
                        int _totalMonths = Convert.ToInt32(HttpRequest.Params["_totalMonths"]);
                        decimal _totalFine = Convert.ToDecimal(HttpRequest.Params["_totalFine"]);
                        decimal _totalDiscount = Convert.ToDecimal(HttpRequest.Params["_totalDiscount"]);
                        decimal _totalReceiptAmount = Convert.ToDecimal(HttpRequest.Params["_totalReceiptAmount"]);
                        decimal _totalPaid = Convert.ToDecimal(HttpRequest.Params["_totalPaid"]);
                        decimal _pendingAmount = Convert.ToDecimal(HttpRequest.Params["_pendingAmount"]);
                        string _pendingDate = HttpRequest.Params["_pendingDate"];
                        string _remark = HttpRequest.Params["_remark"];
                        Int64 _paymentMethodId = Convert.ToInt64(HttpRequest.Params["_paymentMethodId"]);
                        string _referenceNumber = HttpRequest.Params["_referenceNumber"];
                        string _currentDateVal = DateTime.UtcNow.ToString("ddMMyy");
                        Int64 _submittedByLoginId = _LoginID_Exact;
                        //--------------ReceiptFeeType Table--------------------------
                        string _feeTypeId = (HttpRequest.Params["_feeTypeId"]);
                        string _feeTypeName = (HttpRequest.Params["_feeTypeName"]);
                        string _feeTypeAmount = HttpRequest.Params["_feeTypeAmount"];
                        string _monthlyMonthName = HttpRequest.Params["_monthlyMonthName"];
                        string _transportMonthName = HttpRequest.Params["_transportMonthName"];
                        string _activityMonthName = HttpRequest.Params["_activityMonthName"];
                        decimal _monthlyFine = Convert.ToDecimal(HttpRequest.Params["_monthlyFine"]);
                        decimal _transportFine = Convert.ToDecimal(HttpRequest.Params["_transportFine"]);
                        decimal _activityFine = Convert.ToDecimal(HttpRequest.Params["_activityFine"]);
                        //--------------------------------------------------------------------
                        int _mode = Convert.ToInt32(HttpRequest.Params["_mode"]);
                        SQL_ParametersInsertManagePayFee ParametersInsertManagePayFee = new SQL_ParametersInsertManagePayFee()
                        {
                            Id = _id,
                            StudentId = _studentId,
                            PaidOn = _paidOn,
                            TotalFeeTypeAmount = _totalFeeTypeAmount,
                            TotalMonths = _totalMonths,
                            TotalFine = _totalFine,
                            TotalDiscount = _totalDiscount,
                            TotalReceiptAmount = _totalReceiptAmount,
                            TotalPaid = _totalPaid,
                            PendingAmount = _pendingAmount,
                            PendingDate = _pendingDate,
                            Remark = _remark,
                            PaymentMethodId = _paymentMethodId,
                            ReferenceNumber = _referenceNumber,
                            CurrentDateVal = _currentDateVal,
                            SubmittedByLoginId = _submittedByLoginId,
                            FeeTypeId = _feeTypeId,
                            FeeTypeName = _feeTypeName,
                            FeeTypeAmount = _feeTypeAmount,
                            MonthlyMonthName = _monthlyMonthName,
                            TransportMonthName = _transportMonthName,
                            ActivityMonthName = _activityMonthName,
                            MonthlyFine = _monthlyFine,
                            TransportFine = _transportFine,
                            ActivityFine = _activityFine,
                            Mode = _mode

                        };
                        _resp = Call_sp_InsertUpdatePayFee_SP<ResponseViewModel>(ParametersInsertManagePayFee).FirstOrDefault();

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

        //-Delete PayFee by PayFee-Id-- 
        [Authorize(Roles = "Admin,Staff")]
        [Route("DeletePayFeeById")]
        [HttpGet]
        public HttpResponseMessage DeletePayFee(Int64 PayFeeID)
        {
            try
            {
                //--Get User Identity
                var identity = User.Identity as ClaimsIdentity;

                //--Check if user is authorized user or not
                if (identity != null)
                {
                    ResponseViewModel _resp = new ResponseViewModel();

                    //--Delete PayFee by PayFee-ID
                    SQL_ParametersViewModel_VM PayFeeSQLParameter_VM = new SQL_ParametersViewModel_VM()
                    {
                        Id = PayFeeID,
                        Mode = 5,
                    };
                    _resp = Call_ManagePayFee_SP<ResponseViewModel>(PayFeeSQLParameter_VM).FirstOrDefault();

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

        //--Print-Data of Pay-Fee Invoice-- 
        [Authorize(Roles = "Admin,Staff")]
        [Route("print/payfee/invoice")]
        [HttpGet]
        public HttpResponseMessage PrintPayFeeInoiveData(Int64 id)
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

                    if (_UserLogin != null)
                    {
                        PayFeeListData_VM lstPayFee = new PayFeeListData_VM();
                        string _PrintData = "";

                        //--Get PayFee-Detail by PayFee-ID
                        SQL_ParametersViewModel_VM PayFeeSQLParameter_VM = new SQL_ParametersViewModel_VM()
                        {
                            Id = id,
                            Mode = 11,
                        };
                        lstPayFee = Call_ManagePayFee_SP<PayFeeListData_VM>(PayFeeSQLParameter_VM).FirstOrDefault();

                        if (lstPayFee != null)
                        {
                            if (lstPayFee.Remarks == null)
                            {
                                lstPayFee.Remarks = "";
                            }

                            #region Create Custom Student-ID (Student-ID/ClassNumber/SectionWord/Session)

                            string Custom_Student_ID = "";

                            if (lstPayFee.StudentId > 0 && lstPayFee.StudentId < 10)
                            {
                                Custom_Student_ID = "0" + lstPayFee.StudentId.ToString();
                            }
                            else
                            {
                                Custom_Student_ID = lstPayFee.StudentId.ToString();
                            }
                            //-----class number like 1st class is 01 ---
                            if (lstPayFee.ClassName != "" && lstPayFee.ClassName != null)
                            {
                                string a = (lstPayFee.ClassName).Trim();
                                var ClassNumber = int.Parse((a).Substring(0, a.Length - 2));
                                if (ClassNumber > 0 && ClassNumber < 10)
                                {
                                    Custom_Student_ID = Custom_Student_ID + "/" + "0" + ClassNumber.ToString();
                                }
                                else
                                {
                                    Custom_Student_ID = Custom_Student_ID + "/" + ClassNumber.ToString();
                                }
                            }
                            //--------section Name like Section A and SectionLastWord is A -------
                            if (lstPayFee.SectionName != "")
                            {
                                string b = (lstPayFee.SectionName).Trim();
                                string SectionLastWord = (b).Substring(b.Length - 1);
                                Custom_Student_ID = Custom_Student_ID + "/" + SectionLastWord;
                            }
                            //------session name------
                            if (lstPayFee.SectionName != "")
                            {
                                string SectionName = (lstPayFee.SectionName).Trim();
                                Custom_Student_ID = Custom_Student_ID + "/" + SectionName;
                            }
                            #endregion

                            //--Get Print-html
                            //_PrintData = objPrint.Get_InvoiceWithIdentityCard_PrintableData(lstPayFee, Custom_Student_ID);
                            _PrintData = objPrint.Get_InvoicePrintableData(lstPayFee);
                        }

                        //--Create response
                        var objResponse = new
                        {
                            status = 1,
                            message = "Success",
                            data = new
                            { invoiceData = _PrintData }
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
                var objResponse = new { status = -100, message = "Internal Server Error!", data = "" ,errorMessage = ex.Message.ToString()};
                //sending response as error
                return Request.CreateResponse(HttpStatusCode.InternalServerError, objResponse);
            }
        }

        //--Send-Data of Pay-Fee Invoice-- 
        [Authorize(Roles = "Admin,Staff")]
        [Route("send/payfee/invoice")]
        [HttpPost]
        public HttpResponseMessage SendPayFeeInoiveData()
        {
            try
            {
                //--Get User Identity
                var identity = User.Identity as ClaimsIdentity;

                //--Check if user is authorized user or not
                if (identity != null)
                {
                    ResponseViewModel _resp = new ResponseViewModel();
                    PayFeeListData_VM lstPayFee = new PayFeeListData_VM();

                    //--Create object of HttpRequest
                    var HttpRequest = HttpContext.Current.Request;

                    //--Get all parameter's value of Form-Data (by Key-Name)
                    Int64 _Id = Convert.ToInt64(HttpRequest.Params["Id"]);
                    string _StudentEmail = HttpRequest.Params["studentEmail"];

                    //--Get Pay-Fee Installment Record by Id
                    SqlParameter[] queryParams_PayFee = new SqlParameter[] {
                    new SqlParameter("id", _Id),
                    new SqlParameter("branchId", "0"),
                    new SqlParameter("from_date", ""),
                    new SqlParameter("to_date", ""),
                    new SqlParameter("studentId", "0"),
                    new SqlParameter("courseId", "0"),
                    new SqlParameter("mode", "4")
                    };
                    lstPayFee = db.Database.SqlQuery<PayFeeListData_VM>("exec sp_ManagePayFee @id,@branchId,@from_date,@to_date,@studentId,@courseId,@mode", queryParams_PayFee).FirstOrDefault();

                    if (lstPayFee != null)
                    {
                        lstPayFee.StudentEmail = _StudentEmail;

                        //--Get Print-html
                        string _InvoiceData = objPrint.Get_InvoicePrintableData(lstPayFee);

                        #region Send Pay-Fee-Invoice Data on Student's Email-Address
                        string _Subject = "Contact us Fee managment";
                        objSend.Send(lstPayFee.StudentName, _Subject, _StudentEmail, _InvoiceData, "");
                        #endregion

                        //--Create response
                        var objResponse = new
                        {
                            status = 1,
                            message = "Invoice has been successfully sent"
                        };

                        //sending response as OK
                        return Request.CreateResponse(HttpStatusCode.OK, objResponse);
                    }
                    else
                    {
                        //--Create response
                        var objResponse = new
                        {
                            status = 0,
                            message = "Sorry, there is some technical issue, please try again later!",
                        };

                        //sending response as OK
                        return Request.CreateResponse(HttpStatusCode.OK, objResponse);
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

    }
}