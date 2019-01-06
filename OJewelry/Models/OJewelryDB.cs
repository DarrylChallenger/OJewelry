namespace OJewelry.Models
{
    using System;
    using System.Data.Entity;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Linq;
    using System.Collections.Generic;
    using Microsoft.AspNet.Identity;
    using System.Web;
    using Microsoft.AspNet.Identity.EntityFramework;
    using System.Security.Principal;
    using System.Threading;

    public partial class OJewelryDB : DbContext
    {
        public OJewelryDB()
            : base("name=OJewelryDB")
        {

            //string loggedOnUserName = User.Identity.GetUserId();
            /*
            System.Security.Principal.WindowsPrincipal principal = new WindowsPrincipal();
            System.Security.Principal.WindowsIdentity p = new WindowsIdentity();
            */
            bIsAdmin = false;
            bIsGuest = false;

            AppDomain.CurrentDomain.SetPrincipalPolicy(PrincipalPolicy.WindowsPrincipal);
            //WindowsPrincipal MyPrincipal = (WindowsPrincipal)Thread.CurrentPrincipal;


            if (Thread.CurrentPrincipal.IsInRole("Admin") == true)
            {
                bIsAdmin = true;
            }
            if (Thread.CurrentPrincipal.IsInRole("Guest") == true)
            {
                bIsGuest = true;
            }
            userId = Thread.CurrentPrincipal.Identity.GetUserId();
         }

        //private string loggedOnUserName;
        string userId;
        bool bIsAdmin;
        bool bIsGuest;
        public IEnumerable<Company> Companies
        {
            get {
                // Get logged on user
                //return _Companies;
                if (bIsAdmin)
                {
                    return _Companies;
                }
                return _Companies.Join(CompaniesUsers.Where(x => x.UserId == userId), c => c.Id, cu => cu.CompanyId, (c, cu) => c);
            }
        }

        public Company FindCompany(int? i)
        {
            return _Companies.Find(i);
        }

        public Company AddCompany(Company company)
        {
            return _Companies.Add(company);
        }

        public Company RemoveCompany(Company company)
        {
            return _Companies.Remove(company);
        }

        public bool bUserIsGuest()
        {
            return bIsGuest;
        }

        public bool bUserIsAdmin()
        {
            return bIsAdmin;
        }

        public virtual DbSet<ACL> ACLs { get; set; }
        //public virtual DbSet<AssemblyCost> AssemblyCosts { get; set; }
        public virtual DbSet<Buyer> Buyers { get; set; }
        public virtual DbSet<Casting> Castings { get; set; }
        public virtual DbSet<Client> Clients { get; set; }
        public virtual DbSet<Collection> Collections { get; set; }
        public virtual DbSet<Company> _Companies { get; set; }
        public virtual DbSet<CompanyUser> CompaniesUsers { get; set; }
        //public virtual DbSet<Component> Components { get; set; }
        public virtual DbSet<Stone> Stones { get; set; }
        public virtual DbSet<Finding> Findings { get; set; }
        //public virtual DbSet<ComponentType> ComponentTypes { get; set; }
        public virtual DbSet<Contact> Contacts { get; set; }
        public virtual DbSet<JewelryType> JewelryTypes { get; set; }
        public virtual DbSet<Labor> Labors { get; set; }
        public virtual DbSet<Memo> Memos { get; set; }
        public virtual DbSet<MetalCode> MetalCodes { get; set; }
        public virtual DbSet<MetalWeightUnit> MetalWeightUnits { get; set; }
        public virtual DbSet<Misc> Miscs { get; set; }
        public virtual DbSet<Presenter> Presenters { get; set; }
        public virtual DbSet<SalesLedger> SalesLedgers { get; set; }
        public virtual DbSet<StyleCasting> StyleCastings { get; set; }
        //public virtual DbSet<StyleComponent> StyleComponents { get; set; }
        public virtual DbSet<StyleStone> StyleStones { get; set; }
        public virtual DbSet<StyleFinding> StyleFindings { get; set; }
        public virtual DbSet<StyleLabor> StyleLabors { get; set; }
        public virtual DbSet<StyleMisc> StyleMiscs { get; set; }
        public virtual DbSet<Style> Styles { get; set; }
        public virtual DbSet<Vendor> Vendors { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Casting>()
                .Property(e => e.Name)
                .IsUnicode(false);

            modelBuilder.Entity<Casting>()
                .Property(e => e.Price)
                .HasPrecision(8, 2);

            modelBuilder.Entity<Casting>()
                .Property(e => e.Labor)
                .HasPrecision(8, 2);

            modelBuilder.Entity<Client>()
                .Property(e => e.Name)
                .IsUnicode(false);

            modelBuilder.Entity<Client>()
                .Property(e => e.Phone)
                .IsUnicode(false);

            modelBuilder.Entity<Client>()
                .Property(e => e.Email)
                .IsUnicode(false);

            modelBuilder.Entity<Collection>()
                .Property(e => e.Name)
                .IsUnicode(false);

            modelBuilder.Entity<Collection>()
                .HasMany(e => e.Styles)
                .WithRequired(e => e.Collection)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Company>()
                .Property(e => e.Name)
                .IsUnicode(false);

            modelBuilder.Entity<Company>()
                .HasMany(e => e.Collections)
                .WithRequired(e => e.Company)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Company>()
                .HasMany(e => e.Clients)
                .WithRequired(e => e.Company)
                .WillCascadeOnDelete(true);

            // Add foreign key
            modelBuilder.Entity<Company>()
                .HasMany(e => e.Presenters)
                .WithRequired(e => e.Company)
                .WillCascadeOnDelete(false);
            //

            modelBuilder.Entity<Stone>()
                .Property(e => e.Name)
                .IsUnicode(false);

            modelBuilder.Entity<Stone>()
                .Property(e => e.Price)
                .HasPrecision(19, 4);

            modelBuilder.Entity<Stone>()
                .Property(e => e.Price)
                .HasPrecision(19, 4);

            modelBuilder.Entity<Stone>()
                .Property(e => e.StoneSize)
                .IsUnicode(false);

            modelBuilder.Entity<Finding>()
                .Property(e => e.Name)
                .IsUnicode(false);

            modelBuilder.Entity<Finding>()
                .Property(e => e.Price)
                .HasPrecision(19, 4);

            modelBuilder.Entity<Finding>()
                .Property(e => e.Weight)
                .HasPrecision(19, 4);

            /*modelBuilder.Entity<ComponentType>()
                .Property(e => e.Name)
                .IsUnicode(false);

            modelBuilder.Entity<ComponentType>()
                .HasMany(e => e.Components)
                .WithRequired(e => e.ComponentType)
                .WillCascadeOnDelete(false);*/

            modelBuilder.Entity<Contact>()
                .Property(e => e.Name)
                .IsUnicode(false);

            modelBuilder.Entity<Contact>()
                .Property(e => e.Phone)
                .IsUnicode(false);

            modelBuilder.Entity<Contact>()
                .Property(e => e.Email)
                .IsUnicode(false);

            modelBuilder.Entity<JewelryType>()
                .Property(e => e.Name)
                .IsUnicode(false);

            modelBuilder.Entity<Labor>()
                .Property(e => e.Name)
                .IsUnicode(false);

            modelBuilder.Entity<Labor>()
                .Property(e => e.Desc)
                .IsUnicode(false);

            modelBuilder.Entity<Labor>()
                .Property(e => e.PricePerHour)
                .HasPrecision(8, 5);

            modelBuilder.Entity<Labor>()
                .Property(e => e.PricePerPiece)
                .HasPrecision(8, 5);

            modelBuilder.Entity<Labor>()
                .HasMany(e => e.StyleLabors)
                .WithRequired(e => e.Labor)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<MetalCode>()
                .Property(e => e.Code)
                .IsFixedLength();

            modelBuilder.Entity<MetalCode>()
                .Property(e => e.Desc)
                .IsUnicode(false);

            modelBuilder.Entity<MetalWeightUnit>()
                .Property(e => e.Unit)
                .IsFixedLength();

            modelBuilder.Entity<MetalWeightUnit>()
                .HasMany(e => e.Styles)
                .WithOptional(e => e.MetalWeightUnit)
                .HasForeignKey(e => e.MetalWtUnitId);

            modelBuilder.Entity<Misc>()
                .Property(e => e.Name)
                .IsUnicode(false);

            modelBuilder.Entity<Misc>()
                .Property(e => e.Desc)
                .IsUnicode(false);

            modelBuilder.Entity<Misc>()
                .Property(e => e.PricePerPiece)
                .HasPrecision(8, 5);

            modelBuilder.Entity<Misc>()
                .HasMany(e => e.StyleMiscs)
                .WithRequired(e => e.Misc)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Presenter>()
                .HasMany(e => e.Memos)
                .WithRequired(e => e.Presenter)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<SalesLedger>()
                .Property(e => e.Notes)
                .IsFixedLength();

            modelBuilder.Entity<Style>()
                .Property(e => e.StyleNum)
                .IsUnicode(false);

            modelBuilder.Entity<Style>()
                .Property(e => e.StyleName)
                .IsUnicode(false);

            modelBuilder.Entity<Style>()
                .Property(e => e.Desc)
                .IsUnicode(false);

            /* W, L, chain length changed to strings
             * modelBuilder.Entity<Style>()
                .Property(e => e.Width)
                .HasPrecision(8, 5);

            modelBuilder.Entity<Style>()
                .Property(e => e.Length)
                .HasPrecision(8, 5);

             * modelBuilder.Entity<Style>()
                .Property(e => e.ChainLength)
                .HasPrecision(8, 5);
            */

            modelBuilder.Entity<Style>()
                .Property(e => e.RetailRatio)
                .HasPrecision(8, 5);

            modelBuilder.Entity<Style>()
                .Property(e => e.RedlineRatio)
                .HasPrecision(8, 5);

            modelBuilder.Entity<Style>()
                .HasMany(e => e.StyleStones)
                .WithRequired(e => e.Style)
                .WillCascadeOnDelete(true); // delete links when deleting style

            modelBuilder.Entity<Style>()
                .HasMany(e => e.StyleFindings)
                .WithRequired(e => e.Style)
                .WillCascadeOnDelete(true);

            modelBuilder.Entity<Shape>()
                .Property(e => e.Name)
                .IsUnicode(false);


            // delete links when deleting style
            /*
                                    modelBuilder.Entity<Style>()
                                        .HasMany(e => e.StyleCastings)
                                        .WithRequired(e => e.Style)
                        //                .Map(m=>m.MapKey("FK_StyleCasting_ToStyles"))
                                        .WillCascadeOnDelete(true); // delete links when deleting style (need to modify databases directly with cascade delete - done)

                                    modelBuilder.Entity<Style>()
                                        .HasMany(e => e.StyleLabors)
                                        .WithRequired(e => e.Style)
                         //               .Map(m => m.MapKey("FK_StyleLabor_ToStyles"))
                                        .WillCascadeOnDelete(true); // delete links when deleting style (need to modify databases directly with cascade delete)

                                    modelBuilder.Entity<Style>()
                                        .HasMany(e => e.StyleMiscs)
                                        .WithRequired(e => e.Style)
                          //              .Map(m => m.MapKey("FK_StyleMisc_ToStyles"))
                                        .WillCascadeOnDelete(true); // delete links when deleting style (need to modify databases directly with cascade delete)
                        */
            modelBuilder.Entity<Vendor>()
                .Property(e => e.Name)
                .IsUnicode(false);

            modelBuilder.Entity<Vendor>()
                .Property(e => e.Phone)
                .IsOptional()
                .IsUnicode(false);

            modelBuilder.Entity<Vendor>()
                .Property(e => e.Email)
                .IsOptional()
                .IsUnicode(false);
        }

        public System.Data.Entity.DbSet<OJewelry.Models.Shape> Shapes { get; set; }
    }
}
