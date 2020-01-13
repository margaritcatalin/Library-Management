using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.Entity;
using System.Linq;
using Moq;
using NUnit.Framework;
using PublicLibrary.BusinessLayer;
using PublicLibrary.Data_Mapper;
using PublicLibrary.Domain_Model;
using Telerik.JustMock.EntityFramework;

namespace PublicLibraryTests
{
    [TestFixture]
    public class BookUnitTests
    {
        private Edition _edition;
        private Author _author;
        private BookService _bookService;
        private Category _category;
        private LibraryDb _libraryDbMock;

        [SetUp]
        public void SetUp()
        {
            BookStock bookStock = new BookStock
            {
                Amount = 100,
                LectureRoomAmount = 10
            };
            _edition = new Edition
            {
                Name = "Teora",
                BookType = "Hardcover",
                Pages = 256,
                BookStock = bookStock
            };
            _author = new Author
            {
                Name = "Ioan Slavici"
            };
            _category = new Category
            {
                Name = "Drama",
            };
            _libraryDbMock = EntityFrameworkMock.Create<LibraryDb>();
            EntityFrameworkMock.PrepareMock(_libraryDbMock);
            _bookService = new BookService(new BookRepository(_libraryDbMock), new CategoriesService(new CategoriesRepository(_libraryDbMock)),
                new ReaderRepository(_libraryDbMock));
/*            var edition2 = new Edition
            {
                Name = "All",
                BookType = "Hardcover",
                Pages = 256,
                BookStock = bookStock2
            };*/

            /*

                        Book book2 = new Book
                        {
                            Name = "Padurea spanzuratilor",
                            Authors = new List<Author>
                            {
                                new Author
                                {
                                    Name = "Lucian Blaga"
                                }
                            },
                            Editions = new List<Edition>
                            {
                                edition2
                            },
                            Categories = new List<Category>
                            {
                                new Category
                                {
                                    Name = "",
                                    ParentCategory = parentCategory
                                }
                            }
                        };*/
        }

        [Test]
        public void TestAddNullBook()
        {
            Book book = null;
            var result = _bookService.CreateBook(book);
            Assert.False(result);
            Assert.True(_libraryDbMock.Books.Count() == 0);
        }

        [Test]
        public void TestAddBookWithNullCategories()
        {
            Book book = new Book
            {
                Name = "Moara cu Noroc",
                Authors = new List<Author> {_author},
                Editions = new List<Edition> {_edition},
                Categories = null
            };
            var result = _bookService.CreateBook(book);
            Assert.False(result);
            Assert.True(_libraryDbMock.Books.Count() == 0);
        }

        [Test]
        public void TestAddBookWithNoCategory()
        {
            Book book = new Book
            {
                Name = "Moara cu noroc",
                Authors = new List<Author> {_author},
                Editions = new List<Edition> {_edition},
                Categories = new List<Category>()
            };
            var result = _bookService.CreateBook(book);
            Assert.False(result);
        }

        [Test]
        public void TestAddBookWithOneCategory()
        {
            Book book = new Book
            {
                Name = "Moara cu noroc",
                Authors = new List<Author> {_author},
                Editions = new List<Edition> {_edition},
                Categories = new List<Category> {_category}
            };
            _bookService.CreateBook(book);
            Assert.True(_libraryDbMock.Books.Count() == 1);
        }

        [Test]
        public void TestAddBookWithMoreThanDOMCategories()
        {
            var DOM = int.Parse(ConfigurationManager.AppSettings["DOM"]);
            DOM = DOM + 1;
            var categoriesList = new List<Category>();
            for (int i = 0; i < DOM; i++)
            {
                categoriesList.Add(new Category {Name = i.ToString()});
            }

            Book book = new Book
            {
                Name = "Moara cu noroc",
                Authors = new List<Author> {_author},
                Editions = new List<Edition> {_edition},
                Categories = categoriesList
            };
            var result = _bookService.CreateBook(book);
            Assert.False(result);
            Assert.True(_libraryDbMock.Books.Count() == 0);
        }

        [Test]
        public void TestAddBookWithEmptyName()
        {
            Book book = new Book
            {
                Name = "",
                Authors = new List<Author> {_author},
                Editions = new List<Edition> {_edition},
                Categories = new List<Category> {_category}
            };
            var result = _bookService.CreateBook(book);
            Assert.False(result);
            Assert.True(_libraryDbMock.Books.Count() == 0);
        }

        [Test]
        public void TestAddBookWithNoName()
        {
            Book book = new Book
            {
                Name = null,
                Authors = new List<Author> {_author},
                Editions = new List<Edition> {_edition},
                Categories = new List<Category> {_category}
            };
            var result = _bookService.CreateBook(book);
            Assert.False(result);
            Assert.True(_libraryDbMock.Books.Count() == 0);
        }

        [Test]
        public void TestAddBookWithNameLengthLessThanThree()
        {
            Book book = new Book
            {
                Name = "ab",
                Authors = new List<Author> { _author },
                Editions = new List<Edition> { _edition },
                Categories = new List<Category> { _category }
            };
            var result = _bookService.CreateBook(book);
            Assert.False(result);
            Assert.True(_libraryDbMock.Books.Count() == 0);
        }

