// <copyright file="Book.cs" company="Transilvania University of Brasov">
// Margarit Marian Catalin
// </copyright>

namespace LibraryManagement.DataMapper
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using LibraryManagement.DomainModel;

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
        /// Gets or sets book domains.
        /// </summary>
        public virtual ICollection<Domain> Categories { get; set; }

        /// <summary>
        /// Gets or sets book editions.
        /// </summary>
        public virtual ICollection<Edition> Editions { get; set; }

        /// <summary>
        /// Gets or sets book authors.
        /// </summary>
        public virtual ICollection<Author> Authors { get; set; }

        /// <summary>
        /// Gets or sets rented books.
        /// </summary>
        public virtual ICollection<RentedBook> RentedBooks { get; set; }
    }
}