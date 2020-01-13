// <copyright file="Reader.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace PublicLibrary.Domain_Model
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    /// <summary>
    /// The reader entity.
    /// </summary>
    public class Reader
    {
        /// <summary>
        /// Gets or sets id.
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Gets or sets firstName.
        /// </summary>
        public string FirstName { get; set; }

        /// <summary>
        /// Gets or sets lastName.
        /// </summary>
        public string LastName { get; set; }

        /// <summary>
        /// Gets or sets address.
        /// </summary>
        public string Address { get; set; }

        /// <summary>
        /// Gets or sets phone.
        /// </summary>
        [Index(IsUnique = true)]
        [StringLength(450)]
        public string Phone { get; set; }

        /// <summary>
        /// Gets or sets email.
        /// </summary>
        [Index(IsUnique = true)]
        [StringLength(450)]
        public string Email { get; set; }

        /// <summary>
        /// Gets or sets bookwithdrawls.
        /// </summary>
        public virtual ICollection<BookWithdrawal> BookWithdrawals { get; set; }

        /// <summary>
        /// Gets or sets extensions.
        /// </summary>
        public virtual ICollection<Extension> Extensions { get; set; }
    }
}