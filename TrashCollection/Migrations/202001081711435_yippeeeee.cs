namespace TrashCollection.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class yippeeeee : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Customers", "PickupDay", c => c.String());
            AlterColumn("dbo.Customers", "ExtraPickupDate", c => c.DateTime(nullable: false));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Customers", "ExtraPickupDate", c => c.Int(nullable: false));
            AlterColumn("dbo.Customers", "PickupDay", c => c.DateTime(nullable: false));
        }
    }
}
