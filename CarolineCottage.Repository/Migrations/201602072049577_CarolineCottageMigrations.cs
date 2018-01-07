namespace CarolineCottage.Repository.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class CarolineCottageMigrations : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Bookings",
                c => new
                    {
                        BookingID = c.Int(nullable: false, identity: true),
                        WeekStartDate = c.DateTime(nullable: false, precision: 7, storeType: "datetime2"),
                        BookingStatus = c.Int(nullable: false),
                        WeekPrice = c.Int(nullable: false),
                        AvailableForShortBreaks = c.Boolean(nullable: false),
                        Comment = c.String(),
                    })
                .PrimaryKey(t => t.BookingID);
            
            CreateTable(
                "dbo.Users",
                c => new
                    {
                        UserID = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                        EditedByID = c.Int(nullable: false),
                        DateEdited = c.DateTime(nullable: false, precision: 7, storeType: "datetime2"),
                        PasswordEnc = c.String(),
                        DateSet = c.DateTime(nullable: false, precision: 7, storeType: "datetime2"),
                        Salt = c.String(),
                    })
                .PrimaryKey(t => t.UserID);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.Users");
            DropTable("dbo.Bookings");
        }
    }
}
