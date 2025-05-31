namespace SchoolManagementSystem.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddFieldsInFieldTypeValuesTable : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.FieldTypeValues", "Status", c => c.Int(nullable: false));
            AddColumn("dbo.FieldTypeValues", "CreatedOn", c => c.DateTime(nullable: false));
            AddColumn("dbo.FieldTypeValues", "UpdatedOn", c => c.DateTime(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.FieldTypeValues", "UpdatedOn");
            DropColumn("dbo.FieldTypeValues", "CreatedOn");
            DropColumn("dbo.FieldTypeValues", "Status");
        }
    }
}
