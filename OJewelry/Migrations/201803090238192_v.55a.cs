namespace OJewelry.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class v55a : DbMigration
    {
        public override void Up()
        {
            DropColumn("dbo.Styles", "Image");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Styles", "Image", c => c.Binary(storeType: "image"));
        }
    }
}
