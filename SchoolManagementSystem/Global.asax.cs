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
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            GlobalConfiguration.Configure(WebApiConfig.Register);
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);

            // Setup Initial database values

            try
            {
                SchoolManagementContext db = new SchoolManagementContext();

                #region Add Admin/stafftype default entries in database if not exist
                //Add Admin Credentials if not in database
                var admin = db.UserLogin.Where(u => u.Username == "admin").FirstOrDefault();
                if(admin == null)
                {
                    UserLogin adminLogin = new UserLogin()
                    {
                        Username = "admin",
                        Email = "admin@gmail.com",
                        Password = Common.EDClass.Encrypt("admin123#"),
                        PhoneNumber = "+910000000000",
                        CountryPhoneCode_Only = "+91",
                        PhoneNumber_Only = "0000000000",
                        IsDefaultPassword = 1,
                        LoginStatus = 1,
                        UserTypeId = 1,
                    };
                    db.UserLogin.Add(adminLogin);
                    db.SaveChanges();
                }
                
                // Add staff-FieldType if not in database
                var fieldType = db.FieldType.Where(f => f.TypeName == "Staff").FirstOrDefault();
                if(fieldType == null)
                {
                    FieldType staffFieldType = new FieldType()
                    {
                        TypeName = "Staff",
                        Status = 1,
                        CreatedOn = DateTime.UtcNow,
                        UpdatedOn = DateTime.UtcNow,
                        CreatedByLoginId = 1,
                        UpdatedByLoginId = 1,
                        IsDeleted = 0,
                        DeletedOn = new DateTime(2000, 01, 01)
                    };
                    db.FieldType.Add(staffFieldType);
                    db.SaveChanges();

                    //Add staff-FieldTypeValues
                    List<string> staffFieldTypeValues = new List<string>()
                    {
                        "Principal",
                        "Wise Principal",
                        "Teacher",
                        "Peon",
                        "Driver"
                    };
                    foreach(var value in staffFieldTypeValues)
                    {
                        db.FieldTypeValue.Add(new FieldTypeValue()
                        {
                            FieldTypeId = staffFieldType.Id,
                            Value = value,
                            Status = 1,
                            CreatedOn = DateTime.UtcNow,
                            UpdatedOn = DateTime.UtcNow
                        });
                    }
                    db.SaveChanges();
                }
                #endregion
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }

        }
    }
}
