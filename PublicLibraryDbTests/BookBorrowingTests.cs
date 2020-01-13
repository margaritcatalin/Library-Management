using System;
using System.Collections.Generic;
using System.Configuration;
using NUnit.Framework;
using PublicLibrary.BusinessLayer;
using PublicLibrary.Data_Mapper;
using PublicLibrary.Domain_Model;
using Assert = NUnit.Framework.Assert;

namespace PublicLibraryDbTests
{
    [TestFixture]
    public class BookBorrowingTests
    {

        private LibraryDb _libraryDb;
        private BookService _bookService;
        private Author _author;
        private Edition _edition;
        private BookStock _bookStock;
        private Category _category;
        private Employee _employee;
        private Reader _reader;
        private Book _book;
        private List<Book> _booksList;
        private ReaderService _readerService;

        [SetUp]
        public void SetUp()
        {
            _libraryDb = new LibraryDb();
            ReaderRepository readerRepository = new ReaderRepository(_libraryDb);
            EmployeeService employeeService = new EmployeeService(new EmployeeRepository(_libraryDb));
            _readerService = new ReaderService(readerRepository);
            _bookService = new BookService(new BookRepository(_libraryDb),new CategoriesService(new CategoriesRepository(_libraryDb)),readerRepository  );
            _author =new Author{Name = "Ioan Slavici"};
            _bookStock = new BookStock{Amount = 14,LectureRoomAmount = 10};
            var bookStock2 = new BookStock { Amount = 1000, LectureRoomAmount = 0 };
            var bookstock3 = new BookStock { Amount = 12,LectureRoomAmount = 10};
            _edition = new Edition { Name = "Teora", BookType = "Hardcover", Pages = 320, BookStock = _bookStock };
            var edition2 = new Edition { Name = "First Edition", BookType = "Hardcover", Pages = 320, BookStock = bookstock3 };
            _category = new Category{Name = "Nuvela"};

            _employee = new Employee()
            {
                FirstName = "Catalin",
                LastName = "Vola",
                Email = "catalin.V@yahoo.com",
                Phone = "7345345568",
                Address = "Florii nr.15",
            };
             _reader = new Reader
            {
                FirstName = "Al Alekku",
                LastName = "Alexandru",
                Email = "alexandru.v@yahoo.com",
                Phone = "1234567890",
                Address = "Str.Memorandului nr.4",
                Extensions = new List<Extension>()
            };
             _book = new Book
            {
                Name = "Moara cu noroc",
                Authors = new[] { _author },
                Editions = new[] { _edition },
                Categories = new[] { _category }
            };
             
             _readerService.AddReader(_reader);
             employeeService.AddEmployee(_employee);
             _bookService.CreateBook(_book);
             for (int i = 0; i < 25; i++)
             {
                 _bookService.CreateBook(new Book
                 {
                     Name = $"Moara cu noroc{(char) ('a' + i)}",
                     Authors = new[] {_author},
                     Editions = new[]
                         {new Edition {Name = $"Teora{(char) ('a' + i)}", BookType = "Hardcover", Pages = 320, BookStock = bookStock2},},
                     Categories = new[] {new Category{Name = $"noroc{(char) ('a' + i)}"}, }
                 });
             }
             for (int i = 0; i < 25; i++)
             {
                 _bookService.CreateBook(new Book
                 {
                     Name = $"Mara{(char)('a' + i)}",
                     Authors = new[] { _author },
                     Editions = new[]
                         {new Edition {Name = $"Teora{(char) ('a' + i)}", BookType = "Hardcover", Pages = 320, BookStock = bookStock2},},
                     Categories = new[] { _category }, 
                 });
             }

             _bookService.CreateBook(new Book
             {
                 Name = "Zana Zorilor",
                 Authors = new[] {_author},
                 Editions = new[] {edition2},
                 Categories = new[] {_category}
             });
        }

        [TearDown]
        public void Cleanup()
        {
            _libraryDb.Database.Delete();
        }

