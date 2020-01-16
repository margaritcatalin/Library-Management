// <copyright file="CategoriesUnitTests.cs" company="Transilvania University of Brasov">
// Margarit Marian Catalin
// </copyright>

namespace LibraryManagementTests
{
    using System.Collections.Generic;
    using System.Linq;
    using NUnit.Framework;
    using LibraryManagement.BusinessLayer;
    using LibraryManagement.DataMapper;
    using LibraryManagement.DomainModel;
    using Telerik.JustMock.EntityFramework;

    /// <summary>
    /// Categories unit tests.
    /// </summary>
    [TestFixture]
    public class DomainsUnitTests
    {
        private DomainsService domainsService;

        private BookService bookService;

        private LibraryDbContext libraryContextMock;

        /// <summary>
        /// Testes setup.
        /// </summary>
        [SetUp]
        public void SetUp()
        {
            this.libraryContextMock = EntityFrameworkMock.Create<LibraryDbContext>();
            this.domainsService = new DomainsService(new DomainsRepository(this.libraryContextMock));
            this.bookService = new BookService(
    new BookRepository(this.libraryContextMock),
    new DomainsService(new DomainsRepository(this.libraryContextMock)),
    new ReaderRepository(this.libraryContextMock));
        }

        /// <summary>
        /// Test add domain witch is part of subdomain.
        /// </summary>
        [Test]
        public void TestDomainIsPartOfSubDomain()
        {
            var c1 = new Domain { Name = "C1" };
            var c2 = new Domain { Name = "C2" };
            var c3 = new Domain { Name = "C3" };
            var c4 = new Domain { Name = "C4", ParentDomain = c1 };

            var result = this.domainsService.DomainIsPartOfCategories(c4, new List<Domain> { c2, c3, c1 });

            Assert.True(result);
        }

        /// <summary>
        /// Test add a parent domain and it is part of subdomain.
        /// </summary>
        [Test]
        public void TestParentDomainIsPartOfSubDomain()
        {
            var c1 = new Domain { Name = "C1" };
            var c2 = new Domain { Name = "C2", ParentDomain = c1 };
            var c3 = new Domain { Name = "C3", ParentDomain = c1 };
            var c4 = new Domain { Name = "C4", ParentDomain = c1 };

            var result = this.domainsService.DomainIsPartOfCategories(c1, new List<Domain> { c2, c3, c4 });

            Assert.True(result);
        }

        /// <summary>
        /// Test add domain and it is not part of subdomain.
        /// </summary>
        [Test]
        public void TestDomainIsNotPartOfSubDomain()
        {
            var c1 = new Domain { Name = "C1" };
            var c2 = new Domain { Name = "C2" };
            var c3 = new Domain { Name = "C3" };

            var result = this.domainsService.DomainIsPartOfCategories(c1, new List<Domain> { c2, c3 });

            Assert.False(result);
        }

        /// <summary>
        /// Test add a null domain.
        /// </summary>
        [Test]
        public void AddNullDomain()
        {
            var result = this.domainsService.AddDomain(null);
            Assert.False(result);
            Assert.True(this.libraryContextMock.Categories.Count() == 0);
        }

        /// <summary>
        /// Test add a domain with null name.
        /// </summary>
        [Test]
        public void AddDomainWithNullName()
        {
            var c = new Domain { Name = null };
            var result = this.domainsService.AddDomain(c);
            Assert.False(result);
            Assert.True(this.libraryContextMock.Categories.Count() == 0);
        }

        /// <summary>
        /// Test add a domain with empty name.
        /// </summary>
        [Test]
        public void AddDomainWithEmptyName()
        {
            var c = new Domain { Name = string.Empty };
            var result = this.domainsService.AddDomain(c);
            Assert.False(result);
            Assert.True(this.libraryContextMock.Categories.Count() == 0);
        }

