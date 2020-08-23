namespace LibraryManagement.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class testcol2 : DbMigration
    {
        public override void Up()
        {
            DropColumn("dbo.UserReviews", "Test");
        }
        
        public override void Down()
        {
            AddColumn("dbo.UserReviews", "Test", c => c.String());
        }
    }
}
