namespace LibraryManagement.Migrations
{
    using System.Data.Entity.Migrations;

    public partial class InitialCreate : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Auctions",
                c => new
                {
                    Id = c.Int(nullable: false, identity: true),
                    StartDate = c.DateTime(nullable: false),
                    EndDate = c.DateTime(nullable: false),
                    Ended = c.Boolean(nullable: false),
                    Auctioneer_Id = c.Int(),
                    Product_Id = c.Int(),
                    StartPrice_Id = c.Int(),
                })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.AuctionUsers", t => t.Auctioneer_Id)
                .ForeignKey("dbo.Products", t => t.Product_Id)
                .ForeignKey("dbo.Prices", t => t.StartPrice_Id)
                .Index(t => t.Auctioneer_Id)
                .Index(t => t.Product_Id)
                .Index(t => t.StartPrice_Id);

            CreateTable(
                "dbo.AuctionUsers",
                c => new
                {
                    Id = c.Int(nullable: false, identity: true),
                    FirstName = c.String(maxLength: 450),
                    LastName = c.String(maxLength: 450),
                    Gender = c.String(),
                    Role = c.String(),
                })
                .PrimaryKey(t => t.Id);

            CreateTable(
                "dbo.Bids",
                c => new
                {
                    Id = c.Int(nullable: false, identity: true),
                    BidDate = c.DateTime(nullable: false),
                    Auction_Id = c.Int(),
                    BidPrice_Id = c.Int(),
                    BidUser_Id = c.Int(),
                })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Auctions", t => t.Auction_Id)
                .ForeignKey("dbo.Prices", t => t.BidPrice_Id)
                .ForeignKey("dbo.AuctionUsers", t => t.BidUser_Id)
                .Index(t => t.Auction_Id)
                .Index(t => t.BidPrice_Id)
                .Index(t => t.BidUser_Id);

            CreateTable(
                "dbo.Prices",
                c => new
                {
                    Id = c.Int(nullable: false, identity: true),
                    Currency = c.String(maxLength: 450),
                    Value = c.Double(nullable: false),
                })
                .PrimaryKey(t => t.Id);

            CreateTable(
                "dbo.UserReviews",
                c => new
                {
                    Id = c.Int(nullable: false, identity: true),
                    Description = c.String(maxLength: 450),
                    Score = c.Int(nullable: false),
                    ReviewByUser_Id = c.Int(),
                    ReviewForUser_Id = c.Int(),
                    AuctionUser_Id = c.Int(),
                    AuctionUser_Id1 = c.Int(),
                })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.AuctionUsers", t => t.ReviewByUser_Id)
                .ForeignKey("dbo.AuctionUsers", t => t.ReviewForUser_Id)
                .ForeignKey("dbo.AuctionUsers", t => t.AuctionUser_Id)
                .ForeignKey("dbo.AuctionUsers", t => t.AuctionUser_Id1)
                .Index(t => t.ReviewByUser_Id)
                .Index(t => t.ReviewForUser_Id)
                .Index(t => t.AuctionUser_Id)
                .Index(t => t.AuctionUser_Id1);

            CreateTable(
                "dbo.Products",
                c => new
                {
                    Id = c.Int(nullable: false, identity: true),
                    Name = c.String(maxLength: 450),
                    AuctionUser_Id = c.Int(),
                })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.AuctionUsers", t => t.AuctionUser_Id)
                .Index(t => t.AuctionUser_Id);

            CreateTable(
                "dbo.Categories",
                c => new
                {
                    Id = c.Int(nullable: false, identity: true),
                    Name = c.String(maxLength: 450),
                    ParentCategory_Id = c.Int(),
                })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Categories", t => t.ParentCategory_Id)
                .Index(t => t.ParentCategory_Id);

            CreateTable(
                "dbo.CategoryProducts",
                c => new
                {
                    Category_Id = c.Int(nullable: false),
                    Product_Id = c.Int(nullable: false),
                })
                .PrimaryKey(t => new { t.Category_Id, t.Product_Id })
                .ForeignKey("dbo.Categories", t => t.Category_Id, cascadeDelete: true)
                .ForeignKey("dbo.Products", t => t.Product_Id, cascadeDelete: true)
                .Index(t => t.Category_Id)
                .Index(t => t.Product_Id);
        }

        public override void Down()
        {
            DropForeignKey("dbo.Auctions", "StartPrice_Id", "dbo.Prices");
            DropForeignKey("dbo.Auctions", "Product_Id", "dbo.Products");
            DropForeignKey("dbo.UserReviews", "AuctionUser_Id1", "dbo.AuctionUsers");
            DropForeignKey("dbo.Products", "AuctionUser_Id", "dbo.AuctionUsers");
            DropForeignKey("dbo.CategoryProducts", "Product_Id", "dbo.Products");
            DropForeignKey("dbo.CategoryProducts", "Category_Id", "dbo.Categories");
            DropForeignKey("dbo.Categories", "ParentCategory_Id", "dbo.Categories");
            DropForeignKey("dbo.UserReviews", "AuctionUser_Id", "dbo.AuctionUsers");
            DropForeignKey("dbo.UserReviews", "ReviewForUser_Id", "dbo.AuctionUsers");
            DropForeignKey("dbo.UserReviews", "ReviewByUser_Id", "dbo.AuctionUsers");
            DropForeignKey("dbo.Bids", "BidUser_Id", "dbo.AuctionUsers");
            DropForeignKey("dbo.Bids", "BidPrice_Id", "dbo.Prices");
            DropForeignKey("dbo.Bids", "Auction_Id", "dbo.Auctions");
            DropForeignKey("dbo.Auctions", "Auctioneer_Id", "dbo.AuctionUsers");
            DropIndex("dbo.CategoryProducts", new[] { "Product_Id" });
            DropIndex("dbo.CategoryProducts", new[] { "Category_Id" });
            DropIndex("dbo.Categories", new[] { "ParentCategory_Id" });
            DropIndex("dbo.Products", new[] { "AuctionUser_Id" });
            DropIndex("dbo.UserReviews", new[] { "AuctionUser_Id1" });
            DropIndex("dbo.UserReviews", new[] { "AuctionUser_Id" });
            DropIndex("dbo.UserReviews", new[] { "ReviewForUser_Id" });
            DropIndex("dbo.UserReviews", new[] { "ReviewByUser_Id" });
            DropIndex("dbo.Bids", new[] { "BidUser_Id" });
            DropIndex("dbo.Bids", new[] { "BidPrice_Id" });
            DropIndex("dbo.Bids", new[] { "Auction_Id" });
            DropIndex("dbo.Auctions", new[] { "StartPrice_Id" });
            DropIndex("dbo.Auctions", new[] { "Product_Id" });
            DropIndex("dbo.Auctions", new[] { "Auctioneer_Id" });
            DropTable("dbo.CategoryProducts");
            DropTable("dbo.Categories");
            DropTable("dbo.Products");
            DropTable("dbo.UserReviews");
            DropTable("dbo.Prices");
            DropTable("dbo.Bids");
            DropTable("dbo.AuctionUsers");
            DropTable("dbo.Auctions");
        }
    }
}