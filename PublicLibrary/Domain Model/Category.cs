// <copyright file="Category.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace PublicLibrary.Domain_Model
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using PublicLibrary.Data_Mapper;

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
        /// Gets or sets books.
        /// </summary>
        public virtual ICollection<Book> Books { get; set; }

        /// <summary>
        /// Gets or sets parent category.
        /// </summary>
        public Category ParentCategory { get; set; }
    }
}