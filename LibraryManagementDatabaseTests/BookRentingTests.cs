// <copyright file="BookRentTests.cs" company="Transilvania University of Brasov">
// Margarit Marian Catalin
// </copyright>

namespace LibraryManagementDatabaseTests
{
    using System;
    using System.Collections.Generic;
    using System.Configuration;
    using NUnit.Framework;
    using LibraryManagement.BusinessLayer;
    using LibraryManagement.DataMapper;
    using LibraryManagement.DomainModel;
    using Assert = NUnit.Framework.Assert;

    /// <summary>
    /// BookRent tests.
    /// </summary>
    [TestFixture]
    public class BookRentTests
    {
        private LibraryDbContext libraryContext;

        private BookService bookService;

        private Author defaultTestAuthor;

        private Edition defaultTestEdition;

        private BookStock bookStock;

        private Domain defaultTestDomain;

        private Librarian librarian;

        private Reader reader;

        private Book defaultTestBook;

        private ReaderService readerService;

        /// <summary>
        /// Tests setup.
        /// </summary>
        [SetUp]
        public void SetUp()
        {
            this.libraryContext = new LibraryDbContext();
            var readerRepository = new ReaderRepository(this.libraryContext);
            var librarianService = new LibrarianService(new LibrarianRepository(this.libraryContext));
            this.readerService = new ReaderService(readerRepository);
            this.bookService = new BookService(
                new BookRepository(this.libraryContext),
                new DomainsService(new DomainsRepository(this.libraryContext)),
                readerRepository);
            this.defaultTestAuthor = new Author { FirstName = "Estera", LastName = "Balas" };
            this.bookStock = new BookStock { Amount = 14, LectureRoomAmount = 10 };
            var bookStock2 = new BookStock { Amount = 1000, LectureRoomAmount = 0 };
            var bookstock3 = new BookStock { Amount = 12, LectureRoomAmount = 10 };
            this.defaultTestEdition = new Edition
                            {
                                Name = "Corint", BookType = "Plasticcover", Pages = 320, BookStock = this.bookStock,
                            };
            var edition2 = new Edition
                           {
                               Name = "Ultimate Edition", BookType = "Plasticcover", Pages = 320, BookStock = bookstock3,
                           };
            this.defaultTestDomain = new Domain { Name = "Novel" };

            this.librarian = new Librarian()
                             {
                                 FirstName = "Catalin",
                                 LastName = "Marcus",
                                 Email = "catalin.V@yahoo.com",
                                 Phone = "0765477898",
                                 Address = "Florii nr.15",
                                 Gender = "M",
                             };
            this.reader = new Reader
                           {
                               FirstName = "Aly Baba",
                               LastName = "Ionescu",
                               Email = "marcu.ionel@gmail.com",
                               Phone = "1234567890",
                               Address = "Str.Camil Petrescu nr.23",
                               Extensions = new List<Extension>(),
                               Gender = "M",
                           };
            this.defaultTestBook = new Book
                         {
                             Name = "Java for junior",
                             Authors = new[] { this.defaultTestAuthor },
                             Editions = new[] { this.defaultTestEdition },
                             Categories = new[] { this.defaultTestDomain },
                         };

            this.readerService.AddReader(this.reader);
            librarianService.AddLibrarian(this.librarian);
            this.bookService.CreateBook(this.defaultTestBook);
            for (var i = 0; i < 25; i++)
            {
                this.bookService.CreateBook(
                    new Book
                    {
                        Name = $"Java for junior{(char)('a' + i)}",
                        Authors = new[] { this.defaultTestAuthor },
                        Editions = new[]
                                   {
                                       new Edition
                                       {
                                           Name = $"Corint{(char)('a' + i)}",
                                           BookType = "Plasticcover",
                                           Pages = 320,
                                           BookStock = bookStock2,
                                       },
                                   },
                        Categories = new[] { new Domain { Name = $"noroc{(char)('a' + i)}" }, },
                    });
            }

            for (var i = 0; i < 25; i++)
            {
                this.bookService.CreateBook(
                    new Book
                    {
                        Name = $"SQL Server{(char)('a' + i)}",
                        Authors = new[] { this.defaultTestAuthor },
                        Editions = new[]
                                   {
                                       new Edition
                                       {
                                           Name = $"Corint{(char)('a' + i)}",
                                           BookType = "Plasticcover",
                                           Pages = 320,
                                           BookStock = bookStock2,
                                       },
                                   },
                        Categories = new[] { this.defaultTestDomain },
                    });
            }

            this.bookService.CreateBook(
                new Book
                {
                    Name = "Testing is important",
                    Authors = new[] { this.defaultTestAuthor },
                    Editions = new[] { edition2 },
                    Categories = new[] { this.defaultTestDomain },
                });
        }

