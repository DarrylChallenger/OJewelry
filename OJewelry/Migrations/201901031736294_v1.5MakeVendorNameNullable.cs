namespace OJewelry.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class v15MakeVendorNameNullable : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Vendors", "Name", c => c.String(maxLength: 50, unicode: false));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Vendors", "Name", c => c.String(nullable: false, maxLength: 50, unicode: false));
        }
    }
}
