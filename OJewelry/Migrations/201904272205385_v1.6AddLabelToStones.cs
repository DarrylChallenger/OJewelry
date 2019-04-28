namespace OJewelry.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class v16AddLabelToStones : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Stones", "Label", c => c.String(maxLength: 50));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Stones", "Label");
        }
    }
}
