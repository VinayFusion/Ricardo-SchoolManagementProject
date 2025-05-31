using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SchoolManagementSystem.ViewModel
{
    public class SQL_ParametersViewModel_VM
    {
        public Int64 Id { get; set; }
        public Int64 ClassId { get; set; }
        public Int64 SessionId { get; set; }
        public string SessionName { get; set; } = "";
        public int Mode { get; set; }
    }
    public class SQL_ParametersInsertManagePayFee
    {
        //--------------ManagePayFee Parameters --------------------
        public Int64 Id { get; set; }
        public Int64 StudentId { get; set; }
        public string PaidOn { get; set; }
        public decimal TotalFeeTypeAmount { get; set; }
        public int TotalMonths { get; set; }
        public decimal TotalFine { get; set; }
        public decimal TotalDiscount { get; set; }
        public decimal TotalReceiptAmount { get; set; }
        public decimal TotalPaid { get; set; }
        public decimal PendingAmount { get; set; }
        public string PendingDate { get; set; } = "";
        public string Remark { get; set; } = "";
        public Int64 PaymentMethodId { get; set; }
        public string ReferenceNumber { get; set; } = "";
        public string CurrentDateVal { get; set; }
        public Int64 SubmittedByLoginId { get; set; }
        //----------------------------------------------------------
        //------------------ReceiptFeeType Parameters----------------
        public string FeeTypeId { get; set; }
        public string FeeTypeName { get; set; }
        public string FeeTypeAmount { get; set; }
        public string MonthlyMonthName { get; set; } = "";
        public string TransportMonthName { get; set; } = "";
        public string ActivityMonthName { get; set; } = "";
        public decimal MonthlyFine { get; set; }
        public decimal TransportFine { get; set; }
        public decimal ActivityFine { get; set; }
        //-----------------------------------------------------------
        public int Mode { get; set; }
    }
}