namespace SchoolManagementSystem.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddSessionTable : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Sessions",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        SessionName = c.String(),
                        StartDate = c.String(),
                        StartDate_DateTimeFormat = c.DateTime(nullable: false),
                        EndDate = c.String(),
                        EndDate_DateTimeFormat = c.DateTime(nullable: false),
                        Status = c.Int(nullable: false),
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
            DropTable("dbo.Sessions");
        }
    }
}
