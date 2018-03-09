namespace OJewelry.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class v55 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Styles", "ImageUrl", c => c.String(maxLength: 255));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Styles", "ImageUrl");
        }
    }
}
