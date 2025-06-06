using SchoolManagementSystem.DAL;
using SchoolManagementSystem.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;

namespace SchoolManagementSystem
{
    public class MvcApplication : System.Web.HttpApplication
    {
        //protected void Application_Start()
        //{
        //    AreaRegistration.RegisterAllAreas();
        //    GlobalConfiguration.Configure(WebApiConfig.Register);
        //    FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
        //    RouteConfig.RegisterRoutes(RouteTable.Routes);
        //    BundleConfig.RegisterBundles(BundleTable.Bundles);

        //    // Setup Initial database values

        //    try
        //    {
        //        SchoolManagementContext db = new SchoolManagementContext();

        //        #region Add Admin/stafftype default entries in database if not exist
        //        //Add Admin Credentials if not in database
        //        var admin = db.UserLogin.Where(u => u.Username == "admin").FirstOrDefault();
        //        if (admin == null)
        //        {
        //            UserLogin adminLogin = new UserLogin()
        //            {
        //                Username = "admin",
        //                Email = "admin@gmail.com",
        //                Password = Common.EDClass.Encrypt("admin123#"),
        //                PhoneNumber = "+910000000000",
        //                CountryPhoneCode_Only = "+91",
        //                PhoneNumber_Only = "0000000000",
        //                IsDefaultPassword = 1,
        //                LoginStatus = 1,
        //                UserTypeId = 2,
        //            };
        //            db.UserLogin.Add(adminLogin);
        //            db.SaveChanges();
        //        }

        //        // Add staff-FieldType if not in database
        //        var fieldType = db.FieldType.Where(f => f.TypeName == "Staff").FirstOrDefault();
        //        if (fieldType == null)
        //        {
        //            FieldType staffFieldType = new FieldType()
        //            {
        //                TypeName = "Staff",
        //                Status = 1,
        //                CreatedOn = DateTime.UtcNow,
        //                UpdatedOn = DateTime.UtcNow,
        //                CreatedByLoginId = 1,
        //                UpdatedByLoginId = 1,
        //                IsDeleted = 0,
        //                DeletedOn = new DateTime(2000, 01, 01)
        //            };
        //            db.FieldType.Add(staffFieldType);
        //            db.SaveChanges();

        //            //Add staff-FieldTypeValues
        //            List<string> staffFieldTypeValues = new List<string>()
        //            {
        //                "Principal",
        //                "Wise Principal",
        //                "Teacher",
        //                "Peon",
        //                "Driver"
        //            };
        //            foreach (var value in staffFieldTypeValues)
        //            {
        //                db.FieldTypeValue.Add(new FieldTypeValue()
        //                {
        //                    FieldTypeId = staffFieldType.Id,
        //                    Value = value,
        //                    Status = 1,
        //                    CreatedOn = DateTime.UtcNow,
        //                    UpdatedOn = DateTime.UtcNow
        //                });
        //            }
        //            db.SaveChanges();
        //        }
        //        #endregion
        //    }
        //    catch (Exception ex)
        //    {
        //        Console.WriteLine(ex);
        //    }

        //}

        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            GlobalConfiguration.Configure(WebApiConfig.Register);
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);

            // Setup Initial database values
            //try
            //{
            //    SchoolManagementContext db = new SchoolManagementContext();

            //    #region Add SuperAdmin Default Entry
            //    var superAdmin = db.UserLogin.FirstOrDefault(u => u.Username == "superadmin");
            //    if (superAdmin == null)
            //    {
            //        UserLogin superAdminLogin = new UserLogin()
            //        {
            //            Username = "superadmin",
            //            Email = "superadmin@gmail.com",
            //            Password = Common.EDClass.Encrypt("superadmin123#"), // Already encrypted password
            //            PhoneNumber = "+919999999999",
            //            CountryPhoneCode_Only = "+91",
            //            PhoneNumber_Only = "9999999999",
            //            IsDefaultPassword = 0,
            //            LoginStatus = 1,
            //            UserTypeId = 1 // SuperAdmin role
            //        };
            //        db.UserLogin.Add(superAdminLogin);
            //        db.SaveChanges();

            //        // Insert into SuperAdmins table
            //        SuperAdmin sa = new SuperAdmin()
            //        {
            //            FirstName = "Super",
            //            LastName = "Admin",
            //            Pincode = "111111",
            //            Address = "Address",
            //            CreatedOn = DateTime.UtcNow,
            //            CreatedByLoginId = superAdminLogin.Id,
            //            LoginId = superAdminLogin.Id,
            //            IsDeleted = 0,
            //            DeletedOn = new DateTime(2000, 01, 01)
            //        };
            //        db.SuperAdmin.Add(sa);
            //        db.SaveChanges();
            //    }
            //    #endregion

            //    #region Add Admin (School) Default Entry
            //    var admin = db.UserLogin.FirstOrDefault(u => u.Username == "admin");
            //    if (admin == null)
            //    {
            //        UserLogin adminLogin = new UserLogin()
            //        {
            //            Username = "admin",
            //            Email = "admin@gmail.com",
            //            Password = Common.EDClass.Encrypt("admin123#"), // Use encryption method
            //            PhoneNumber = "+910000000000",
            //            CountryPhoneCode_Only = "+91",
            //            PhoneNumber_Only = "0000000000",
            //            IsDefaultPassword = 1,
            //            LoginStatus = 1,
            //            UserTypeId = 2 // Admin (School role)
            //        };
            //        db.UserLogin.Add(adminLogin);
            //        db.SaveChanges();

            //        // Add into Schools or Admins table as needed (if you have one)
            //        // You can insert into Schools table similarly using adminLogin.Id
            //    }
            //    #endregion

            //    #region Add FieldType "Staff" and Values if not exist
            //    var fieldType = db.FieldType.FirstOrDefault(f => f.TypeName == "Staff");
            //    if (fieldType == null)
            //    {
            //        FieldType staffFieldType = new FieldType()
            //        {
            //            TypeName = "Staff",
            //            Status = 1,
            //            CreatedOn = DateTime.UtcNow,
            //            UpdatedOn = DateTime.UtcNow,
            //            CreatedByLoginId = 1,
            //            UpdatedByLoginId = 1,
            //            IsDeleted = 0,
            //            DeletedOn = new DateTime(2000, 01, 01)
            //        };
            //        db.FieldType.Add(staffFieldType);
            //        db.SaveChanges();

            //        List<string> staffFieldTypeValues = new List<string>()
            //         {
            //             "Principal",
            //             "Wise Principal",
            //             "Teacher",
            //             "Peon",
            //             "Driver"
            //         };
            //        foreach (var value in staffFieldTypeValues)
            //        {
            //            db.FieldTypeValue.Add(new FieldTypeValue()
            //            {
            //                FieldTypeId = staffFieldType.Id,
            //                Value = value,
            //                Status = 1,
            //                CreatedOn = DateTime.UtcNow,
            //                UpdatedOn = DateTime.UtcNow
            //            });
            //        }
            //        db.SaveChanges();
            //    }
            //    #endregion
            //}
            //catch (Exception ex)
            //{
            //    Console.WriteLine("Error initializing database: " + ex.Message);
            //}
        }
    }
}
