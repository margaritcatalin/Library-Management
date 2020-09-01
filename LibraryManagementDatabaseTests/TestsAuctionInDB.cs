// <copyright file="TestsAuctionInDB.cs" company="Transilvania University of Brasov">
// Margarit Marian Catalin
// </copyright>

namespace LibraryManagementDatabaseTests
{
    using System;
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
    public class TestsAuctionInDB
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
        /// Test check if auction is ended.
        /// </summary>
        [Test]
        public void TestCheckIfAuctionIsEnded()
        {
            this.ClearDatabase();
            var auctionId = 1;
            var auctionUser = new AuctionUser { FirstName = "Ionel", LastName = "Pascu", Gender = "M" };
            var result2 = this.auctionUserService.AddAuctionUser(auctionUser, Role.Seller);
            var startPrice = new Price { Currency = "Euro", Value = 88.5 };
            var priceResult = this.priceService.AddPrice(startPrice);
            var category = new Category { Name = "Legume" };
            var categoryResult = this.categoryService.AddCategory(category);
            var product = new Product { Name = "Fasole", Category = new[] { category } };
            var productResult = this.productService.AddProduct(product);
            var startDate = DateTime.Now;
            var endDate = DateTime.Now.AddDays(5);
            var auction = new Auction
            {
                Id = auctionId,
                Auctioneer = auctionUser,
                Product = product,
                StartPrice = startPrice,
                StartDate = startDate,
                EndDate = endDate,
                Ended = false,
            };
            var auctionResult = this.auctionService.AddAuction(auction);
            var auctionUserSaved =
                this.auctionUserService.GetAuctionUserByFistNameAndLastName(auctionUser.FirstName, auctionUser.LastName);
            var endResult = this.auctionService.EndAuctionByUser(auction, auctionUser);
            var isEnded = this.auctionService.CheckIfAuctionIsEnded(auction);
            Assert.IsTrue(isEnded);
        }

        /// <summary>
        /// Test End auction by user and it is already ended.
        /// </summary>
        [Test]
        public void TestEndAuctionByUserAlreadyEnded()
        {
            this.ClearDatabase();
            var auctionId = 1;
            var auctionUser = new AuctionUser { FirstName = "Ionel", LastName = "Pascu", Gender = "M" };
            var result2 = this.auctionUserService.AddAuctionUser(auctionUser, Role.Seller);
            auctionUser =
                this.auctionUserService.GetAuctionUserByFistNameAndLastName(auctionUser.FirstName, auctionUser.LastName);
            var startPrice = new Price { Currency = "Euro", Value = 88.5 };
            var priceResult = this.priceService.AddPrice(startPrice);
            var category = new Category { Name = "Legume" };
            var categoryResult = this.categoryService.AddCategory(category);
            var product = new Product { Name = "Fasole", Category = new[] { category } };
            var productResult = this.productService.AddProduct(product);
            var auction = new Auction
            {
                Id = auctionId,
                Auctioneer = auctionUser,
                Product = product,
                StartPrice = startPrice,
                StartDate = DateTime.Now,
                EndDate = DateTime.Now.AddDays(5),
                Ended = false,
            };
            var auctionResult = this.auctionService.AddAuction(auction);
            var auctionById = this.auctionService.GetAuctionById(auctionId);
            var isEnded = this.auctionService.EndAuctionByUser(auctionById, auctionUser);
            Assert.IsTrue(isEnded);
            auctionById = this.auctionService.GetAuctionById(auctionId);
            var isEnded2 = this.auctionService.EndAuctionByUser(auctionById, auctionUser);
            Assert.IsFalse(isEnded2);
        }

        /// <summary>
        /// Test End auction by user.
        /// </summary>
        [Test]
        public void TestEndAuctionByAnotherUser()
        {
            this.ClearDatabase();
            var auctionId = 1;
            var auctionUser = new AuctionUser { FirstName = "Ionel", LastName = "Pascu", Gender = "M" };
            var result2 = this.auctionUserService.AddAuctionUser(auctionUser, Role.Seller);
            auctionUser =
                this.auctionUserService.GetAuctionUserByFistNameAndLastName(auctionUser.FirstName, auctionUser.LastName);
            var startPrice = new Price { Currency = "Euro", Value = 88.5 };
            var priceResult = this.priceService.AddPrice(startPrice);
            var category = new Category { Name = "Legume" };
            var categoryResult = this.categoryService.AddCategory(category);
            var product = new Product { Name = "Fasole", Category = new[] { category } };
            var productResult = this.productService.AddProduct(product);
            var auction = new Auction
            {
                Id = auctionId,
                Auctioneer = auctionUser,
                Product = product,
                StartPrice = startPrice,
                StartDate = DateTime.Now,
                EndDate = DateTime.Now.AddDays(5),
                Ended = false,
            };
            var auctionResult = this.auctionService.AddAuction(auction);
            var auctionById = this.auctionService.GetAuctionById(auctionId);
            var auctionUser2 = new AuctionUser { Id = 2, FirstName = "Manuel", LastName = "Grigore", Gender = "M" };
            var resultU2 = this.auctionUserService.AddAuctionUser(auctionUser2, Role.Seller);
            auctionUser2 =
                this.auctionUserService.GetAuctionUserByFistNameAndLastName(auctionUser2.FirstName, auctionUser2.LastName);
            var isEnded = this.auctionService.EndAuctionByUser(auctionById, auctionUser2);
            auctionById = this.auctionService.GetAuctionById(auctionId);
            Assert.IsFalse(auctionById.Ended);
        }

        /// <summary>
        /// Test End auction by user.
        /// </summary>
        [Test]
        public void TestEndAuctionByUser()
        {
            this.ClearDatabase();
            var auctionId = 1;
            var auctionUser = new AuctionUser { FirstName = "Ionel", LastName = "Pascu", Gender = "M" };
            var result2 = this.auctionUserService.AddAuctionUser(auctionUser, Role.Seller);
            auctionUser =
                this.auctionUserService.GetAuctionUserByFistNameAndLastName(auctionUser.FirstName, auctionUser.LastName);
            var startPrice = new Price { Currency = "Euro", Value = 88.5 };
            var priceResult = this.priceService.AddPrice(startPrice);
            var category = new Category { Name = "Legume" };
            var categoryResult = this.categoryService.AddCategory(category);
            var product = new Product { Name = "Fasole", Category = new[] { category } };
            var productResult = this.productService.AddProduct(product);
            var auction = new Auction
            {
                Id = auctionId,
                Auctioneer = auctionUser,
                Product = product,
                StartPrice = startPrice,
                StartDate = DateTime.Now,
                EndDate = DateTime.Now.AddDays(5),
                Ended = false,
            };
            var auctionResult = this.auctionService.AddAuction(auction);
            var auctionById = this.auctionService.GetAuctionById(auctionId);
            var isEnded = this.auctionService.EndAuctionByUser(auctionById, auctionUser);
            auctionById = this.auctionService.GetAuctionById(auctionId);
            Assert.IsTrue(auctionById.Ended);
        }

        /// <summary>
        /// Test Get Number of all started auction for user.
        /// </summary>
        [Test]
        public void TestGetAllAuctionsUnderEndOne()
        {
            this.ClearDatabase();
            var auctionUser = new AuctionUser { FirstName = "Ionel", LastName = "Pascu", Gender = "M" };
            var result = this.auctionUserService.AddAuctionUser(auctionUser, Role.Seller);
            var startPrice = new Price { Currency = "Euro", Value = 88.5 };
            var priceResult = this.priceService.AddPrice(startPrice);
            var maxNumberOfMonths = int.Parse(ConfigurationManager.AppSettings["MAX_NUMBER_OF_MONTHS"]);
            var startDate = DateTime.Now;
            for (var i = 1; i <= 4; i++)
            {
                var category = new Category { Name = "Legume" + i };
                var categoryResult = this.categoryService.AddCategory(category);
                var product = new Product { Name = "Fasole" + i, Category = new[] { category } };
                var productResult = this.productService.AddProduct(product);
                var auction = new Auction
                {
                    Auctioneer = auctionUser,
                    Product = product,
                    StartPrice = startPrice,
                    StartDate = startDate,
                    EndDate = startDate.AddDays(6),
                    Ended = false,
                };
                this.auctionService.AddAuction(auction);
            }

            var auctions = this.auctionService.GetAuctions();
            var endResult = this.auctionService.EndAuctionByUser(auctions.ToList()[0], auctionUser);
            auctions = this.auctionService.GetAuctions();
            Assert.True(auctions.Count() == 4);
        }

