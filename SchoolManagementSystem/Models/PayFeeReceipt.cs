using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace SchoolManagementSystem.Models
{
    public class PayFeeReceipt
    {
        [Key]
        public Int64 Id { get; set; }
        public Int64 StudentId { get; set; }
        public int  Is_Paid { get; set; }
        public string ReceiptNumber { get; set; }
        public Int64 ReceiptNumber_Numeric { get; set; }
        public DateTime PaidOn_DateTimeFormat { get; set; }
        public string PaidOn { get; set; }
        public decimal TotalFeeTypeAmount { get; set; } 
        public int SelectedMonthCount { get; set; } 
        public decimal TotalFine { get; set; }
        public decimal TotalDiscount { get; set; }
        public decimal TotalReceiptAmount { get; set; }
        public decimal TotalPaid { get; set; }
        public decimal PendingAmount { get; set; }
        public string PendingDate { get; set; }
        public string Remark { get; set; }
        public Int64 PaymentMethodId { get; set; }
        public string ReferenceNumber { get; set; }

        public DateTime CreatedOn { get; set; }
        public Int64 CreatedByLoginId { get; set; }
        public int IsDeleted { get; set; }
        public DateTime DeletedOn { get; set; }
    }

    

}