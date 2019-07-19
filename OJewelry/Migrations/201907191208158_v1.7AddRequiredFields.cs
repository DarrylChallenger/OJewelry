namespace OJewelry.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class v17AddRequiredFields : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Stones", "ShapeId", "dbo.Shapes");
            DropIndex("dbo.Stones", new[] { "ShapeId" });
            AlterColumn("dbo.Vendors", "Name", c => c.String(nullable: false, maxLength: 50, unicode: false));
            AlterColumn("dbo.Stones", "ShapeId", c => c.Int(nullable: false));
            CreateIndex("dbo.Stones", "ShapeId");
            AddForeignKey("dbo.Stones", "ShapeId", "dbo.Shapes", "Id", cascadeDelete: true);
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Stones", "ShapeId", "dbo.Shapes");
            DropIndex("dbo.Stones", new[] { "ShapeId" });
            AlterColumn("dbo.Stones", "ShapeId", c => c.Int());
            AlterColumn("dbo.Vendors", "Name", c => c.String(maxLength: 50, unicode: false));
            CreateIndex("dbo.Stones", "ShapeId");
            AddForeignKey("dbo.Stones", "ShapeId", "dbo.Shapes", "Id");
        }
    }
}
