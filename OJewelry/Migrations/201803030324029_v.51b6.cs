namespace OJewelry.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class v51b6 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Companies", "StreetAddr", c => c.String(maxLength: 150));
            AddColumn("dbo.Companies", "Addr2", c => c.String(maxLength: 150));
            AddColumn("dbo.Companies", "Phone", c => c.String(maxLength: 50));
            AlterColumn("dbo.Buyers", "Email", c => c.String(maxLength: 50));
            AlterColumn("dbo.Buyers", "Phone", c => c.String(maxLength: 10));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Buyers", "Phone", c => c.String(nullable: false, maxLength: 10));
            AlterColumn("dbo.Buyers", "Email", c => c.String(nullable: false, maxLength: 50));
            DropColumn("dbo.Companies", "Phone");
            DropColumn("dbo.Companies", "Addr2");
            DropColumn("dbo.Companies", "StreetAddr");
        }
    }
}
