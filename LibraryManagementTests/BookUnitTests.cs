// <copyright file="BookUnitTests.cs" company="Transilvania University of Brasov">
// Margarit Marian Catalin
// </copyright>

namespace LibraryManagementTests
{
    using System.Collections.Generic;
    using System.Configuration;
    using System.Linq;
    using NUnit.Framework;
    using LibraryManagement.BusinessLayer;
    using LibraryManagement.DataMapper;
    using LibraryManagement.DomainModel;
    using Telerik.JustMock.EntityFramework;

    /// <summary>
    /// Book unit tests.
    /// </summary>
    [TestFixture]
    public class BookUnitTests
    {
        private Edition defaultTestEdition;

        private Author defaultTestAuthor;

        private BookService bookService;

        private Domain defaultTestDomain;

        private LibraryDbContext libraryContextMock;

        /// <summary>
        /// Tests setup.
        /// </summary>
        [SetUp]
        public void SetUp()
        {
            var bookStock = new BookStock { Amount = 100, LectureRoomAmount = 10 };
            this.defaultTestEdition = new Edition { Name = "Corint", BookType = "Plasticcover", Pages = 256, BookStock = bookStock };
            this.defaultTestAuthor = new Author { FirstName = "Estera", LastName = "Balas" };
            this.defaultTestDomain = new Domain { Name = "Action", };
            this.libraryContextMock = EntityFrameworkMock.Create<LibraryDbContext>();
            EntityFrameworkMock.PrepareMock(this.libraryContextMock);
            this.bookService = new BookService(
                new BookRepository(this.libraryContextMock),
                new DomainsService(new DomainsRepository(this.libraryContextMock)),
                new ReaderRepository(this.libraryContextMock));
        }

        /// <summary>
        /// Test add a null book.
        /// </summary>
        [Test]
        public void TestAddNullBook()
        {
            Book book = null;
            var result = this.bookService.CreateBook(book);
            Assert.False(result);
            Assert.True(this.libraryContextMock.Books.Count() == 0);
        }

        /// <summary>
        /// Test add a book with null domains.
        /// </summary>
        [Test]
        public void TestAddBookWithNullCategories()
        {
            var book = new Book
            {
                Name = "Testing is important",
                Authors = new List<Author> { this.defaultTestAuthor },
                Editions = new List<Edition> { this.defaultTestEdition },
                Categories = null,
            };
            var result = this.bookService.CreateBook(book);
            Assert.False(result);
            Assert.True(this.libraryContextMock.Books.Count() == 0);
        }

        /// <summary>
        /// Test add book with no domain.
        /// </summary>
        [Test]
        public void TestAddBookWithNoDomain()
        {
            var book = new Book
            {
                Name = "Java for junior",
                Authors = new List<Author> { this.defaultTestAuthor },
                Editions = new List<Edition> { this.defaultTestEdition },
                Categories = new List<Domain>(),
            };
            var result = this.bookService.CreateBook(book);
            Assert.False(result);
        }

        /// <summary>
        /// Test add book with one domain.
        /// </summary>
        [Test]
        public void TestAddBookWithOneDomain()
        {
            var book = new Book
            {
                Name = "Java for junior",
                Authors = new List<Author> { this.defaultTestAuthor },
                Editions = new List<Edition> { this.defaultTestEdition },
                Categories = new List<Domain> { this.defaultTestDomain },
            };
            this.bookService.CreateBook(book);
            Assert.True(this.libraryContextMock.Books.Count() == 1);
        }

        /// <summary>
        /// Test add book with more than DOM domains.
        /// </summary>
        [Test]
        public void TestAddBookWithMoreThanDOMCategories()
        {
            var dOM = int.Parse(ConfigurationManager.AppSettings["DOM"]);
            dOM = dOM + 1;
            var domainsList = new List<Domain>();
            for (var i = 0; i < dOM; i++)
            {
                domainsList.Add(new Domain { Name = i.ToString() });
            }

            var book = new Book
            {
                Name = "Java for junior",
                Authors = new List<Author> { this.defaultTestAuthor },
                Editions = new List<Edition> { this.defaultTestEdition },
                Categories = domainsList,
            };
            var result = this.bookService.CreateBook(book);
            Assert.False(result);
            Assert.True(this.libraryContextMock.Books.Count() == 0);
        }

        /// <summary>
        /// Test add book with empty name.
        /// </summary>
        [Test]
        public void TestAddBookWithEmptyName()
        {
            var book = new Book
            {
                Name = string.Empty,
                Authors = new List<Author> { this.defaultTestAuthor },
                Editions = new List<Edition> { this.defaultTestEdition },
                Categories = new List<Domain> { this.defaultTestDomain },
            };
            var result = this.bookService.CreateBook(book);
            Assert.False(result);
            Assert.True(this.libraryContextMock.Books.Count() == 0);
        }

