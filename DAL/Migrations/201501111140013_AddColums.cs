namespace DAL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddColums : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Cars", "Kilometrage", c => c.Int(nullable: false));
            AddColumn("dbo.Cars", "isAvailable", c => c.Boolean(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Cars", "isAvailable");
            DropColumn("dbo.Cars", "Kilometrage");
        }
    }
}
