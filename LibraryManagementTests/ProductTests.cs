// <copyright file="ProductTests.cs" company="Transilvania University of Brasov">
// Margarit Marian Catalin
// </copyright>

using System.Collections.Generic;
using System.Configuration;
using LibraryManagement.DomainModel;
using LibraryManagement.Util;

namespace LibraryManagementTests
{
    using System.Linq;
    using NUnit.Framework;
    using LibraryManagement.BusinessLayer;
    using LibraryManagement.DataMapper;
    using Telerik.JustMock.EntityFramework;

    /// <summary>
    /// The product unit tests.
    /// </summary>
    [TestFixture]
    public class ProductTests
    {
        private LibraryDbContext libraryContextMock;

        private ProductService productService;
        private CategoryService categoryService;

        /// <summary>
        /// Tests setup.
        /// </summary>
        [SetUp]
        public void SetUp()
        {
            this.libraryContextMock = EntityFrameworkMock.Create<LibraryDbContext>();
            this.productService = new ProductService(new ProductRepository(this.libraryContextMock));
            this.categoryService = new CategoryService(new CategoryRepository(this.libraryContextMock));
        }

        /// <summary>
        /// Test add a new product.
        /// </summary>
        [Test]
        public void TestAddProduct()
        {
            var category = new Category {Name = "Legume"};
            var resultCategory = this.categoryService.AddCategory(category);
            var product = new Product {Name = "Varza", Categories = new[] {category}};
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
            var product = new Product {Name = "Varza", Categories = null};
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

            var product = new Product {Name = "Varza", Categories = list};
            var result = this.productService.AddProduct(product);
            Assert.True(!this.libraryContextMock.Products.Any());
        }

        /// <summary>
        /// Test add an product with null name.
        /// </summary>
        [Test]
        public void TestAddNullNameProduct()
        {
            var category = new Category {Name = "Legume"};
            var resultCategory = this.categoryService.AddCategory(category);
            var product = new Product {Name = null, Categories = new[] {category}};
            var result = this.productService.AddProduct(product);
            Assert.True(!this.libraryContextMock.Products.Any());
        }

        /// <summary>
        /// Test add add product with empty name.
        /// </summary>
        [Test]
        public void TestAddEmptyNameProduct()
        {
            var category = new Category {Name = "Legume"};
            var resultCategory = this.categoryService.AddCategory(category);
            var product = new Product {Name = string.Empty, Categories = new[] {category}};
            var result = this.productService.AddProduct(product);
            Assert.True(!this.libraryContextMock.Products.Any());
        }

        /// <summary>
        /// Test add product with smaller name.
        /// </summary>
        [Test]
        public void TestAddSmallerNameProduct()
        {
            var category = new Category {Name = "Legume"};
            var resultCategory = this.categoryService.AddCategory(category);
            var product = new Product {Name = "Aa", Categories = new[] {category}};
            var result = this.productService.AddProduct(product);
            Assert.True(!this.libraryContextMock.Products.Any());
        }

