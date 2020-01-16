// <copyright file="Author.cs" company="Transilvania University of Brasov">
// Margarit Marian Catalin
// </copyright>

namespace LibraryManagement.DataMapper
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
        /// Gets or sets Author firstName.
        /// </summary>
        [Index(IsUnique = true)]
        [StringLength(450)]
        public string FirstName { get; set; }

        /// <summary>
        /// Gets or sets author lastName.
        /// </summary>
        [Index(IsUnique = true)]
        [StringLength(450)]
        public string LastName { get; set; }

        /// <summary>
        /// Gets or sets author gender.
        /// </summary>
        public string Gender { get; set; }

        /// <summary>
        /// Gets or sets author books.
        /// </summary>
        public ICollection<Book> Books { get; set; }

        /// <inheritdoc/>
        public override string ToString()
        {
            return this.FirstName;
        }
    }
}