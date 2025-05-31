namespace SchoolManagementSystem.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class student_tableUpdate_SessionIDAndSectionId : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Students", "SessionId", c => c.Long(nullable: false));
            AddColumn("dbo.Students", "SectionId", c => c.Long(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Students", "SectionId");
            DropColumn("dbo.Students", "SessionId");
        }
    }
}
