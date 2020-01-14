// <copyright file="BookBorrowingTests.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace PublicLibraryDbTests
{
    using System;
    using System.Collections.Generic;
    using System.Configuration;
    using NUnit.Framework;
    using PublicLibrary.BusinessLayer;
    using PublicLibrary.Data_Mapper;
    using PublicLibrary.Domain_Model;
    using Assert = NUnit.Framework.Assert;

    /// <summary>
    /// BookBorrowing tests.
    /// </summary>
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

        private List<Book> booksList;

        private ReaderService readerService;

        /// <summary>
        /// Tests setup.
        /// </summary>
        [SetUp]
        public void SetUp()
        {
            this.libraryDb = new LibraryDb();
            var readerRepository = new ReaderRepository(this.libraryDb);
            var employeeService = new EmployeeService(new EmployeeRepository(this.libraryDb));
            this.readerService = new ReaderService(readerRepository);
            this.bookService = new BookService(
                new BookRepository(this.libraryDb),
                new CategoriesService(new CategoriesRepository(this.libraryDb)),
                readerRepository);
            this.author = new Author { FirstName = "Ioan", LastName = "Slavici" };
            this.bookStock = new BookStock { Amount = 14, LectureRoomAmount = 10 };
            var bookStock2 = new BookStock { Amount = 1000, LectureRoomAmount = 0 };
            var bookstock3 = new BookStock { Amount = 12, LectureRoomAmount = 10 };
            this.edition = new Edition
                            {
                                Name = "Teora", BookType = "Hardcover", Pages = 320, BookStock = this.bookStock,
                            };
            var edition2 = new Edition
                           {
                               Name = "First Edition", BookType = "Hardcover", Pages = 320, BookStock = bookstock3,
                           };
            this.category = new Category { Name = "Nuvela" };

            this.employee = new Employee()
                             {
                                 FirstName = "Catalin",
                                 LastName = "Vola",
                                 Email = "catalin.V@yahoo.com",
                                 Phone = "7345345568",
                                 Address = "Florii nr.15",
                                 Gender = "M",
                             };
            this.reader = new Reader
                           {
                               FirstName = "Al Alekku",
                               LastName = "Alexandru",
                               Email = "alexandru.v@yahoo.com",
                               Phone = "1234567890",
                               Address = "Str.Memorandului nr.4",
                               Extensions = new List<Extension>(),
                               Gender = "M",
                           };
            this.book = new Book
                         {
                             Name = "Moara cu noroc",
                             Authors = new[] { this.author },
                             Editions = new[] { this.edition },
                             Categories = new[] { this.category },
                         };

            this.readerService.AddReader(this.reader);
            employeeService.AddEmployee(this.employee);
            this.bookService.CreateBook(this.book);
            for (var i = 0; i < 25; i++)
            {
                this.bookService.CreateBook(
                    new Book
                    {
                        Name = $"Moara cu noroc{(char)('a' + i)}",
                        Authors = new[] { this.author },
                        Editions = new[]
                                   {
                                       new Edition
                                       {
                                           Name = $"Teora{(char)('a' + i)}",
                                           BookType = "Hardcover",
                                           Pages = 320,
                                           BookStock = bookStock2,
                                       },
                                   },
                        Categories = new[] { new Category { Name = $"noroc{(char)('a' + i)}" }, },
                    });
            }

            for (var i = 0; i < 25; i++)
            {
                this.bookService.CreateBook(
                    new Book
                    {
                        Name = $"Mara{(char)('a' + i)}",
                        Authors = new[] { this.author },
                        Editions = new[]
                                   {
                                       new Edition
                                       {
                                           Name = $"Teora{(char)('a' + i)}",
                                           BookType = "Hardcover",
                                           Pages = 320,
                                           BookStock = bookStock2,
                                       },
                                   },
                        Categories = new[] { this.category },
                    });
            }

            this.bookService.CreateBook(
                new Book
                {
                    Name = "Zana Zorilor",
                    Authors = new[] { this.author },
                    Editions = new[] { edition2 },
                    Categories = new[] { this.category },
                });
        }

        /// <summary>
        /// Clean database.
        /// </summary>
        [TearDown]
        public void Cleanup()
        {
            this.libraryDb.Database.Delete();
        }

        /// <summary>
        /// Test Borrow one book.
        /// </summary>
        [Test]
        public void TestBorrowOneBook()
        {
            var result = this.bookService.BorrowBooks(
                new List<Borrowing> { new Borrowing { BookName = "Moara cu noroc", EditionName = "Teora" } },
                this.reader,
                this.employee,
                DateTime.Now);

            Assert.True(result != null);
        }

        /// <summary>
        /// Test Borrow one book with is not in stock.
        /// </summary>
        [Test]
        public void TestBorrowOneBookNotInStock()
        {
            var result = this.bookService.BorrowBooks(
                new List<Borrowing> { new Borrowing { BookName = "Zana Zorilor", EditionName = "First Edition" } },
                this.reader,
                this.employee,
                DateTime.Now);

            Assert.False(result != null);
        }

        /// <summary>
        /// Test borrow books in delta.
        /// </summary>
        [Test]
        public void TestBorrowBooksWithinDelta()
        {
            var delta = int.Parse(ConfigurationManager.AppSettings["DELTA"]);

            var result1 = this.bookService.BorrowBooks(
                new List<Borrowing> { new Borrowing { BookName = "Moara cu noroc", EditionName = "Teora" } },
                this.reader,
                this.employee,
                DateTime.Now);
            var result2 = this.bookService.BorrowBooks(
                new List<Borrowing> { new Borrowing { BookName = "Moara cu noroc", EditionName = "Teora" } },
                this.reader,
                this.employee,
                DateTime.Now.AddDays(delta - 1));

            Assert.True(result1 != null);
            Assert.False(result2 != null);
        }

        /// <summary>
        /// Test borrow book in outside delta.
        /// </summary>
        [Test]
        public void TestBorrowBooksOutsideDelta()
        {
            var delta = int.Parse(ConfigurationManager.AppSettings["DELTA"]);

            var result1 = this.bookService.BorrowBooks(
                new List<Borrowing> { new Borrowing { BookName = "Moara cu noroc", EditionName = "Teora" } },
                this.reader,
                this.employee,
                DateTime.Now);
            var result2 = this.bookService.BorrowBooks(
                new List<Borrowing> { new Borrowing { BookName = "Moara cu noroc", EditionName = "Teora" } },
                this.reader,
                this.employee,
                DateTime.Now.AddDays(delta + 1));

            Assert.True(result1 != null);
            Assert.True(result2 != null);
        }

        /// <summary>
        /// Test borrow books in less NMCPER.
        /// </summary>
        [Test]
        public void TestBorrowBooksLessNMCPER()
        {
            var nmc = int.Parse(ConfigurationManager.AppSettings["NMC"]);

            for (var i = 0; i < nmc - 1; i++)
            {
                var result = this.bookService.BorrowBooks(
                    new List<Borrowing>
                    {
                        new Borrowing
                        {
                            BookName = $"Moara cu noroc{(char)('a' + i)}", EditionName = $"Teora{(char)('a' + i)}",
                        },
                    },
                    this.reader,
                    this.employee,
                    DateTime.Now.AddDays(i));
                Assert.True(result != null);
            }
        }

        /// <summary>
        /// Test borrow books more NMCPER.
        /// </summary>
        [Test]
        public void TestBorrowBooksMoreNMCPER()
        {
            var nmc = int.Parse(ConfigurationManager.AppSettings["NMC"]);

            for (var i = 0; i < nmc + 1; i++)
            {
                var result = this.bookService.BorrowBooks(
                    new List<Borrowing>
                    {
                        new Borrowing
                        {
                            BookName = $"Moara cu noroc{(char)('a' + i)}", EditionName = $"Teora{(char)('a' + i)}",
                        },
                    },
                    this.reader,
                    this.employee,
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
        public void TestBorrowBooksLessNCZ()
        {
            var ncz = int.Parse(ConfigurationManager.AppSettings["NCZ"]);

            for (var i = 0; i < ncz - 1; i++)
            {
                var result = this.bookService.BorrowBooks(
                    new List<Borrowing>
                    {
                        new Borrowing
                        {
                            BookName = $"Moara cu noroc{(char)('a' + i)}", EditionName = $"Teora{(char)('a' + i)}",
                        },
                    },
                    this.reader,
                    this.employee,
                    DateTime.Now);
                Assert.True(result != null);
            }
        }

        /// <summary>
        /// Test borrow books with more ncz.
        /// </summary>
        [Test]
        public void TestBorrowBooksMoreNCZ()
        {
            var ncz = int.Parse(ConfigurationManager.AppSettings["NCZ"]);

            for (var i = 0; i < ncz + 1; i++)
            {
                var result = this.bookService.BorrowBooks(
                    new List<Borrowing>
                    {
                        new Borrowing
                        {
                            BookName = $"Moara cu noroc{(char)('a' + i)}", EditionName = $"Teora{(char)('a' + i)}",
                        },
                    },
                    this.reader,
                    this.employee,
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
        public void TestBorrowLessCBooks()
        {
            var c = int.Parse(ConfigurationManager.AppSettings["C"]);
            var borrowList = new List<Borrowing>();

            for (var i = 0; i < c - 1; i++)
            {
                borrowList.Add(
                    new Borrowing
                    {
                        BookName = $"Moara cu noroc{(char)('a' + i)}", EditionName = $"Teora{(char)('a' + i)}",
                    });
            }

            var result = this.bookService.BorrowBooks(borrowList, this.reader, this.employee, DateTime.Now);
            Assert.True(result != null);
        }

        /// <summary>
        /// Test borrow more c books.
        /// </summary>
        [Test]
        public void TestBorrowMoreCBooks()
        {
            var c = int.Parse(ConfigurationManager.AppSettings["C"]);
            var borrowList = new List<Borrowing>();

            for (var i = 0; i < c + 1; i++)
            {
                borrowList.Add(
                    new Borrowing
                    {
                        BookName = $"Moara cu noroc{(char)('a' + i)}", EditionName = $"Teora{(char)('a' + i)}",
                    });
            }

            var result = this.bookService.BorrowBooks(borrowList, this.reader, this.employee, DateTime.Now);
            Assert.False(result != null);
        }

        /// <summary>
        /// Test borrow books with less DL.
        /// </summary>
        [Test]
        public void TestBorrowBooksLessDL()
        {
            var d = int.Parse(ConfigurationManager.AppSettings["D"]);

            for (var i = 0; i < d - 1; i++)
            {
                var result = this.bookService.BorrowBooks(
                    new List<Borrowing>
                    {
                        new Borrowing { BookName = $"Mara{(char)('a' + i)}", EditionName = $"Teora{(char)('a' + i)}", },
                    },
                    this.reader,
                    this.employee,
                    DateTime.Now.AddDays(i * 7));
                Assert.True(result != null);
            }
        }

        /// <summary>
        /// Test borrow books with more DL.
        /// </summary>
        [Test]
        public void TestBorrowBooksMoreDL()
        {
            var d = int.Parse(ConfigurationManager.AppSettings["D"]);

            for (var i = 0; i < d + 1; i++)
            {
                var result = this.bookService.BorrowBooks(
                    new List<Borrowing>
                    {
                        new Borrowing { BookName = $"Mara{(char)('a' + i)}", EditionName = $"Teora{(char)('a' + i)}", },
                    },
                    this.reader,
                    this.employee,
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

            var bw = this.bookService.BorrowBooks(
                new List<Borrowing> { new Borrowing { BookName = "Moara cu noroc", EditionName = "Teora" } },
                this.reader,
                this.employee,
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

            var bw = this.bookService.BorrowBooks(
                new List<Borrowing> { new Borrowing { BookName = "Moara cu noroc", EditionName = "Teora" } },
                this.reader,
                this.employee,
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