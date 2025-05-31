namespace SchoolManagementSystem.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class update_1 : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.UserLogins",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        Username = c.String(),
                        Email = c.String(),
                        PhoneNumber = c.String(),
                        CountryPhoneCode_Only = c.String(),
                        PhoneNumber_Only = c.String(),
                        Password = c.String(),
                        UserTypeId = c.Long(nullable: false),
                        LoginStatus = c.Int(nullable: false),
                        IsDefaultPassword = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.UserTypes",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        UserTypeName = c.String(),
                        Status = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.UserTypes");
            DropTable("dbo.UserLogins");
        }
    }
}
