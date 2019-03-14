namespace OJewelry.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class v15IssuesFromJan5MeetingIter4 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Stones", "Qty", c => c.Int(nullable: false));
            AlterColumn("dbo.Styles", "Width", c => c.String());
            AlterColumn("dbo.Styles", "Length", c => c.String());
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Styles", "Length", c => c.Decimal(precision: 8, scale: 5));
            AlterColumn("dbo.Styles", "Width", c => c.Decimal(precision: 8, scale: 5));
            DropColumn("dbo.Stones", "Qty");
        }
    }
}
