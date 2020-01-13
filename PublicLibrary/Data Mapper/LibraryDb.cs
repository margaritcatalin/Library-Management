// <copyright file="LibraryDb.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace PublicLibrary.Data_Mapper
{
    using System.Data.Entity;
    using PublicLibrary.Domain_Model;

    public class LibraryDb : DbContext
    {
        // Your context has been configured to use a 'LibraryDb' connection string from your application's 
        // configuration file (App.config or Web.config). By default, this connection string targets the 
        // 'PublicLibrary.Data_Mapper.LibraryDb' database on your LocalDb instance. 
        // 
        // If you wish to target a different database and/or database provider, modify the 'LibraryDb' 
        // connection string in the application configuration file.
        public LibraryDb()
            : base("name=LibraryDB")
        {
            Database.SetInitializer(new DropCreateDatabaseIfModelChanges<LibraryDb>());
        }

        /// <summary>
        /// The books.
        /// </summary>
        public DbSet<Book> Books { get; set; }

        /// <summary>
        /// The authors.
        /// </summary>
        public DbSet<Author> Authors { get; set; }

        /// <summary>
        /// The bookwitdrawls.
        /// </summary>
        public DbSet<BookWithdrawal> BookWithdrawals { get; set; }

        /// <summary>
        /// The borrowed books.
        /// </summary>
        public DbSet<BorrowedBook> BorrowedBooks { get; set; }

        /// <summary>
        /// The categories.
        /// </summary>
        public DbSet<Category> Categories { get; set; }

        /// <summary>
        /// The editions.
        /// </summary>
        public DbSet<Edition> Editions { get; set; }

        /// <summary>
        /// The readers.
        /// </summary>
        public DbSet<Reader> Readers { get; set; }

        /// <summary>
        /// The employees.
        /// </summary>
        public DbSet<Employee> Employees { get; set; }

        /// <summary>
        /// The books.
        /// </summary>
        public DbSet<BookStock> BookStocks { get; set; }

        /// <summary>
        /// The extensions.
        /// </summary>
        public DbSet<Extension> Extensions { get; set; }

        // Add a DbSet for each entity type that you want to include in your model. For more information 
        // on configuring and using a Code First model, see http://go.microsoft.com/fwlink/?LinkId=390109.

        // public virtual DbSet<MyEntity> MyEntities { get; set; }
    }
}