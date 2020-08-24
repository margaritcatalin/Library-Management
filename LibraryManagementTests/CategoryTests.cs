// <copyright file="CategoryTests.cs" company="Transilvania University of Brasov">
// Margarit Marian Catalin
// </copyright>

namespace LibraryManagementTests
{
    using System.Linq;
    using LibraryManagement.BusinessLayer;
    using LibraryManagement.DataMapper;
    using LibraryManagement.DomainModel;
    using NUnit.Framework;
    using Telerik.JustMock.EntityFramework;

    /// <summary>
    /// The category unit tests.
    /// </summary>
    [TestFixture]
    public class CategoryTests
    {
        /// <summary>
        /// Defines the libraryContextMock.
        /// </summary>
        private LibraryDbContext libraryContextMock;

        /// <summary>
        /// Defines the categoryService.
        /// </summary>
        private CategoryService categoryService;

        /// <summary>
        /// Defines the productService.
        /// </summary>
        private ProductService productService;

        /// <summary>
        /// Tests setup.
        /// </summary>
        [SetUp]
        public void SetUp()
        {
            this.libraryContextMock = EntityFrameworkMock.Create<LibraryDbContext>();
            EntityFrameworkMock.PrepareMock(this.libraryContextMock);
            this.categoryService = new CategoryService(new CategoryRepository(this.libraryContextMock));
            this.productService = new ProductService(new ProductRepository(this.libraryContextMock));
        }

        /// <summary>
        /// Test add a new root category.
        /// </summary>
        [Test]
        public void TestAddRootCategory()
        {
            var category = new Category { Name = "Legume" };
            var resultCategory = this.categoryService.AddCategory(category);
            Assert.True(this.libraryContextMock.Categories.Count() == 1);
        }

        /// <summary>
        /// Test add a new category with subcategory.
        /// </summary>
        [Test]
        public void TestAddCategoryWithParent()
        {
            var category = new Category { Name = "Legume" };
            var resultCategory = this.categoryService.AddCategory(category);
            var subCategory = new Category { Name = "Radacinoase", ParentCategory_Id = category.Id };
            var resultBubCategory = this.categoryService.AddCategory(subCategory);
            Assert.True(this.libraryContextMock.Categories.Count() == 1);
        }

        /// <summary>
        /// Test add a null category.
        /// </summary>
        [Test]
        public void TestAddNullCategory()
        {
            var result = this.categoryService.AddCategory(null);
            Assert.True(!this.libraryContextMock.Categories.Any());
        }

        /// <summary>
        /// Test add an category with null name.
        /// </summary>
        [Test]
        public void TestAddNullFirstNameCategory()
        {
            var category = new Category { Name = null };
            var resultCategory = this.categoryService.AddCategory(category);
            Assert.True(!this.libraryContextMock.Categories.Any());
        }

        /// <summary>
        /// Test add add category with empty name.
        /// </summary>
        [Test]
        public void TestAddEmptyFirstNameCategory()
        {
            var category = new Category { Name = string.Empty };
            var resultCategory = this.categoryService.AddCategory(category);
            Assert.True(!this.libraryContextMock.Categories.Any());
        }

        /// <summary>
        /// Test add category with smaller name.
        /// </summary>
        [Test]
        public void TestAddSmallerFirstNameCategory()
        {
            var category = new Category { Name = "Aa" };
            var resultCategory = this.categoryService.AddCategory(category);
            Assert.True(!this.libraryContextMock.Categories.Any());
        }

        /// <summary>
        /// Test add category with longer name.
        /// </summary>
        [Test]
        public void TestAddLongerFirstNameCategory()
        {
            var category = new Category
            {
                Name =
                    "LongLongLongLongLongLongLongLongLongLongLongLongLongLongLongLongLongLongLongLongLongLongLongLongLongLongLongLongLongLong"
            };
            var resultCategory = this.categoryService.AddCategory(category);
            Assert.True(!this.libraryContextMock.Categories.Any());
        }

