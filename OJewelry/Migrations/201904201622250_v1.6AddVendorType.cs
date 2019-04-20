namespace OJewelry.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class v16AddVendorType : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.VendorType",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(nullable: false, maxLength: 10),
                    })
                .PrimaryKey(t => t.Id);
            
            AddColumn("dbo.Vendors", "VendorType_Id", c => c.Int());
            CreateIndex("dbo.Vendors", "VendorType_Id");
            AddForeignKey("dbo.Vendors", "VendorType_Id", "dbo.VendorType", "Id");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Vendors", "VendorType_Id", "dbo.VendorType");
            DropIndex("dbo.Vendors", new[] { "VendorType_Id" });
            DropColumn("dbo.Vendors", "VendorType_Id");
            DropTable("dbo.VendorType");
        }
    }
}
