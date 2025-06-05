namespace SchoolManagementSystem.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddColumnsinSchool_Table1 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Schools", "UpdatedOn", c => c.DateTime(nullable: false));
            AddColumn("dbo.Schools", "UpdatedByLoginId", c => c.Long(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Schools", "UpdatedByLoginId");
            DropColumn("dbo.Schools", "UpdatedOn");
        }
    }
}
