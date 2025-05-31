namespace SchoolManagementSystem.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Add_ClassSection_Table_and_RemoveSection_In_ClassTable : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.ClassDetails",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        SessionId = c.Long(nullable: false),
                        ClassName = c.String(),
                        CreatedOn = c.DateTime(nullable: false),
                        CreatedByLoginId = c.Long(nullable: false),
                        UpdatedOn = c.DateTime(nullable: false),
                        UpdatedByLoginId = c.Long(nullable: false),
                        IsDeleted = c.Int(nullable: false),
                        DeletedOn = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.ClassSections",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        ClassId = c.Long(nullable: false),
                        SectionId = c.Long(nullable: false),
                        Status = c.Int(nullable: false),
                        CreatedOn = c.DateTime(nullable: false),
                        CreatedByLoginId = c.Long(nullable: false),
                        UpdatedOn = c.DateTime(nullable: false),
                        UpdatedByLoginId = c.Long(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            DropTable("dbo.Classes");
        }
        
        public override void Down()
        {
            CreateTable(
                "dbo.Classes",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        SessionId = c.Long(nullable: false),
                        ClassName = c.String(),
                        Section_A = c.Int(nullable: false),
                        Section_B = c.Int(nullable: false),
                        Section_C = c.Int(nullable: false),
                        Section_D = c.Int(nullable: false),
                        Section_E = c.Int(nullable: false),
                        CreatedOn = c.DateTime(nullable: false),
                        CreatedByLoginId = c.Long(nullable: false),
                        UpdatedOn = c.DateTime(nullable: false),
                        UpdatedByLoginId = c.Long(nullable: false),
                        IsDeleted = c.Int(nullable: false),
                        DeletedOn = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            DropTable("dbo.ClassSections");
            DropTable("dbo.ClassDetails");
        }
    }
}
