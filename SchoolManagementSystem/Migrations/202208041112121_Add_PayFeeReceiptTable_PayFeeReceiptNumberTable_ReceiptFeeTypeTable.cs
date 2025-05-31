namespace SchoolManagementSystem.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Add_PayFeeReceiptTable_PayFeeReceiptNumberTable_ReceiptFeeTypeTable : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.PayFeeReceipts",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        StudentId = c.Long(nullable: false),
                        Is_Paid = c.Int(nullable: false),
                        ReceiptNumber = c.String(),
                        ReceiptNumber_Numeric = c.Long(nullable: false),
                        PaidOn_DateTimeFormat = c.DateTime(nullable: false),
                        PaidOn = c.String(),
                        MonthNumber = c.Int(nullable: false),
                        MonthName = c.String(),
                        TotalFeeTypeAmount = c.Decimal(nullable: false, precision: 18, scale: 2),
                        SelectedMonthCount = c.Int(nullable: false),
                        TotalFine = c.Decimal(nullable: false, precision: 18, scale: 2),
                        TotalReceiptAmount = c.Decimal(nullable: false, precision: 18, scale: 2),
                        TotalPaid = c.Decimal(nullable: false, precision: 18, scale: 2),
                        PendingAmount = c.Decimal(nullable: false, precision: 18, scale: 2),
                        PendingDate = c.String(),
                        Remark = c.String(),
                        PaymentMethodId = c.Long(nullable: false),
                        ReferenceNumber = c.String(),
                        CreatedOn = c.DateTime(nullable: false),
                        CreatedByLoginId = c.Long(nullable: false),
                        IsDeleted = c.Int(nullable: false),
                        DeletedOn = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.PayFeeReceiptNumbers",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        LastReceiptNumber = c.Long(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.ReceiptFeeTypes",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        PayFeeReceiptId = c.Long(nullable: false),
                        FeeTypeId = c.Long(nullable: false),
                        FeeTypeName = c.String(),
                        FeeTypeAmount = c.Decimal(nullable: false, precision: 18, scale: 2),
                        CreatedOn = c.DateTime(nullable: false),
                        CreatedByLoginId = c.Long(nullable: false),
                        IsDeleted = c.Int(nullable: false),
                        DeletedOn = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.ReceiptFeeTypes");
            DropTable("dbo.PayFeeReceiptNumbers");
            DropTable("dbo.PayFeeReceipts");
        }
    }
}
