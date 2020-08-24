// <copyright file="Bid.cs" company="Transilvania University of Brasov">
// Margarit Marian Catalin
// </copyright>
// <summary>This is the Bid entity class.</summary>

namespace LibraryManagement.DomainModel
{
    using System;

    /// <summary>
    /// Bid entity.
    /// </summary>
    public class Bid
    {
        /// <summary>
        /// Gets or sets id.
        /// </summary>
        /// <value>
        /// The identifier.
        /// </value>
        public int Id { get; set; }

        /// <summary>
        /// Gets or sets auction.
        /// </summary>
        /// <value>
        /// The auction.
        /// </value>
        public Auction Auction { get; set; }

        /// <summary>
        /// Gets or sets BidPrice.
        /// </summary>
        /// <value>
        /// The bid price.
        /// </value>
        public Price BidPrice { get; set; }

        /// <summary>
        /// Gets or sets BidUser.
        /// </summary>
        /// <value>
        /// The bid user.
        /// </value>
        public AuctionUser BidUser { get; set; }

        /// <summary>
        /// Gets or sets bid date.
        /// </summary>
        /// <value>
        /// The bid date.
        /// </value>
        public DateTime BidDate { get; set; }
    }
}