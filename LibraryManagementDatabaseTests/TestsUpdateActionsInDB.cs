// <copyright file="TestsUpdateActionsInDB.cs" company="Transilvania University of Brasov">
// Margarit Marian Catalin
// </copyright>

namespace LibraryManagementDatabaseTests
{
    using System;
    using System.Collections.Generic;
    using System.Configuration;
    using System.Linq;
    using LibraryManagement.BusinessLayer;
    using LibraryManagement.DataMapper;
    using LibraryManagement.DomainModel;
    using LibraryManagement.Util;
    using NUnit.Framework;

    /// <summary>
    /// The the auction with database insertion.
    /// </summary>
    [TestFixture]
    public class TestsUpdateActionsInDB
    {
        /// <summary>
        /// Defines the libraryContext.
        /// </summary>
        private LibraryDbContext libraryContext;

        /// <summary>
        /// Defines the bidService.
        /// </summary>
        private BidService bidService;

        /// <summary>
        /// Defines the productService.
        /// </summary>
        private ProductService productService;

        /// <summary>
        /// Defines the auctionUserService.
        /// </summary>
        private AuctionUserService auctionUserService;

        /// <summary>
        /// Defines the auctionService.
        /// </summary>
        private AuctionService auctionService;

        /// <summary>
        /// Defines the categoryService.
        /// </summary>
        private CategoryService categoryService;

        /// <summary>
        /// Defines the priceService.
        /// </summary>
        private PriceService priceService;

        /// <summary>
        /// Defines the userReviewService.
        /// </summary>
        private UserReviewService userReviewService;

        /// <summary>
        /// The test setup.
        /// </summary>
        [SetUp]
        public void SetUp()
        {
            this.libraryContext = new LibraryDbContext();
            this.productService = new ProductService(new ProductRepository(this.libraryContext));
            this.priceService = new PriceService(new PriceRepository(this.libraryContext));
            this.userReviewService = new UserReviewService(new UserReviewRepository(this.libraryContext));
            this.auctionUserService =
                new AuctionUserService(new AuctionUserRepository(this.libraryContext), this.userReviewService);
            this.categoryService = new CategoryService(new CategoryRepository(this.libraryContext));
            this.auctionService = new AuctionService(new AuctionRepository(this.libraryContext), this.categoryService, this.auctionUserService);
            this.bidService = new BidService(new BidRepository(this.libraryContext), this.auctionService);
        }

        /// <summary>
        /// Clean database after tests.
        /// </summary>
        [TearDown]
        public void Cleanup()
        {
            this.libraryContext.Database.Delete();
        }

        /// <summary>
        /// Test get user score with more than k reviews.
        /// </summary>
        [Test]
        public void TestGetUserScoreWithMoreThanKReviews()
        {
            this.ClearDatabase();
            var auctionUser = new AuctionUser { FirstName = "Ionel", LastName = "Pascu", Gender = "M" };
            var reviewUser = new AuctionUser { FirstName = "Popa", LastName = "Andrei", Gender = "M" };
            var resultUser1 = this.auctionUserService.AddAuctionUser(auctionUser, Role.Buyer);
            var resultUser2 = this.auctionUserService.AddAuctionUser(reviewUser, Role.Buyer);
            var user1 = this.auctionUserService.GetAuctionUserByFistNameAndLastName("Ionel", "Pascu");
            var user2 = this.auctionUserService.GetAuctionUserByFistNameAndLastName("Popa", "Andrei");
            var averageOf = int.Parse(ConfigurationManager.AppSettings["SCORE_AVERAGE_OF"]);
            var sumScores = 0;
            for (var i = 0; i <= averageOf + 1; i++)
            {
                var review1 = new UserReview
                { Description = "He is a good man" + i, Score = 1 + i, ReviewByUser = user2, ReviewForUser = user1 };
                var resultReview1 = this.userReviewService.AddUserReview(review1);
                if (i <= averageOf)
                {
                    sumScores += review1.Score;
                }
            }

            var user = this.auctionUserService.GetAuctionUserByFistNameAndLastName("Ionel", "Pascu");
            var userScore = this.auctionUserService.GetAuctionUserScore(user.Id);
            int correctScore = sumScores / averageOf;
            Assert.True(userScore == correctScore);
        }

        /// <summary>
        /// Test get user score without reviews.
        /// </summary>
        [Test]
        public void TestGetUserScoreWithReviews()
        {
            this.ClearDatabase();
            var auctionUser = new AuctionUser { FirstName = "Ionel", LastName = "Pascu", Gender = "M" };
            var reviewUser = new AuctionUser { FirstName = "Popa", LastName = "Andrei", Gender = "M" };
            var resultUser1 = this.auctionUserService.AddAuctionUser(auctionUser, Role.Buyer);
            var resultUser2 = this.auctionUserService.AddAuctionUser(reviewUser, Role.Buyer);
            var user1 = this.auctionUserService.GetAuctionUserByFistNameAndLastName("Ionel", "Pascu");
            var user2 = this.auctionUserService.GetAuctionUserByFistNameAndLastName("Popa", "Andrei");

            var review1 = new UserReview
            { Description = "He is a good man", Score = 4, ReviewByUser = user2, ReviewForUser = user1 };
            var review2 = new UserReview
            { Description = "He is a bad man", Score = 2, ReviewByUser = user2, ReviewForUser = user1 };
            var resultReview1 = this.userReviewService.AddUserReview(review1);
            var resultReview2 = this.userReviewService.AddUserReview(review2);
            var user = this.auctionUserService.GetAuctionUserByFistNameAndLastName("Ionel", "Pascu");
            var userScore = this.auctionUserService.GetAuctionUserScore(user.Id);
            int correctScore = (review1.Score + review2.Score) / 2;
            Assert.True(userScore == correctScore);
        }

        /// <summary>
        /// Test get user score without reviews.
        /// </summary>
        [Test]
        public void TestGetUserScoreWithReviewsDontHaveDefaultScore()
        {
            this.ClearDatabase();
            var auctionUser = new AuctionUser { FirstName = "Ionel", LastName = "Pascu", Gender = "M" };
            var reviewUser = new AuctionUser { FirstName = "Popa", LastName = "Andrei", Gender = "M" };
            var resultUser1 = this.auctionUserService.AddAuctionUser(auctionUser, Role.Buyer);
            var resultUser2 = this.auctionUserService.AddAuctionUser(reviewUser, Role.Buyer);
            var user1 = this.auctionUserService.GetAuctionUserByFistNameAndLastName("Ionel", "Pascu");
            var user2 = this.auctionUserService.GetAuctionUserByFistNameAndLastName("Popa", "Andrei");

            var review1 = new UserReview
            { Description = "He is a good man", Score = 4, ReviewByUser = user2, ReviewForUser = user1 };
            var review2 = new UserReview
            { Description = "He is a bad man", Score = 2, ReviewByUser = user2, ReviewForUser = user1 };
            var resultReview1 = this.userReviewService.AddUserReview(review1);
            var resultReview2 = this.userReviewService.AddUserReview(review2);
            var user = this.auctionUserService.GetAuctionUserByFistNameAndLastName("Ionel", "Pascu");
            var userScore = this.auctionUserService.GetAuctionUserScore(user.Id);
            var defaultScore = int.Parse(ConfigurationManager.AppSettings["DEFAULT_SCORE"]);
            Assert.True(userScore != defaultScore);
        }