        /// <summary>
        /// Test Get Auction last price.
        /// </summary>
        [Test]
        public void TestGetAuctionLastPriceWithBids()
        {
            this.ClearDatabase();
            var auctionUser = new AuctionUser { FirstName = "Ionel", LastName = "Pascu", Gender = "M" };
            var result2 = this.auctionUserService.AddAuctionUser(auctionUser, Role.Seller);
            var startPrice = new Price { Currency = "Euro", Value = 88.5 };
            var priceResult = this.priceService.AddPrice(startPrice);
            var category = new Category { Name = "Legume" };
            var categoryResult = this.categoryService.AddCategory(category);
            var product = new Product { Name = "Fasole", Category = new[] { category } };
            var productResult = this.productService.AddProduct(product);
            var auction = new Auction
            {
                Auctioneer = auctionUser,
                Product = product,
                StartPrice = startPrice,
                StartDate = DateTime.Now,
                EndDate = DateTime.Now.AddDays(5),
                Ended = false,
            };
            var auctionResult = this.auctionService.AddAuction(auction);
            var auctionUserById = this.auctionService.GetAuctionById(auction.Id);
            var auctionUser2 = new AuctionUser { FirstName = "Ioana", LastName = "Pascu", Gender = "F" };
            var resultAuctionUser2 = this.auctionUserService.AddAuctionUser(auctionUser2, Role.Buyer);
            var reviewUser = this.auctionUserService.GetAuctionUserByFistNameAndLastName("Ioana", "Pascu");

            var bidPrice = new Price { Currency = "Euro", Value = startPrice.Value + 1 };
            var bidPriceResult = this.priceService.AddPrice(bidPrice);
            var bid = new Bid { Auction = auction, BidUser = reviewUser, BidPrice = bidPrice, BidDate = DateTime.Now };
            var resultBid = this.bidService.AddBid(bid);
            auctionUserById = this.auctionService.GetAuctionById(auction.Id);
            var lastPrice = this.auctionService.GetAuctionLastPrice(auctionUserById);
            Assert.IsTrue(lastPrice.Id == bidPrice.Id);
        }

        /// <summary>
        /// Test get last auctions with one of it ended.
        /// </summary>
        [Test]
        public void TestGetLastAuctionForUserWithEndedOne()
        {
            this.ClearDatabase();
            var auctionUser = new AuctionUser { FirstName = "Ionel", LastName = "Pascu", Gender = "M" };
            var result2 = this.auctionUserService.AddAuctionUser(auctionUser, Role.Seller);
            var startPrice = new Price { Currency = "Euro", Value = 88.5 };
            var priceResult = this.priceService.AddPrice(startPrice);
            var category = new Category { Name = "Legume" };
            var categoryResult = this.categoryService.AddCategory(category);
            var product = new Product { Name = "Fasole", Category = new[] { category } };
            var productResult = this.productService.AddProduct(product);
            var startDate = DateTime.Now;
            var endDate = DateTime.Now.AddDays(3);
            var auction = new Auction
            {
                Auctioneer = auctionUser,
                Product = product,
                StartPrice = startPrice,
                StartDate = startDate,
                EndDate = endDate,
                Ended = false,
            };
            var auctionResult = this.auctionService.AddAuction(auction);

            var startPrice2 = new Price { Currency = "Euro", Value = 848.5 };
            var priceResult2 = this.priceService.AddPrice(startPrice2);
            var category2 = new Category { Name = "Mere" };
            var categoryResult2 = this.categoryService.AddCategory(category2);
            var product2 = new Product { Name = "Caise", Category = new[] { category2 } };
            var productResult2 = this.productService.AddProduct(product2);
            var startDate2 = DateTime.Now;
            var endDate2 = DateTime.Now.AddDays(4);
            var auction2 = new Auction
            {
                Auctioneer = auctionUser,
                Product = product2,
                StartPrice = startPrice2,
                StartDate = startDate2,
                EndDate = endDate2,
                Ended = false,
            };
            var auctionResult2 = this.auctionService.AddAuction(auction2);
            var isEnded = this.auctionService.EndAuctionByUser(auction2, auctionUser);
            var lastAuction = this.auctionService.GetLastAuctionForUser(auctionUser);
            Assert.IsTrue(lastAuction.Id == auction.Id);
        }

        /// <summary>
        /// Test get last auctions with more than one.
        /// </summary>
        [Test]
        public void TestGetLastAuctionForUserWithMoreThanOneAuctions()
        {
            this.ClearDatabase();
            var auctionUser = new AuctionUser { FirstName = "Ionel", LastName = "Pascu", Gender = "M" };
            var result2 = this.auctionUserService.AddAuctionUser(auctionUser, Role.Seller);
            var startPrice = new Price { Currency = "Euro", Value = 88.5 };
            var priceResult = this.priceService.AddPrice(startPrice);
            var category = new Category { Name = "Legume" };
            var categoryResult = this.categoryService.AddCategory(category);
            var product = new Product { Name = "Fasole", Category = new[] { category } };
            var productResult = this.productService.AddProduct(product);
            var startDate = DateTime.Now;
            var endDate = DateTime.Now.AddDays(3);
            var auction = new Auction
            {
                Auctioneer = auctionUser,
                Product = product,
                StartPrice = startPrice,
                StartDate = startDate,
                EndDate = endDate,
                Ended = false,
            };
            var auctionResult = this.auctionService.AddAuction(auction);

            var startPrice2 = new Price { Currency = "Euro", Value = 848.5 };
            var priceResult2 = this.priceService.AddPrice(startPrice2);
            var category2 = new Category { Name = "Mere" };
            var categoryResult2 = this.categoryService.AddCategory(category2);
            var product2 = new Product { Name = "Caise", Category = new[] { category2 } };
            var productResult2 = this.productService.AddProduct(product2);
            var startDate2 = DateTime.Now;
            var endDate2 = DateTime.Now.AddDays(4);
            var auction2 = new Auction
            {
                Auctioneer = auctionUser,
                Product = product2,
                StartPrice = startPrice2,
                StartDate = startDate2,
                EndDate = endDate2,
                Ended = false,
            };
            var auctionResult2 = this.auctionService.AddAuction(auction2);
            var lastAuction = this.auctionService.GetLastAuctionForUser(auctionUser);
            Assert.IsTrue(lastAuction.Id == auction2.Id);
        }

