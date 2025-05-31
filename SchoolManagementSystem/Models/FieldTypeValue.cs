using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace SchoolManagementSystem.Models
{
    public class FieldTypeValue
    {
        [Key]
        public Int64 Id { get; set; }
        public Int64 FieldTypeId { get; set; }
        public string Value { get; set; }
        public int Status { get; set; } //Active or Inavtive
        public DateTime CreatedOn { get; set; }
        public DateTime UpdatedOn { get; set; }
    }
}