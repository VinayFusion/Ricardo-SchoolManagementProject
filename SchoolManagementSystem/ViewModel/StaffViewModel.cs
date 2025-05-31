using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SchoolManagementSystem.ViewModel
{
    public class StaffViewModel
    {
        public Int64 Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Pincode { get; set; }
        public string Address { get; set; }
        public DateTime CreatedOn { get; set; }
        public Int64 CreatedByLoginId { get; set; }
        public DateTime UpdatedOn { get; set; }
        public Int64 UpdatedByLoginId { get; set; }
        public Int64 LoginId { get; set; }
        
        public string ProfileImage { get; set; }

        public string Gender { get; set; }
        public decimal? WorkExperienceInYears { get; set; }
        public decimal Salary { get; set; }
        public string JoiningDate { get; set; }
        public DateTime JoiningDate_DateTime { get; set; }

        //--Login Detail
        public string Username { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public string CountryPhoneCode_Only { get; set; }
        public string PhoneNumber_Only { get; set; }
        public string Password { get; set; }
        public Int64 UserTypeId { get; set; } //Admin, Staff or Student
        public int LoginStatus { get; set; }

        public Int64 StaffFieldTypeValueId { get; set; }
        public string StaffTypeName { get; set; }
    }

}