        /// <summary>
        /// Test Get All started auction for user in a specific category.
        /// </summary>
        [Test]
        public void TestGetStartedAuctionByUserAndCategoryUnderEndOne()
        {
            this.ClearDatabase();
            var auctionUser = new AuctionUser { FirstName = "Ionel", LastName = "Pascu", Gender = "M" };
            var result = this.auctionUserService.AddAuctionUser(auctionUser, Role.Seller);
            var startPrice = new Price { Currency = "Euro", Value = 88.5 };
            var priceResult = this.priceService.AddPrice(startPrice);
            var maxNumberOfMonths = int.Parse(ConfigurationManager.AppSettings["MAX_NUMBER_OF_MONTHS"]);
            var startDate = DateTime.Now;
            var category = new Category { Name = "Legume" };
            var categoryResult = this.categoryService.AddCategory(category);
            for (var i = 1; i <= 3; i++)
            {
                var product = new Product { Name = "Fasole" + i, Category = new[] { category } };
                var productResult = this.productService.AddProduct(product);
                var auction = new Auction
                {
                    Auctioneer = auctionUser,
                    Product = product,
                    StartPrice = startPrice,
                    StartDate = startDate,
                    EndDate = startDate.AddDays(6),
                    Ended = false,
                };
                this.auctionService.AddAuction(auction);
            }

            var startedAuctions = this.auctionService.GetStartedAuctionsByUserAndCategory(auctionUser, category);
            var endResult = this.auctionService.EndAuctionByUser(startedAuctions.ToList()[0], auctionUser);
            startedAuctions = this.auctionService.GetStartedAuctionsByUserAndCategory(auctionUser, category);
            Assert.True(startedAuctions.Count() == 2);
        }

        /// <summary>
        /// Test Get Number of all started auction for user.
        /// </summary>
        [Test]
        public void TestGetStartedAuctionByUserUnderEndOne()
        {
            this.ClearDatabase();
            var auctionUser = new AuctionUser { FirstName = "Ionel", LastName = "Pascu", Gender = "M" };
            var result = this.auctionUserService.AddAuctionUser(auctionUser, Role.Seller);
            var startPrice = new Price { Currency = "Euro", Value = 88.5 };
            var priceResult = this.priceService.AddPrice(startPrice);
            var maxNumberOfMonths = int.Parse(ConfigurationManager.AppSettings["MAX_NUMBER_OF_MONTHS"]);
            var startDate = DateTime.Now;
            for (var i = 1; i <= 4; i++)
            {
                var category = new Category { Name = "Legume" + i };
                var categoryResult = this.categoryService.AddCategory(category);
                var product = new Product { Name = "Fasole" + i, Category = new[] { category } };
                var productResult = this.productService.AddProduct(product);
                var auction = new Auction
                {
                    Auctioneer = auctionUser,
                    Product = product,
                    StartPrice = startPrice,
                    StartDate = startDate,
                    EndDate = startDate.AddDays(6),
                    Ended = false,
                };
                this.auctionService.AddAuction(auction);
            }

            var startedAuctions = this.auctionService.GetStartedAuctionsByUser(auctionUser);
            var endResult = this.auctionService.EndAuctionByUser(startedAuctions.ToList()[0], auctionUser);
            startedAuctions = this.auctionService.GetStartedAuctionsByUser(auctionUser);
            Assert.True(startedAuctions.Count() == 3);
        }

        /// <summary>
        /// Test Update auction startPrice until bid.
        /// </summary>
        [Test]
        public void TestUpdateAuctionStartPriceUnderBid()
        {
            this.ClearDatabase();
            var auctionUser = new AuctionUser { FirstName = "Ionel", LastName = "Pascu", Gender = "M" };
            var result2 = this.auctionUserService.AddAuctionUser(auctionUser, Role.Seller);
            var startPrice = new Price { Currency = "Euro", Value = 88.5 };
            var priceResult = this.priceService.AddPrice(startPrice);
            var category = new Category { Name = "Legume" };
            var categoryResult = this.categoryService.AddCategory(category);
            var product = new Product { Name = "Fasole", Category = new[] { category } };
            var productResult = this.productService.AddProduct(product);
            var auction = new Auction
            {
                Auctioneer = auctionUser,
                Product = product,
                StartPrice = startPrice,
                StartDate = DateTime.Now,
                EndDate = DateTime.Now.AddDays(5),
                Ended = false,
            };
            var auctionResult = this.auctionService.AddAuction(auction);
            var auctionUserById = this.auctionService.GetAuctionById(auction.Id);
            var auctionUser2 = new AuctionUser { FirstName = "Ioana", LastName = "Pascu", Gender = "F" };
            var resultAuctionUser2 = this.auctionUserService.AddAuctionUser(auctionUser2, Role.Buyer);
            var userBuyer = this.auctionUserService.GetAuctionUserByFistNameAndLastName(auctionUser2.FirstName, auctionUser2.LastName);
            var bidPrice = new Price { Currency = "Euro", Value = startPrice.Value + 1 };
            var bidPriceResult = this.priceService.AddPrice(bidPrice);
            var bid = new Bid { Auction = auction, BidUser = userBuyer, BidPrice = bidPrice, BidDate = DateTime.Now };
            var resultBid = this.bidService.AddBid(bid);
            var startPrice2 = new Price { Currency = "Dolar", Value = 444.5 };
            var priceResult2 = this.priceService.AddPrice(startPrice2);
            auctionUserById.StartPrice = startPrice2;
            var updateResult = this.auctionService.UpdateAuction(auctionUserById);
            Assert.IsFalse(updateResult);
        }

        /// <summary>
        /// Test Update auction start date.
        /// </summary>
        [Test]
        public void TestUpdateAuctionStartDate()
        {
            this.ClearDatabase();
            var auctionUser = new AuctionUser { FirstName = "Ionel", LastName = "Pascu", Gender = "M" };
            var result2 = this.auctionUserService.AddAuctionUser(auctionUser, Role.Seller);
            var startPrice = new Price { Currency = "Euro", Value = 88.5 };
            var priceResult = this.priceService.AddPrice(startPrice);
            var category = new Category { Name = "Legume" };
            var categoryResult = this.categoryService.AddCategory(category);
            var product = new Product { Name = "Fasole", Category = new[] { category } };
            var productResult = this.productService.AddProduct(product);
            var startDate = DateTime.Now;
            var auction = new Auction
            {
                Auctioneer = auctionUser,
                Product = product,
                StartPrice = startPrice,
                StartDate = startDate,
                EndDate = DateTime.Now.AddDays(5),
                Ended = false,
            };
            var auctionResult = this.auctionService.AddAuction(auction);
            var auctionUserById = this.auctionService.GetAuctionById(auction.Id);
            auctionUserById.StartDate = DateTime.Now.AddDays(2);
            this.auctionService.UpdateAuction(auctionUserById);
            auctionUserById = this.auctionService.GetAuctionById(auction.Id);
            Assert.IsFalse(DateTime.Compare(auctionUserById.StartDate, startDate) == 0);
        }

        /// <summary>
        /// Test Update auction bad start date.
        /// </summary>
        [Test]
        public void TestUpdateAuctionBadStartDate()
        {
            this.ClearDatabase();
            var auctionUser = new AuctionUser { FirstName = "Ionel", LastName = "Pascu", Gender = "M" };
            var result2 = this.auctionUserService.AddAuctionUser(auctionUser, Role.Seller);
            var startPrice = new Price { Currency = "Euro", Value = 88.5 };
            var priceResult = this.priceService.AddPrice(startPrice);
            var category = new Category { Name = "Legume" };
            var categoryResult = this.categoryService.AddCategory(category);
            var product = new Product { Name = "Fasole", Category = new[] { category } };
            var productResult = this.productService.AddProduct(product);
            var startDate = DateTime.Now;
            var auction = new Auction
            {
                Auctioneer = auctionUser,
                Product = product,
                StartPrice = startPrice,
                StartDate = startDate,
                EndDate = DateTime.Now.AddDays(5),
                Ended = false,
            };
            var auctionResult = this.auctionService.AddAuction(auction);
            var auctionUserById = this.auctionService.GetAuctionById(auction.Id);
            auctionUserById.StartDate = DateTime.Now.AddDays(7);
            this.auctionService.UpdateAuction(auctionUserById);
            TestsUtil.UndoingChangesDbEntityLevel(this.libraryContext, auction);
            auctionUserById = this.auctionService.GetAuctionById(auction.Id);
            Assert.IsTrue(DateTime.Compare(auctionUserById.StartDate, startDate) == 0);
        }

