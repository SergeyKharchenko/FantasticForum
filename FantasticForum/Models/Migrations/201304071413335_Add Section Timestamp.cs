namespace Models.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddSectionTimestamp : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Section", "Timestamp", c => c.Binary(nullable: false, fixedLength: true, timestamp: true, storeType: "rowversion"));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Section", "Timestamp");
        }
    }
}
