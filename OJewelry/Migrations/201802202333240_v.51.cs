namespace OJewelry.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class v51 : DbMigration
    {
        public override void Up()
        {
            CreateIndex("dbo.Components", "MetalCodeId");
            AddForeignKey("dbo.Components", "MetalCodeId", "dbo.MetalCodes", "Id");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Components", "MetalCodeId", "dbo.MetalCodes");
            DropIndex("dbo.Components", new[] { "MetalCodeId" });
        }
    }
}
