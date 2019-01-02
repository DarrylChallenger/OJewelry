namespace OJewelry.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class v15AddDefaultStoneVendor : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Companies", "defaultStoneVendor", c => c.Int());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Companies", "defaultStoneVendor");
        }
    }
}
