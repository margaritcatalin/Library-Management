// <copyright file="BidTests.cs" company="Transilvania University of Brasov">
// Margarit Marian Catalin
// </copyright>

using System;
using System.Collections.Generic;
using System.Configuration;
using LibraryManagement.DomainModel;
using LibraryManagement.Util;

namespace LibraryManagementTests
{
    using System.Linq;
    using NUnit.Framework;
    using LibraryManagement.BusinessLayer;
    using LibraryManagement.DataMapper;
    using Telerik.JustMock.EntityFramework;

    /// <summary>
    /// The bid unit tests.
    /// </summary>
    [TestFixture]
    public class BidTests
    {
        private LibraryDbContext libraryContextMock;

        private BidService bidService;
        private ProductService productService;
        private AuctionUserService auctionUserService;
        private AuctionService auctionService;
        private CategoryService categoryService;
        private PriceService priceService;
        private UserReviewService userReviewService;

        /// <summary>
        /// Tests setup.
        /// </summary>
        [SetUp]
        public void SetUp()
        {
            this.libraryContextMock = EntityFrameworkMock.Create<LibraryDbContext>();
            this.priceService = new PriceService(new PriceRepository(this.libraryContextMock));
            this.productService = new ProductService(new ProductRepository(this.libraryContextMock));
            this.categoryService = new CategoryService(new CategoryRepository(this.libraryContextMock));
            this.userReviewService = new UserReviewService(new UserReviewRepository(this.libraryContextMock));
            this.auctionUserService = new AuctionUserService(new AuctionUserRepository(this.libraryContextMock),
                this.userReviewService);
            this.auctionService = new AuctionService(new AuctionRepository(this.libraryContextMock),
                this.categoryService, this.auctionUserService);
            this.bidService = new BidService(new BidRepository(this.libraryContextMock), this.auctionService);
        }

        /// <summary>
        /// Test add a new bid.
        /// </summary>
        [Test]
        public void TestAddBid()
        {
            var auctionUser = new AuctionUser {Id = 1, FirstName = "Ionel", LastName = "Pascu", Gender = "M"};
            var result = this.auctionUserService.AddAuctionUser(auctionUser, Role.Seller);
            var userSeller = this.auctionUserService.GetAuctionUserByFistNameAndLastName(auctionUser.FirstName,
                auctionUser.LastName);
            var startPrice = new Price {Id = 1, Currency = "Euro", Value = 88.5};
            var priceResult = this.priceService.AddPrice(startPrice);
            var category = new Category {Id = 1, Name = "Legume"};
            var categoryResult = this.categoryService.AddCategory(category);
            var product = new Product {Id = 1, Name = "Fasole", Categories = new[] {category}};
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
            var auctionUser2 = new AuctionUser {Id = 2, FirstName = "Ioana", LastName = "Pascu", Gender = "F"};
            var result2 = this.auctionUserService.AddAuctionUser(auctionUser2, Role.Buyer);
            var userBuyer = this.auctionUserService.GetAuctionUserByFistNameAndLastName(auctionUser2.FirstName,
                auctionUser2.LastName);
            var bidPrice = new Price {Id = 2, Currency = "Euro", Value = 108.5};
            var bidPriceResult = this.priceService.AddPrice(bidPrice);
            var auctionById = this.auctionService.GetAuctionById(1);
            var bid = new Bid
                {Id = 1, Auction = auctionById, BidUser = userBuyer, BidPrice = bidPrice, BidDate = DateTime.Now};
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
            var auctionUser = new AuctionUser {Id = 1, FirstName = "Ionel", LastName = "Pascu", Gender = "M"};
            var result = this.auctionUserService.AddAuctionUser(auctionUser, Role.Seller);
            var userSeller = this.auctionUserService.GetAuctionUserByFistNameAndLastName(auctionUser.FirstName,
                auctionUser.LastName);
            var startPrice = new Price {Id = 1, Currency = "Euro", Value = 88.5};
            var priceResult = this.priceService.AddPrice(startPrice);
            var category = new Category {Id = 1, Name = "Legume"};
            var categoryResult = this.categoryService.AddCategory(category);
            var product = new Product {Id = 1, Name = "Fasole", Categories = new[] {category}};
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
            var auctionUser2 = new AuctionUser {Id = 2, FirstName = "Ioana", LastName = "Pascu", Gender = "F"};
            var result2 = this.auctionUserService.AddAuctionUser(auctionUser2, Role.Buyer);
            var userBuyer = this.auctionUserService.GetAuctionUserByFistNameAndLastName(auctionUser2.FirstName,
                auctionUser2.LastName);
            var bidPrice = new Price {Id = 2, Currency = "Euro", Value = 108.5};
            var bidPriceResult = this.priceService.AddPrice(bidPrice);
            var auctionById = this.auctionService.GetAuctionById(1);
            var bid = new Bid
                {Id = 1, Auction = null, BidUser = userBuyer, BidPrice = bidPrice, BidDate = DateTime.Now};
            var resultBid = this.bidService.AddBid(bid);
            Assert.True(!this.libraryContextMock.Bids.Any());
        }

