using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SchoolManagementSystem.ViewModel
{
    public class SchoolViewModel
    {
        public Int64 Id { get; set; }
        public string SchoolName { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Address { get; set; }
        public string Pincode { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public string PhoneNumber_Only { get; set; }
        public string Password { get; set; }
        public int LoginStatus { get; set; } 
        public int SchoolType { get; set; }
        public string ProfileImage { get; set; }
        public string SchoolLogoImage { get; set; }
    }
}