        /// <summary>
        /// Test update firstName for auction user.
        /// </summary>
        [Test]
        public void TestUpdateFirstNameForAuctionUser()
        {
            this.ClearDatabase();
            var auctionUser = new AuctionUser { FirstName = "Ionel", LastName = "Pascu", Gender = "M" };

            var result = this.auctionUserService.AddAuctionUser(auctionUser, Role.Buyer);
            auctionUser.FirstName = "Marcus";
            var updateResult = this.auctionUserService.UpdateAuctionUser(auctionUser);

            var user = this.auctionUserService.GetAuctionUserByFistNameAndLastName("Ionel", "Pascu");
            var userUpdated = this.auctionUserService.GetAuctionUserByFistNameAndLastName("Marcus", "Pascu");

            Assert.IsTrue(user == null && userUpdated != null);
        }

        /// <summary>
        /// Test update with a wrong role name for auction user.
        /// </summary>
        [Test]
        public void TestUpdateWithAWrongRoleNameForAuctionUser()
        {
            this.ClearDatabase();
            var auctionUser = new AuctionUser { FirstName = "Ionel", LastName = "Pascu", Gender = "M" };

            var result = this.auctionUserService.AddAuctionUser(auctionUser, Role.Buyer);
            auctionUser.Role = "Administrator";
            var updateResult = this.auctionUserService.UpdateAuctionUser(auctionUser);
            TestsUtil.UndoingChangesDbEntityLevel(this.libraryContext, auctionUser);
            var user = this.auctionUserService.GetAuctionUserById(auctionUser.Id);
            Assert.IsFalse(updateResult);
            Assert.IsTrue(user.Role.Equals(Role.Buyer.Value));
        }

        /// <summary>
        /// Test update role name with lower first for auction user.
        /// </summary>
        [Test]
        public void TestUpdateRoleNameWithLowerFirstForAuctionUser()
        {
            this.ClearDatabase();
            var auctionUser = new AuctionUser { FirstName = "Ionel", LastName = "Pascu", Gender = "M" };

            var result = this.auctionUserService.AddAuctionUser(auctionUser, Role.Buyer);
            auctionUser.Role = Role.Seller.Value.ToLower();
            var updateResult = this.auctionUserService.UpdateAuctionUser(auctionUser);
            TestsUtil.UndoingChangesDbEntityLevel(this.libraryContext, auctionUser);
            var user = this.auctionUserService.GetAuctionUserById(auctionUser.Id);
            Assert.IsFalse(updateResult);
            Assert.IsTrue(user.Role.Equals(Role.Buyer.Value));
        }
        
        /// <summary>
        /// Test update role name with empty string for auction user.
        /// </summary>
        [Test]
        public void TestUpdateRoleNameWithEmptyStringForAuctionUser()
        {
            this.ClearDatabase();
            var auctionUser = new AuctionUser { FirstName = "Ionel", LastName = "Pascu", Gender = "M" };

            var result = this.auctionUserService.AddAuctionUser(auctionUser, Role.Buyer);
            auctionUser.Role = string.Empty;
            var updateResult = this.auctionUserService.UpdateAuctionUser(auctionUser);
            TestsUtil.UndoingChangesDbEntityLevel(this.libraryContext, auctionUser);
            var user = this.auctionUserService.GetAuctionUserById(auctionUser.Id);
            Assert.IsFalse(updateResult);
            Assert.IsTrue(user.Role.Equals(Role.Buyer.Value));
        }

        /// <summary>
        /// Test update with a firstName for auction user.
        /// </summary>
        [Test]
        public void TestUpdateWithBadFirstNameForAuctionUser()
        {
            this.ClearDatabase();

            var auctionUser = new AuctionUser { FirstName = "Ionel", LastName = "Pascu", Gender = "M" };

            var result = this.auctionUserService.AddAuctionUser(auctionUser, Role.Buyer);
            auctionUser.FirstName = "!@$%#";
            var updateResult = this.auctionUserService.UpdateAuctionUser(auctionUser);

            var user = this.auctionUserService.GetAuctionUserByFistNameAndLastName("Ionel", "Pascu");
            var userUpdated = this.auctionUserService.GetAuctionUserByFistNameAndLastName("!@$%#", "Pascu");

            Assert.IsTrue(user != null && userUpdated == null);
        }

        /// <summary>
        /// Test update lastName for auction user.
        /// </summary>
        [Test]
        public void TestUpdateLastNameForAuctionUser()
        {
            this.ClearDatabase();

            var auctionUser = new AuctionUser { FirstName = "Ionel", LastName = "Pascu", Gender = "M" };

            var result = this.auctionUserService.AddAuctionUser(auctionUser, Role.Buyer);
            auctionUser.LastName = "Marcus";
            var updateResult = this.auctionUserService.UpdateAuctionUser(auctionUser);

            var user = this.auctionUserService.GetAuctionUserByFistNameAndLastName("Ionel", "Pascu");
            var userUpdated = this.auctionUserService.GetAuctionUserByFistNameAndLastName("Ionel", "Marcus");

            Assert.IsTrue(user == null && userUpdated != null);
        }

        /// <summary>
        /// Test update firstName for auction user.
        /// </summary>
        [Test]
        public void TestUpdateWithBadLastNameForAuctionUser()
        {
            this.ClearDatabase();

            var auctionUser = new AuctionUser { FirstName = "Ionel", LastName = "Pascu", Gender = "M" };

            var result = this.auctionUserService.AddAuctionUser(auctionUser, Role.Buyer);
            auctionUser.LastName = "!@$%#";
            var updateResult = this.auctionUserService.UpdateAuctionUser(auctionUser);

            var user = this.auctionUserService.GetAuctionUserByFistNameAndLastName("Ionel", "Pascu");
            var userUpdated = this.auctionUserService.GetAuctionUserByFistNameAndLastName("Ionel", "!@$%#");

            Assert.IsTrue(user != null && userUpdated == null);
        }

        /// <summary>
        /// Test update gender for auction user.
        /// </summary>
        [Test]
        public void TestUpdateGenderForAuctionUser()
        {
            this.ClearDatabase();

            var auctionUser = new AuctionUser { FirstName = "Ionel", LastName = "Pascu", Gender = "M" };

            var result = this.auctionUserService.AddAuctionUser(auctionUser, Role.Buyer);
            auctionUser.Gender = "F";
            var updateResult = this.auctionUserService.UpdateAuctionUser(auctionUser);

            var user = this.auctionUserService.GetAuctionUserByFistNameAndLastName("Ionel", "Pascu");

            Assert.IsTrue(user.Gender == "F");
        }

        /// <summary>
        /// Test update with bag gender for auction user.
        /// </summary>
        [Test]
        public void TestUpdateWithBadGenderForAuctionUser()
        {
            this.ClearDatabase();

            var auctionUser = new AuctionUser { FirstName = "Ionel", LastName = "Pascu", Gender = "M" };

            var result = this.auctionUserService.AddAuctionUser(auctionUser, Role.Buyer);
            auctionUser.Gender = "C";
            var updateResult = this.auctionUserService.UpdateAuctionUser(auctionUser);
            TestsUtil.UndoingChangesDbEntityLevel(this.libraryContext, auctionUser);

            var user = this.auctionUserService.GetAuctionUserByFistNameAndLastName("Ionel", "Pascu");

            Assert.IsTrue(user.Gender == "M");
        }

