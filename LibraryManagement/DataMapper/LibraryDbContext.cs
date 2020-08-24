// <copyright file="LibraryDbContext.cs" company="Transilvania University of Brasov">
// Margarit Marian Catalin
// </copyright>

namespace LibraryManagement.DataMapper
{
    using LibraryManagement.DomainModel;
    using System.Data.Entity;

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
        /// Gets or sets the Products
        /// Gets or sets Products..
        /// </summary>
        public DbSet<Product> Products { get; set; }

        /// <summary>
        /// Gets or sets the AuctionUsers
        /// Gets or sets AuctionUsers..
        /// </summary>
        public DbSet<AuctionUser> AuctionUsers { get; set; }

        /// <summary>
        /// Gets or sets the Auctions
        /// Gets or sets Auctions..
        /// </summary>
        public DbSet<Auction> Auctions { get; set; }

        /// <summary>
        /// Gets or sets the Bids
        /// Gets or sets Bids..
        /// </summary>
        public DbSet<Bid> Bids { get; set; }

        /// <summary>
        /// Gets or sets the Categories
        /// Gets or sets Categories..
        /// </summary>
        public DbSet<Category> Categories { get; set; }

        /// <summary>
        /// Gets or sets the Prices
        /// Gets or sets Prices..
        /// </summary>
        public DbSet<Price> Prices { get; set; }

        /// <summary>
        /// Gets or sets the UserReviews
        /// Gets or sets UserReviews..
        /// </summary>
        public DbSet<UserReview> UserReviews { get; set; }
    }
}