using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace SchoolManagementSystem.Models
{
    public class Student
    {
        [Key]
        public Int64 Id { get; set; }
        public Int64 LoginId { get; set; }
        public Int64 SessionId { get; set; }
        public Int64 SectionId { get; set; }

        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Gender { get; set; }
        public string DateOfBirth { get; set; }
        public string BloodGroup { get; set; } 
        public string FatherName { get; set; }
        public string MotherName { get; set; }

        // Address Details
        public string Pincode { get; set; }
        public string Address { get; set; }

        public string ProfileImage { get; set; }
        public int HasTakenTransportService { get; set; }
        public decimal TransportAmount { get; set; }

        public DateTime CreatedOn { get; set; }
        public Int64 CreatedByLoginId { get; set; }
        public DateTime UpdatedOn { get; set; }
        public Int64 UpdatedByLoginId { get; set; }
        public int IsDeleted { get; set; }
        public DateTime DeletedOn { get; set; }
    }
}