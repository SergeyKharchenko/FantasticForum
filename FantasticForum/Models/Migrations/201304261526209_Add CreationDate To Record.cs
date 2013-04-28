namespace Models.Migrations
{
    using System.Data.Entity.Migrations;
    
    public partial class AddCreationDateToRecord : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Record", "CreationDate", c => c.DateTime(nullable: false));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Record", "CreationDate", c => c.String(nullable: false));
        }
    }
}
