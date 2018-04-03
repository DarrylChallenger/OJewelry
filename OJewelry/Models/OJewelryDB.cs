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

    public partial class OJewelryDB : DbContext
    {
        public OJewelryDB()
            : base("name=OJewelryDB")
        {
            loggedOnUserName = HttpContext.Current.User.Identity.GetUserName();
            sec = new ApplicationDbContext();
            ApplicationUser user = sec.Users.Where(x => x.UserName == loggedOnUserName).FirstOrDefault();
            if (user != null)
            {
                userId = user.Id;
            } else
            {
                userId = "";
            }
            IdentityRole role = sec.Roles.ToList().Where(x=>x.Name.ToString() == "Admin").FirstOrDefault();
            bIsAdmin = false;
            if (role != null && user != null)
            {
                IdentityUserRole ur = user.Roles.FirstOrDefault();
                if (ur != null)
                {
                    bIsAdmin = role.Id == ur.RoleId;
                }
            }
        }

        private string loggedOnUserName;
        string userId;
        bool bIsAdmin;
        private ApplicationDbContext sec;
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

        public virtual DbSet<ACL> ACLs { get; set; }
        public virtual DbSet<Buyer> Buyers { get; set; }
        public virtual DbSet<Casting> Castings { get; set; }
        public virtual DbSet<Client> Clients { get; set; }
        public virtual DbSet<Collection> Collections { get; set; }
        public virtual DbSet<Company> _Companies { get; set; }
        public virtual DbSet<CompanyUser> CompaniesUsers { get; set; }
        public virtual DbSet<Component> Components { get; set; }
        public virtual DbSet<ComponentType> ComponentTypes { get; set; }
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
        public virtual DbSet<StyleComponent> StyleComponents { get; set; }
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

            modelBuilder.Entity<Component>()
                .Property(e => e.Name)
                .IsUnicode(false);

            modelBuilder.Entity<Component>()
                .Property(e => e.Price)
                .HasPrecision(19, 4);

            modelBuilder.Entity<Component>()
                .Property(e => e.PricePerHour)
                .HasPrecision(19, 4);

            modelBuilder.Entity<Component>()
                .Property(e => e.PricePerPiece)
                .HasPrecision(19, 4);

            modelBuilder.Entity<Component>()
                .Property(e => e.MetalMetal)
                .IsUnicode(false);

            modelBuilder.Entity<Component>()
                .Property(e => e.MetalLabor)
                .HasPrecision(19, 4);

            modelBuilder.Entity<Component>()
                .Property(e => e.StoneSize)
                .IsUnicode(false);

            modelBuilder.Entity<Component>()
                .Property(e => e.StonePPC)
                .HasPrecision(19, 4);

            modelBuilder.Entity<Component>()
                .Property(e => e.FindingsMetal)
                .IsUnicode(false);

            modelBuilder.Entity<Component>()
                .HasMany(e => e.StyleComponents)
                .WithRequired(e => e.Component)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<ComponentType>()
                .Property(e => e.Name)
                .IsUnicode(false);

            modelBuilder.Entity<ComponentType>()
                .HasMany(e => e.Components)
                .WithRequired(e => e.ComponentType)
                .WillCascadeOnDelete(false);

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

            modelBuilder.Entity<Style>()
                .Property(e => e.Width)
                .HasPrecision(8, 5);

            modelBuilder.Entity<Style>()
                .Property(e => e.Length)
                .HasPrecision(8, 5);

            modelBuilder.Entity<Style>()
                .Property(e => e.ChainLength)
                .HasPrecision(8, 5);

            modelBuilder.Entity<Style>()
                .Property(e => e.RetailRatio)
                .HasPrecision(8, 5);

            modelBuilder.Entity<Style>()
                .Property(e => e.RedlineRatio)
                .HasPrecision(8, 5);

            modelBuilder.Entity<Style>()
                .HasMany(e => e.StyleComponents)
                .WithRequired(e => e.Style)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Style>()
                .HasMany(e => e.StyleLabors)
                .WithRequired(e => e.Style)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Style>()
                .HasMany(e => e.StyleMiscs)
                .WithRequired(e => e.Style)
                .WillCascadeOnDelete(false);

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
    }
}
