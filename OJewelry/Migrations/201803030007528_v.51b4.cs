namespace OJewelry.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class v51b4 : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Clients", "Phone", c => c.String(maxLength: 10, unicode: false));
            AlterColumn("dbo.Clients", "Email", c => c.String(maxLength: 50, unicode: false));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Clients", "Email", c => c.String(nullable: false, maxLength: 50, unicode: false));
            AlterColumn("dbo.Clients", "Phone", c => c.String(nullable: false, maxLength: 10, unicode: false));
        }
    }
}
