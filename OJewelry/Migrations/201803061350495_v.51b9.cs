namespace OJewelry.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class v51b9 : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Presenters", "ShortName", c => c.String());
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Presenters", "ShortName", c => c.String(maxLength: 10));
        }
    }
}
