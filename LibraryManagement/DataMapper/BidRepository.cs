// <copyright file="BidRepository.cs" company="Transilvania University of Brasov">
// Margarit Marian Catalin
// </copyright>
// <summary>This is the Bid repository class.</summary>

namespace LibraryManagement.DataMapper
{
    using System.Collections.Generic;
    using System.Data.Entity;
    using System.Linq;
    using System.Reflection;
    using LibraryManagement.DomainModel;
    using LibraryManagement.Util;

    /// <summary>
    /// The Bid repository.
    /// </summary>
    public class BidRepository
    {
        /// <summary>
        /// Defines the libraryContext.
        /// </summary>
        private readonly LibraryDbContext libraryContext;

        /// <summary>
        /// Initializes a new instance of the <see cref="BidRepository"/> class.
        /// </summary>
        /// <param name="libraryContext">The database manager.</param>
        public BidRepository(LibraryDbContext libraryContext)
        {
            this.libraryContext = libraryContext;
        }

        /// <summary>
        /// Add a new Bid.
        /// </summary>
        /// <param name="bid">The new Bid.</param>
        /// <returns>If Bid was added.</returns>
        public bool AddBid(Bid bid)
        {
            this.libraryContext.Bids.Add(bid);
            var successful = this.libraryContext.SaveChanges() != 0;
            if (successful)
            {
                LoggerUtil.LogInfo($"Bid added successfully : {bid.Id}", MethodBase.GetCurrentMethod());
            }
            else
            {
                LoggerUtil.LogError($"Bid failed to add to database : {bid.Id}", MethodBase.GetCurrentMethod());
            }

            return successful;
        }

        /// <summary>
        /// Get All Bids.
        /// </summary>
        /// <returns>All Bids from the database.</returns>
        public IEnumerable<Bid> GetBids()
        {
            return this.libraryContext.Bids.ToList();
        }

        /// <summary>
        /// Get Bid by id.
        /// </summary>
        /// <param name="id">The Bid id.</param>
        /// <returns>A Bid if exist or null if not.</returns>
        public Bid GetBidById(int id)
        {
            return this.libraryContext.Bids.Find(id);
        }

        /// <summary>
        /// Update a Bid.
        /// </summary>
        /// <param name="bid">The Bid what will be updated.</param>
        /// <returns>If Bid was updated.</returns>
        public bool UpdateBid(Bid bid)
        {
            this.libraryContext.Entry(bid).State = EntityState.Modified;
            var successful = this.libraryContext.SaveChanges() != 0;
            if (successful)
            {
                LoggerUtil.LogInfo($"Bid updated successfully : {bid.Id}", MethodBase.GetCurrentMethod());
            }

            return successful;
        }

        /// <summary>
        /// Delete a Bid.
        /// </summary>
        /// <param name="id">The Bid id.</param>
        /// <returns>If Bid was deleted.</returns>
        public bool DeleteBid(int id)
        {
            var bid = this.libraryContext.Bids.Find(id);
            if (bid != null)
            {
                this.libraryContext.Bids.Remove(bid);
            }
            else
            {
                LoggerUtil.LogWarning($"Bid failed to delete from database. We don't found a bid with id: {id}", MethodBase.GetCurrentMethod());
                return false;
            }

            var successful = this.libraryContext.SaveChanges() != 0;
            if (successful)
            {
                LoggerUtil.LogInfo($"Bid was deleted successfully : {id} ", MethodBase.GetCurrentMethod());
            }
            else
            {
                LoggerUtil.LogError($"Bid failed to delete from database : {id}", MethodBase.GetCurrentMethod());
            }

            return successful;
        }
    }
}