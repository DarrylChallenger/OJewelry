namespace OJewelry.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class v20_MakeCastingsAndFindingsQtyDecimal : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Castings", "Qty", c => c.Decimal(precision: 18, scale: 2));
            AlterColumn("dbo.StyleFinding", "Qty", c => c.Decimal(precision: 18, scale: 2));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.StyleFinding", "Qty", c => c.Int());
            AlterColumn("dbo.Castings", "Qty", c => c.Int());
        }
    }
}
