namespace OJewelry.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class v51b7 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Companies", "JobTitle", c => c.String(maxLength: 50));
            AddColumn("dbo.Companies", "Website", c => c.String(maxLength: 50));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Companies", "Website");
            DropColumn("dbo.Companies", "JobTitle");
        }
    }
}
