namespace SchoolManagementSystem.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class RemoveStaffType : DbMigration
    {
        public override void Up()
        {
            DropColumn("dbo.Staffs", "StaffTypeId");
            DropTable("dbo.StaffTypes");
        }
        
        public override void Down()
        {
            CreateTable(
                "dbo.StaffTypes",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        Name = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            AddColumn("dbo.Staffs", "StaffTypeId", c => c.Long(nullable: false));
        }
    }
}