        /// <summary>
        /// Test update role for auction user.
        /// </summary>
        [Test]
        public void TestUpdateRoleForAuctionUser()
        {
            this.ClearDatabase();

            var auctionUser = new AuctionUser { FirstName = "Ionel", LastName = "Pascu", Gender = "M" };

            var result = this.auctionUserService.AddAuctionUser(auctionUser, Role.Buyer);
            auctionUser.Role = Role.Seller.Value;
            var updateResult = this.auctionUserService.UpdateAuctionUser(auctionUser);
            TestsUtil.UndoingChangesDbEntityLevel(this.libraryContext, auctionUser);

            var user = this.auctionUserService.GetAuctionUserByFistNameAndLastName("Ionel", "Pascu");

            Assert.IsTrue(user.Role == Role.Seller.Value);
        }

        /// <summary>
        /// Test update with bad role for auction user.
        /// </summary>
        [Test]
        public void TestUpdateWithBadRoleForAuctionUser()
        {
            this.ClearDatabase();

            var auctionUser = new AuctionUser { FirstName = "Ionel", LastName = "Pascu", Gender = "M" };

            var result = this.auctionUserService.AddAuctionUser(auctionUser, Role.Buyer);
            auctionUser.Role = "Customer";
            var updateResult = this.auctionUserService.UpdateAuctionUser(auctionUser);
            TestsUtil.UndoingChangesDbEntityLevel(this.libraryContext, auctionUser);

            var user = this.auctionUserService.GetAuctionUserByFistNameAndLastName("Ionel", "Pascu");

            Assert.IsTrue(user.Role == Role.Buyer.Value);
        }

        /// <summary>
        /// Test update auction for bid.
        /// </summary>
        [Test]
        public void TestUpdateBidAuction()
        {
            this.ClearDatabase();
            var auctionUser = new AuctionUser { FirstName = "Ionel", LastName = "Pascu", Gender = "M" };
            var result = this.auctionUserService.AddAuctionUser(auctionUser, Role.Seller);
            var userSeller = this.auctionUserService.GetAuctionUserByFistNameAndLastName(auctionUser.FirstName, auctionUser.LastName);
            var startPrice = new Price { Currency = "Euro", Value = 88.5 };
            var priceResult = this.priceService.AddPrice(startPrice);
            var category = new Category { Name = "Legume" };
            var categoryResult = this.categoryService.AddCategory(category);
            var product = new Product { Name = "Fasole", Category = new[] { category } };
            var productResult = this.productService.AddProduct(product);
            var auction = new Auction
            {
                Auctioneer = userSeller,
                Product = product,
                StartPrice = startPrice,
                StartDate = DateTime.Now,
                EndDate = DateTime.Now.AddDays(5),
                Ended = false,
            };
            var auctionResult = this.auctionService.AddAuction(auction);
            var auctionUser2 = new AuctionUser { Id = 2, FirstName = "Ioana", LastName = "Pascu", Gender = "F" };
            var result2 = this.auctionUserService.AddAuctionUser(auctionUser2, Role.Buyer);
            var userBuyer = this.auctionUserService.GetAuctionUserByFistNameAndLastName(auctionUser2.FirstName, auctionUser2.LastName);
            var bidPrice = new Price { Id = 2, Currency = "Euro", Value = startPrice.Value + 1 };
            var bidPriceResult = this.priceService.AddPrice(bidPrice);
            var auctionById = this.auctionService.GetAuctionById(auction.Id);
            var bid = new Bid
            {
                Auction = auctionById,
                BidUser = userBuyer,
                BidPrice = bidPrice,
                BidDate = DateTime.Now
            };
            var resultBid = this.bidService.AddBid(bid);
            var bidById = this.bidService.GetBidById(bid.Id);
            var auction2 = new Auction
            {
                Auctioneer = userSeller,
                Product = product,
                StartPrice = startPrice,
                StartDate = DateTime.Now,
                EndDate = DateTime.Now.AddDays(7),
                Ended = false,
            };
            var auction2Result = this.auctionService.AddAuction(auction2);

            bidById.Auction = auction2;
            var updateResult = this.bidService.UpdateBid(bidById);
            TestsUtil.UndoingChangesDbEntityLevel(this.libraryContext, bidById);
            bidById = this.bidService.GetBidById(bid.Id);
            Assert.IsTrue(bidById.Auction.Id == auction2.Id);
        }

        /// <summary>
        /// Test update with a wrong auction for bid.
        /// </summary>
        [Test]
        public void TestUpdateWithBadBidAuction()
        {
            this.ClearDatabase();
            var auctionUser = new AuctionUser { FirstName = "Ionel", LastName = "Pascu", Gender = "M" };
            var result = this.auctionUserService.AddAuctionUser(auctionUser, Role.Seller);
            var userSeller = this.auctionUserService.GetAuctionUserByFistNameAndLastName(auctionUser.FirstName, auctionUser.LastName);
            var startPrice = new Price { Currency = "Euro", Value = 88.5 };
            var priceResult = this.priceService.AddPrice(startPrice);
            var category = new Category { Name = "Legume" };
            var categoryResult = this.categoryService.AddCategory(category);
            var product = new Product { Name = "Fasole", Category = new[] { category } };
            var productResult = this.productService.AddProduct(product);
            var auction = new Auction
            {
                Auctioneer = userSeller,
                Product = product,
                StartPrice = startPrice,
                StartDate = DateTime.Now,
                EndDate = DateTime.Now.AddDays(5),
                Ended = false,
            };
            var auctionResult = this.auctionService.AddAuction(auction);
            var auctionUser2 = new AuctionUser { FirstName = "Ioana", LastName = "Pascu", Gender = "F" };
            var result2 = this.auctionUserService.AddAuctionUser(auctionUser2, Role.Buyer);
            var userBuyer = this.auctionUserService.GetAuctionUserByFistNameAndLastName(auctionUser2.FirstName, auctionUser2.LastName);
            var bidPrice = new Price { Currency = "Euro", Value = startPrice.Value + 1 };
            var bidPriceResult = this.priceService.AddPrice(bidPrice);
            var auctionById = this.auctionService.GetAuctionById(1);
            var bid = new Bid
            {
                Auction = auctionById,
                BidUser = userBuyer,
                BidPrice = bidPrice,
                BidDate = DateTime.Now
            };
            var resultBid = this.bidService.AddBid(bid);
            var bidById = this.bidService.GetBidById(bid.Id);
            bidById.Auction = null;
            var updateResult = this.bidService.UpdateBid(bidById);
            Assert.IsFalse(updateResult);
        }

