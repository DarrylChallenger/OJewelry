namespace OJewelry.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class v17VendorTypeId : DbMigration
    {
        public override void Up()
        {
            Sql("UPDATE dbo.Vendors SET TypeId =  1 WHERE TypeId IS NULL");
            DropForeignKey("dbo.Vendors", "TypeId", "dbo.VendorType");
            DropIndex("dbo.Vendors", new[] { "TypeId" });
            AlterColumn("dbo.Vendors", "TypeId", c => c.Int(nullable: false));
            CreateIndex("dbo.Vendors", "TypeId");
            AddForeignKey("dbo.Vendors", "TypeId", "dbo.VendorType", "Id", cascadeDelete: true);
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Vendors", "TypeId", "dbo.VendorType");
            DropIndex("dbo.Vendors", new[] { "TypeId" });
            AlterColumn("dbo.Vendors", "TypeId", c => c.Int());
            CreateIndex("dbo.Vendors", "TypeId");
            AddForeignKey("dbo.Vendors", "TypeId", "dbo.VendorType", "Id");
        }
    }
}
