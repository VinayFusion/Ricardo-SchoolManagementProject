using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SchoolManagementSystem.ViewModel
{
    public class PayFeeViewModel
    {
        

        // PayFeeReceipt-------------------------
        public Int64 Id { get; set; }
        public Int64 FeeTypeId { get; set; }
        public string SessionName { get; set; }
        public string StudentName { get; set; }
        public string FatherName { get; set; }
        public string FeeTypeName { get; set; }
        public int Is_Paid { get; set; }
        public decimal TotalFeeTypeAmount { get; set; }
        public int SelectedMonthCount { get; set; }
        public decimal TotalFine { get; set; }
        public decimal TotalReceiptAmount { get; set; }
        public decimal TotalPaid { get; set; }
        public string PaidOn { get; set; }
        public string MonthName { get; set; }
        public int MonthNumber { get; set; }
        public decimal PendingAmount { get; set; }
        public string PendingDate { get; set; }
        public string Remark { get; set; }
      
       
    }

    public class PayFeeListData_VM
    {
        public Int64 Id { get; set; }
        public Int64 PayFeeId { get; set; }
        public string ReceiptNumber { get; set; }
        public string PaidOn { get; set; }
        public string MonthsName { get; set; }
        public decimal TotalReceiptAmount { get; set; }
        public decimal TotalFine { get; set; }
        public decimal TotalDiscount { get; set; }
        public decimal PendingAmount { get; set; }
        public decimal TotalPaid { get; set; }
        public string Remarks { get; set; }
        public string RegisteredBy { get; set; }

        //------------Student details ----------------
        public Int64 StudentId { get; set; }
        public string StudentName { get; set; }
        public string StudentEmail { get; set; }
        public string StudentMobileNumber { get; set; }
        public string Student_FatherName { get; set; }
        public string Student_MotherName { get; set; }
        public string Student_ProfileImage { get; set; }
        public string ClassName { get; set; }
        public string SectionName { get; set; }
        public string SessionName { get; set; }

        //-------------school deatils ------------------
        public string BranchName { get; set; } = "";
        public string BranchAddress { get; set; } = "";
        public string BranchMobileNumber { get; set; } = "";
    }
}