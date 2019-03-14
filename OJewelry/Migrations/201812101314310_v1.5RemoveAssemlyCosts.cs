namespace OJewelry.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class v15RemoveAssemlyCosts : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.JewelryTypes", "PackagingCost", c => c.Decimal(nullable: false, precision: 18, scale: 2));
            AddColumn("dbo.JewelryTypes", "FinishingCost", c => c.Decimal(nullable: false, precision: 18, scale: 2));
            AddColumn("dbo.MetalCodes", "Market", c => c.Decimal(nullable: false, precision: 18, scale: 2));
            AddColumn("dbo.MetalCodes", "Multiplier", c => c.Single(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.MetalCodes", "Multiplier");
            DropColumn("dbo.MetalCodes", "Market");
            DropColumn("dbo.JewelryTypes", "FinishingCost");
            DropColumn("dbo.JewelryTypes", "PackagingCost");
        }
    }
}
