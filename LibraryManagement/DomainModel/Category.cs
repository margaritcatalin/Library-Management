// <copyright file="Category.cs" company="Transilvania University of Brasov">
// Margarit Marian Catalin
// </copyright>
// <summary>This is the Category entity class.</summary>

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
        /// <value>
        /// The identifier.
        /// </value>
        public int Id { get; set; }

        /// <summary>
        /// Gets or sets name.
        /// </summary>
        /// <value>
        /// The category name.
        /// </value>
        [StringLength(450)]
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets products.
        /// </summary>
        /// <value>
        /// The product.
        /// </value>
        public virtual ICollection<Product> Product { get; set; }

        /// <summary>
        /// Gets or sets parent category.
        /// </summary>
        /// <value>
        /// The parent category identifier.
        /// </value>
        public int? ParentCategory_Id { get; set; }

        /// <summary>
        /// Gets or sets parent category.
        /// </summary>
        /// <value>
        /// The parent category.
        /// </value>
        [ForeignKey("ParentCategory_Id")]
        public Category ParentCategory { get; set; }

        /// <summary>
        /// Gets or sets subcategories
        /// category.
        /// </summary>
        /// <value>
        /// The categories.
        /// </value>
        public ICollection<Category> Categories { get; set; }
    }
}