namespace OJewelry.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    /*    
    public partial class v11_AddStonesShapesFindings : DbMigration
    {
        public override void Up2()
        {
            DropForeignKey("dbo.Stones", "ShapeId", "dbo.Shapes");
            DropPrimaryKey("dbo.Shapes");
            AlterColumn("dbo.Shapes", "Id", c => c.Int(nullable: false, identity: true));
            AddPrimaryKey("dbo.Shapes", "Id");
            AddForeignKey("dbo.Stones", "ShapeId", "dbo.Shapes", "Id");
        }
        
        public override void Down2()
        {
            DropForeignKey("dbo.Stones", "ShapeId", "dbo.Shapes");
            DropPrimaryKey("dbo.Shapes");
            AlterColumn("dbo.Shapes", "Id", c => c.Int(nullable: false));
            AddPrimaryKey("dbo.Shapes", "Id");
            AddForeignKey("dbo.Stones", "ShapeId", "dbo.Shapes", "Id");
        }
    }
   */
}