        /// <summary>
        /// Test add a domain with small name.
        /// </summary>
        [Test]
        public void AddDomainNameLengthLessThanThree()
        {
            var c = new Domain { Name = "Mi" };
            var result = this.domainsService.AddDomain(c);
            Assert.False(result);
            Assert.True(this.libraryContextMock.Categories.Count() == 0);
        }

        /// <summary>
        /// Test add a domain with bigger name.
        /// </summary>
        [Test]
        public void AddDomainNameLengthMoreThanLimitEighty()
        {
            var c = new Domain
            {
                Name = "LongLongLongLongLongLongLongLongLongLongLongLongLongLongLongLongLongLongLongLongLongLongLongLongLongLongLongLongLongLong",
            };
            var result = this.domainsService.AddDomain(c);
            Assert.False(result);
            Assert.True(this.libraryContextMock.Categories.Count() == 0);
        }

        /// <summary>
        /// Test add domain with digit in name.
        /// </summary>
        [Test]
        public void AddDomainNameWithDigit()
        {
            var c = new Domain { Name = "2425" };
            var result = this.domainsService.AddDomain(c);
            Assert.False(result);
            Assert.True(this.libraryContextMock.Categories.Count() == 0);
        }

        /// <summary>
        /// Test add domain with symbol in name.
        /// </summary>
        [Test]
        public void AddDomainNameWithSymbol()
        {
            var c = new Domain { Name = "@&abcd" };
            var result = this.domainsService.AddDomain(c);
            Assert.False(result);
            Assert.True(this.libraryContextMock.Categories.Count() == 0);
        }

        /// <summary>
        /// Test add domain with space in name.
        /// </summary>
        [Test]
        public void AddDomainNameWithWhiteSpace()
        {
            var c = new Domain { Name = "Action and Drama" };
            var result = this.domainsService.AddDomain(c);
            Assert.False(this.libraryContextMock.Categories.Count() == 0);
        }

        /// <summary>
        /// Test add a domain.
        /// </summary>
        [Test]
        public void AddDomain()
        {
            var c = new Domain { Name = "Fiction" };
            var result = this.domainsService.AddDomain(c);
            Assert.True(this.libraryContextMock.Categories.Count() == 1);
        }

        /// <summary>
        /// Test get a domain.
        /// </summary>
        [Test]
        public void GetDomain()
        {
            var c = new Domain { Name = "Fiction" };
            var result = this.domainsService.AddDomain(c);

            c = this.domainsService.GetDomain(c.Name);
            Assert.NotNull(c);
        }

        /// <summary>
        /// Test get a domain by null name.
        /// </summary>
        [Test]
        public void GetNullDomain()
        {
            var c = this.domainsService.GetDomain(null);
            Assert.Null(c);
        }

        /// <summary>
        /// Test get a domain by empty name.
        /// </summary>
        [Test]
        public void GetEmptyDomain()
        {
            var c = this.domainsService.GetDomain(string.Empty);
            Assert.Null(c);
        }

        /// <summary>
        /// Test get an unknown domain.
        /// </summary>
        [Test]
        public void GetUnknownDomain()
        {
            var c = new Domain { Name = "Fiction" };
            var result = this.domainsService.AddDomain(c);

            c = this.domainsService.GetDomain("Philosophy");
            Assert.Null(c);
        }

        /// <summary>
        /// Test add a subdomain.
        /// </summary>
        [Test]
        public void AddSubCateory()
        {
            Domain c = new Domain { Name = "Science" };
            Domain c2 = new Domain { Name = "Fiction", ParentDomain = c };
            var result = this.domainsService.AddDomain(c);
            var result2 = this.domainsService.AddDomain(c2);
            Assert.True(c2.ParentDomain != null);
            Assert.True(this.libraryContextMock.Categories.Count() == 2);
        }

