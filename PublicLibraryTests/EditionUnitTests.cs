// <copyright file="EditionUnitTests.cs" company="Transilvania University of Brasov">
// Margarit Marian Catalin
// </copyright>

namespace LibraryManagementTests
{
    using System.Collections.Generic;
    using System.Linq;
    using NUnit.Framework;
    using LibraryManagement.BusinessLayer;
    using LibraryManagement.Data_Mapper;
    using LibraryManagement.Domain_Model;
    using Telerik.JustMock.EntityFramework;

    /// <summary>
    /// Edition unit tests.
    /// </summary>
    [TestFixture]
    public class EditionUnitTests
    {
        private LibraryDbContext libraryContextMock;

        private BookService bookService;

        private Book defaultTestBook;

        private Edition defaultTestEdition;

        /// <summary>
        /// Tests setup.
        /// </summary>
        [SetUp]
        public void SetUp()
        {
            this.libraryContextMock = EntityFrameworkMock.Create<LibraryDbContext>();
            this.bookService = new BookService(
                new BookRepository(this.libraryContextMock),
                new CategoriesService(new CategoriesRepository(this.libraryContextMock)),
                new ReaderRepository(this.libraryContextMock));

            this.defaultTestBook = new Book
                         {
                             Name = "Testing is important",
                             Authors = new List<Author>() { new Author { FirstName = "Estera", LastName = "Balas" } },
                             Categories = new List<Category> { new Category { Name = "Novel" } },
                             Editions = new List<Edition>
                                        {
                                            new Edition { Name = "Ultimate Edition", BookType = "Plastic Cover", Pages = 100 },
                                        },
                         };
            this.defaultTestEdition = this.defaultTestBook.Editions.First();
            this.bookService.CreateBook(this.defaultTestBook);
            this.defaultTestEdition.Book = this.defaultTestBook;
            this.libraryContextMock.Editions.Add(this.defaultTestEdition);
        }

        /// <summary>
        /// Test add edition for a null book.
        /// </summary>
        [Test]
        public void TestAddEditionNullBook()
        {
            var edition = new Edition { Name = "New edition", BookType = "Plastic Cover", Pages = 100 };
            this.bookService.AddEdition(null, edition);
            Assert.True(this.defaultTestBook.Editions.Count() == 1);
        }

        /// <summary>
        /// Test add a null edition.
        /// </summary>
        [Test]
        public void TestAddNullEdition()
        {
            this.bookService.AddEdition(this.defaultTestBook, null);
            Assert.True(this.defaultTestBook.Editions.Count() == 1);
        }

        /// <summary>
        /// Test add an edition.
        /// </summary>
        [Test]
        public void TestAddEdition()
        {
            var edition = new Edition { Name = "New edition", BookType = "Plastic Cover", Pages = 100 };
            this.bookService.AddEdition(this.defaultTestBook, edition);
            Assert.True(this.defaultTestBook.Editions.Count() == 2);
        }

        /// <summary>
        /// Test add edition with null name.
        /// </summary>
        [Test]
        public void TestAddEditionNullName()
        {
            var edition = new Edition { Name = null, BookType = "Plastic Cover", Pages = 100 };
            this.bookService.AddEdition(this.defaultTestBook, edition);
            Assert.True(this.defaultTestBook.Editions.Count() == 1);
        }

        /// <summary>
        /// Test add edition with empty name.
        /// </summary>
        [Test]
        public void TestAddEditionEmptyName()
        {
            var edition = new Edition { Name = string.Empty, BookType = "Plastic Cover", Pages = 100 };
            this.bookService.AddEdition(this.defaultTestBook, edition);
            Assert.True(this.defaultTestBook.Editions.Count() == 1);
        }

