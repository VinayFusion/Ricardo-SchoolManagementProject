using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace SchoolManagementSystem.Models
{
    public class Staff
    {
        [Key]
        public Int64 Id { get; set; }
        public Int64 LoginId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Pincode { get; set; }
        public string Address { get; set; }
        public string ProfileImage { get; set; }
        public string Gender { get; set; }
        public decimal? WorkExperienceInYears { get; set; }
        public decimal Salary { get; set; }
        public string JoiningDate { get; set; }
        public DateTime JoiningDate_DateTime { get; set; }

        public DateTime CreatedOn { get; set; }
        public Int64 CreatedByLoginId { get; set; }
        public DateTime UpdatedOn { get; set; }
        public Int64 UpdatedByLoginId { get; set; }
        public int IsDeleted { get; set; }
        public DateTime DeletedOn { get; set; }

        public Int64 StaffFieldTypeValueId { get; set; }
    }
}