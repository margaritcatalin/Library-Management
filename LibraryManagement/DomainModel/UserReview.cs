// <copyright file="Price.cs" company="Transilvania University of Brasov">
// Margarit Marian Catalin
// </copyright>

namespace LibraryManagement.DomainModel
{
    using System.ComponentModel.DataAnnotations;

    /// <summary>
    /// UserRole entity.
    /// </summary>
    public class UserReview
    {
        /// <summary>
        /// Gets or sets UserReview code.
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Gets or sets UserReview Description.
        /// </summary>
        [StringLength(450)]
        public string Description { get; set; }

        /// <summary>
        /// Gets or sets UserReview Score.
        /// </summary>
        public int Score { get; set; }

        /// <summary>
        /// Gets or sets ReviewForUser.
        /// </summary>
        public AuctionUser ReviewForUser { get; set; }

        /// <summary>
        /// Gets or sets ReviewByUser.
        /// </summary>
        public AuctionUser ReviewByUser { get; set; }
    }
}