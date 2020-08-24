// <copyright file="ProductService.cs" company="Transilvania University of Brasov">
// Margarit Marian Catalin
// </copyright>
// <summary>This is the Product service class.</summary>

namespace LibraryManagement.BusinessLayer
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Reflection;
    using Castle.Core.Internal;
    using LibraryManagement.DataMapper;
    using LibraryManagement.DomainModel;
    using LibraryManagement.Util;

    /// <summary>
    /// The Product service.
    /// </summary>
    public class ProductService
    {
        /// <summary>
        /// Defines the productRepository.
        /// </summary>
        private readonly ProductRepository productRepository;

        /// <summary>
        /// Initializes a new instance of the <see cref="ProductService"/> class.
        /// </summary>
        /// <param name="productRepository">The Product repository.</param>
        public ProductService(ProductRepository productRepository)
        {
            this.productRepository = productRepository;
        }

        /// <summary>
        /// Add a new Product.
        /// </summary>
        /// <param name="product">The Product.</param>
        /// <returns>If Product was added.</returns>
        public bool AddProduct(Product product)
        {
            if (this.ValidateProduct(product))
            {
                return this.productRepository.AddProduct(product);
            }

            return false;
        }

        /// <summary>
        /// Get All AuctionUsers.
        /// </summary>
        /// <returns>All AuctionUsers.</returns>
        public IEnumerable<Product> GetProducts()
        {
            return this.productRepository.GetProducts();
        }

        /// <summary>
        /// Get Product by Product id.
        /// </summary>
        /// <param name="id">The Product id.</param>
        /// <returns>An Product.</returns>
        public Product GetProductById(int id)
        {
            return this.productRepository.GetProductById(id);
        }

        /// <summary>
        /// Update an Product.
        /// </summary>
        /// <param name="product">The Product.</param>
        /// <returns>If Product was updated.</returns>
        public bool UpdateProduct(Product product)
        {
            if (this.ValidateProduct(product))
            {
                return this.productRepository.UpdateProduct(product);
            }

            return false;
        }

        /// <summary>
        /// Delete Product.
        /// </summary>
        /// <param name="id">The Product id.</param>
        /// <returns>If Product was deleted.</returns>
        public bool DeleteProduct(int id)
        {
            return this.productRepository.DeleteProduct(id);
        }

        /// <summary>
        /// Validation for Product user.
        /// </summary>
        /// <param name="product">The Product.</param>
        /// <returns>If Product is valid or not.</returns>
        private bool ValidateProduct(Product product)
        {
            if (product == null)
            {
                LoggerUtil.LogInfo($"Product is invalid. You tried to add a null Product.", MethodBase.GetCurrentMethod());
                return false;
            }

            if (product.Category.IsNullOrEmpty())
            {
                LoggerUtil.LogInfo($"Product is invalid. You need to add a category to product.", MethodBase.GetCurrentMethod());
                return false;
            }

            if (product.Name.IsNullOrEmpty())
            {
                LoggerUtil.LogInfo($"Product is invalid. You tried to add a Product with null empty name.", MethodBase.GetCurrentMethod());
                return false;
            }

            if ((product.Name.Length < 3) || (product.Name.Length > 100))
            {
                LoggerUtil.LogInfo($"Product is invalid. You tried to add a Product with invalid lenght name.", MethodBase.GetCurrentMethod());
                return false;
            }

            if (char.IsLower(product.Name.First()))
            {
                LoggerUtil.LogInfo($"Product is invalid. You tried to add a Product with name with lower case.", MethodBase.GetCurrentMethod());
                return false;
            }

            return true;
        }
    }
}