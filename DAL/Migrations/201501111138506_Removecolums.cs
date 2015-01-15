namespace DAL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Removecolums : DbMigration
    {
        public override void Up()
        {
            DropColumn("dbo.Cars", "Kilometrage");
            DropColumn("dbo.Cars", "isAvailable");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Cars", "isAvailable", c => c.Boolean(nullable: false));
            AddColumn("dbo.Cars", "Kilometrage", c => c.Int(nullable: false));
        }
    }
}