        [Test]
        public void TestBorrowOneBook()
        {
            var result = _bookService.BorrowBooks(
                new List<Borrowing> {new Borrowing{BookName = "Moara cu noroc",EditionName = "Teora"} }, _reader,
                _employee,DateTime.Now);

            Assert.True(result!=null);
        }
        [Test]
        public void TestBorrowOneBookNotInStock()
        {
            var result = _bookService.BorrowBooks(
                new List<Borrowing> { new Borrowing { BookName = "Zana Zorilor", EditionName = "First Edition" } }, _reader,
                _employee, DateTime.Now);

            Assert.False(result != null);
        }
        [Test]
        public void TestBorrowBooksWithinDelta()
        {
            var delta = int.Parse(ConfigurationManager.AppSettings["DELTA"]);

            var result1 = _bookService.BorrowBooks(
                new List<Borrowing> { new Borrowing { BookName = "Moara cu noroc", EditionName = "Teora" } }, _reader,
                _employee, DateTime.Now);
            var result2 = _bookService.BorrowBooks(
                new List<Borrowing> { new Borrowing { BookName = "Moara cu noroc", EditionName = "Teora" } }, _reader,
                _employee,DateTime.Now.AddDays(delta-1));

            Assert.True(result1!=null);
            Assert.False(result2 != null);
        }
        [Test]
        public void TestBorrowBooksOutsideDelta()
        {
            var delta = int.Parse(ConfigurationManager.AppSettings["DELTA"]);

            var result1 = _bookService.BorrowBooks(
                new List<Borrowing> { new Borrowing { BookName = "Moara cu noroc", EditionName = "Teora" } }, _reader,
                _employee, DateTime.Now);
            var result2 = _bookService.BorrowBooks(
                new List<Borrowing> { new Borrowing { BookName = "Moara cu noroc", EditionName = "Teora" } }, _reader,
                _employee, DateTime.Now.AddDays(delta+1));

            Assert.True(result1 != null);
            Assert.True(result2 != null);
        }
        [Test]
        public void TestBorrowBooksLessNMCPER()
        {
            var nmc = int.Parse(ConfigurationManager.AppSettings["NMC"]);
            //var per = int.Parse(ConfigurationManager.AppSettings["PER"]);

            for (int i = 0; i < nmc-1; i++)
            {
                var result = _bookService.BorrowBooks(
                    new List<Borrowing> { new Borrowing { BookName = $"Moara cu noroc{(char)('a' + i)}", EditionName = $"Teora{(char)('a' + i)}" } }, _reader,
                    _employee, DateTime.Now.AddDays(i));
                Assert.True(result != null);
            }
        }
        [Test]
        public void TestBorrowBooksMoreNMCPER()
        {
            var nmc = int.Parse(ConfigurationManager.AppSettings["NMC"]);
            //var per = int.Parse(ConfigurationManager.AppSettings["PER"]);

            for (int i = 0; i < nmc+1; i++)
            {
                var result = _bookService.BorrowBooks(
                    new List<Borrowing> { new Borrowing { BookName = $"Moara cu noroc{(char)('a' + i)}", EditionName = $"Teora{(char)('a' + i)}" } }, _reader,
                    _employee, DateTime.Now.AddDays(i));
                if (i != nmc)
                {
                    Assert.True(result != null);
                }
                else
                {
                    Assert.False(result != null);
                }
            }
        }
        [Test]
        public void TestBorrowBooksLessNCZ()
        {
            var ncz = int.Parse(ConfigurationManager.AppSettings["NCZ"]);

            for (int i = 0; i < ncz - 1; i++)
            {
                var result = _bookService.BorrowBooks(
                    new List<Borrowing> { new Borrowing { BookName = $"Moara cu noroc{(char)('a' + i)}", EditionName = $"Teora{(char)('a' + i)}" } }, _reader,
                    _employee, DateTime.Now);
                Assert.True(result != null);
            }
        }
        [Test]
        public void TestBorrowBooksMoreNCZ()
        {
            var ncz = int.Parse(ConfigurationManager.AppSettings["NCZ"]);

            for (int i = 0; i < ncz + 1; i++)
            {
                var result = _bookService.BorrowBooks(
                    new List<Borrowing> { new Borrowing { BookName = $"Moara cu noroc{(char)('a' + i)}", EditionName = $"Teora{(char)('a' + i)}" } }, _reader,
                    _employee, DateTime.Now);
                if (i != ncz)
                {
                    Assert.True(result != null);
                }
                else
                {
                    Assert.False(result != null);
                }
            }
        }

