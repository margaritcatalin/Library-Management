// <copyright file="LibraryDbContext.cs" company="Transilvania University of Brasov">
// Margarit Marian Catalin
// </copyright>
// <summary>This is the Database context class.</summary>

namespace LibraryManagement.DataMapper
{
    using System.Data.Entity;
    using LibraryManagement.DomainModel;

    /// <summary>
    /// The library database manager.
    /// </summary>
    public class LibraryDbContext : DbContext
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="LibraryDbContext"/> class.
        /// </summary>
        public LibraryDbContext()
            : base("name=LibraryContext")
        {
            Database.SetInitializer(new DropCreateDatabaseIfModelChanges<LibraryDbContext>());
        }

        /// <summary>
        /// Gets or sets the Products.
        /// </summary>
        /// <value>The products.</value>
        public DbSet<Product> Products { get; set; }

        /// <summary>
        /// Gets or sets the AuctionUsers.
        /// </summary>
        /// <value>
        /// The auction users.
        /// </value>
        public DbSet<AuctionUser> AuctionUsers { get; set; }

        /// <summary>
        /// Gets or sets the Auctions.
        /// </summary>
        /// <value>
        /// The auctions.
        /// </value>
        public DbSet<Auction> Auctions { get; set; }

        /// <summary>
        /// Gets or sets the Bids.
        /// </summary>
        /// <value>
        /// The bids from database.
        /// </value>
        public DbSet<Bid> Bids { get; set; }

        /// <summary>
        /// Gets or sets the Categories.
        /// </summary>
        /// <value>
        /// The categories.
        /// </value>
        public DbSet<Category> Categories { get; set; }

        /// <summary>
        /// Gets or sets the Prices.
        /// </summary>
        /// <value>
        /// The prices.
        /// </value>
        public DbSet<Price> Prices { get; set; }

        /// <summary>
        /// Gets or sets the UserReviews.
        /// </summary>
        /// <value>
        /// The user reviews.
        /// </value>
        public DbSet<UserReview> UserReviews { get; set; }
    }
}