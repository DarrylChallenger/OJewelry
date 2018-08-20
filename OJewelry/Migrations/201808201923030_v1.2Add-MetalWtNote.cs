namespace OJewelry.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class v12AddMetalWtNote : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Styles", "MetalWtNote", c => c.String(maxLength: 50));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Styles", "MetalWtNote");
        }
    }
}
