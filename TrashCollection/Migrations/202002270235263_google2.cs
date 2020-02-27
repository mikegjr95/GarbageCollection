namespace TrashCollection.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class google2 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Customers", "Lat", c => c.Double(nullable: false));
            AddColumn("dbo.Customers", "Lng", c => c.Double(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Customers", "Lng");
            DropColumn("dbo.Customers", "Lat");
        }
    }
}
