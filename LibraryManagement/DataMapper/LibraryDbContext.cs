// <copyright file="LibraryDbContext.cs" company="Transilvania University of Brasov">
// Margarit Marian Catalin
// </copyright>

namespace LibraryManagement.DataMapper
{
    using System.Data.Entity;
    using LibraryManagement.DomainModel;

    /// <summary>
    /// The library database manager.
    /// </summary>
    public class LibraryDbContext : DbContext
    {
        // Your context has been configured to use a 'LibraryDbContext' connection string from your application's
        // configuration file (App.config or Web.config). By default, this connection string targets the
        // 'LibraryManagement.DataMapper.LibraryDbContext' database on your LocalDb instance.
        // If you wish to target a different database and/or database provider, modify the 'LibraryDbContext'
        // connection string in the application configuration file.

        /// <summary>
        /// Initializes a new instance of the <see cref="LibraryDbContext"/> class.
        /// </summary>
        public LibraryDbContext()
            : base("name=LibraryContext")
        {
            Database.SetInitializer(new DropCreateDatabaseIfModelChanges<LibraryDbContext>());
        }

        /// <summary>
        /// Gets or sets Products.
        /// </summary>
        public DbSet<Product> Products { get; set; }

        /// <summary>
        /// Gets or sets AuctionUsers.
        /// </summary>
        public DbSet<AuctionUser> AuctionUsers { get; set; }

        /// <summary>
        /// Gets or sets Auctions.
        /// </summary>
        public DbSet<Auction> Auctions { get; set; }

        /// <summary>
        /// Gets or sets Bids.
        /// </summary>
        public DbSet<Bid> Bids { get; set; }

        /// <summary>
        /// Gets or sets Categories.
        /// </summary>
        public DbSet<Category> Categories { get; set; }

        /// <summary>
        /// Gets or sets Prices.
        /// </summary>
        public DbSet<Price> Prices { get; set; }

        /// <summary>
        /// Gets or sets UserReviews.
        /// </summary>
        public DbSet<UserReview> UserReviews { get; set; }


        // Add a DbSet for each entity type that you want to include in your model. For more information
        // on configuring and using a Code First model, see http://go.microsoft.com/fwlink/?LinkId=390109.

        // public virtual DbSet<MyEntity> MyEntities { get; set; }
    }
}