namespace OJewelry.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class v17MakeLaborQtyFractional : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Labor", "Qty", c => c.Decimal(precision: 18, scale: 2));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Labor", "Qty", c => c.Int());
        }
    }
}
