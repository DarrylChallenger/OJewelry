namespace OJewelry.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class v16NewStoneFields : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Stones", "ParentHandle", c => c.String(maxLength: 1024));
            AddColumn("dbo.Stones", "Title", c => c.String(maxLength: 128));
            AddColumn("dbo.Stones", "Tags", c => c.String(maxLength: 1024));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Stones", "Tags");
            DropColumn("dbo.Stones", "Title");
            DropColumn("dbo.Stones", "ParentHandle");
        }
    }
}
