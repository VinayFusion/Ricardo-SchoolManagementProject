namespace SchoolManagementSystem.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Add_Class_Table : DbMigration
    {
        public override void Up()
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
            
        }
        
        public override void Down()
        {
            DropTable("dbo.Classes");
        }
    }
}
