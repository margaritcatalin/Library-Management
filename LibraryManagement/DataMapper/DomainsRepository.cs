// <copyright file="CategoriesRepository.cs" company="Transilvania University of Brasov">
// Margarit Marian Catalin
// </copyright>

namespace LibraryManagement.DataMapper
{
    using System.Linq;
    using System.Reflection;
    using LibraryManagement.DomainModel;

    /// <summary>
    /// The domains repository.
    /// </summary>
    public class DomainsRepository
    {
        private readonly LibraryDbContext libraryContext;

        /// <summary>
        /// Initializes a new instance of the <see cref="DomainsRepository"/> class.
        /// </summary>
        /// <param name="libraryContext">The library database.</param>
        public DomainsRepository(LibraryDbContext libraryContext)
        {
            this.libraryContext = libraryContext;
        }

        /// <summary>
        /// Add a new domain.
        /// </summary>
        /// <param name="domain">The new domain.</param>
        /// <returns>If domain was added.</returns>
        public bool AddDomain(Domain domain)
        {
            this.libraryContext.Categories.Add(domain);
            var successful = this.libraryContext.SaveChanges() != 0;
            if (successful)
            {
                LoggerUtil.LogInfo($"Domain added successfully : {domain.Name} ", MethodBase.GetCurrentMethod());
            }
            else
            {
                LoggerUtil.LogError($"Domain failed to add to db : {domain.Name}", MethodBase.GetCurrentMethod());
            }

            return successful;
        }

        /// <summary>
        /// Get domain by name.
        /// </summary>
        /// <param name="name">The name.</param>
        /// <returns>A domain.</returns>
        public Domain GetDomain(string name)
        {
            return this.libraryContext.Categories.FirstOrDefault(c => c.Name.Equals(name));
        }
    }
}