// <copyright file="Author.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace PublicLibrary.Data_Mapper
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    /// <summary>
    /// Author entity.
    /// </summary>
    public class Author
    {
        /// <summary>
        /// Gets or sets Author code.
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Gets or sets Author name.
        /// </summary>
        [Index(IsUnique = true)]
        [StringLength(450)]
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets author books.
        /// </summary>
        public ICollection<Book> Books { get; set; }


        /// <inheritdoc/>
        public override string ToString()
        {
            return this.Name;
        }
    }
}