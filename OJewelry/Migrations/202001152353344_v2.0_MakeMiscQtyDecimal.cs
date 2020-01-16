namespace OJewelry.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class v20_MakeMiscQtyDecimal : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Misc", "Qty", c => c.Decimal(precision: 18, scale: 2));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Misc", "Qty", c => c.Int());
        }
    }
}