        [Test]
        public void TestBorrowLessCBooks()
        {
            var c = int.Parse(ConfigurationManager.AppSettings["C"]);
            var borrowList = new List<Borrowing>();
                
            for (int i = 0; i < c - 1; i++)
            {
                borrowList.Add(new Borrowing { BookName = $"Moara cu noroc{(char)('a' + i)}", EditionName = $"Teora{(char)('a' + i)}" });
            }
            var result = _bookService.BorrowBooks(
                borrowList, _reader,
                _employee, DateTime.Now);
            Assert.True(result != null);
        }
        [Test]
        public void TestBorrowMoreCBooks()
        {
            var c = int.Parse(ConfigurationManager.AppSettings["C"]);
            var borrowList = new List<Borrowing>();

            for (int i = 0; i < c + 1; i++)
            {
                borrowList.Add(new Borrowing { BookName = $"Moara cu noroc{(char)('a' + i)}", EditionName = $"Teora{(char)('a' + i)}" });
            }
            var result = _bookService.BorrowBooks(
                borrowList, _reader,
                _employee, DateTime.Now);
            Assert.False(result != null);
        }
        [Test]
        public void TestBorrowBooksLessDL()
        {
            var d = int.Parse(ConfigurationManager.AppSettings["D"]);
            //var l = int.Parse(ConfigurationManager.AppSettings["L"]);

            for (int i = 0; i < d - 1; i++)
            {
                var result = _bookService.BorrowBooks(
                    new List<Borrowing> { new Borrowing { BookName = $"Mara{(char)('a' + i)}", EditionName = $"Teora{(char)('a' + i)}" } }, _reader,
                    _employee, DateTime.Now.AddDays(i*7));
                Assert.True(result != null);
            }
        }
        [Test]
        public void TestBorrowBooksMoreDL()
        {
            var d = int.Parse(ConfigurationManager.AppSettings["D"]);
            //var l = int.Parse(ConfigurationManager.AppSettings["L"]);

            for (int i = 0; i < d + 1; i++)
            {
                var result = _bookService.BorrowBooks(
                    new List<Borrowing> { new Borrowing { BookName = $"Mara{(char)('a' + i)}", EditionName = $"Teora{(char)('a' + i)}" } }, _reader,
                    _employee, DateTime.Now.AddDays(i * 7));
                if (i != d)
                {
                    Assert.True(result != null);
                }
                else
                {
                    Assert.False(result != null);
                }
            }
        }

        [Test]
        public void TestAddExtensionsLessThanLim()
        {
            var lim = int.Parse(ConfigurationManager.AppSettings["LIM"]);

            var bw = _bookService.BorrowBooks(
                new List<Borrowing> { new Borrowing { BookName = "Moara cu noroc", EditionName = "Teora" } }, _reader,
                _employee, DateTime.Now);

            for(int i = 0; i <= lim; i++)
            {
                var result = _readerService.AddExtension(_reader, bw);
                Assert.True(result);
            }
        }
        [Test]
        public void TestAddExtensionsMoreThanLim()
        {
            var lim = int.Parse(ConfigurationManager.AppSettings["LIM"]);

            var bw = _bookService.BorrowBooks(
                new List<Borrowing> { new Borrowing { BookName = "Moara cu noroc", EditionName = "Teora" } }, _reader,
                _employee, DateTime.Now);

            for (int i = 0; i <= lim+1; i++)
            {
                var result = _readerService.AddExtension(_reader, bw);
                if (i != lim+1)
                {
                    Assert.True(result);
                }
                else
                {
                    Assert.False(result );
                }
            }
        }
    }
}
