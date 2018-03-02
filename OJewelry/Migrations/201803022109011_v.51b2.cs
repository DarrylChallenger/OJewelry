namespace OJewelry.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class v51b2 : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Vendors", "Phone", c => c.String(maxLength: 50, unicode: false));
            AlterColumn("dbo.Vendors", "Email", c => c.String(maxLength: 50, unicode: false));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Vendors", "Email", c => c.String(nullable: false, maxLength: 50, unicode: false));
            AlterColumn("dbo.Vendors", "Phone", c => c.String(nullable: false, maxLength: 50, unicode: false));
        }
    }
}
