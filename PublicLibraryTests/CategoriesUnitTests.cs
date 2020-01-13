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
        private LibraryDb _libraryDbMock;

        [SetUp]
        public void SetUp()
        {
            _libraryDbMock = EntityFrameworkMock.Create<LibraryDb>();

            _categoriesService = new CategoriesService(new CategoriesRepository(_libraryDbMock));
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
            Category c = new Category { Name = "DraDraDraDraDraDraDraDraDraDraDraDraDraDraDraDr" +
                                               "DraDraDraDraDraDraDraDra" +
                                               "DraDraDraDraDraDra" +
                                               "DraDraDraDraDra" +
                                               "DraDraDraDraDraDraDraaDraDr" +
                                               "DraDraDraDraDraDraDraDraDra" +
                                               "aDraDraDraDraDraDraDraDraDraDraDra"
            };
            var result = _categoriesService.AddCategory(c);
            Assert.False(result);
            Assert.True(_libraryDbMock.Categories.Count() == 0);
        }
        [Test]
        public void AddCategoryNameWithDigit()
        {
            Category c = new Category { Name = "2425" };
            var result = _categoriesService.AddCategory(c);
            Assert.False(result);
            Assert.True(_libraryDbMock.Categories.Count() == 0);
        }

        [Test]
        public void AddCategoryNameWithSymbol()
        {
            Category c = new Category { Name = "@&kfgdg" };
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
    }
}
