namespace OJewelry.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class v14AddForeignKeyPresenterToCompany : DbMigration
    {
        public override void Up()
        {
            DropIndex("dbo.Presenters", new[] { "CompanyId" });
            AlterColumn("dbo.Presenters", "CompanyId", c => c.Int(nullable: false));
            CreateIndex("dbo.Presenters", "CompanyId");
        }
        
        public override void Down()
        {
            DropIndex("dbo.Presenters", new[] { "CompanyId" });
            AlterColumn("dbo.Presenters", "CompanyId", c => c.Int());
            CreateIndex("dbo.Presenters", "CompanyId");
        }
    }
}
