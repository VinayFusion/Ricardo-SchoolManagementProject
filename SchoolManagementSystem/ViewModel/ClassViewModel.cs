using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SchoolManagementSystem.ViewModel
{
    public class ClassViewModel
    {
        // this use in mode 1 in sp_manageclassDetail and ManageClass.js (get all list)
        public Int64 ClassId { get; set; }
        public string ClassName { get; set; }

        // this use in Mode 4 in sp_manageclassDetail and ManageSection.js Get ddl 
        public Int64 FieldId { get; set; }
        public string FieldTypeId { get; set; }
        public string FieldValue { get; set; }
        public string FieldType { get; set; }
    }
}