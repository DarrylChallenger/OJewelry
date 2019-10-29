namespace OJewelry.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class v20_AddVendorTypeEnum : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Vendors", "TypeId", "dbo.VendorType");
            DropForeignKey("dbo.Vendors", "VendorTypeId", "dbo.VendorType");
            DropIndex("dbo.Vendors", new[] { "TypeId" });
            AddColumn("dbo.Vendors", "Type_Type", c => c.Int(nullable: false));
            DropColumn("dbo.Vendors", "TypeId");
            DropTable("dbo.VendorType");
        }
        
        public override void Down()
        {
            CreateTable(
                "dbo.VendorType",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(nullable: false, maxLength: 10),
                    })
                .PrimaryKey(t => t.Id);
            
            AddColumn("dbo.Vendors", "TypeId", c => c.Int(nullable: false));
            DropColumn("dbo.Vendors", "Type_Type");
            CreateIndex("dbo.Vendors", "TypeId");
            AddForeignKey("dbo.Vendors", "VendorTypeId", "dbo.VendorType", "Id", cascadeDelete: true);
            AddForeignKey("dbo.Vendors", "TypeId", "dbo.VendorType", "Id", cascadeDelete: true);
        }
    }
}