        /// <summary>
        /// Test add edition with small name.
        /// </summary>
        [Test]
        public void TestAddEditionSmallName()
        {
            var edition = new Edition { Name = "Ab", BookType = "Plastic Cover", Pages = 100 };
            this.bookService.AddEdition(this.defaultTestBook, edition);
            Assert.True(this.defaultTestBook.Editions.Count() == 1);
        }

        /// <summary>
        /// Test add edition with too long name.
        /// </summary>
        [Test]
        public void TestAddEditionLongName()
        {
            var edition = new Edition
                          {
                              Name =
                                  "LongLongLongLongLongLongLongLongLongLongLongLongLongLongLongLongLongLongLongLongLongLongLongLongLongLongLongLongLongLongLongLongLongLong",
                              BookType = "Plastic Cover",
                              Pages = 100,
                          };
            this.bookService.AddEdition(this.defaultTestBook, edition);
            Assert.True(this.defaultTestBook.Editions.Count() == 1);
        }

        /// <summary>
        /// Test add edition with symbol in name.
        /// </summary>
        [Test]
        public void TestAddEditionNameSymbol()
        {
            var edition = new Edition { Name = "Invalid Edition@$@$@%%*&", BookType = "Plastic Cover", Pages = 100 };
            this.bookService.AddEdition(this.defaultTestBook, edition);
            Assert.True(this.defaultTestBook.Editions.Count() == 1);
        }

        /// <summary>
        /// Test add edition with lower case.
        /// </summary>
        [Test]
        public void TestAddEditionNameLowerCase()
        {
            var edition = new Edition { Name = "invalid Edition", BookType = "Plastic Cover", Pages = 100 };
            this.bookService.AddEdition(this.defaultTestBook, edition);
            Assert.True(this.defaultTestBook.Editions.Count() == 1);
        }

        /// <summary>
        /// Test add edition with null type.
        /// </summary>
        [Test]
        public void TestAddEditionNullType()
        {
            var edition = new Edition { Name = "Invalid Edition", BookType = null, Pages = 100 };
            this.bookService.AddEdition(this.defaultTestBook, edition);
            Assert.True(this.defaultTestBook.Editions.Count() == 1);
        }

        /// <summary>
        /// Test add edition with empty type.
        /// </summary>
        [Test]
        public void TestAddEditionEmptyType()
        {
            var edition = new Edition { Name = "Invalid Edition", BookType = string.Empty, Pages = 100 };
            this.bookService.AddEdition(this.defaultTestBook, edition);
            Assert.True(this.defaultTestBook.Editions.Count() == 1);
        }

        /// <summary>
        /// Test add edition with small type.
        /// </summary>
        [Test]
        public void TestAddEditionSmallType()
        {
            var edition = new Edition { Name = "Invalid Edition", BookType = "Aa", Pages = 100 };
            this.bookService.AddEdition(this.defaultTestBook, edition);
            Assert.True(this.defaultTestBook.Editions.Count() == 1);
        }

        /// <summary>
        /// Test add edition with bigger type.
        /// </summary>
        [Test]
        public void TestAddEditionLongType()
        {
            var edition = new Edition
                          {
                              Name = "Invalid Edition",
                              BookType =
                                  "LongLongLongLongLongLongLongLongLongLongLongLongLongLongLongLongLongLongLongLongLongLongLongLongLongLong",
                              Pages = 100,
                          };
            this.bookService.AddEdition(this.defaultTestBook, edition);
            Assert.True(this.defaultTestBook.Editions.Count() == 1);
        }

        /// <summary>
        /// Test add edition with symbol in type.
        /// </summary>
        [Test]
        public void TestAddEditionTypeSymbol()
        {
            var edition = new Edition { Name = "Invalid Edition", BookType = "Hardcover@#%(#@*&$(*%", Pages = 100 };
            this.bookService.AddEdition(this.defaultTestBook, edition);
            Assert.True(this.defaultTestBook.Editions.Count() == 1);
        }

