namespace Models.Migrations
{
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
            
        }
        
        public override void Down()
        {
            DropIndex("dbo.Topic", new[] { "SectionId" });
            DropForeignKey("dbo.Topic", "SectionId", "dbo.Section");
            DropTable("dbo.Topic");
            DropTable("dbo.Section");
        }
    }
}
