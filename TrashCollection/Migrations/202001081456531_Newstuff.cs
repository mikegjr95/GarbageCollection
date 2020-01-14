namespace TrashCollection.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Newstuff : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Customers",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        ApplicationId = c.String(maxLength: 128),
                        PickupDay = c.DateTime(nullable: false),
                        FirstName = c.String(),
                        LastName = c.String(),
                        ExtraPickupDate = c.Int(nullable: false),
                        StreetAddress = c.Double(nullable: false),
                        City = c.String(),
                        State = c.String(),
                        ZipCode = c.Int(nullable: false),
                        Balance = c.Double(nullable: false),
                        SuspendedStart = c.DateTime(nullable: false),
                        SuspendedEnd = c.DateTime(nullable: false),
                        PickupConfirmation = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.AspNetUsers", t => t.ApplicationId)
                .Index(t => t.ApplicationId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Customers", "ApplicationId", "dbo.AspNetUsers");
            DropIndex("dbo.Customers", new[] { "ApplicationId" });
            DropTable("dbo.Customers");
        }
    }
}