        /// <summary>
        /// Test add Book with null name.
        /// </summary>
        [Test]
        public void TestAddBookWithNoName()
        {
            var book = new Book
            {
                Name = null,
                Authors = new List<Author> { this.defaultTestAuthor },
                Editions = new List<Edition> { this.defaultTestEdition },
                Categories = new List<Domain> { this.defaultTestDomain },
            };
            var result = this.bookService.CreateBook(book);
            Assert.False(result);
            Assert.True(this.libraryContextMock.Books.Count() == 0);
        }

        /// <summary>
        /// Test add book with less name.
        /// </summary>
        [Test]
        public void TestAddBookWithNameLengthLessThanThree()
        {
            var book = new Book
            {
                Name = "ab",
                Authors = new List<Author> { this.defaultTestAuthor },
                Editions = new List<Edition> { this.defaultTestEdition },
                Categories = new List<Domain> { this.defaultTestDomain },
            };
            var result = this.bookService.CreateBook(book);
            Assert.False(result);
            Assert.True(this.libraryContextMock.Books.Count() == 0);
        }

        /// <summary>
        /// Test add book with bigger name.
        /// </summary>
        [Test]
        public void TestAddBookWithNameLengthBiggerThanEighy()
        {
            var book = new Book
            {
                Name = "LongNameLongNameLongNameLongNameLongNameLongNameLongNameLongNameLongNameLongNameLongNameLongNameLongNameLongNameLongNameLongNameLongNameLongNameLongNameLongNameLongNameLongName",
                Authors = new List<Author> { this.defaultTestAuthor },
                Editions = new List<Edition> { this.defaultTestEdition },
                Categories = new List<Domain> { this.defaultTestDomain },
            };
            var result = this.bookService.CreateBook(book);
            Assert.False(result);
            Assert.True(this.libraryContextMock.Books.Count() == 0);
        }

        /// <summary>
        /// Test add book with digits name.
        /// </summary>
        [Test]
        public void TestAddBookNameWithDigits()
        {
            var book = new Book
            {
                Name = "3454",
                Authors = new List<Author> { this.defaultTestAuthor },
                Editions = new List<Edition> { this.defaultTestEdition },
                Categories = new List<Domain> { this.defaultTestDomain },
            };
            var result = this.bookService.CreateBook(book);
            Assert.False(result);
            Assert.True(this.libraryContextMock.Books.Count() == 0);
        }

        /// <summary>
        /// Test add book with symbol in name.
        /// </summary>
        [Test]
        public void TestAddBookNameWithSymbols()
        {
            var book = new Book
            {
                Name = "*&()_",
                Authors = new List<Author> { this.defaultTestAuthor },
                Editions = new List<Edition> { this.defaultTestEdition },
                Categories = new List<Domain> { this.defaultTestDomain },
            };
            var result = this.bookService.CreateBook(book);
            Assert.False(result);
            Assert.True(this.libraryContextMock.Books.Count() == 0);
        }

        /// <summary>
        /// Test add book with white spaces in name.
        /// </summary>
        [Test]
        public void TestAddBookWithNameContainsWhitespaces()
        {
            var book = new Book
            {
                Name = "Fram ursul polar",
                Authors = new List<Author> { this.defaultTestAuthor },
                Editions = new List<Edition> { this.defaultTestEdition },
                Categories = new List<Domain> { this.defaultTestDomain },
            };
            var result = this.bookService.CreateBook(book);
            Assert.False(this.libraryContextMock.Books.Count() == 0);
        }

        /// <summary>
        /// Test add book with two authors.
        /// </summary>
        [Test]
        public void TestAddBookWithTwoAuthors()
        {
            var book = new Book
            {
                Name = "Testing is important",
                Authors =
                               new List<Author>
                               {
                                   this.defaultTestAuthor, new Author { FirstName = "Robert", LastName = "German", },
                               },
                Editions = new List<Edition> { this.defaultTestEdition },
                Categories = new List<Domain> { this.defaultTestDomain },
            };
            var result = this.bookService.CreateBook(book);
            Assert.True(this.libraryContextMock.Books.Count() == 1);
        }

        /// <summary>
        /// Test add book with empty author.
        /// </summary>
        [Test]
        public void TestAddBookWithNoAuthors()
        {
            var book = new Book
            {
                Name = "Testing is important",
                Authors = new List<Author> { },
                Editions = new List<Edition> { this.defaultTestEdition },
                Categories = new List<Domain> { this.defaultTestDomain },
            };
            var result = this.bookService.CreateBook(book);
            Assert.False(result);
            Assert.True(this.libraryContextMock.Books.Count() == 0);
        }

