using SchoolManagementSystem.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace SchoolManagementSystem.DAL
{
    public class SchoolManagementContext : DbContext
    {
        public DbSet<UserLogin> UserLogin { get; set; }
        public DbSet<UserType> UserType { get; set; }
        public DbSet<Staff> Staff { get; set; }
        public DbSet<FieldType> FieldType { get; set; }
        public DbSet<FieldTypeValue> FieldTypeValue { get; set; }
        public DbSet<Session> Session { get; set; }
        public DbSet<ClassDetail> ClassDetail { get; set; }
        public DbSet<ClassSection> ClassSection { get; set; }
        public DbSet<Student> Student { get; set; }
        public DbSet<Admin> Admin { get; set; }
        public DbSet<SessionFee> SessionFee { get; set; }
        public DbSet<PayFeeReceipt> PayFeeReceipt { get; set; }
        public DbSet<PayFeeReceiptNumber> PayFeeReceiptNumber { get; set; }
        public DbSet<ReceiptFeeType> ReceiptFeeType { get; set; }
        public DbSet<School> School { get; set; }
        public DbSet<SuperAdmin> SuperAdmin { get; set; }

    }
}