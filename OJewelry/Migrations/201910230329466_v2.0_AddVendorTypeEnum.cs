namespace OJewelry.Migrations
{
    using OJewelry.Classes;
    using System.Data;
//    using System.Linq;
    using System;
    using System.Data.Entity.Migrations;
    using System.Collections.Generic;

    public partial class v20_AddVendorTypeEnum : DbMigration
    {
        public Dictionary<int, vendorTypeEnum> VendorTypeMap = new Dictionary<int, vendorTypeEnum>();
        public void InitVendorMigrationMap()
        {
            VendorTypeMap.Add(0, vendorTypeEnum.General);
            VendorTypeMap.Add(1, vendorTypeEnum.Stone);
            VendorTypeMap.Add(2, vendorTypeEnum.Finding);
            VendorTypeMap.Add(3, vendorTypeEnum.Casting);
            VendorTypeMap.Add(4, vendorTypeEnum.Labor);
        }

        public override void Up()
        {
            // typeId -> Type_Type 
            InitVendorMigrationMap();
            DropForeignKey("dbo.Vendors", "TypeId", "dbo.VendorType");
            DropForeignKey("dbo.Vendors", "VendorTypeId", "dbo.VendorType");
            DropIndex("dbo.Vendors", new[] { "TypeId" });
            AddColumn("dbo.Vendors", "Type_Type", c => c.Int(nullable: false));
            foreach(var mapping in VendorTypeMap)
            {
                Sql($"update Vendors set Type_Type = {mapping.Key} where TypeId = {mapping.Value}");
            }
            //DropColumn("dbo.Vendors", "TypeId");
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
