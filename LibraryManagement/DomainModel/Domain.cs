// <copyright file="Domain.cs" company="Transilvania University of Brasov">
// Margarit Marian Catalin
// </copyright>

namespace LibraryManagement.DomainModel
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using LibraryManagement.DataMapper;

    /// <summary>
    /// Domain entity.
    /// </summary>
    public class Domain
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
        /// Gets or sets parent domain.
        /// </summary>
        public Domain ParentDomain { get; set; }
    }
}