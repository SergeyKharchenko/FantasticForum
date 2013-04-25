namespace Models.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddRecord : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Records",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Text = c.String(nullable: false),
                        CreationDate = c.String(nullable: false),
                        TopicId = c.Int(nullable: false),
                        Timestamp = c.Binary(nullable: false, fixedLength: true, timestamp: true, storeType: "rowversion"),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Topic", t => t.TopicId)
                .Index(t => t.TopicId);
            
        }
        
        public override void Down()
        {
            DropIndex("dbo.Records", new[] { "TopicId" });
            DropForeignKey("dbo.Records", "TopicId", "dbo.Topic");
            DropTable("dbo.Records");
        }
    }
}
