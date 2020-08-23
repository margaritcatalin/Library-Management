// <copyright file="Category.cs" company="Transilvania University of Brasov">
// Margarit Marian Catalin
// </copyright>

namespace LibraryManagement.DomainModel
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using LibraryManagement.DataMapper;

    /// <summary>
    /// Category entity.
    /// </summary>
    public class Category
    {
        /// <summary>
        /// Gets or sets id.
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Gets or sets name.
        /// </summary>
        [Index(IsUnique = true)]
        [StringLength(450)]
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets products.
        /// </summary>
        public ICollection<Product> Products { get; set; }

        /// <summary>
        /// Gets or sets parent category.
        /// </summary>
        public Category ParentCategory { get; set; }
    }
}