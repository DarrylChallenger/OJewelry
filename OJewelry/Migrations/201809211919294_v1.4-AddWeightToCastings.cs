namespace OJewelry.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class v14AddWeightToCastings : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Castings", "MetalWeight", c => c.Decimal(nullable: false, precision: 18, scale: 2, defaultValue: 1));
            AddColumn("dbo.Castings", "MetalWtUnitId", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Castings", "MetalWtUnitId");
            DropColumn("dbo.Castings", "MetalWeight");
        }
    }
}
