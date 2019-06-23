namespace OJewelry.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class v17RemovePresenterNameReq : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Presenters", "Name", c => c.String(maxLength: 50));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Presenters", "Name", c => c.String(nullable: false, maxLength: 50));
        }
    }
}
