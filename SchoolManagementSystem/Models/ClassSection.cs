using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace SchoolManagementSystem.Models
{
    public class ClassSection
    {
        [Key]
        public Int64 Id { get; set; }
        public Int64 SessionId { get; set; }
        public Int64 SectionId { get; set; }
        public int Status { get; set; }
        public DateTime CreatedOn { get; set; }
        public Int64 CreatedByLoginId { get; set; }
        public DateTime UpdatedOn { get; set; }
        public Int64 UpdatedByLoginId { get; set; }
    }
}