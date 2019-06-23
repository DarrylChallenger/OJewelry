namespace OJewelry.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class v17AddLaborTable : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.LaborTable",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        CompanyId = c.Int(nullable: false),
                        Name = c.String(nullable: false, maxLength: 100),
                        pph = c.Decimal(precision: 18, scale: 2),
                        ppp = c.Decimal(precision: 18, scale: 2),
                        VendorId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Companies", t => t.CompanyId, cascadeDelete: true)
                .ForeignKey("dbo.Vendors", t => t.VendorId, cascadeDelete: true)
                .Index(t => t.CompanyId)
                .Index(t => t.VendorId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.LaborTable", "VendorId", "dbo.Vendors");
            DropForeignKey("dbo.LaborTable", "CompanyId", "dbo.Companies");
            DropIndex("dbo.LaborTable", new[] { "VendorId" });
            DropIndex("dbo.LaborTable", new[] { "CompanyId" });
            DropTable("dbo.LaborTable");
        }
    }
}
