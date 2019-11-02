namespace OJewelry.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class v20AddVendorToLaborAndMisc : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Labor", "Vendor", c => c.String(maxLength: 50));
            AddColumn("dbo.Misc", "Vendor", c => c.String(maxLength: 50));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Misc", "Vendor");
            DropColumn("dbo.Labor", "Vendor");
        }
    }
}