        /// <summary>
        /// Test Update auction end date.
        /// </summary>
        [Test]
        public void TestUpdateAuctionEbdDate()
        {
            this.ClearDatabase();
            var auctionUser = new AuctionUser { FirstName = "Ionel", LastName = "Pascu", Gender = "M" };
            var result2 = this.auctionUserService.AddAuctionUser(auctionUser, Role.Seller);
            var startPrice = new Price { Currency = "Euro", Value = 88.5 };
            var priceResult = this.priceService.AddPrice(startPrice);
            var category = new Category { Name = "Legume" };
            var categoryResult = this.categoryService.AddCategory(category);
            var product = new Product { Name = "Fasole", Category = new[] { category } };
            var productResult = this.productService.AddProduct(product);
            var startDate = DateTime.Now;
            var endDate = DateTime.Now.AddDays(5);
            var auction = new Auction
            {
                Auctioneer = auctionUser,
                Product = product,
                StartPrice = startPrice,
                StartDate = startDate,
                EndDate = endDate,
                Ended = false,
            };
            var auctionResult = this.auctionService.AddAuction(auction);
            var auctionUserById = this.auctionService.GetAuctionById(auction.Id);
            auctionUserById.EndDate = DateTime.Now.AddDays(2);
            this.auctionService.UpdateAuction(auctionUserById);
            auctionUserById = this.auctionService.GetAuctionById(auction.Id);
            Assert.IsFalse(DateTime.Compare(auctionUserById.EndDate, endDate) == 0);
        }

        /// <summary>
        /// Test Update auction startPrice.
        /// </summary>
        [Test]
        public void TestUpdateAuctionStartPrice()
        {
            this.ClearDatabase();
            var auctionUser = new AuctionUser { FirstName = "Ionel", LastName = "Pascu", Gender = "M" };
            var result2 = this.auctionUserService.AddAuctionUser(auctionUser, Role.Seller);
            var startPrice = new Price { Currency = "Euro", Value = 88.5 };
            var priceResult = this.priceService.AddPrice(startPrice);
            var category = new Category { Name = "Legume" };
            var categoryResult = this.categoryService.AddCategory(category);
            var product = new Product { Name = "Fasole", Category = new[] { category } };
            var productResult = this.productService.AddProduct(product);
            var auction = new Auction
            {
                Auctioneer = auctionUser,
                Product = product,
                StartPrice = startPrice,
                StartDate = DateTime.Now,
                EndDate = DateTime.Now.AddDays(5),
                Ended = false,
            };
            var auctionResult = this.auctionService.AddAuction(auction);
            var auctionUserById = this.auctionService.GetAuctionById(auction.Id);
            var startPrice2 = new Price { Id = 5, Currency = "Euro", Value = 444.5 };
            var priceResult2 = this.priceService.AddPrice(startPrice2);
            auctionUserById.StartPrice = startPrice2;
            this.auctionService.UpdateAuction(auctionUserById);
            auctionUserById = this.auctionService.GetAuctionById(auction.Id);
            Assert.IsTrue(auctionUserById.StartPrice.Id == startPrice2.Id);
        }

        /// <summary>
        /// Test Update auction product.
        /// </summary>
        [Test]
        public void TestUpdateAuctionProduct()
        {
            this.ClearDatabase();
            var auctionUser = new AuctionUser { FirstName = "Ionel", LastName = "Pascu", Gender = "M" };
            var result2 = this.auctionUserService.AddAuctionUser(auctionUser, Role.Seller);
            var startPrice = new Price { Currency = "Euro", Value = 88.5 };
            var priceResult = this.priceService.AddPrice(startPrice);
            var category = new Category { Name = "Legume" };
            var categoryResult = this.categoryService.AddCategory(category);
            var product = new Product { Name = "Fasole", Category = new[] { category } };
            var productResult = this.productService.AddProduct(product);
            var auction = new Auction
            {
                Auctioneer = auctionUser,
                Product = product,
                StartPrice = startPrice,
                StartDate = DateTime.Now,
                EndDate = DateTime.Now.AddDays(5),
                Ended = false,
            };
            var auctionResult = this.auctionService.AddAuction(auction);
            var auctionUserById = this.auctionService.GetAuctionById(auction.Id);
            var product2 = new Product { Id = 3, Name = "Macaroane", Category = new[] { category } };
            var productResult2 = this.productService.AddProduct(product2);
            auctionUserById.Product = product2;
            this.auctionService.UpdateAuction(auctionUserById);
            auctionUserById = this.auctionService.GetAuctionById(auction.Id);
            Assert.IsTrue(auctionUserById.Product.Id == product2.Id);
        }
        
        /// <summary>
        /// Test Update auction and auction is deleted.
        /// </summary>
        [Test]
        public void TestUpdateAuctionProductAndAuctionIsDeleted()
        {
            this.ClearDatabase();
            var auctionUser = new AuctionUser { FirstName = "Ionel", LastName = "Pascu", Gender = "M" };
            var result2 = this.auctionUserService.AddAuctionUser(auctionUser, Role.Seller);
            var startPrice = new Price { Currency = "Euro", Value = 88.5 };
            var priceResult = this.priceService.AddPrice(startPrice);
            var category = new Category { Name = "Legume" };
            var categoryResult = this.categoryService.AddCategory(category);
            var product = new Product { Name = "Fasole", Category = new[] { category } };
            var productResult = this.productService.AddProduct(product);
            var auction = new Auction
            {
                Auctioneer = auctionUser,
                Product = product,
                StartPrice = startPrice,
                StartDate = DateTime.Now,
                EndDate = DateTime.Now.AddDays(5),
                Ended = false,
            };
            var auctionResult = this.auctionService.AddAuction(auction);
            var auctionUserById = this.auctionService.GetAuctionById(auction.Id);
            var product2 = new Product { Id = 3, Name = "Macaroane", Category = new[] { category } };
            var productResult2 = this.productService.AddProduct(product2);
            var deleteResult = this.auctionService.DeleteAuction(auction.Id);
            auctionUserById.Product = product2;
            var updateResult = this.auctionService.UpdateAuction(auctionUserById);
            Assert.IsFalse(updateResult);
        }

        /// <summary>
        /// Test Update auction bad product.
        /// </summary>
        [Test]
        public void TestUpdateAuctionBadProduct()
        {
            this.ClearDatabase();
            var auctionUser = new AuctionUser { FirstName = "Ionel", LastName = "Pascu", Gender = "M" };
            var result2 = this.auctionUserService.AddAuctionUser(auctionUser, Role.Seller);
            var startPrice = new Price { Currency = "Euro", Value = 88.5 };
            var priceResult = this.priceService.AddPrice(startPrice);
            var category = new Category { Name = "Legume" };
            var categoryResult = this.categoryService.AddCategory(category);
            var product = new Product { Name = "Fasole", Category = new[] { category } };
            var productResult = this.productService.AddProduct(product);
            var auction = new Auction
            {
                Auctioneer = auctionUser,
                Product = product,
                StartPrice = startPrice,
                StartDate = DateTime.Now,
                EndDate = DateTime.Now.AddDays(5),
                Ended = false,
            };
            var auctionResult = this.auctionService.AddAuction(auction);
            var auctionUserById = this.auctionService.GetAuctionById(auction.Id);
            var product2 = new Product { Id = 3, Name = string.Empty, Category = new[] { category } };
            var productResult2 = this.productService.AddProduct(product2);
            auctionUserById.Product = product2;
            this.auctionService.UpdateAuction(auctionUserById);
            auctionUserById = this.auctionService.GetAuctionById(auction.Id);
            Assert.IsFalse(auctionUserById.Auctioneer.Id == product2.Id);
        }
        
