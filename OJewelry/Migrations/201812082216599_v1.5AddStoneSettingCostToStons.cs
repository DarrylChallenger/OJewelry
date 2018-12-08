namespace OJewelry.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class v15AddStoneSettingCostToStones : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Stones", "SettingCost", c => c.Decimal(nullable: false, storeType: "money"));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Stones", "SettingCost");
        }
    }
}
