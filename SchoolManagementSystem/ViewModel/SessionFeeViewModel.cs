using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SchoolManagementSystem.ViewModel
{
    public class SessionFeeViewModel
    {
        public Int64 Id { get; set; }
        public Int64 ClassId { get; set; }
        public string ClassName { get; set; }
        public Int64 SessionId { get; set; }
        public string SessionName { get; set; }
        public int FeeTypeId { get; set; }
        public string FeeTypeName { get; set; }
        public int FeeAmount { get; set; }
        public string Remark { get; set; }
    }


}