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
    public class CategoriesUnitTests
    {
        private CategoriesService _categoriesService;
        private BookService _bookService;
        private LibraryDb _libraryDbMock;

        [SetUp]
        public void SetUp()
        {
            _libraryDbMock = EntityFrameworkMock.Create<LibraryDb>();

            _categoriesService = new CategoriesService(new CategoriesRepository(_libraryDbMock));
            _bookService = new BookService(new BookRepository(_libraryDbMock), _categoriesService,
                new ReaderRepository(_libraryDbMock));
        }

        [Test]
        public void TestCategoryIsPartOfSubCategory()
        {
            Category c1 = new Category {Name = "C1" };
            Category c2 = new Category { Name = "C2" };
            Category c3 = new Category { Name = "C3" };
            Category c4 = new Category { Name = "C4", ParentCategory = c1};

            var result =_categoriesService.CategoryIsPartOfCategories(c4, new List<Category>{c2,c3,c1});

            Assert.True(result);
        }
        [Test]
        public void TestParentCategoryIsPartOfSubCategory()
        {
            Category c1 = new Category { Name = "C1" };
            Category c2 = new Category { Name = "C2", ParentCategory = c1 };
            Category c3 = new Category { Name = "C3", ParentCategory = c1 };
            Category c4 = new Category { Name = "C4", ParentCategory = c1 };

            var result = _categoriesService.CategoryIsPartOfCategories(c1, new List<Category> { c2, c3, c4 });

            Assert.True(result);
        }
        [Test]
        public void TestCategoryIsNotPartOfSubCategory()
        {
            Category c1 = new Category { Name = "C1" };
            Category c2 = new Category { Name = "C2" };
            Category c3 = new Category { Name = "C3" };

            var result = _categoriesService.CategoryIsPartOfCategories(c1, new List<Category> { c2, c3 });

            Assert.False(result);
        }

        [Test]
        public void AddNullCategory()
        {
            var result = _categoriesService.AddCategory(null);
            Assert.False(result);
            Assert.True(_libraryDbMock.Categories.Count() == 0);
        }
        [Test]
        public void AddCategoryWithNullName()
        {
            Category c = new Category { Name = null };
            var result = _categoriesService.AddCategory(c);
            Assert.False(result);
            Assert.True(_libraryDbMock.Categories.Count() == 0);
        }
        [Test]
        public void AddCategoryWithEmptyName()
        {
            Category c = new Category { Name = "" };
            var result = _categoriesService.AddCategory(c);
            Assert.False(result);
            Assert.True(_libraryDbMock.Categories.Count() == 0);
        }

        [Test]
        public void AddCategoryNameLengthLessThanThree()
        {
            Category c = new Category { Name = "Da" };
            var result = _categoriesService.AddCategory(c);
            Assert.False(result);
            Assert.True(_libraryDbMock.Categories.Count() == 0);
        }

        [Test]
        public void AddCategoryNameLengthMoreThanLimitEighty()
        {
            Category c = new Category { Name = "LengthLengthLengthLengthLengthLengthLengthLengthLength" +
                                               "LengthLengthLengthLengthLengthLengthLengthLengthLength" +
                                               "LengthLengthLengthLengthLengthLengthLengthLengthLength" +
                                               "LengthLengthLengthLengthLengthLengthLengthLengthLength" +
                                               "LengthLengthLengthLengthLengthLengthLengthLengthLength" +
                                               "LengthLengthLengthLengthLengthLengthLengthLengthLength" +
                                               "LengthLengthLengthLengthLengthLengthLengthLengthLength"
            };
            var result = _categoriesService.AddCategory(c);
            Assert.False(result);
            Assert.True(_libraryDbMock.Categories.Count() == 0);
        }
        [Test]
        public void AddCategoryNameWithDigit()
        {
            Category c = new Category { Name = "2326546" };
            var result = _categoriesService.AddCategory(c);
            Assert.False(result);
            Assert.True(_libraryDbMock.Categories.Count() == 0);
        }

        [Test]
        public void AddCategoryNameWithSymbol()
        {
            Category c = new Category { Name = "@&abcd" };
            var result = _categoriesService.AddCategory(c);
            Assert.False(result);
            Assert.True(_libraryDbMock.Categories.Count() == 0);
        }

        [Test]
        public void AddCategoryNameWithWhiteSpace()
        {
            Category c = new Category { Name = "Drama and thriller" };
            var result = _categoriesService.AddCategory(c);
            Assert.False(_libraryDbMock.Categories.Count() == 0);
        }
        [Test]
        public void AddCategory()
        {
            Category c = new Category{Name = "Science"};
            var result = _categoriesService.AddCategory(c);
            Assert.True(_libraryDbMock.Categories.Count()==1);
        }
        [Test]
        public void AddSubCateory()
        {
            Category c = new Category { Name = "Science" };
            Category c2 = new Category { Name = "Test", ParentCategory=c };
            var result = _categoriesService.AddCategory(c);
            var result2 = _categoriesService.AddCategory(c2);
            Assert.True(c2.ParentCategory != null);
            Assert.True(_libraryDbMock.Categories.Count() == 2);
        }
        [Test]
        public void AddSubCateoryWithNoParentCategory()
        {
            Category c = new Category { Name = "Science" };
            Category c2 = new Category { Name = "Test"};
            var result = _categoriesService.AddCategory(c);
            var result2 = _categoriesService.AddCategory(c2);
            Assert.False(c2.ParentCategory != null);
            Assert.True(_libraryDbMock.Categories.Count() == 2);
        }

        [Test]
        public void IsPartOfCategory()
        {
            Category c = new Category { Name = "Science" };
            var result = _categoriesService.AddCategory(c);

            Book book = new Book
            {
                Name = "Moara cu Noroc",
                Authors = new List<Author> { new Author { Name = "Mihail Sadoveanu" } },
                Editions = new List<Edition> { new Edition
            {
                Name = "Teora",
                BookType = "Hardcover",
                Pages = 256,
                BookStock = new BookStock
            {
                Amount = 100,
                LectureRoomAmount = 10
            }
            } },
                Categories = new List<Category> { c }
            };
            var resultB = _bookService.CreateBook(book);
            Assert.True(_categoriesService.IsPartOfCategory(book,c));
            Assert.True(_libraryDbMock.Categories.Count() == 1 && _libraryDbMock.Books.Count()==1);
        }

        [Test]
        public void IsNotPartOfCategory()
        {
            Category c = new Category { Name = "Science" };
            var result = _categoriesService.AddCategory(c);
            Category c2 = new Category { Name = "Test" };
            var result2 = _categoriesService.AddCategory(c2);
            Book book = new Book
            {
                Name = "Moara cu Noroc",
                Authors = new List<Author> { new Author { Name = "Mihail Sadoveanu" } },
                Editions = new List<Edition> { new Edition
            {
                Name = "Teora",
                BookType = "Hardcover",
                Pages = 256,
                BookStock = new BookStock
            {
                Amount = 100,
                LectureRoomAmount = 10
            }
            } },
                Categories = new List<Category> { c }
            };
            var resultB = _bookService.CreateBook(book);
            Assert.False(_categoriesService.IsPartOfCategory(book, c2));
            Assert.True(_libraryDbMock.Categories.Count() == 2 && _libraryDbMock.Books.Count() == 1);
        }
        [Test]
        public void IsNotPartOfParentCategory()
        {
            Category c = new Category { Name = "Science" };
            var result = _categoriesService.AddCategory(c);
            Category c1 = new Category { Name = "Science2" };
            var result1 = _categoriesService.AddCategory(c1);
            Category c2 = new Category { Name = "Test", ParentCategory=c };
            var result2 = _categoriesService.AddCategory(c2);
            Book book = new Book
            {
                Name = "Moara cu Noroc",
                Authors = new List<Author> { new Author { Name = "Mihail Sadoveanu" } },
                Editions = new List<Edition> { new Edition
            {
                Name = "Teora",
                BookType = "Hardcover",
                Pages = 256,
                BookStock = new BookStock
            {
                Amount = 100,
                LectureRoomAmount = 10
            }
            } },
                Categories = new List<Category> { c1 }
            };
            var resultB = _bookService.CreateBook(book);
            Assert.False(_categoriesService.IsPartOfCategory(book, c));
            Assert.True(_libraryDbMock.Categories.Count() == 2 && _libraryDbMock.Books.Count() == 1);
        }
        [Test]
        public void IsPartOfParentCategory()
        {
            Category c = new Category { Name = "Science" };
            var result = _categoriesService.AddCategory(c);
            Category c2 = new Category { Name = "Test",ParentCategory=c };
            var result2 = _categoriesService.AddCategory(c2);
            Book book = new Book
            {
                Name = "Moara cu Noroc",
                Authors = new List<Author> { new Author { Name = "Mihail Sadoveanu" } },
                Editions = new List<Edition> { new Edition
            {
                Name = "Teora",
                BookType = "Hardcover",
                Pages = 256,
                BookStock = new BookStock
            {
                Amount = 100,
                LectureRoomAmount = 10
            }
            } },
                Categories = new List<Category> { c2 }
            };
            var resultB = _bookService.CreateBook(book);
            Assert.True(_categoriesService.IsPartOfCategory(book, c));
            Assert.True(_libraryDbMock.Categories.Count() == 2 && _libraryDbMock.Books.Count() == 1);
        }
        [Test]
        public void IsPartOfCategoryNullCategory()
        {
            Category c = new Category { Name = "Science" };
            var result = _categoriesService.AddCategory(c);

            Book book = new Book
            {
                Name = "Moara cu Noroc",
                Authors = new List<Author> { new Author { Name = "Mihail Sadoveanu" } },
                Editions = new List<Edition> { new Edition
            {
                Name = "Teora",
                BookType = "Hardcover",
                Pages = 256,
                BookStock = new BookStock
            {
                Amount = 100,
                LectureRoomAmount = 10
            }
            } },
                Categories = new List<Category> { c }
            };
            var resultB = _bookService.CreateBook(book);
            Assert.False(_categoriesService.IsPartOfCategory(book, null));
            Assert.True(_libraryDbMock.Categories.Count() == 1 && _libraryDbMock.Books.Count() == 1);
        }
        [Test]
        public void CategoryIsPartOfCategoriesNotNullCategory()
        {
            Category c = new Category { Name = "Science" };
            Category c2 = new Category { Name = "Test", ParentCategory = c };
            var result = _categoriesService.AddCategory(c);
            var result2 = _categoriesService.AddCategory(c2);
            List<Category> categories = new List<Category>();
            categories.Add(c);
            categories.Add(c2);
            Assert.True(_categoriesService.CategoryIsPartOfCategories(c, categories));
            Assert.True(_libraryDbMock.Categories.Count() == 2);
        }
        [Test]
        public void CategoryIsPartOfCategoriesNullCategory()
        {
            Category c = new Category { Name = "Science" };
            Category c2 = new Category { Name = "Test", ParentCategory = c };
            var result = _categoriesService.AddCategory(c);
            var result2 = _categoriesService.AddCategory(c2);
            List<Category> categories = new List<Category>();
            categories.Add(c);
            categories.Add(c2);
            Assert.False(_categoriesService.CategoryIsPartOfCategories(null, categories));
            Assert.True(_libraryDbMock.Categories.Count() == 2);
        }
    }
}
