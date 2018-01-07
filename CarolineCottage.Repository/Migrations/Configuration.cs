namespace CarolineCottage.Repository.Migrations
{
    using System;
    using System.Data.Common;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Data.Entity.Migrations.History;
    using System.Linq;
    using CarolineCottageClasses;
    internal sealed class Configuration : DbMigrationsConfiguration<CarolineCottage.Repository.CarolineCottageDatabase.CarolineCottageDbContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = false;
            MigrationsDirectory = @"Migrations";

            //SetSqlGenerator("MySql.Data.MySqlClient", new MySql.Data.Entity.MySqlMigrationSqlGenerator());

            //SetHistoryContextFactory("MySql.Data.MySqlClient", (conn, schema) => new MySqlHistoryContext(conn, schema));


        }

        protected override void Seed(CarolineCottage.Repository.CarolineCottageDatabase.CarolineCottageDbContext dbContext)
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

            if (dbContext.Users.Count() == 0)
            {
                User user = new User();
                user.Name = "nina";
                user.Salt = "9gj8BVfA";
                user.PasswordEnc = "B6Zz0WZf9qUuY1vRaypyTLUq4Fk=";
                user.DateSet = DateTime.Now;
                dbContext.Users.Add(user);
                dbContext.SaveChanges();
            }
        }
    }

    //public class MySqlHistoryContext : HistoryContext
    //{

    //    public MySqlHistoryContext(DbConnection connection, string defaultSchema) : base(connection, defaultSchema)
    //    {

    //    }

    //    protected override void OnModelCreating(DbModelBuilder modelBuilder)
    //    {
    //        base.OnModelCreating(modelBuilder);

    //        modelBuilder.Entity<HistoryRow>().Property(h => h.MigrationId).HasMaxLength(100).IsRequired();
    //        modelBuilder.Entity<HistoryRow>().Property(h => h.ContextKey).HasMaxLength(200).IsRequired();
    //    }
    //}

}
