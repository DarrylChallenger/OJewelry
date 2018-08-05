namespace OJewelry.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class v11addfindingweight : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Findings", "Weight", c => c.Decimal(precision: 18, scale: 2));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Findings", "Weight");
        }
    }
}
