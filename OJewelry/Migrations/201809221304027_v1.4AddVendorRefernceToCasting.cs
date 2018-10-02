namespace OJewelry.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class v14AddVendorRefernceToCasting : DbMigration
    {
        public override void Up()
        {
            CreateIndex("dbo.Castings", "VendorId");
            AddForeignKey("dbo.Castings", "VendorId", "dbo.Vendors", "Id");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Castings", "VendorId", "dbo.Vendors");
            DropIndex("dbo.Castings", new[] { "VendorId" });
        }
    }
}
