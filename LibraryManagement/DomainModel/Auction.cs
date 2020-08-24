// <copyright file="Auction.cs" company="Transilvania University of Brasov">
// Margarit Marian Catalin
// </copyright>

namespace LibraryManagement.DomainModel
{
    using System;
    using System.Collections.Generic;

    /// <summary>
    /// The Auction entity.
    /// </summary>
    public class Auction
    {
        /// <summary>
        /// Gets or sets id.
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Gets or sets auctioneer.
        /// </summary>
        public AuctionUser Auctioneer { get; set; }

        /// <summary>
        /// Gets or sets start price.
        /// </summary>
        public Product Product { get; set; }

        /// <summary>
        /// Gets or sets start price.
        /// </summary>
        public Price StartPrice { get; set; }

        /// <summary>
        /// Gets or sets start date.
        /// </summary>
        public DateTime StartDate { get; set; }

        /// <summary>
        /// Gets or sets end date.
        /// </summary>
        public DateTime EndDate { get; set; }

        /// <summary>
        /// Gets or sets ended.
        /// </summary>
        public bool Ended { get; set; }

        /// <summary>
        /// Gets or sets Auction bids.
        /// </summary>
        public ICollection<Bid> Bid { get; set; }
    }
}