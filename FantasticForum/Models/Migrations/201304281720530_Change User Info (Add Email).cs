namespace Models.Migrations
{
    using System.Data.Entity.Migrations;
    
    public partial class ChangeUserInfoAddEmail : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.User", "Email", c => c.String(nullable: false, maxLength: 30));
            DropColumn("dbo.User", "FirstName");
            DropColumn("dbo.User", "LastName");
        }
        
        public override void Down()
        {
            AddColumn("dbo.User", "LastName", c => c.String(nullable: false, maxLength: 30));
            AddColumn("dbo.User", "FirstName", c => c.String(nullable: false, maxLength: 30));
            DropColumn("dbo.User", "Email");
        }
    }
}
