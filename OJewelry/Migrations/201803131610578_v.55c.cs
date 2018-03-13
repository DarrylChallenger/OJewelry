namespace OJewelry.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class v55c : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Contacts",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(maxLength: 50, unicode: false),
                        Phone = c.String(maxLength: 10, unicode: false),
                        Email = c.String(maxLength: 50, unicode: false),
                        JobTitle = c.String(maxLength: 50),
                        PresenterId = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Presenters", t => t.PresenterId)
                .Index(t => t.PresenterId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Contacts", "PresenterId", "dbo.Presenters");
            DropIndex("dbo.Contacts", new[] { "PresenterId" });
            DropTable("dbo.Contacts");
        }
    }
}
