namespace OJewelry.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class v61CascadeDeletes : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Clients", "CompanyID", "dbo.Companies");
            DropForeignKey("dbo.StyleComponents", "StyleId", "dbo.Styles");
            DropForeignKey("dbo.StyleLabor", "StyleId", "dbo.Styles");
            DropForeignKey("dbo.StyleMisc", "StyleId", "dbo.Styles");
            DropIndex("dbo.Clients", new[] { "CompanyID" });
            AlterColumn("dbo.Clients", "CompanyID", c => c.Int(nullable: false));
            CreateIndex("dbo.Clients", "CompanyID");
            AddForeignKey("dbo.Clients", "CompanyID", "dbo.Companies", "Id", cascadeDelete: true);
            AddForeignKey("dbo.StyleComponents", "StyleId", "dbo.Styles", "Id", cascadeDelete: true);
            AddForeignKey("dbo.StyleLabor", "StyleId", "dbo.Styles", "Id", cascadeDelete: true);
            AddForeignKey("dbo.StyleMisc", "StyleId", "dbo.Styles", "Id", cascadeDelete: true);
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.StyleMisc", "StyleId", "dbo.Styles");
            DropForeignKey("dbo.StyleLabor", "StyleId", "dbo.Styles");
            DropForeignKey("dbo.StyleComponents", "StyleId", "dbo.Styles");
            DropForeignKey("dbo.Clients", "CompanyID", "dbo.Companies");
            DropIndex("dbo.Clients", new[] { "CompanyID" });
            AlterColumn("dbo.Clients", "CompanyID", c => c.Int());
            CreateIndex("dbo.Clients", "CompanyID");
            AddForeignKey("dbo.StyleMisc", "StyleId", "dbo.Styles", "Id");
            AddForeignKey("dbo.StyleLabor", "StyleId", "dbo.Styles", "Id");
            AddForeignKey("dbo.StyleComponents", "StyleId", "dbo.Styles", "Id");
            AddForeignKey("dbo.Clients", "CompanyID", "dbo.Companies", "Id");
        }
    }
}
