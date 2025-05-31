namespace SchoolManagementSystem.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Add_AdminTable : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Admins",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        FirstName = c.String(),
                        LastName = c.String(),
                        Pincode = c.String(),
                        Address = c.String(),
                        CreatedOn = c.DateTime(nullable: false),
                        CreatedByLoginId = c.Long(nullable: false),
                        UpdatedOn = c.DateTime(nullable: false),
                        UpdatedByLoginId = c.Long(nullable: false),
                        LoginId = c.Long(nullable: false),
                        IsDeleted = c.Int(nullable: false),
                        DeletedOn = c.DateTime(nullable: false),
                        ProfileImage = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.Admins");
        }
    }
}
