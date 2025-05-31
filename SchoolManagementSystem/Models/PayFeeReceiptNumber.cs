using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace SchoolManagementSystem.Models
{
    public class PayFeeReceiptNumber
    {
        [Key]
        public Int64 Id { get; set; }
        public Int64 LastReceiptNumber { get; set; }
    }
}