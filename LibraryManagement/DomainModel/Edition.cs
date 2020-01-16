// <copyright file="Edition.cs" company="Transilvania University of Brasov">
// Margarit Marian Catalin
// </copyright>

namespace LibraryManagement.DomainModel
{
    using System.Collections.Generic;
    using LibraryManagement.DataMapper;

    /// <summary>
    /// Edition entity.
    /// </summary>
    public class Edition
    {
        /// <summary>
        /// Gets or sets id.
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Gets or sets name.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets pages.
        /// </summary>
        public int Pages { get; set; }

        /// <summary>
        /// Gets or sets book type.
        /// </summary>
        public string BookType { get; set; }

        /// <summary>
        /// Gets or sets book.
        /// </summary>
        public Book Book { get; set; }

        /// <summary>
        /// Gets or sets borrowed books.
        /// </summary>
        public virtual ICollection<BorrowedBook> BorrowedBooks { get; set; }

        /// <summary>
        /// Gets or sets book stock.
        /// </summary>
        public BookStock BookStock { get; set; }

        /// <inheritdoc/>
        public override string ToString()
        {
            return this.Name;
        }
    }
}