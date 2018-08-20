namespace OJewelry.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class v12MetalWt_deciml : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Styles", "MetalWeight", c => c.Decimal(precision: 18, scale: 2));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Styles", "MetalWeight", c => c.Int());
        }
    }
}
