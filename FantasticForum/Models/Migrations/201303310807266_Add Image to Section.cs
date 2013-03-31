namespace Models.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddImagetoSection : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Image",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        ImageMimeType = c.String(),
                        FileName = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            AddColumn("dbo.Section", "ImageId", c => c.Int());
            AddForeignKey("dbo.Section", "ImageId", "dbo.Image", "Id");
            CreateIndex("dbo.Section", "ImageId");
        }
        
        public override void Down()
        {
            DropIndex("dbo.Section", new[] { "ImageId" });
            DropForeignKey("dbo.Section", "ImageId", "dbo.Image");
            DropColumn("dbo.Section", "ImageId");
            DropTable("dbo.Image");
        }
    }
}
