// <copyright file="PriceRepository.cs" company="Transilvania University of Brasov">
// Margarit Marian Catalin
// </copyright>
// <summary>This is the Price repository class.</summary>

namespace LibraryManagement.DataMapper
{
    using System.Collections.Generic;
    using System.Data.Entity;
    using System.Linq;
    using System.Reflection;
    using LibraryManagement.DomainModel;
    using LibraryManagement.Util;

    /// <summary>
    /// The Price repository.
    /// </summary>
    public class PriceRepository
    {
        /// <summary>
        /// Defines the libraryContext.
        /// </summary>
        private readonly LibraryDbContext libraryContext;

        /// <summary>
        /// Initializes a new instance of the <see cref="PriceRepository"/> class.
        /// </summary>
        /// <param name="libraryContext">The database manager.</param>
        public PriceRepository(LibraryDbContext libraryContext)
        {
            this.libraryContext = libraryContext;
        }

        /// <summary>
        /// Add a new price.
        /// </summary>
        /// <param name="price">The new price.</param>
        /// <returns>If price was added.</returns>
        public bool AddPrice(Price price)
        {
            this.libraryContext.Prices.Add(price);
            var successful = this.libraryContext.SaveChanges() != 0;
            if (successful)
            {
                LoggerUtil.LogInfo($"Price added successfully : {price.Value}{price.Currency} ", MethodBase.GetCurrentMethod());
            }
            else
            {
                LoggerUtil.LogError($"Price failed to add to database : {price.Value}{price.Currency}", MethodBase.GetCurrentMethod());
            }

            return successful;
        }

        /// <summary>
        /// Get All prices.
        /// </summary>
        /// <returns>All prices.</returns>
        public IEnumerable<Price> GetPrices()
        {
            return this.libraryContext.Prices.ToList();
        }

        /// <summary>
        /// Get Price by id.
        /// </summary>
        /// <param name="id">The price id.</param>
        /// <returns>A price if exist or null if not.</returns>
        public Price GetPriceById(int id)
        {
            return this.libraryContext.Prices.Find(id);
        }

        /// <summary>
        /// Update a price.
        /// </summary>
        /// <param name="price">The price.</param>
        /// <returns>If price was updated.</returns>
        public bool UpdatePrice(Price price)
        {
            this.libraryContext.Entry(price).State = EntityState.Modified;
            var successful = this.libraryContext.SaveChanges() != 0;
            if (successful)
            {
                LoggerUtil.LogInfo($"Price updated successfully : {price.Value}{price.Currency}", MethodBase.GetCurrentMethod());
            }

            return successful;
        }

        /// <summary>
        /// Delete a price.
        /// </summary>
        /// <param name="id">The price id.</param>
        /// <returns>If price was deleted.</returns>
        public bool DeletePrice(int id)
        {
            var price = this.libraryContext.Prices.Find(id);
            if (price != null)
            {
                this.libraryContext.Prices.Remove(price);
            }
            else
            {
                LoggerUtil.LogWarning($"Price failed to delete from database. We don't found a price with id: {id}", MethodBase.GetCurrentMethod());
                return false;
            }

            var successful = this.libraryContext.SaveChanges() != 0;
            if (successful)
            {
                LoggerUtil.LogInfo($"Price was deleted successfully : {id} ", MethodBase.GetCurrentMethod());
            }
            else
            {
                LoggerUtil.LogError($"Price failed to delete from database : {id}", MethodBase.GetCurrentMethod());
            }

            return successful;
        }
    }
}