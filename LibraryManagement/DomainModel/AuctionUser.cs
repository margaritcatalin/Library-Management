// <copyright file="AuctionUser.cs" company="Transilvania University of Brasov">
// Margarit Marian Catalin
// </copyright>
// <summary>This is the AuctionUser entity class.</summary>

namespace LibraryManagement.DomainModel
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    /// <summary>
    /// Auctioneer entity.
    /// </summary>
    public class AuctionUser
    {
        /// <summary>
        /// Gets or sets the identifier.
        /// </summary>
        /// <value>
        /// The identifier.
        /// </value>
        public int Id { get; set; }

        /// <summary>
        /// Gets or sets the first name.
        /// </summary>
        /// <value>
        /// The first name.
        /// </value>
        [StringLength(450)]
        public string FirstName { get; set; }

        /// <summary>
        /// Gets or sets the last name.
        /// </summary>
        /// <value>
        /// The last name.
        /// </value>
        [StringLength(450)]
        public string LastName { get; set; }

        /// <summary>
        /// Gets or sets the gender.
        /// </summary>
        /// <value>
        /// The gender.
        /// </value>
        public string Gender { get; set; }

        /// <summary>
        /// Gets or sets the role.
        /// </summary>
        /// <value>
        /// The role from database.
        /// </value>
        public string Role { get; set; }

        /// <summary>
        /// Gets the products.
        /// </summary>
        /// <value>
        /// The products.
        /// </value>
        public ICollection<Product> Products { get; }

        /// <summary>
        /// Gets or sets the auctions.
        /// </summary>
        /// <value>
        /// The auctions.
        /// </value>
        public ICollection<Auction> Auctions { get; set; }

        /// <summary>
        /// Gets or sets AuctionUser bids.
        /// </summary>
        /// <value>
        /// The bids from database.
        /// </value>
        public ICollection<Bid> Bids { get; set; }

        /// <summary>
        /// Gets AuctionUser reviews.
        /// </summary>
        /// <value>
        /// The reviews from database.
        /// </value>
        public ICollection<UserReview> Reviews { get; }

        /// <summary>
        /// Gets AuctionUser given reviews.
        /// </summary>
        /// <value>
        /// The given reviews.
        /// </value>
        public ICollection<UserReview> GivenReviews { get; }
    }
}