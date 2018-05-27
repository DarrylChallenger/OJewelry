namespace OJewelry.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class v652 : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Styles", "Quantity", c => c.Double(nullable: false));
            // need to manually drop DEFAULT constraint on UnitsSold
            AlterColumn("dbo.Styles", "UnitsSold", c => c.Double(nullable: false));  
            AlterColumn("dbo.Memo", "Quantity", c => c.Double(nullable: false));
            AlterColumn("dbo.SalesLedger", "UnitsSold", c => c.Double());
        }
        
        public override void Down()
        {
            AlterColumn("dbo.SalesLedger", "UnitsSold", c => c.Int());
            AlterColumn("dbo.Memo", "Quantity", c => c.Int(nullable: false));
            AlterColumn("dbo.Styles", "UnitsSold", c => c.Int(nullable: false));
            AlterColumn("dbo.Styles", "Quantity", c => c.Int(nullable: false));
        }
    }
}
