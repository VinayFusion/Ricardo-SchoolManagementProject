namespace SchoolManagementSystem.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Update_Session_fields : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Sessions", "ClassId", c => c.Long(nullable: false));
            AddColumn("dbo.Sessions", "StartYear", c => c.String());
            AddColumn("dbo.Sessions", "EndYear", c => c.String());
            DropColumn("dbo.Sessions", "StartDate");
            DropColumn("dbo.Sessions", "StartDate_DateTimeFormat");
            DropColumn("dbo.Sessions", "EndDate");
            DropColumn("dbo.Sessions", "EndDate_DateTimeFormat");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Sessions", "EndDate_DateTimeFormat", c => c.DateTime(nullable: false));
            AddColumn("dbo.Sessions", "EndDate", c => c.String());
            AddColumn("dbo.Sessions", "StartDate_DateTimeFormat", c => c.DateTime(nullable: false));
            AddColumn("dbo.Sessions", "StartDate", c => c.String());
            DropColumn("dbo.Sessions", "EndYear");
            DropColumn("dbo.Sessions", "StartYear");
            DropColumn("dbo.Sessions", "ClassId");
        }
    }
}
