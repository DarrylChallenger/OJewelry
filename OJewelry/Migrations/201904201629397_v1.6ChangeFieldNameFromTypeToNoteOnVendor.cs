namespace OJewelry.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class v16ChangeFieldNameFromTypeToNoteOnVendor : DbMigration
    {
        public override void Up()
        {
            //AddColumn("dbo.Vendors", "Notes", c => c.String(maxLength: 50));
            //DropColumn("dbo.Vendors", "Type");
            RenameColumn("dbo.Vendors", "Type", "Notes");
        }
        
        public override void Down()
        {
            //AddColumn("dbo.Vendors", "Type", c => c.String(maxLength: 50));
            //DropColumn("dbo.Vendors", "Notes");
            RenameColumn("dbo.Vendors", "Notes", "Type");
        }
    }
}