        /// <summary>
        /// Clean database.
        /// </summary>
        [TearDown]
        public void Cleanup()
        {
            this.libraryContext.Database.Delete();
        }

        /// <summary>
        /// Test Rent one book.
        /// </summary>
        [Test]
        public void TestRentOneBook()
        {
            var result = this.bookService.RentBooks(
                new List<Rent> { new Rent { BookName = "Java for junior", EditionName = "Corint" } },
                this.reader,
                this.librarian,
                DateTime.Now);

            Assert.True(result != null);
        }

        /// <summary>
        /// Test Rent one book with is not in stock.
        /// </summary>
        [Test]
        public void TestRentOneBookNotInStock()
        {
            var result = this.bookService.RentBooks(
                new List<Rent> { new Rent { BookName = "Testing is important", EditionName = "Ultimate Edition" } },
                this.reader,
                this.librarian,
                DateTime.Now);

            Assert.False(result != null);
        }

        /// <summary>
        /// Test borrow books in delta.
        /// </summary>
        [Test]
        public void TestRentBooksWithinDelta()
        {
            var delta = int.Parse(ConfigurationManager.AppSettings["DELTA"]);

            var result1 = this.bookService.RentBooks(
                new List<Rent> { new Rent { BookName = "Java for junior", EditionName = "Corint" } },
                this.reader,
                this.librarian,
                DateTime.Now);
            var result2 = this.bookService.RentBooks(
                new List<Rent> { new Rent { BookName = "Java for junior", EditionName = "Corint" } },
                this.reader,
                this.librarian,
                DateTime.Now.AddDays(delta - 1));

            Assert.True(result1 != null);
            Assert.False(result2 != null);
        }

        /// <summary>
        /// Test borrow book in outside delta.
        /// </summary>
        [Test]
        public void TestRentBooksOutsideDelta()
        {
            var delta = int.Parse(ConfigurationManager.AppSettings["DELTA"]);

            var result1 = this.bookService.RentBooks(
                new List<Rent> { new Rent { BookName = "Java for junior", EditionName = "Corint" } },
                this.reader,
                this.librarian,
                DateTime.Now);
            var result2 = this.bookService.RentBooks(
                new List<Rent> { new Rent { BookName = "Java for junior", EditionName = "Corint" } },
                this.reader,
                this.librarian,
                DateTime.Now.AddDays(delta + 1));

            Assert.True(result1 != null);
            Assert.True(result2 != null);
        }

        /// <summary>
        /// Test borrow books in less NMCPER.
        /// </summary>
        [Test]
        public void TestRentBooksLessNMCPER()
        {
            var nmc = int.Parse(ConfigurationManager.AppSettings["NMC"]);

            for (var i = 0; i < nmc - 1; i++)
            {
                var result = this.bookService.RentBooks(
                    new List<Rent>
                    {
                        new Rent
                        {
                            BookName = $"Java for junior{(char)('a' + i)}", EditionName = $"Corint{(char)('a' + i)}",
                        },
                    },
                    this.reader,
                    this.librarian,
                    DateTime.Now.AddDays(i));
                Assert.True(result != null);
            }
        }

