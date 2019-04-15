namespace OJewelry.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class v16ExtendStyleNumto25 : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Styles", "StyleNum", c => c.String(nullable: false, maxLength: 25, unicode: false));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Styles", "StyleNum", c => c.String(nullable: false, maxLength: 15, unicode: false));
        }
    }
}
