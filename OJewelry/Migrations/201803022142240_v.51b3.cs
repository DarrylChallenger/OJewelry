namespace OJewelry.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class v51b3 : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Presenters", "Phone", c => c.String(maxLength: 10));
            AlterColumn("dbo.Presenters", "Email", c => c.String(maxLength: 50));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Presenters", "Email", c => c.String(nullable: false, maxLength: 50));
            AlterColumn("dbo.Presenters", "Phone", c => c.String(nullable: false, maxLength: 10));
        }
    }
}
