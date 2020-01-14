// <copyright file="AuthorUnitTests.cs" company="Transilvania University of Brasov">
// Margarit Marian Catalin
// </copyright>

namespace PublicLibraryTests
{
    using System.Linq;
    using NUnit.Framework;
    using PublicLibrary.BusinessLayer;
    using PublicLibrary.Data_Mapper;
    using Telerik.JustMock.EntityFramework;

    /// <summary>
    /// The author unit tests.
    /// </summary>
    [TestFixture]
    public class AuthorUnitTests
    {
        private LibraryDbContext libraryContextMock;

        private AuthorService authorService;

        /// <summary>
        /// Tests setup.
        /// </summary>
        [SetUp]
        public void SetUp()
        {
            this.libraryContextMock = EntityFrameworkMock.Create<LibraryDbContext>();
            this.authorService = new AuthorService(new AuthorRepository(this.libraryContextMock));
        }

        /// <summary>
        /// Test add a new author.
        /// </summary>
        [Test]
        public void TestAddAuthor()
        {
            var author = new Author { FirstName = "Estera", LastName = "Balas", Gender = "M" };
            var result = this.authorService.AddAuthor(author);
            Assert.True(this.libraryContextMock.Authors.Count() == 1);
        }

        /// <summary>
        /// Test add a null author.
        /// </summary>
        [Test]
        public void TestAddNullAuthor()
        {
            var result = this.authorService.AddAuthor(null);
            Assert.True(this.libraryContextMock.Authors.Count() == 0);
        }

        /// <summary>
        /// Test add an author with null firstname.
        /// </summary>
        [Test]
        public void TestAddNullFirstNameAuthor()
        {
            var author = new Author { FirstName = null, LastName = "Balas", Gender = "M" };
            var result = this.authorService.AddAuthor(author);
            Assert.True(this.libraryContextMock.Authors.Count() == 0);
        }

        /// <summary>
        /// Test add add author with empty firstName.
        /// </summary>
        [Test]
        public void TestAddEmptyFirstNameAuthor()
        {
            var author = new Author { FirstName = string.Empty, LastName = "Balas", Gender = "M" };
            var result = this.authorService.AddAuthor(author);
            Assert.True(this.libraryContextMock.Authors.Count() == 0);
        }

        /// <summary>
        /// Test add author with smaller firstName.
        /// </summary>
        [Test]
        public void TestAddSmallerFirstNameAuthor()
        {
            var author = new Author { FirstName = "Aa", LastName = "Balas", Gender = "M" };
            var result = this.authorService.AddAuthor(author);
            Assert.True(this.libraryContextMock.Authors.Count() == 0);
        }

        /// <summary>
        /// Test add author with longer firstName.
        /// </summary>
        [Test]
        public void TestAddLongerFirstNameAuthor()
        {
            var author = new Author
                         {
                             FirstName =
                                 "LongLongLongLongLongLongLongLongLongLongLongLongLongLongLongLongLongLongLongLongLongLongLongLongLongLongLongLongLongLong",
                             LastName = "Balas",
                             Gender = "M",
                         };
            var result = this.authorService.AddAuthor(author);
            Assert.True(this.libraryContextMock.Authors.Count() == 0);
        }

        /// <summary>
        /// Test add author with lower firstname.
        /// </summary>
        [Test]
        public void TestAddLowerFirstNameAuthor()
        {
            var author = new Author { FirstName = "gigel", LastName = "Frone", Gender = "M" };
            var result = this.authorService.AddAuthor(author);
            Assert.True(this.libraryContextMock.Authors.Count() == 0);
        }

        /// <summary>
        /// Add author with digit in firstName.
        /// </summary>
        [Test]
        public void TestAddDigitFirstNameAuthor()
        {
            var author = new Author { FirstName = "Al13Mihai", LastName = "Cioran", Gender = "M" };
            var result = this.authorService.AddAuthor(author);
            Assert.True(this.libraryContextMock.Authors.Count() == 0);
        }

        /// <summary>
        /// Test add an author with firstname with symbols.
        /// </summary>
        [Test]
        public void TestAddSymbolFirstNameAuthor()
        {
            var author = new Author { FirstName = "Aly$^@*#^*", LastName = "Baba", Gender = "M" };
            var result = this.authorService.AddAuthor(author);
            Assert.True(this.libraryContextMock.Authors.Count() == 0);
        }

