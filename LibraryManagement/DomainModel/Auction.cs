// <copyright file="Auction.cs" company="Transilvania University of Brasov">
// Margarit Marian Catalin
// </copyright>
// <summary>This is the Auction entity class.</summary>

namespace LibraryManagement.DomainModel
{
    using System;
    using System.Collections.Generic;

    /// <summary>
    /// The Auction entity.
    /// </summary>
    public class Auction
    {
        /// <summary>Gets or sets the identifier.</summary>
        /// <value>The identifier.</value>
        public int Id { get; set; }

        /// <summary>Gets or sets the auctioneer.</summary>
        /// <value>The auctioneer.</value>
        public AuctionUser Auctioneer { get; set; }

        /// <summary>
        /// Gets or sets the product.
        /// </summary>
        /// <value>
        /// The product.
        /// </value>
        public Product Product { get; set; }

        /// <summary>
        /// Gets or sets the start price.
        /// </summary>
        /// <value>
        /// The start price.
        /// </value>
        public Price StartPrice { get; set; }

        /// <summary>
        /// Gets or sets the start date.
        /// </summary>
        /// <value>
        /// The start date.
        /// </value>
        public DateTime StartDate { get; set; }

        /// <summary>
        /// Gets or sets the end date.
        /// </summary>
        /// <value>
        /// The end date.
        /// </value>
        public DateTime EndDate { get; set; }

        /// <summary>Gets or sets a value indicating whether this <see cref="Auction" /> is ended.</summary>
        /// <value>
        /// <c>true</c> if ended; otherwise, <c>false</c>.</value>
        public bool Ended { get; set; }

        /// <summary>Gets or sets the bid.</summary>
        /// <value>The bid from database.</value>
        public ICollection<Bid> Bid { get; set; }
    }
}
