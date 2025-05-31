namespace SchoolManagementSystem.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class update_StudentTable_AddTransport_COL : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Students", "HasTakenTransportService", c => c.Int(nullable: false));
            AddColumn("dbo.Students", "TransportAmount", c => c.Decimal(nullable: false, precision: 18, scale: 2));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Students", "TransportAmount");
            DropColumn("dbo.Students", "HasTakenTransportService");
        }
    }
}
