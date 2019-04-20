namespace OJewelry.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class v16AddQtyToFindings : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Findings", "Qty", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Findings", "Qty");
        }
    }
}
