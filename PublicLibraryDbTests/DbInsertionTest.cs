// <copyright file="DbInsertionTest.cs" company="Transilvania University of Brasov">
// Margarit Marian Catalin
// </copyright>

namespace LibraryManagementDatabaseTests
{
    using System.Collections.Generic;
    using NUnit.Framework;
    using LibraryManagement.BusinessLayer;
    using LibraryManagement.Data_Mapper;
    using LibraryManagement.Domain_Model;

    /// <summary>
    /// The database insertion test.
    /// </summary>
    [TestFixture]
    public class DbInsertionTest
    {
        private LibraryDbContext libraryContext;

        /// <summary>
        /// The test setup.
        /// </summary>
        [SetUp]
        public void SetUp()
        {
            this.libraryContext = new LibraryDbContext();
        }

        /// <summary>
        /// Clean database after tests.
        /// </summary>
        [TearDown]
        public void Cleanup()
        {
            this.libraryContext.Database.Delete();
        }

        /// <summary>
        /// Test insert a new category.
        /// </summary>
        [Test]
        public void TestAddCategoryToDb()
        {
            var category = new Category { Name = "Fiction", };
            var categoriesService = new CategoriesService(new CategoriesRepository(this.libraryContext));
            var result = categoriesService.AddCategory(category);
            Assert.True(result);
        }

        /// <summary>
        /// Test added a new employee.
        /// </summary>
        [Test]
        public void TestAddEmployeeToDb()
        {
            var employee = new Employee
                           {
                               FirstName = "Margarit",
                               LastName = "Marian",
                               Email = "testemail@test.com",
                               Phone = "0734445567",
                               Address = "Str. Egretei nr31",
                               Gender = "M",
                           };
            var employeeService = new EmployeeService(new EmployeeRepository(this.libraryContext));
            var result = employeeService.AddEmployee(employee);
            Assert.True(result);
        }

        /// <summary>
        /// Test add a new reader.
        /// </summary>
        [Test]
        public void TestAddReaderToDb()
        {
            var reader = new Reader
                         {
                             FirstName = "Test",
                             LastName = "User",
                             Email = "testemail@test.com",
                             Phone = "0754356789",
                             Address = "Str.Egretei nr.22",
                             Extensions = new List<Extension>(),
                             Gender = "F",
                         };
            var readerService = new ReaderService(new ReaderRepository(this.libraryContext));
            var result = readerService.AddReader(reader);
            Assert.True(result);
        }

        /// <summary>
        /// Test add a new author.
        /// </summary>
        [Test]
        public void TestAddAuthor()
        {
            var author = new Author { FirstName = "Estera", LastName = "Balas", Gender = "M" };
            var authorService = new AuthorService(new AuthorRepository(this.libraryContext));
            var result = authorService.AddAuthor(author);
            Assert.True(result);
        }
    }
}