        /// <summary>
        /// Test add a new bid with null bid user.
        /// </summary>
        [Test]
        public void TestAddWithNullBidUser()
        {
            var auctionUser = new AuctionUser {Id = 1, FirstName = "Ionel", LastName = "Pascu", Gender = "M"};
            var result = this.auctionUserService.AddAuctionUser(auctionUser, Role.Seller);
            var userSeller = this.auctionUserService.GetAuctionUserByFistNameAndLastName(auctionUser.FirstName,
                auctionUser.LastName);
            var startPrice = new Price {Id = 1, Currency = "Euro", Value = 88.5};
            var priceResult = this.priceService.AddPrice(startPrice);
            var category = new Category {Id = 1, Name = "Legume"};
            var categoryResult = this.categoryService.AddCategory(category);
            var product = new Product {Id = 1, Name = "Fasole", Categories = new[] {category}};
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
            var auctionUser2 = new AuctionUser {Id = 2, FirstName = "Ioana", LastName = "Pascu", Gender = "F"};
            var result2 = this.auctionUserService.AddAuctionUser(auctionUser2, Role.Buyer);
            var userBuyer = this.auctionUserService.GetAuctionUserByFistNameAndLastName(auctionUser2.FirstName,
                auctionUser2.LastName);
            var bidPrice = new Price {Id = 2, Currency = "Euro", Value = 108.5};
            var bidPriceResult = this.priceService.AddPrice(bidPrice);
            var auctionById = this.auctionService.GetAuctionById(1);
            var bid = new Bid
                {Id = 1, Auction = auctionById, BidUser = null, BidPrice = bidPrice, BidDate = DateTime.Now};
            var resultBid = this.bidService.AddBid(bid);
            Assert.True(!this.libraryContextMock.Bids.Any());
        }

        /// <summary>
        /// Test add a new bid with null bid price.
        /// </summary>
        [Test]
        public void TestAddWithNullBidPrice()
        {
            var auctionUser = new AuctionUser {Id = 1, FirstName = "Ionel", LastName = "Pascu", Gender = "M"};
            var result = this.auctionUserService.AddAuctionUser(auctionUser, Role.Seller);
            var userSeller = this.auctionUserService.GetAuctionUserByFistNameAndLastName(auctionUser.FirstName,
                auctionUser.LastName);
            var startPrice = new Price {Id = 1, Currency = "Euro", Value = 88.5};
            var priceResult = this.priceService.AddPrice(startPrice);
            var category = new Category {Id = 1, Name = "Legume"};
            var categoryResult = this.categoryService.AddCategory(category);
            var product = new Product {Id = 1, Name = "Fasole", Categories = new[] {category}};
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
            var auctionUser2 = new AuctionUser {Id = 2, FirstName = "Ioana", LastName = "Pascu", Gender = "F"};
            var result2 = this.auctionUserService.AddAuctionUser(auctionUser2, Role.Buyer);
            var userBuyer = this.auctionUserService.GetAuctionUserByFistNameAndLastName(auctionUser2.FirstName,
                auctionUser2.LastName);
            var bidPrice = new Price {Id = 2, Currency = "Euro", Value = 108.5};
            var bidPriceResult = this.priceService.AddPrice(bidPrice);
            var auctionById = this.auctionService.GetAuctionById(1);
            var bid = new Bid
                {Id = 1, Auction = auctionById, BidUser = userBuyer, BidPrice = null, BidDate = DateTime.Now};
            var resultBid = this.bidService.AddBid(bid);
            Assert.True(!this.libraryContextMock.Bids.Any());
        }