        /// <summary>
        /// Test update user for bid.
        /// </summary>
        [Test]
        public void TestUpdateBidUser()
        {
            this.ClearDatabase();
            var auctionUser = new AuctionUser { FirstName = "Ionel", LastName = "Pascu", Gender = "M" };
            var result = this.auctionUserService.AddAuctionUser(auctionUser, Role.Seller);
            var userSeller = this.auctionUserService.GetAuctionUserByFistNameAndLastName(auctionUser.FirstName, auctionUser.LastName);
            var startPrice = new Price { Currency = "Euro", Value = 88.5 };
            var priceResult = this.priceService.AddPrice(startPrice);
            var category = new Category { Name = "Legume" };
            var categoryResult = this.categoryService.AddCategory(category);
            var product = new Product { Name = "Fasole", Category = new[] { category } };
            var productResult = this.productService.AddProduct(product);
            var auction = new Auction
            {
                Auctioneer = userSeller,
                Product = product,
                StartPrice = startPrice,
                StartDate = DateTime.Now,
                EndDate = DateTime.Now.AddDays(5),
                Ended = false,
            };
            var auctionResult = this.auctionService.AddAuction(auction);
            var auctionUser2 = new AuctionUser { FirstName = "Ioana", LastName = "Pascu", Gender = "F" };
            var result2 = this.auctionUserService.AddAuctionUser(auctionUser2, Role.Buyer);
            var userBuyer = this.auctionUserService.GetAuctionUserByFistNameAndLastName(auctionUser2.FirstName, auctionUser2.LastName);
            var bidPrice = new Price { Currency = "Euro", Value = startPrice.Value + 1 };
            var bidPriceResult = this.priceService.AddPrice(bidPrice);
            var auctionById = this.auctionService.GetAuctionById(auction.Id);
            var bid = new Bid
            {
                Auction = auctionById,
                BidUser = userBuyer,
                BidPrice = bidPrice,
                BidDate = DateTime.Now
            };
            var resultBid = this.bidService.AddBid(bid);
            var bidById = this.bidService.GetBidById(bid.Id);
            var auctionUser3 = new AuctionUser { Id = 2, FirstName = "Anton", LastName = "Mihai", Gender = "F" };
            var result3 = this.auctionUserService.AddAuctionUser(auctionUser3, Role.Buyer);
            var userBuyer3 = this.auctionUserService.GetAuctionUserByFistNameAndLastName(auctionUser3.FirstName, auctionUser3.LastName);

            bidById.BidUser = userBuyer3;
            var updateResult = this.bidService.UpdateBid(bidById);
            TestsUtil.UndoingChangesDbEntityLevel(this.libraryContext, bidById);

            bidById = this.bidService.GetBidById(bid.Id);
            Assert.IsTrue(bidById.BidUser.Id == userBuyer3.Id);
        }

        /// <summary>
        /// Test update user null for bid.
        /// </summary>
        [Test]
        public void TestUpdateBidUserNull()
        {
            this.ClearDatabase();
            var auctionUser = new AuctionUser { FirstName = "Ionel", LastName = "Pascu", Gender = "M" };
            var result = this.auctionUserService.AddAuctionUser(auctionUser, Role.Seller);
            var userSeller = this.auctionUserService.GetAuctionUserByFistNameAndLastName(auctionUser.FirstName, auctionUser.LastName);
            var startPrice = new Price { Currency = "Euro", Value = 88.5 };
            var priceResult = this.priceService.AddPrice(startPrice);
            var category = new Category { Name = "Legume" };
            var categoryResult = this.categoryService.AddCategory(category);
            var product = new Product { Name = "Fasole", Category = new[] { category } };
            var productResult = this.productService.AddProduct(product);
            var auction = new Auction
            {
                Auctioneer = userSeller,
                Product = product,
                StartPrice = startPrice,
                StartDate = DateTime.Now,
                EndDate = DateTime.Now.AddDays(5),
                Ended = false,
            };
            var auctionResult = this.auctionService.AddAuction(auction);
            var auctionUser2 = new AuctionUser { FirstName = "Ioana", LastName = "Pascu", Gender = "F" };
            var result2 = this.auctionUserService.AddAuctionUser(auctionUser2, Role.Buyer);
            var userBuyer = this.auctionUserService.GetAuctionUserByFistNameAndLastName(auctionUser2.FirstName, auctionUser2.LastName);
            var bidPrice = new Price { Currency = "Euro", Value = startPrice.Value + 1 };
            var bidPriceResult = this.priceService.AddPrice(bidPrice);
            var auctionById = this.auctionService.GetAuctionById(auction.Id);
            var bid = new Bid
            {
                Auction = auctionById,
                BidUser = userBuyer,
                BidPrice = bidPrice,
                BidDate = DateTime.Now
            };
            var resultBid = this.bidService.AddBid(bid);
            var bidById = this.bidService.GetBidById(bid.Id);
            bidById.BidUser = null;
            var updateResult = this.bidService.UpdateBid(bidById);
            Assert.IsFalse(updateResult);
        }
        
        /// <summary>
        /// Test update with a wrong user for bid.
        /// </summary>
        [Test]
        public void TestUpdateWithBadBidUser()
        {
            this.ClearDatabase();
            var auctionUser = new AuctionUser { FirstName = "Ionel", LastName = "Pascu", Gender = "M" };
            var result = this.auctionUserService.AddAuctionUser(auctionUser, Role.Seller);
            var userSeller = this.auctionUserService.GetAuctionUserByFistNameAndLastName(auctionUser.FirstName, auctionUser.LastName);
            var startPrice = new Price { Currency = "Euro", Value = 88.5 };
            var priceResult = this.priceService.AddPrice(startPrice);
            var category = new Category { Name = "Legume" };
            var categoryResult = this.categoryService.AddCategory(category);
            var product = new Product { Name = "Fasole", Category = new[] { category } };
            var productResult = this.productService.AddProduct(product);
            var auction = new Auction
            {
                Id = 1,
                Auctioneer = userSeller,
                Product = product,
                StartPrice = startPrice,
                StartDate = DateTime.Now,
                EndDate = DateTime.Now.AddDays(5),
                Ended = false,
            };
            var auctionResult = this.auctionService.AddAuction(auction);
            var auctionUser2 = new AuctionUser { FirstName = "Ioana", LastName = "Pascu", Gender = "F" };
            var result2 = this.auctionUserService.AddAuctionUser(auctionUser2, Role.Buyer);
            var userBuyer = this.auctionUserService.GetAuctionUserByFistNameAndLastName(auctionUser2.FirstName, auctionUser2.LastName);
            var bidPrice = new Price { Currency = "Euro", Value = startPrice.Value + 1 };
            var bidPriceResult = this.priceService.AddPrice(bidPrice);
            var auctionById = this.auctionService.GetAuctionById(auction.Id);
            var bid = new Bid
            {
                Auction = auctionById,
                BidUser = userBuyer,
                BidPrice = bidPrice,
                BidDate = DateTime.Now
            };
            var resultBid = this.bidService.AddBid(bid);
            var bidById = this.bidService.GetBidById(bid.Id);
            var auctionUser3 = new AuctionUser { FirstName = "Anton", LastName = "Mihai", Gender = "F" };
            var result3 = this.auctionUserService.AddAuctionUser(auctionUser3, Role.Seller);
            var userBuyer3 = this.auctionUserService.GetAuctionUserByFistNameAndLastName(auctionUser3.FirstName, auctionUser3.LastName);
            bidById.BidUser = userBuyer3;
            var updateResult = this.bidService.UpdateBid(bidById);
            Assert.IsFalse(updateResult);
        }

