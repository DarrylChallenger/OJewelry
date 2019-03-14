namespace OJewelry.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class v15AddCompanyIdToJTVendSSMC : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.JewelryTypes", "CompanyId", c => c.Int());
            AddColumn("dbo.Vendors", "CompanyId", c => c.Int());
            AddColumn("dbo.Shapes", "CompanyId", c => c.Int());
            AddColumn("dbo.MetalCodes", "CompanyId", c => c.Int());
            CreateIndex("dbo.JewelryTypes", "CompanyId");
            CreateIndex("dbo.Vendors", "CompanyId");
            CreateIndex("dbo.Shapes", "CompanyId");
            CreateIndex("dbo.MetalCodes", "CompanyId");
            AddForeignKey("dbo.JewelryTypes", "CompanyId", "dbo.Companies", "Id");
            AddForeignKey("dbo.Vendors", "CompanyId", "dbo.Companies", "Id");
            AddForeignKey("dbo.Shapes", "CompanyId", "dbo.Companies", "Id");
            AddForeignKey("dbo.MetalCodes", "CompanyId", "dbo.Companies", "Id");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.MetalCodes", "CompanyId", "dbo.Companies");
            DropForeignKey("dbo.Shapes", "CompanyId", "dbo.Companies");
            DropForeignKey("dbo.Vendors", "CompanyId", "dbo.Companies");
            DropForeignKey("dbo.JewelryTypes", "CompanyId", "dbo.Companies");
            DropIndex("dbo.MetalCodes", new[] { "CompanyId" });
            DropIndex("dbo.Shapes", new[] { "CompanyId" });
            DropIndex("dbo.Vendors", new[] { "CompanyId" });
            DropIndex("dbo.JewelryTypes", new[] { "CompanyId" });
            DropColumn("dbo.MetalCodes", "CompanyId");
            DropColumn("dbo.Shapes", "CompanyId");
            DropColumn("dbo.Vendors", "CompanyId");
            DropColumn("dbo.JewelryTypes", "CompanyId");
        }
    }
}