        /// <summary>
        /// Test borrow books more NMCPER.
        /// </summary>
        [Test]
        public void TestRentBooksMoreNMCPER()
        {
            var nmc = int.Parse(ConfigurationManager.AppSettings["NMC"]);

            for (var i = 0; i < nmc + 1; i++)
            {
                var result = this.bookService.RentBooks(
                    new List<Rent>
                    {
                        new Rent
                        {
                            BookName = $"Java for junior{(char)('a' + i)}", EditionName = $"Corint{(char)('a' + i)}",
                        },
                    },
                    this.reader,
                    this.librarian,
                    DateTime.Now.AddDays(i));
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

        /// <summary>
        /// Test borrow books in less NCZ.
        /// </summary>
        [Test]
        public void TestRentBooksLessNCZ()
        {
            var ncz = int.Parse(ConfigurationManager.AppSettings["NCZ"]);

            for (var i = 0; i < ncz - 1; i++)
            {
                var result = this.bookService.RentBooks(
                    new List<Rent>
                    {
                        new Rent
                        {
                            BookName = $"Java for junior{(char)('a' + i)}", EditionName = $"Corint{(char)('a' + i)}",
                        },
                    },
                    this.reader,
                    this.librarian,
                    DateTime.Now);
                Assert.True(result != null);
            }
        }

        /// <summary>
        /// Test borrow books with more ncz.
        /// </summary>
        [Test]
        public void TestRentBooksMoreNCZ()
        {
            var ncz = int.Parse(ConfigurationManager.AppSettings["NCZ"]);

            for (var i = 0; i < ncz + 1; i++)
            {
                var result = this.bookService.RentBooks(
                    new List<Rent>
                    {
                        new Rent
                        {
                            BookName = $"Java for junior{(char)('a' + i)}", EditionName = $"Corint{(char)('a' + i)}",
                        },
                    },
                    this.reader,
                    this.librarian,
                    DateTime.Now);
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

        /// <summary>
        /// Test borrow less c books.
        /// </summary>
        [Test]
        public void TestRentLessCBooks()
        {
            var c = int.Parse(ConfigurationManager.AppSettings["C"]);
            var borrowList = new List<Rent>();

            for (var i = 0; i < c - 1; i++)
            {
                borrowList.Add(
                    new Rent
                    {
                        BookName = $"Java for junior{(char)('a' + i)}", EditionName = $"Corint{(char)('a' + i)}",
                    });
            }

            var result = this.bookService.RentBooks(borrowList, this.reader, this.librarian, DateTime.Now);
            Assert.True(result != null);
        }

        /// <summary>
        /// Test borrow more c books.
        /// </summary>
        [Test]
        public void TestRentMoreCBooks()
        {
            var c = int.Parse(ConfigurationManager.AppSettings["C"]);
            var borrowList = new List<Rent>();

            for (var i = 0; i < c + 1; i++)
            {
                borrowList.Add(
                    new Rent
                    {
                        BookName = $"Java for junior{(char)('a' + i)}", EditionName = $"Corint{(char)('a' + i)}",
                    });
            }

            var result = this.bookService.RentBooks(borrowList, this.reader, this.librarian, DateTime.Now);
            Assert.False(result != null);
        }

        /// <summary>
        /// Test borrow books with less DL.
        /// </summary>
        [Test]
        public void TestRentBooksLessDL()
        {
            var d = int.Parse(ConfigurationManager.AppSettings["D"]);

            for (var i = 0; i < d - 1; i++)
            {
                var result = this.bookService.RentBooks(
                    new List<Rent>
                    {
                        new Rent { BookName = $"SQL Server{(char)('a' + i)}", EditionName = $"Corint{(char)('a' + i)}", },
                    },
                    this.reader,
                    this.librarian,
                    DateTime.Now.AddDays(i * 7));
                Assert.True(result != null);
            }
        }

        /// <summary>
        /// Test borrow books with more DL.
        /// </summary>
        [Test]
        public void TestRentBooksMoreDL()
        {
            var d = int.Parse(ConfigurationManager.AppSettings["D"]);

            for (var i = 0; i < d + 1; i++)
            {
                var result = this.bookService.RentBooks(
                    new List<Rent>
                    {
                        new Rent { BookName = $"SQL Server{(char)('a' + i)}", EditionName = $"Corint{(char)('a' + i)}", },
                    },
                    this.reader,
                    this.librarian,
                    DateTime.Now.AddDays(i * 7));
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

        /// <summary>
        /// Test Add extension for less than lim.
        /// </summary>
        [Test]
        public void TestAddExtensionsLessThanLim()
        {
            var lim = int.Parse(ConfigurationManager.AppSettings["LIM"]);

            var bw = this.bookService.RentBooks(
                new List<Rent> { new Rent { BookName = "Java for junior", EditionName = "Corint" } },
                this.reader,
                this.librarian,
                DateTime.Now);

            for (var i = 0; i <= lim; i++)
            {
                var result = this.readerService.AddExtension(this.reader, bw);
                Assert.True(result);
            }
        }

        /// <summary>
        /// Test add extensions more than lim.
        /// </summary>
        [Test]
        public void TestAddExtensionsMoreThanLim()
        {
            var lim = int.Parse(ConfigurationManager.AppSettings["LIM"]);

            var bw = this.bookService.RentBooks(
                new List<Rent> { new Rent { BookName = "Java for junior", EditionName = "Corint" } },
                this.reader,
                this.librarian,
                DateTime.Now);

            for (var i = 0; i <= lim + 1; i++)
            {
                var result = this.readerService.AddExtension(this.reader, bw);
                if (i != lim + 1)
                {
                    Assert.True(result);
                }
                else
                {
                    Assert.False(result);
                }
            }
        }
    }
}