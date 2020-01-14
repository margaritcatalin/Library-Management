// <copyright file="Employee.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace PublicLibrary.Data_Mapper
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using PublicLibrary.Domain_Model;

    /// <summary>
    /// Employee entity.
    /// </summary>
    public class Employee
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
        /// Gets or sets gender.
        /// </summary>
        public string Gender { get; set; }

        /// <summary>
        /// Gets or sets email.
        /// </summary>
        [Index(IsUnique = true)]
        [StringLength(450)]
        public string Email { get; set; }

        /// <summary>
        /// Gets or sets address.
        /// </summary>
        public string Address { get; set; }

        /// <summary>
        /// Gets or sets phone number.
        /// </summary>
        public string Phone { get; set; }

        /// <summary>
        /// Gets or sets bookwithdrawls.
        /// </summary>
        public virtual ICollection<BookWithdrawal> BookWithdrawals { get; set; }
    }
}