// <copyright file="CategoriesService.cs" company="Transilvania University of Brasov">
// Margarit Marian Catalin
// </copyright>

namespace LibraryManagement.BusinessLayer
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Reflection;
    using Castle.Core.Internal;
    using LibraryManagement.DataMapper;
    using LibraryManagement.DomainModel;

    /// <summary>
    /// The domains service.
    /// </summary>
    public class DomainsService
    {
        private readonly DomainsRepository domainRepository;

        /// <summary>
        /// Initializes a new instance of the <see cref="DomainsService"/> class.
        /// </summary>
        /// <param name="domainRepository">The domain repository.</param>
        public DomainsService(DomainsRepository domainRepository)
        {
            this.domainRepository = domainRepository;
        }

        /// <summary>
        /// Add a new domain.
        /// </summary>
        /// <param name="domain">The new domain.</param>
        /// <returns>If domain was added.</returns>
        public bool AddDomain(Domain domain)
        {
            if (domain == null)
            {
                LoggerUtil.LogInfo($"Your domain is invalid. Domain is null.", MethodBase.GetCurrentMethod());
                return false;
            }

            if (domain.Name.IsNullOrEmpty())
            {
                LoggerUtil.LogInfo($"Your domain is invalid. Domain name is null or empty.", MethodBase.GetCurrentMethod());
                return false;
            }

            if ((domain.Name.Length < 3) || (domain.Name.Length > 80))
            {
                LoggerUtil.LogInfo($"Your domain is invalid. Domain name has a invalid length.", MethodBase.GetCurrentMethod());
                return false;
            }

            if (domain.Name.Any(c => !(char.IsLetter(c) || char.IsWhiteSpace(c))))
            {
                LoggerUtil.LogInfo($"Your domain is invalid. Domain name is invalid.", MethodBase.GetCurrentMethod());
                return false;
            }

            return this.domainRepository.AddDomain(domain);
        }

        /// <summary>
        /// Check if book is in domain.
        /// </summary>
        /// <param name="book">The book.</param>
        /// <param name="domain">The domain.</param>
        /// <returns>If book is in domain.</returns>
        public bool IsPartOfDomain(Book book, Domain domain)
        {
            return this.DomainIsPartOfCategories(domain, book.Categories.ToList());
        }

        /// <summary>
        /// Check if domain is in domain list.
        /// </summary>
        /// <param name="domain">The domain.</param>
        /// <param name="domains">The domains list.</param>
        /// <returns>If domain is in domains list.</returns>
        public bool DomainIsPartOfCategories(Domain domain, List<Domain> domains)
        {
            while (domain != null)
            {
                if (this.CheckCategories(domain, domains))
                {
                    return true;
                }

                domain = domain.ParentDomain;
            }

            return false;
        }

        /// <summary>
        /// Get domains by name.
        /// </summary>
        /// <param name="name">The name.</param>
        /// <returns>A domain.</returns>
        public Domain GetDomain(string name)
        {
            if (name.IsNullOrEmpty())
            {
                LoggerUtil.LogInfo($"Param name is required.", MethodBase.GetCurrentMethod());
                return null;
            }

            return this.domainRepository.GetDomain(name);
        }

        /// <summary>
        /// Check domains if is in domains.
        /// </summary>
        /// <param name="domain">The domain.</param>
        /// <param name="domains">The domains.</param>
        /// <returns>If is in domains.</returns>
        private bool CheckCategories(Domain domain, List<Domain> domains)
        {
            foreach (var bookDomain in domains)
            {
                var currentDomain = bookDomain;

                while (currentDomain != null)
                {
                    if (domain.Name.Equals(currentDomain.Name))
                    {
                        return true;
                    }

                    currentDomain = currentDomain.ParentDomain;
                }
            }

            return false;
        }
    }
}