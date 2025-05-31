using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace SchoolManagementSystem.Models
{
    public class Session
    {
        [Key]
        public Int64 Id { get; set; }
        public Int64 ClassId { get; set; }
        public string SessionName { get; set; }
        public string StartYear { get; set; }
        public string EndYear { get; set; }
        public int Status { get; set; }
        public DateTime CreatedOn { get; set; }
        public Int64 CreatedByLoginId { get; set; }
        public DateTime UpdatedOn { get; set; }
        public Int64 UpdatedByLoginId { get; set; }
        public int IsDeleted { get; set; }
        public DateTime DeletedOn { get; set; }

    }
}