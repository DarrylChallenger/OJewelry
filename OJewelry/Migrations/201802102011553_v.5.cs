namespace OJewelry.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class v5 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Components", "MetalCodeId", c => c.Int(defaultValue:1));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Components", "MetalCodeId");
        }
    }
}