        /// <summary>
        /// Test add a subdomain with no parent domain.
        /// </summary>
        [Test]
        public void AddSubCateoryWithNoParentDomain()
        {
            Domain c = new Domain { Name = "Fiction" };
            Domain c2 = new Domain { Name = "Science" };
            var result = this.domainsService.AddDomain(c);
            var result2 = this.domainsService.AddDomain(c2);
            Assert.False(c2.ParentDomain != null);
            Assert.True(this.libraryContextMock.Categories.Count() == 2);
        }

        /// <summary>
        /// Test is part of domain.
        /// </summary>
        [Test]
        public void IsPartOfDomain()
        {
            Domain c = new Domain { Name = "Fiction" };
            var result = this.domainsService.AddDomain(c);

            Book book = new Book
            {
                Name = "Testing is important",
                Authors = new List<Author> { new Author { FirstName = "Estera", LastName = "Balas" } },
                Editions = new List<Edition>
                {
                    new Edition
            {
                Name = "Corint",
                BookType = "Plasticcover",
                Pages = 256,
                BookStock = new BookStock
            {
                Amount = 100,
                LectureRoomAmount = 10,
            },
            },
                },
                Categories = new List<Domain> { c },
            };
            var resultB = this.bookService.CreateBook(book);
            Assert.True(this.domainsService.IsPartOfDomain(book, c));
            Assert.True(this.libraryContextMock.Categories.Count() == 1 && this.libraryContextMock.Books.Count() == 1);
        }

        /// <summary>
        /// Test if is not part of domain.
        /// </summary>
        [Test]
        public void IsNotPartOfDomain()
        {
            Domain c = new Domain { Name = "Fiction" };
            var result = this.domainsService.AddDomain(c);
            Domain c2 = new Domain { Name = "Test" };
            var result2 = this.domainsService.AddDomain(c2);
            Book book = new Book
            {
                Name = "Testing is important",
                Authors = new List<Author> { new Author { FirstName = "Estera", LastName = "Balas" } },
                Editions = new List<Edition>
                {
                    new Edition
            {
                Name = "Corint",
                BookType = "Plasticcover",
                Pages = 256,
                BookStock = new BookStock
            {
                Amount = 100,
                LectureRoomAmount = 10,
            },
            },
                },
                Categories = new List<Domain> { c },
            };
            var resultB = this.bookService.CreateBook(book);
            Assert.False(this.domainsService.IsPartOfDomain(book, c2));
            Assert.True(this.libraryContextMock.Categories.Count() == 2 && this.libraryContextMock.Books.Count() == 1);
        }

        /// <summary>
        /// Test check if is not part of parent domain.
        /// </summary>
        [Test]
        public void IsNotPartOfParentDomain()
        {
            Domain c = new Domain { Name = "Fiction" };
            var result = this.domainsService.AddDomain(c);
            Domain c1 = new Domain { Name = "Science2" };
            var result1 = this.domainsService.AddDomain(c1);
            Domain c2 = new Domain { Name = "Test", ParentDomain = c };
            var result2 = this.domainsService.AddDomain(c2);
            Book book = new Book
            {
                Name = "Testing is important",
                Authors = new List<Author> { new Author { FirstName = "Estera", LastName = "Balas" } },
                Editions = new List<Edition>
                {
                    new Edition
            {
                Name = "Corint",
                BookType = "Plasticcover",
                Pages = 256,
                BookStock = new BookStock
            {
                Amount = 100,
                LectureRoomAmount = 10,
            },
            },
                },
                Categories = new List<Domain> { c1 },
            };
            var resultB = this.bookService.CreateBook(book);
            Assert.False(this.domainsService.IsPartOfDomain(book, c));
            Assert.True(this.libraryContextMock.Categories.Count() == 2 && this.libraryContextMock.Books.Count() == 1);
        }

