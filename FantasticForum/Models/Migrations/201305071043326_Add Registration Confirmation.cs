namespace Models.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddRegistrationConfirmation : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.User", "Guid", c => c.String(maxLength: 36));
            AddColumn("dbo.User", "IsConfirmed", c => c.Boolean(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.User", "IsConfirmed");
            DropColumn("dbo.User", "Guid");
        }
    }
}
