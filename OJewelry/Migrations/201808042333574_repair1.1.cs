namespace OJewelry.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class repair11 : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Stones", "ShapeId", "dbo.Shapes");
            DropPrimaryKey("dbo.Shapes");
            AlterColumn("dbo.Shapes", "Id", c => c.Int(nullable: false, identity: true));
            AddPrimaryKey("dbo.Shapes", "Id");
            AddForeignKey("dbo.Stones", "ShapeId", "dbo.Shapes", "Id");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Stones", "ShapeId", "dbo.Shapes");
            DropPrimaryKey("dbo.Shapes");
            AlterColumn("dbo.Shapes", "Id", c => c.Int(nullable: false));
            AddPrimaryKey("dbo.Shapes", "Id");
            AddForeignKey("dbo.Stones", "ShapeId", "dbo.Shapes", "Id");
        }
    }
}
