// <copyright file="RentedBook.cs" company="Transilvania University of Brasov">
// Margarit Marian Catalin
// </copyright>

namespace LibraryManagement.DomainModel
{
    using LibraryManagement.DataMapper;

    /// <summary>
    /// The rented book entity.
    /// </summary>
    public class RentedBook
    {
        /// <summary>
        /// Gets or sets id.
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Gets or sets book.
        /// </summary>
        public Book Book { get; set; }

        /// <summary>
        /// Gets or sets bookwithdrawl.
        /// </summary>
        public BookWithdrawal BookWithdrawal { get; set; }

        /// <summary>
        /// Gets or sets edition.
        /// </summary>
        public Edition Edition { get; set; }
    }
}