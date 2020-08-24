// <copyright file="LibraryDbContext.cs" company="Transilvania University of Brasov">
// Margarit Marian Catalin
// </copyright>

namespace LibraryManagement.Util
{
    /// <summary>
    /// Defines the <see cref="Role" />.
    /// </summary>
    public class Role
    {
        /// <summary>
        /// Prevents a default instance of the <see cref="Role"/> class from being created.
        /// </summary>
        /// <param name="value">The value<see cref="string"/>.</param>
        private Role(string value)
        {
            Value = value;
        }

        /// <summary>
        /// Gets or sets the Value.
        /// </summary>
        public string Value { get; set; }

        /// <summary>
        /// Gets the Seller.
        /// </summary>
        public static Role Seller
        {
            get { return new Role("Seller"); }
        }

        /// <summary>
        /// Gets the Buyer.
        /// </summary>
        public static Role Buyer
        {
            get { return new Role("Buyer"); }
        }
    }
}