namespace SchoolManagementSystem.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Modify_FeeType_In_SessionFee : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.SessionFees", "FeeTypeId", c => c.Int(nullable: false));
            DropColumn("dbo.SessionFees", "FeeType");
        }
        
        public override void Down()
        {
            AddColumn("dbo.SessionFees", "FeeType", c => c.String());
            DropColumn("dbo.SessionFees", "FeeTypeId");
        }
    }
}
