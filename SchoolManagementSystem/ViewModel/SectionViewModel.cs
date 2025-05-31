using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace SchoolManagementSystem.ViewModel
{
    public class SectionViewModel 
    {
        public Int64 Id { get; set; }
        public string SectionId { get; set; }
        public string SectionName { get; set; }
        public int Status { get; set; }
        public Int64 SessionId { get; set; }
        public string SessionName { get; set; }

        public Int64 ClassId { get; set; }
        public string FeeId { get; set; }
        public string FeeName { get; set; }
        public string FeeAmount { get; set; }


    }
}