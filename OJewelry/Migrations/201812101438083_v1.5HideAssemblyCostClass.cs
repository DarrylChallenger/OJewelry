namespace OJewelry.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class v15HideAssemblyCostClass : DbMigration
    {
        public override void Up()
        {
            //DropTable("dbo.Cost");
        }
        
        public override void Down()
        {
            /*CreateTable(
                "dbo.Cost",
                c => new
                    {
                        companyId = c.Int(nullable: false),
                        costDataJSON = c.String(),
                    })
                .PrimaryKey(t => t.companyId);
            */
        }
    }
}
