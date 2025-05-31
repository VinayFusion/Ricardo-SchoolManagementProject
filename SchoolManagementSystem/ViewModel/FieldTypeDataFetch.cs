using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SchoolManagementSystem.ViewModel
{
    public class FieldTypeDataFetch
    {
        public Int64 Id { get; set; }
        public Int64 FieldTypeId { get; set; }
        public string TypeName { get; set; }
        public string Value { get; set; }
        public int Status { get; set; } //Active or Inavtive
        public DateTime CreatedOn { get; set; }
        public DateTime UpdatedOn { get; set; }
        public Int64 CreatedByLoginId { get; set; }
        public Int64 UpdatedByLoginId { get; set; }
        public int IsDeleted { get; set; }
        public DateTime DeletedOn { get; set; }
    }
}