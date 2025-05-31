namespace SchoolManagementSystem.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class update_2 : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Staffs",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        LoginId = c.Long(nullable: false),
                        StaffTypeId = c.Long(nullable: false),
                        FirstName = c.String(),
                        LastName = c.String(),
                        Pincode = c.String(),
                        Address = c.String(),
                        ProfileImage = c.String(),
                        Gender = c.String(),
                        WorkExperienceInYears = c.Decimal(precision: 18, scale: 2),
                        Salary = c.Decimal(nullable: false, precision: 18, scale: 2),
                        JoiningDate = c.String(),
                        JoiningDate_DateTime = c.DateTime(nullable: false),
                        CreatedOn = c.DateTime(nullable: false),
                        CreatedByLoginId = c.Long(nullable: false),
                        UpdatedOn = c.DateTime(nullable: false),
                        UpdatedByLoginId = c.Long(nullable: false),
                        IsDeleted = c.Int(nullable: false),
                        DeletedOn = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.StaffTypes",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        Name = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.StaffTypes");
            DropTable("dbo.Staffs");
        }
    }
}
