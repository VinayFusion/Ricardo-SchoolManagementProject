using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SchoolManagementSystem.ViewModel
{
    public class ResponseViewModel
    {
        public int ret { get; set; }
        public string responseMessage { get; set; }
        public string PreviousProfileImage { get; set; }
    }

    public class JsonResponseViewModel
    {
        public int status { get; set; }
        public string message { get; set; }
        public object data { get; set; }
    }
}