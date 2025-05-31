namespace SchoolManagementSystem.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class update_ManagePayFee_and_ReceiptFeeType_Table : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.PayFeeReceipts", "TotalDiscount", c => c.Decimal(nullable: false, precision: 18, scale: 2));
            AddColumn("dbo.ReceiptFeeTypes", "MonthName", c => c.String());
            AddColumn("dbo.ReceiptFeeTypes", "MonthYear", c => c.String());
            AddColumn("dbo.ReceiptFeeTypes", "Fine", c => c.Int(nullable: false));
            DropColumn("dbo.PayFeeReceipts", "MonthNumber");
            DropColumn("dbo.PayFeeReceipts", "MonthName");
        }
        
        public override void Down()
        {
            AddColumn("dbo.PayFeeReceipts", "MonthName", c => c.String());
            AddColumn("dbo.PayFeeReceipts", "MonthNumber", c => c.Int(nullable: false));
            DropColumn("dbo.ReceiptFeeTypes", "Fine");
            DropColumn("dbo.ReceiptFeeTypes", "MonthYear");
            DropColumn("dbo.ReceiptFeeTypes", "MonthName");
            DropColumn("dbo.PayFeeReceipts", "TotalDiscount");
        }
    }
}
