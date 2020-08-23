namespace LibraryManagement.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class testcoly : DbMigration
    {
        public override void Up()
        {
            DropIndex("dbo.Prices", new[] { "Currency" });
            DropIndex("dbo.UserReviews", new[] { "Description" });
            DropIndex("dbo.Products", "Id");
            DropIndex("dbo.Categories", new[] { "Name" });
        }
        
        public override void Down()
        {
            CreateIndex("dbo.Categories", "Name", unique: true);
            CreateIndex("dbo.Products", "Name", unique: true, name: "Id");
            CreateIndex("dbo.UserReviews", "Description", unique: true);
            CreateIndex("dbo.Prices", "Currency", unique: true);
        }
    }
}