        /// <summary>
        /// Test add product with longer name.
        /// </summary>
        [Test]
        public void TestAddLongerNameProduct()
        {
            var category = new Category {Name = "Legume"};
            var resultCategory = this.categoryService.AddCategory(category);
            var product = new Product
            {
                Name =
                    "LongLongLongLongLongLongLongLongLongLongLongLongLongLongLongLongLongLongLongLongLongLongLongLongLongLongLongLongLongLong",
                Categories = new[] {category},
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
            var category = new Category {Name = "Legume"};
            var resultCategory = this.categoryService.AddCategory(category);
            var product = new Product {Name = "legume", Categories = new[] {category}};
            var result = this.productService.AddProduct(product);
            Assert.True(!this.libraryContextMock.Products.Any());
        }

        /// <summary>
        /// Test get product by id and he is in the db.
        /// </summary>
        [Test]
        public void TestGetProductByGoodId()
        {
            var category = new Category {Name = "Legume"};
            var resultCategory = this.categoryService.AddCategory(category);
            var product = new Product {Name = "Varza", Categories = new[] {category}};
            var result = this.productService.AddProduct(product);
            var products = this.productService.GetProducts();
            var currentProduct = products.ToList()[0];
            var productById = this.productService.GetProductById(currentProduct.Id);
            Assert.NotNull(productById);
        }

        /// <summary>
        /// Test get product by id and it is not in the db.
        /// </summary>
        [Test]
        public void TestGetProductByBadId()
        {
            var category = new Category {Name = "Legume"};
            var resultCategory = this.categoryService.AddCategory(category);
            var product = new Product {Name = "varza", Categories = new[] {category}};
            var result = this.productService.AddProduct(product);
            var products = this.productService.GetProducts();
            var currentProduct = products.ToList()[0];
            var productById = this.productService.GetProductById(currentProduct.Id + 1);
            Assert.Null(productById);
        }

        /// <summary>
        /// Test get all products.
        /// </summary>
        [Test]
        public void TestGetAllProducts()
        {
            var category = new Category {Name = "Legume"};
            var resultCategory = this.categoryService.AddCategory(category);
            var product = new Product {Name = "Varza", Categories = new[] {category}};
            var product2 = new Product {Name = "Fasole", Categories = new[] {category}};
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
            var category = new Category {Name = "Legume"};
            var resultCategory = this.categoryService.AddCategory(category);
            var product = new Product {Name = "Varza", Categories = new[] {category}};
            var product2 = new Product {Name = "aa", Categories = new[] {category}};
            var result = this.productService.AddProduct(product);
            var result2 = this.productService.AddProduct(product2);

            var products = this.productService.GetProducts();
            Assert.IsFalse(products.Count() == 2);
        }

        /// <summary>
        /// Test update name for auction user.
        /// </summary>
        [Test]
        public void TestUpdateNameForProduct()
        {
            var category = new Category {Name = "Legume"};
            var resultCategory = this.categoryService.AddCategory(category);
            var product = new Product {Name = "Varza", Categories = new[] {category}};
            var result = this.productService.AddProduct(product);
            product.Name = "Fasole";
            var updateResult = this.productService.UpdateProduct(product);
            var products = this.productService.GetProducts();
            var currentProduct = products.ToList()[0];
            var productById = this.productService.GetProductById(currentProduct.Id);
            Assert.IsTrue(productById.Name.Equals("Fasole"));
        }

        /// <summary>
        /// Test update with a name for auction user.
        /// </summary>
        [Test]
        public void TestUpdateWithBadNameForProduct()
        {
            var category = new Category {Name = "Legume"};
            var resultCategory = this.categoryService.AddCategory(category);
            var product = new Product {Name = "Varza", Categories = new[] {category}};
            var result = this.productService.AddProduct(product);
            product.Name = "aa";
            var updateResult = this.productService.UpdateProduct(product);
            var products = this.productService.GetProducts();
            var currentProduct = products.ToList()[0];
            var productById = this.productService.GetProductById(currentProduct.Id);
            Assert.IsFalse(productById.Name.Equals("aa"));
        }

        /// <summary>
        /// Test update categories for auction user.
        /// </summary>
        [Test]
        public void TestUpdateCategoriesForProduct()
        {
            var category = new Category {Name = "Legume"};
            var resultCategory = this.categoryService.AddCategory(category);
            var category2 = new Category {Name = "Electronice"};
            var resultCategory2 = this.categoryService.AddCategory(category2);
            var product = new Product {Name = "Varza", Categories = new[] {category}};
            var result = this.productService.AddProduct(product);
            product.Categories = new[] {category2};
            var updateResult = this.productService.UpdateProduct(product);
            var products = this.productService.GetProducts();
            var currentProduct = products.ToList()[0];
            var productById = this.productService.GetProductById(currentProduct.Id);
            Assert.IsTrue(productById.Categories.ToList()[0].Name.Equals("Electronice"));
        }

        /// <summary>
        /// Test update firstName for auction user.
        /// </summary>
        [Test]
        public void TestUpdateWithBadLastNameForProduct()
        {
            var category = new Category {Name = "Legume"};
            var resultCategory = this.categoryService.AddCategory(category);
            var category2 = new Category {Name = "Electronice"};
            var resultCategory2 = this.categoryService.AddCategory(category2);
            var product = new Product {Name = "Varza", Categories = new[] {category}};
            var result = this.productService.AddProduct(product);
            product.Categories = new List<Category>();
            var updateResult = this.productService.UpdateProduct(product);
            var products = this.productService.GetProducts();
            var currentProduct = products.ToList()[0];
            var productById = this.productService.GetProductById(currentProduct.Id);
            Assert.IsFalse(productById.Categories.ToList()[0].Name.Equals("Electronice"));
        }

        /// <summary>
        /// Test delete Product.
        /// </summary>
        [Test]
        public void TestDeleteProduct()
        {
            var category = new Category {Name = "Legume"};
            var resultCategory = this.categoryService.AddCategory(category);
            var product = new Product {Name = "Varza", Categories = new[] {category}};
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
            var category = new Category {Name = "Legume"};
            var resultCategory = this.categoryService.AddCategory(category);
            var product = new Product {Name = "Varza", Categories = new[] {category}};
            var result = this.productService.AddProduct(product);
            var products = this.productService.GetProducts();
            var currentProduct = products.ToList()[0];
            var deleteResult = this.productService.DeleteProduct(currentProduct.Id + 1);
            Assert.True(this.libraryContextMock.Products.Count() == 1);
        }
    }
}