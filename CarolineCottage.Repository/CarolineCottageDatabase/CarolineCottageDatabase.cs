using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Entity;
using CarolineCottage.Repository.CarolineCottageClasses;
using System.Data.Entity.Migrations.History;
using Mbrrace.Repository.Conventions;

namespace CarolineCottage.Repository.CarolineCottageDatabase
{
    public class CarolineCottageDbContext : DbContext
    {

        public CarolineCottageDbContext() 
            :base("CCConnectionString")
        {

        }

        public CarolineCottageDbContext(string connectionString) 
            : base(connectionString)
        {

        }

        public DbSet<Booking> Bookings { get; set; }
        public DbSet<User> Users { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Conventions.Add(new DateTime2Convention());
        }

    }



    /* short guide
         * 
         * Tools -> NuGet Package Manager -> Package Manager Console
         * Select CarolineCottage.Repository from project list
         * Ensure that the project will build successfully.
         * enable-migrations -ContextTypeName: CarolineCottageDbContext -MigrationsDirectory:CarolineCottageMigrations
         * Call Add-Migration CarolineCottageMigrations
         * Call Update-database
         * 
         *      Update-Database -Script -SourceMigration:"BaseName" -TargetMigration:"TargetName"
         *      Update-database TargetMigration:0
         *      Update-Database -TargetMigration $InitialDatabase'
         *      Update-Database -Script -SourceMigration:"201512070002273_CarolineCottageMigrations" -TargetMigration:"201512070002273_CarolineCottageMigrations"
         *      Update-Database -Script -SourceMigration:"201411191320595_ChristmasTwoMigrations3" -TargetMigration:"201411281838563_ChristmasTwoMigrations4"
         */
}
