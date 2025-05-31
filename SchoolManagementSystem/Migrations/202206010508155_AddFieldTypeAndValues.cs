namespace SchoolManagementSystem.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddFieldTypeAndValues : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.FieldTypes",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        TypeName = c.String(),
                        Status = c.Int(nullable: false),
                        CreatedOn = c.DateTime(nullable: false),
                        CreatedByLoginId = c.Long(nullable: false),
                        UpdatedOn = c.DateTime(nullable: false),
                        UpdatedByLoginId = c.Long(nullable: false),
                        IsDeleted = c.Int(nullable: false),
                        DeletedOn = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.FieldTypeValues",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        FieldTypeId = c.Long(nullable: false),
                        Value = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            AddColumn("dbo.Staffs", "StaffFieldTypeValueId", c => c.Long(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Staffs", "StaffFieldTypeValueId");
            DropTable("dbo.FieldTypeValues");
            DropTable("dbo.FieldTypes");
        }
    }
}
