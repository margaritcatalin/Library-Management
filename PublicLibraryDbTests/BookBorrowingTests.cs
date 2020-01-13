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

        private LibraryDb libraryDb;
        private BookService bookService;
        private Author author;
        private Edition edition;
        private BookStock bookStock;
        private Category category;
        private Employee employee;
        private Reader reader;
        private Book book;
        private ReaderService readerService;

        [SetUp]
        public void SetUp()
        {
            libraryDb = new LibraryDb();
            ReaderRepository readerRepository = new ReaderRepository(libraryDb);
            EmployeeService employeeService = new EmployeeService(new EmployeeRepository(libraryDb));
            readerService = new ReaderService(readerRepository);
            bookService = new BookService(new BookRepository(libraryDb),new CategoriesService(new CategoriesRepository(libraryDb)),readerRepository  );
            author =new Author{Name = "Ioan Slavici"};
            bookStock = new BookStock{Amount = 14,LectureRoomAmount = 10};
            var bookStock2 = new BookStock { Amount = 1000, LectureRoomAmount = 0 };
            var bookstock3 = new BookStock { Amount = 12,LectureRoomAmount = 10};
            edition = new Edition { Name = "Teora", BookType = "Hardcover", Pages = 320, BookStock = bookStock };
            var edition2 = new Edition { Name = "First Edition", BookType = "Hardcover", Pages = 320, BookStock = bookstock3 };
            category = new Category{Name = "Nuvela"};

            employee = new Employee()
            {
                FirstName = "Catalin",
                LastName = "Vola",
                Email = "catalin.V@yahoo.com",
                Phone = "7345345568",
                Address = "Florii nr.15",
            };
             reader = new Reader
            {
                FirstName = "Al Alekku",
                LastName = "Alexandru",
                Email = "alexandru.v@yahoo.com",
                Phone = "1234567890",
                Address = "Str.Memorandului nr.4",
                Extensions = new List<Extension>()
            };
             book = new Book
            {
                Name = "Moara cu noroc",
                Authors = new[] { author },
                Editions = new[] { edition },
                Categories = new[] { category }
            };
             
             readerService.AddReader(reader);
             employeeService.AddEmployee(employee);
             bookService.CreateBook(book);
             for (int i = 0; i < 25; i++)
             {
                 bookService.CreateBook(new Book
                 {
                     Name = $"Moara cu noroc{(char) ('a' + i)}",
                     Authors = new[] {author},
                     Editions = new[]
                         {new Edition {Name = $"Teora{(char) ('a' + i)}", BookType = "Hardcover", Pages = 320, BookStock = bookStock2},},
                     Categories = new[] {new Category{Name = $"noroc{(char) ('a' + i)}"}, }
                 });
             }
             for (int i = 0; i < 25; i++)
             {
                 bookService.CreateBook(new Book
                 {
                     Name = $"Mara{(char)('a' + i)}",
                     Authors = new[] { author },
                     Editions = new[]
                         {new Edition {Name = $"Teora{(char) ('a' + i)}", BookType = "Hardcover", Pages = 320, BookStock = bookStock2},},
                     Categories = new[] { category }, 
                 });
             }

             bookService.CreateBook(new Book
             {
                 Name = "Zana Zorilor",
                 Authors = new[] {author},
                 Editions = new[] {edition2},
                 Categories = new[] {category}
             });
        }

        [TearDown]
        public void Cleanup()
        {
            libraryDb.Database.Delete();
        }

        [Test]
        public void TestBorrowOneBook()
        {
            var result = bookService.BorrowBooks(
                new List<Borrowing> {new Borrowing{BookName = "Moara cu noroc",EditionName = "Teora"} }, reader,
                employee,DateTime.Now);

            Assert.True(result!=null);
        }
        [Test]
        public void TestBorrowOneBookNotInStock()
        {
            var result = bookService.BorrowBooks(
                new List<Borrowing> { new Borrowing { BookName = "Zana Zorilor", EditionName = "First Edition" } }, reader,
                employee, DateTime.Now);

            Assert.False(result != null);
        }
        [Test]
        public void TestBorrowBooksWithinDelta()
        {
            var delta = int.Parse(ConfigurationManager.AppSettings["DELTA"]);

            var result1 = bookService.BorrowBooks(
                new List<Borrowing> { new Borrowing { BookName = "Moara cu noroc", EditionName = "Teora" } }, reader,
                employee, DateTime.Now);
            var result2 = bookService.BorrowBooks(
                new List<Borrowing> { new Borrowing { BookName = "Moara cu noroc", EditionName = "Teora" } }, reader,
                employee,DateTime.Now.AddDays(delta-1));

            Assert.True(result1!=null);
            Assert.False(result2 != null);
        }
        [Test]
        public void TestBorrowBooksOutsideDelta()
        {
            var delta = int.Parse(ConfigurationManager.AppSettings["DELTA"]);

            var result1 = bookService.BorrowBooks(
                new List<Borrowing> { new Borrowing { BookName = "Moara cu noroc", EditionName = "Teora" } }, reader,
                employee, DateTime.Now);
            var result2 = bookService.BorrowBooks(
                new List<Borrowing> { new Borrowing { BookName = "Moara cu noroc", EditionName = "Teora" } }, reader,
                employee, DateTime.Now.AddDays(delta+1));

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
                var result = bookService.BorrowBooks(
                    new List<Borrowing> { new Borrowing { BookName = $"Moara cu noroc{(char)('a' + i)}", EditionName = $"Teora{(char)('a' + i)}" } }, reader,
                    employee, DateTime.Now.AddDays(i));
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
                var result = bookService.BorrowBooks(
                    new List<Borrowing> { new Borrowing { BookName = $"Moara cu noroc{(char)('a' + i)}", EditionName = $"Teora{(char)('a' + i)}" } }, reader,
                    employee, DateTime.Now.AddDays(i));
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
                var result = bookService.BorrowBooks(
                    new List<Borrowing> { new Borrowing { BookName = $"Moara cu noroc{(char)('a' + i)}", EditionName = $"Teora{(char)('a' + i)}" } }, reader,
                    employee, DateTime.Now);
                Assert.True(result != null);
            }
        }
        [Test]
        public void TestBorrowBooksMoreNCZ()
        {
            var ncz = int.Parse(ConfigurationManager.AppSettings["NCZ"]);

            for (int i = 0; i < ncz + 1; i++)
            {
                var result = bookService.BorrowBooks(
                    new List<Borrowing> { new Borrowing { BookName = $"Moara cu noroc{(char)('a' + i)}", EditionName = $"Teora{(char)('a' + i)}" } }, reader,
                    employee, DateTime.Now);
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
            var result = bookService.BorrowBooks(
                borrowList, reader,
                employee, DateTime.Now);
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
            var result = bookService.BorrowBooks(
                borrowList, reader,
                employee, DateTime.Now);
            Assert.False(result != null);
        }
        [Test]
        public void TestBorrowBooksLessDL()
        {
            var d = int.Parse(ConfigurationManager.AppSettings["D"]);
            //var l = int.Parse(ConfigurationManager.AppSettings["L"]);

            for (int i = 0; i < d - 1; i++)
            {
                var result = bookService.BorrowBooks(
                    new List<Borrowing> { new Borrowing { BookName = $"Mara{(char)('a' + i)}", EditionName = $"Teora{(char)('a' + i)}" } }, reader,
                    employee, DateTime.Now.AddDays(i*7));
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
                var result = bookService.BorrowBooks(
                    new List<Borrowing> { new Borrowing { BookName = $"Mara{(char)('a' + i)}", EditionName = $"Teora{(char)('a' + i)}" } }, reader,
                    employee, DateTime.Now.AddDays(i * 7));
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

            var bw = bookService.BorrowBooks(
                new List<Borrowing> { new Borrowing { BookName = "Moara cu noroc", EditionName = "Teora" } }, reader,
                employee, DateTime.Now);

            for(int i = 0; i <= lim; i++)
            {
                var result = readerService.AddExtension(reader, bw);
                Assert.True(result);
            }
        }
        [Test]
        public void TestAddExtensionsMoreThanLim()
        {
            var lim = int.Parse(ConfigurationManager.AppSettings["LIM"]);

            var bw = bookService.BorrowBooks(
                new List<Borrowing> { new Borrowing { BookName = "Moara cu noroc", EditionName = "Teora" } }, reader,
                employee, DateTime.Now);

            for (int i = 0; i <= lim+1; i++)
            {
                var result = readerService.AddExtension(reader, bw);
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
