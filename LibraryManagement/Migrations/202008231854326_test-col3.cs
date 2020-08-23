namespace LibraryManagement.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class testcol3 : DbMigration
    {
        public override void Up()
        {
            DropIndex("dbo.AuctionUsers", new[] { "FirstName" });
            DropIndex("dbo.AuctionUsers", new[] { "LastName" });
        }
        
        public override void Down()
        {
            CreateIndex("dbo.AuctionUsers", "LastName", unique: true);
            CreateIndex("dbo.AuctionUsers", "FirstName", unique: true);
        }
    }
}
