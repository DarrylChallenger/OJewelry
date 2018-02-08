namespace OJewelry.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class v45 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Collections", "Notes", c => c.String(maxLength: 512));
            AddColumn("dbo.Styles", "RetailPrice", c => c.Decimal(precision: 18, scale: 2));
            AlterColumn("dbo.SalesLedger", "Id", c => c.Int(nullable: false, identity: true));
            AlterColumn("dbo.SalesLedger", "Notes", c => c.String(maxLength: 512, fixedLength: true));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.SalesLedger", "Notes", c => c.String(maxLength: 10, fixedLength: true));
            AlterColumn("dbo.SalesLedger", "Id", c => c.Int(nullable: false));
            DropColumn("dbo.Styles", "RetailPrice");
            DropColumn("dbo.Collections", "Notes");
        }
    }
}
