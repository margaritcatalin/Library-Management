// <copyright file="Bid.cs" company="Transilvania University of Brasov">
// Margarit Marian Catalin
// </copyright>

using System;
using LibraryManagement.DataMapper;

namespace LibraryManagement.DomainModel
{
    /// <summary>
    /// Bid entity.
    /// </summary>
    public class Bid
    {
        /// <summary>
        /// Gets or sets id.
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Gets or sets auction.
        /// </summary>
        public Auction Auction { get; set; }

        /// <summary>
        /// Gets or sets BidPrice.
        /// </summary>
        public Price BidPrice { get; set; }

        /// <summary>
        /// Gets or sets BidUser.
        /// </summary>
        public AuctionUser BidUser { get; set; }
        
        /// <summary>
        /// Gets or sets bid date.
        /// </summary>
        public DateTime BidDate { get; set; }
    }
}