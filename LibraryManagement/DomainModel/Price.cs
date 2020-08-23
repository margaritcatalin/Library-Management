// <copyright file="Price.cs" company="Transilvania University of Brasov">
// Margarit Marian Catalin
// </copyright>

namespace LibraryManagement.DomainModel
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    /// <summary>
    /// Price entity.
    /// </summary>
    public class Price
    {
        /// <summary>
        /// Gets or sets Price code.
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Gets or sets Price name.
        /// </summary>
        [StringLength(450)]
        public string Currency { get; set; }

        /// <summary>
        /// Gets or sets Price name.
        /// </summary>
        public double Value { get; set; }
    }
}