        /// <summary>
        /// Test Update auction null product.
        /// </summary>
        [Test]
        public void TestUpdateAuctionNullProduct()
        {
            this.ClearDatabase();
            var auctionUser = new AuctionUser { FirstName = "Ionel", LastName = "Pascu", Gender = "M" };
            var result2 = this.auctionUserService.AddAuctionUser(auctionUser, Role.Seller);
            var startPrice = new Price { Currency = "Euro", Value = 88.5 };
            var priceResult = this.priceService.AddPrice(startPrice);
            var category = new Category { Name = "Legume" };
            var categoryResult = this.categoryService.AddCategory(category);
            var product = new Product { Name = "Fasole", Category = new[] { category } };
            var productResult = this.productService.AddProduct(product);
            var auction = new Auction
            {
                Auctioneer = auctionUser,
                Product = product,
                StartPrice = startPrice,
                StartDate = DateTime.Now,
                EndDate = DateTime.Now.AddDays(5),
                Ended = false,
            };
            var auctionResult = this.auctionService.AddAuction(auction);
            var auctionUserById = this.auctionService.GetAuctionById(auction.Id);
            auctionUserById.Product = null;
            var updateResult = this.auctionService.UpdateAuction(auctionUserById);
            Assert.IsFalse(updateResult);
        }

        /// <summary>
        /// Test Update auction with bad user.
        /// </summary>
        [Test]
        public void TestUpdateAuctionBadUser()
        {
            this.ClearDatabase();
            var auctionUser = new AuctionUser { FirstName = "Ionel", LastName = "Pascu", Gender = "M" };
            var result2 = this.auctionUserService.AddAuctionUser(auctionUser, Role.Seller);
            var startPrice = new Price { Currency = "Euro", Value = 88.5 };
            var priceResult = this.priceService.AddPrice(startPrice);
            var category = new Category { Name = "Legume" };
            var categoryResult = this.categoryService.AddCategory(category);
            var product = new Product { Name = "Fasole", Category = new[] { category } };
            var productResult = this.productService.AddProduct(product);
            var auction = new Auction
            {
                Auctioneer = auctionUser,
                Product = product,
                StartPrice = startPrice,
                StartDate = DateTime.Now,
                EndDate = DateTime.Now.AddDays(5),
                Ended = false,
            };
            var auctionResult = this.auctionService.AddAuction(auction);
            var auctionUserById = this.auctionService.GetAuctionById(auction.Id);
            var auctionUser2 = new AuctionUser { FirstName = "Marcu", LastName = "Andrei", Gender = "M" };
            var resultUser2 = this.auctionUserService.AddAuctionUser(auctionUser2, Role.Buyer);
            auctionUserById.Auctioneer = auctionUser2;
            var updateResult = this.auctionService.UpdateAuction(auctionUserById);
            Assert.IsFalse(updateResult);
        }

        /// <summary>
        /// Test delete auction.
        /// </summary>
        [Test]
        public void TestDeleteAuction()
        {
            this.ClearDatabase();
            var auctionUser = new AuctionUser { FirstName = "Ionel", LastName = "Pascu", Gender = "M" };
            var result2 = this.auctionUserService.AddAuctionUser(auctionUser, Role.Seller);
            var startPrice = new Price { Currency = "Euro", Value = 88.5 };
            var priceResult = this.priceService.AddPrice(startPrice);
            var category = new Category { Name = "Legume" };
            var categoryResult = this.categoryService.AddCategory(category);
            var product = new Product { Name = "Fasole", Category = new[] { category } };
            var productResult = this.productService.AddProduct(product);
            var startDate = DateTime.Now;
            var endDate = DateTime.Now.AddDays(5);
            var auction = new Auction
            {
                Auctioneer = auctionUser,
                Product = product,
                StartPrice = startPrice,
                StartDate = startDate,
                EndDate = endDate,
                Ended = false,
            };
            var auctionResult = this.auctionService.AddAuction(auction);
            var deleteResult = this.auctionService.DeleteAuction(auction.Id);
            Assert.True(!this.libraryContext.Auctions.Any());
        }

        /// <summary>
        /// Test Update auction user.
        /// </summary>
        [Test]
        public void TestUpdateAuctionUser()
        {
            this.ClearDatabase();
            var auctionUser = new AuctionUser { FirstName = "Ionel", LastName = "Pascu", Gender = "M" };
            var result2 = this.auctionUserService.AddAuctionUser(auctionUser, Role.Seller);
            var startPrice = new Price { Currency = "Euro", Value = 88.5 };
            var priceResult = this.priceService.AddPrice(startPrice);
            var category = new Category { Name = "Legume" };
            var categoryResult = this.categoryService.AddCategory(category);
            var product = new Product { Name = "Fasole", Category = new[] { category } };
            var productResult = this.productService.AddProduct(product);
            var auction = new Auction
            {
                Auctioneer = auctionUser,
                Product = product,
                StartPrice = startPrice,
                StartDate = DateTime.Now,
                EndDate = DateTime.Now.AddDays(5),
                Ended = false,
            };
            var auctionResult = this.auctionService.AddAuction(auction);
            var auctionUserById = this.auctionService.GetAuctionById(auction.Id);
            var auctionUser2 = new AuctionUser { Id = 2, FirstName = "Marcu", LastName = "Andrei", Gender = "M" };
            var resultUser2 = this.auctionUserService.AddAuctionUser(auctionUser2, Role.Seller);
            auctionUserById.Auctioneer = auctionUser2;
            this.auctionService.UpdateAuction(auctionUserById);
            auctionUserById = this.auctionService.GetAuctionById(auction.Id);
            Assert.IsTrue(auctionUserById.Auctioneer.Id == auctionUser2.Id);
        }

        /// <summary>
        /// Test Update auction bad start date.
        /// </summary>
        [Test]
        public void TestUpdateAuctionBadEndDate()
        {
            var auctionUser = new AuctionUser { FirstName = "Ionel", LastName = "Pascu", Gender = "M" };
            var result2 = this.auctionUserService.AddAuctionUser(auctionUser, Role.Seller);
            var startPrice = new Price { Currency = "Euro", Value = 88.5 };
            var priceResult = this.priceService.AddPrice(startPrice);
            var category = new Category { Name = "Legume" };
            var categoryResult = this.categoryService.AddCategory(category);
            var product = new Product { Name = "Fasole", Category = new[] { category } };
            var productResult = this.productService.AddProduct(product);
            var startDate = DateTime.Now;
            var maxM = int.Parse(ConfigurationManager.AppSettings["MAX_NUMBER_OF_MONTHS"]);
            var endDate = DateTime.Now.AddMonths(2);
            var auction = new Auction
            {
                Auctioneer = auctionUser,
                Product = product,
                StartPrice = startPrice,
                StartDate = startDate,
                EndDate = endDate,
                Ended = false,
            };
            var auctionResult = this.auctionService.AddAuction(auction);
            var auctionUserById = this.auctionService.GetAuctionById(auction.Id);
            auctionUserById.EndDate = endDate.AddMonths(maxM + 1);
            this.auctionService.UpdateAuction(auctionUserById);
            TestsUtil.UndoingChangesDbEntityLevel(this.libraryContext, auction);
            auctionUserById = this.auctionService.GetAuctionById(auction.Id);
            Assert.IsTrue(DateTime.Compare(auctionUserById.EndDate, endDate) == 0);
        }

        /// <summary>
        /// Test add an auction.
        /// </summary>
        [Test]
        public void TestAddAuction()
        {
            this.ClearDatabase();
            var auctionUser = new AuctionUser { FirstName = "Ionel", LastName = "Pascu", Gender = "M" };
            var result = this.auctionUserService.AddAuctionUser(auctionUser, Role.Seller);
            var startPrice = new Price { Currency = "Euro", Value = 88.5 };
            var priceResult = this.priceService.AddPrice(startPrice);
            var category = new Category { Name = "Legume" };
            _ = this.categoryService.AddCategory(category);
            var product = new Product { Name = "Fasole", Category = new[] { category } };
            var productResult = this.productService.AddProduct(product);
            var auction = new Auction
            {
                Auctioneer = auctionUser,
                Product = product,
                StartPrice = startPrice,
                StartDate = DateTime.Now,
                EndDate = DateTime.Now.AddDays(5),
                Ended = false,
            };
            var auctionResult = this.auctionService.AddAuction(auction);
            Assert.True(this.libraryContext.Auctions.Count() == 1);
        }

