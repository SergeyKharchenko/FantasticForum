namespace Models.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Init : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Section",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Title = c.String(nullable: false, maxLength: 30),
                        ImageId = c.String(),
                        Timestamp = c.Binary(nullable: false, fixedLength: true, timestamp: true, storeType: "rowversion"),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Topic",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Title = c.String(nullable: false, maxLength: 100),
                        SectionId = c.Int(nullable: false),
                        Timestamp = c.Binary(nullable: false, fixedLength: true, timestamp: true, storeType: "rowversion"),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Section", t => t.SectionId)
                .Index(t => t.SectionId);
            
            CreateTable(
                "dbo.Record",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Text = c.String(nullable: false),
                        CreationDate = c.DateTime(nullable: false),
                        TopicId = c.Int(nullable: false),
                        UserId = c.Int(nullable: false),
                        Timestamp = c.Binary(nullable: false, fixedLength: true, timestamp: true, storeType: "rowversion"),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Topic", t => t.TopicId)
                .ForeignKey("dbo.User", t => t.UserId)
                .Index(t => t.TopicId)
                .Index(t => t.UserId);
            
            CreateTable(
                "dbo.User",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Email = c.String(maxLength: 100),
                        Password = c.String(nullable: false, maxLength: 30),
                        Guid = c.String(maxLength: 36),
                        IsConfirmed = c.Boolean(nullable: false),
                        ImageId = c.String(),
                        Timestamp = c.Binary(nullable: false, fixedLength: true, timestamp: true, storeType: "rowversion"),
                    })
                .PrimaryKey(t => t.Id);
            
        }
        
        public override void Down()
        {
            DropIndex("dbo.Record", new[] { "UserId" });
            DropIndex("dbo.Record", new[] { "TopicId" });
            DropIndex("dbo.Topic", new[] { "SectionId" });
            DropForeignKey("dbo.Record", "UserId", "dbo.User");
            DropForeignKey("dbo.Record", "TopicId", "dbo.Topic");
            DropForeignKey("dbo.Topic", "SectionId", "dbo.Section");
            DropTable("dbo.User");
            DropTable("dbo.Record");
            DropTable("dbo.Topic");
            DropTable("dbo.Section");
        }
    }
}
