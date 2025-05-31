using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace SchoolManagementSystem.Models
{
    public class UserType
    {
        [Key]
        public Int64 Id { get; set; }
        public string UserTypeName { get; set; } // (Admin, Staff, Student)
        public int Status { get; set; }
    }
}