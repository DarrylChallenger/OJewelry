namespace OJewelry.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class v20AddMarkupToCompany : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Companies", "markup", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Companies", "markup");
        }
    }
}
