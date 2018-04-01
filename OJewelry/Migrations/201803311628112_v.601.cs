namespace OJewelry.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class v601 : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.CompanyUsers", "UserId", c => c.String(nullable: false, maxLength: 128));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.CompanyUsers", "UserId", c => c.String(nullable: false));
        }
    }
}
