namespace SchoolManagementSystem.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Modify_SessionId_in_SessionFee : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.SessionFees", "SessionId", c => c.Long(nullable: false));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.SessionFees", "SessionId", c => c.String());
        }
    }
}
