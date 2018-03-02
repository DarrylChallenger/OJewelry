namespace OJewelry.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class v51b : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Presenters", "ShortName", c => c.String(maxLength: 10));
            AddColumn("dbo.Vendors", "Type", c => c.String(maxLength: 50));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Vendors", "Type");
            DropColumn("dbo.Presenters", "ShortName");
        }
    }
}
