namespace OJewelry.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class v51b5 : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Clients", "Name", c => c.String(maxLength: 50, unicode: false));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Clients", "Name", c => c.String(nullable: false, maxLength: 50, unicode: false));
        }
    }
}
