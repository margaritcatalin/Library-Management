// <copyright file="BookUnitTests.cs" company="Transilvania University of Brasov">
// Margarit Marian Catalin
// </copyright>

namespace PublicLibraryTests
{
    using System.Collections.Generic;
    using System.Configuration;
    using System.Linq;
    using NUnit.Framework;
    using PublicLibrary.BusinessLayer;
    using PublicLibrary.Data_Mapper;
    using PublicLibrary.Domain_Model;
    using Telerik.JustMock.EntityFramework;

    /// <summary>
    /// Book unit tests.
    /// </summary>
    [TestFixture]
    public class BookUnitTests
    {
        private Edition edition;

        private Author author;

        private BookService bookService;

        private Category category;

        private LibraryDb libraryDbMock;

        /// <summary>
        /// Tests setup.
        /// </summary>
        [SetUp]
        public void SetUp()
        {
            var bookStock = new BookStock { Amount = 100, LectureRoomAmount = 10 };
            this.edition = new Edition { Name = "Corint", BookType = "Plasticcover", Pages = 256, BookStock = bookStock };
            this.author = new Author { FirstName = "Estera", LastName = "Balas" };
            this.category = new Category { Name = "Action", };
            this.libraryDbMock = EntityFrameworkMock.Create<LibraryDb>();
            EntityFrameworkMock.PrepareMock(this.libraryDbMock);
            this.bookService = new BookService(
                new BookRepository(this.libraryDbMock),
                new CategoriesService(new CategoriesRepository(this.libraryDbMock)),
                new ReaderRepository(this.libraryDbMock));
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
            Assert.True(this.libraryDbMock.Books.Count() == 0);
        }

        /// <summary>
        /// Test add a book with null categories.
        /// </summary>
        [Test]
        public void TestAddBookWithNullCategories()
        {
            var book = new Book
            {
                Name = "Testing is important",
                Authors = new List<Author> { this.author },
                Editions = new List<Edition> { this.edition },
                Categories = null,
            };
            var result = this.bookService.CreateBook(book);
            Assert.False(result);
            Assert.True(this.libraryDbMock.Books.Count() == 0);
        }

        /// <summary>
        /// Test add book with no category.
        /// </summary>
        [Test]
        public void TestAddBookWithNoCategory()
        {
            var book = new Book
            {
                Name = "Java for junior",
                Authors = new List<Author> { this.author },
                Editions = new List<Edition> { this.edition },
                Categories = new List<Category>(),
            };
            var result = this.bookService.CreateBook(book);
            Assert.False(result);
        }

        /// <summary>
        /// Test add book with one category.
        /// </summary>
        [Test]
        public void TestAddBookWithOneCategory()
        {
            var book = new Book
            {
                Name = "Java for junior",
                Authors = new List<Author> { this.author },
                Editions = new List<Edition> { this.edition },
                Categories = new List<Category> { this.category },
            };
            this.bookService.CreateBook(book);
            Assert.True(this.libraryDbMock.Books.Count() == 1);
        }

