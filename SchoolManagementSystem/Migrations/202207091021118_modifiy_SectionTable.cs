namespace SchoolManagementSystem.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class modifiy_SectionTable : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.ClassSections", "SessionId", c => c.Long(nullable: false));
            DropColumn("dbo.ClassSections", "ClassId");
        }
        
        public override void Down()
        {
            AddColumn("dbo.ClassSections", "ClassId", c => c.Long(nullable: false));
            DropColumn("dbo.ClassSections", "SessionId");
        }
    }
}
