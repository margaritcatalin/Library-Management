// <copyright file="PriceTests.cs" company="Transilvania University of Brasov">
// Margarit Marian Catalin
// </copyright>

using System;
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
    /// The price unit tests.
    /// </summary>
    [TestFixture]
    public class PriceTests
    {
        private LibraryDbContext libraryContextMock;

        private PriceService priceService;

        /// <summary>
        /// Tests setup.
        /// </summary>
        [SetUp]
        public void SetUp()
        {
            this.libraryContextMock = EntityFrameworkMock.Create<LibraryDbContext>();
            this.priceService = new PriceService(new PriceRepository(this.libraryContextMock));
        }

        /// <summary>
        /// Test add a new price.
        /// </summary>
        [Test]
        public void TestAddPrice()
        {
            var price = new Price {Currency = "Euro", Value = 54.5};
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
            var price = new Price {Currency = null, Value = 65.5};
            var result = this.priceService.AddPrice(price);
            Assert.True(!this.libraryContextMock.Prices.Any());
        }

        /// <summary>
        /// Test add add price with empty Currency.
        /// </summary>
        [Test]
        public void TestAddEmptyCurrencyPrice()
        {
            var price = new Price {Currency = string.Empty, Value = 4.5};
            var result = this.priceService.AddPrice(price);
            Assert.True(!this.libraryContextMock.Prices.Any());
        }

        /// <summary>
        /// Test add price with smaller price value.
        /// </summary>
        [Test]
        public void TestAddLessThanZeroPrice()
        {
            var price = new Price {Currency = "Euro", Value = -5};
            var result = this.priceService.AddPrice(price);
            Assert.True(!this.libraryContextMock.Prices.Any());
        }

        /// <summary>
        /// Test add price with zero price value.
        /// </summary>
        [Test]
        public void TestAddZeroValuePrice()
        {
            var price = new Price {Currency = "Euro", Value = 0};
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
            var price = new Price {Currency = "Euro", Value = minPrice-1};
            var result = this.priceService.AddPrice(price);
            Assert.True(!this.libraryContextMock.Prices.Any());
        }

        /// <summary>
        /// Test get price by id and he is in the db.
        /// </summary>
        [Test]
        public void TestGetPriceByGoodId()
        {
            var price = new Price {Currency = "Euro", Value = 54.5};
            var result = this.priceService.AddPrice(price);
            var prices = this.priceService.GetPrices();
            var priceId=prices.ToList()[0].Id;
            var priceById = this.priceService.GetPriceById(priceId);
            Assert.NotNull(priceById);
        }

        /// <summary>
        /// Test get price by id and he is not in the db.
        /// </summary>
        [Test]
        public void TestGetPriceByBadId()
        {
            var price = new Price {Currency = "Euro", Value = 54.5};
            var result = this.priceService.AddPrice(price);
            var prices = this.priceService.GetPrices();
            var priceId=prices.ToList()[0].Id;
            var priceById = this.priceService.GetPriceById(priceId + 1);
            Assert.Null(priceById);
        }

        /// <summary>
        /// Test get all prices.
        /// </summary>
        [Test]
        public void TestGetAllPrices()
        {
            var price = new Price {Currency = "Euro", Value = 54.5};
            var price2 = new Price {Currency = "Euro", Value = 74.5};
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
            var price = new Price {Currency = "Euro", Value = 54.5};
            var price2 = new Price {Currency = string.Empty, Value = 74.5};
            var result = this.priceService.AddPrice(price);
            var result2 = this.priceService.AddPrice(price2);
            var prices = this.priceService.GetPrices();
            Assert.IsFalse(prices.Count() == 2);
        }

        /// <summary>
        /// Test update Currency for price.
        /// </summary>
        [Test]
        public void TestUpdateCurrencyForPrice()
        {
            var price = new Price {Currency = "Euro", Value = 54.5};
            var result = this.priceService.AddPrice(price);
            var prices = this.priceService.GetPrices();
            var priceId=prices.ToList()[0].Id;
            var priceById = this.priceService.GetPriceById(priceId);
            priceById.Currency = "Dolar";
            var updateResult = this.priceService.UpdatePrice(price);
            var priceUpdated = this.priceService.GetPriceById(priceId);
            Assert.IsTrue(priceUpdated.Currency.Equals("Dolar"));
        }

        /// <summary>
        /// Test update with a wrong currency for auction price.
        /// </summary>
        [Test]
        public void TestUpdateWithBadCurrencyForPrice()
        {
            var price = new Price {Currency = "Euro", Value = 54.5};
            var result = this.priceService.AddPrice(price);
            var prices = this.priceService.GetPrices();
            var priceId=prices.ToList()[0].Id;
            var priceById = this.priceService.GetPriceById(priceId);
            priceById.Currency =string.Empty;
            var updateResult = this.priceService.UpdatePrice(price);
            var priceUpdated = this.priceService.GetPriceById(priceId);
            Assert.IsFalse(priceUpdated.Currency.Equals("Dolar"));
        }

        /// <summary>
        /// Test update Value for price.
        /// </summary>
        [Test]
        public void TestUpdateValueForPrice()
        {
            var price = new Price {Currency = "Euro", Value = 54.5};
            var result = this.priceService.AddPrice(price);
            var prices = this.priceService.GetPrices();
            var priceId=prices.ToList()[0].Id;
            var priceById = this.priceService.GetPriceById(priceId);
            priceById.Value = 99.3;
            var updateResult = this.priceService.UpdatePrice(priceById);
            var priceUpdated = this.priceService.GetPriceById(priceId);
            Assert.IsTrue(double.Equals(priceUpdated.Value, 99.3));
        }

        /// <summary>
        /// Test update with a wrong Value for auction price.
        /// </summary>
        [Test]
        public void TestUpdateWithBadValueForPrice()
        {
            var price = new Price {Currency = "Euro", Value = 54.5};
            var result = this.priceService.AddPrice(price);
            var prices = this.priceService.GetPrices();
            var priceId=prices.ToList()[0].Id;
            var priceById = this.priceService.GetPriceById(priceId);
            priceById.Value = 0.0;
            var updateResult = this.priceService.UpdatePrice(priceById);
            var priceUpdated = this.priceService.GetPriceById(priceId);
            Assert.IsFalse(double.Equals(priceUpdated.Value, 0.0));
        }
        
        /// <summary>
        /// Test delete Price.
        /// </summary>
        [Test]
        public void TestDeletePrice()
        {
            var price = new Price {Currency = "Euro", Value = 54.5};
            var result = this.priceService.AddPrice(price);
            var prices = this.priceService.GetPrices();
            var priceId=prices.ToList()[0].Id;
            var deleteResult = this.priceService.DeletePrice(priceId);
            Assert.True(!this.libraryContextMock.Prices.Any());
        }

        /// <summary>
        /// Test delete Price.
        /// </summary>
        [Test]
        public void TestDeletePriceWithAWrongId()
        {
            var price = new Price {Currency = "Euro", Value = 54.5};
            var result = this.priceService.AddPrice(price);
            var prices = this.priceService.GetPrices();
            var priceId=prices.ToList()[0].Id;
            var deleteResult = this.priceService.DeletePrice(priceId+1);
            Assert.True(this.libraryContextMock.Prices.Count() == 1);
        }
        
    }
}