        /// <summary>
        /// Test add book with more than DOM categories.
        /// </summary>
        [Test]
        public void TestAddBookWithMoreThanDOMCategories()
        {
            var dOM = int.Parse(ConfigurationManager.AppSettings["DOM"]);
            dOM = dOM + 1;
            var categoriesList = new List<Category>();
            for (var i = 0; i < dOM; i++)
            {
                categoriesList.Add(new Category { Name = i.ToString() });
            }

            var book = new Book
            {
                Name = "Java for junior",
                Authors = new List<Author> { this.author },
                Editions = new List<Edition> { this.edition },
                Categories = categoriesList,
            };
            var result = this.bookService.CreateBook(book);
            Assert.False(result);
            Assert.True(this.libraryDbMock.Books.Count() == 0);
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
                Authors = new List<Author> { this.author },
                Editions = new List<Edition> { this.edition },
                Categories = new List<Category> { this.category },
            };
            var result = this.bookService.CreateBook(book);
            Assert.False(result);
            Assert.True(this.libraryDbMock.Books.Count() == 0);
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
                Authors = new List<Author> { this.author },
                Editions = new List<Edition> { this.edition },
                Categories = new List<Category> { this.category },
            };
            var result = this.bookService.CreateBook(book);
            Assert.False(result);
            Assert.True(this.libraryDbMock.Books.Count() == 0);
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
                Authors = new List<Author> { this.author },
                Editions = new List<Edition> { this.edition },
                Categories = new List<Category> { this.category },
            };
            var result = this.bookService.CreateBook(book);
            Assert.False(result);
            Assert.True(this.libraryDbMock.Books.Count() == 0);
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
                Authors = new List<Author> { this.author },
                Editions = new List<Edition> { this.edition },
                Categories = new List<Category> { this.category },
            };
            var result = this.bookService.CreateBook(book);
            Assert.False(result);
            Assert.True(this.libraryDbMock.Books.Count() == 0);
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
                Authors = new List<Author> { this.author },
                Editions = new List<Edition> { this.edition },
                Categories = new List<Category> { this.category },
            };
            var result = this.bookService.CreateBook(book);
            Assert.False(result);
            Assert.True(this.libraryDbMock.Books.Count() == 0);
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
                Authors = new List<Author> { this.author },
                Editions = new List<Edition> { this.edition },
                Categories = new List<Category> { this.category },
            };
            var result = this.bookService.CreateBook(book);
            Assert.False(result);
            Assert.True(this.libraryDbMock.Books.Count() == 0);
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
                Authors = new List<Author> { this.author },
                Editions = new List<Edition> { this.edition },
                Categories = new List<Category> { this.category },
            };
            var result = this.bookService.CreateBook(book);
            Assert.False(this.libraryDbMock.Books.Count() == 0);
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
                                   this.author, new Author { FirstName = "Robert", LastName = "German", },
                               },
                Editions = new List<Edition> { this.edition },
                Categories = new List<Category> { this.category },
            };
            var result = this.bookService.CreateBook(book);
            Assert.True(this.libraryDbMock.Books.Count() == 1);
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
                Editions = new List<Edition> { this.edition },
                Categories = new List<Category> { this.category },
            };
            var result = this.bookService.CreateBook(book);
            Assert.False(result);
            Assert.True(this.libraryDbMock.Books.Count() == 0);
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
                Editions = new List<Edition> { this.edition },
                Categories = new List<Category> { this.category },
            };
            var result = this.bookService.CreateBook(book);
            Assert.False(result);
            Assert.True(this.libraryDbMock.Books.Count() == 0);
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
                Authors = new List<Author> { this.author },
                Editions = new List<Edition> { },
                Categories = new List<Category> { this.category },
            };
            var result = this.bookService.CreateBook(book);
            Assert.False(result);
            Assert.True(this.libraryDbMock.Books.Count() == 0);
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
                Authors = new List<Author> { this.author },
                Editions = null,
                Categories = new List<Category> { this.category },
            };
            var result = this.bookService.CreateBook(book);
            Assert.False(result);
            Assert.True(this.libraryDbMock.Books.Count() == 0);
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
                Authors = new List<Author> { this.author },
                Editions = new List<Edition> { this.edition, new Edition { Name = "Girofar" } },
                Categories = new List<Category> { this.category },
            };
            var result = this.bookService.CreateBook(book);
            Assert.True(this.libraryDbMock.Books.Count() == 1);
        }

        /// <summary>
        /// Test add book with related categories.
        /// </summary>
        [Test]
        public void TestAddBookWithRelatedCategories()
        {
            var book = new Book
            {
                Name = "Testing is important",
                Authors = new List<Author> { this.author },
                Editions = new List<Edition> { this.edition },
                Categories = new List<Category>
                                        {
                                            this.category,
                                            new Category { Name = "Science", ParentCategory = this.category, },
                                        },
            };
            var result = this.bookService.CreateBook(book);
            Assert.False(result);
            Assert.True(this.libraryDbMock.Books.Count() == 0);
        }

        /// <summary>
        /// Test add book with related indirect categories.
        /// </summary>
        [Test]
        public void TestAddBookWithRelatedIndirectCategories()
        {
            var book = new Book
            {
                Name = "Testing is important",
                Authors = new List<Author> { this.author },
                Editions = new List<Edition> { this.edition },
                Categories = new List<Category>
                                        {
                                            this.category,
                                            new Category
                                            {
                                                Name = "Science",
                                                ParentCategory =
                                                    new Category { ParentCategory = this.category, Name = "Comedy", },
                                            },
                                        },
            };
            var result = this.bookService.CreateBook(book);
            Assert.False(result);
            Assert.True(this.libraryDbMock.Books.Count() == 0);
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
                Authors = new List<Author> { this.author },
                Editions = new List<Edition> { this.edition },
                Categories = new List<Category> { this.category },
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
                Authors = new List<Author> { this.author },
                Editions = new List<Edition> { this.edition },
                Categories = new List<Category> { this.category },
            };
            this.bookService.CreateBook(book);
            book = this.bookService.GetBook("Fram ursul polar.");
            Assert.Null(book);
        }
    }
}