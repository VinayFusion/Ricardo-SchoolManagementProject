using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SchoolManagementSystem.ViewModel
{
    public class StudentViewModel
    {
        public Int64 LoginId { get; set; }

        // student Class ,session,Section Details-----
        public Int64 SessionId { get; set; }
        public string SessionName { get; set; }
        public Int64 SectionId { get; set; }
        public string SectionName { get; set; }
        public Int64 ClassId { get; set; }
        public string ClassName { get; set; }
        //---------------------------------------------

        // student detail
        public Int64 Id { get; set; }
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

        //--Login Detail
        public string Username { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public string CountryPhoneCode_Only { get; set; }
        public string PhoneNumber_Only { get; set; }
        public string Password { get; set; }
        public Int64 UserTypeId { get; set; } //Admin, Staff or Student
        public int LoginStatus { get; set; }


        //---------------------
        public string MonthName { get; set; }
        public int MonthNumber { get; set; }
        public int TotalMonth { get; set; }
        //-----------------------

        //---------this field use in ManagePayFee ---
        public string FeeId { get; set; }
        public string FeeName { get; set; }
        public string FeeAmount { get; set; }
        public decimal PendingAmount { get; set; }
        //---------------------------------------------


    }


}