        /// <summary>
        /// Test add author with spaces in firstName.
        /// </summary>
        [Test]
        public void TestAddWhiteSpaceFirstNameAuthor()
        {
            var author = new Author { FirstName = "Aly Baba", LastName = "Ionel", Gender = "M" };
            var result = this.authorService.AddAuthor(author);
            Assert.True(this.libraryContextMock.Authors.Count() == 1);
        }

        /// <summary>
        /// Test add author with dash in first name.
        /// </summary>
        [Test]
        public void TestAddDashFirstNameAuthor()
        {
            var author = new Author { FirstName = "Aly-Baba", LastName = "Kalu", Gender = "M" };
            var result = this.authorService.AddAuthor(author);
            Assert.True(this.libraryContextMock.Authors.Count() == 1);
        }

        /// <summary>
        /// Test add author with null last name.
        /// </summary>
        [Test]
        public void TestAddNullLastNameAuthor()
        {
            var author = new Author { FirstName = "Horatiu", LastName = null, Gender = "M" };
            var result = this.authorService.AddAuthor(author);
            Assert.True(this.libraryContextMock.Authors.Count() == 0);
        }

        /// <summary>
        /// Test add author with empty last name.
        /// </summary>
        [Test]
        public void TestAddEmptyLastNameAuthor()
        {
            var author = new Author { FirstName = "Horatiu", LastName = string.Empty, Gender = "M" };
            var result = this.authorService.AddAuthor(author);
            Assert.True(this.libraryContextMock.Authors.Count() == 0);
        }

        /// <summary>
        /// Test added author with smaller lastName.
        /// </summary>
        [Test]
        public void TestAddSmallerLastNameAuthor()
        {
            var author = new Author { FirstName = "Horatiu", LastName = "small", Gender = "M" };
            var result = this.authorService.AddAuthor(author);
            Assert.True(this.libraryContextMock.Authors.Count() == 0);
        }

        /// <summary>
        /// Test add author with longer lastname.
        /// </summary>
        [Test]
        public void TestAddLongerLastNameAuthor()
        {
            var author = new Author
                         {
                             FirstName = "Horatiu",
                             LastName =
                                 "LongLongLongLongLongLongLongLongLongLongLongLongLongLongLongLongLongLongLongLongLongLongLongLongLongLongLongLongLong",
                             Gender = "M",
                         };
            var result = this.authorService.AddAuthor(author);
            Assert.True(this.libraryContextMock.Authors.Count() == 0);
        }

        /// <summary>
        /// Test add author with digit lastName.
        /// </summary>
        [Test]
        public void TestAddDigitLastNameAuthor()
        {
            var author = new Author { FirstName = "Fratele", LastName = "Galusca5", Gender = "M" };
            var result = this.authorService.AddAuthor(author);
            Assert.True(this.libraryContextMock.Authors.Count() == 0);
        }

        /// <summary>
        /// Test add author with symbol in lastname.
        /// </summary>
        [Test]
        public void TestAddSymbolLastNameAuthor()
        {
            var author = new Author { FirstName = "Horatiu", LastName = "Vlad$@%^@#%$)_*", Gender = "M" };
            var result = this.authorService.AddAuthor(author);
            Assert.True(this.libraryContextMock.Authors.Count() == 0);
        }

        /// <summary>
        /// Test add author with lowercase lastname.
        /// </summary>
        [Test]
        public void TestAddLowercaseLastNameAuthor()
        {
            var author = new Author { FirstName = "Case", LastName = "lower", Gender = "M" };
            var result = this.authorService.AddAuthor(author);
            Assert.True(this.libraryContextMock.Authors.Count() == 0);
        }

        /// <summary>
        /// Add author with space in lastName.
        /// </summary>
        [Test]
        public void TestAddWhiteSpaceLastNameAuthor()
        {
            var author = new Author { FirstName = "Estera", LastName = "Aly Baba", Gender = "M" };
            var result = this.authorService.AddAuthor(author);
            Assert.True(this.libraryContextMock.Authors.Count() == 1);
        }

        /// <summary>
        /// Test add author with dash in lastName.
        /// </summary>
        [Test]
        public void TestAddDashLastNameAuthor()
        {
            var author = new Author { FirstName = "Galusca", LastName = "Aly-Baba", Gender = "M" };
            var result = this.authorService.AddAuthor(author);
            Assert.True(this.libraryContextMock.Authors.Count() == 1);
        }

