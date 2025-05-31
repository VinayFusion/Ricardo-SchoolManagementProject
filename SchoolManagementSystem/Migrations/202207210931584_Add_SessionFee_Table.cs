namespace SchoolManagementSystem.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Add_SessionFee_Table : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.SessionFees",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        ClassId = c.Long(nullable: false),
                        SessionId = c.String(),
                        FeeType = c.String(),
                        FeeAmount = c.Int(nullable: false),
                        Remark = c.String(),
                        CreatedOn = c.DateTime(nullable: false),
                        CreatedByLoginId = c.Long(nullable: false),
                        UpdatedOn = c.DateTime(nullable: false),
                        UpdatedByLoginId = c.Long(nullable: false),
                        IsDeleted = c.Int(nullable: false),
                        DeletedOn = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.SessionFees");
        }
    }
}
