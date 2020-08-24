// <copyright file="Product.cs" company="Transilvania University of Brasov">
// Margarit Marian Catalin
// </copyright>
// <summary>This is the Product entity class.</summary>

namespace LibraryManagement.DomainModel
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    /// <summary>
    /// The Product entity.
    /// </summary>
    public class Product
    {
        /// <summary>
        /// Gets or sets Product code.
        /// </summary>
        /// <value>
        /// The identifier.
        /// </value>
        public int Id { get; set; }

        /// <summary>
        /// Gets or sets Product name.
        /// </summary>
        /// <value>
        /// The product name.
        /// </value>
        [StringLength(450)]
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets Product category.
        /// </summary>
        /// <value>
        /// The product category.
        /// </value>
        public ICollection<Category> Category { get; set; }
    }
}