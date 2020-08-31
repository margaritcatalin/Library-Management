// <copyright file="BidTests.cs" company="Transilvania University of Brasov">
// Margarit Marian Catalin
// </copyright>

namespace LibraryManagementTests
{
    using System;
    using System.Linq;
    using LibraryManagement.BusinessLayer;
    using LibraryManagement.DataMapper;
    using LibraryManagement.DomainModel;
    using LibraryManagement.Util;
    using NUnit.Framework;
    using Telerik.JustMock.EntityFramework;

    /// <summary>
    /// The bid unit tests.
    /// </summary>
    [TestFixture]
    public class BidTests
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
            this.libraryContextMock = EntityFrameworkMock.Create<LibraryDbContext>();
            EntityFrameworkMock.PrepareMock(this.libraryContextMock);
            this.priceService = new PriceService(new PriceRepository(this.libraryContextMock));
            this.productService = new ProductService(new ProductRepository(this.libraryContextMock));
            this.categoryService = new CategoryService(new CategoryRepository(this.libraryContextMock));
            this.userReviewService = new UserReviewService(new UserReviewRepository(this.libraryContextMock));
            this.auctionUserService = new AuctionUserService(new AuctionUserRepository(this.libraryContextMock), this.userReviewService);
            this.auctionService = new AuctionService(new AuctionRepository(this.libraryContextMock), this.categoryService, this.auctionUserService);
            this.bidService = new BidService(new BidRepository(this.libraryContextMock), this.auctionService);
        }

        /// <summary>
        /// Test add a new bid.
        /// </summary>
        [Test]
        public void TestAddBid()
        {
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
            { Auction = auctionById, BidUser = userBuyer, BidPrice = bidPrice, BidDate = DateTime.Now };
            var resultBid = this.bidService.AddBid(bid);
            Assert.True(this.libraryContextMock.Bids.Count() == 1);
        }

        /// <summary>
        /// Test add a null new bid.
        /// </summary>
        [Test]
        public void TestAddNullBid()
        {
            var resultBid = this.bidService.AddBid(null);
            Assert.True(!this.libraryContextMock.Bids.Any());
        }

        /// <summary>
        /// Test add a new bid with null auction.
        /// </summary>
        [Test]
        public void TestAddWithNullAuction()
        {
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
            var bidPrice = new Price { Id = 2, Currency = "Euro", Value = 108.5 };
            var bidPriceResult = this.priceService.AddPrice(bidPrice);
            var auctionById = this.auctionService.GetAuctionById(1);
            var bid = new Bid
            { Id = 1, Auction = null, BidUser = userBuyer, BidPrice = bidPrice, BidDate = DateTime.Now };
            var resultBid = this.bidService.AddBid(bid);
            Assert.True(!this.libraryContextMock.Bids.Any());
        }

        /// <summary>
        /// Test add a new bid with null bid user.
        /// </summary>
        [Test]
        public void TestAddWithNullBidUser()
        {
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
            var bidPrice = new Price { Id = 2, Currency = "Euro", Value = 108.5 };
            var bidPriceResult = this.priceService.AddPrice(bidPrice);
            var auctionById = this.auctionService.GetAuctionById(1);
            var bid = new Bid
            { Id = 1, Auction = auctionById, BidUser = null, BidPrice = bidPrice, BidDate = DateTime.Now };
            var resultBid = this.bidService.AddBid(bid);
            Assert.True(!this.libraryContextMock.Bids.Any());
        }

        /// <summary>
        /// Test add a new bid with null bid price.
        /// </summary>
        [Test]
        public void TestAddWithNullBidPrice()
        {
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
            var bidPrice = new Price { Id = 2, Currency = "Euro", Value = 108.5 };
            var bidPriceResult = this.priceService.AddPrice(bidPrice);
            var auctionById = this.auctionService.GetAuctionById(1);
            var bid = new Bid
            { Id = 1, Auction = auctionById, BidUser = userBuyer, BidPrice = null, BidDate = DateTime.Now };
            var resultBid = this.bidService.AddBid(bid);
            Assert.True(!this.libraryContextMock.Bids.Any());
        }

        /// <summary>
        /// Test add a new bid with wrong date.
        /// </summary>
        [Test]
        public void TestAddWithWrongDate()
        {
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
            var bidPrice = new Price { Id = 2, Currency = "Euro", Value = 108.5 };
            var bidPriceResult = this.priceService.AddPrice(bidPrice);
            var auctionById = this.auctionService.GetAuctionById(1);
            var bid = new Bid
            {
                Id = 1,
                Auction = auctionById,
                BidUser = userBuyer,
                BidPrice = bidPrice,
                BidDate = DateTime.Now.AddDays(5)
            };
            var resultBid = this.bidService.AddBid(bid);
            Assert.True(!this.libraryContextMock.Bids.Any());
        }

        /// <summary>
        /// Test add a new bid with wrong price currency.
        /// </summary>
        [Test]
        public void TestAddWithWrongPriceCurrency()
        {
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
            var bidPrice = new Price { Id = 2, Currency = "Dolar", Value = 108.5 };
            var bidPriceResult = this.priceService.AddPrice(bidPrice);
            var auctionById = this.auctionService.GetAuctionById(1);
            var bid = new Bid
            {
                Id = 1,
                Auction = auctionById,
                BidUser = userBuyer,
                BidPrice = bidPrice,
                BidDate = DateTime.Now
            };
            var resultBid = this.bidService.AddBid(bid);
            Assert.True(!this.libraryContextMock.Bids.Any());
        }

        /// <summary>
        /// Test add a new bid with a small price.
        /// </summary>
        [Test]
        public void TestAddWithWrongPriceSmallPrice()
        {
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
            var bidPrice = new Price { Id = 2, Currency = "Euro", Value = startPrice.Value - 1 };
            var bidPriceResult = this.priceService.AddPrice(bidPrice);
            var auctionById = this.auctionService.GetAuctionById(1);
            var bid = new Bid
            {
                Id = 1,
                Auction = auctionById,
                BidUser = userBuyer,
                BidPrice = bidPrice,
                BidDate = DateTime.Now
            };
            var resultBid = this.bidService.AddBid(bid);
            Assert.True(!this.libraryContextMock.Bids.Any());
        }

                /// <summary>
        /// Test add a new bid with biggest price.
        /// </summary>
        [Test]
        public void TestAddWithWrongPriceBiggestPrice()
        {
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
            var bidPrice = new Price { Id = 2, Currency = "Euro", Value = startPrice.Value * 2 };
            var bidPriceResult = this.priceService.AddPrice(bidPrice);
            var auctionById = this.auctionService.GetAuctionById(1);
            var bid = new Bid
            {
                Id = 1,
                Auction = auctionById,
                BidUser = userBuyer,
                BidPrice = bidPrice,
                BidDate = DateTime.Now
            };
            var resultBid = this.bidService.AddBid(bid);
            Assert.True(!this.libraryContextMock.Bids.Any());
        }
        
        /// <summary>
        /// Test add a new bid with wrong price currency.
        /// </summary>
        [Test]
        public void TestAddWithWrongUserRole()
        {
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
            var result2 = this.auctionUserService.AddAuctionUser(auctionUser2, Role.Seller);
            var userBuyer = this.auctionUserService.GetAuctionUserByFistNameAndLastName(auctionUser2.FirstName, auctionUser2.LastName);
            var bidPrice = new Price { Id = 2, Currency = "Euro", Value = 108.5 };
            var bidPriceResult = this.priceService.AddPrice(bidPrice);
            var auctionById = this.auctionService.GetAuctionById(1);
            var bid = new Bid
            {
                Id = 1,
                Auction = auctionById,
                BidUser = userBuyer,
                BidPrice = bidPrice,
                BidDate = DateTime.Now
            };
            var resultBid = this.bidService.AddBid(bid);
            Assert.True(!this.libraryContextMock.Bids.Any());
        }

        /// <summary>
        /// Test get bid by id and he is in the database.
        /// </summary>
        [Test]
        public void TestGetBidByGoodId()
        {
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
            var bidPrice = new Price { Id = 2, Currency = "Euro", Value = 108.5 };
            var bidPriceResult = this.priceService.AddPrice(bidPrice);
            var auctionById = this.auctionService.GetAuctionById(1);
            var bidId = 1;
            var bid = new Bid
            {
                Id = bidId,
                Auction = auctionById,
                BidUser = userBuyer,
                BidPrice = bidPrice,
                BidDate = DateTime.Now
            };
            var resultBid = this.bidService.AddBid(bid);
            var priceById = this.priceService.GetPriceById(bidId);
            Assert.NotNull(priceById);
        }

        /// <summary>
        /// Test get bid by id and he is not in the database.
        /// </summary>
        [Test]
        public void TestGetBidByBadId()
        {
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
            var bidPrice = new Price { Id = 2, Currency = "Euro", Value = 108.5 };
            var bidPriceResult = this.priceService.AddPrice(bidPrice);
            var auctionById = this.auctionService.GetAuctionById(1);
            var bidId = 1;
            var bid = new Bid
            {
                Id = bidId,
                Auction = auctionById,
                BidUser = userBuyer,
                BidPrice = bidPrice,
                BidDate = DateTime.Now
            };
            var resultBid = this.bidService.AddBid(bid);
            var bidById = this.bidService.GetBidById(bidId + 1);
            Assert.Null(bidById);
        }

        /// <summary>
        /// Test get all bids.
        /// </summary>
        [Test]
        public void TestGetAllBids()
        {
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
            var bid = new Bid
            {
                Id = bidId,
                Auction = auctionById,
                BidUser = userBuyer,
                BidPrice = bidPrice,
                BidDate = DateTime.Now
            };
            var resultBid = this.bidService.AddBid(bid);
            var bids = this.bidService.GetBids();
            Assert.IsTrue(bids.Count() == 1);
        }

        /// <summary>
        /// Test get all bids with a wrong user bid.
        /// </summary>
        [Test]
        public void TestGetAllBidsWithAWrongBid()
        {
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
            var bidPrice = new Price { Id = 2, Currency = "Euro", Value = 108.5 };
            var bidPriceResult = this.priceService.AddPrice(bidPrice);
            var auctionById = this.auctionService.GetAuctionById(1);
            var bidId = 1;
            var bid = new Bid
            {
                Id = bidId,
                Auction = auctionById,
                BidUser = null,
                BidPrice = bidPrice,
                BidDate = DateTime.Now
            };
            var resultBid = this.bidService.AddBid(bid);
            var bids = this.bidService.GetBids();
            Assert.IsFalse(bids.Count() == 1);
        }

        /// <summary>
        /// Test delete bid.
        /// </summary>
        [Test]
        public void TestDeleteBid()
        {
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
            var deleteResult = this.bidService.DeleteBid(bidId);
            Assert.True(!this.libraryContextMock.Bids.Any());
        }

        /// <summary>
        /// Test delete bid with it is not in the database.
        /// </summary>
        [Test]
        public void TestDeleteBidWithAWrongId()
        {
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
            var deleteResult = this.bidService.DeleteBid(bidId + 1);
            Assert.True(this.libraryContextMock.Bids.Count() == 1);
        }
    }
}