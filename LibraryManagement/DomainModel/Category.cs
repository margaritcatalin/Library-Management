// <copyright file="Category.cs" company="Transilvania University of Brasov">
// Margarit Marian Catalin
// </copyright>

namespace LibraryManagement.DomainModel
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

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
        [StringLength(450)]
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets products.
        /// </summary>
        public virtual ICollection<Product> Product { get; set; }

        /// <summary>
        /// Gets or sets parent category.
        /// </summary>
        public int? ParentCategory_Id { get; set; }

        /// <summary>
        /// Gets or sets parent category.
        /// </summary>
        [ForeignKey("ParentCategory_Id")]
        public Category ParentCategory { get; set; }

        /// <summary>
        /// Gets or sets subcategories
        /// category.
        /// </summary>
        public ICollection<Category> Categories { get; set; }
    }
}