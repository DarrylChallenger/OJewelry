namespace OJewelry.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class v11_AddStonesShapesFindings : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Components", "CompanyId", "dbo.Companies");
            DropForeignKey("dbo.Components", "ComponentTypeId", "dbo.ComponentTypes");
            DropForeignKey("dbo.Components", "MetalCodeId", "dbo.MetalCodes");
            DropForeignKey("dbo.StyleComponents", "ComponentId", "dbo.Components");
            DropForeignKey("dbo.Components", "VendorId", "dbo.Vendors");
            DropForeignKey("dbo.StyleComponents", "StyleId", "dbo.Styles");
            DropIndex("dbo.StyleComponents", new[] { "StyleId" });
            DropIndex("dbo.StyleComponents", new[] { "ComponentId" });
            DropIndex("dbo.Components", new[] { "CompanyId" });
            DropIndex("dbo.Components", new[] { "ComponentTypeId" });
            DropIndex("dbo.Components", new[] { "VendorId" });
            DropIndex("dbo.Components", new[] { "MetalCodeId" });
            CreateTable(
                "dbo.StyleFinding",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        StyleId = c.Int(nullable: false),
                        FindingId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Findings", t => t.FindingId, cascadeDelete: true)
                .ForeignKey("dbo.Styles", t => t.StyleId, cascadeDelete: true)
                .Index(t => t.StyleId)
                .Index(t => t.FindingId);
            
            CreateTable(
                "dbo.Findings",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        CompanyId = c.Int(),
                        VendorId = c.Int(),
                        Name = c.String(maxLength: 50, unicode: false),
                        Desc = c.String(maxLength: 50),
                        Price = c.Decimal(precision: 19, scale: 4),
                        PricePerHour = c.Decimal(precision: 19, scale: 4),
                        PricePerPiece = c.Decimal(precision: 19, scale: 4),
                        MetalCodeId = c.Int(),
                        Qty = c.Int(),
                        FindingsMetal = c.String(unicode: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Companies", t => t.CompanyId)
                .ForeignKey("dbo.MetalCodes", t => t.MetalCodeId)
                .ForeignKey("dbo.Vendors", t => t.VendorId)
                .Index(t => t.CompanyId)
                .Index(t => t.VendorId)
                .Index(t => t.MetalCodeId);
            
            CreateTable(
                "dbo.StyleStone",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        StyleId = c.Int(nullable: false),
                        StoneId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Stones", t => t.StoneId, cascadeDelete: true)
                .ForeignKey("dbo.Styles", t => t.StyleId, cascadeDelete: true)
                .Index(t => t.StyleId)
                .Index(t => t.StoneId);
            
            CreateTable(
                "dbo.Stones",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        CompanyId = c.Int(),
                        VendorId = c.Int(),
                        Name = c.String(maxLength: 50, unicode: false),
                        Desc = c.String(maxLength: 50),
                        CtWt = c.Int(),
                        StoneSize = c.String(maxLength: 50, unicode: false),
                        ShapeId = c.Int(),
                        Price = c.Decimal(storeType: "money"),
                        Qty = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Companies", t => t.CompanyId)
                .ForeignKey("dbo.Shapes", t => t.ShapeId)
                .ForeignKey("dbo.Vendors", t => t.VendorId)
                .Index(t => t.CompanyId)
                .Index(t => t.VendorId)
                .Index(t => t.ShapeId);
            
            CreateTable(
                "dbo.Shapes",
                c => new
                    {
                        Id = c.Int(nullable: false),
                        Name = c.String(nullable: false, maxLength: 10, unicode: false),
                    })
                .PrimaryKey(t => t.Id);
            
            DropTable("dbo.StyleComponents");
            DropTable("dbo.Components");
            DropTable("dbo.ComponentTypes");
        }
        
        public override void Down()
        {
            CreateTable(
                "dbo.ComponentTypes",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(nullable: false, maxLength: 50, unicode: false),
                        Sequence = c.Int(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Components",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        CompanyId = c.Int(),
                        ComponentTypeId = c.Int(nullable: false),
                        Name = c.String(nullable: false, maxLength: 50, unicode: false),
                        VendorId = c.Int(),
                        Price = c.Decimal(nullable: false, storeType: "money"),
                        PricePerHour = c.Decimal(nullable: false, storeType: "money"),
                        PricePerPiece = c.Decimal(nullable: false, storeType: "money"),
                        MetalLabor = c.Decimal(storeType: "money"),
                        StonesCtWt = c.Int(),
                        StoneSize = c.String(maxLength: 50, unicode: false),
                        StonePPC = c.Decimal(storeType: "money"),
                        MetalCodeId = c.Int(),
                        FindingsMetal = c.String(maxLength: 50, unicode: false),
                        MetalMetal = c.String(maxLength: 50, unicode: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.StyleComponents",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        StyleId = c.Int(nullable: false),
                        ComponentId = c.Int(nullable: false),
                        Quantity = c.Int(),
                    })
                .PrimaryKey(t => t.Id);
            
            DropForeignKey("dbo.StyleStone", "StyleId", "dbo.Styles");
            DropForeignKey("dbo.Stones", "VendorId", "dbo.Vendors");
            DropForeignKey("dbo.StyleStone", "StoneId", "dbo.Stones");
            DropForeignKey("dbo.Stones", "ShapeId", "dbo.Shapes");
            DropForeignKey("dbo.Stones", "CompanyId", "dbo.Companies");
            DropForeignKey("dbo.StyleFinding", "StyleId", "dbo.Styles");
            DropForeignKey("dbo.Findings", "VendorId", "dbo.Vendors");
            DropForeignKey("dbo.StyleFinding", "FindingId", "dbo.Findings");
            DropForeignKey("dbo.Findings", "MetalCodeId", "dbo.MetalCodes");
            DropForeignKey("dbo.Findings", "CompanyId", "dbo.Companies");
            DropIndex("dbo.Stones", new[] { "ShapeId" });
            DropIndex("dbo.Stones", new[] { "VendorId" });
            DropIndex("dbo.Stones", new[] { "CompanyId" });
            DropIndex("dbo.StyleStone", new[] { "StoneId" });
            DropIndex("dbo.StyleStone", new[] { "StyleId" });
            DropIndex("dbo.Findings", new[] { "MetalCodeId" });
            DropIndex("dbo.Findings", new[] { "VendorId" });
            DropIndex("dbo.Findings", new[] { "CompanyId" });
            DropIndex("dbo.StyleFinding", new[] { "FindingId" });
            DropIndex("dbo.StyleFinding", new[] { "StyleId" });
            DropTable("dbo.Shapes");
            DropTable("dbo.Stones");
            DropTable("dbo.StyleStone");
            DropTable("dbo.Findings");
            DropTable("dbo.StyleFinding");
            CreateIndex("dbo.Components", "MetalCodeId");
            CreateIndex("dbo.Components", "VendorId");
            CreateIndex("dbo.Components", "ComponentTypeId");
            CreateIndex("dbo.Components", "CompanyId");
            CreateIndex("dbo.StyleComponents", "ComponentId");
            CreateIndex("dbo.StyleComponents", "StyleId");
            AddForeignKey("dbo.StyleComponents", "StyleId", "dbo.Styles", "Id", cascadeDelete: true);
            AddForeignKey("dbo.Components", "VendorId", "dbo.Vendors", "Id");
            AddForeignKey("dbo.StyleComponents", "ComponentId", "dbo.Components", "Id");
            AddForeignKey("dbo.Components", "MetalCodeId", "dbo.MetalCodes", "Id");
            AddForeignKey("dbo.Components", "ComponentTypeId", "dbo.ComponentTypes", "Id");
            AddForeignKey("dbo.Components", "CompanyId", "dbo.Companies", "Id");
        }
    }
}
