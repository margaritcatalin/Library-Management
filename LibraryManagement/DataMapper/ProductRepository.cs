// <copyright file="ProductRepository.cs" company="Transilvania University of Brasov">
// Margarit Marian Catalin
// </copyright>
// <summary>This is the Product repository class.</summary>

namespace LibraryManagement.DataMapper
{
    using System.Collections.Generic;
    using System.Data.Entity;
    using System.Linq;
    using System.Reflection;
    using LibraryManagement.DomainModel;
    using LibraryManagement.Util;

    /// <summary>
    /// The Product repository.
    /// </summary>
    public class ProductRepository
    {
        /// <summary>
        /// Defines the libraryContext.
        /// </summary>
        private readonly LibraryDbContext libraryContext;

        /// <summary>
        /// Initializes a new instance of the <see cref="ProductRepository"/> class.
        /// </summary>
        /// <param name="libraryContext">The database manager.</param>
        public ProductRepository(LibraryDbContext libraryContext)
        {
            this.libraryContext = libraryContext;
        }

        /// <summary>
        /// Add a new Product.
        /// </summary>
        /// <param name="product">The new Product.</param>
        /// <returns>If Product was added.</returns>
        public bool AddProduct(Product product)
        {
            this.libraryContext.Products.Add(product);
            var successful = this.libraryContext.SaveChanges() != 0;
            if (successful)
            {
                LoggerUtil.LogInfo($"Product added successfully : {product.Id},{product.Name} ", MethodBase.GetCurrentMethod());
            }
            else
            {
                LoggerUtil.LogError($"Product failed to add to database : {product.Id},{product.Name}", MethodBase.GetCurrentMethod());
            }

            return successful;
        }

        /// <summary>
        /// Get All Products.
        /// </summary>
        /// <returns>All Products.</returns>
        public IEnumerable<Product> GetProducts()
        {
            return this.libraryContext.Products.ToList();
        }

        /// <summary>
        /// Get Product by id.
        /// </summary>
        /// <param name="id">The Product id.</param>
        /// <returns>A Product.</returns>
        public Product GetProductById(int id)
        {
            return this.libraryContext.Products.Find(id);
        }

        /// <summary>
        /// Update a Product.
        /// </summary>
        /// <param name="product">The Product.</param>
        /// <returns>If Product was updated.</returns>
        public bool UpdateProduct(Product product)
        {
            this.libraryContext.Entry(product).State = EntityState.Modified;
            var successful = this.libraryContext.SaveChanges() != 0;
            if (successful)
            {
                LoggerUtil.LogInfo($"Product updated successfully : {product.Id},{product.Name}", MethodBase.GetCurrentMethod());
            }

            return successful;
        }

        /// <summary>
        /// Delete a Product.
        /// </summary>
        /// <param name="id">The Product id.</param>
        /// <returns>If Product was deleted.</returns>
        public bool DeleteProduct(int id)
        {
            var product = this.libraryContext.Products.Find(id);
            if (product != null)
            {
                this.libraryContext.Products.Remove(product);
            }
            else
            {
                LoggerUtil.LogWarning($"Product failed to delete from database. We don't found a product with id: {id}", MethodBase.GetCurrentMethod());
                return false;
            }

            var successful = this.libraryContext.SaveChanges() != 0;
            if (successful)
            {
                LoggerUtil.LogInfo($"Product was deleted successfully : {id} ", MethodBase.GetCurrentMethod());
            }
            else
            {
                LoggerUtil.LogError($"Product failed to delete from database : {id}", MethodBase.GetCurrentMethod());
            }

            return successful;
        }
    }
}