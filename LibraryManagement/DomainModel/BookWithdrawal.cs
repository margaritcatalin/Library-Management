// <copyright file="BookWithdrawal.cs" company="Transilvania University of Brasov">
// Margarit Marian Catalin
// </copyright>

namespace LibraryManagement.DomainModel
{
    using System;
    using System.Collections.Generic;
    using LibraryManagement.DataMapper;

    /// <summary>
    /// The bookWihdrawl entity.
    /// </summary>
    public class BookWithdrawal
    {
        /// <summary>
        /// Gets or sets id.
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Gets or sets date.
        /// </summary>
        public DateTime Date { get; set; }

        /// <summary>
        /// Gets or sets reader.
        /// </summary>
        public Reader Reader { get; set; }

        /// <summary>
        /// Gets or sets rented books.
        /// </summary>
        public ICollection<RentedBook> RentedBooks { get; set; }

        /// <summary>
        /// Gets or sets extensions.
        /// </summary>
        public ICollection<Extension> Extensions { get; set; }

        /// <summary>
        /// Gets or sets librarian.
        /// </summary>
        public Librarian Librarian { get; set; }
    }
}