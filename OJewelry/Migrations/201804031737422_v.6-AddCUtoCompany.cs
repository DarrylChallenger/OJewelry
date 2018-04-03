namespace OJewelry.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class v6AddCUtoCompany : DbMigration
    {
        public override void Up()
        {
            CreateIndex("dbo.CompanyUsers", "CompanyId");
            AddForeignKey("dbo.CompanyUsers", "CompanyId", "dbo.Companies", "Id", cascadeDelete: true);
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.CompanyUsers", "CompanyId", "dbo.Companies");
            DropIndex("dbo.CompanyUsers", new[] { "CompanyId" });
        }
    }
}
