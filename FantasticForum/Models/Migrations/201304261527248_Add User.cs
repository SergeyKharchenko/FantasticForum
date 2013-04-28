namespace Models.Migrations
{
    using System.Data.Entity.Migrations;
    
    public partial class AddUser : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.User",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        FirstName = c.String(nullable: false, maxLength: 30),
                        LastName = c.String(nullable: false, maxLength: 30),
                        Password = c.String(nullable: false, maxLength: 30),
                        ImageId = c.String(),
                        Timestamp = c.Binary(nullable: false, fixedLength: true, timestamp: true, storeType: "rowversion"),
                    })
                .PrimaryKey(t => t.Id);
            
            AddColumn("dbo.Record", "UserId", c => c.Int(nullable: false));
            AddForeignKey("dbo.Record", "UserId", "dbo.User", "Id");
            CreateIndex("dbo.Record", "UserId");
        }
        
        public override void Down()
        {
            DropIndex("dbo.Record", new[] { "UserId" });
            DropForeignKey("dbo.Record", "UserId", "dbo.User");
            DropColumn("dbo.Record", "UserId");
            DropTable("dbo.User");
        }
    }
}
