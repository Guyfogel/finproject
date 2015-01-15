namespace DAL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class InitialCreate : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Cars",
                c => new
                    {
                        CarID = c.Int(nullable: false, identity: true),
                        Kilometrage = c.Int(nullable: false),
                        isAvailable = c.Boolean(nullable: false),
                        Photo = c.Binary(),
                        CarType_CarTypeID = c.Int(nullable: false),
                        Location_LocationId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.CarID)
                .ForeignKey("dbo.CarTypes", t => t.CarType_CarTypeID, cascadeDelete: true)
                .ForeignKey("dbo.Locations", t => t.Location_LocationId, cascadeDelete: true)
                .Index(t => t.CarType_CarTypeID)
                .Index(t => t.Location_LocationId);
            
            CreateTable(
                "dbo.CarTypes",
                c => new
                    {
                        CarTypeID = c.Int(nullable: false, identity: true),
                        Manufacturer = c.String(),
                        CarModel = c.String(),
                        PricePerDay = c.Decimal(nullable: false, precision: 18, scale: 2),
                        PricePerLateDay = c.Decimal(nullable: false, precision: 18, scale: 2),
                        Gear = c.String(),
                    })
                .PrimaryKey(t => t.CarTypeID);
            
            CreateTable(
                "dbo.Locations",
                c => new
                    {
                        LocationId = c.Int(nullable: false, identity: true),
                        LocationName = c.String(),
                        LocationAddress = c.String(),
                    })
                .PrimaryKey(t => t.LocationId);
            
            CreateTable(
                "dbo.Orders",
                c => new
                    {
                        OrderID = c.Int(nullable: false, identity: true),
                        LendDate = c.DateTime(nullable: false),
                        ReturnDate = c.DateTime(nullable: false),
                        ActualReturnDate = c.DateTime(nullable: false),
                        Car_CarID = c.Int(nullable: false),
                        User_UserID = c.Int(),
                    })
                .PrimaryKey(t => t.OrderID)
                .ForeignKey("dbo.Cars", t => t.Car_CarID, cascadeDelete: true)
                .ForeignKey("dbo.Users", t => t.User_UserID)
                .Index(t => t.Car_CarID)
                .Index(t => t.User_UserID);
            
            CreateTable(
                "dbo.Users",
                c => new
                    {
                        UserID = c.Int(nullable: false, identity: true),
                        Username = c.String(nullable: false, maxLength: 12),
                        PersonNum = c.Int(nullable: false),
                        Name = c.String(nullable: false, maxLength: 20),
                        Email = c.String(nullable: false),
                        Gender = c.Int(nullable: false),
                        Role = c.Int(nullable: false),
                        Pic = c.Binary(),
                    })
                .PrimaryKey(t => t.UserID);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Orders", "User_UserID", "dbo.Users");
            DropForeignKey("dbo.Orders", "Car_CarID", "dbo.Cars");
            DropForeignKey("dbo.Cars", "Location_LocationId", "dbo.Locations");
            DropForeignKey("dbo.Cars", "CarType_CarTypeID", "dbo.CarTypes");
            DropIndex("dbo.Orders", new[] { "User_UserID" });
            DropIndex("dbo.Orders", new[] { "Car_CarID" });
            DropIndex("dbo.Cars", new[] { "Location_LocationId" });
            DropIndex("dbo.Cars", new[] { "CarType_CarTypeID" });
            DropTable("dbo.Users");
            DropTable("dbo.Orders");
            DropTable("dbo.Locations");
            DropTable("dbo.CarTypes");
            DropTable("dbo.Cars");
        }
    }
}
