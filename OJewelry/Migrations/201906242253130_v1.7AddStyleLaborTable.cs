namespace OJewelry.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class v17AddStyleLaborTable : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.StyleLaborTable",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        StyleId = c.Int(nullable: false),
                        LaborTableId = c.Int(nullable: false),
                        LaborItem_Id = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.LaborTable", t => t.LaborItem_Id)
                .ForeignKey("dbo.Styles", t => t.StyleId, cascadeDelete: true)
                .Index(t => t.StyleId)
                .Index(t => t.LaborItem_Id);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.StyleLaborTable", "StyleId", "dbo.Styles");
            DropForeignKey("dbo.StyleLaborTable", "LaborItem_Id", "dbo.LaborTable");
            DropIndex("dbo.StyleLaborTable", new[] { "LaborItem_Id" });
            DropIndex("dbo.StyleLaborTable", new[] { "StyleId" });
            DropTable("dbo.StyleLaborTable");
        }
    }
}
