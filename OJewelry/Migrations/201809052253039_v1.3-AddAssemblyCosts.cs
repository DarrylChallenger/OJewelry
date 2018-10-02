namespace OJewelry.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class v13AddAssemblyCosts : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Cost",
                c => new
                    {
                        companyId = c.Int(nullable: false),
                        costDataJSON = c.String(),
                    })
                .PrimaryKey(t => t.companyId);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.Cost");
        }
    }
}
