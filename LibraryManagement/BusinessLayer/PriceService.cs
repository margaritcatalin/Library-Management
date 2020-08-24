// <copyright file="PriceService.cs" company="Transilvania University of Brasov">
// Margarit Marian Catalin
// </copyright>
// <summary>This is the Price service class.</summary>

namespace LibraryManagement.BusinessLayer
{
    using System.Collections.Generic;
    using System.Configuration;
    using System.Reflection;
    using Castle.Core.Internal;
    using LibraryManagement.DataMapper;
    using LibraryManagement.DomainModel;
    using LibraryManagement.Util;

    /// <summary>
    /// The Price service.
    /// </summary>
    public class PriceService
    {
        /// <summary>
        /// Defines the priceRepository.
        /// </summary>
        private readonly PriceRepository priceRepository;

        /// <summary>
        /// Initializes a new instance of the <see cref="PriceService"/> class.
        /// </summary>
        /// <param name="priceRepository">The Price repository.</param>
        public PriceService(PriceRepository priceRepository)
        {
            this.priceRepository = priceRepository;
        }

        /// <summary>
        /// Add a new Price.
        /// </summary>
        /// <param name="price">The Price.</param>
        /// <returns>If Price was added.</returns>
        public bool AddPrice(Price price)
        {
            if (this.ValidatePrice(price))
            {
                return this.priceRepository.AddPrice(price);
            }

            return false;
        }

        /// <summary>
        /// Get All AuctionUsers.
        /// </summary>
        /// <returns>All AuctionUsers.</returns>
        public IEnumerable<Price> GetPrices()
        {
            return this.priceRepository.GetPrices();
        }

        /// <summary>
        /// Get Price by Price id.
        /// </summary>
        /// <param name="id">The Price id.</param>
        /// <returns>A price if it is in the database or null if not.</returns>
        public Price GetPriceById(int id)
        {
            return this.priceRepository.GetPriceById(id);
        }

        /// <summary>
        /// Update an Price.
        /// </summary>
        /// <param name="price">The Price.</param>
        /// <returns>If Price was updated.</returns>
        public bool UpdatePrice(Price price)
        {
            if (this.ValidatePrice(price))
            {
                return this.priceRepository.UpdatePrice(price);
            }

            return false;
        }

        /// <summary>
        /// Delete Price.
        /// </summary>
        /// <param name="id">The Price id.</param>
        /// <returns>If Price was deleted.</returns>
        public bool DeletePrice(int id)
        {
            return this.priceRepository.DeletePrice(id);
        }

        /// <summary>
        /// Validation for Price user.
        /// </summary>
        /// <param name="price">The Price.</param>
        /// <returns>If Price is valid or not.</returns>
        private bool ValidatePrice(Price price)
        {
            if (price == null)
            {
                LoggerUtil.LogInfo($"Price is invalid. You tried to add an null Price.", MethodBase.GetCurrentMethod());
                return false;
            }

            if (price.Currency.IsNullOrEmpty())
            {
                LoggerUtil.LogInfo($"Price is invalid. You need to add a currency.", MethodBase.GetCurrentMethod());
                return false;
            }

            if (price.Value <= 0)
            {
                LoggerUtil.LogInfo($"Price is invalid. Price value is need to be more than 0.", MethodBase.GetCurrentMethod());
                return false;
            }

            var minPrice = double.Parse(ConfigurationManager.AppSettings["MIN_PRICE"]);

            if (price.Value < minPrice)
            {
                LoggerUtil.LogInfo($"Price is invalid. You tried to add a small price.", MethodBase.GetCurrentMethod());
                return false;
            }

            return true;
        }
    }
}