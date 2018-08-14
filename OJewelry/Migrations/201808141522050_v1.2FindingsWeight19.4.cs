namespace OJewelry.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class v12FindingsWeight194 : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Findings", "Weight", c => c.Decimal(precision: 19, scale: 4));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Findings", "Weight", c => c.Decimal(precision: 18, scale: 2));
        }
    }
}
