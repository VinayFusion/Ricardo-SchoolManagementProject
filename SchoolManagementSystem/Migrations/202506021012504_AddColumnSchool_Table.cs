namespace SchoolManagementSystem.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddColumnSchool_Table : DbMigration
    {
        public override void Up()
        {
            DropPrimaryKey("dbo.Schools");
            AddColumn("dbo.Schools", "LoginId", c => c.Long(nullable: false));
            AddColumn("dbo.Schools", "ProfileImage", c => c.String());
            AlterColumn("dbo.Schools", "Id", c => c.Long(nullable: false, identity: true));
            AddPrimaryKey("dbo.Schools", "Id");
        }
        
        public override void Down()
        {
            DropPrimaryKey("dbo.Schools");
            AlterColumn("dbo.Schools", "Id", c => c.Int(nullable: false, identity: true));
            DropColumn("dbo.Schools", "ProfileImage");
            DropColumn("dbo.Schools", "LoginId");
            AddPrimaryKey("dbo.Schools", "Id");
        }
    }
}
