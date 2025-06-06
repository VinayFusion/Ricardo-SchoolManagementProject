namespace SchoolManagementSystem.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class redo : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.FieldTypes", "DeletedOn", c => c.DateTime(nullable: false));
            AlterColumn("dbo.SuperAdmins", "DeletedOn", c => c.DateTime(nullable: false));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.SuperAdmins", "DeletedOn", c => c.DateTime());
            AlterColumn("dbo.FieldTypes", "DeletedOn", c => c.DateTime());
        }
    }
}
