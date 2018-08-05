namespace OJewelry.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class v11makepricemandatory : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Stones", "Name", c => c.String(nullable: false, maxLength: 50, unicode: false));
            AlterColumn("dbo.Stones", "StoneSize", c => c.String(nullable: false, maxLength: 50, unicode: false));
            AlterColumn("dbo.Stones", "Price", c => c.Decimal(nullable: false, storeType: "money"));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Stones", "Price", c => c.Decimal(storeType: "money"));
            AlterColumn("dbo.Stones", "StoneSize", c => c.String(maxLength: 50, unicode: false));
            AlterColumn("dbo.Stones", "Name", c => c.String(maxLength: 50, unicode: false));
        }
    }
}
