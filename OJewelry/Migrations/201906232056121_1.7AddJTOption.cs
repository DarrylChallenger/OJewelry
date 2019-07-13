namespace OJewelry.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class _17AddJTOption : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.JewelryTypes", "bUseLaborTable", c => c.Boolean(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.JewelryTypes", "bUseLaborTable");
        }
    }
}