        /// <summary>
        /// Test add category with lower name.
        /// </summary>
        [Test]
        public void TestAddLowerFirstNameCategory()
        {
            var category = new Category { Name = "legume" };
            var resultCategory = this.categoryService.AddCategory(category);
            Assert.True(!this.libraryContextMock.Categories.Any());
        }

        /// <summary>
        /// Test get category.
        /// </summary>
        [Test]
        public void TestGetCategoryByName()
        {
            var category = new Category { Name = "Legume" };
            var result = this.categoryService.AddCategory(category);
            category = this.categoryService.GetCategoryByName(category.Name);
            Assert.NotNull(category);
        }

        /// <summary>
        /// Test get category with null name.
        /// </summary>
        [Test]
        public void TestGetNullNameCategory()
        {
            var category = new Category { Name = "Legume" };
            var result = this.categoryService.AddCategory(category);
            category = this.categoryService.GetCategoryByName(null);
            Assert.Null(category);
        }

        /// <summary>
        /// Test get Category with empty name.
        /// </summary>
        [Test]
        public void TestGetEmptyNameCategory()
        {
            var category = new Category { Name = "Legume" };
            var result = this.categoryService.AddCategory(category);
            category =
                this.categoryService.GetCategoryByName(string.Empty);
            Assert.Null(category);
        }

        /// <summary>
        /// Test get category and he is not in database.
        /// </summary>
        [Test]
        public void TestGetBadNameCategory()
        {
            var category = new Category { Name = "Legume" };
            var result = this.categoryService.AddCategory(category);
            category = this.categoryService.GetCategoryByName("Aparate");
            Assert.Null(category);
        }

        /// <summary>
        /// Test get category by id and he is in the database.
        /// </summary>
        [Test]
        public void TestGetCategoryByGoodId()
        {
            var category = new Category { Name = "Legume" };
            var resultCategory = this.categoryService.AddCategory(category);
            var categories = this.categoryService.GetCategories();
            var currentCategory = categories.ToList()[0];
            var categoryById = this.categoryService.GetCategoryById(currentCategory.Id);
            Assert.NotNull(categoryById);
        }

        /// <summary>
        /// Test get category by id and it is not in the database.
        /// </summary>
        [Test]
        public void TestGetCategoryByBadId()
        {
            var category = new Category { Name = "Legume" };
            var resultCategory = this.categoryService.AddCategory(category);
            var categories = this.categoryService.GetCategories();
            var currentCategory = categories.ToList()[0];
            var categoryById = this.categoryService.GetCategoryById(currentCategory.Id + 1);
            Assert.Null(categoryById);
        }

        /// <summary>
        /// Test get all categories.
        /// </summary>
        [Test]
        public void TestGetAllCategories()
        {
            var category = new Category { Name = "Legume" };
            var resultCategory = this.categoryService.AddCategory(category);
            var category2 = new Category { Name = "Fasole" };
            var result2 = this.categoryService.AddCategory(category2);

            var categories = this.categoryService.GetCategories();
            Assert.IsTrue(categories.Count() == 2);
        }

        /// <summary>
        /// Test get all categories with a wrong category.
        /// </summary>
        [Test]
        public void TestGetAllCategoriesWithAWrongCategory()
        {
            var category = new Category { Name = "Legume" };
            var resultCategory = this.categoryService.AddCategory(category);
            var category2 = new Category { Name = "aa" };
            var result2 = this.categoryService.AddCategory(category2);
            var categories = this.categoryService.GetCategories();
            Assert.IsFalse(categories.Count() == 2);
        }

        /// <summary>
        /// Test delete Category.
        /// </summary>
        [Test]
        public void TestDeleteCategory()
        {
            var category = new Category { Name = "Legume" };
            var resultCategory = this.categoryService.AddCategory(category);
            var categoryByName = this.categoryService.GetCategoryByName(category.Name);
            var deleteResult = this.categoryService.DeleteCategory(categoryByName.Id);
            Assert.True(!this.libraryContextMock.Categories.Any());
        }

