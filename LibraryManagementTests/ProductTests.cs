// <copyright file="ProductTests.cs" company="Transilvania University of Brasov">
// Margarit Marian Catalin
// </copyright>

namespace LibraryManagementTests
{
    using System.Collections.Generic;
    using System.Linq;
    using LibraryManagement.BusinessLayer;
    using LibraryManagement.DataMapper;
    using LibraryManagement.DomainModel;
    using NUnit.Framework;
    using Telerik.JustMock.EntityFramework;

    /// <summary>
    /// The product unit tests.
    /// </summary>
    [TestFixture]
    public class ProductTests
    {
        /// <summary>
        /// Defines the libraryContextMock.
        /// </summary>
        private LibraryDbContext libraryContextMock;

        /// <summary>
        /// Defines the productService.
        /// </summary>
        private ProductService productService;

        /// <summary>
        /// Defines the categoryService.
        /// </summary>
        private CategoryService categoryService;

        /// <summary>
        /// Tests setup.
        /// </summary>
        [SetUp]
        public void SetUp()
        {
            this.libraryContextMock = EntityFrameworkMock.Create<LibraryDbContext>();
            EntityFrameworkMock.PrepareMock(this.libraryContextMock);
            this.productService = new ProductService(new ProductRepository(this.libraryContextMock));
            this.categoryService = new CategoryService(new CategoryRepository(this.libraryContextMock));
        }

        /// <summary>
        /// Test add a new product.
        /// </summary>
        [Test]
        public void TestAddProduct()
        {
            var category = new Category { Name = "Legume" };
            var resultCategory = this.categoryService.AddCategory(category);
            var product = new Product { Name = "Varza", Category = new[] { category } };
            var result = this.productService.AddProduct(product);
            Assert.True(this.libraryContextMock.Products.Count() == 1);
        }

        /// <summary>
        /// Test add a null product.
        /// </summary>
        [Test]
        public void TestAddNullProduct()
        {
            var result = this.productService.AddProduct(null);
            Assert.True(!this.libraryContextMock.Products.Any());
        }

        /// <summary>
        /// Test add a new product with null categories.
        /// </summary>
        [Test]
        public void TestAddNullCategoriesProduct()
        {
            var product = new Product { Name = "Varza", Category = null };
            var result = this.productService.AddProduct(product);
            Assert.True(!this.libraryContextMock.Products.Any());
        }

        /// <summary>
        /// Test add a new product with null categories.
        /// </summary>
        [Test]
        public void TestAddEmptyCategoriesProduct()
        {
            ICollection<Category> list = new List<Category>();

            var product = new Product { Name = "Varza", Category = list };
            var result = this.productService.AddProduct(product);
            Assert.True(!this.libraryContextMock.Products.Any());
        }

        /// <summary>
        /// Test add an product with null name.
        /// </summary>
        [Test]
        public void TestAddNullNameProduct()
        {
            var category = new Category { Name = "Legume" };
            var resultCategory = this.categoryService.AddCategory(category);
            var product = new Product { Name = null, Category = new[] { category } };
            var result = this.productService.AddProduct(product);
            Assert.True(!this.libraryContextMock.Products.Any());
        }

        /// <summary>
        /// Test add add product with empty name.
        /// </summary>
        [Test]
        public void TestAddEmptyNameProduct()
        {
            var category = new Category { Name = "Legume" };
            var resultCategory = this.categoryService.AddCategory(category);
            var product = new Product { Name = string.Empty, Category = new[] { category } };
            var result = this.productService.AddProduct(product);
            Assert.True(!this.libraryContextMock.Products.Any());
        }

        /// <summary>
        /// Test add product with smaller name.
        /// </summary>
        [Test]
        public void TestAddSmallerNameProduct()
        {
            var category = new Category { Name = "Legume" };
            var resultCategory = this.categoryService.AddCategory(category);
            var product = new Product { Name = "Aa", Category = new[] { category } };
            var result = this.productService.AddProduct(product);
            Assert.True(!this.libraryContextMock.Products.Any());
        }

