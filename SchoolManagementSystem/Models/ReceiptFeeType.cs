using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace SchoolManagementSystem.Models
{
    public class ReceiptFeeType
    {
        [Key]
        public Int64 Id { get; set; }      
        public Int64 PayFeeReceiptId { get; set; }
        public Int64 FeeTypeId { get; set; }
        public string FeeTypeName { get; set; }
        public decimal FeeTypeAmount { get; set; }
        public string MonthName { get; set; }
        public string MonthYear { get; set; }
        public int Fine { get; set; }


        public DateTime CreatedOn { get; set; }
        public Int64 CreatedByLoginId { get; set; }
        public int IsDeleted { get; set; }
        public DateTime DeletedOn { get; set; }

    }
}