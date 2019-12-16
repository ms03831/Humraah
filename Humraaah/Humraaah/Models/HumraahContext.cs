namespace Humraaah.Models
{
    using System;
    using System.Data.Entity;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Linq;

    public partial class HumraahContext : DbContext
    {
        public HumraahContext()
            : base("name=HumraahContext")
        {
        }

        public virtual DbSet<C__MigrationHistory> C__MigrationHistory { get; set; }
        public virtual DbSet<Address> Addresses { get; set; }
        public virtual DbSet<Ambulance> Ambulances { get; set; }
        public virtual DbSet<AspNetRole> AspNetRoles { get; set; }
        public virtual DbSet<AspNetUserClaim> AspNetUserClaims { get; set; }
        public virtual DbSet<AspNetUserLogin> AspNetUserLogins { get; set; }
        public virtual DbSet<AspNetUser> AspNetUsers { get; set; }
        public virtual DbSet<aType> aTypes { get; set; }
        public virtual DbSet<Booking> Bookings { get; set; }
        public virtual DbSet<Driver> Drivers { get; set; }
        public virtual DbSet<Station> Stations { get; set; }
        public virtual DbSet<User_> User_ { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Address>()
                .Property(e => e.Locality)
                .IsUnicode(false);

            modelBuilder.Entity<Address>()
                .Property(e => e.Lat)
                .HasPrecision(18, 0);

            modelBuilder.Entity<Address>()
                .Property(e => e.Lng)
                .HasPrecision(18, 0);

            modelBuilder.Entity<Address>()
                .HasMany(e => e.Stations)
                .WithRequired(e => e.Address)
                .HasForeignKey(e => e.Address_id)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Address>()
                .HasMany(e => e.User_)
                .WithRequired(e => e.Address)
                .HasForeignKey(e => e.Address_id)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Ambulance>()
                .Property(e => e.Station_id)
                .IsUnicode(false);

            modelBuilder.Entity<Ambulance>()
                .Property(e => e.Plate)
                .IsUnicode(false);

            modelBuilder.Entity<Ambulance>()
                .Property(e => e.Color)
                .IsUnicode(false);

            modelBuilder.Entity<Ambulance>()
                .HasMany(e => e.Bookings)
                .WithRequired(e => e.Ambulance)
                .HasForeignKey(e => e.Ambulance_id)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<AspNetRole>()
                .HasMany(e => e.AspNetUsers)
                .WithMany(e => e.AspNetRoles)
                .Map(m => m.ToTable("AspNetUserRoles").MapLeftKey("RoleId").MapRightKey("UserId"));

            modelBuilder.Entity<AspNetUser>()
                .HasMany(e => e.AspNetUserClaims)
                .WithRequired(e => e.AspNetUser)
                .HasForeignKey(e => e.UserId);

            modelBuilder.Entity<AspNetUser>()
                .HasMany(e => e.AspNetUserLogins)
                .WithRequired(e => e.AspNetUser)
                .HasForeignKey(e => e.UserId);

            modelBuilder.Entity<aType>()
                .Property(e => e.vehicleType)
                .IsUnicode(false);

            modelBuilder.Entity<aType>()
                .HasMany(e => e.Ambulances)
                .WithRequired(e => e.aType)
                .HasForeignKey(e => e.aType_id)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Booking>()
                .Property(e => e.User__id)
                .IsUnicode(false);

            modelBuilder.Entity<Driver>()
                .HasMany(e => e.Ambulances)
                .WithRequired(e => e.Driver)
                .HasForeignKey(e => e.Driver_id)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Station>()
                .Property(e => e.id)
                .IsUnicode(false);

            modelBuilder.Entity<Station>()
                .Property(e => e.Organization)
                .IsUnicode(false);

            modelBuilder.Entity<Station>()
                .HasMany(e => e.Ambulances)
                .WithRequired(e => e.Station)
                .HasForeignKey(e => e.Station_id)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<User_>()
                .Property(e => e.id)
                .IsUnicode(false);

            modelBuilder.Entity<User_>()
                .HasMany(e => e.Bookings)
                .WithRequired(e => e.User_)
                .HasForeignKey(e => e.User__id)
                .WillCascadeOnDelete(false);
        }
    }
}
