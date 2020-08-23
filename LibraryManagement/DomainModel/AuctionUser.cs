// <copyright file="AuctionUser.cs" company="Transilvania University of Brasov">
// Margarit Marian Catalin
// </copyright>


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
        /// Gets or sets AuctionUser code.
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Gets or sets AuctionUser firstName.
        /// </summary>
        [StringLength(450)]
        public string FirstName { get; set; }

        /// <summary>
        /// Gets or sets AuctionUser lastName.
        /// </summary>
        [StringLength(450)]
        public string LastName { get; set; }

        /// <summary>
        /// Gets or sets AuctionUser gender.
        /// </summary>
        public string Gender { get; set; }

        /// <summary>
        /// Gets or sets AuctionUser role.
        /// </summary>
        public string Role { get; set; }

        /// <summary>
        /// Gets or sets AuctionUser products.
        /// </summary>
        public ICollection<Product> Products { get; set; }

        /// <summary>
        /// Gets or sets AuctionUser auctions.
        /// </summary>
        public ICollection<Auction> Auctions { get; set; }

        /// <summary>
        /// Gets or sets AuctionUser bids.
        /// </summary>
        public ICollection<Bid> Bids { get; set; }

        /// <summary>
        /// Gets or sets AuctionUser reviews.
        /// </summary>
        public ICollection<UserReview> Reviews { get; set; }

        /// <summary>
        /// Gets or sets AuctionUser given reviews.
        /// </summary>
        public ICollection<UserReview> GivenReviews { get; set; }
    }
}