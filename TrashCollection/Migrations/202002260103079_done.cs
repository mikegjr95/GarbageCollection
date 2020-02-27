namespace TrashCollection.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class done : DbMigration
    {
        public override void Up()
        {
            DropColumn("dbo.Employees", "dayFilter");
            DropColumn("dbo.Employees", "daysOfWeek_DataGroupField");
            DropColumn("dbo.Employees", "daysOfWeek_DataTextField");
            DropColumn("dbo.Employees", "daysOfWeek_DataValueField");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Employees", "daysOfWeek_DataValueField", c => c.String());
            AddColumn("dbo.Employees", "daysOfWeek_DataTextField", c => c.String());
            AddColumn("dbo.Employees", "daysOfWeek_DataGroupField", c => c.String());
            AddColumn("dbo.Employees", "dayFilter", c => c.String());
        }
    }
}
