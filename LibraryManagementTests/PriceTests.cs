// <copyright file="PriceTests.cs" company="Transilvania University of Brasov">
// Margarit Marian Catalin
// </copyright>

namespace LibraryManagementTests
{
    using System.Configuration;
    using System.Linq;
    using LibraryManagement.BusinessLayer;
    using LibraryManagement.DataMapper;
    using LibraryManagement.DomainModel;
    using NUnit.Framework;
    using Telerik.JustMock.EntityFramework;

    /// <summary>
    /// The price unit tests.
    /// </summary>
    [TestFixture]
    public class PriceTests
    {
        /// <summary>
        /// Defines the libraryContextMock.
        /// </summary>
        private LibraryDbContext libraryContextMock;

        /// <summary>
        /// Defines the priceService.
        /// </summary>
        private PriceService priceService;

        /// <summary>
        /// Tests setup.
        /// </summary>
        [SetUp]
        public void SetUp()
        {
            this.libraryContextMock = EntityFrameworkMock.Create<LibraryDbContext>();
            EntityFrameworkMock.PrepareMock(this.libraryContextMock);
            this.priceService = new PriceService(new PriceRepository(this.libraryContextMock));
        }

        /// <summary>
        /// Test add a new price.
        /// </summary>
        [Test]
        public void TestAddPrice()
        {
            var price = new Price { Currency = "Euro", Value = 54.5 };
            var result = this.priceService.AddPrice(price);
            Assert.True(this.libraryContextMock.Prices.Count() == 1);
        }

        /// <summary>
        /// Test add a null price.
        /// </summary>
        [Test]
        public void TestAddNullPrice()
        {
            var result = this.priceService.AddPrice(null);
            Assert.True(!this.libraryContextMock.Prices.Any());
        }

        /// <summary>
        /// Test add an price with null currency.
        /// </summary>
        [Test]
        public void TestAddNullCurrencyPrice()
        {
            var price = new Price { Currency = null, Value = 65.5 };
            var result = this.priceService.AddPrice(price);
            Assert.True(!this.libraryContextMock.Prices.Any());
        }

        /// <summary>
        /// Test add add price with empty Currency.
        /// </summary>
        [Test]
        public void TestAddEmptyCurrencyPrice()
        {
            var price = new Price { Currency = string.Empty, Value = 4.5 };
            var result = this.priceService.AddPrice(price);
            Assert.True(!this.libraryContextMock.Prices.Any());
        }

        /// <summary>
        /// Test add price with smaller price value.
        /// </summary>
        [Test]
        public void TestAddLessThanZeroPrice()
        {
            var price = new Price { Currency = "Euro", Value = -5 };
            var result = this.priceService.AddPrice(price);
            Assert.True(!this.libraryContextMock.Prices.Any());
        }

        /// <summary>
        /// Test add price with zero price value.
        /// </summary>
        [Test]
        public void TestAddZeroValuePrice()
        {
            var price = new Price { Currency = "Euro", Value = 0 };
            var result = this.priceService.AddPrice(price);
            Assert.True(!this.libraryContextMock.Prices.Any());
        }

        /// <summary>
        /// Test add price with less than min price value.
        /// </summary>
        [Test]
        public void TestAddLessThanMinPrice()
        {
            var minPrice = double.Parse(ConfigurationManager.AppSettings["MIN_PRICE"]);
            var price = new Price { Currency = "Euro", Value = minPrice - 1 };
            var result = this.priceService.AddPrice(price);
            Assert.True(!this.libraryContextMock.Prices.Any());
        }

        /// <summary>
        /// Test get price by id and he is in the database.
        /// </summary>
        [Test]
        public void TestGetPriceByGoodId()
        {
            var price = new Price { Currency = "Euro", Value = 54.5 };
            var result = this.priceService.AddPrice(price);
            var priceById = this.priceService.GetPriceById(price.Id);
            Assert.NotNull(priceById);
        }

        /// <summary>
        /// Test get price by id and he is not in the database.
        /// </summary>
        [Test]
        public void TestGetPriceByBadId()
        {
            var price = new Price { Currency = "Euro", Value = 54.5 };
            var result = this.priceService.AddPrice(price);
            var prices = this.priceService.GetPrices();
            var priceId = prices.ToList()[0].Id;
            var priceById = this.priceService.GetPriceById(priceId + 1);
            Assert.Null(priceById);
        }

        /// <summary>
        /// Test get all prices.
        /// </summary>
        [Test]
        public void TestGetAllPrices()
        {
            var price = new Price { Currency = "Euro", Value = 54.5 };
            var price2 = new Price { Currency = "Euro", Value = 74.5 };
            var result = this.priceService.AddPrice(price);
            var result2 = this.priceService.AddPrice(price2);
            var prices = this.priceService.GetPrices();
            Assert.IsTrue(prices.Count() == 2);
        }

        /// <summary>
        /// Test get all prices with a wrong currency.
        /// </summary>
        [Test]
        public void TestGetAllPricesWithAWrongCurrency()
        {
            var price = new Price { Currency = "Euro", Value = 54.5 };
            var price2 = new Price { Currency = string.Empty, Value = 74.5 };
            var result = this.priceService.AddPrice(price);
            var result2 = this.priceService.AddPrice(price2);
            var prices = this.priceService.GetPrices();
            Assert.IsFalse(prices.Count() == 2);
        }

        /// <summary>
        /// Test delete Price.
        /// </summary>
        [Test]
        public void TestDeletePrice()
        {
            var price = new Price { Currency = "Euro", Value = 54.5 };
            var result = this.priceService.AddPrice(price);
            var prices = this.priceService.GetPrices();
            var priceId = prices.ToList()[0].Id;
            var deleteResult = this.priceService.DeletePrice(priceId);
            Assert.True(!this.libraryContextMock.Prices.Any());
        }

        /// <summary>
        /// Test delete Price.
        /// </summary>
        [Test]
        public void TestDeletePriceWithAWrongId()
        {
            var price = new Price { Currency = "Euro", Value = 54.5 };
            var result = this.priceService.AddPrice(price);
            var prices = this.priceService.GetPrices();
            var priceId = prices.ToList()[0].Id;
            var deleteResult = this.priceService.DeletePrice(priceId + 1);
            Assert.True(this.libraryContextMock.Prices.Count() == 1);
        }
    }
}