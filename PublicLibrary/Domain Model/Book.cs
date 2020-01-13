// <copyright file="Book.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace PublicLibrary.Data_Mapper
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using PublicLibrary.Domain_Model;

    /// <summary>
    /// The book entity.
    /// </summary>
    public class Book
    {
        /// <summary>
        /// Gets or sets book code.
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Gets or sets book name.
        /// </summary>
        [Index("Id", IsUnique = true)]
        [StringLength(450)]
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets book categories.
        /// </summary>
        public virtual ICollection<Category> Categories { get; set; }

        /// <summary>
        /// Gets or sets book editions.
        /// </summary>
        public virtual ICollection<Edition> Editions { get; set; }

        /// <summary>
        /// Gets or sets book authors.
        /// </summary>
        public virtual ICollection<Author> Authors { get; set; }

        /// <summary>
        /// Gets or sets borrowed books.
        /// </summary>
        public virtual ICollection<BorrowedBook> BorrowedBooks { get; set; }
    }
}