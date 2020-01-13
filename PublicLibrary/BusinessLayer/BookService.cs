// <copyright file="BookService.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace PublicLibrary.BusinessLayer
{
    using System;
    using System.Collections.Generic;
    using System.Configuration;
    using System.Linq;
    using Castle.Core.Internal;
    using PublicLibrary.Data_Mapper;
    using PublicLibrary.Domain_Model;

    /// <summary>
    /// This is a service for Book entity.
    /// </summary>
    public class BookService
    {
        private readonly BookRepository bookRepository;

        private readonly CategoriesService categoriesService;

        private readonly ReaderRepository readerRepository;

        private readonly ReaderService readerService;

        /// <summary>
        /// Initializes a new instance of the <see cref="BookService"/> class.
        /// </summary>
        /// <param name="bookRepository"> The book repositoty.</param>
        /// <param name="categoriesService">The category service.</param>
        /// <param name="readerRepository">The reader repository.</param>
        public BookService(BookRepository bookRepository, CategoriesService categoriesService, ReaderRepository readerRepository)
        {
            this.bookRepository = bookRepository;
            this.categoriesService = categoriesService;
            this.readerRepository = readerRepository;
            this.readerService = new ReaderService(readerRepository);
        }

        /// <summary>
        /// With this method you can create a new book.
        /// </summary>
        /// <param name="book"> The new book.</param>
        /// <returns>If book is created.</returns>
        public bool CreateBook(Book book)
        {
            if (book == null)
            {
                LoggerUtil.LogInfo($"The CreateBook method was called with a null book.");
                return false;
            }

            if (string.IsNullOrEmpty(book.Name))
            {
                LoggerUtil.LogInfo($"The CreateBook method was called with a book with null or empty name.");
                return false;
            }

            if (book.Name.Length < 3 || book.Name.Length > 80)
            {
                LoggerUtil.LogInfo($"The CreateBook method was called with a book with a invalid book name({book.Name}).");
                return false;
            }

            if (book.Name.Any(c => !(char.IsLetter(c) || char.IsWhiteSpace(c))))
            {
                LoggerUtil.LogInfo($"The CreateBook method was called with a book with a invalid book name({book.Name}).");
                return false;
            }

            if (book.Categories.IsNullOrEmpty())
            {
                LoggerUtil.LogInfo($"The CreateBook method was called with a book with no category.");
                return false;
            }

            if (book.Authors.IsNullOrEmpty())
            {
                LoggerUtil.LogInfo($"The CreateBook method was called with a book with no author.");

                return false;
            }

            if (book.Editions.IsNullOrEmpty())
            {
                LoggerUtil.LogInfo($"The CreateBook method was called with a book with no edition.");

                return false;
            }

            int dOM = int.Parse(ConfigurationManager.AppSettings["DOM"]);

            if (book.Categories.Count > dOM)
            {
                LoggerUtil.LogInfo($"Book {book.Name} had too many categories:{book.Categories.Count}, limit is {dOM}");
                return false;
            }

            foreach (var bookCategory in book.Categories)
            {
                var categories = book.Categories.ToList();
                categories.Remove(bookCategory);
                if (this.categoriesService.CategoryIsPartOfCategories(bookCategory, categories))
                {
                    return false;
                }
            }

            var addBook = this.bookRepository.AddBook(book);
            if (addBook)
            {
                LoggerUtil.LogInfo($"Successfully added book {book.Name} from author {book.Authors.First()} of edition {book.Editions.First()}");
            }

            return addBook;
        }

        public bool AddEdition(Book book, Edition edition)
        {
            if (book == null)
            {
                LoggerUtil.LogInfo($"The AddEdition method was called with a null book.");
                return false;
            }

            if (edition == null)
            {
                LoggerUtil.LogInfo($"The AddEdition method was called with a null edition.");
                return false;
            }

            if (edition.Name.IsNullOrEmpty())
            {
                LoggerUtil.LogInfo($"The Edition Name is null or empty.");
                return false;
            }

            if (edition.Name.Length < 3 || edition.Name.Length > 80)
            {
                LoggerUtil.LogInfo($"The Edition Name is invalid.");
                return false;
            }

            if (edition.Name.Any(c => !(char.IsLetterOrDigit(c) || char.IsWhiteSpace(c))))
            {
                LoggerUtil.LogInfo($"The Edition Name is invalid.");
                return false;
            }

            if (char.IsLower(edition.Name.First()))
            {
                LoggerUtil.LogInfo($"The Edition Name is started with lower case.");
                return false;
            }

            if (edition.BookType.IsNullOrEmpty())
            {
                LoggerUtil.LogInfo($"The Edition Book type is null or empty.");
                return false;
            }

            if (edition.BookType.Length < 3 || edition.BookType.Length > 80)
            {
                LoggerUtil.LogInfo($"The Edition Book Type is invalid.");
                return false;
            }

            if (edition.BookType.Any(c => !(char.IsLetter(c) || char.IsWhiteSpace(c))))
            {
                LoggerUtil.LogInfo($"The Edition Book Type is invalid.");
                return false;
            }

            if (char.IsLower(edition.BookType.First()))
            {
                LoggerUtil.LogInfo($"The Edition Book Type is started with lower case.");
                return false;
            }

            if (edition.Pages > 100000)
            {
                LoggerUtil.LogInfo($"The Edition have to many pages.");
                return false;
            }

            if (edition.Pages <= 0)
            {
                LoggerUtil.LogInfo($"The edition does not have enough pages.");
                return false;
            }

            var bookFromDb = this.bookRepository.GetBook(book);
            return this.bookRepository.AddEdition(bookFromDb, edition);
        }

        /// <summary>
        /// Borrow books.
        /// </summary>
        /// <param name="books"> The books list.</param>
        /// <param name="reader"> The reader.</param>
        /// <param name="employee"> The employee.</param>
        /// <param name="dateOfBorrowing"> The date of borrowing.</param>
        /// <returns>The bookwithdrawl.</returns>
        public BookWithdrawal BorrowBooks(List<Borrowing> books, Reader reader, Employee employee, DateTime dateOfBorrowing)
        {
            LoggerUtil.LogInfo($"Borrowing book {books.First().BookName}");

            foreach (var book in books)
            {
                if (!this.CanBorrowBook(book.BookName, book.EditionName))
                {
                    return null;
                }
            }

            var booksToBorrow = books.Select(b => this.bookRepository.GetBook(b.BookName)).ToList();
            if (!this.readerService.CanBorrowBooks(booksToBorrow, reader, employee, dateOfBorrowing))
            {
                return null;
            }

            var readerFromDb = this.readerService.GetReader(reader.Email, reader.Phone);
            var employeeFromDb = this.readerRepository.GetEmployee(employee);
            BookWithdrawal bw = null;
            if (employeeFromDb != null && readerFromDb != null)
            {
                bw = this.bookRepository.BorrowBooks(books, readerFromDb, employeeFromDb);
            }

            LoggerUtil.LogInfo(bw != null
                ? $"Borrowing book {books.First().BookName} completed successfully"
                : $"Borrowing book {books.First().BookName} failed");
            return bw;
        }

        /// <summary>
        /// Check if book can be borrow.
        /// </summary>
        /// <param name="bookName"> The book Name.</param>
        /// <param name="editionName">The edition Name.</param>
        /// <returns>If book can be borrow.</returns>
        public bool CanBorrowBook(string bookName, string editionName)
        {
            var edition = this.bookRepository.GetEdition(bookName, editionName);
            var bookStock = edition.BookStock;
            var amountOfBorrowedBooks = this.GetNumberOfBorrowedBooks(edition);
            float leftovers = bookStock.Amount - amountOfBorrowedBooks - bookStock.LectureRoomAmount - 1;

            return leftovers / bookStock.Amount > 0.1f;

        }

        /// <summary>
        /// Get Number of borrowed books.
        /// </summary>
        /// <param name="edition">Edition for the book.</param>
        /// <returns>Number of books.</returns>
        public int GetNumberOfBorrowedBooks(Edition edition)
        {
            var borrowedBooks = edition.BorrowedBooks;

            var bookWithdrawals = borrowedBooks.Select(b => b.BookWithdrawal);

            bookWithdrawals = bookWithdrawals.Where(bw => DateTime.Now.CompareTo(bw.Date) == 1);

            return bookWithdrawals.Count();
        }
    }
}
