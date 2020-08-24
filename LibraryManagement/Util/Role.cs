// <copyright file="Role.cs" company="Transilvania University of Brasov">
// Margarit Marian Catalin
// </copyright>
// <summary>This is the Role class.</summary>

namespace LibraryManagement.Util
{
    /// <summary>
    /// Defines the <see cref="Role" />.
    /// </summary>
    public class Role
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Role" /> class.
        /// </summary>
        /// <param name="value">The value.</param>
        private Role(string value)
        {
            this.Value = value;
        }

        /// <summary>
        /// Gets the Seller.
        /// </summary>
        /// <value>
        /// The seller role.
        /// </value>
        public static Role Seller
        {
            get { return new Role("Seller"); }
        }

        /// <summary>
        /// Gets the Buyer.
        /// </summary>
        /// <value>
        /// The buyer role.
        /// </value>
        public static Role Buyer
        {
            get { return new Role("Buyer"); }
        }

        /// <summary>
        /// Gets or sets the Value.
        /// </summary>
        /// <value>
        /// The value.
        /// </value>
        public string Value { get; set; }
    }
}