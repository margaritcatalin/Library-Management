// <copyright file="Price.cs" company="Transilvania University of Brasov">
// Margarit Marian Catalin
// </copyright>
// <summary>This is the Price entity class.</summary>

namespace LibraryManagement.DomainModel
{
    using System.ComponentModel.DataAnnotations;

    /// <summary>
    /// Price entity.
    /// </summary>
    public class Price
    {
        /// <summary>
        /// Gets or sets Price code.
        /// </summary>
        /// <value>
        /// The identifier.
        /// </value>
        public int Id { get; set; }

        /// <summary>
        /// Gets or sets Price name.
        /// </summary>
        /// <value>
        /// The price currency.
        /// </value>
        [StringLength(450)]
        public string Currency { get; set; }

        /// <summary>
        /// Gets or sets Price name.
        /// </summary>
        /// <value>
        /// The price value.
        /// </value>
        public double Value { get; set; }
    }
}