        /// <summary>
        /// Test add auction with wrong end date.
        /// </summary>
        [Test]
        public void TestAddMoreThanMaxNumberOfAuctionInSameCategory()
        {
            this.ClearDatabase();
            var auctionUser = new AuctionUser { FirstName = "Ionel", LastName = "Pascu", Gender = "M" };
            var result = this.auctionUserService.AddAuctionUser(auctionUser, Role.Seller);
            var startPrice = new Price { Currency = "Euro", Value = 88.5 };
            var priceResult = this.priceService.AddPrice(startPrice);
            var category = new Category { Name = "Legume" };
            var categoryResult = this.categoryService.AddCategory(category);
            var product = new Product { Name = "Fasole", Category = new[] { category } };
            var productResult = this.productService.AddProduct(product);
            var maxNumberOfMonths = int.Parse(ConfigurationManager.AppSettings["MAX_NUMBER_OF_MONTHS"]);
            var startDate = DateTime.Now;
            var maxNumberOfAuctions = int.Parse(ConfigurationManager.AppSettings["MAX_NUMBER_OF_AUCTION_IN_CATEGORY"]);
            for (var i = 0; i <= maxNumberOfAuctions + 1; i++)
            {
                var auction = new Auction
                {
                    Auctioneer = auctionUser,
                    Product = product,
                    StartPrice = startPrice,
                    StartDate = startDate.AddDays(i),
                    EndDate = startDate.AddDays(4 + i),
                    Ended = false,
                };
                this.auctionService.AddAuction(auction);
            }

            Assert.True(this.libraryContext.Auctions.Count() == maxNumberOfAuctions);
        }

        /// <summary>
        /// Test add auction with wrong end date.
        /// </summary>
        [Test]
        public void TestAddMoreThanMaxNumberOfAuctionInSameRoot()
        {
            this.ClearDatabase();
            var auctionUser = new AuctionUser { FirstName = "Ionel", LastName = "Pascu", Gender = "M" };
            var result = this.auctionUserService.AddAuctionUser(auctionUser, Role.Seller);
            var startPrice = new Price { Currency = "Euro", Value = 88.5 };
            var priceResult = this.priceService.AddPrice(startPrice);
            var rootCategory = new Category { Name = "Legume" };
            var rootCategoryResult = this.categoryService.AddCategory(rootCategory);
            var maxNumberOfMonths = int.Parse(ConfigurationManager.AppSettings["MAX_NUMBER_OF_MONTHS"]);
            var startDate = DateTime.Now;
            var maxNumberOfAuctions = int.Parse(ConfigurationManager.AppSettings["MAX_NUMBER_OF_AUCTION_IN_CATEGORY"]);
            for (var i = 0; i <= maxNumberOfAuctions + 1; i++)
            {
                var category = new Category { Name = "Legume" + i, ParentCategory = rootCategory };
                var categoryResult = this.categoryService.AddCategory(category);
                var product = new Product { Name = "Fasole" + i, Category = new[] { category } };
                var productResult = this.productService.AddProduct(product);
                var auction = new Auction
                {
                    Auctioneer = auctionUser,
                    Product = product,
                    StartPrice = startPrice,
                    StartDate = startDate,
                    EndDate = startDate.AddDays(6),
                    Ended = false,
                };
                this.auctionService.AddAuction(auction);
            }

            Assert.True(this.libraryContext.Auctions.Count() == maxNumberOfAuctions);
        }

        /// <summary>
        /// Test add auction with wrong end date.
        /// </summary>
        [Test]
        public void TestAddMoreThanMaxNumberOfAuctionInMoreCategories()
        {
            this.ClearDatabase();
            var auctionUser = new AuctionUser { FirstName = "Ionel", LastName = "Pascu", Gender = "M" };
            var result = this.auctionUserService.AddAuctionUser(auctionUser, Role.Seller);
            var startPrice = new Price { Currency = "Euro", Value = 88.5 };
            var priceResult = this.priceService.AddPrice(startPrice);
            var maxNumberOfMonths = int.Parse(ConfigurationManager.AppSettings["MAX_NUMBER_OF_MONTHS"]);
            var startDate = DateTime.Now;
            var maxNumberOfAuctions = int.Parse(ConfigurationManager.AppSettings["MAX_NUMBER_OF_AUCTION"]);
            for (var i = 0; i <= maxNumberOfAuctions + 1; i++)
            {
                var category = new Category { Name = "Legume" + i };
                var categoryResult = this.categoryService.AddCategory(category);
                var product = new Product { Name = "Fasole" + i, Category = new[] { category } };
                var productResult = this.productService.AddProduct(product);
                var auction = new Auction
                {
                    Auctioneer = auctionUser,
                    Product = product,
                    StartPrice = startPrice,
                    StartDate = startDate,
                    EndDate = startDate.AddDays(6),
                    Ended = false,
                };
                this.auctionService.AddAuction(auction);
            }

            Assert.True(this.libraryContext.Auctions.Count() == maxNumberOfAuctions);
        }

        /// <summary>
        /// Test Get Number of all started auction for user.
        /// </summary>
        [Test]
        public void TestGetStartedAuctionByUser()
        {
            this.ClearDatabase();
            var auctionUser = new AuctionUser { FirstName = "Ionel", LastName = "Pascu", Gender = "M" };
            var result = this.auctionUserService.AddAuctionUser(auctionUser, Role.Seller);
            var startPrice = new Price { Currency = "Euro", Value = 88.5 };
            var priceResult = this.priceService.AddPrice(startPrice);
            var maxNumberOfMonths = int.Parse(ConfigurationManager.AppSettings["MAX_NUMBER_OF_MONTHS"]);
            var startDate = DateTime.Now;
            for (var i = 1; i <= 4; i++)
            {
                var category = new Category { Name = "Legume" + i };
                var categoryResult = this.categoryService.AddCategory(category);
                var product = new Product { Name = "Fasole" + i, Category = new[] { category } };
                var productResult = this.productService.AddProduct(product);
                var auction = new Auction
                {
                    Auctioneer = auctionUser,
                    Product = product,
                    StartPrice = startPrice,
                    StartDate = startDate,
                    EndDate = startDate.AddDays(6),
                    Ended = false,
                };
                this.auctionService.AddAuction(auction);
            }

            var startedAuctions = this.auctionService.GetStartedAuctionsByUser(auctionUser);
            Assert.True(startedAuctions.Count() == 4);
        }

        /// <summary>
        /// Test Get All started auction for user in a specific category.
        /// </summary>
        [Test]
        public void TestGetStartedAuctionByUserAndCategory()
        {
            this.ClearDatabase();
            var auctionUser = new AuctionUser { FirstName = "Ionel", LastName = "Pascu", Gender = "M" };
            var result = this.auctionUserService.AddAuctionUser(auctionUser, Role.Seller);
            var startPrice = new Price { Currency = "Euro", Value = 88.5 };
            var priceResult = this.priceService.AddPrice(startPrice);
            var maxNumberOfMonths = int.Parse(ConfigurationManager.AppSettings["MAX_NUMBER_OF_MONTHS"]);
            var category = new Category { Name = "Legume" };
            var categoryResult = this.categoryService.AddCategory(category);
            var startDate = DateTime.Now;
            for (var i = 1; i <= 3; i++)
            {
                var product = new Product { Name = "Fasole" + i, Category = new[] { category } };
                var productResult = this.productService.AddProduct(product);
                var auction = new Auction
                {
                    Auctioneer = auctionUser,
                    Product = product,
                    StartPrice = startPrice,
                    StartDate = startDate,
                    EndDate = startDate.AddDays(6),
                    Ended = false,
                };
                this.auctionService.AddAuction(auction);
            }

            var startedAuctions = this.auctionService.GetStartedAuctionsByUserAndCategory(auctionUser, category);
            Assert.True(startedAuctions.Count() == 3);
        }