        /// <summary>
        /// Test update price for bid.
        /// </summary>
        [Test]
        public void TestUpdateBidPrice()
        {
            this.ClearDatabase();
            var auctionUser = new AuctionUser { FirstName = "Ionel", LastName = "Pascu", Gender = "M" };
            var result = this.auctionUserService.AddAuctionUser(auctionUser, Role.Seller);
            var userSeller = this.auctionUserService.GetAuctionUserByFistNameAndLastName(auctionUser.FirstName, auctionUser.LastName);
            var startPrice = new Price { Currency = "Euro", Value = 88.5 };
            var priceResult = this.priceService.AddPrice(startPrice);
            var category = new Category { Name = "Legume" };
            var categoryResult = this.categoryService.AddCategory(category);
            var product = new Product { Name = "Fasole", Category = new[] { category } };
            var productResult = this.productService.AddProduct(product);
            var auction = new Auction
            {
                Auctioneer = userSeller,
                Product = product,
                StartPrice = startPrice,
                StartDate = DateTime.Now,
                EndDate = DateTime.Now.AddDays(5),
                Ended = false,
            };
            var auctionResult = this.auctionService.AddAuction(auction);
            var auctionUser2 = new AuctionUser { FirstName = "Ioana", LastName = "Pascu", Gender = "F" };
            var result2 = this.auctionUserService.AddAuctionUser(auctionUser2, Role.Buyer);
            var userBuyer = this.auctionUserService.GetAuctionUserByFistNameAndLastName(auctionUser2.FirstName, auctionUser2.LastName);
            var bidPrice = new Price { Currency = "Euro", Value = startPrice.Value + 1 };
            var bidPriceResult = this.priceService.AddPrice(bidPrice);
            var auctionById = this.auctionService.GetAuctionById(auction.Id);
            var bid = new Bid
            {
                Auction = auctionById,
                BidUser = userBuyer,
                BidPrice = bidPrice,
                BidDate = DateTime.Now
            };
            var resultBid = this.bidService.AddBid(bid);
            var bidById = this.bidService.GetBidById(bid.Id);
            var bidPrice2 = new Price { Id = 3, Currency = "Euro", Value = 108.5 };
            var bidPriceResult2 = this.priceService.AddPrice(bidPrice2);

            bidById.BidPrice = bidPrice2;
            var updateResult = this.bidService.UpdateBid(bidById);
            TestsUtil.UndoingChangesDbEntityLevel(this.libraryContext, bidById);

            bidById = this.bidService.GetBidById(bid.Id);
            Assert.IsTrue(bidById.BidPrice.Id == bidPrice2.Id);
        }

        /// <summary>
        /// Test update with a wrong auction for bid.
        /// </summary>
        [Test]
        public void TestUpdateWithBadBidPrice()
        {
            this.ClearDatabase();
            var auctionUser = new AuctionUser { FirstName = "Ionel", LastName = "Pascu", Gender = "M" };
            var result = this.auctionUserService.AddAuctionUser(auctionUser, Role.Seller);
            var userSeller = this.auctionUserService.GetAuctionUserByFistNameAndLastName(auctionUser.FirstName, auctionUser.LastName);
            var startPrice = new Price { Currency = "Euro", Value = 88.5 };
            var priceResult = this.priceService.AddPrice(startPrice);
            var category = new Category { Name = "Legume" };
            var categoryResult = this.categoryService.AddCategory(category);
            var product = new Product { Name = "Fasole", Category = new[] { category } };
            var productResult = this.productService.AddProduct(product);
            var auction = new Auction
            {
                Auctioneer = userSeller,
                Product = product,
                StartPrice = startPrice,
                StartDate = DateTime.Now,
                EndDate = DateTime.Now.AddDays(5),
                Ended = false,
            };
            var auctionResult = this.auctionService.AddAuction(auction);
            var auctionUser2 = new AuctionUser { FirstName = "Ioana", LastName = "Pascu", Gender = "F" };
            var result2 = this.auctionUserService.AddAuctionUser(auctionUser2, Role.Buyer);
            var userBuyer = this.auctionUserService.GetAuctionUserByFistNameAndLastName(auctionUser2.FirstName, auctionUser2.LastName);
            var bidPrice = new Price { Currency = "Euro", Value = startPrice.Value + 1 };
            var bidPriceResult = this.priceService.AddPrice(bidPrice);
            var auctionById = this.auctionService.GetAuctionById(auction.Id);
            var bid = new Bid
            {
                Auction = auctionById,
                BidUser = userBuyer,
                BidPrice = bidPrice,
                BidDate = DateTime.Now
            };
            var resultBid = this.bidService.AddBid(bid);
            var bidById = this.bidService.GetBidById(bid.Id);
            var bidPrice2 = new Price { Id = 3, Currency = "Dolar", Value = 108.5 };
            var bidPriceResult2 = this.priceService.AddPrice(bidPrice2);

            bidById.BidPrice = bidPrice2;
            var updateResult = this.bidService.UpdateBid(bidById);
            Assert.IsFalse(updateResult);
        }

        /// <summary>
        /// Test update date for bid.
        /// </summary>
        [Test]
        public void TestUpdateBidDate()
        {
            this.ClearDatabase();
            var auctionUser = new AuctionUser { Id = 1, FirstName = "Ionel", LastName = "Pascu", Gender = "M" };
            var result = this.auctionUserService.AddAuctionUser(auctionUser, Role.Seller);
            var userSeller = this.auctionUserService.GetAuctionUserByFistNameAndLastName(auctionUser.FirstName, auctionUser.LastName);
            var startPrice = new Price { Id = 1, Currency = "Euro", Value = 88.5 };
            var priceResult = this.priceService.AddPrice(startPrice);
            var category = new Category { Id = 1, Name = "Legume" };
            var categoryResult = this.categoryService.AddCategory(category);
            var product = new Product { Id = 1, Name = "Fasole", Category = new[] { category } };
            var productResult = this.productService.AddProduct(product);
            var auction = new Auction
            {
                Id = 1,
                Auctioneer = userSeller,
                Product = product,
                StartPrice = startPrice,
                StartDate = DateTime.Now,
                EndDate = DateTime.Now.AddDays(5),
                Ended = false,
            };
            var auctionResult = this.auctionService.AddAuction(auction);
            var auctionUser2 = new AuctionUser { Id = 2, FirstName = "Ioana", LastName = "Pascu", Gender = "F" };
            var result2 = this.auctionUserService.AddAuctionUser(auctionUser2, Role.Buyer);
            var userBuyer = this.auctionUserService.GetAuctionUserByFistNameAndLastName(auctionUser2.FirstName, auctionUser2.LastName);
            var bidPrice = new Price { Id = 2, Currency = "Euro", Value = startPrice.Value + 1 };
            var bidPriceResult = this.priceService.AddPrice(bidPrice);
            var auctionById = this.auctionService.GetAuctionById(1);
            var bidId = 1;
            var bidDate = DateTime.Now;
            var bid = new Bid
            {
                Id = bidId,
                Auction = auctionById,
                BidUser = userBuyer,
                BidPrice = bidPrice,
                BidDate = bidDate
            };
            var resultBid = this.bidService.AddBid(bid);
            var bidById = this.bidService.GetBidById(bidId);
            bidById.BidDate = bidDate.AddDays(2);
            var updateResult = this.bidService.UpdateBid(bidById);
            TestsUtil.UndoingChangesDbEntityLevel(this.libraryContext, bidById);
            bidById = this.bidService.GetBidById(bidId);
            Assert.IsTrue(DateTime.Compare(bidById.BidDate, bidDate) == 0);
        }

