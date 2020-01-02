namespace OJewelry.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class v20ChangeLaborVendorToType : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Labor", "VendorStr", c => c.String(maxLength: 50));
            AddColumn("dbo.Labor", "VendorId", c => c.Int());
            CreateIndex("dbo.Labor", "VendorId");
            AddForeignKey("dbo.Labor", "VendorId", "dbo.Vendors", "Id");
            DropColumn("dbo.Labor", "Vendor");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Labor", "Vendor", c => c.String(maxLength: 50));
            DropForeignKey("dbo.Labor", "VendorId", "dbo.Vendors");
            DropIndex("dbo.Labor", new[] { "VendorId" });
            DropColumn("dbo.Labor", "VendorId");
            DropColumn("dbo.Labor", "VendorStr");
        }
    }
}
