namespace OJewelry.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class v17UpdateLaborTableDBModel : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.StyleLaborTable", "Qty", c => c.Decimal(nullable: false, precision: 18, scale: 2));
        }
        
        public override void Down()
        {
            DropColumn("dbo.StyleLaborTable", "Qty");
        }
    }
}