        /// <summary>
        /// Test Get all started auctions.
        /// </summary>
        [Test]
        public void TestGetAllAuctions()
        {
            this.ClearDatabase();
            var auctionUser = new AuctionUser { FirstName = "Ionel", LastName = "Pascu", Gender = "M" };
            var result = this.auctionUserService.AddAuctionUser(auctionUser, Role.Seller);
            var startPrice = new Price { Currency = "Euro", Value = 88.5 };
            var priceResult = this.priceService.AddPrice(startPrice);
            var maxNumberOfMonths = int.Parse(ConfigurationManager.AppSettings["MAX_NUMBER_OF_MONTHS"]);
            var startDate = DateTime.Now;
            for (var i = 1; i <= 4; i++)
            {
                var category = new Category { Name = "Legume" + i };
                var categoryResult = this.categoryService.AddCategory(category);
                var product = new Product { Name = "Fasole" + i, Category = new[] { category } };
                var productResult = this.productService.AddProduct(product);
                var auction = new Auction
                {
                    Auctioneer = auctionUser,
                    Product = product,
                    StartPrice = startPrice,
                    StartDate = startDate,
                    EndDate = startDate.AddDays(6),
                    Ended = false,
                };
                this.auctionService.AddAuction(auction);
            }

            var auctions = this.auctionService.GetAuctions();
            Assert.True(auctions.Count() == 4);
        }

        /// <summary>
        /// Test get auction by id and he is in the database.
        /// </summary>
        [Test]
        public void TestGetAuctionByGoodId()
        {
            this.ClearDatabase();
            var auctionUser = new AuctionUser { FirstName = "Ionel", LastName = "Pascu", Gender = "M" };
            var result = this.auctionUserService.AddAuctionUser(auctionUser, Role.Seller);
            var startPrice = new Price { Currency = "Euro", Value = 88.5 };
            var priceResult = this.priceService.AddPrice(startPrice);
            var category = new Category { Name = "Legume" };
            var categoryResult = this.categoryService.AddCategory(category);
            var product = new Product { Name = "Fasole", Category = new[] { category } };
            var productResult = this.productService.AddProduct(product);
            var auction = new Auction
            {
                Auctioneer = auctionUser,
                Product = product,
                StartPrice = startPrice,
                StartDate = DateTime.Now,
                EndDate = DateTime.Now.AddDays(5),
                Ended = false,
            };
            var auctionResult = this.auctionService.AddAuction(auction);
            var auctionUserById = this.auctionService.GetAuctionById(auction.Id);
            Assert.NotNull(auctionUserById);
        }

        /// <summary>
        /// Test get auctionUser by id and he is not in the database.
        /// </summary>
        [Test]
        public void TestGetAuctionByBadId()
        {
            this.ClearDatabase();
            var auctionUser = new AuctionUser { FirstName = "Ionel", LastName = "Pascu", Gender = "M" };
            var result = this.auctionUserService.AddAuctionUser(auctionUser, Role.Seller);
            var startPrice = new Price { Currency = "Euro", Value = 88.5 };
            var priceResult = this.priceService.AddPrice(startPrice);
            var category = new Category { Name = "Legume" };
            var categoryResult = this.categoryService.AddCategory(category);
            var product = new Product { Name = "Fasole", Category = new[] { category } };
            var productResult = this.productService.AddProduct(product);
            var auction = new Auction
            {
                Auctioneer = auctionUser,
                Product = product,
                StartPrice = startPrice,
                StartDate = DateTime.Now,
                EndDate = DateTime.Now.AddDays(5),
                Ended = false,
            };
            var auctionResult = this.auctionService.AddAuction(auction);
            var auctionUserById = this.auctionService.GetAuctionById(auction.Id + 1);
            Assert.Null(auctionUserById);
        }

        /// <summary>
        /// Test delete auction if it is not in the database.
        /// </summary>
        [Test]
        public void TestDeleteAuctionWithAWrongId()
        {
            var auctionUser = new AuctionUser { FirstName = "Ionel", LastName = "Pascu", Gender = "M" };
            var result2 = this.auctionUserService.AddAuctionUser(auctionUser, Role.Seller);
            var startPrice = new Price { Currency = "Euro", Value = 88.5 };
            var priceResult = this.priceService.AddPrice(startPrice);
            var category = new Category { Name = "Legume" };
            var categoryResult = this.categoryService.AddCategory(category);
            var product = new Product { Name = "Fasole", Category = new[] { category } };
            var productResult = this.productService.AddProduct(product);
            var startDate = DateTime.Now;
            var endDate = DateTime.Now.AddDays(5);
            var auction = new Auction
            {
                Auctioneer = auctionUser,
                Product = product,
                StartPrice = startPrice,
                StartDate = startDate,
                EndDate = endDate,
                Ended = false,
            };
            var auctionResult = this.auctionService.AddAuction(auction);
            var deleteResult = this.auctionService.DeleteAuction(auction.Id + 1);
            Assert.True(this.libraryContext.Auctions.Count() == 1);
        }

        /// <summary>
        /// Test check if auction is not ended.
        /// </summary>
        [Test]
        public void TestCheckIfAuctionIsNotEnded()
        {
            var auctionUser = new AuctionUser { FirstName = "Ionel", LastName = "Pascu", Gender = "M" };
            var result2 = this.auctionUserService.AddAuctionUser(auctionUser, Role.Seller);
            var startPrice = new Price { Currency = "Euro", Value = 88.5 };
            var priceResult = this.priceService.AddPrice(startPrice);
            var category = new Category { Name = "Legume" };
            var categoryResult = this.categoryService.AddCategory(category);
            var product = new Product { Name = "Fasole", Category = new[] { category } };
            var productResult = this.productService.AddProduct(product);
            var startDate = DateTime.Now;
            var endDate = DateTime.Now.AddDays(5);
            var auction = new Auction
            {
                Auctioneer = auctionUser,
                Product = product,
                StartPrice = startPrice,
                StartDate = startDate,
                EndDate = endDate,
                Ended = false,
            };
            var auctionResult = this.auctionService.AddAuction(auction);
            var isEnded = this.auctionService.CheckIfAuctionIsEnded(auction);
            Assert.IsFalse(isEnded);
        }

        /// <summary>
        /// Test Get last auction for an user.
        /// </summary>
        [Test]
        public void TestGetLastAuctionForUser()
        {
            this.ClearDatabase();
            var auctionUser = new AuctionUser { FirstName = "Ionel", LastName = "Pascu", Gender = "M" };
            var result2 = this.auctionUserService.AddAuctionUser(auctionUser, Role.Seller);
            var startPrice = new Price { Currency = "Euro", Value = 88.5 };
            var priceResult = this.priceService.AddPrice(startPrice);
            var category = new Category { Name = "Legume" };
            var categoryResult = this.categoryService.AddCategory(category);
            var product = new Product { Name = "Fasole", Category = new[] { category } };
            var productResult = this.productService.AddProduct(product);
            var startDate = DateTime.Now;
            var endDate = DateTime.Now.AddDays(3);
            var auction = new Auction
            {
                Auctioneer = auctionUser,
                Product = product,
                StartPrice = startPrice,
                StartDate = startDate,
                EndDate = endDate,
                Ended = false,
            };
            var auctionResult = this.auctionService.AddAuction(auction);

            var startPrice2 = new Price { Currency = "Euro", Value = 848.5 };
            var priceResult2 = this.priceService.AddPrice(startPrice2);
            var category2 = new Category { Name = "Mere" };
            var categoryResult2 = this.categoryService.AddCategory(category2);
            var product2 = new Product { Name = "Caise", Category = new[] { category2 } };
            var productResult2 = this.productService.AddProduct(product2);
            var startDate2 = DateTime.Now;
            var endDate2 = DateTime.Now.AddDays(4);
            var auction2 = new Auction
            {
                Auctioneer = auctionUser,
                Product = product2,
                StartPrice = startPrice2,
                StartDate = startDate2,
                EndDate = endDate2,
                Ended = false,
            };
            var auctionResult2 = this.auctionService.AddAuction(auction2);
            var lastAuction = this.auctionService.GetLastAuctionForUser(auctionUser);
            Assert.IsTrue(lastAuction.Id == auction2.Id);
        }

