// <copyright file="AuthorService.cs" company="Transilvania University of Brasov">
// Margarit Marian Catalin
// </copyright>

namespace LibraryManagement.BusinessLayer
{
    using System.Linq;
    using System.Reflection;
    using Castle.Core.Internal;
    using LibraryManagement.DataMapper;

    /// <summary>
    /// The author service.
    /// </summary>
    public class AuthorService
    {
        private readonly AuthorRepository authorRepository;

        /// <summary>Initializes a new instance of the <see cref="AuthorService"/> class.</summary>
        /// <param name="authorRepository">The author repository.</param>
        public AuthorService(AuthorRepository authorRepository)
        {
            this.authorRepository = authorRepository;
        }

        /// <summary>
        /// Addd a new author.
        /// </summary>
        /// <param name="author">The author.</param>
        /// <returns>If author was added.</returns>
        public bool AddAuthor(Author author)
        {
            if (author == null)
            {
                LoggerUtil.LogInfo($"Author is invalid. You tried to add an null author.", MethodBase.GetCurrentMethod());
                return false;
            }

            if (author.FirstName.IsNullOrEmpty())
            {
                LoggerUtil.LogInfo($"Author is invalid. You tried to add an author wtih null empty firstName.", MethodBase.GetCurrentMethod());
                return false;
            }

            if ((author.FirstName.Length < 3) || (author.FirstName.Length > 100))
            {
                LoggerUtil.LogInfo($"Author is invalid. You tried to add an author with invalid lenght firstName.", MethodBase.GetCurrentMethod());
                return false;
            }

            if (!author.FirstName.All(a => char.IsLetter(a) || char.IsWhiteSpace(a) || (a == '-')))
            {
                LoggerUtil.LogInfo($"Author is invalid. You tried to add an author with invalid character in FirstName.", MethodBase.GetCurrentMethod());
                return false;
            }

            if (char.IsLower(author.FirstName.First()))
            {
                LoggerUtil.LogInfo($"Author is invalid. You tried to add an author with firstName with lower case.", MethodBase.GetCurrentMethod());
                return false;
            }

            if (author.LastName.IsNullOrEmpty())
            {
                LoggerUtil.LogInfo($"Author is invalid. You tried to add an author with last name null or empty.", MethodBase.GetCurrentMethod());
                return false;
            }

            if ((author.LastName.Length < 3) || (author.LastName.Length > 100))
            {
                LoggerUtil.LogInfo($"Author is invalid. You tried to add an author with invalid length for lastName.", MethodBase.GetCurrentMethod());
                return false;
            }

            if (!author.LastName.All(a => char.IsLetter(a) || char.IsWhiteSpace(a) || (a == '-')))
            {
                LoggerUtil.LogInfo($"Author is invalid. You tried to add an author with invalid characters.", MethodBase.GetCurrentMethod());
                return false;
            }

            if (char.IsLower(author.LastName.First()))
            {
                LoggerUtil.LogInfo($"Author is invalid. You tried to add an author with lower case LastName.", MethodBase.GetCurrentMethod());
                return false;
            }

            if (author.Gender.IsNullOrEmpty())
            {
                LoggerUtil.LogInfo($"Author is invalid. You tried to add an author with null or empty gender.", MethodBase.GetCurrentMethod());
                return false;
            }

            if (!(author.Gender.Equals("M") || author.Gender.Equals("F")))
            {
                LoggerUtil.LogInfo($"Author is invalid. You tried to add an author with invalid gender.", MethodBase.GetCurrentMethod());
                return false;
            }

            return this.authorRepository.AddAuthor(author);
        }

        /// <summary>
        /// Get author by firstName and lastName.
        /// </summary>
        /// <param name="firstName">The first name.</param>
        /// <param name="lastName">The last name.</param>
        /// <returns>An author.</returns>
        public Author GetAuthor(string firstName, string lastName)
        {
            if (firstName.IsNullOrEmpty())
            {
                LoggerUtil.LogInfo($"Param firstName is required.", MethodBase.GetCurrentMethod());
                return null;
            }

            if (lastName.IsNullOrEmpty())
            {
                LoggerUtil.LogInfo($"Param lastName is required.", MethodBase.GetCurrentMethod());
                return null;
            }

            return this.authorRepository.GetAuthor(firstName, lastName);
        }
    }
}