namespace SchoolManagementSystem.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class RemoveParameterFromSchoolTbl : DbMigration
    {
        public override void Up()
        {
            DropColumn("dbo.Schools", "Email");
            DropColumn("dbo.Schools", "ContactNumber");
            DropColumn("dbo.Schools", "Password");
            DropColumn("dbo.Schools", "Status");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Schools", "Status", c => c.Int(nullable: false));
            AddColumn("dbo.Schools", "Password", c => c.String());
            AddColumn("dbo.Schools", "ContactNumber", c => c.String());
            AddColumn("dbo.Schools", "Email", c => c.String());
        }
    }
}