        /// <summary>
        /// Test Get Auction last price without bids.
        /// </summary>
        [Test]
        public void TestGetAuctionLastPriceNoBids()
        {
            this.ClearDatabase();
            var auctionUser = new AuctionUser { FirstName = "Ionel", LastName = "Pascu", Gender = "M" };
            var result2 = this.auctionUserService.AddAuctionUser(auctionUser, Role.Seller);
            var startPrice = new Price { Currency = "Euro", Value = 88.5 };
            var priceResult = this.priceService.AddPrice(startPrice);
            var category = new Category { Name = "Legume" };
            var categoryResult = this.categoryService.AddCategory(category);
            var product = new Product { Name = "Fasole", Category = new[] { category } };
            var productResult = this.productService.AddProduct(product);
            var auction = new Auction
            {
                Auctioneer = auctionUser,
                Product = product,
                StartPrice = startPrice,
                StartDate = DateTime.Now,
                EndDate = DateTime.Now.AddDays(5),
                Ended = false,
            };
            var auctionResult = this.auctionService.AddAuction(auction);
            var auctionUserById = this.auctionService.GetAuctionById(auction.Id);
            var lastPrice = this.auctionService.GetAuctionLastPrice(auctionUserById);
            Assert.IsTrue(lastPrice.Id == startPrice.Id);
        }

        /// <summary>
        /// Test add auction with a user how has more than min score (from reviews).
        /// </summary>
        [Test]
        public void TestAddAuctionWithAnUserHowHasMoreThanMinScore()
        {
            this.ClearDatabase();
            var auctionUser = new AuctionUser { FirstName = "Ionel", LastName = "Pascu", Gender = "M" };
            var result = this.auctionUserService.AddAuctionUser(auctionUser, Role.Seller);
            var startPrice = new Price { Currency = "Euro", Value = 88.5 };
            var priceResult = this.priceService.AddPrice(startPrice);
            var category = new Category { Name = "Legume" };
            var categoryResult = this.categoryService.AddCategory(category);
            var product = new Product { Name = "Fasole", Category = new[] { category } };
            var productResult = this.productService.AddProduct(product);
            var maxNumberOfMonths = int.Parse(ConfigurationManager.AppSettings["MAX_NUMBER_OF_MONTHS"]);
            var startDate = DateTime.Now;
            var auction = new Auction
            {
                Auctioneer = auctionUser,
                Product = product,
                StartPrice = startPrice,
                StartDate = startDate,
                EndDate = startDate.AddDays(1),
                Ended = false,
            };
            var auctionResult = this.auctionService.AddAuction(auction);
            var reviewUser = new AuctionUser { FirstName = "Anghel", LastName = "Pascu", Gender = "M" };
            var resultReviewUser = this.auctionUserService.AddAuctionUser(reviewUser, Role.Buyer);
            var userReview = new UserReview
            { ReviewByUser = reviewUser, ReviewForUser = auctionUser, Score = 7, Description = "This is a test." };
            var reviewResult = this.userReviewService.AddUserReview(userReview);
            var auction2 = new Auction
            {
                Auctioneer = auctionUser,
                Product = product,
                StartPrice = startPrice,
                StartDate = startDate,
                EndDate = startDate.AddDays(1),
                Ended = false,
            };
            var auction2Result = this.auctionService.AddAuction(auction);
            Assert.True(this.libraryContext.Auctions.Count() == 2);
        }
        
        /// <summary>
        /// Test add auction with a user how has less than min score and no auctions.
        /// </summary>
        [Test]
        public void TestAddAuctionWithAnBlockedUserNoAuctions()
        {
            this.ClearDatabase();
            var auctionUser = new AuctionUser { FirstName = "Ionel", LastName = "Pascu", Gender = "M" };
            var result = this.auctionUserService.AddAuctionUser(auctionUser, Role.Seller);
            var reviewUser = new AuctionUser { FirstName = "Anghel", LastName = "Pascu", Gender = "M" };
            var resultReviewUser = this.auctionUserService.AddAuctionUser(reviewUser, Role.Buyer);
            var userReview = new UserReview
                { ReviewByUser = reviewUser, ReviewForUser = auctionUser, Score = 2, Description = "This is a test." };
            var reviewResult = this.userReviewService.AddUserReview(userReview);
            var startPrice = new Price { Currency = "Euro", Value = 88.5 };
            var priceResult = this.priceService.AddPrice(startPrice);
            var category = new Category { Name = "Legume" };
            var categoryResult = this.categoryService.AddCategory(category);
            var product = new Product { Name = "Fasole", Category = new[] { category } };
            var productResult = this.productService.AddProduct(product);
            var maxNumberOfMonths = int.Parse(ConfigurationManager.AppSettings["MAX_NUMBER_OF_MONTHS"]);
            var startDate = DateTime.Now;
            var auction = new Auction
            {
                Auctioneer = auctionUser,
                Product = product,
                StartPrice = startPrice,
                StartDate = startDate,
                EndDate = startDate.AddDays(1),
                Ended = false,
            };
            var auctionResult = this.auctionService.AddAuction(auction);
            Assert.True(this.libraryContext.Auctions.Count() == 1);
        }
        
        /// <summary>
        /// Test add auction with a user how has less than min score.
        /// </summary>
        [Test]
        public void TestAddAuctionWithAnBlockedUser()
        {
            this.ClearDatabase();
            var auctionUser = new AuctionUser { FirstName = "Ionel", LastName = "Pascu", Gender = "M" };
            var result = this.auctionUserService.AddAuctionUser(auctionUser, Role.Seller);
            var startPrice = new Price { Currency = "Euro", Value = 88.5 };
            var priceResult = this.priceService.AddPrice(startPrice);
            var category = new Category { Name = "Legume" };
            var categoryResult = this.categoryService.AddCategory(category);
            var product = new Product { Name = "Fasole", Category = new[] { category } };
            var productResult = this.productService.AddProduct(product);
            var maxNumberOfMonths = int.Parse(ConfigurationManager.AppSettings["MAX_NUMBER_OF_MONTHS"]);
            var startDate = DateTime.Now;
            var auction = new Auction
            {
                Auctioneer = auctionUser,
                Product = product,
                StartPrice = startPrice,
                StartDate = startDate,
                EndDate = startDate.AddDays(1),
                Ended = false,
            };
            var auctionResult = this.auctionService.AddAuction(auction);
            var reviewUser = new AuctionUser { FirstName = "Anghel", LastName = "Pascu", Gender = "M" };
            var resultReviewUser = this.auctionUserService.AddAuctionUser(reviewUser, Role.Buyer);
            var userReview = new UserReview
            { ReviewByUser = reviewUser, ReviewForUser = auctionUser, Score = 2, Description = "This is a test." };
            var reviewResult = this.userReviewService.AddUserReview(userReview);
            var auction2 = new Auction
            {
                Auctioneer = auctionUser,
                Product = product,
                StartPrice = startPrice,
                StartDate = startDate,
                EndDate = startDate.AddDays(1),
                Ended = false,
            };
            var auction2Result = this.auctionService.AddAuction(auction);
            Assert.IsFalse(auction2Result);
            Assert.True(this.libraryContext.Auctions.Count() == 1);
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