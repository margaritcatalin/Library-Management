// <copyright file="AuctionTests.cs" company="Transilvania University of Brasov">
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
            this.auctionUserService = new AuctionUserService(new AuctionUserRepository(this.libraryContextMock),
                this.userReviewService);
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
    }
}