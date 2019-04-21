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

        protected override void Seed(OJewelry.Models.OJewelryDB context)
        {
            //  This method will be called after migrating to the latest version.

            //  You can use the DbSet<T>.AddOrUpdate() helper extension method 
            //  to avoid creating duplicate seed data. E.g.
            //
            //    context.People.AddOrUpdate(
            //      p => p.FullName,
            //      new Person { FullName = "Andrew Peters" },
            //      new Person { FullName = "Brice Lambson" },
            //      new Person { FullName = "Rowan Miller" }
            //    );
            //
            if (context.VendorTypes.Count() == 0)
            {
                context.VendorTypes.Add(new VendorType { Id = 1, Name = "Stone" });
                context.VendorTypes.Add(new VendorType { Id = 2, Name = "Finding" });
                context.VendorTypes.Add(new VendorType { Id = 3, Name = "Casting" });
                context.VendorTypes.Add(new VendorType { Id = 4, Name = "Labor" });
            }
        }
    }
}