        /// <summary>
        /// Test update name for category.
        /// </summary>
        [Test]
        public void TestUpdateNameForCategory()
        {
            this.ClearDatabase();
            var category = new Category { Name = "Legume" };
            var resultCategory = this.categoryService.AddCategory(category);
            category = this.categoryService.GetCategoryByName(category.Name);
            category.Name = "Fasole";
            var updateResult = this.categoryService.UpdateCategory(category);
            TestsUtil.UndoingChangesDbEntityLevel(this.libraryContext, category);
            var categoryById = this.categoryService.GetCategoryById(category.Id);
            Assert.IsTrue(categoryById.Name.Equals("Fasole"));
        }

        /// <summary>
        /// Test update with a name for category.
        /// </summary>
        [Test]
        public void TestUpdateWithBadNameForCategory()
        {
            this.ClearDatabase();
            var category = new Category { Name = "Legume" };
            var resultCategory = this.categoryService.AddCategory(category);
            category.Name = "aa";
            var updateResult = this.categoryService.UpdateCategory(category);
            TestsUtil.UndoingChangesDbEntityLevel(this.libraryContext, category);
            var categoryById = this.categoryService.GetCategoryById(category.Id);
            Assert.IsFalse(categoryById.Name.Equals("aa"));
        }

        /// <summary>
        /// Test update parent category for category.
        /// </summary>
        [Test]
        public void TestUpdateParentCategoryForCategory()
        {
            this.ClearDatabase();
            var category = new Category { Name = "Legume" };
            var resultCategory = this.categoryService.AddCategory(category);
            var category2 = new Category { Name = "Electronice" };
            var resultCategory2 = this.categoryService.AddCategory(category2);
            var result = this.categoryService.AddCategory(category);
            category.ParentCategory = category2;
            var updateResult = this.categoryService.UpdateCategory(category);
            TestsUtil.UndoingChangesDbEntityLevel(this.libraryContext, category);
            var categoryByName = this.categoryService.GetCategoryById(category.Id);
            Assert.IsNotNull(categoryByName.ParentCategory);
        }

        /// <summary>
        /// Test update parent category for category to be root category.
        /// </summary>
        [Test]
        public void TestUpdateWithNullParentCategoryForCategory()
        {
            this.ClearDatabase();
            var category = new Category { Name = "Legume" };
            var resultCategory = this.categoryService.AddCategory(category);
            var category2 = new Category { Name = "Electronice", ParentCategory_Id = category.Id };
            var resultCategory2 = this.categoryService.AddCategory(category2);
            category2.ParentCategory = null;
            var updateResult = this.categoryService.UpdateCategory(category2);
            TestsUtil.UndoingChangesDbEntityLevel(this.libraryContext, category2);
            var categoryById = this.categoryService.GetCategoryById(category2.Id);
            Assert.IsNull(categoryById.ParentCategory);
        }

        /// <summary>
        /// Test update Currency for price.
        /// </summary>
        [Test]
        public void TestUpdateCurrencyForPrice()
        {
            this.ClearDatabase();
            var price = new Price { Currency = "Euro", Value = 54.5 };
            var result = this.priceService.AddPrice(price);
            var prices = this.priceService.GetPrices();
            var priceId = prices.ToList()[0].Id;
            var priceById = this.priceService.GetPriceById(priceId);
            priceById.Currency = "Dolar";
            var updateResult = this.priceService.UpdatePrice(price);
            TestsUtil.UndoingChangesDbEntityLevel(this.libraryContext, price);
            var priceUpdated = this.priceService.GetPriceById(priceId);
            Assert.IsTrue(priceUpdated.Currency.Equals("Dolar"));
        }

        /// <summary>
        /// Test update with a wrong currency for auction price.
        /// </summary>
        [Test]
        public void TestUpdateWithBadCurrencyForPrice()
        {
            this.ClearDatabase();
            var price = new Price { Currency = "Euro", Value = 54.5 };
            var result = this.priceService.AddPrice(price);
            var priceById = this.priceService.GetPriceById(price.Id);
            priceById.Currency = string.Empty;
            var updateResult = this.priceService.UpdatePrice(price);
            TestsUtil.UndoingChangesDbEntityLevel(this.libraryContext, price);
            var priceUpdated = this.priceService.GetPriceById(price.Id);
            Assert.IsFalse(priceUpdated.Currency.Equals("Dolar"));
        }

        /// <summary>
        /// Test update Value for price.
        /// </summary>
        [Test]
        public void TestUpdateValueForPrice()
        {
            this.ClearDatabase();
            var price = new Price { Currency = "Euro", Value = 54.5 };
            var result = this.priceService.AddPrice(price);
            var priceById = this.priceService.GetPriceById(price.Id);
            priceById.Value = 99.3;
            var updateResult = this.priceService.UpdatePrice(priceById);
            TestsUtil.UndoingChangesDbEntityLevel(this.libraryContext, price);
            var priceUpdated = this.priceService.GetPriceById(price.Id);
            Assert.IsTrue(double.Equals(priceUpdated.Value, 99.3));
        }

        /// <summary>
        /// Test update with a wrong Value for auction price.
        /// </summary>
        [Test]
        public void TestUpdateWithBadValueForPrice()
        {
            this.ClearDatabase();
            var price = new Price { Currency = "Euro", Value = 54.5 };
            var result = this.priceService.AddPrice(price);
            var priceById = this.priceService.GetPriceById(price.Id);
            priceById.Value = 0.0;
            var updateResult = this.priceService.UpdatePrice(priceById);
            TestsUtil.UndoingChangesDbEntityLevel(this.libraryContext, price);
            var priceUpdated = this.priceService.GetPriceById(price.Id);
            Assert.IsFalse(double.Equals(priceUpdated.Value, 0.0));
        }

        /// <summary>
        /// Test update name for auction user.
        /// </summary>
        [Test]
        public void TestUpdateNameForProduct()
        {
            this.ClearDatabase();
            var category = new Category { Name = "Legume" };
            var resultCategory = this.categoryService.AddCategory(category);
            var product = new Product { Name = "Varza", Category = new[] { category } };
            var result = this.productService.AddProduct(product);
            product.Name = "Fasole";
            var updateResult = this.productService.UpdateProduct(product);
            TestsUtil.UndoingChangesDbEntityLevel(this.libraryContext, product);
            var productById = this.productService.GetProductById(product.Id);
            Assert.IsTrue(productById.Name.Equals("Fasole"));
        }

        /// <summary>
        /// Test update with a name for auction user.
        /// </summary>
        [Test]
        public void TestUpdateWithBadNameForProduct()
        {
            this.ClearDatabase();
            var category = new Category { Name = "Legume" };
            var resultCategory = this.categoryService.AddCategory(category);
            var product = new Product { Name = "Varza", Category = new[] { category } };
            var result = this.productService.AddProduct(product);
            product.Name = "aa";
            var updateResult = this.productService.UpdateProduct(product);
            TestsUtil.UndoingChangesDbEntityLevel(this.libraryContext, product);
            var productById = this.productService.GetProductById(product.Id);
            Assert.IsFalse(productById.Name.Equals("aa"));
        }

