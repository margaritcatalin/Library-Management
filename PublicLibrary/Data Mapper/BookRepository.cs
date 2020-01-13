// <copyright file="BookRepository.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace PublicLibrary.Data_Mapper
{
    using System;
    using System.Collections.Generic;
    using System.Data.Entity;
    using System.Linq;
    using PublicLibrary.Domain_Model;

    /// <summary>
    /// The Book repository.
    /// </summary>
    public class BookRepository
    {
        private readonly LibraryDb libraryDb;

        /// <summary>
        /// Initializes a new instance of the <see cref="BookRepository"/> class.
        /// </summary>
        /// <param name="libraryDb">The database connection.</param>
        public BookRepository(LibraryDb libraryDb)
        {
            this.libraryDb = libraryDb;
        }

        /// <summary>
        /// Add a new book.
        /// </summary>
        /// <param name="book">The book.</param>
        /// <returns>If book was created.</returns>
        public bool AddBook(Book book)
        {
            book.Authors = book.Authors.Select(a =>
            {
                var dba = this.libraryDb.Authors.FirstOrDefault(dbA => dbA.Name.Equals(a.Name));
                if (dba == null)
                {
                    return a;
                }
                else
                {
                    return dba;
                }
            }).ToList();

            book.Categories = book.Categories.Select(c =>
            {
                var dbc = this.libraryDb.Categories.FirstOrDefault(dbC => dbC.Name.Equals(c.Name));
                if (dbc == null)
                {
                    return c;
                }
                else
                {
                    return dbc;
                }
            }).ToList();

            this.libraryDb.Books.Add(book);
            return this.libraryDb.SaveChanges() != 0;
        }

        /// <summary>
        /// Get a book by bookName.
        /// </summary>
        /// <param name="bookName">The book name.</param>
        /// <returns>The book.</returns>
        public Book GetBook(string bookName)
        {
            var result = this.libraryDb.Books.FirstOrDefault(b => b.Name.Equals(bookName));
            return result;
        }

        /// <summary>
        /// Ged edition by book name and edition name.
        /// </summary>
        /// <param name="bookName">The book name.</param>
        /// <param name="editionName">The edition name.</param>
        /// <returns>The edition.</returns>
        public Edition GetEdition(string bookName, string editionName)
        {
            var edition = this.libraryDb.Editions.Include(e => e.BookStock)
                .Include(e => e.BorrowedBooks)
                .Include(e => e.BorrowedBooks.Select(bb => bb.BookWithdrawal))
                .First(e => e.Name.Equals(editionName) && e.Book.Name.Equals(bookName));

            return edition;
        }

        /// <summary>
        /// Get editions.
        /// </summary>
        /// <param name="editions">The editions.</param>
        /// <returns>Editions list.</returns>
        public List<Edition> GetEditions(List<Borrowing> editions)
        {
            return editions.Select(e => this.GetEdition(e.BookName, e.EditionName)).ToList();
        }

        /// <summary>
        /// Borrow books.
        /// </summary>
        /// <param name="editionsList">The editions.</param>
        /// <param name="reader">The reader.</param>
        /// <param name="employee">The employee.</param>
        /// <returns>The bookwithdrawl.</returns>
        public BookWithdrawal BorrowBooks(List<Borrowing> editionsList,Reader reader, Employee employee)
        {
            List<Edition> editions = this.GetEditions(editionsList);

            BookWithdrawal bookWithdrawal = new BookWithdrawal
            {
                BorrowedBooks = editions.Select(e => new BorrowedBook
                {
                    Book = e.Book,
                    Edition = e,
                }).ToList(),
                Date = DateTime.Now,
                Reader = reader,
                Employee = employee,
                Extensions = new List<Extension>(),
            };

            this.libraryDb.BookWithdrawals.Add(bookWithdrawal);
            if (this.libraryDb.SaveChanges() == 0)
            {
                return null;
            }

            return bookWithdrawal;
        }

        /// <summary>
        /// Get a book by bookToGet.
        /// </summary>
        /// <param name="bookToGet">The book.</param>
        /// <returns>The specific book.</returns>
        public Book GetBook(Book bookToGet)
        {
            return this.libraryDb.Books.FirstOrDefault(b => b.Name.Equals(bookToGet.Name));
        }

        /// <summary>
        /// Add a new edition.
        /// </summary>
        /// <param name="book">The book.</param>
        /// <param name="edition">The new edition.</param>
        /// <returns>If edition was added.</returns>
        public bool AddEdition(Book book, Edition edition)
        {
            book.Editions.Add(edition);
            return this.libraryDb.SaveChanges() != 0;
        }
    }
}