namespace OJewelry.Migrations
{
    using OJewelry.Models;
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;

    internal sealed class Configuration : DbMigrationsConfiguration<OJewelry.Models.OJewelryDB>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = false;
        }
    }
}
