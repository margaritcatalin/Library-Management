using System;
using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;
using PublicLibrary.BusinessLayer;
using PublicLibrary.Data_Mapper;
using PublicLibrary.Domain_Model;
using Telerik.JustMock.EntityFramework;

namespace PublicLibraryTests
{
    [TestFixture]
    public class EditionUnitTests
    {
        private LibraryDb _libraryDbMock;
        private BookService _bookService;
        private Book _book;
        [SetUp]
        public void SetUp()
        {
            _libraryDbMock = EntityFrameworkMock.Create<LibraryDb>();
            _bookService = new BookService(new BookRepository(_libraryDbMock),
                new CategoriesService(new CategoriesRepository(_libraryDbMock)), new ReaderRepository(_libraryDbMock));

            var edition = new Edition{Name = "First Edition"};
            _book = new Book
            {
                Name = "Moara cu Noroc",
                Authors = new List<Author>()
                {
                    new Author{Name = "Ioan Slavici"}
                },
                Categories = new List<Category>
                {
                    new Category{Name = "Novel"}
                },
                Editions = new List<Edition>
                {
                    edition
                },
            };
            _libraryDbMock.Editions.Add(edition);
            _bookService.CreateBook(_book);

        }

        [Test]
        public void TestAddEditionNullBook()
        {
            var edition = new Edition
            {
                Name = "Second edition",
                BookType = "Hard cover",
                Pages = 100
            };
            _bookService.AddEdition(null, edition);
            Assert.True(_book.Editions.Count() == 1);
        }
        [Test]
        public void TestAddNullEdition()
        {
            _bookService.AddEdition(_book, null);
            Assert.True(_book.Editions.Count() == 1);
        }
        [Test]
        public void TestAddEdition()
        {
            var edition = new Edition
            {
                Name = "Second edition",
                BookType = "Hard cover",
                Pages = 100
            };
            _bookService.AddEdition(_book, edition);
            Assert.True(_book.Editions.Count() == 2);
        }
        [Test]
        public void TestAddEditionNullName()
        {
            var edition = new Edition
            {
                Name = null,
                BookType = "Hard cover",
                Pages = 100
            };
            _bookService.AddEdition(_book, edition);
            Assert.True(_book.Editions.Count() == 1);
        }
        [Test]
        public void TestAddEditionEmptyName()
        {
            var edition = new Edition
            {
                Name = "",
                BookType = "Hard cover",
                Pages = 100
            };
            _bookService.AddEdition(_book, edition);
            Assert.True(_book.Editions.Count() == 1);
        }
        [Test]
        public void TestAddEditionSmallName()
        {
            var edition = new Edition
            {
                Name = "Aa",
                BookType = "Hard cover",
                Pages = 100
            };
            _bookService.AddEdition(_book, edition);
            Assert.True(_book.Editions.Count() == 1);
        }
        [Test]
        public void TestAddEditionLongName()
        {
            var edition = new Edition
            {
                Name = "AAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAA",
                BookType = "Hard cover",
                Pages = 100
            };
            _bookService.AddEdition(_book, edition);
            Assert.True(_book.Editions.Count() == 1);
        }
        [Test]
        public void TestAddEditionNameSymbol()
        {
            var edition = new Edition
            {
                Name = "Second Edition@$@$@%%*&",
                BookType = "Hard cover",
                Pages = 100
            };
            _bookService.AddEdition(_book, edition);
            Assert.True(_book.Editions.Count() == 1);
        }
        [Test]
        public void TestAddEditionNameLowerCase()
        {
            var edition = new Edition
            {
                Name = "second Edition",
                BookType = "Hard cover",
                Pages = 100
            };
            _bookService.AddEdition(_book, edition);
            Assert.True(_book.Editions.Count() == 1);
        }

        [Test]
        public void TestAddEditionNullType()
        {
            var edition = new Edition
            {
                Name = "Second Edition",
                BookType = null,
                Pages = 100
            };
            _bookService.AddEdition(_book, edition);
            Assert.True(_book.Editions.Count() == 1);
        }
        [Test]
        public void TestAddEditionEmptyType()
        {
            var edition = new Edition
            {
                Name = "Second Edition",
                BookType = "",
                Pages = 100
            };
            _bookService.AddEdition(_book, edition);
            Assert.True(_book.Editions.Count() == 1);
        }
        [Test]
        public void TestAddEditionSmallType()
        {
            var edition = new Edition
            {
                Name = "Second Edition",
                BookType = "Aa",
                Pages = 100
            };
            _bookService.AddEdition(_book, edition);
            Assert.True(_book.Editions.Count() == 1);
        }
        [Test]
        public void TestAddEditionLongType()
        {
            var edition = new Edition
            {
                Name = "Second Edition",
                BookType = "AAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAA",
                Pages = 100
            };
            _bookService.AddEdition(_book, edition);
            Assert.True(_book.Editions.Count() == 1);
        }
        [Test]
        public void TestAddEditionTypeSymbol()
        {
            var edition = new Edition
            {
                Name = "Second Edition",
                BookType = "Hardcover@#%(#@*&$(*%",
                Pages = 100
            };
            _bookService.AddEdition(_book, edition);
            Assert.True(_book.Editions.Count() == 1);
        }
        [Test]
        public void TestAddEditionTypeDigit()
        {
            var edition = new Edition
            {
                Name = "Second Edition",
                BookType = "Hardcover23",
                Pages = 100
            };
            _bookService.AddEdition(_book, edition);
            Assert.True(_book.Editions.Count() == 1);
        }
        [Test]
        public void TestAddEditionTypeWhiteSpace()
        {
            var edition = new Edition
            {
                Name = "Second Edition",
                BookType = "Hardcover slim",
                Pages = 100
            };
            _bookService.AddEdition(_book, edition);
            Assert.True(_book.Editions.Count() == 2);
        }
        [Test]
        public void TestAddEditionNegativeNumberOfPages()
        {
            var edition = new Edition
            {
                Name = "Second Edition",
                BookType = "Hardcover slim",
                Pages = -10
            };
            _bookService.AddEdition(_book, edition);
            Assert.True(_book.Editions.Count() == 1);
        }
        [Test]
        public void TestAddEditionZeroPages()
        {
            var edition = new Edition
            {
                Name = "Second Edition",
                BookType = "Hardcover slim",
                Pages = 0
            };
            _bookService.AddEdition(_book, edition);
            Assert.True(_book.Editions.Count() == 1);
        }
        [Test]
        public void TestAddEditionTooManyPages()
        {
            var edition = new Edition
            {
                Name = "Second Edition",
                BookType = "Hardcover slim",
                Pages = 99999999
            };
            _bookService.AddEdition(_book, edition);
            Assert.True(_book.Editions.Count() == 1);
        }
        [Test]
        public void TestAddEditionTypeLowercase()
        {
            var edition = new Edition
            {
                Name = "Second Edition",
                BookType = "hardcover slim",
                Pages = 99999999
            };
            _bookService.AddEdition(_book, edition);
            Assert.True(_book.Editions.Count() == 1);
        }
    }
}
