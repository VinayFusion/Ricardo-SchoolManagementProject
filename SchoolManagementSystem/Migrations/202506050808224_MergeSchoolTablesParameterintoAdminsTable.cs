namespace SchoolManagementSystem.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class MergeSchoolTablesParameterintoAdminsTable : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Admins", "SchoolName", c => c.String());
            AddColumn("dbo.Admins", "SchoolType", c => c.Int(nullable: false));
            AddColumn("dbo.Admins", "SchoolLogoImage", c => c.String());
            DropTable("dbo.Schools");
        }
        
        public override void Down()
        {
            CreateTable(
                "dbo.Schools",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        LoginId = c.Long(nullable: false),
                        SchoolName = c.String(),
                        Address = c.String(),
                        Pincode = c.String(),
                        SchoolType = c.Int(nullable: false),
                        ProfileImage = c.String(),
                        CreatedOn = c.DateTime(nullable: false),
                        CreatedByLoginId = c.Long(nullable: false),
                        UpdatedOn = c.DateTime(nullable: false),
                        UpdatedByLoginId = c.Long(nullable: false),
                        IsDeleted = c.Int(nullable: false),
                        DeletedOn = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            DropColumn("dbo.Admins", "SchoolLogoImage");
            DropColumn("dbo.Admins", "SchoolType");
            DropColumn("dbo.Admins", "SchoolName");
        }
    }
}
