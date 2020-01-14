// <copyright file="BookRepository.cs" company="Transilvania University of Brasov">
// Margarit Marian Catalin
// </copyright>

namespace PublicLibrary.Data_Mapper
{
    using System;
    using System.Collections.Generic;
    using System.Data.Entity;
    using System.Linq;
    using PublicLibrary.Domain_Model;

    /// <summary>
    /// The book repository.
    /// </summary>
    public class BookRepository
    {
        private readonly LibraryDbContext libraryContext;

        /// <summary>
        /// Initializes a new instance of the <see cref="BookRepository"/> class.
        /// </summary>
        /// <param name="libraryContext">The library db.</param>
        public BookRepository(LibraryDbContext libraryContext)
        {
            this.libraryContext = libraryContext;
        }

        /// <summary>
        /// Add a new book.
        /// </summary>
        /// <param name="book">The book.</param>
        /// <returns>If book was added.</returns>
        public bool AddBook(Book book)
        {
            book.Authors = book.Authors.Select(
                a =>
                {
                    var dba = this.libraryContext.Authors.FirstOrDefault(
                        dbA => dbA.FirstName.Equals(a.FirstName) && dbA.LastName.Equals(a.LastName));
                    if (dba == null)
                    {
                        return a;
                    }
                    else
                    {
                        return dba;
                    }
                }).ToList();

            book.Categories = book.Categories.Select(
                c =>
                {
                    var dbc = this.libraryContext.Categories.FirstOrDefault(dbC => dbC.Name.Equals(c.Name));
                    if (dbc == null)
                    {
                        return c;
                    }
                    else
                    {
                        return dbc;
                    }
                }).ToList();

            this.libraryContext.Books.Add(book);
            return this.libraryContext.SaveChanges() != 0;
        }

        /// <summary>
        /// Get book by name.
        /// </summary>
        /// <param name="bookName">The book name.</param>
        /// <returns>A book.</returns>
        public Book GetBook(string bookName)
        {
            var result = this.libraryContext.Books.FirstOrDefault(b => b.Name.Equals(bookName));
            return result;
        }

        /// <summary>
        /// Get edition by book name adn edition name.
        /// </summary>
        /// <param name="bookName">The book name.</param>
        /// <param name="editionName">The edition name.</param>
        /// <returns>An edition.</returns>
        public Edition GetEdition(string bookName, string editionName)
        {
            var edition = this.libraryContext.Editions.Include(e => e.BookStock).Include(e => e.BorrowedBooks)
                .Include(e => e.BorrowedBooks.Select(bb => bb.BookWithdrawal))
                .FirstOrDefault(e => e.Name.Equals(editionName) && e.Book.Name.Equals(bookName));

            return edition;
        }

        /// <summary>
        /// Get editions.
        /// </summary>
        /// <param name="editions">The editions.</param>
        /// <returns>A list with editions.</returns>
        public List<Edition> GetEditions(List<Borrowing> editions)
        {
            return editions.Select(e => this.GetEdition(e.BookName, e.EditionName)).ToList();
        }

        /// <summary>
        /// Borrow books.
        /// </summary>
        /// <param name="editionsList">The edition list.</param>
        /// <param name="reader">The reader.</param>
        /// <param name="employee">The employee.</param>
        /// <returns>The bookwithdrawl.</returns>
        public BookWithdrawal BorrowBooks(List<Borrowing> editionsList, Reader reader, Employee employee)
        {
            var editions = this.GetEditions(editionsList);

            var bookWithdrawal = new BookWithdrawal
                                 {
                                     BorrowedBooks =
                                         editions.Select(e => new BorrowedBook { Book = e.Book, Edition = e }).ToList(),
                                     Date = DateTime.Now,
                                     Reader = reader,
                                     Employee = employee,
                                     Extensions = new List<Extension>(),
                                 };

            this.libraryContext.BookWithdrawals.Add(bookWithdrawal);
            if (this.libraryContext.SaveChanges() == 0)
            {
                return null;
            }

            return bookWithdrawal;
        }

        /// <summary>
        /// Find book by bookToGet.
        /// </summary>
        /// <param name="bookToGet">The book.</param>
        /// <returns>A book.</returns>
        public Book GetBook(Book bookToGet)
        {
            return this.libraryContext.Books.FirstOrDefault(b => b.Name.Equals(bookToGet.Name));
        }

        /// <summary>
        /// Add a new edition.
        /// </summary>
        /// <param name="book">The book.</param>
        /// <param name="edition">The edition.</param>
        /// <returns>If edition was added.</returns>
        public bool AddEdition(Book book, Edition edition)
        {
            if (book.Editions == null)
            {
                book.Editions = new List<Edition> { edition };
            }
            else
            {
                book.Editions.Add(edition);
            }

            return this.libraryContext.SaveChanges() != 0;
        }
    }
}