        /// <summary>
        /// Test update categories for auction user.
        /// </summary>
        [Test]
        public void TestUpdateCategoriesForProduct()
        {
            this.ClearDatabase();
            var category = new Category { Name = "Legume" };
            var resultCategory = this.categoryService.AddCategory(category);
            var category2 = new Category { Name = "Electronice" };
            var resultCategory2 = this.categoryService.AddCategory(category2);
            var categoryList = new List<Category>();
            categoryList.Add(category);
            var product = new Product { Name = "Varza", Category = categoryList };
            var result = this.productService.AddProduct(product);
            var productById = this.productService.GetProductById(product.Id);
            var categoryList2 = new List<Category>();
            categoryList2.Add(category2);
            productById.Category = categoryList2;
            var updateResult = this.productService.UpdateProduct(product);
            TestsUtil.UndoingChangesDbEntityLevel(this.libraryContext, product);
            productById = this.productService.GetProductById(product.Id);
            Assert.IsTrue(productById.Category.ToList()[0].Name.Equals("Electronice"));
        }

        /// <summary>
        /// Test update name for product.
        /// </summary>
        [Test]
        public void TestUpdateCategoriesForProductToEmptyList()
        {
            this.ClearDatabase();
            var category = new Category { Name = "Legume" };
            var resultCategory = this.categoryService.AddCategory(category);
            var category2 = new Category { Name = "Electronice" };
            var resultCategory2 = this.categoryService.AddCategory(category2);
            var categoryList = new List<Category>();
            categoryList.Add(category);
            var product = new Product { Name = "Varza", Category = categoryList };
            var result = this.productService.AddProduct(product);
            var productById = this.productService.GetProductById(product.Id);
            productById.Category = new List<Category>();
            var updateResult = this.productService.UpdateProduct(product);
            Assert.IsFalse(updateResult);
        }

        /// <summary>
        /// Test update description for auction user.
        /// </summary>
        [Test]
        public void TestUpdateDescriptionForUserReview()
        {
            this.ClearDatabase();
            var userReview = new AuctionUser { FirstName = "Ionel", LastName = "Pascu", Gender = "M" };
            var reviewUser = new AuctionUser { FirstName = "Popa", LastName = "Andrei", Gender = "M" };
            var resultUser1 = this.auctionUserService.AddAuctionUser(userReview, Role.Buyer);
            var resultUser2 = this.auctionUserService.AddAuctionUser(reviewUser, Role.Buyer);
            var user1 = this.auctionUserService.GetAuctionUserByFistNameAndLastName("Ionel", "Pascu");
            var user2 = this.auctionUserService.GetAuctionUserByFistNameAndLastName("Popa", "Andrei");

            var review1 = new UserReview
            { Description = "He is a good man", Score = 4, ReviewByUser = user2, ReviewForUser = user1 };
            var resultReview1 = this.userReviewService.AddUserReview(review1);
            var reviewById = this.userReviewService.GetUserReviewById(review1.Id);
            reviewById.Description = "New description";
            var review = this.userReviewService.UpdateUserReview(reviewById);
            TestsUtil.UndoingChangesDbEntityLevel(this.libraryContext, reviewById);
            var newReview = this.userReviewService.GetUserReviewById(reviewById.Id);
            Assert.IsTrue(newReview.Description.Equals("New description"));
        }

        /// <summary>
        /// Test update with a description for auction user.
        /// </summary>
        [Test]
        public void TestUpdateWithBadDescriptionForUserReview()
        {
            this.ClearDatabase();
            var userReview = new AuctionUser { FirstName = "Ionel", LastName = "Pascu", Gender = "M" };
            var reviewUser = new AuctionUser { FirstName = "Popa", LastName = "Andrei", Gender = "M" };
            var resultUser1 = this.auctionUserService.AddAuctionUser(userReview, Role.Buyer);
            var resultUser2 = this.auctionUserService.AddAuctionUser(reviewUser, Role.Buyer);
            var user1 = this.auctionUserService.GetAuctionUserByFistNameAndLastName("Ionel", "Pascu");
            var user2 = this.auctionUserService.GetAuctionUserByFistNameAndLastName("Popa", "Andrei");

            var review1 = new UserReview
            { Description = "He is a good man", Score = 4, ReviewByUser = user2, ReviewForUser = user1 };
            var resultReview1 = this.userReviewService.AddUserReview(review1);
            var reviewById = this.userReviewService.GetUserReviewById(review1.Id);
            reviewById.Description = null;
            var review = this.userReviewService.UpdateUserReview(reviewById);
            TestsUtil.UndoingChangesDbEntityLevel(this.libraryContext, reviewById);
            var newReview = this.userReviewService.GetUserReviewById(reviewById.Id);
            Assert.IsTrue(newReview.Description.Equals("He is a good man"));
        }

        /// <summary>
        /// Test update score for auction user.
        /// </summary>
        [Test]
        public void TestUpdateScoreForUserReview()
        {
            this.ClearDatabase();
            var userReview = new AuctionUser { FirstName = "Ionel", LastName = "Pascu", Gender = "M" };
            var reviewUser = new AuctionUser { FirstName = "Popa", LastName = "Andrei", Gender = "M" };
            var resultUser1 = this.auctionUserService.AddAuctionUser(userReview, Role.Buyer);
            var resultUser2 = this.auctionUserService.AddAuctionUser(reviewUser, Role.Buyer);
            var user1 = this.auctionUserService.GetAuctionUserByFistNameAndLastName("Ionel", "Pascu");
            var user2 = this.auctionUserService.GetAuctionUserByFistNameAndLastName("Popa", "Andrei");

            var review1 = new UserReview
            { Description = "He is a good man", Score = 4, ReviewByUser = user2, ReviewForUser = user1 };
            var resultReview1 = this.userReviewService.AddUserReview(review1);
            var reviewById = this.userReviewService.GetUserReviewById(review1.Id);
            reviewById.Score = 7;
            var review = this.userReviewService.UpdateUserReview(reviewById);
            TestsUtil.UndoingChangesDbEntityLevel(this.libraryContext, reviewById);
            var newReview = this.userReviewService.GetUserReviewById(reviewById.Id);
            Assert.IsTrue(newReview.Score == 7);
        }

        /// <summary>
        /// Test update with a wrong score for auction user.
        /// </summary>
        [Test]
        public void TestUpdateWithBadScoreForUserReview()
        {
            this.ClearDatabase();
            var userReview = new AuctionUser { FirstName = "Ionel", LastName = "Pascu", Gender = "M" };
            var reviewUser = new AuctionUser { FirstName = "Popa", LastName = "Andrei", Gender = "M" };
            var resultUser1 = this.auctionUserService.AddAuctionUser(userReview, Role.Buyer);
            var resultUser2 = this.auctionUserService.AddAuctionUser(reviewUser, Role.Buyer);
            var user1 = this.auctionUserService.GetAuctionUserByFistNameAndLastName("Ionel", "Pascu");
            var user2 = this.auctionUserService.GetAuctionUserByFistNameAndLastName("Popa", "Andrei");

            var review1 = new UserReview
            { Description = "He is a good man", Score = 4, ReviewByUser = user2, ReviewForUser = user1 };
            var resultReview1 = this.userReviewService.AddUserReview(review1);
            var reviewById = this.userReviewService.GetUserReviewById(review1.Id);
            reviewById.Score = -3;
            var review = this.userReviewService.UpdateUserReview(reviewById);
            TestsUtil.UndoingChangesDbEntityLevel(this.libraryContext, reviewById);
            var newReview = this.userReviewService.GetUserReviewById(reviewById.Id);
            Assert.IsFalse(newReview.Score == -3);
        }

