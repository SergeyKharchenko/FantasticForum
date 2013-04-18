namespace Models.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddTopic : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Topic",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Title = c.String(nullable: false, maxLength: 100),
                        SectionId = c.Int(nullable: false),
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
        }
    }
}
