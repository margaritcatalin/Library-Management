// <copyright file="LibrarianRepository.cs" company="Transilvania University of Brasov">
// Margarit Marian Catalin
// </copyright>

namespace LibraryManagement.DataMapper
{
    using System.Linq;
    using System.Reflection;

    /// <summary>
    /// The librarian repository.
    /// </summary>
    public class LibrarianRepository
    {
        private readonly LibraryDbContext libraryContext;

        /// <summary>
        /// Initializes a new instance of the <see cref="LibrarianRepository"/> class.
        /// </summary>
        /// <param name="libraryContext">The library database.</param>
        public LibrarianRepository(LibraryDbContext libraryContext)
        {
            this.libraryContext = libraryContext;
        }

        /// <summary>
        /// Add a new librarian.
        /// </summary>
        /// <param name="librarian">The librarian.</param>
        /// <returns>If librarian was added.</returns>
        public bool AddLibrarian(Librarian librarian)
        {
            this.libraryContext.Librarians.Add(librarian);
            var successful = this.libraryContext.SaveChanges() != 0;
            if (successful)
            {
                LoggerUtil.LogInfo($"Librarian added successfully : {librarian.FirstName} {librarian.LastName}", MethodBase.GetCurrentMethod());
            }
            else
            {
                LoggerUtil.LogError($"Librarian failed to add to db : {librarian.FirstName} {librarian.LastName}", MethodBase.GetCurrentMethod());
            }

            return successful;
        }

        /// <summary>
        /// Get librarian by email.
        /// </summary>
        /// <param name="email">The email.</param>
        /// <returns>An librarian.</returns>
        public Librarian GetLibrarian(string email)
        {
            return this.libraryContext.Librarians.FirstOrDefault(e => e.Email.Equals(email));
        }
    }
}