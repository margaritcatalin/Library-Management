using PublicLibrary.BusinessLayer;
using PublicLibrary.Domain_Model;

namespace PublicLibrary.Data_Mapper
{
    using System;
    using System.Data.Entity;
    using System.Linq;

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

        public DbSet<Book> Books { get; set; }

        public DbSet<Author> Authors { get; set; }

        public DbSet<BookWithdrawal> BookWithdrawals { get; set; }

        public DbSet<BorrowedBook> BorrowedBooks { get; set; }

        public DbSet<Category> Categories { get; set; }

        public DbSet<Edition> Editions { get; set; }

        public DbSet<Reader> Readers { get; set; }

        public DbSet<Employee> Employees { get; set; }

        public DbSet<BookStock> BookStocks { get; set; }

        public DbSet<Extension> Extensions { get; set; }


        // Add a DbSet for each entity type that you want to include in your model. For more information 
        // on configuring and using a Code First model, see http://go.microsoft.com/fwlink/?LinkId=390109.

        // public virtual DbSet<MyEntity> MyEntities { get; set; }
    }

    //public class MyEntity
    //{
    //    public int Id { get; set; }
    //    public string Name { get; set; }
    //}
}