        /// <summary>
        /// Test add product with longer name.
        /// </summary>
        [Test]
        public void TestAddLongerNameProduct()
        {
            var category = new Category { Name = "Legume" };
            var resultCategory = this.categoryService.AddCategory(category);
            var product = new Product
            {
                Name =
                    "LongLongLongLongLongLongLongLongLongLongLongLongLongLongLongLongLongLongLongLongLongLongLongLongLongLongLongLongLongLong",
                Category = new[] { category },
            };
            var result = this.productService.AddProduct(product);
            Assert.True(!this.libraryContextMock.Products.Any());
        }

        /// <summary>
        /// Test add product with lower name.
        /// </summary>
        [Test]
        public void TestAddLowerNameProduct()
        {
            var category = new Category { Name = "Legume" };
            var resultCategory = this.categoryService.AddCategory(category);
            var product = new Product { Name = "legume", Category = new[] { category } };
            var result = this.productService.AddProduct(product);
            Assert.True(!this.libraryContextMock.Products.Any());
        }

        /// <summary>
        /// Test get product by id and he is in the database.
        /// </summary>
        [Test]
        public void TestGetProductByGoodId()
        {
            var category = new Category { Name = "Legume" };
            var resultCategory = this.categoryService.AddCategory(category);
            var product = new Product { Name = "Varza", Category = new[] { category } };
            var result = this.productService.AddProduct(product);
            var productById = this.productService.GetProductById(product.Id);
            Assert.NotNull(productById);
        }

        /// <summary>
        /// Test get product by id and it is not in the database.
        /// </summary>
        [Test]
        public void TestGetProductByBadId()
        {
            var category = new Category { Name = "Legume" };
            var resultCategory = this.categoryService.AddCategory(category);
            var product = new Product { Name = "Varza", Category = new[] { category } };
            var result = this.productService.AddProduct(product);
            var productById = this.productService.GetProductById(product.Id + 1);
            Assert.Null(productById);
        }

        /// <summary>
        /// Test get all products.
        /// </summary>
        [Test]
        public void TestGetAllProducts()
        {
            var category = new Category { Name = "Legume" };
            var resultCategory = this.categoryService.AddCategory(category);
            var product = new Product { Name = "Varza", Category = new[] { category } };
            var product2 = new Product { Name = "Fasole", Category = new[] { category } };
            var result = this.productService.AddProduct(product);
            var result2 = this.productService.AddProduct(product2);

            var products = this.productService.GetProducts();
            Assert.IsTrue(products.Count() == 2);
        }

        /// <summary>
        /// Test get all products with a wrong product.
        /// </summary>
        [Test]
        public void TestGetAllProductsWithAWrongProduct()
        {
            var category = new Category { Name = "Legume" };
            var resultCategory = this.categoryService.AddCategory(category);
            var product = new Product { Name = "Varza", Category = new[] { category } };
            var product2 = new Product { Name = "aa", Category = new[] { category } };
            var result = this.productService.AddProduct(product);
            var result2 = this.productService.AddProduct(product2);

            var products = this.productService.GetProducts();
            Assert.IsFalse(products.Count() == 2);
        }

        /// <summary>
        /// Test delete Product.
        /// </summary>
        [Test]
        public void TestDeleteProduct()
        {
            var category = new Category { Name = "Legume" };
            var resultCategory = this.categoryService.AddCategory(category);
            var product = new Product { Name = "Varza", Category = new[] { category } };
            var result = this.productService.AddProduct(product);
            var products = this.productService.GetProducts();
            var currentProduct = products.ToList()[0];
            var deleteResult = this.productService.DeleteProduct(currentProduct.Id);
            Assert.True(!this.libraryContextMock.Products.Any());
        }

        /// <summary>
        /// Test delete Product.
        /// </summary>
        [Test]
        public void TestDeleteProductWithAWrongId()
        {
            var category = new Category { Name = "Legume" };
            var resultCategory = this.categoryService.AddCategory(category);
            var product = new Product { Name = "Varza", Category = new[] { category } };
            var result = this.productService.AddProduct(product);
            var products = this.productService.GetProducts();
            var currentProduct = products.ToList()[0];
            var deleteResult = this.productService.DeleteProduct(currentProduct.Id + 1);
            Assert.True(this.libraryContextMock.Products.Count() == 1);
        }
    }
}