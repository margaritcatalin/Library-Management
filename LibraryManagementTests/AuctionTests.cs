// <copyright file="AuctionTests.cs" company="Transilvania University of Brasov">
// Margarit Marian Catalin
// </copyright>

using System;
using System.Collections.Generic;
using System.Configuration;
using LibraryManagement.DomainModel;
using LibraryManagement.DomainModel.Util;

namespace LibraryManagementTests
{
    using System.Linq;
    using NUnit.Framework;
    using LibraryManagement.BusinessLayer;
    using LibraryManagement.DataMapper;
    using Telerik.JustMock.EntityFramework;

    /// <summary>
    /// The auction unit tests.
    /// </summary>
    [TestFixture]
    public class AuctionTests
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
            this.productService = new ProductService(new ProductRepository(this.libraryContextMock));
            this.priceService = new PriceService(new PriceRepository(this.libraryContextMock));
            this.userReviewService = new UserReviewService(new UserReviewRepository(this.libraryContextMock));
            this.auctionUserService = new AuctionUserService(new AuctionUserRepository(this.libraryContextMock),this.userReviewService);
            this.categoryService = new CategoryService(new CategoryRepository(this.libraryContextMock));
            this.auctionService = new AuctionService(new AuctionRepository(this.libraryContextMock),
                this.categoryService, this.auctionUserService);
            this.bidService = new BidService(new BidRepository(this.libraryContextMock), this.auctionService);

        }

        /// <summary>
        /// Test add an auction.
        /// </summary>
        [Test]
        public void TestAddAuction()
        {
            var auctionUser = new AuctionUser {FirstName = "Ionel", LastName = "Pascu", Gender = "M"};
            var result = this.auctionUserService.AddAuctionUser(auctionUser, Role.Seller);
            var startPrice = new Price {Currency = "Euro", Value = 88.5};
            var priceResult = this.priceService.AddPrice(startPrice);
            var category = new Category {Name = "Legume"};
            var categoryResult = this.categoryService.AddCategory(category);
            var product = new Product {Name = "Fasole", Categories = new[] {category}};
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
            Assert.True(this.libraryContextMock.Auctions.Count() == 1);
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
            var startPrice = new Price {Currency = "Euro", Value = 88.5};
            var priceResult = this.priceService.AddPrice(startPrice);
            var category = new Category {Name = "Legume"};
            var categoryResult = this.categoryService.AddCategory(category);
            var product = new Product {Name = "Fasole", Categories = new[] {category}};
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
            var auctionUser = new AuctionUser {FirstName = "Ionel", LastName = "Pascu", Gender = "M"};
            var result = this.auctionUserService.AddAuctionUser(auctionUser, Role.Buyer);
            var startPrice = new Price {Currency = "Euro", Value = 88.5};
            var priceResult = this.priceService.AddPrice(startPrice);
            var category = new Category {Name = "Legume"};
            var categoryResult = this.categoryService.AddCategory(category);
            var product = new Product {Name = "Fasole", Categories = new[] {category}};
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
            var auctionUser = new AuctionUser {FirstName = "Ionel", LastName = "Pascu", Gender = "M"};
            var result = this.auctionUserService.AddAuctionUser(auctionUser, Role.Buyer);
            var startPrice = new Price {Currency = "Euro", Value = 88.5};
            var priceResult = this.priceService.AddPrice(startPrice);
            var category = new Category {Name = "Legume"};
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
        public void TestAddWithNullStartPriceAuction()
        {
            var auctionUser = new AuctionUser {FirstName = "Ionel", LastName = "Pascu", Gender = "M"};
            var result = this.auctionUserService.AddAuctionUser(auctionUser, Role.Buyer);
            var category = new Category {Name = "Legume"};
            var minPrice = double.Parse(ConfigurationManager.AppSettings["MIN_PRICE"]);
            var startPrice = new Price {Currency = "Euro", Value = minPrice - 1};
            var priceResult = this.priceService.AddPrice(startPrice);
            var categoryResult = this.categoryService.AddCategory(category);
            var product = new Product {Name = "Fasole", Categories = new[] {category}};
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
        public void TestAddWithStartPriceLessThanMinPriceAuction()
        {
            var auctionUser = new AuctionUser {FirstName = "Ionel", LastName = "Pascu", Gender = "M"};
            var result = this.auctionUserService.AddAuctionUser(auctionUser, Role.Buyer);
            var category = new Category {Name = "Legume"};
            var categoryResult = this.categoryService.AddCategory(category);
            var product = new Product {Name = "Fasole", Categories = new[] {category}};
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
            var auctionUser = new AuctionUser {FirstName = "Ionel", LastName = "Pascu", Gender = "M"};
            var result = this.auctionUserService.AddAuctionUser(auctionUser, Role.Seller);
            var startPrice = new Price {Currency = "Euro", Value = 88.5};
            var priceResult = this.priceService.AddPrice(startPrice);
            var category = new Category {Name = "Legume"};
            var categoryResult = this.categoryService.AddCategory(category);
            var product = new Product {Name = "Fasole", Categories = new[] {category}};
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
        /// Test add auction wih wrong end date.
        /// </summary>
        [Test]
        public void TestAddWithStartDateSameWithEndDateAuction()
        {
            var auctionUser = new AuctionUser {FirstName = "Ionel", LastName = "Pascu", Gender = "M"};
            var result = this.auctionUserService.AddAuctionUser(auctionUser, Role.Seller);
            var startPrice = new Price {Currency = "Euro", Value = 88.5};
            var priceResult = this.priceService.AddPrice(startPrice);
            var category = new Category {Name = "Legume"};
            var categoryResult = this.categoryService.AddCategory(category);
            var product = new Product {Name = "Fasole", Categories = new[] {category}};
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
            var auctionUser = new AuctionUser {FirstName = "Ionel", LastName = "Pascu", Gender = "M"};
            var result = this.auctionUserService.AddAuctionUser(auctionUser, Role.Seller);
            var startPrice = new Price {Currency = "Euro", Value = 88.5};
            var priceResult = this.priceService.AddPrice(startPrice);
            var category = new Category {Name = "Legume"};
            var categoryResult = this.categoryService.AddCategory(category);
            var product = new Product {Name = "Fasole", Categories = new[] {category}};
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
        /// Test add auction wih wrong end date.
        /// </summary>
        [Test]
        public void TestAddWithEndDateMoreThanMaxMonthsAuction()
        {
            var auctionUser = new AuctionUser {FirstName = "Ionel", LastName = "Pascu", Gender = "M"};
            var result = this.auctionUserService.AddAuctionUser(auctionUser, Role.Seller);
            var startPrice = new Price {Currency = "Euro", Value = 88.5};
            var priceResult = this.priceService.AddPrice(startPrice);
            var category = new Category {Name = "Legume"};
            var categoryResult = this.categoryService.AddCategory(category);
            var product = new Product {Name = "Fasole", Categories = new[] {category}};
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
        /// Test add auction with a user how has less than min score.
        /// </summary>
        [Test]
        public void TestAddAuctionWithAnBlockedUser()
        {
            var auctionUser = new AuctionUser {FirstName = "Ionel", LastName = "Pascu", Gender = "M"};
            var result = this.auctionUserService.AddAuctionUser(auctionUser, Role.Seller);
            var startPrice = new Price {Currency = "Euro", Value = 88.5};
            var priceResult = this.priceService.AddPrice(startPrice);
            var category = new Category {Name = "Legume"};
            var categoryResult = this.categoryService.AddCategory(category);
            var product = new Product {Name = "Fasole", Categories = new[] {category}};
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

            var reviewUser = new AuctionUser {Id = 2, FirstName = "Anghel", LastName = "Pascu", Gender = "M"};
            var resultReviewUser = this.auctionUserService.AddAuctionUser(reviewUser, Role.Buyer);
            var userReview = new UserReview
                {ReviewByUser = reviewUser, ReviewForUser = auctionUser, Score = 2, Description = "This is a test."};
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
            Assert.True(this.libraryContextMock.Auctions.Count() == 1);
        }

        /// <summary>
        /// Test add auction wih wrong end date.
        /// </summary>
        [Test]
        public void TestAddMoreThanMaxNumberOfAuctionInSameCategory()
        {
            var auctionUser = new AuctionUser {FirstName = "Ionel", LastName = "Pascu", Gender = "M"};
            var result = this.auctionUserService.AddAuctionUser(auctionUser, Role.Seller);
            var startPrice = new Price {Currency = "Euro", Value = 88.5};
            var priceResult = this.priceService.AddPrice(startPrice);
            var category = new Category {Name = "Legume"};
            var categoryResult = this.categoryService.AddCategory(category);
            var product = new Product {Name = "Fasole", Categories = new[] {category}};
            var productResult = this.productService.AddProduct(product);
            var maxNumberOfMonths = int.Parse(ConfigurationManager.AppSettings["MAX_NUMBER_OF_MONTHS"]);
            var startDate = DateTime.Now;
            var maxNumberOfAuctions = int.Parse(ConfigurationManager.AppSettings["MAX_NUMBER_OF_AUCTION_IN_CATEGORY"]);
            for (var i = 0; i <= maxNumberOfAuctions + 1; i++)
            {
                var auction = new Auction
                {
                    Id = i,
                    Auctioneer = auctionUser,
                    Product = product,
                    StartPrice = startPrice,
                    StartDate = startDate,
                    EndDate = startDate.AddDays(6),
                    Ended = false,
                };
                this.auctionService.AddAuction(auction);
            }

            Assert.True(this.libraryContextMock.Auctions.Count() == maxNumberOfAuctions);
        }

        /// <summary>
        /// Test add auction wih wrong end date.
        /// </summary>
        [Test]
        public void TestAddMoreThanMaxNumberOfAuctionInSameRoot()
        {
            var auctionUser = new AuctionUser {FirstName = "Ionel", LastName = "Pascu", Gender = "M"};
            var result = this.auctionUserService.AddAuctionUser(auctionUser, Role.Seller);
            var startPrice = new Price {Currency = "Euro", Value = 88.5};
            var priceResult = this.priceService.AddPrice(startPrice);
            var rootCategory = new Category {Name = "Legume"};
            var rootCategoryResult = this.categoryService.AddCategory(rootCategory);
            var maxNumberOfMonths = int.Parse(ConfigurationManager.AppSettings["MAX_NUMBER_OF_MONTHS"]);
            var startDate = DateTime.Now;
            var maxNumberOfAuctions = int.Parse(ConfigurationManager.AppSettings["MAX_NUMBER_OF_AUCTION_IN_CATEGORY"]);
            for (var i = 0; i <= maxNumberOfAuctions + 1; i++)
            {
                var category = new Category {Id = i, Name = "Legume", ParentCategory = rootCategory};
                var categoryResult = this.categoryService.AddCategory(category);
                var product = new Product {Id = i, Name = "Fasole", Categories = new[] {category}};
                var productResult = this.productService.AddProduct(product);
                var auction = new Auction
                {
                    Id = i,
                    Auctioneer = auctionUser,
                    Product = product,
                    StartPrice = startPrice,
                    StartDate = startDate,
                    EndDate = startDate.AddDays(6),
                    Ended = false,
                };
                this.auctionService.AddAuction(auction);
            }

            Assert.True(this.libraryContextMock.Auctions.Count() == maxNumberOfAuctions);
        }

        /// <summary>
        /// Test add auction wih wrong end date.
        /// </summary>
        [Test]
        public void TestAddMoreThanMaxNumberOfAuctionInMoreCategories()
        {
            var auctionUser = new AuctionUser {FirstName = "Ionel", LastName = "Pascu", Gender = "M"};
            var result = this.auctionUserService.AddAuctionUser(auctionUser, Role.Seller);
            var startPrice = new Price {Currency = "Euro", Value = 88.5};
            var priceResult = this.priceService.AddPrice(startPrice);
            var maxNumberOfMonths = int.Parse(ConfigurationManager.AppSettings["MAX_NUMBER_OF_MONTHS"]);
            var startDate = DateTime.Now;
            var maxNumberOfAuctions = int.Parse(ConfigurationManager.AppSettings["MAX_NUMBER_OF_AUCTION"]);
            for (var i = 0; i <= maxNumberOfAuctions + 1; i++)
            {
                var category = new Category {Id = i, Name = "Legume" + i};
                var categoryResult = this.categoryService.AddCategory(category);
                var product = new Product {Id = i, Name = "Fasole", Categories = new[] {category}};
                var productResult = this.productService.AddProduct(product);
                var auction = new Auction
                {
                    Id = i,
                    Auctioneer = auctionUser,
                    Product = product,
                    StartPrice = startPrice,
                    StartDate = startDate,
                    EndDate = startDate.AddDays(6),
                    Ended = false,
                };
                this.auctionService.AddAuction(auction);
            }

            Assert.True(this.libraryContextMock.Auctions.Count() == maxNumberOfAuctions);
        }

        /// <summary>
        /// Test Get Number of all started auction for user.
        /// </summary>
        [Test]
        public void TestGetStartedAuctionByUser()
        {
            var auctionUser = new AuctionUser {FirstName = "Ionel", LastName = "Pascu", Gender = "M"};
            var result = this.auctionUserService.AddAuctionUser(auctionUser, Role.Seller);
            var startPrice = new Price {Currency = "Euro", Value = 88.5};
            var priceResult = this.priceService.AddPrice(startPrice);
            var maxNumberOfMonths = int.Parse(ConfigurationManager.AppSettings["MAX_NUMBER_OF_MONTHS"]);
            var startDate = DateTime.Now;
            for (var i = 1; i <= 4; i++)
            {
                var category = new Category {Id = i, Name = "Legume" + i};
                var categoryResult = this.categoryService.AddCategory(category);
                var product = new Product {Id = i, Name = "Fasole", Categories = new[] {category}};
                var productResult = this.productService.AddProduct(product);
                var auction = new Auction
                {
                    Id = i,
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
        /// Test Get Number of all started auction for user.
        /// </summary>
        [Test]
        public void TestGetStartedAuctionByUserUnderEndOne()
        {
            var auctionUser = new AuctionUser {FirstName = "Ionel", LastName = "Pascu", Gender = "M"};
            var result = this.auctionUserService.AddAuctionUser(auctionUser, Role.Seller);
            var startPrice = new Price {Currency = "Euro", Value = 88.5};
            var priceResult = this.priceService.AddPrice(startPrice);
            var maxNumberOfMonths = int.Parse(ConfigurationManager.AppSettings["MAX_NUMBER_OF_MONTHS"]);
            var startDate = DateTime.Now;
            for (var i = 1; i <= 4; i++)
            {
                var category = new Category {Id = i, Name = "Legume" + i};
                var categoryResult = this.categoryService.AddCategory(category);
                var product = new Product {Id = i, Name = "Fasole", Categories = new[] {category}};
                var productResult = this.productService.AddProduct(product);
                var auction = new Auction
                {
                    Id = i,
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
        /// Test Get All started auction for user in a specific category.
        /// </summary>
        [Test]
        public void TestGetStartedAuctionByUserAndCategory()
        {
            var auctionUser = new AuctionUser {FirstName = "Ionel", LastName = "Pascu", Gender = "M"};
            var result = this.auctionUserService.AddAuctionUser(auctionUser, Role.Seller);
            var startPrice = new Price {Currency = "Euro", Value = 88.5};
            var priceResult = this.priceService.AddPrice(startPrice);
            var maxNumberOfMonths = int.Parse(ConfigurationManager.AppSettings["MAX_NUMBER_OF_MONTHS"]);
            var category = new Category {Name = "Legume"};
            var categoryResult = this.categoryService.AddCategory(category);
            var startDate = DateTime.Now;
            for (var i = 1; i <= 3; i++)
            {
                var product = new Product {Id = i, Name = "Fasole", Categories = new[] {category}};
                var productResult = this.productService.AddProduct(product);
                var auction = new Auction
                {
                    Id = i,
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
        /// Test Get All started auction for user in a specific category.
        /// </summary>
        [Test]
        public void TestGetStartedAuctionByUserAndCategoryUnderEndOne()
        {
            var auctionUser = new AuctionUser {FirstName = "Ionel", LastName = "Pascu", Gender = "M"};
            var result = this.auctionUserService.AddAuctionUser(auctionUser, Role.Seller);
            var startPrice = new Price {Currency = "Euro", Value = 88.5};
            var priceResult = this.priceService.AddPrice(startPrice);
            var maxNumberOfMonths = int.Parse(ConfigurationManager.AppSettings["MAX_NUMBER_OF_MONTHS"]);
            var startDate = DateTime.Now;
            var category = new Category {Name = "Legume"};
            var categoryResult = this.categoryService.AddCategory(category);
            for (var i = 1; i <= 3; i++)
            {
                var product = new Product {Id = i, Name = "Fasole", Categories = new[] {category}};
                var productResult = this.productService.AddProduct(product);
                var auction = new Auction
                {
                    Id = i,
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
        /// Test Get all started auctions.
        /// </summary>
        [Test]
        public void TestGetAllAuctions()
        {
            var auctionUser = new AuctionUser {FirstName = "Ionel", LastName = "Pascu", Gender = "M"};
            var result = this.auctionUserService.AddAuctionUser(auctionUser, Role.Seller);
            var startPrice = new Price {Currency = "Euro", Value = 88.5};
            var priceResult = this.priceService.AddPrice(startPrice);
            var maxNumberOfMonths = int.Parse(ConfigurationManager.AppSettings["MAX_NUMBER_OF_MONTHS"]);
            var startDate = DateTime.Now;
            for (var i = 1; i <= 4; i++)
            {
                var category = new Category {Id = i, Name = "Legume" + i};
                var categoryResult = this.categoryService.AddCategory(category);
                var product = new Product {Id = i, Name = "Fasole", Categories = new[] {category}};
                var productResult = this.productService.AddProduct(product);
                var auction = new Auction
                {
                    Id = i,
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
        /// Test Get Number of all started auction for user.
        /// </summary>
        [Test]
        public void TestGetAllAuctionsUnderEndOne()
        {
            var auctionUser = new AuctionUser {FirstName = "Ionel", LastName = "Pascu", Gender = "M"};
            var result = this.auctionUserService.AddAuctionUser(auctionUser, Role.Seller);
            var startPrice = new Price {Currency = "Euro", Value = 88.5};
            var priceResult = this.priceService.AddPrice(startPrice);
            var maxNumberOfMonths = int.Parse(ConfigurationManager.AppSettings["MAX_NUMBER_OF_MONTHS"]);
            var startDate = DateTime.Now;
            for (var i = 1; i <= 4; i++)
            {
                var category = new Category {Id = i, Name = "Legume" + i};
                var categoryResult = this.categoryService.AddCategory(category);
                var product = new Product {Id = i, Name = "Fasole", Categories = new[] {category}};
                var productResult = this.productService.AddProduct(product);
                var auction = new Auction
                {
                    Id = i,
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
        /// Test get auction by id and he is in the db.
        /// </summary>
        [Test]
        public void TestGetAuctionByGoodId()
        {
            var auctionId = 0;
            var auctionUser = new AuctionUser {FirstName = "Ionel", LastName = "Pascu", Gender = "M"};
            var result = this.auctionUserService.AddAuctionUser(auctionUser, Role.Seller);
            var startPrice = new Price {Currency = "Euro", Value = 88.5};
            var priceResult = this.priceService.AddPrice(startPrice);
            var category = new Category {Name = "Legume"};
            var categoryResult = this.categoryService.AddCategory(category);
            var product = new Product {Name = "Fasole", Categories = new[] {category}};
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
            var auctionUserById = this.auctionService.GetAuctionById(auctionId);
            Assert.NotNull(auctionUserById);
        }

        /// <summary>
        /// Test get auctionUser by id and he is not in the db.
        /// </summary>
        [Test]
        public void TestGetAuctionByBadId()
        {
            var auctionId = 0;
            var auctionUser = new AuctionUser {FirstName = "Ionel", LastName = "Pascu", Gender = "M"};
            var result = this.auctionUserService.AddAuctionUser(auctionUser, Role.Seller);
            var startPrice = new Price {Currency = "Euro", Value = 88.5};
            var priceResult = this.priceService.AddPrice(startPrice);
            var category = new Category {Name = "Legume"};
            var categoryResult = this.categoryService.AddCategory(category);
            var product = new Product {Name = "Fasole", Categories = new[] {category}};
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
            var auctionUserById = this.auctionService.GetAuctionById(auctionId + 1);
            Assert.Null(auctionUserById);
        }

        /// <summary>
        /// Test Update auction user.
        /// </summary>
        [Test]
        public void TestUpdateAuctionUser()
        {
            var auctionId = 0;
            var auctionUser = new AuctionUser {FirstName = "Ionel", LastName = "Pascu", Gender = "M"};
            var result2 = this.auctionUserService.AddAuctionUser(auctionUser, Role.Seller);
            var startPrice = new Price {Currency = "Euro", Value = 88.5};
            var priceResult = this.priceService.AddPrice(startPrice);
            var category = new Category {Name = "Legume"};
            var categoryResult = this.categoryService.AddCategory(category);
            var product = new Product {Name = "Fasole", Categories = new[] {category}};
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
            var auctionUserById = this.auctionService.GetAuctionById(auctionId);
            var auctionUser2 = new AuctionUser {Id = 2, FirstName = "Marcu", LastName = "Andrei", Gender = "M"};
            var resultUser2 = this.auctionUserService.AddAuctionUser(auctionUser2, Role.Seller);
            auctionUserById.Auctioneer = auctionUser2;
            this.auctionService.UpdateAuction(auctionUserById);
            auctionUserById = this.auctionService.GetAuctionById(auctionId);
            Assert.IsTrue(auctionUserById.Auctioneer.Id == auctionUser2.Id);
        }

        /// <summary>
        /// Test Update auction with bad user.
        /// </summary>
        [Test]
        public void TestUpdateAuctionBadUser()
        {
            var auctionId = 0;
            var auctionUser = new AuctionUser {FirstName = "Ionel", LastName = "Pascu", Gender = "M"};
            var result2 = this.auctionUserService.AddAuctionUser(auctionUser, Role.Seller);
            var startPrice = new Price {Currency = "Euro", Value = 88.5};
            var priceResult = this.priceService.AddPrice(startPrice);
            var category = new Category {Name = "Legume"};
            var categoryResult = this.categoryService.AddCategory(category);
            var product = new Product {Name = "Fasole", Categories = new[] {category}};
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
            var auctionUserById = this.auctionService.GetAuctionById(auctionId);
            var result = this.auctionUserService.AddAuctionUser(auctionUser, Role.Seller);
            var auctionUser2 = new AuctionUser {Id = 2, FirstName = "Marcu", LastName = "Andrei", Gender = "M"};
            var resultUser2 = this.auctionUserService.AddAuctionUser(auctionUser2, Role.Buyer);
            auctionUserById.Auctioneer = auctionUser2;
            this.auctionService.UpdateAuction(auctionUserById);
            auctionUserById = this.auctionService.GetAuctionById(auctionId);
            Assert.IsFalse(auctionUserById.Auctioneer.Id == auctionUser2.Id);
        }

        /// <summary>
        /// Test Update auction product.
        /// </summary>
        [Test]
        public void TestUpdateAuctionProduct()
        {
            var auctionId = 0;
            var auctionUser = new AuctionUser {FirstName = "Ionel", LastName = "Pascu", Gender = "M"};
            var result2 = this.auctionUserService.AddAuctionUser(auctionUser, Role.Seller);
            var startPrice = new Price {Currency = "Euro", Value = 88.5};
            var priceResult = this.priceService.AddPrice(startPrice);
            var category = new Category {Name = "Legume"};
            var categoryResult = this.categoryService.AddCategory(category);
            var product = new Product {Name = "Fasole", Categories = new[] {category}};
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
            var auctionUserById = this.auctionService.GetAuctionById(auctionId);
            var product2 = new Product {Id = 3, Name = "Macaroane", Categories = new[] {category}};
            var productResult2 = this.productService.AddProduct(product2);
            auctionUserById.Product = product2;
            this.auctionService.UpdateAuction(auctionUserById);
            auctionUserById = this.auctionService.GetAuctionById(auctionId);
            Assert.IsTrue(auctionUserById.Product.Id == product2.Id);
        }

        /// <summary>
        /// Test Update auction bad product.
        /// </summary>
        [Test]
        public void TestUpdateAuctionBadProduct()
        {
            var auctionId = 0;
            var auctionUser = new AuctionUser {FirstName = "Ionel", LastName = "Pascu", Gender = "M"};
            var result2 = this.auctionUserService.AddAuctionUser(auctionUser, Role.Seller);
            var startPrice = new Price {Currency = "Euro", Value = 88.5};
            var priceResult = this.priceService.AddPrice(startPrice);
            var category = new Category {Name = "Legume"};
            var categoryResult = this.categoryService.AddCategory(category);
            var product = new Product {Name = "Fasole", Categories = new[] {category}};
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
            var auctionUserById = this.auctionService.GetAuctionById(auctionId);
            var product2 = new Product {Id = 3, Name = string.Empty, Categories = new[] {category}};
            var productResult2 = this.productService.AddProduct(product2);
            auctionUserById.Product = product2;
            this.auctionService.UpdateAuction(auctionUserById);
            auctionUserById = this.auctionService.GetAuctionById(auctionId);
            Assert.IsFalse(auctionUserById.Auctioneer.Id == product2.Id);
        }

        /// <summary>
        /// Test Update auction startPrice.
        /// </summary>
        [Test]
        public void TestUpdateAuctionStartPrice()
        {
            var auctionId = 0;
            var auctionUser = new AuctionUser {FirstName = "Ionel", LastName = "Pascu", Gender = "M"};
            var result2 = this.auctionUserService.AddAuctionUser(auctionUser, Role.Seller);
            var startPrice = new Price {Currency = "Euro", Value = 88.5};
            var priceResult = this.priceService.AddPrice(startPrice);
            var category = new Category {Name = "Legume"};
            var categoryResult = this.categoryService.AddCategory(category);
            var product = new Product {Name = "Fasole", Categories = new[] {category}};
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
            var auctionUserById = this.auctionService.GetAuctionById(auctionId);
            var startPrice2 = new Price {Id = 5, Currency = "Euro", Value = 444.5};
            var priceResult2 = this.priceService.AddPrice(startPrice2);
            auctionUserById.StartPrice = startPrice2;
            this.auctionService.UpdateAuction(auctionUserById);
            auctionUserById = this.auctionService.GetAuctionById(auctionId);
            Assert.IsTrue(auctionUserById.Auctioneer.Id == startPrice2.Id);
        }

        /// <summary>
        /// Test Update auction startPrice until bid.
        /// </summary>
        [Test]
        public void TestUpdateAuctionStartPriceUnderBid()
        {
            var auctionId = 0;
            var auctionUser = new AuctionUser {FirstName = "Ionel", LastName = "Pascu", Gender = "M"};
            var result2 = this.auctionUserService.AddAuctionUser(auctionUser, Role.Seller);
            var startPrice = new Price {Currency = "Euro", Value = 88.5};
            var priceResult = this.priceService.AddPrice(startPrice);
            var category = new Category {Name = "Legume"};
            var categoryResult = this.categoryService.AddCategory(category);
            var product = new Product {Name = "Fasole", Categories = new[] {category}};
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
            var auctionUserById = this.auctionService.GetAuctionById(auctionId);
            var auctionUser2 = new AuctionUser {Id = 33, FirstName = "Ioana", LastName = "Pascu", Gender = "F"};
            var resultAuctionUser2 = this.auctionUserService.AddAuctionUser(auctionUser, Role.Buyer);
            var userBuyer = this.auctionUserService.GetAuctionUserByFistNameAndLastName(auctionUser2.FirstName,
                auctionUser2.LastName);
            var bidPrice = new Price {Id = 2, Currency = "Euro", Value = 108.5};
            var bidPriceResult = this.priceService.AddPrice(bidPrice);
            var bid = new Bid {Auction = auction, BidUser = userBuyer, BidPrice = bidPrice, BidDate = DateTime.Now};
            var resultBid = this.bidService.AddBid(bid);
            var startPrice2 = new Price {Id = 5, Currency = "Dolar", Value = 444.5};
            var priceResult2 = this.priceService.AddPrice(startPrice2);
            auctionUserById.StartPrice = startPrice2;
            this.auctionService.UpdateAuction(auctionUserById);
            auctionUserById = this.auctionService.GetAuctionById(auctionId);
            Assert.IsFalse(auctionUserById.Auctioneer.Id == startPrice2.Id);
        }

        /// <summary>
        /// Test Update auction start date.
        /// </summary>
        [Test]
        public void TestUpdateAuctionStartDate()
        {
            var auctionId = 0;
            var auctionUser = new AuctionUser {FirstName = "Ionel", LastName = "Pascu", Gender = "M"};
            var result2 = this.auctionUserService.AddAuctionUser(auctionUser, Role.Seller);
            var startPrice = new Price {Currency = "Euro", Value = 88.5};
            var priceResult = this.priceService.AddPrice(startPrice);
            var category = new Category {Name = "Legume"};
            var categoryResult = this.categoryService.AddCategory(category);
            var product = new Product {Name = "Fasole", Categories = new[] {category}};
            var productResult = this.productService.AddProduct(product);
            var startDate = DateTime.Now;
            var auction = new Auction
            {
                Id = auctionId,
                Auctioneer = auctionUser,
                Product = product,
                StartPrice = startPrice,
                StartDate = startDate,
                EndDate = DateTime.Now.AddDays(5),
                Ended = false,
            };
            var auctionResult = this.auctionService.AddAuction(auction);
            var auctionUserById = this.auctionService.GetAuctionById(auctionId);
            auctionUserById.StartDate = DateTime.Now.AddDays(2);
            this.auctionService.UpdateAuction(auctionUserById);
            auctionUserById = this.auctionService.GetAuctionById(auctionId);
            Assert.IsFalse(DateTime.Compare(auctionUserById.StartDate, startDate) == 0);
        }

        /// <summary>
        /// Test Update auction bad start date.
        /// </summary>
        [Test]
        public void TestUpdateAuctionBadStartDate()
        {
            var auctionId = 0;
            var auctionUser = new AuctionUser {FirstName = "Ionel", LastName = "Pascu", Gender = "M"};
            var result2 = this.auctionUserService.AddAuctionUser(auctionUser, Role.Seller);
            var startPrice = new Price {Currency = "Euro", Value = 88.5};
            var priceResult = this.priceService.AddPrice(startPrice);
            var category = new Category {Name = "Legume"};
            var categoryResult = this.categoryService.AddCategory(category);
            var product = new Product {Name = "Fasole", Categories = new[] {category}};
            var productResult = this.productService.AddProduct(product);
            var startDate = DateTime.Now;
            var auction = new Auction
            {
                Id = auctionId,
                Auctioneer = auctionUser,
                Product = product,
                StartPrice = startPrice,
                StartDate = startDate,
                EndDate = DateTime.Now.AddDays(5),
                Ended = false,
            };
            var auctionResult = this.auctionService.AddAuction(auction);
            var auctionUserById = this.auctionService.GetAuctionById(auctionId);
            auctionUserById.StartDate = DateTime.Now.AddDays(7);
            this.auctionService.UpdateAuction(auctionUserById);
            auctionUserById = this.auctionService.GetAuctionById(auctionId);
            Assert.IsTrue(DateTime.Compare(auctionUserById.StartDate, startDate) == 0);
        }

        /// <summary>
        /// Test Update auction end date.
        /// </summary>
        [Test]
        public void TestUpdateAuctionEbdDate()
        {
            var auctionId = 0;
            var auctionUser = new AuctionUser {FirstName = "Ionel", LastName = "Pascu", Gender = "M"};
            var result2 = this.auctionUserService.AddAuctionUser(auctionUser, Role.Seller);
            var startPrice = new Price {Currency = "Euro", Value = 88.5};
            var priceResult = this.priceService.AddPrice(startPrice);
            var category = new Category {Name = "Legume"};
            var categoryResult = this.categoryService.AddCategory(category);
            var product = new Product {Name = "Fasole", Categories = new[] {category}};
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
            var auctionUserById = this.auctionService.GetAuctionById(auctionId);
            auctionUserById.EndDate = DateTime.Now.AddDays(2);
            this.auctionService.UpdateAuction(auctionUserById);
            auctionUserById = this.auctionService.GetAuctionById(auctionId);
            Assert.IsFalse(DateTime.Compare(auctionUserById.EndDate, endDate) == 0);
        }

        /// <summary>
        /// Test Update auction bad start date.
        /// </summary>
        [Test]
        public void TestUpdateAuctionBadEndDate()
        {
            var auctionId = 0;
            var auctionUser = new AuctionUser {FirstName = "Ionel", LastName = "Pascu", Gender = "M"};
            var result2 = this.auctionUserService.AddAuctionUser(auctionUser, Role.Seller);
            var startPrice = new Price {Currency = "Euro", Value = 88.5};
            var priceResult = this.priceService.AddPrice(startPrice);
            var category = new Category {Name = "Legume"};
            var categoryResult = this.categoryService.AddCategory(category);
            var product = new Product {Name = "Fasole", Categories = new[] {category}};
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
            var auctionUserById = this.auctionService.GetAuctionById(auctionId);
            auctionUserById.EndDate = startDate;
            this.auctionService.UpdateAuction(auctionUserById);
            auctionUserById = this.auctionService.GetAuctionById(auctionId);
            Assert.IsTrue(DateTime.Compare(auctionUserById.EndDate, endDate) == 0);
        }

        /// <summary>
        /// Test delete auction.
        /// </summary>
        [Test]
        public void TestDeleteAuction()
        {
            var auctionId = 0;
            var auctionUser = new AuctionUser {FirstName = "Ionel", LastName = "Pascu", Gender = "M"};
            var result2 = this.auctionUserService.AddAuctionUser(auctionUser, Role.Seller);
            var startPrice = new Price {Currency = "Euro", Value = 88.5};
            var priceResult = this.priceService.AddPrice(startPrice);
            var category = new Category {Name = "Legume"};
            var categoryResult = this.categoryService.AddCategory(category);
            var product = new Product {Name = "Fasole", Categories = new[] {category}};
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
            var deleteResult = this.auctionService.DeleteAuction(auctionId);
            Assert.True(!this.libraryContextMock.Auctions.Any());
        }

        /// <summary>
        /// Test delete auction if it is not in the db.
        /// </summary>
        [Test]
        public void TestDeleteAuctionWithAWrongId()
        {
            var auctionId = 0;
            var auctionUser = new AuctionUser {FirstName = "Ionel", LastName = "Pascu", Gender = "M"};
            var result2 = this.auctionUserService.AddAuctionUser(auctionUser, Role.Seller);
            var startPrice = new Price {Currency = "Euro", Value = 88.5};
            var priceResult = this.priceService.AddPrice(startPrice);
            var category = new Category {Name = "Legume"};
            var categoryResult = this.categoryService.AddCategory(category);
            var product = new Product {Name = "Fasole", Categories = new[] {category}};
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
            var deleteResult = this.auctionService.DeleteAuction(auctionId + 1);
            Assert.True(this.libraryContextMock.Auctions.Count() == 1);
        }

        /// <summary>
        /// Test check if auction is not ended.
        /// </summary>
        [Test]
        public void TestCheckIfAuctionIsNotEnded()
        {
            var auctionId = 0;
            var auctionUser = new AuctionUser {FirstName = "Ionel", LastName = "Pascu", Gender = "M"};
            var result2 = this.auctionUserService.AddAuctionUser(auctionUser, Role.Seller);
            var startPrice = new Price {Currency = "Euro", Value = 88.5};
            var priceResult = this.priceService.AddPrice(startPrice);
            var category = new Category {Name = "Legume"};
            var categoryResult = this.categoryService.AddCategory(category);
            var product = new Product {Name = "Fasole", Categories = new[] {category}};
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
            var isEnded = this.auctionService.CheckIfAuctionIsEnded(auction);
            Assert.IsFalse(isEnded);
        }

        /// <summary>
        /// Test Get last auction for an user.
        /// </summary>
        [Test]
        public void TestGetLastAuctionForUser()
        {
            var auctionId = 0;
            var auctionUser = new AuctionUser {FirstName = "Ionel", LastName = "Pascu", Gender = "M"};
            var result2 = this.auctionUserService.AddAuctionUser(auctionUser, Role.Seller);
            var startPrice = new Price {Currency = "Euro", Value = 88.5};
            var priceResult = this.priceService.AddPrice(startPrice);
            var category = new Category {Name = "Legume"};
            var categoryResult = this.categoryService.AddCategory(category);
            var product = new Product {Name = "Fasole", Categories = new[] {category}};
            var productResult = this.productService.AddProduct(product);
            var startDate = DateTime.Now;
            var endDate = DateTime.Now.AddDays(3);
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

            var auctionId2 = 2;
            var startPrice2 = new Price {Currency = "Euro", Value = 848.5};
            var priceResult2 = this.priceService.AddPrice(startPrice2);
            var category2 = new Category {Name = "Mere"};
            var categoryResult2 = this.categoryService.AddCategory(category2);
            var product2 = new Product {Name = "Caise", Categories = new[] {category2}};
            var productResult2 = this.productService.AddProduct(product2);
            var startDate2 = DateTime.Now;
            var endDate2 = DateTime.Now.AddDays(4);
            var auction2 = new Auction
            {
                Id = auctionId2,
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
        /// Test get last auctions with one of it ended.
        /// </summary>
        [Test]
        public void TestGetLastAuctionForUserWithEndedOne()
        {
            var auctionId = 0;
            var auctionUser = new AuctionUser {FirstName = "Ionel", LastName = "Pascu", Gender = "M"};
            var result2 = this.auctionUserService.AddAuctionUser(auctionUser, Role.Seller);
            var startPrice = new Price {Currency = "Euro", Value = 88.5};
            var priceResult = this.priceService.AddPrice(startPrice);
            var category = new Category {Name = "Legume"};
            var categoryResult = this.categoryService.AddCategory(category);
            var product = new Product {Name = "Fasole", Categories = new[] {category}};
            var productResult = this.productService.AddProduct(product);
            var startDate = DateTime.Now;
            var endDate = DateTime.Now.AddDays(3);
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

            var auctionId2 = 2;
            var startPrice2 = new Price {Currency = "Euro", Value = 848.5};
            var priceResult2 = this.priceService.AddPrice(startPrice2);
            var category2 = new Category {Name = "Mere"};
            var categoryResult2 = this.categoryService.AddCategory(category2);
            var product2 = new Product {Name = "Caise", Categories = new[] {category2}};
            var productResult2 = this.productService.AddProduct(product2);
            var startDate2 = DateTime.Now;
            var endDate2 = DateTime.Now.AddDays(4);
            var auction2 = new Auction
            {
                Id = auctionId2,
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
            Assert.IsTrue(lastAuction.Id == auction2.Id);
        }

        /// <summary>
        /// Test Get last auction without auctions.
        /// </summary>
        [Test]
        public void TestGetLastAuctionForUserWithoutAuctions()
        {
            var auctionUser = new AuctionUser {FirstName = "Ionel", LastName = "Pascu", Gender = "M"};
            var result2 = this.auctionUserService.AddAuctionUser(auctionUser, Role.Seller);
            var lastAuction = this.auctionService.GetLastAuctionForUser(auctionUser);
            Assert.IsNull(lastAuction);
        }

        /// <summary>
        /// Test check if auction is ended.
        /// </summary>
        [Test]
        public void TestCheckIfAuctionIsEnded()
        {
            var auctionId = 0;
            var auctionUser = new AuctionUser {FirstName = "Ionel", LastName = "Pascu", Gender = "M"};
            var result2 = this.auctionUserService.AddAuctionUser(auctionUser, Role.Seller);
            var startPrice = new Price {Currency = "Euro", Value = 88.5};
            var priceResult = this.priceService.AddPrice(startPrice);
            var category = new Category {Name = "Legume"};
            var categoryResult = this.categoryService.AddCategory(category);
            var product = new Product {Name = "Fasole", Categories = new[] {category}};
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
            var auctionUserSaved = this.auctionUserService.GetAuctionUserByFistNameAndLastName(auctionUser.FirstName,auctionUser.LastName);
            var endResult = this.auctionService.EndAuctionByUser(auction, auctionUser);
            var isEnded = this.auctionService.CheckIfAuctionIsEnded(auction);
            Assert.IsTrue(isEnded);
        }

        /// <summary>
        /// Test Get Auction last price
        /// </summary>
        [Test]
        public void TestGetAuctionLastPriceWithBids()
        {
            var auctionId = 0;
            var auctionUser = new AuctionUser {FirstName = "Ionel", LastName = "Pascu", Gender = "M"};
            var result2 = this.auctionUserService.AddAuctionUser(auctionUser, Role.Seller);
            var startPrice = new Price {Currency = "Euro", Value = 88.5};
            var priceResult = this.priceService.AddPrice(startPrice);
            var category = new Category {Name = "Legume"};
            var categoryResult = this.categoryService.AddCategory(category);
            var product = new Product {Name = "Fasole", Categories = new[] {category}};
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
            var auctionUserById = this.auctionService.GetAuctionById(auctionId);
            var auctionUser2 = new AuctionUser {Id = 33, FirstName = "Ioana", LastName = "Pascu", Gender = "F"};
            var resultAuctionUser2 = this.auctionUserService.AddAuctionUser(auctionUser, Role.Buyer);
            var reviewUser = this.auctionUserService.GetAuctionUserByFistNameAndLastName("Ioana", "Pascu");

            var bidPrice = new Price {Id = 2, Currency = "Euro", Value = 108.5};
            var bidPriceResult = this.priceService.AddPrice(bidPrice);
            var bid = new Bid {Auction = auction, BidUser = reviewUser, BidPrice = bidPrice, BidDate = DateTime.Now};
            var resultBid = this.bidService.AddBid(bid);
            auctionUserById = this.auctionService.GetAuctionById(auctionId);
            var lastPrice = this.auctionService.GetAuctionLastPrice(auctionUserById);
            Assert.IsTrue(lastPrice.Id == bidPrice.Id);
        }
        
        /// <summary>
        /// Test Get Auction last price without bids
        /// </summary>
        [Test]
        public void TestGetAuctionLastPriceNoBids()
        {
            var auctionId = 0;
            var auctionUser = new AuctionUser {FirstName = "Ionel", LastName = "Pascu", Gender = "M"};
            var result2 = this.auctionUserService.AddAuctionUser(auctionUser, Role.Seller);
            var startPrice = new Price {Currency = "Euro", Value = 88.5};
            var priceResult = this.priceService.AddPrice(startPrice);
            var category = new Category {Name = "Legume"};
            var categoryResult = this.categoryService.AddCategory(category);
            var product = new Product {Name = "Fasole", Categories = new[] {category}};
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
            var auctionUserById = this.auctionService.GetAuctionById(auctionId);
            var lastPrice = this.auctionService.GetAuctionLastPrice(auctionUserById);
            Assert.IsTrue(lastPrice.Id == startPrice.Id);
        }
        
        /// <summary>
        /// Test End auction by user
        /// </summary>
        [Test]
        public void TestEndAuctionByUser()
        {
            var auctionId = 0;
            var auctionUser = new AuctionUser {FirstName = "Ionel", LastName = "Pascu", Gender = "M"};
            var result2 = this.auctionUserService.AddAuctionUser(auctionUser, Role.Seller);
            auctionUser = this.auctionUserService.GetAuctionUserByFistNameAndLastName(auctionUser.FirstName,auctionUser.LastName);
            var startPrice = new Price {Currency = "Euro", Value = 88.5};
            var priceResult = this.priceService.AddPrice(startPrice);
            var category = new Category {Name = "Legume"};
            var categoryResult = this.categoryService.AddCategory(category);
            var product = new Product {Name = "Fasole", Categories = new[] {category}};
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
            var isEnded = this.auctionService.EndAuctionByUser(auctionById,auctionUser);
            auctionById = this.auctionService.GetAuctionById(auctionId);
            Assert.IsTrue(auctionById.Ended);
        }
        
        /// <summary>
        /// Test End auction by user and it is already ended
        /// </summary>
        [Test]
        public void TestEndAuctionByUserAlreadyEnded()
        {
            var auctionId = 0;
            var auctionUser = new AuctionUser {FirstName = "Ionel", LastName = "Pascu", Gender = "M"};
            var result2 = this.auctionUserService.AddAuctionUser(auctionUser, Role.Seller);
            auctionUser = this.auctionUserService.GetAuctionUserByFistNameAndLastName(auctionUser.FirstName,auctionUser.LastName);
            var startPrice = new Price {Currency = "Euro", Value = 88.5};
            var priceResult = this.priceService.AddPrice(startPrice);
            var category = new Category {Name = "Legume"};
            var categoryResult = this.categoryService.AddCategory(category);
            var product = new Product {Name = "Fasole", Categories = new[] {category}};
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
            var isEnded = this.auctionService.EndAuctionByUser(auctionById,auctionUser);
            auctionById = this.auctionService.GetAuctionById(auctionId);
            var isEnded2 = this.auctionService.EndAuctionByUser(auctionById,auctionUser);
            Assert.IsFalse(isEnded2);
        }
        
        /// <summary>
        /// Test End auction by user
        /// </summary>
        [Test]
        public void TestEndAuctionByAnotherUser()
        {
            var auctionId = 0;
            var auctionUser = new AuctionUser {FirstName = "Ionel", LastName = "Pascu", Gender = "M"};
            var result2 = this.auctionUserService.AddAuctionUser(auctionUser, Role.Seller);
            auctionUser = this.auctionUserService.GetAuctionUserByFistNameAndLastName(auctionUser.FirstName,auctionUser.LastName);
            var startPrice = new Price {Currency = "Euro", Value = 88.5};
            var priceResult = this.priceService.AddPrice(startPrice);
            var category = new Category {Name = "Legume"};
            var categoryResult = this.categoryService.AddCategory(category);
            var product = new Product {Name = "Fasole", Categories = new[] {category}};
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
            var auctionUser2 = new AuctionUser {Id=2,FirstName = "Manuel", LastName = "Grigore", Gender = "M"};
            var resultU2 = this.auctionUserService.AddAuctionUser(auctionUser2, Role.Seller);
            auctionUser2 = this.auctionUserService.GetAuctionUserByFistNameAndLastName(auctionUser2.FirstName,auctionUser2.LastName);
            var isEnded = this.auctionService.EndAuctionByUser(auctionById,auctionUser2);
            auctionById = this.auctionService.GetAuctionById(auctionId);
            Assert.IsFalse(auctionById.Ended);
        }
    }
}