using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace SchoolManagementSystem.Models
{
    public class UserLogin
    {
        [Key]
        public Int64 Id { get; set; }
        public string Username { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public string CountryPhoneCode_Only { get; set; }
        public string PhoneNumber_Only { get; set; }
        public string Password { get; set; }
        public Int64 UserTypeId { get; set; } //Admin, Staff or Student
        public int LoginStatus { get; set; }
        public int IsDefaultPassword { get; set; }
    }
}