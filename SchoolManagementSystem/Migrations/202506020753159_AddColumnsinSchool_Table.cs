namespace SchoolManagementSystem.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddColumnsinSchool_Table : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Schools", "Pincode", c => c.String());
            AddColumn("dbo.Schools", "SchoolType", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Schools", "SchoolType");
            DropColumn("dbo.Schools", "Pincode");
        }
    }
}
