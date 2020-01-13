using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using Castle.Core.Internal;
using PublicLibrary.Data_Mapper;
using PublicLibrary.Domain_Model;

namespace PublicLibrary.BusinessLayer
{
    public class BookService
    {
        private readonly BookRepository _bookRepository;

        private readonly CategoriesService _categoriesService;

        private readonly ReaderRepository _readerRepository;
        private readonly ReaderService _readerService;
        public BookService(BookRepository bookRepository, CategoriesService categoriesService, ReaderRepository readerRepository)
        {
            this._bookRepository = bookRepository;
            this._categoriesService = categoriesService;
            this._readerRepository = readerRepository;
            this._readerService = new ReaderService(_readerRepository);
        }

        public bool CreateBook(Book book)
        {
            //Vali pune logguri
            if (book == null) return false;
            if (string.IsNullOrEmpty(book.Name)) return false;
            if (book.Name.Length < 3 || book.Name.Length > 80) return false;
            if (book.Name.Any(c => !(char.IsLetter(c) || char.IsWhiteSpace(c)))) return false;
            if (book.Categories.IsNullOrEmpty()) return false;
            if (book.Authors.IsNullOrEmpty()) return false;
            if (book.Editions.IsNullOrEmpty()) return false;
            

            var DOM = int.Parse(ConfigurationManager.AppSettings["DOM"]);

            if (book.Categories.Count>DOM)
            {
                LoggerUtil.LogInfo($"Book {book.Name} had too many categories:{book.Categories.Count}, limit is {DOM}");
                return false;
            }

            foreach (var bookCategory in book.Categories)
            {
                var categories = book.Categories.ToList();
                categories.Remove(bookCategory);
                if (_categoriesService.CategoryIsPartOfCategories(bookCategory, categories))
                {
                    return false;
                }
            }

            var addBook = _bookRepository.AddBook(book);
            if (addBook)
            {
                LoggerUtil.LogInfo($"Successfully added book {book.Name} from author {book.Authors.First()} of edition {book.Editions.First()}");
            }

            return addBook;
        }

        public bool AddEdition(Book book, Edition edition)
        {
            if (book == null) return false;
            if (edition == null) return false;
            if (edition.Name.IsNullOrEmpty()) return false;
            if (edition.Name.Length < 3 || edition.Name.Length > 80) return false;
            if (edition.Name.Any(c => !(char.IsLetterOrDigit(c) || char.IsWhiteSpace(c)))) return false;
            if (char.IsLower(edition.Name.First())) return false;


            if (edition.BookType.IsNullOrEmpty()) return false;
            if (edition.BookType.Length < 3 || edition.BookType.Length > 80) return false;
            if (edition.BookType.Any(c => !(char.IsLetter(c) || char.IsWhiteSpace(c)))) return false;
            if (char.IsLower(edition.BookType.First())) return false;

            if (edition.Pages > 100000) return false;
            if (edition.Pages <= 0) return false;

            var bookFromDb = _bookRepository.GetBook(book);
            return _bookRepository.AddEdition(bookFromDb, edition);
        }

        

        public BookWithdrawal BorrowBooks(List<Borrowing> books, Reader reader,Employee employee,DateTime dateOfBorrowing)
        {
            LoggerUtil.LogInfo($"Borrowing book {books.First().BookName}");

            foreach (var book in books)
            {
                if (!CanBorrowBook(book.BookName, book.EditionName)) return null;
            }

            var booksToBorrow = books.Select(b => _bookRepository.GetBook(b.BookName)).ToList();
            if(!_readerService.CanBorrowBooks(booksToBorrow, reader,employee, dateOfBorrowing)) return null;

            var readerFromDb = _readerService.GetReader(reader.Email,reader.Phone);
            var employeeFromDb = _readerRepository.GetEmployee(employee);
            BookWithdrawal bw = null;
            if (employeeFromDb != null && readerFromDb != null)
            {
                bw = _bookRepository.BorrowBooks(books, readerFromDb, employeeFromDb);
            }

            LoggerUtil.LogInfo(bw!=null
                ? $"Borrowing book {books.First().BookName} completed successfully"
                : $"Borrowing book {books.First().BookName} failed");
            return bw;
        }

        public bool CanBorrowBook(string bookName, string editionName)
        {
            var edition = _bookRepository.GetEdition(bookName, editionName);
            var bookStock = edition.BookStock;
            var amountOfBorrowedBooks = GetNumberOfBorrowedBooks(edition);
            float leftovers = bookStock.Amount - amountOfBorrowedBooks - bookStock.LectureRoomAmount - 1;

            return leftovers / bookStock.Amount > 0.1f;

        }

        public int GetNumberOfBorrowedBooks(Edition edition)
        {
            var borrowedBooks = edition.BorrowedBooks;

            var bookWithdrawals = borrowedBooks.Select(b => b.BookWithdrawal);

            bookWithdrawals = bookWithdrawals.Where(bw => DateTime.Now.CompareTo(bw.Date) == 1);

            return bookWithdrawals.Count();
        }
        
    }
}
