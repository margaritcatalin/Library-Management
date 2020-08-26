// <copyright file="AuctionTests.cs" company="Transilvania University of Brasov">
// Margarit Marian Catalin
// </copyright>

namespace LibraryManagementTests
{
    using System;
    using System.Configuration;
    using System.Linq;
    using LibraryManagement.BusinessLayer;
    using LibraryManagement.DataMapper;
    using LibraryManagement.DomainModel;
    using LibraryManagement.Util;
    using NUnit.Framework;
    using Telerik.JustMock.EntityFramework;

    /// <summary>
    /// The auction unit tests.
    /// </summary>
    [TestFixture]
    public class AuctionTests
    {
        /// <summary>
        /// Defines the libraryContextMock.
        /// </summary>
        private LibraryDbContext libraryContextMock;

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
        /// Tests setup.
        /// </summary>
        [SetUp]
        public void SetUp()
        {
            this.libraryContextMock = EntityFrameworkMock.Create<LibraryDbContext>().PrepareMock();
            this.productService = new ProductService(new ProductRepository(this.libraryContextMock));
            this.priceService = new PriceService(new PriceRepository(this.libraryContextMock));
            this.userReviewService = new UserReviewService(new UserReviewRepository(this.libraryContextMock));
            this.auctionUserService = new AuctionUserService(new AuctionUserRepository(this.libraryContextMock), this.userReviewService);
            this.categoryService = new CategoryService(new CategoryRepository(this.libraryContextMock));
            this.auctionService = new AuctionService(new AuctionRepository(this.libraryContextMock), this.categoryService, this.auctionUserService);
            this.bidService = new BidService(new BidRepository(this.libraryContextMock), this.auctionService);
        }

        /// <summary>
        /// Test add an null auction.
        /// </summary>
        [Test]
        public void TestAddNullAuction()
        {
            var auctionResult = this.auctionService.AddAuction(null);
            Assert.True(!this.libraryContextMock.Auctions.Any());
        }

        /// <summary>
        /// Test add an null user.
        /// </summary>
        [Test]
        public void TestAddWithNullUserAuction()
        {
            var startPrice = new Price { Currency = "Euro", Value = 88.5 };
            var priceResult = this.priceService.AddPrice(startPrice);
            var category = new Category { Name = "Legume" };
            var categoryResult = this.categoryService.AddCategory(category);
            var product = new Product { Name = "Fasole", Category = new[] { category } };
            var productResult = this.productService.AddProduct(product);
            var auction = new Auction
            {
                Auctioneer = null,
                Product = product,
                StartPrice = startPrice,
                StartDate = DateTime.Now,
                EndDate = DateTime.Now.AddDays(5),
                Ended = false,
            };
            var auctionResult = this.auctionService.AddAuction(auction);
            Assert.True(!this.libraryContextMock.Auctions.Any());
        }

        /// <summary>
        /// Test add an auction with wrong user.
        /// </summary>
        [Test]
        public void TestAddWithWrongUserRoleAuction()
        {
            var auctionUser = new AuctionUser { FirstName = "Ionel", LastName = "Pascu", Gender = "M" };
            var result = this.auctionUserService.AddAuctionUser(auctionUser, Role.Buyer);
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
            Assert.True(!this.libraryContextMock.Auctions.Any());
        }

        /// <summary>
        /// Test add an auction with wrong product.
        /// </summary>
        [Test]
        public void TestAddWithNullProductAuction()
        {
            var auctionUser = new AuctionUser { FirstName = "Ionel", LastName = "Pascu", Gender = "M" };
            var result = this.auctionUserService.AddAuctionUser(auctionUser, Role.Buyer);
            var startPrice = new Price { Currency = "Euro", Value = 88.5 };
            var priceResult = this.priceService.AddPrice(startPrice);
            var category = new Category { Name = "Legume" };
            var categoryResult = this.categoryService.AddCategory(category);
            var auction = new Auction
            {
                Auctioneer = auctionUser,
                Product = null,
                StartPrice = startPrice,
                StartDate = DateTime.Now,
                EndDate = DateTime.Now.AddDays(5),
                Ended = false,
            };
            var auctionResult = this.auctionService.AddAuction(auction);
            Assert.True(!this.libraryContextMock.Auctions.Any());
        }