        /// <summary>
        /// Test add edition with digit in type.
        /// </summary>
        [Test]
        public void TestAddEditionTypeDigit()
        {
            var edition = new Edition { Name = "Invalid Edition", BookType = "Hardcover23", Pages = 100 };
            this.bookService.AddEdition(this.defaultTestBook, edition);
            Assert.True(this.defaultTestBook.Editions.Count() == 1);
        }

        /// <summary>
        /// Test add edition with white space in type.
        /// </summary>
        [Test]
        public void TestAddEditionTypeWhiteSpace()
        {
            var edition = new Edition { Name = "Second Edition", BookType = "Plastic Cover", Pages = 100 };
            this.bookService.AddEdition(this.defaultTestBook, edition);
            Assert.True(this.defaultTestBook.Editions.Count() == 2);
        }

        /// <summary>
        /// Test add edition with negative number of pages.
        /// </summary>
        [Test]
        public void TestAddEditionNegativeNumberOfPages()
        {
            var edition = new Edition { Name = "Invalid Edition", BookType = "Plastic Cover", Pages = -10 };
            this.bookService.AddEdition(this.defaultTestBook, edition);
            Assert.True(this.defaultTestBook.Editions.Count() == 1);
        }

        /// <summary>
        /// Test add edition with 0 pages.
        /// </summary>
        [Test]
        public void TestAddEditionZeroPages()
        {
            var edition = new Edition { Name = "Invalid Edition", BookType = "Plastic Cover", Pages = 0 };
            this.bookService.AddEdition(this.defaultTestBook, edition);
            Assert.True(this.defaultTestBook.Editions.Count() == 1);
        }

        /// <summary>
        /// Test add edition with too many pages.
        /// </summary>
        [Test]
        public void TestAddEditionTooManyPages()
        {
            var edition = new Edition { Name = "Invalid Edition", BookType = "Plastic Cover", Pages = 99999999 };
            this.bookService.AddEdition(this.defaultTestBook, edition);
            Assert.True(this.defaultTestBook.Editions.Count() == 1);
        }

        /// <summary>
        /// Test add edition with type lowercase.
        /// </summary>
        [Test]
        public void TestAddEditionTypeLowercase()
        {
            var edition = new Edition { Name = "Second Edition", BookType = "plastic Cover", Pages = 99999999 };
            this.bookService.AddEdition(this.defaultTestBook, edition);
            Assert.True(this.defaultTestBook.Editions.Count() == 1);
        }

        /// <summary>
        /// Test get edition.
        /// </summary>
        [Test]
        public void GetEdition()
        {
            var edition = this.bookService.GetEdition("Testing is important", "Ultimate Edition");
            Assert.NotNull(edition);
        }

        /// <summary>
        /// Test get edition by null.
        /// </summary>
        [Test]
        public void GetNullEdition()
        {
            var edition = this.bookService.GetEdition(null, null);
            Assert.Null(edition);
        }

        /// <summary>
        /// Test get edition by empty strings.
        /// </summary>
        [Test]
        public void GetEmptyEdition()
        {
            var edition = this.bookService.GetEdition(string.Empty, string.Empty);
            Assert.Null(edition);
        }

        /// <summary>
        /// Test get unknown edition.
        /// </summary>
        [Test]
        public void GetUnknownEdition()
        {
            var edition = this.bookService.GetEdition("Testing in java", "Jean De Lichte");
            Assert.Null(edition);
        }

        /// <summary>
        /// Test get edition by unknown book name.
        /// </summary>
        [Test]
        public void GetEditionBadBookName()
        {
            var edition = this.bookService.GetEdition("Testing in java", "Ultimate Edition");
            Assert.Null(edition);
        }

        /// <summary>
        /// Test get edition with unknown edition name.
        /// </summary>
        [Test]
        public void GetEditionBadEditionName()
        {
            var edition = this.bookService.GetEdition("Testing is important", "Second Edition");
            Assert.Null(edition);
        }
    }
}