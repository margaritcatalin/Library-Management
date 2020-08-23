namespace LibraryManagement.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class testcol : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.UserReviews", "Test", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.UserReviews", "Test");
        }
    }
}
