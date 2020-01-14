// <copyright file="Extension.cs" company="Transilvania University of Brasov">
// Margarit Marian Catalin
// </copyright>

namespace PublicLibrary.Domain_Model
{
    using System;

    /// <summary>
    /// The extension entity.
    /// </summary>
    public class Extension
    {
        /// <summary>
        /// Gets or sets id.
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Gets or sets reader.
        /// </summary>
        public Reader Reader { get; set; }

        /// <summary>
        /// Gets or sets date.
        /// </summary>
        public DateTime Date { get; set; }

        /// <summary>
        /// Gets or sets bookwithdrawl.
        /// </summary>
        public BookWithdrawal BookWithdrawal { get; set; }
    }
}