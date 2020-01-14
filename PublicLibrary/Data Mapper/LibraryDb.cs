// <copyright file="LibraryDb.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace PublicLibrary.Data_Mapper
{
    using System.Data.Entity;
    using PublicLibrary.Domain_Model;

    /// <summary>
    /// The library database manager.
    /// </summary>
    public class LibraryDb : DbContext
    {
        // Your context has been configured to use a 'LibraryDb' connection string from your application's
        // configuration file (App.config or Web.config). By default, this connection string targets the
        // 'PublicLibrary.Data_Mapper.LibraryDb' database on your LocalDb instance.
        // If you wish to target a different database and/or database provider, modify the 'LibraryDb'
        // connection string in the application configuration file.

        /// <summary>
        /// Initializes a new instance of the <see cref="LibraryDb"/> class.
        /// </summary>
        public LibraryDb()
            : base("name=LibraryDB")
        {
            Database.SetInitializer(new DropCreateDatabaseIfModelChanges<LibraryDb>());
        }

        /// <summary>
        /// Gets or sets books.
        /// </summary>
        public DbSet<Book> Books { get; set; }

        /// <summary>
        /// Gets or sets authors.
        /// </summary>
        public DbSet<Author> Authors { get; set; }

        /// <summary>
        /// Gets or sets bookwithdrawl.
        /// </summary>
        public DbSet<BookWithdrawal> BookWithdrawals { get; set; }

        /// <summary>
        /// Gets or sets borrowed books.
        /// </summary>
        public DbSet<BorrowedBook> BorrowedBooks { get; set; }

        /// <summary>
        /// Gets or sets categories.
        /// </summary>
        public DbSet<Category> Categories { get; set; }

        /// <summary>
        /// Gets or sets editions.
        /// </summary>
        public DbSet<Edition> Editions { get; set; }

        /// <summary>
        /// Gets or sets readers.
        /// </summary>
        public DbSet<Reader> Readers { get; set; }

        /// <summary>
        /// Gets or sets employees.
        /// </summary>
        public DbSet<Employee> Employees { get; set; }

        /// <summary>
        /// Gets or sets bookstocks.
        /// </summary>
        public DbSet<BookStock> BookStocks { get; set; }

        /// <summary>
        /// Gets or sets extensions.
        /// </summary>
        public DbSet<Extension> Extensions { get; set; }

        // Add a DbSet for each entity type that you want to include in your model. For more information
        // on configuring and using a Code First model, see http://go.microsoft.com/fwlink/?LinkId=390109.

        // public virtual DbSet<MyEntity> MyEntities { get; set; }
    }
}