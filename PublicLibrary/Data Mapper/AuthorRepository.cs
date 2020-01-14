// <copyright file="AuthorRepository.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace PublicLibrary.Data_Mapper
{
    using System.Linq;

    /// <summary>
    /// The author repository.
    /// </summary>
    public class AuthorRepository
    {
        private LibraryDb libraryDb;

        /// <summary>
        /// Initializes a new instance of the <see cref="AuthorRepository"/> class.
        /// </summary>
        /// <param name="libraryDb">Tha database manager.</param>
        public AuthorRepository(LibraryDb libraryDb)
        {
            this.libraryDb = libraryDb;
        }

        /// <summary>
        /// Get author by firstname and lastname.
        /// </summary>
        /// <param name="firstName">The first name.</param>
        /// <param name="lastName">The last name.</param>
        /// <returns>An author.</returns>
        public Author GetAuthor(string firstName, string lastName)
        {
            return this.libraryDb.Authors.FirstOrDefault(
                a => a.FirstName.Equals(firstName) && a.LastName.Equals(lastName));
        }

        /// <summary>
        /// Add a new author.
        /// </summary>
        /// <param name="author">The new author.</param>
        /// <returns>If author was added.</returns>
        public bool AddAuthor(Author author)
        {
            this.libraryDb.Authors.Add(author);
            return this.libraryDb.SaveChanges() != 0;
        }
    }
}