        /// <summary>
        /// Test delete Category.
        /// </summary>
        [Test]
        public void TestDeleteCategoryWithAWrongId()
        {
            var category = new Category { Name = "Legume" };
            var resultCategory = this.categoryService.AddCategory(category);
            var categoryByName = this.categoryService.GetCategoryByName(category.Name);
            var deleteResult = this.categoryService.DeleteCategory(categoryByName.Id + 1);
            Assert.True(this.libraryContextMock.Categories.Count() == 1);
        }

        /// <summary>
        /// Test Get all categories for product.
        /// </summary>
        [Test]
        public void TestGetAllCategoriesForProduct()
        {
            var category = new Category { Name = "Legume" };
            var resultCategory = this.categoryService.AddCategory(category);
            var product = new Product { Name = "Varza", Category = new[] { category } };
            var result = this.productService.AddProduct(product);
            var products = this.productService.GetProducts();
            var productId = products.ToList()[0].Id;
            var productById = this.productService.GetProductById(productId);
            var productCategories = this.categoryService.GetAllCategoriesProduct(productById);
            Assert.True(productCategories.Count() == 1);
        }

        /// <summary>
        /// Test Get all categories for product with roots.
        /// </summary>
        [Test]
        public void TestGetAllCategoriesForProductWithMultipleRootCategories()
        {
            var category = new Category { Name = "Legume" };
            var resultCategory = this.categoryService.AddCategory(category);
            var category2 = new Category { Name = "Legume" };
            var resultCategory2 = this.categoryService.AddCategory(category2);
            var product = new Product { Name = "Varza", Category = new[] { category, category2 } };
            var result = this.productService.AddProduct(product);
            var products = this.productService.GetProducts();
            var productId = products.ToList()[0].Id;
            var productById = this.productService.GetProductById(productId);
            var productCategories = this.categoryService.GetAllCategoriesProduct(productById);
            Assert.True(productCategories.Count() == 2);
        }

        /// <summary>
        /// Test Get all categories for product with sub category.
        /// </summary>
        [Test]
        public void TestGetAllCategoriesForProductWithASimpleSubCategory()
        {
            var category = new Category { Name = "Legume" };
            var resultCategory = this.categoryService.AddCategory(category);
            var category2 = new Category { Name = "Legume", ParentCategory = category };
            var resultCategory2 = this.categoryService.AddCategory(category2);
            var product = new Product { Name = "Varza", Category = new[] { category2 } };
            var result = this.productService.AddProduct(product);
            var products = this.productService.GetProducts();
            var productId = products.ToList()[0].Id;
            var productById = this.productService.GetProductById(productId);
            var productCategories = this.categoryService.GetAllCategoriesProduct(productById);
            Assert.True(productCategories.Count() == 2);
        }

        /// <summary>
        /// Test Get all categories for product with sub sub  category.
        /// </summary>
        [Test]
        public void TestGetAllCategoriesForProductWithASimpleSubSubCategory()
        {
            var category = new Category { Name = "Legume" };
            var resultCategory = this.categoryService.AddCategory(category);
            var category2 = new Category { Name = "Legume", ParentCategory = category };
            var resultCategory2 = this.categoryService.AddCategory(category2);
            var category3 = new Category { Name = "Legume", ParentCategory = category2 };
            var resultCategory3 = this.categoryService.AddCategory(category3);
            var product = new Product { Name = "Varza", Category = new[] { category3 } };
            var result = this.productService.AddProduct(product);
            var products = this.productService.GetProducts();
            var productId = products.ToList()[0].Id;
            var productById = this.productService.GetProductById(productId);
            var productCategories = this.categoryService.GetAllCategoriesProduct(productById);
            Assert.True(productCategories.Count() == 3);
        }
    }
}