        /// <summary>
        /// Test add an auction with wrong price.
        /// </summary>
        [Test]
        public void TestAddWithStartPriceLessThanMinPriceAuction()
        {
            var auctionUser = new AuctionUser { FirstName = "Ionel", LastName = "Pascu", Gender = "M" };
            var result = this.auctionUserService.AddAuctionUser(auctionUser, Role.Buyer);
            var category = new Category { Name = "Legume" };
            var minPrice = double.Parse(ConfigurationManager.AppSettings["MIN_PRICE"]);
            var startPrice = new Price { Currency = "Euro", Value = minPrice - 1 };
            var priceResult = this.priceService.AddPrice(startPrice);
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
            Assert.True(!this.libraryContextMock.Auctions.Any());
        }

        /// <summary>
        /// Test add an auction with wrong price.
        /// </summary>
        [Test]
        public void TestAddWithNullStartPriceAuction()
        {
            var auctionUser = new AuctionUser { FirstName = "Ionel", LastName = "Pascu", Gender = "M" };
            var result = this.auctionUserService.AddAuctionUser(auctionUser, Role.Buyer);
            var category = new Category { Name = "Legume" };
            var categoryResult = this.categoryService.AddCategory(category);
            var product = new Product { Name = "Fasole", Category = new[] { category } };
            var productResult = this.productService.AddProduct(product);
            var auction = new Auction
            {
                Auctioneer = auctionUser,
                Product = product,
                StartPrice = null,
                StartDate = DateTime.Now,
                EndDate = DateTime.Now.AddDays(5),
                Ended = false,
            };
            var auctionResult = this.auctionService.AddAuction(auction);
            Assert.True(!this.libraryContextMock.Auctions.Any());
        }

        /// <summary>
        /// Test add an wrong auction.
        /// </summary>
        [Test]
        public void TestAddWithWrongStartDateAuction()
        {
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
                StartDate = DateTime.Now.AddDays(-1),
                EndDate = DateTime.Now.AddDays(5),
                Ended = false,
            };
            var auctionResult = this.auctionService.AddAuction(auction);
            Assert.True(!this.libraryContextMock.Auctions.Any());
        }

        /// <summary>
        /// Test add auction  with  wrong end date.
        /// </summary>
        [Test]
        public void TestAddWithStartDateSameWithEndDateAuction()
        {
            var auctionUser = new AuctionUser { FirstName = "Ionel", LastName = "Pascu", Gender = "M" };
            var result = this.auctionUserService.AddAuctionUser(auctionUser, Role.Seller);
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
                EndDate = startDate,
                Ended = false,
            };
            var auctionResult = this.auctionService.AddAuction(auction);
            Assert.True(!this.libraryContextMock.Auctions.Any());
        }

        /// <summary>
        /// Test add auction with a wrong date.
        /// </summary>
        [Test]
        public void TestAddWithStartDateMoreThanEndDateAuction()
        {
            var auctionUser = new AuctionUser { FirstName = "Ionel", LastName = "Pascu", Gender = "M" };
            var result = this.auctionUserService.AddAuctionUser(auctionUser, Role.Seller);
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
                StartDate = startDate.AddDays(5),
                EndDate = startDate,
                Ended = false,
            };
            var auctionResult = this.auctionService.AddAuction(auction);
            Assert.True(!this.libraryContextMock.Auctions.Any());
        }

        /// <summary>
        /// Test add auction  with  wrong end date.
        /// </summary>
        [Test]
        public void TestAddWithEndDateMoreThanMaxMonthsAuction()
        {
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
                EndDate = startDate.AddMonths(maxNumberOfMonths + 1),
                Ended = false,
            };
            var auctionResult = this.auctionService.AddAuction(auction);
            Assert.True(!this.libraryContextMock.Auctions.Any());
        }

        /// <summary>
        /// Test Get last auction without auctions.
        /// </summary>
        [Test]
        public void TestGetLastAuctionForUserWithoutAuctions()
        {
            var auctionUser = new AuctionUser { FirstName = "Ionel", LastName = "Pascu", Gender = "M" };
            var result2 = this.auctionUserService.AddAuctionUser(auctionUser, Role.Seller);
            var lastAuction = this.auctionService.GetLastAuctionForUser(auctionUser);
            Assert.IsNull(lastAuction);
        }
    }
}