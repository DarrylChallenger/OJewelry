namespace OJewelry.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class v17MakePresenterNameRequired : DbMigration
    {
        public override void Up()
        {
            Sql("update p set p.Name = 'Location' + (SELECT Convert(varchar, rn)) FROM (select id, Name, companyId, ROW_NUMBER() OVER (partition by companyId order by id) as rn from Presenters where Name is null) as p");
            AlterColumn("dbo.Presenters", "Name", c => c.String(nullable: false, maxLength: 50));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Presenters", "Name", c => c.String(maxLength: 50));
        }
    }
}
