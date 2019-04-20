namespace OJewelry.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class v16AddTypeReferenceToVendor : DbMigration
    {
        public override void Up()
        {
            RenameColumn(table: "dbo.Vendors", name: "VendorType_Id", newName: "Type_Id");
            RenameIndex(table: "dbo.Vendors", name: "IX_VendorType_Id", newName: "IX_Type_Id");
        }
        
        public override void Down()
        {
            RenameIndex(table: "dbo.Vendors", name: "IX_Type_Id", newName: "IX_VendorType_Id");
            RenameColumn(table: "dbo.Vendors", name: "Type_Id", newName: "VendorType_Id");
        }
    }
}
