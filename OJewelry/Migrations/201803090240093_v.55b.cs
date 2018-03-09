namespace OJewelry.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class v55b : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Styles", "Image", c => c.String(maxLength: 255));
            DropColumn("dbo.Styles", "ImageUrl");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Styles", "ImageUrl", c => c.String(maxLength: 255));
            DropColumn("dbo.Styles", "Image");
        }
    }
}