        [Test]
        public void TestAddBookWithNameLengthBiggerThanEighy()
        {
            Book book = new Book
            {
                Name = "anaaremereanaaremereanaaremer" +
                       "anaaremere" +
                       "anaaremereanaaremereanaaremere" +
                       "anaaremereanaaremere" +
                       "anaaremereanaaremere" +
                       "anaaremereanaaremere" +
                       "anaaremereanaaremere" +
                       "anaaremereanaaremere" +
                       "eanaaremereanaaremereanaaremere",
                Authors = new List<Author> { _author },
                Editions = new List<Edition> { _edition },
                Categories = new List<Category> { _category }
            };
            var result = _bookService.CreateBook(book);
            Assert.False(result);
            Assert.True(_libraryDbMock.Books.Count() == 0);
        }

        [Test]
        public void TestAddBookNameWithDigits()
        {
            Book book = new Book
            {
                Name = "34",
                Authors = new List<Author> { _author },
                Editions = new List<Edition> { _edition },
                Categories = new List<Category> { _category }
            };
            var result = _bookService.CreateBook(book);
            Assert.False(result);
            Assert.True(_libraryDbMock.Books.Count() == 0);
        }

        [Test]
        public void TestAddBookNameWithSymbols()
        {
            Book book = new Book
            {
                Name = "*&()_",
                Authors = new List<Author> { _author },
                Editions = new List<Edition> { _edition },
                Categories = new List<Category> { _category }
            };
            var result = _bookService.CreateBook(book);
            Assert.False(result);
            Assert.True(_libraryDbMock.Books.Count() == 0);
        }

        [Test]
        public void TestAddBookWithNameContainsWhitespaces()
        {
            Book book = new Book
            {
                Name = "Toate panzele sus",
                Authors = new List<Author> { _author },
                Editions = new List<Edition> { _edition },
                Categories = new List<Category> { _category }
            };
            var result = _bookService.CreateBook(book);
            Assert.False(_libraryDbMock.Books.Count() == 0);
        }
        [Test]
        public void TestAddBookWithTwoAuthors()
        {
            Book book = new Book
            {
                Name = "Moara cu Noroc",
                Authors = new List<Author> {_author, new Author {Name = "Mihail Sadoveanu"}},
                Editions = new List<Edition> {_edition},
                Categories = new List<Category> {_category}
            };
            var result = _bookService.CreateBook(book);
            Assert.True(_libraryDbMock.Books.Count() == 1);
        }

        [Test]
        public void TestAddBookWithNoAuthors()
        {
            Book book = new Book
            {
                Name = "Moara cu Noroc",
                Authors = new List<Author> { },
                Editions = new List<Edition> {_edition},
                Categories = new List<Category> {_category}
            };
            var result = _bookService.CreateBook(book);
            Assert.False(result);
            Assert.True(_libraryDbMock.Books.Count() == 0);
        }

        [Test]
        public void TestAddBookWithNullAuthors()
        {
            Book book = new Book
            {
                Name = "Moara cu Noroc",
                Authors = null,
                Editions = new List<Edition> {_edition},
                Categories = new List<Category> {_category}
            };
            var result = _bookService.CreateBook(book);
            Assert.False(result);
            Assert.True(_libraryDbMock.Books.Count() == 0);
        }

        [Test]
        public void TestAddBookWithNoEditions()
        {
            Book book = new Book
            {
                Name = "Moara cu Noroc",
                Authors = new List<Author> {_author},
                Editions = new List<Edition> { },
                Categories = new List<Category> {_category}
            };
            var result = _bookService.CreateBook(book);
            Assert.False(result);
            Assert.True(_libraryDbMock.Books.Count() == 0);
        }

        [Test]
        public void TestAddBookWithNullEditions()
        {
            Book book = new Book
            {
                Name = "Moara cu Noroc",
                Authors = new List<Author> {_author},
                Editions = null,
                Categories = new List<Category> {_category}
            };
            var result = _bookService.CreateBook(book);
            Assert.False(result);
            Assert.True(_libraryDbMock.Books.Count() == 0);
        }

        [Test]
        public void TestAddBookWithTwoEditions()
        {
            Book book = new Book
            {
                Name = "Moara cu Noroc",
                Authors = new List<Author> {_author},
                Editions = new List<Edition> {_edition, new Edition {Name = "Marta"}},
                Categories = new List<Category> {_category}
            };
            var result = _bookService.CreateBook(book);
            Assert.True(_libraryDbMock.Books.Count() == 1);
        }

        [Test]
        public void TestAddBookWithRelatedCategories()
        {
            Book book = new Book
            {
                Name = "Moara cu Noroc",
                Authors = new List<Author> {_author},
                Editions = new List<Edition> {_edition},
                Categories = new List<Category> {_category, new Category {Name = "Romance", ParentCategory = _category}}
            };
            var result = _bookService.CreateBook(book);
            Assert.False(result);
            Assert.True(_libraryDbMock.Books.Count() == 0);
        }

        [Test]
        public void TestAddBookWithRelatedIndirectCategories()
        {
            Book book = new Book
            {
                Name = "Moara cu Noroc",
                Authors = new List<Author> {_author},
                Editions = new List<Edition> {_edition},
                Categories = new List<Category>
                {
                    _category, new Category
                    {
                        Name = "Romance", ParentCategory = new Category
                        {
                            ParentCategory = _category,
                            Name = "Horror"
                        }
                    }
                }
            };
            var result = _bookService.CreateBook(book);
            Assert.False(result);
            Assert.True(_libraryDbMock.Books.Count() == 0);
        }
    }
}