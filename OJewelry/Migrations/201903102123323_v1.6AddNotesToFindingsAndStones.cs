namespace OJewelry.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class v16AddNotesToFindingsAndStones : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Findings", "Note", c => c.String(maxLength: 2048));
            AddColumn("dbo.Stones", "Note", c => c.String(maxLength: 2048));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Stones", "Note");
            DropColumn("dbo.Findings", "Note");
        }
    }
}
