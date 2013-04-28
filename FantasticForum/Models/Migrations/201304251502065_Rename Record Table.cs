namespace Models.Migrations
{
    using System.Data.Entity.Migrations;
    
    public partial class RenameRecordTable : DbMigration
    {
        public override void Up()
        {
            RenameTable(name: "dbo.Records", newName: "Record");
        }
        
        public override void Down()
        {
            RenameTable(name: "dbo.Record", newName: "Records");
        }
    }
}