        /// <summary>
        /// Test add a new bid with wrong date.
        /// </summary>
        [Test]
        public void TestAddWithWrongDate()
        {
            var auctionUser = new AuctionUser {Id = 1, FirstName = "Ionel", LastName = "Pascu", Gender = "M"};
            var result = this.auctionUserService.AddAuctionUser(auctionUser, Role.Seller);
            var userSeller = this.auctionUserService.GetAuctionUserByFistNameAndLastName(auctionUser.FirstName,
                auctionUser.LastName);
            var startPrice = new Price {Id = 1, Currency = "Euro", Value = 88.5};
            var priceResult = this.priceService.AddPrice(startPrice);
            var category = new Category {Id = 1, Name = "Legume"};
            var categoryResult = this.categoryService.AddCategory(category);
            var product = new Product {Id = 1, Name = "Fasole", Categories = new[] {category}};
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
            var auctionUser2 = new AuctionUser {Id = 2, FirstName = "Ioana", LastName = "Pascu", Gender = "F"};
            var result2 = this.auctionUserService.AddAuctionUser(auctionUser2, Role.Buyer);
            var userBuyer = this.auctionUserService.GetAuctionUserByFistNameAndLastName(auctionUser2.FirstName,
                auctionUser2.LastName);
            var bidPrice = new Price {Id = 2, Currency = "Euro", Value = 108.5};
            var bidPriceResult = this.priceService.AddPrice(bidPrice);
            var auctionById = this.auctionService.GetAuctionById(1);
            var bid = new Bid
            {
                Id = 1, Auction = auctionById, BidUser = userBuyer, BidPrice = bidPrice,
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
            var auctionUser = new AuctionUser {Id = 1, FirstName = "Ionel", LastName = "Pascu", Gender = "M"};
            var result = this.auctionUserService.AddAuctionUser(auctionUser, Role.Seller);
            var userSeller = this.auctionUserService.GetAuctionUserByFistNameAndLastName(auctionUser.FirstName,
                auctionUser.LastName);
            var startPrice = new Price {Id = 1, Currency = "Euro", Value = 88.5};
            var priceResult = this.priceService.AddPrice(startPrice);
            var category = new Category {Id = 1, Name = "Legume"};
            var categoryResult = this.categoryService.AddCategory(category);
            var product = new Product {Id = 1, Name = "Fasole", Categories = new[] {category}};
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
            var auctionUser2 = new AuctionUser {Id = 2, FirstName = "Ioana", LastName = "Pascu", Gender = "F"};
            var result2 = this.auctionUserService.AddAuctionUser(auctionUser2, Role.Buyer);
            var userBuyer = this.auctionUserService.GetAuctionUserByFistNameAndLastName(auctionUser2.FirstName,
                auctionUser2.LastName);
            var bidPrice = new Price {Id = 2, Currency = "Dolar", Value = 108.5};
            var bidPriceResult = this.priceService.AddPrice(bidPrice);
            var auctionById = this.auctionService.GetAuctionById(1);
            var bid = new Bid
            {
                Id = 1, Auction = auctionById, BidUser = userBuyer, BidPrice = bidPrice, BidDate = DateTime.Now
            };
            var resultBid = this.bidService.AddBid(bid);
            Assert.True(!this.libraryContextMock.Bids.Any());
        }

        /// <summary>
        /// Test add a new bid with wrong price currency.
        /// </summary>
        [Test]
        public void TestAddWithWrongPriceSmallPrice()
        {
            var auctionUser = new AuctionUser {Id = 1, FirstName = "Ionel", LastName = "Pascu", Gender = "M"};
            var result = this.auctionUserService.AddAuctionUser(auctionUser, Role.Seller);
            var userSeller = this.auctionUserService.GetAuctionUserByFistNameAndLastName(auctionUser.FirstName,
                auctionUser.LastName);
            var startPrice = new Price {Id = 1, Currency = "Euro", Value = 88.5};
            var priceResult = this.priceService.AddPrice(startPrice);
            var category = new Category {Id = 1, Name = "Legume"};
            var categoryResult = this.categoryService.AddCategory(category);
            var product = new Product {Id = 1, Name = "Fasole", Categories = new[] {category}};
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
            var auctionUser2 = new AuctionUser {Id = 2, FirstName = "Ioana", LastName = "Pascu", Gender = "F"};
            var result2 = this.auctionUserService.AddAuctionUser(auctionUser2, Role.Buyer);
            var userBuyer = this.auctionUserService.GetAuctionUserByFistNameAndLastName(auctionUser2.FirstName,
                auctionUser2.LastName);
            var bidPrice = new Price {Id = 2, Currency = "Euro", Value = 88.5 + 1};
            var bidPriceResult = this.priceService.AddPrice(bidPrice);
            var auctionById = this.auctionService.GetAuctionById(1);
            var bid = new Bid
            {
                Id = 1, Auction = auctionById, BidUser = userBuyer, BidPrice = bidPrice, BidDate = DateTime.Now
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
            var auctionUser = new AuctionUser {Id = 1, FirstName = "Ionel", LastName = "Pascu", Gender = "M"};
            var result = this.auctionUserService.AddAuctionUser(auctionUser, Role.Seller);
            var userSeller = this.auctionUserService.GetAuctionUserByFistNameAndLastName(auctionUser.FirstName,
                auctionUser.LastName);
            var startPrice = new Price {Id = 1, Currency = "Euro", Value = 88.5};
            var priceResult = this.priceService.AddPrice(startPrice);
            var category = new Category {Id = 1, Name = "Legume"};
            var categoryResult = this.categoryService.AddCategory(category);
            var product = new Product {Id = 1, Name = "Fasole", Categories = new[] {category}};
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
            var auctionUser2 = new AuctionUser {Id = 2, FirstName = "Ioana", LastName = "Pascu", Gender = "F"};
            var result2 = this.auctionUserService.AddAuctionUser(auctionUser2, Role.Seller);
            var userBuyer = this.auctionUserService.GetAuctionUserByFistNameAndLastName(auctionUser2.FirstName,
                auctionUser2.LastName);
            var bidPrice = new Price {Id = 2, Currency = "Euro", Value = 108.5};
            var bidPriceResult = this.priceService.AddPrice(bidPrice);
            var auctionById = this.auctionService.GetAuctionById(1);
            var bid = new Bid
            {
                Id = 1, Auction = auctionById, BidUser = userBuyer, BidPrice = bidPrice, BidDate = DateTime.Now
            };
            var resultBid = this.bidService.AddBid(bid);
            Assert.True(!this.libraryContextMock.Bids.Any());
        }

        /// <summary>
        /// Test get bid by id and he is in the db.
        /// </summary>
        [Test]
        public void TestGetBidByGoodId()
        {
            var auctionUser = new AuctionUser {Id = 1, FirstName = "Ionel", LastName = "Pascu", Gender = "M"};
            var result = this.auctionUserService.AddAuctionUser(auctionUser, Role.Seller);
            var userSeller = this.auctionUserService.GetAuctionUserByFistNameAndLastName(auctionUser.FirstName,
                auctionUser.LastName);
            var startPrice = new Price {Id = 1, Currency = "Euro", Value = 88.5};
            var priceResult = this.priceService.AddPrice(startPrice);
            var category = new Category {Id = 1, Name = "Legume"};
            var categoryResult = this.categoryService.AddCategory(category);
            var product = new Product {Id = 1, Name = "Fasole", Categories = new[] {category}};
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
            var auctionUser2 = new AuctionUser {Id = 2, FirstName = "Ioana", LastName = "Pascu", Gender = "F"};
            var result2 = this.auctionUserService.AddAuctionUser(auctionUser2, Role.Buyer);
            var userBuyer = this.auctionUserService.GetAuctionUserByFistNameAndLastName(auctionUser2.FirstName,
                auctionUser2.LastName);
            var bidPrice = new Price {Id = 2, Currency = "Euro", Value = 108.5};
            var bidPriceResult = this.priceService.AddPrice(bidPrice);
            var auctionById = this.auctionService.GetAuctionById(1);
            var bidId = 1;
            var bid = new Bid
            {
                Id = bidId, Auction = auctionById, BidUser = userBuyer, BidPrice = bidPrice, BidDate = DateTime.Now
            };
            var resultBid = this.bidService.AddBid(bid);
            var priceById = this.priceService.GetPriceById(bidId);
            Assert.NotNull(priceById);
        }

        /// <summary>
        /// Test get bid by id and he is not in the db.
        /// </summary>
        [Test]
        public void TestGetBidByBadId()
        {
            var auctionUser = new AuctionUser {Id = 1, FirstName = "Ionel", LastName = "Pascu", Gender = "M"};
            var result = this.auctionUserService.AddAuctionUser(auctionUser, Role.Seller);
            var userSeller = this.auctionUserService.GetAuctionUserByFistNameAndLastName(auctionUser.FirstName,
                auctionUser.LastName);
            var startPrice = new Price {Id = 1, Currency = "Euro", Value = 88.5};
            var priceResult = this.priceService.AddPrice(startPrice);
            var category = new Category {Id = 1, Name = "Legume"};
            var categoryResult = this.categoryService.AddCategory(category);
            var product = new Product {Id = 1, Name = "Fasole", Categories = new[] {category}};
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
            var auctionUser2 = new AuctionUser {Id = 2, FirstName = "Ioana", LastName = "Pascu", Gender = "F"};
            var result2 = this.auctionUserService.AddAuctionUser(auctionUser2, Role.Buyer);
            var userBuyer = this.auctionUserService.GetAuctionUserByFistNameAndLastName(auctionUser2.FirstName,
                auctionUser2.LastName);
            var bidPrice = new Price {Id = 2, Currency = "Euro", Value = 108.5};
            var bidPriceResult = this.priceService.AddPrice(bidPrice);
            var auctionById = this.auctionService.GetAuctionById(1);
            var bidId = 1;
            var bid = new Bid
            {
                Id = bidId, Auction = auctionById, BidUser = userBuyer, BidPrice = bidPrice, BidDate = DateTime.Now
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
            var auctionUser = new AuctionUser {Id = 1, FirstName = "Ionel", LastName = "Pascu", Gender = "M"};
            var result = this.auctionUserService.AddAuctionUser(auctionUser, Role.Seller);
            var userSeller = this.auctionUserService.GetAuctionUserByFistNameAndLastName(auctionUser.FirstName,
                auctionUser.LastName);
            var startPrice = new Price {Id = 1, Currency = "Euro", Value = 88.5};
            var priceResult = this.priceService.AddPrice(startPrice);
            var category = new Category {Id = 1, Name = "Legume"};
            var categoryResult = this.categoryService.AddCategory(category);
            var product = new Product {Id = 1, Name = "Fasole", Categories = new[] {category}};
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
            var auctionUser2 = new AuctionUser {Id = 2, FirstName = "Ioana", LastName = "Pascu", Gender = "F"};
            var result2 = this.auctionUserService.AddAuctionUser(auctionUser2, Role.Buyer);
            var userBuyer = this.auctionUserService.GetAuctionUserByFistNameAndLastName(auctionUser2.FirstName,
                auctionUser2.LastName);
            var bidPrice = new Price {Id = 2, Currency = "Euro", Value = 108.5};
            var bidPriceResult = this.priceService.AddPrice(bidPrice);
            var auctionById = this.auctionService.GetAuctionById(1);
            var bidId = 1;
            var bid = new Bid
            {
                Id = bidId, Auction = auctionById, BidUser = userBuyer, BidPrice = bidPrice, BidDate = DateTime.Now
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
            var auctionUser = new AuctionUser {Id = 1, FirstName = "Ionel", LastName = "Pascu", Gender = "M"};
            var result = this.auctionUserService.AddAuctionUser(auctionUser, Role.Seller);
            var userSeller = this.auctionUserService.GetAuctionUserByFistNameAndLastName(auctionUser.FirstName,
                auctionUser.LastName);
            var startPrice = new Price {Id = 1, Currency = "Euro", Value = 88.5};
            var priceResult = this.priceService.AddPrice(startPrice);
            var category = new Category {Id = 1, Name = "Legume"};
            var categoryResult = this.categoryService.AddCategory(category);
            var product = new Product {Id = 1, Name = "Fasole", Categories = new[] {category}};
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
            var auctionUser2 = new AuctionUser {Id = 2, FirstName = "Ioana", LastName = "Pascu", Gender = "F"};
            var result2 = this.auctionUserService.AddAuctionUser(auctionUser2, Role.Buyer);
            var userBuyer = this.auctionUserService.GetAuctionUserByFistNameAndLastName(auctionUser2.FirstName,
                auctionUser2.LastName);
            var bidPrice = new Price {Id = 2, Currency = "Euro", Value = 108.5};
            var bidPriceResult = this.priceService.AddPrice(bidPrice);
            var auctionById = this.auctionService.GetAuctionById(1);
            var bidId = 1;
            var bid = new Bid
            {
                Id = bidId, Auction = auctionById, BidUser = null, BidPrice = bidPrice, BidDate = DateTime.Now
            };
            var resultBid = this.bidService.AddBid(bid);
            var bids = this.bidService.GetBids();
            Assert.IsFalse(bids.Count() == 1);
        }

        /// <summary>
        /// Test update auction for bid.
        /// </summary>
        [Test]
        public void TestUpdateBidAuction()
        {
            var auctionUser = new AuctionUser {Id = 1, FirstName = "Ionel", LastName = "Pascu", Gender = "M"};
            var result = this.auctionUserService.AddAuctionUser(auctionUser, Role.Seller);
            var userSeller = this.auctionUserService.GetAuctionUserByFistNameAndLastName(auctionUser.FirstName,
                auctionUser.LastName);
            var startPrice = new Price {Id = 1, Currency = "Euro", Value = 88.5};
            var priceResult = this.priceService.AddPrice(startPrice);
            var category = new Category {Id = 1, Name = "Legume"};
            var categoryResult = this.categoryService.AddCategory(category);
            var product = new Product {Id = 1, Name = "Fasole", Categories = new[] {category}};
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
            var auctionUser2 = new AuctionUser {Id = 2, FirstName = "Ioana", LastName = "Pascu", Gender = "F"};
            var result2 = this.auctionUserService.AddAuctionUser(auctionUser2, Role.Buyer);
            var userBuyer = this.auctionUserService.GetAuctionUserByFistNameAndLastName(auctionUser2.FirstName,
                auctionUser2.LastName);
            var bidPrice = new Price {Id = 2, Currency = "Euro", Value = 108.5};
            var bidPriceResult = this.priceService.AddPrice(bidPrice);
            var auctionById = this.auctionService.GetAuctionById(1);
            var bidId = 1;
            var bid = new Bid
            {
                Id = bidId, Auction = auctionById, BidUser = userBuyer, BidPrice = bidPrice, BidDate = DateTime.Now
            };
            var resultBid = this.bidService.AddBid(bid);
            var bidById = this.bidService.GetBidById(bidId);
            var auction2 = new Auction
            {
                Id = 2,
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
            bidById = this.bidService.GetBidById(bidId);
            Assert.IsTrue(bidById.Auction.Id == auction2.Id);
        }

        /// <summary>
        /// Test update with a wrong auction for bid.
        /// </summary>
        [Test]
        public void TestUpdateWithBadBidAuction()
        {
            var auctionUser = new AuctionUser {Id = 1, FirstName = "Ionel", LastName = "Pascu", Gender = "M"};
            var result = this.auctionUserService.AddAuctionUser(auctionUser, Role.Seller);
            var userSeller = this.auctionUserService.GetAuctionUserByFistNameAndLastName(auctionUser.FirstName,
                auctionUser.LastName);
            var startPrice = new Price {Id = 1, Currency = "Euro", Value = 88.5};
            var priceResult = this.priceService.AddPrice(startPrice);
            var category = new Category {Id = 1, Name = "Legume"};
            var categoryResult = this.categoryService.AddCategory(category);
            var product = new Product {Id = 1, Name = "Fasole", Categories = new[] {category}};
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
            var auctionUser2 = new AuctionUser {Id = 2, FirstName = "Ioana", LastName = "Pascu", Gender = "F"};
            var result2 = this.auctionUserService.AddAuctionUser(auctionUser2, Role.Buyer);
            var userBuyer = this.auctionUserService.GetAuctionUserByFistNameAndLastName(auctionUser2.FirstName,
                auctionUser2.LastName);
            var bidPrice = new Price {Id = 2, Currency = "Euro", Value = 108.5};
            var bidPriceResult = this.priceService.AddPrice(bidPrice);
            var auctionById = this.auctionService.GetAuctionById(1);
            var bidId = 1;
            var bid = new Bid
            {
                Id = bidId, Auction = auctionById, BidUser = userBuyer, BidPrice = bidPrice, BidDate = DateTime.Now
            };
            var resultBid = this.bidService.AddBid(bid);
            var bidById = this.bidService.GetBidById(bidId);
            bidById.Auction = null;
            var updateResult = this.bidService.UpdateBid(bidById);
            bidById = this.bidService.GetBidById(bidId);
            Assert.IsNotNull(bidById.Auction);
        }

        /// <summary>
        /// Test update user for bid.
        /// </summary>
        [Test]
        public void TestUpdateBidUser()
        {
            var auctionUser = new AuctionUser {Id = 1, FirstName = "Ionel", LastName = "Pascu", Gender = "M"};
            var result = this.auctionUserService.AddAuctionUser(auctionUser, Role.Seller);
            var userSeller = this.auctionUserService.GetAuctionUserByFistNameAndLastName(auctionUser.FirstName,
                auctionUser.LastName);
            var startPrice = new Price {Id = 1, Currency = "Euro", Value = 88.5};
            var priceResult = this.priceService.AddPrice(startPrice);
            var category = new Category {Id = 1, Name = "Legume"};
            var categoryResult = this.categoryService.AddCategory(category);
            var product = new Product {Id = 1, Name = "Fasole", Categories = new[] {category}};
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
            var auctionUser2 = new AuctionUser {Id = 2, FirstName = "Ioana", LastName = "Pascu", Gender = "F"};
            var result2 = this.auctionUserService.AddAuctionUser(auctionUser2, Role.Buyer);
            var userBuyer = this.auctionUserService.GetAuctionUserByFistNameAndLastName(auctionUser2.FirstName,
                auctionUser2.LastName);
            var bidPrice = new Price {Id = 2, Currency = "Euro", Value = 108.5};
            var bidPriceResult = this.priceService.AddPrice(bidPrice);
            var auctionById = this.auctionService.GetAuctionById(1);
            var bidId = 1;
            var bid = new Bid
            {
                Id = bidId, Auction = auctionById, BidUser = userBuyer, BidPrice = bidPrice, BidDate = DateTime.Now
            };
            var resultBid = this.bidService.AddBid(bid);
            var bidById = this.bidService.GetBidById(bidId);
            var auctionUser3 = new AuctionUser {Id = 2, FirstName = "Anton", LastName = "Mihai", Gender = "F"};
            var result3 = this.auctionUserService.AddAuctionUser(auctionUser3, Role.Buyer);
            var userBuyer3 = this.auctionUserService.GetAuctionUserByFistNameAndLastName(auctionUser3.FirstName,
                auctionUser3.LastName);

            bidById.BidUser = userBuyer3;
            var updateResult = this.bidService.UpdateBid(bidById);
            bidById = this.bidService.GetBidById(bidId);
            Assert.IsTrue(bidById.BidUser.Id == userBuyer3.Id);
        }

        /// <summary>
        /// Test update with a wrong userr for bid.
        /// </summary>
        [Test]
        public void TestUpdateWithBadBidUser()
        {
            var auctionUser = new AuctionUser {Id = 1, FirstName = "Ionel", LastName = "Pascu", Gender = "M"};
            var result = this.auctionUserService.AddAuctionUser(auctionUser, Role.Seller);
            var userSeller = this.auctionUserService.GetAuctionUserByFistNameAndLastName(auctionUser.FirstName,
                auctionUser.LastName);
            var startPrice = new Price {Id = 1, Currency = "Euro", Value = 88.5};
            var priceResult = this.priceService.AddPrice(startPrice);
            var category = new Category {Id = 1, Name = "Legume"};
            var categoryResult = this.categoryService.AddCategory(category);
            var product = new Product {Id = 1, Name = "Fasole", Categories = new[] {category}};
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
            var auctionUser2 = new AuctionUser {Id = 2, FirstName = "Ioana", LastName = "Pascu", Gender = "F"};
            var result2 = this.auctionUserService.AddAuctionUser(auctionUser2, Role.Buyer);
            var userBuyer = this.auctionUserService.GetAuctionUserByFistNameAndLastName(auctionUser2.FirstName,
                auctionUser2.LastName);
            var bidPrice = new Price {Id = 2, Currency = "Euro", Value = 108.5};
            var bidPriceResult = this.priceService.AddPrice(bidPrice);
            var auctionById = this.auctionService.GetAuctionById(1);
            var bidId = 1;
            var bid = new Bid
            {
                Id = bidId, Auction = auctionById, BidUser = userBuyer, BidPrice = bidPrice, BidDate = DateTime.Now
            };
            var resultBid = this.bidService.AddBid(bid);
            var bidById = this.bidService.GetBidById(bidId);
            var auctionUser3 = new AuctionUser {Id = 2, FirstName = "Anton", LastName = "Mihai", Gender = "F"};
            var result3 = this.auctionUserService.AddAuctionUser(auctionUser3, Role.Seller);
            var userBuyer3 = this.auctionUserService.GetAuctionUserByFistNameAndLastName(auctionUser3.FirstName,
                auctionUser3.LastName);
            bidById.BidUser = userBuyer3;
            var updateResult = this.bidService.UpdateBid(bidById);
            bidById = this.bidService.GetBidById(bidId);
            Assert.IsFalse(bidById.BidUser.Id == userBuyer3.Id);
        }

        /// <summary>
        /// Test update price for bid.
        /// </summary>
        [Test]
        public void TestUpdateBidPrice()
        {
            var auctionUser = new AuctionUser {Id = 1, FirstName = "Ionel", LastName = "Pascu", Gender = "M"};
            var result = this.auctionUserService.AddAuctionUser(auctionUser, Role.Seller);
            var userSeller = this.auctionUserService.GetAuctionUserByFistNameAndLastName(auctionUser.FirstName,
                auctionUser.LastName);
            var startPrice = new Price {Id = 1, Currency = "Euro", Value = 88.5};
            var priceResult = this.priceService.AddPrice(startPrice);
            var category = new Category {Id = 1, Name = "Legume"};
            var categoryResult = this.categoryService.AddCategory(category);
            var product = new Product {Id = 1, Name = "Fasole", Categories = new[] {category}};
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
            var auctionUser2 = new AuctionUser {Id = 2, FirstName = "Ioana", LastName = "Pascu", Gender = "F"};
            var result2 = this.auctionUserService.AddAuctionUser(auctionUser2, Role.Buyer);
            var userBuyer = this.auctionUserService.GetAuctionUserByFistNameAndLastName(auctionUser2.FirstName,
                auctionUser2.LastName);
            var bidPrice = new Price {Id = 2, Currency = "Euro", Value = 108.5};
            var bidPriceResult = this.priceService.AddPrice(bidPrice);
            var auctionById = this.auctionService.GetAuctionById(1);
            var bidId = 1;
            var bid = new Bid
            {
                Id = bidId, Auction = auctionById, BidUser = userBuyer, BidPrice = bidPrice, BidDate = DateTime.Now
            };
            var resultBid = this.bidService.AddBid(bid);
            var bidById = this.bidService.GetBidById(bidId);
            var bidPrice2 = new Price {Id = 3, Currency = "Euro", Value = 108.5};
            var bidPriceResult2 = this.priceService.AddPrice(bidPrice2);

            bidById.BidPrice = bidPrice2;
            var updateResult = this.bidService.UpdateBid(bidById);
            bidById = this.bidService.GetBidById(bidId);
            Assert.IsTrue(bidById.BidPrice.Id == bidPrice2.Id);
        }

        /// <summary>
        /// Test update with a wrong auction for bid.
        /// </summary>
        [Test]
        public void TestUpdateWithBadBidPrice()
        {
            var auctionUser = new AuctionUser {Id = 1, FirstName = "Ionel", LastName = "Pascu", Gender = "M"};
            var result = this.auctionUserService.AddAuctionUser(auctionUser, Role.Seller);
            var userSeller = this.auctionUserService.GetAuctionUserByFistNameAndLastName(auctionUser.FirstName,
                auctionUser.LastName);
            var startPrice = new Price {Id = 1, Currency = "Euro", Value = 88.5};
            var priceResult = this.priceService.AddPrice(startPrice);
            var category = new Category {Id = 1, Name = "Legume"};
            var categoryResult = this.categoryService.AddCategory(category);
            var product = new Product {Id = 1, Name = "Fasole", Categories = new[] {category}};
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
            var auctionUser2 = new AuctionUser {Id = 2, FirstName = "Ioana", LastName = "Pascu", Gender = "F"};
            var result2 = this.auctionUserService.AddAuctionUser(auctionUser2, Role.Buyer);
            var userBuyer = this.auctionUserService.GetAuctionUserByFistNameAndLastName(auctionUser2.FirstName,
                auctionUser2.LastName);
            var bidPrice = new Price {Id = 2, Currency = "Euro", Value = 108.5};
            var bidPriceResult = this.priceService.AddPrice(bidPrice);
            var auctionById = this.auctionService.GetAuctionById(1);
            var bidId = 1;
            var bid = new Bid
            {
                Id = bidId, Auction = auctionById, BidUser = userBuyer, BidPrice = bidPrice, BidDate = DateTime.Now
            };
            var resultBid = this.bidService.AddBid(bid);
            var bidById = this.bidService.GetBidById(bidId);
            var bidPrice2 = new Price {Id = 3, Currency = "Dolar", Value = 108.5};
            var bidPriceResult2 = this.priceService.AddPrice(bidPrice2);

            bidById.BidPrice = bidPrice2;
            var updateResult = this.bidService.UpdateBid(bidById);
            bidById = this.bidService.GetBidById(bidId);
            Assert.IsFalse(bidById.BidPrice.Id == bidPrice2.Id);
        }

        /// <summary>
        /// Test update date for bid.
        /// </summary>
        [Test]
        public void TestUpdateBidDate()
        {
            var auctionUser = new AuctionUser {Id = 1, FirstName = "Ionel", LastName = "Pascu", Gender = "M"};
            var result = this.auctionUserService.AddAuctionUser(auctionUser, Role.Seller);
            var userSeller = this.auctionUserService.GetAuctionUserByFistNameAndLastName(auctionUser.FirstName,
                auctionUser.LastName);
            var startPrice = new Price {Id = 1, Currency = "Euro", Value = 88.5};
            var priceResult = this.priceService.AddPrice(startPrice);
            var category = new Category {Id = 1, Name = "Legume"};
            var categoryResult = this.categoryService.AddCategory(category);
            var product = new Product {Id = 1, Name = "Fasole", Categories = new[] {category}};
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
            var auctionUser2 = new AuctionUser {Id = 2, FirstName = "Ioana", LastName = "Pascu", Gender = "F"};
            var result2 = this.auctionUserService.AddAuctionUser(auctionUser2, Role.Buyer);
            var userBuyer = this.auctionUserService.GetAuctionUserByFistNameAndLastName(auctionUser2.FirstName,
                auctionUser2.LastName);
            var bidPrice = new Price {Id = 2, Currency = "Euro", Value = 108.5};
            var bidPriceResult = this.priceService.AddPrice(bidPrice);
            var auctionById = this.auctionService.GetAuctionById(1);
            var bidId = 1;
            var bidDate = DateTime.Now;
            var bid = new Bid
            {
                Id = bidId, Auction = auctionById, BidUser = userBuyer, BidPrice = bidPrice, BidDate = bidDate
            };
            var resultBid = this.bidService.AddBid(bid);
            var bidById = this.bidService.GetBidById(bidId);
            bidById.BidDate = bidDate.AddDays(2);
            var updateResult = this.bidService.UpdateBid(bidById);
            bidById = this.bidService.GetBidById(bidId);
            Assert.IsTrue(DateTime.Compare(bidById.BidDate, bidDate) == 0);
        }

        /// <summary>
        /// Test delete bid.
        /// </summary>
        [Test]
        public void TestDeleteBid()
        {
            var auctionUser = new AuctionUser {Id = 1, FirstName = "Ionel", LastName = "Pascu", Gender = "M"};
            var result = this.auctionUserService.AddAuctionUser(auctionUser, Role.Seller);
            var userSeller = this.auctionUserService.GetAuctionUserByFistNameAndLastName(auctionUser.FirstName,
                auctionUser.LastName);
            var startPrice = new Price {Id = 1, Currency = "Euro", Value = 88.5};
            var priceResult = this.priceService.AddPrice(startPrice);
            var category = new Category {Id = 1, Name = "Legume"};
            var categoryResult = this.categoryService.AddCategory(category);
            var product = new Product {Id = 1, Name = "Fasole", Categories = new[] {category}};
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
            var auctionUser2 = new AuctionUser {Id = 2, FirstName = "Ioana", LastName = "Pascu", Gender = "F"};
            var result2 = this.auctionUserService.AddAuctionUser(auctionUser2, Role.Buyer);
            var userBuyer = this.auctionUserService.GetAuctionUserByFistNameAndLastName(auctionUser2.FirstName,
                auctionUser2.LastName);
            var bidPrice = new Price {Id = 2, Currency = "Euro", Value = 108.5};
            var bidPriceResult = this.priceService.AddPrice(bidPrice);
            var auctionById = this.auctionService.GetAuctionById(1);
            var bidId = 1;
            var bidDate = DateTime.Now;
            var bid = new Bid
            {
                Id = bidId, Auction = auctionById, BidUser = userBuyer, BidPrice = bidPrice, BidDate = bidDate
            };
            var resultBid = this.bidService.AddBid(bid);
            var deleteResult = this.bidService.DeleteBid(bidId);
            Assert.True(!this.libraryContextMock.Bids.Any());
        }

        /// <summary>
        /// Test delete bid with it is not in the db.
        /// </summary>
        [Test]
        public void TestDeleteBidWithAWrongId()
        {
            var auctionUser = new AuctionUser {Id = 1, FirstName = "Ionel", LastName = "Pascu", Gender = "M"};
            var result = this.auctionUserService.AddAuctionUser(auctionUser, Role.Seller);
            var userSeller = this.auctionUserService.GetAuctionUserByFistNameAndLastName(auctionUser.FirstName,
                auctionUser.LastName);
            var startPrice = new Price {Id = 1, Currency = "Euro", Value = 88.5};
            var priceResult = this.priceService.AddPrice(startPrice);
            var category = new Category {Id = 1, Name = "Legume"};
            var categoryResult = this.categoryService.AddCategory(category);
            var product = new Product {Id = 1, Name = "Fasole", Categories = new[] {category}};
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
            var auctionUser2 = new AuctionUser {Id = 2, FirstName = "Ioana", LastName = "Pascu", Gender = "F"};
            var result2 = this.auctionUserService.AddAuctionUser(auctionUser2, Role.Buyer);
            var userBuyer = this.auctionUserService.GetAuctionUserByFistNameAndLastName(auctionUser2.FirstName,
                auctionUser2.LastName);
            var bidPrice = new Price {Id = 2, Currency = "Euro", Value = 108.5};
            var bidPriceResult = this.priceService.AddPrice(bidPrice);
            var auctionById = this.auctionService.GetAuctionById(1);
            var bidId = 1;
            var bidDate = DateTime.Now;
            var bid = new Bid
            {
                Id = bidId, Auction = auctionById, BidUser = userBuyer, BidPrice = bidPrice, BidDate = bidDate
            };
            var resultBid = this.bidService.AddBid(bid);
            var deleteResult = this.bidService.DeleteBid(bidId + 1);
            Assert.True(this.libraryContextMock.Bids.Count() == 1);
        }
    }
}