        /// <summary>
        /// Test add author with null gender.
        /// </summary>
        [Test]
        public void TestAddNullGenderAuthor()
        {
            var author = new Author { FirstName = "Estera", LastName = "Balas", Gender = null };
            var result = this.authorService.AddAuthor(author);
            Assert.True(this.libraryContextMock.Authors.Count() == 0);
        }

        /// <summary>
        /// Test add author with empty gender.
        /// </summary>
        [Test]
        public void TestAddEmptyGenderAuthor()
        {
            var author = new Author { FirstName = "Estera", LastName = "Balas", Gender = string.Empty };
            var result = this.authorService.AddAuthor(author);
            Assert.True(this.libraryContextMock.Authors.Count() == 0);
        }

        /// <summary>
        /// Test add author with gender Male.
        /// </summary>
        [Test]
        public void TestAddMGenderAuthor()
        {
            var author = new Author { FirstName = "Estera", LastName = "Balas", Gender = "M" };
            var result = this.authorService.AddAuthor(author);
            Assert.True(this.libraryContextMock.Authors.Count() == 1);
        }

        /// <summary>
        /// Test add author with gender Female.
        /// </summary>
        [Test]
        public void TestAddFGenderAuthor()
        {
            var author = new Author { FirstName = "Estera", LastName = "Balas", Gender = "F" };
            var result = this.authorService.AddAuthor(author);
            Assert.True(this.libraryContextMock.Authors.Count() == 1);
        }

        /// <summary>
        /// Test add author with invalid gender.
        /// </summary>
        [Test]
        public void TestAddBadGenderAuthor()
        {
            var author = new Author { FirstName = "Estera", LastName = "Balas", Gender = "C" };
            var result = this.authorService.AddAuthor(author);
            Assert.True(this.libraryContextMock.Authors.Count() == 0);
        }

        /// <summary>
        /// Test get author.
        /// </summary>
        [Test]
        public void TestGetAuthor()
        {
            var author = new Author { FirstName = "Estera", LastName = "Balas", Gender = "M" };
            var result = this.authorService.AddAuthor(author);
            author = this.authorService.GetAuthor(author.FirstName, author.LastName);
            Assert.NotNull(author);
        }

        /// <summary>
        /// Test get author with null firstname.
        /// </summary>
        [Test]
        public void TestGetNullFirstNameAuthor()
        {
            var author = new Author { FirstName = "Estera", LastName = "Balas", Gender = "M" };
            var result = this.authorService.AddAuthor(author);
            author = this.authorService.GetAuthor(null, author.LastName);
            Assert.Null(author);
        }

        /// <summary>
        /// Test get author with null lastName.
        /// </summary>
        [Test]
        public void TestGetNullLastNameAuthor()
        {
            var author = new Author { FirstName = "Estera", LastName = "Balas", Gender = "M" };
            var result = this.authorService.AddAuthor(author);
            author = this.authorService.GetAuthor(author.FirstName, null);
            Assert.Null(author);
        }

        /// <summary>
        /// Test get Author with empty firstName.
        /// </summary>
        [Test]
        public void TestGetEmptyFirstNameAuthor()
        {
            var author = new Author { FirstName = "Estera", LastName = "Balas", Gender = "M" };
            var result = this.authorService.AddAuthor(author);
            author = this.authorService.GetAuthor(string.Empty, author.LastName);
            Assert.Null(author);
        }

        /// <summary>
        /// Test get author with empty lastName.
        /// </summary>
        [Test]
        public void TestGetEmptyLastNameAuthor()
        {
            var author = new Author { FirstName = "Estera", LastName = "Balas", Gender = "M" };
            var result = this.authorService.AddAuthor(author);
            author = this.authorService.GetAuthor(author.FirstName, string.Empty);
            Assert.Null(author);
        }

        /// <summary>
        /// Test get author and he is not in db.
        /// </summary>
        [Test]
        public void TestGetBadNameAuthor()
        {
            var author = new Author { FirstName = "Estera", LastName = "Balas", Gender = "M" };
            var result = this.authorService.AddAuthor(author);
            author = this.authorService.GetAuthor("Estera", "Rebreanu");
            Assert.Null(author);
        }
    }
}