        /// <summary>
        /// Test add book with null authors.
        /// </summary>
        [Test]
        public void TestAddBookWithNullAuthors()
        {
            var book = new Book
            {
                Name = "Testing is important",
                Authors = null,
                Editions = new List<Edition> { this.defaultTestEdition },
                Categories = new List<Domain> { this.defaultTestDomain },
            };
            var result = this.bookService.CreateBook(book);
            Assert.False(result);
            Assert.True(this.libraryContextMock.Books.Count() == 0);
        }

        /// <summary>
        /// Test add book with no edition.
        /// </summary>
        [Test]
        public void TestAddBookWithNoEditions()
        {
            var book = new Book
            {
                Name = "Testing is important",
                Authors = new List<Author> { this.defaultTestAuthor },
                Editions = new List<Edition> { },
                Categories = new List<Domain> { this.defaultTestDomain },
            };
            var result = this.bookService.CreateBook(book);
            Assert.False(result);
            Assert.True(this.libraryContextMock.Books.Count() == 0);
        }

        /// <summary>
        /// Test add book with null edition.
        /// </summary>
        [Test]
        public void TestAddBookWithNullEditions()
        {
            var book = new Book
            {
                Name = "Testing is important",
                Authors = new List<Author> { this.defaultTestAuthor },
                Editions = null,
                Categories = new List<Domain> { this.defaultTestDomain },
            };
            var result = this.bookService.CreateBook(book);
            Assert.False(result);
            Assert.True(this.libraryContextMock.Books.Count() == 0);
        }

        /// <summary>
        /// Test add book with two editions.
        /// </summary>
        [Test]
        public void TestAddBookWithTwoEditions()
        {
            var book = new Book
            {
                Name = "Testing is important",
                Authors = new List<Author> { this.defaultTestAuthor },
                Editions = new List<Edition> { this.defaultTestEdition, new Edition { Name = "Girofar" } },
                Categories = new List<Domain> { this.defaultTestDomain },
            };
            var result = this.bookService.CreateBook(book);
            Assert.True(this.libraryContextMock.Books.Count() == 1);
        }

        /// <summary>
        /// Test add book with related domains.
        /// </summary>
        [Test]
        public void TestAddBookWithRelatedCategories()
        {
            var book = new Book
            {
                Name = "Testing is important",
                Authors = new List<Author> { this.defaultTestAuthor },
                Editions = new List<Edition> { this.defaultTestEdition },
                Categories = new List<Domain>
                                        {
                                            this.defaultTestDomain,
                                            new Domain { Name = "Science", ParentDomain = this.defaultTestDomain, },
                                        },
            };
            var result = this.bookService.CreateBook(book);
            Assert.False(result);
            Assert.True(this.libraryContextMock.Books.Count() == 0);
        }

        /// <summary>
        /// Test add book with related indirect domains.
        /// </summary>
        [Test]
        public void TestAddBookWithRelatedIndirectCategories()
        {
            var book = new Book
            {
                Name = "Testing is important",
                Authors = new List<Author> { this.defaultTestAuthor },
                Editions = new List<Edition> { this.defaultTestEdition },
                Categories = new List<Domain>
                                        {
                                            this.defaultTestDomain,
                                            new Domain
                                            {
                                                Name = "Science",
                                                ParentDomain =
                                                    new Domain { ParentDomain = this.defaultTestDomain, Name = "Comedy", },
                                            },
                                        },
            };
            var result = this.bookService.CreateBook(book);
            Assert.False(result);
            Assert.True(this.libraryContextMock.Books.Count() == 0);
        }

        /// <summary>
        /// Test get a book.
        /// </summary>
        [Test]
        public void TestGetBook()
        {
            var book = new Book
            {
                Name = "Java for junior",
                Authors = new List<Author> { this.defaultTestAuthor },
                Editions = new List<Edition> { this.defaultTestEdition },
                Categories = new List<Domain> { this.defaultTestDomain },
            };
            this.bookService.CreateBook(book);
            book = this.bookService.GetBook(book.Name);
            Assert.NotNull(book);
        }

        /// <summary>
        /// Test get a null book.
        /// </summary>
        [Test]
        public void TestGetNullBook()
        {
            var book = this.bookService.GetBook(null);
            Assert.Null(book);
        }

        /// <summary>
        /// Test get book by empty name.
        /// </summary>
        [Test]
        public void TestGetEmptyBook()
        {
            var book = this.bookService.GetBook(string.Empty);
            Assert.Null(book);
        }

        /// <summary>
        /// Test get Unknown book.
        /// </summary>
        [Test]
        public void TestGetUnknownBook()
        {
            var book = new Book
            {
                Name = "Java for junior",
                Authors = new List<Author> { this.defaultTestAuthor },
                Editions = new List<Edition> { this.defaultTestEdition },
                Categories = new List<Domain> { this.defaultTestDomain },
            };
            this.bookService.CreateBook(book);
            book = this.bookService.GetBook("Fram ursul polar.");
            Assert.Null(book);
        }
    }
}