        /// <summary>
        /// Test update with a user for auction user.
        /// </summary>
        [Test]
        public void TestUpdateWithGoodByUserForUserReview()
        {
            this.ClearDatabase();
            var userReview = new AuctionUser { FirstName = "Ionel", LastName = "Pascu", Gender = "M" };
            var reviewUser = new AuctionUser { FirstName = "Popa", LastName = "Andrei", Gender = "M" };
            var reviewUser3 = new AuctionUser { FirstName = "Popa", LastName = "Danut", Gender = "M" };

            var resultUser1 = this.auctionUserService.AddAuctionUser(userReview, Role.Buyer);
            var resultUser2 = this.auctionUserService.AddAuctionUser(reviewUser, Role.Buyer);
            var resultUser3 = this.auctionUserService.AddAuctionUser(reviewUser3, Role.Seller);

            var user1 = this.auctionUserService.GetAuctionUserByFistNameAndLastName("Ionel", "Pascu");
            var user2 = this.auctionUserService.GetAuctionUserByFistNameAndLastName("Popa", "Andrei");
            var user3 = this.auctionUserService.GetAuctionUserByFistNameAndLastName("Popa", "Danut");

            var review1 = new UserReview
            { Description = "He is a good man", Score = 4, ReviewByUser = user2, ReviewForUser = user1 };
            var resultReview1 = this.userReviewService.AddUserReview(review1);
            var reviewById = this.userReviewService.GetUserReviewById(review1.Id);
            reviewById.ReviewByUser = user3;
            var review = this.userReviewService.UpdateUserReview(reviewById);
            TestsUtil.UndoingChangesDbEntityLevel(this.libraryContext, reviewById);
            var newReview = this.userReviewService.GetUserReviewById(reviewById.Id);
            Assert.IsTrue(newReview.ReviewByUser.Id == user3.Id);
        }

        /// <summary>
        /// Test update with a wrong user for auction user.
        /// </summary>
        [Test]
        public void TestUpdateWithBadByUserForUserReview()
        {
            this.ClearDatabase();
            var userReview = new AuctionUser { FirstName = "Ionel", LastName = "Pascu", Gender = "M" };
            var reviewUser = new AuctionUser { FirstName = "Popa", LastName = "Andrei", Gender = "M" };
            var resultUser1 = this.auctionUserService.AddAuctionUser(userReview, Role.Buyer);
            var resultUser2 = this.auctionUserService.AddAuctionUser(reviewUser, Role.Buyer);
            var user1 = this.auctionUserService.GetAuctionUserByFistNameAndLastName("Ionel", "Pascu");
            var user2 = this.auctionUserService.GetAuctionUserByFistNameAndLastName("Popa", "Andrei");

            var review1 = new UserReview
            { Description = "He is a good man", Score = 4, ReviewByUser = user2, ReviewForUser = user1 };
            var resultReview1 = this.userReviewService.AddUserReview(review1);
            var reviewById = this.userReviewService.GetUserReviewById(review1.Id);
            reviewById.ReviewByUser = user1;
            var updateResult = this.userReviewService.UpdateUserReview(reviewById);
            Assert.IsFalse(updateResult);
        }

        /// <summary>
        /// Test update with a user for auction user.
        /// </summary>
        [Test]
        public void TestUpdateWithGoodForUserForUserReview()
        {
            this.ClearDatabase();
            var userReview = new AuctionUser { FirstName = "Ionel", LastName = "Pascu", Gender = "M" };
            var reviewUser = new AuctionUser { FirstName = "Popa", LastName = "Andrei", Gender = "M" };
            var reviewUser3 = new AuctionUser { FirstName = "Popa", LastName = "Danut", Gender = "M" };

            var resultUser1 = this.auctionUserService.AddAuctionUser(userReview, Role.Buyer);
            var resultUser2 = this.auctionUserService.AddAuctionUser(reviewUser, Role.Buyer);
            var resultUser3 = this.auctionUserService.AddAuctionUser(reviewUser3, Role.Seller);

            var user1 = this.auctionUserService.GetAuctionUserByFistNameAndLastName("Ionel", "Pascu");
            var user2 = this.auctionUserService.GetAuctionUserByFistNameAndLastName("Popa", "Andrei");
            var user3 = this.auctionUserService.GetAuctionUserByFistNameAndLastName("Popa", "Danut");

            var review1 = new UserReview
            { Description = "He is a good man", Score = 4, ReviewByUser = user2, ReviewForUser = user1 };
            var resultReview1 = this.userReviewService.AddUserReview(review1);
            var reviewById = this.userReviewService.GetUserReviewById(review1.Id);
            reviewById.ReviewForUser = user3;
            var review = this.userReviewService.UpdateUserReview(reviewById);
            TestsUtil.UndoingChangesDbEntityLevel(this.libraryContext, reviewById);
            var newReview = this.userReviewService.GetUserReviewById(reviewById.Id);
            Assert.IsTrue(newReview.ReviewForUser.Id == user3.Id);
        }

        /// <summary>
        /// Test update with a wrong user for auction user.
        /// </summary>
        [Test]
        public void TestUpdateWithBadForUserForUserReview()
        {
            this.ClearDatabase();
            var userReview = new AuctionUser { FirstName = "Ionel", LastName = "Pascu", Gender = "M" };
            var reviewUser = new AuctionUser { FirstName = "Popa", LastName = "Andrei", Gender = "M" };
            var resultUser1 = this.auctionUserService.AddAuctionUser(userReview, Role.Buyer);
            var resultUser2 = this.auctionUserService.AddAuctionUser(reviewUser, Role.Buyer);
            var user1 = this.auctionUserService.GetAuctionUserByFistNameAndLastName("Ionel", "Pascu");
            var user2 = this.auctionUserService.GetAuctionUserByFistNameAndLastName("Popa", "Andrei");

            var review1 = new UserReview
            { Description = "He is a good man", Score = 4, ReviewByUser = user2, ReviewForUser = user1 };
            var resultReview1 = this.userReviewService.AddUserReview(review1);
            var reviewById = this.userReviewService.GetUserReviewById(review1.Id);
            reviewById.ReviewForUser = user2;
            var updateResult = this.userReviewService.UpdateUserReview(reviewById);
            Assert.IsFalse(updateResult);
        }

        /// <summary>
        /// The ClearDatabase.
        /// </summary>
        private void ClearDatabase()
        {
            this.libraryContext.Auctions.RemoveRange(this.libraryContext.Auctions);
            this.libraryContext.AuctionUsers.RemoveRange(this.libraryContext.AuctionUsers);
            this.libraryContext.Categories.RemoveRange(this.libraryContext.Categories);
            this.libraryContext.Products.RemoveRange(this.libraryContext.Products);
            this.libraryContext.Prices.RemoveRange(this.libraryContext.Prices);
            this.libraryContext.UserReviews.RemoveRange(this.libraryContext.UserReviews);
            this.libraryContext.Bids.RemoveRange(this.libraryContext.Bids);
        }
    }
}