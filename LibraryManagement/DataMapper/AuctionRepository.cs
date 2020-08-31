// <copyright file="AuctionRepository.cs" company="Transilvania University of Brasov">
// Margarit Marian Catalin
// </copyright>
// <summary>This is the Auction repository class.</summary>

namespace LibraryManagement.DataMapper
{
    using System.Collections.Generic;
    using System.Data.Entity;
    using System.Linq;
    using System.Reflection;
    using LibraryManagement.DomainModel;
    using LibraryManagement.Util;

    /// <summary>
    /// The Auction repository.
    /// </summary>
    public class AuctionRepository
    {
        /// <summary>
        /// Defines the libraryContext.
        /// </summary>
        private readonly LibraryDbContext libraryContext;

        /// <summary>
        /// Initializes a new instance of the <see cref="AuctionRepository"/> class.
        /// </summary>
        /// <param name="libraryContext">The database manager.</param>
        public AuctionRepository(LibraryDbContext libraryContext)
        {
            this.libraryContext = libraryContext;
        }

        /// <summary>
        /// Add a new Auction.
        /// </summary>
        /// <param name="auction">The new Auction.</param>
        /// <returns>If Auction was added.</returns>
        public bool AddAuction(Auction auction)
        {
            this.libraryContext.Auctions.Add(auction);
            var successful = this.libraryContext.SaveChanges() != 0;
            if (successful)
            {
                LoggerUtil.LogInfo($"Auction added successfully ID: {auction.Id}", MethodBase.GetCurrentMethod());
            }
            else
            {
                LoggerUtil.LogError($"Auction failed to add to database ID: {auction.Id}", MethodBase.GetCurrentMethod());
            }

            return successful;
        }

        /// <summary>
        /// Get All Auctions.
        /// </summary>
        /// <returns>All Auctions.</returns>
        public IEnumerable<Auction> GetAuctions()
        {
            return this.libraryContext.Auctions.ToList();
        }

        /// <summary>
        /// Get Auction by id.
        /// </summary>
        /// <param name="id">The Auction id.</param>
        /// <returns>A Auction.</returns>
        public Auction GetAuctionById(int id)
        {
            return this.libraryContext.Auctions.Find(id);
        }

        /// <summary>
        /// Update a Auction.
        /// </summary>
        /// <param name="auction">The auction<see cref="Auction"/>.</param>
        /// <returns>If Auction was updated.</returns>
        public bool UpdateAuction(Auction auction)
        {
            this.libraryContext.Entry(auction).State = EntityState.Modified;
            var successful = this.libraryContext.SaveChanges() != 0;
            if (successful)
            {
                LoggerUtil.LogInfo($"Auction updated successfully ID: {auction.Id}", MethodBase.GetCurrentMethod());
            }

            return successful;
        }

        /// <summary>
        /// Delete a Auction.
        /// </summary>
        /// <param name="id">The Auction id.</param>
        /// <returns>If Auction was deleted.</returns>
        public bool DeleteAuction(int id)
        {
            var auction = this.libraryContext.Auctions.Find(id);
            if (auction != null)
            {
                this.libraryContext.Auctions.Remove(auction);
            }
            else
            {
                LoggerUtil.LogWarning($"Auction failed to delete from database. We don't found a auction with id: {id}", MethodBase.GetCurrentMethod());
                return false;
            }

            var successful = this.libraryContext.SaveChanges() != 0;
            if (successful)
            {
                LoggerUtil.LogInfo($"Auction was deleted successfully : {id} ", MethodBase.GetCurrentMethod());
            }

            return successful;
        }
    }
}