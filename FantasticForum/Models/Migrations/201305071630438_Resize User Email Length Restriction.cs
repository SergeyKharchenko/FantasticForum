namespace Models.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ResizeUserEmailLengthRestriction : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.User", "Email", c => c.String(nullable: false, maxLength: 100));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.User", "Email", c => c.String(nullable: false, maxLength: 30));
        }
    }
}
