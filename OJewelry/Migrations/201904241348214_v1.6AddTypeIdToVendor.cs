namespace OJewelry.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class v16AddTypeIdToVendor : DbMigration
    {
        public override void Up()
        {
            RenameColumn(table: "dbo.Vendors", name: "Type_Id", newName: "TypeId");
            RenameIndex(table: "dbo.Vendors", name: "IX_Type_Id", newName: "IX_TypeId");
        }
        
        public override void Down()
        {
            RenameIndex(table: "dbo.Vendors", name: "IX_TypeId", newName: "IX_Type_Id");
            RenameColumn(table: "dbo.Vendors", name: "TypeId", newName: "Type_Id");
        }
    }
}
