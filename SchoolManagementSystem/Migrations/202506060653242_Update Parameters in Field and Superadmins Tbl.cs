namespace SchoolManagementSystem.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class UpdateParametersinFieldandSuperadminsTbl : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.FieldTypes", "DeletedOn", c => c.DateTime());
            AlterColumn("dbo.SuperAdmins", "DeletedOn", c => c.DateTime());
        }
        
        public override void Down()
        {
            AlterColumn("dbo.SuperAdmins", "DeletedOn", c => c.DateTime(nullable: false));
            AlterColumn("dbo.FieldTypes", "DeletedOn", c => c.DateTime(nullable: false));
        }
    }
}
