// <copyright file="UserReview.cs" company="Transilvania University of Brasov">
// Margarit Marian Catalin
// </copyright>
// <summary>This is the UserReview entity class.</summary>

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
        /// <value>
        /// The identifier.
        /// </value>
        public int Id { get; set; }

        /// <summary>
        /// Gets or sets UserReview Description.
        /// </summary>
        /// <value>
        /// The review description.
        /// </value>
        [StringLength(450)]
        public string Description { get; set; }

        /// <summary>
        /// Gets or sets UserReview Score.
        /// </summary>
        /// <value>
        /// The review score.
        /// </value>
        public int Score { get; set; }

        /// <summary>
        /// Gets or sets ReviewForUser.
        /// </summary>
        /// <value>
        /// The review for user.
        /// </value>
        public AuctionUser ReviewForUser { get; set; }

        /// <summary>
        /// Gets or sets ReviewByUser.
        /// </summary>
        /// <value>
        /// The review by user.
        /// </value>
        public AuctionUser ReviewByUser { get; set; }
    }
}