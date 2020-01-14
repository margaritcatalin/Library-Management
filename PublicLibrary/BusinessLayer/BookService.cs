// <copyright file="BookService.cs" company="Transilvania University of Brasov">
// Margarit Marian Catalin
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
    /// The book service.
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
        /// <param name="bookRepository">The book repository.</param>
        /// <param name="categoriesService">The categories service.</param>
        /// <param name="readerRepository">The reader repository.</param>
        public BookService(
            BookRepository bookRepository,
            CategoriesService categoriesService,
            ReaderRepository readerRepository)
        {
            this.bookRepository = bookRepository;
            this.categoriesService = categoriesService;
            this.readerRepository = readerRepository;
            this.readerService = new ReaderService(this.readerRepository);
        }

        /// <summary>
        /// Add a new book.
        /// </summary>
        /// <param name="book">The book.</param>
        /// <returns>If book was created.</returns>
        public bool CreateBook(Book book)
        {
            if (book == null)
            {
                LoggerUtil.LogInfo($"Your book is invalid. Book is null.");
                return false;
            }

            if (string.IsNullOrEmpty(book.Name))
            {
                LoggerUtil.LogInfo($"Your book is invalid. Book name is null or empty.");
                return false;
            }

            if ((book.Name.Length < 3) || (book.Name.Length > 80))
            {
                LoggerUtil.LogInfo($"Your book is invalid. Param book name has an invalid length.");
                return false;
            }

            if (book.Name.Any(c => !(char.IsLetter(c) || char.IsWhiteSpace(c))))
            {
                LoggerUtil.LogInfo($"Your book is invalid. Param book name is invalid.");
                return false;
            }

            if (book.Categories.IsNullOrEmpty())
            {
                LoggerUtil.LogInfo($"Your book is invalid. Param categories is null or empty.");
                return false;
            }

            if (book.Authors.IsNullOrEmpty())
            {
                LoggerUtil.LogInfo($"Your book is invalid. Param author is null or empty.");
                return false;
            }

            if (book.Editions.IsNullOrEmpty())
            {
                LoggerUtil.LogInfo($"Your book is invalid. Param Edition is null or empty.");
                return false;
            }

            var dOM = int.Parse(ConfigurationManager.AppSettings["DOM"]);

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
                    LoggerUtil.LogInfo($"Your book has a invalid category.");
                    return false;
                }
            }

            var addBook = this.bookRepository.AddBook(book);
            if (addBook)
            {
                LoggerUtil.LogInfo(
                    $"Successfully added book {book.Name} from author {book.Authors.First()} of edition {book.Editions.First()}");
            }

            return addBook;
        }

        /// <summary>
        /// Add a new edition.
        /// </summary>
        /// <param name="book">The book.</param>
        /// <param name="edition">The edition.</param>
        /// <returns>If edition was added.</returns>
        public bool AddEdition(Book book, Edition edition)
        {
            if (book == null)
            {
                LoggerUtil.LogInfo($"Your book is invalid. Book is null.");
                return false;
            }

            if (edition == null)
            {
                LoggerUtil.LogInfo($"Your edition is invalid. Edition is null.");
                return false;
            }

            if (edition.Name.IsNullOrEmpty())
            {
                LoggerUtil.LogInfo($"Your edition is invalid. Edition name is null or empty.");
                return false;
            }

            if ((edition.Name.Length < 3) || (edition.Name.Length > 80))
            {
                LoggerUtil.LogInfo($"Your edition is invalid. You tried to add an edition with invalid length.");
                return false;
            }

            if (edition.Name.Any(c => !(char.IsLetterOrDigit(c) || char.IsWhiteSpace(c))))
            {
                LoggerUtil.LogInfo($"Your edition is invalid. Your edition name is invalid.");
                return false;
            }

            if (char.IsLower(edition.Name.First()))
            {
                LoggerUtil.LogInfo($"Your edition is invalid. Your edition name is need to start with upper case.");
                return false;
            }

            if (edition.BookType.IsNullOrEmpty())
            {
                LoggerUtil.LogInfo($"Your edition is invalid. Your tried to create an edition with null or empty booktype.");
                return false;
            }

            if ((edition.BookType.Length < 3) || (edition.BookType.Length > 80))
            {
                LoggerUtil.LogInfo($"Your edition is invalid. Your booktype length is invalid.");
                return false;
            }

            if (edition.BookType.Any(c => !(char.IsLetter(c) || char.IsWhiteSpace(c))))
            {
                LoggerUtil.LogInfo($"Your edition is invalid. You tried to add an edition with invalid booktype.");
                return false;
            }

            if (char.IsLower(edition.BookType.First()))
            {
                LoggerUtil.LogInfo($"Your edition is invalid. The book type is need to start with uppercase.");
                return false;
            }

            if (edition.Pages > 100000)
            {
                LoggerUtil.LogInfo($"Your edition is invalid. You tried to add an edition with too many pages.");
                return false;
            }

            if (edition.Pages <= 0)
            {
                LoggerUtil.LogInfo($"Your edition is invalid. You tried to add an edition with no pages.");
                return false;
            }

            var bookFromDb = this.bookRepository.GetBook(book);
            return this.bookRepository.AddEdition(bookFromDb, edition);
        }

        /// <summary>
        /// Borrow books.
        /// </summary>
        /// <param name="books">Books list.</param>
        /// <param name="reader">The reader.</param>
        /// <param name="employee">The employee.</param>
        /// <param name="dateOfBorrowing">Date of borrowing.</param>
        /// <returns>A bookwihdrawl.</returns>
        public BookWithdrawal BorrowBooks(
            List<Borrowing> books,
            Reader reader,
            Employee employee,
            DateTime dateOfBorrowing)
        {
            LoggerUtil.LogInfo($"Borrowing book {books.First().BookName}");

            foreach (var book in books)
            {
                if (!this.CanBorrowBook(book.BookName, book.EditionName))
                {
                    LoggerUtil.LogInfo($"{book.BookName} with edition {book.EditionName} can not be borrow.");
                    return null;
                }
            }

            var booksToBorrow = books.Select(b => this.bookRepository.GetBook(b.BookName)).ToList();
            if (!this.readerService.CanBorrowBooks(booksToBorrow, reader, employee, dateOfBorrowing))
            {
                LoggerUtil.LogInfo($"{reader.FirstName} can not borrow a new book.");
                return null;
            }

            var readerFromDb = this.readerService.GetReader(reader.Email, reader.Phone);
            var employeeFromDb = this.readerRepository.GetEmployee(employee);
            BookWithdrawal bw = null;
            if ((employeeFromDb != null) && (readerFromDb != null))
            {
                bw = this.bookRepository.BorrowBooks(books, readerFromDb, employeeFromDb);
            }

            LoggerUtil.LogInfo(
                bw != null
                    ? $"Borrowing book {books.First().BookName} completed successfully"
                    : $"Borrowing book {books.First().BookName} failed");
            return bw;
        }

        /// <summary>
        /// Check if you can borrow a book.
        /// </summary>
        /// <param name="bookName">The book name.</param>
        /// <param name="editionName">The edition name.</param>
        /// <returns>If you can borrow the book.</returns>
        public bool CanBorrowBook(string bookName, string editionName)
        {
            var edition = this.bookRepository.GetEdition(bookName, editionName);
            var bookStock = edition.BookStock;
            var amountOfBorrowedBooks = this.GetNumberOfBorrowedBooks(edition);
            float leftovers = bookStock.Amount - amountOfBorrowedBooks - bookStock.LectureRoomAmount - 1;

            return leftovers / bookStock.Amount > 0.1f;
        }

        /// <summary>
        /// Get number of borrowed books.
        /// </summary>
        /// <param name="edition">The edition.</param>
        /// <returns>Number of borrowed books.</returns>
        public int GetNumberOfBorrowedBooks(Edition edition)
        {
            var borrowedBooks = edition.BorrowedBooks;

            var bookWithdrawals = borrowedBooks.Select(b => b.BookWithdrawal);

            bookWithdrawals = bookWithdrawals.Where(bw => DateTime.Now.CompareTo(bw.Date) == 1);

            return bookWithdrawals.Count();
        }

        /// <summary>
        /// Get edition by book name and edition name.
        /// </summary>
        /// <param name="bookName">The bookName.</param>
        /// <param name="editionName">The editionName.</param>
        /// <returns>An edition.</returns>
        public Edition GetEdition(string bookName, string editionName)
        {
            if (bookName.IsNullOrEmpty() || editionName.IsNullOrEmpty())
            {
                LoggerUtil.LogInfo($"Params bookName and editionName is required.");
                return null;
            }

            return this.bookRepository.GetEdition(bookName, editionName);
        }

        /// <summary>
        /// Get book by name.
        /// </summary>
        /// <param name="name">The book name.</param>
        /// <returns>A book.</returns>
        public Book GetBook(string name)
        {
            if (name.IsNullOrEmpty())
            {
                LoggerUtil.LogInfo($"Param name is required.");
                return null;
            }

            return this.bookRepository.GetBook(name);
        }
    }
}