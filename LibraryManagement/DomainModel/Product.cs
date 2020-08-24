// <copyright file="Product.cs" company="Transilvania University of Brasov">
// Margarit Marian Catalin
// </copyright>

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
        public int Id { get; set; }

        /// <summary>
        /// Gets or sets Product name.
        /// </summary>
        [StringLength(450)]
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets Product category.
        /// </summary>
        public ICollection<Category> Category { get; set; }
    }
}