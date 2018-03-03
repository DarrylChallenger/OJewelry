namespace OJewelry.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class v51b8 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Clients", "JobTitle", c => c.String(maxLength: 50));
            DropColumn("dbo.Companies", "JobTitle");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Companies", "JobTitle", c => c.String(maxLength: 50));
            DropColumn("dbo.Clients", "JobTitle");
        }
    }
}
