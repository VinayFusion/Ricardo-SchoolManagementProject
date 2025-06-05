namespace SchoolManagementSystem.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddColumnSchool_Table01 : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Schools", "SchoolType", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Schools", "SchoolType", c => c.String());
        }
    }
}