        /// <summary>
        /// Test is part of parent domain.
        /// </summary>
        [Test]
        public void IsPartOfParentDomain()
        {
            Domain c = new Domain { Name = "Fiction" };
            var result = this.domainsService.AddDomain(c);
            Domain c2 = new Domain { Name = "Test", ParentDomain = c };
            var result2 = this.domainsService.AddDomain(c2);
            Book book = new Book
            {
                Name = "Testing is important",
                Authors = new List<Author> { new Author { FirstName = "Estera", LastName = "Balas" } },
                Editions = new List<Edition>
                {
                    new Edition
            {
                Name = "Corint",
                BookType = "Plasticcover",
                Pages = 256,
                BookStock = new BookStock
            {
                Amount = 100,
                LectureRoomAmount = 10,
            },
            },
                },
                Categories = new List<Domain> { c2 },
            };
            var resultB = this.bookService.CreateBook(book);
            Assert.True(this.domainsService.IsPartOfDomain(book, c));
            Assert.True(this.libraryContextMock.Categories.Count() == 2 && this.libraryContextMock.Books.Count() == 1);
        }

        /// <summary>
        /// Test is part of null domains.
        /// </summary>
        [Test]
        public void IsPartOfDomainNullDomain()
        {
            Domain c = new Domain { Name = "Fiction" };
            var result = this.domainsService.AddDomain(c);

            Book book = new Book
            {
                Name = "Testing is important",
                Authors = new List<Author> { new Author { FirstName = "Estera", LastName = "Balas" } },
                Editions = new List<Edition>
                {
                    new Edition
            {
                Name = "Corint",
                BookType = "Plasticcover",
                Pages = 256,
                BookStock = new BookStock
            {
                Amount = 100,
                LectureRoomAmount = 10,
            },
            },
                },
                Categories = new List<Domain> { c },
            };
            var resultB = this.bookService.CreateBook(book);
            Assert.False(this.domainsService.IsPartOfDomain(book, null));
            Assert.True(this.libraryContextMock.Categories.Count() == 1 && this.libraryContextMock.Books.Count() == 1);
        }

        /// <summary>
        /// Test domain is part of domains.
        /// </summary>
        [Test]
        public void DomainIsPartOfCategoriesNotNullDomain()
        {
            Domain c = new Domain { Name = "Fiction" };
            Domain c2 = new Domain { Name = "Test", ParentDomain = c };
            var result = this.domainsService.AddDomain(c);
            var result2 = this.domainsService.AddDomain(c2);
            List<Domain> domains = new List<Domain>
            {
                c, c2
            };
            Assert.True(this.domainsService.DomainIsPartOfCategories(c, domains));
            Assert.True(this.libraryContextMock.Categories.Count() == 2);
        }

        /// <summary>
        /// Test domains is part off null domains.
        /// </summary>
        [Test]
        public void DomainIsPartOfCategoriesNullDomain()
        {
            Domain c = new Domain { Name = "Fiction" };
            Domain c2 = new Domain { Name = "Test", ParentDomain = c };
            var result = this.domainsService.AddDomain(c);
            var result2 = this.domainsService.AddDomain(c2);
            List<Domain> domains = new List<Domain>
            {
                c, c2
            };
            Assert.False(this.domainsService.DomainIsPartOfCategories(null, domains));
            Assert.True(this.libraryContextMock.Categories.Count() == 2);
        }

        /// <summary>
        /// Testdomain is not part of domains.
        /// </summary>
        [Test]
        public void DomainIsNotPartOfCategories()
        {
            Domain c = new Domain { Name = "Fiction" };
            Domain c2 = new Domain { Name = "Test", ParentDomain = c };
            Domain c1 = new Domain { Name = "Glob" };
            var result = this.domainsService.AddDomain(c);
            var result1 = this.domainsService.AddDomain(c1);
            var result2 = this.domainsService.AddDomain(c2);
            List<Domain> domains = new List<Domain>
            {
                c, c2
            };
            Assert.False(this.domainsService.DomainIsPartOfCategories(c1, domains));
            Assert.True(this.libraryContextMock.Categories.Count() == 3);
        }
    }
}