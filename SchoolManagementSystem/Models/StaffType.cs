using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace SchoolManagementSystem.Models
{
    public class StaffType
    {
        [Key]
        public Int64 Id { get; set; }
        public string Name { get; set; }

    }
}