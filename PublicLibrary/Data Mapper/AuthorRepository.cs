// <copyright file="AuthorRepository.cs" company="Transilvania University of Brasov">
// Margarit Marian Catalin
// </copyright>

namespace PublicLibrary.Data_Mapper
{
    using System.Linq;

    /// <summary>
    /// The author repository.
    /// </summary>
    public class AuthorRepository
    {
        private readonly LibraryDbContext libraryContext;

        /// <summary>
        /// Initializes a new instance of the <see cref="AuthorRepository"/> class.
        /// </summary>
        /// <param name="libraryContext">Tha database manager.</param>
        public AuthorRepository(LibraryDbContext libraryContext)
        {
            this.libraryContext = libraryContext;
        }

        /// <summary>
        /// Get author by firstname and lastname.
        /// </summary>
        /// <param name="firstName">The first name.</param>
        /// <param name="lastName">The last name.</param>
        /// <returns>An author.</returns>
        public Author GetAuthor(string firstName, string lastName)
        {
            return this.libraryContext.Authors.FirstOrDefault(
                a => a.FirstName.Equals(firstName) && a.LastName.Equals(lastName));
        }

        /// <summary>
        /// Add a new author.
        /// </summary>
        /// <param name="author">The new author.</param>
        /// <returns>If author was added.</returns>
        public bool AddAuthor(Author author)
        {
            this.libraryContext.Authors.Add(author);
            return this.libraryContext.SaveChanges() != 0;
        }
    }
}