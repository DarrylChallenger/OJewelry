namespace OJewelry.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class v45a : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Styles", "UnitsSold", c => c.Int(nullable: false, defaultValue:0));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Styles", "UnitsSold");
        }
    }
}
