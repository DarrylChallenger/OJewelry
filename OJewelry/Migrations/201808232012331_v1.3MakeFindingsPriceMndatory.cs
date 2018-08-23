namespace OJewelry.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class v13MakeFindingsPriceMndatory : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Findings", "Price", c => c.Decimal(nullable: false, precision: 19, scale: 4));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Findings", "Price", c => c.Decimal(precision: 19, scale: 4));
        }
    }
}
