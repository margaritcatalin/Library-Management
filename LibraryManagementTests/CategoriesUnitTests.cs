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
    public class CategoriesUnitTests
    {
        private CategoriesService categoriesService;

        private BookService bookService;

        private LibraryDbContext libraryContextMock;

        /// <summary>
        /// Testes setup.
        /// </summary>
        [SetUp]
        public void SetUp()
        {
            this.libraryContextMock = EntityFrameworkMock.Create<LibraryDbContext>();
            this.categoriesService = new CategoriesService(new CategoriesRepository(this.libraryContextMock));
            this.bookService = new BookService(
    new BookRepository(this.libraryContextMock),
    new CategoriesService(new CategoriesRepository(this.libraryContextMock)),
    new ReaderRepository(this.libraryContextMock));
        }

        /// <summary>
        /// Test add category witch is part of subcategory.
        /// </summary>
        [Test]
        public void TestCategoryIsPartOfSubCategory()
        {
            var c1 = new Category { Name = "C1" };
            var c2 = new Category { Name = "C2" };
            var c3 = new Category { Name = "C3" };
            var c4 = new Category { Name = "C4", ParentCategory = c1 };

            var result = this.categoriesService.CategoryIsPartOfCategories(c4, new List<Category> { c2, c3, c1 });

            Assert.True(result);
        }

        /// <summary>
        /// Test add a parent category and it is part of subcategory.
        /// </summary>
        [Test]
        public void TestParentCategoryIsPartOfSubCategory()
        {
            var c1 = new Category { Name = "C1" };
            var c2 = new Category { Name = "C2", ParentCategory = c1 };
            var c3 = new Category { Name = "C3", ParentCategory = c1 };
            var c4 = new Category { Name = "C4", ParentCategory = c1 };

            var result = this.categoriesService.CategoryIsPartOfCategories(c1, new List<Category> { c2, c3, c4 });

            Assert.True(result);
        }

        /// <summary>
        /// Test add category and it is not part of subcategory.
        /// </summary>
        [Test]
        public void TestCategoryIsNotPartOfSubCategory()
        {
            var c1 = new Category { Name = "C1" };
            var c2 = new Category { Name = "C2" };
            var c3 = new Category { Name = "C3" };

            var result = this.categoriesService.CategoryIsPartOfCategories(c1, new List<Category> { c2, c3 });

            Assert.False(result);
        }

        /// <summary>
        /// Test add a null category.
        /// </summary>
        [Test]
        public void AddNullCategory()
        {
            var result = this.categoriesService.AddCategory(null);
            Assert.False(result);
            Assert.True(this.libraryContextMock.Categories.Count() == 0);
        }

        /// <summary>
        /// Test add a category with null name.
        /// </summary>
        [Test]
        public void AddCategoryWithNullName()
        {
            var c = new Category { Name = null };
            var result = this.categoriesService.AddCategory(c);
            Assert.False(result);
            Assert.True(this.libraryContextMock.Categories.Count() == 0);
        }

        /// <summary>
        /// Test add a category with empty name.
        /// </summary>
        [Test]
        public void AddCategoryWithEmptyName()
        {
            var c = new Category { Name = string.Empty };
            var result = this.categoriesService.AddCategory(c);
            Assert.False(result);
            Assert.True(this.libraryContextMock.Categories.Count() == 0);
        }

        /// <summary>
        /// Test add a category with small name.
        /// </summary>
        [Test]
        public void AddCategoryNameLengthLessThanThree()
        {
            var c = new Category { Name = "Mi" };
            var result = this.categoriesService.AddCategory(c);
            Assert.False(result);
            Assert.True(this.libraryContextMock.Categories.Count() == 0);
        }

        /// <summary>
        /// Test add a category with bigger name.
        /// </summary>
        [Test]
        public void AddCategoryNameLengthMoreThanLimitEighty()
        {
            var c = new Category
            {
                Name = "LongLongLongLongLongLongLongLongLongLongLongLongLongLongLongLongLongLongLongLongLongLongLongLongLongLongLongLongLongLong",
            };
            var result = this.categoriesService.AddCategory(c);
            Assert.False(result);
            Assert.True(this.libraryContextMock.Categories.Count() == 0);
        }

        /// <summary>
        /// Test add category with digit in name.
        /// </summary>
        [Test]
        public void AddCategoryNameWithDigit()
        {
            var c = new Category { Name = "2425" };
            var result = this.categoriesService.AddCategory(c);
            Assert.False(result);
            Assert.True(this.libraryContextMock.Categories.Count() == 0);
        }

        /// <summary>
        /// Test add category with symbol in name.
        /// </summary>
        [Test]
        public void AddCategoryNameWithSymbol()
        {
            var c = new Category { Name = "@&abcd" };
            var result = this.categoriesService.AddCategory(c);
            Assert.False(result);
            Assert.True(this.libraryContextMock.Categories.Count() == 0);
        }

        /// <summary>
        /// Test add category with space in name.
        /// </summary>
        [Test]
        public void AddCategoryNameWithWhiteSpace()
        {
            var c = new Category { Name = "Action and Drama" };
            var result = this.categoriesService.AddCategory(c);
            Assert.False(this.libraryContextMock.Categories.Count() == 0);
        }

        /// <summary>
        /// Test add a category.
        /// </summary>
        [Test]
        public void AddCategory()
        {
            var c = new Category { Name = "Fiction" };
            var result = this.categoriesService.AddCategory(c);
            Assert.True(this.libraryContextMock.Categories.Count() == 1);
        }

        /// <summary>
        /// Test get a category.
        /// </summary>
        [Test]
        public void GetCategory()
        {
            var c = new Category { Name = "Fiction" };
            var result = this.categoriesService.AddCategory(c);

            c = this.categoriesService.GetCategory(c.Name);
            Assert.NotNull(c);
        }

        /// <summary>
        /// Test get a category by null name.
        /// </summary>
        [Test]
        public void GetNullCategory()
        {
            var c = this.categoriesService.GetCategory(null);
            Assert.Null(c);
        }

        /// <summary>
        /// Test get a category by empty name.
        /// </summary>
        [Test]
        public void GetEmptyCategory()
        {
            var c = this.categoriesService.GetCategory(string.Empty);
            Assert.Null(c);
        }

        /// <summary>
        /// Test get an unknown category.
        /// </summary>
        [Test]
        public void GetUnknownCategory()
        {
            var c = new Category { Name = "Fiction" };
            var result = this.categoriesService.AddCategory(c);

            c = this.categoriesService.GetCategory("Philosophy");
            Assert.Null(c);
        }

        /// <summary>
        /// Test add a subcategory.
        /// </summary>
        [Test]
        public void AddSubCateory()
        {
            Category c = new Category { Name = "Science" };
            Category c2 = new Category { Name = "Fiction", ParentCategory = c };
            var result = this.categoriesService.AddCategory(c);
            var result2 = this.categoriesService.AddCategory(c2);
            Assert.True(c2.ParentCategory != null);
            Assert.True(this.libraryContextMock.Categories.Count() == 2);
        }

        /// <summary>
        /// Test add a subcategory with no parent category.
        /// </summary>
        [Test]
        public void AddSubCateoryWithNoParentCategory()
        {
            Category c = new Category { Name = "Fiction" };
            Category c2 = new Category { Name = "Science" };
            var result = this.categoriesService.AddCategory(c);
            var result2 = this.categoriesService.AddCategory(c2);
            Assert.False(c2.ParentCategory != null);
            Assert.True(this.libraryContextMock.Categories.Count() == 2);
        }

        /// <summary>
        /// Test is part of category.
        /// </summary>
        [Test]
        public void IsPartOfCategory()
        {
            Category c = new Category { Name = "Fiction" };
            var result = this.categoriesService.AddCategory(c);

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
                Categories = new List<Category> { c },
            };
            var resultB = this.bookService.CreateBook(book);
            Assert.True(this.categoriesService.IsPartOfCategory(book, c));
            Assert.True(this.libraryContextMock.Categories.Count() == 1 && this.libraryContextMock.Books.Count() == 1);
        }

        /// <summary>
        /// Test if is not part of category.
        /// </summary>
        [Test]
        public void IsNotPartOfCategory()
        {
            Category c = new Category { Name = "Fiction" };
            var result = this.categoriesService.AddCategory(c);
            Category c2 = new Category { Name = "Test" };
            var result2 = this.categoriesService.AddCategory(c2);
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
                Categories = new List<Category> { c },
            };
            var resultB = this.bookService.CreateBook(book);
            Assert.False(this.categoriesService.IsPartOfCategory(book, c2));
            Assert.True(this.libraryContextMock.Categories.Count() == 2 && this.libraryContextMock.Books.Count() == 1);
        }

        /// <summary>
        /// Test check if is not part of parent category.
        /// </summary>
        [Test]
        public void IsNotPartOfParentCategory()
        {
            Category c = new Category { Name = "Fiction" };
            var result = this.categoriesService.AddCategory(c);
            Category c1 = new Category { Name = "Science2" };
            var result1 = this.categoriesService.AddCategory(c1);
            Category c2 = new Category { Name = "Test", ParentCategory = c };
            var result2 = this.categoriesService.AddCategory(c2);
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
                Categories = new List<Category> { c1 },
            };
            var resultB = this.bookService.CreateBook(book);
            Assert.False(this.categoriesService.IsPartOfCategory(book, c));
            Assert.True(this.libraryContextMock.Categories.Count() == 2 && this.libraryContextMock.Books.Count() == 1);
        }

        /// <summary>
        /// Test is part of parent category.
        /// </summary>
        [Test]
        public void IsPartOfParentCategory()
        {
            Category c = new Category { Name = "Fiction" };
            var result = this.categoriesService.AddCategory(c);
            Category c2 = new Category { Name = "Test", ParentCategory = c };
            var result2 = this.categoriesService.AddCategory(c2);
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
                Categories = new List<Category> { c2 },
            };
            var resultB = this.bookService.CreateBook(book);
            Assert.True(this.categoriesService.IsPartOfCategory(book, c));
            Assert.True(this.libraryContextMock.Categories.Count() == 2 && this.libraryContextMock.Books.Count() == 1);
        }

        /// <summary>
        /// Test is part of null categories.
        /// </summary>
        [Test]
        public void IsPartOfCategoryNullCategory()
        {
            Category c = new Category { Name = "Fiction" };
            var result = this.categoriesService.AddCategory(c);

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
                Categories = new List<Category> { c },
            };
            var resultB = this.bookService.CreateBook(book);
            Assert.False(this.categoriesService.IsPartOfCategory(book, null));
            Assert.True(this.libraryContextMock.Categories.Count() == 1 && this.libraryContextMock.Books.Count() == 1);
        }

        /// <summary>
        /// Test category is part of categories.
        /// </summary>
        [Test]
        public void CategoryIsPartOfCategoriesNotNullCategory()
        {
            Category c = new Category { Name = "Fiction" };
            Category c2 = new Category { Name = "Test", ParentCategory = c };
            var result = this.categoriesService.AddCategory(c);
            var result2 = this.categoriesService.AddCategory(c2);
            List<Category> categories = new List<Category>
            {
                c, c2
            };
            Assert.True(this.categoriesService.CategoryIsPartOfCategories(c, categories));
            Assert.True(this.libraryContextMock.Categories.Count() == 2);
        }

        /// <summary>
        /// Test categories is part off null categories.
        /// </summary>
        [Test]
        public void CategoryIsPartOfCategoriesNullCategory()
        {
            Category c = new Category { Name = "Fiction" };
            Category c2 = new Category { Name = "Test", ParentCategory = c };
            var result = this.categoriesService.AddCategory(c);
            var result2 = this.categoriesService.AddCategory(c2);
            List<Category> categories = new List<Category>
            {
                c, c2
            };
            Assert.False(this.categoriesService.CategoryIsPartOfCategories(null, categories));
            Assert.True(this.libraryContextMock.Categories.Count() == 2);
        }

        /// <summary>
        /// Testcategory is not part of categories.
        /// </summary>
        [Test]
        public void CategoryIsNotPartOfCategories()
        {
            Category c = new Category { Name = "Fiction" };
            Category c2 = new Category { Name = "Test", ParentCategory = c };
            Category c1 = new Category { Name = "Glob" };
            var result = this.categoriesService.AddCategory(c);
            var result1 = this.categoriesService.AddCategory(c1);
            var result2 = this.categoriesService.AddCategory(c2);
            List<Category> categories = new List<Category>
            {
                c, c2
            };
            Assert.False(this.categoriesService.CategoryIsPartOfCategories(c1, categories));
            Assert.True(this.libraryContextMock.Categories.Count() == 3);
        }
    }
}