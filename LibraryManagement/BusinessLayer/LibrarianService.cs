// <copyright file="LibrarianService.cs" company="Transilvania University of Brasov">
// Margarit Marian Catalin
// </copyright>

namespace LibraryManagement.BusinessLayer
{
    using System.Linq;
    using System.Reflection;
    using Castle.Core.Internal;
    using LibraryManagement.DataMapper;

    /// <summary>
    /// The librarian service.
    /// </summary>
    public class LibrarianService
    {
        private readonly LibrarianRepository librarianRepository;

        /// <summary>
        /// Initializes a new instance of the <see cref="LibrarianService"/> class.
        /// </summary>
        /// <param name="librarianRepository">The librarian repository.</param>
        public LibrarianService(LibrarianRepository librarianRepository)
        {
            this.librarianRepository = librarianRepository;
        }

        /// <summary>
        /// Add a new librarian.
        /// </summary>
        /// <param name="librarian">The librarian.</param>
        /// <returns>If librarian was created.</returns>
        public bool AddLibrarian(Librarian librarian)
        {
            if (librarian == null)
            {
                LoggerUtil.LogInfo($"Your librarian is invalid. Librarian is null.", MethodBase.GetCurrentMethod());
                return false;
            }

            if (librarian.FirstName.IsNullOrEmpty())
            {
                LoggerUtil.LogInfo($"Your librarian is invalid. Librarian firstName is null or empty.", MethodBase.GetCurrentMethod());
                return false;
            }

            if ((librarian.FirstName.Length < 3) || (librarian.FirstName.Length > 80))
            {
                LoggerUtil.LogInfo($"Your librarian is invalid. Librarian firstName has a invalid length.", MethodBase.GetCurrentMethod());
                return false;
            }

            if (librarian.FirstName.Any(c => !(char.IsLetter(c) || char.IsWhiteSpace(c))))
            {
                LoggerUtil.LogInfo($"Your librarian is invalid. Librarian firstName is invalid.", MethodBase.GetCurrentMethod());
                return false;
            }

            if (char.IsLower(librarian.FirstName.First()))
            {
                LoggerUtil.LogInfo($"Your librarian is invalid. Librarian firstName is need to start with uppercase.", MethodBase.GetCurrentMethod());
                return false;
            }

            if (librarian.LastName.IsNullOrEmpty())
            {
                LoggerUtil.LogInfo($"Your librarian is invalid. Librarian lastName is null or empty.", MethodBase.GetCurrentMethod());
                return false;
            }

            if ((librarian.LastName.Length < 3) || (librarian.LastName.Length > 80))
            {
                LoggerUtil.LogInfo($"Your librarian is invalid. Librarian lastName length is invalid.", MethodBase.GetCurrentMethod());
                return false;
            }

            if (librarian.LastName.Any(c => !(char.IsLetter(c) || char.IsWhiteSpace(c))))
            {
                LoggerUtil.LogInfo($"Your librarian is invalid. Librarian lastName is invalid.", MethodBase.GetCurrentMethod());
                return false;
            }

            if (char.IsLower(librarian.LastName.First()))
            {
                LoggerUtil.LogInfo($"Your librarian is invalid. Librarian lastName is need to start with upperCase.", MethodBase.GetCurrentMethod());
                return false;
            }

            if (librarian.Address.IsNullOrEmpty())
            {
                LoggerUtil.LogInfo($"Your librarian is invalid. Librarian address is null or empty.", MethodBase.GetCurrentMethod());
                return false;
            }

            if ((librarian.Address.Length < 3) || (librarian.Address.Length > 120))
            {
                LoggerUtil.LogInfo($"Your librarian is invalid. Librarian address has an invalid length.", MethodBase.GetCurrentMethod());
                return false;
            }

            if (librarian.Address.Any(
                c => !(char.IsLetterOrDigit(c) || char.IsWhiteSpace(c) || (c == '.') || (c == ','))))
            {
                LoggerUtil.LogInfo($"Your librarian is invalid. Librarian address is invalid.", MethodBase.GetCurrentMethod());
                return false;
            }

            if (librarian.Email.IsNullOrEmpty())
            {
                LoggerUtil.LogInfo($"Your librarian is invalid. Librarian email is null or empty.", MethodBase.GetCurrentMethod());
                return false;
            }

            if ((librarian.Email.Length < 10) || (librarian.Email.Length > 150))
            {
                LoggerUtil.LogInfo($"Your librarian is invalid. Librarian email has invalid length.", MethodBase.GetCurrentMethod());
                return false;
            }

            if (librarian.Email.Any(
                c => !(char.IsLetterOrDigit(c) || (c == '@') || (c == '.') || (c == '_') || (c == '-'))))
            {
                LoggerUtil.LogInfo($"Your librarian is invalid. Librarian email is invalid.", MethodBase.GetCurrentMethod());
                return false;
            }

            if (librarian.Email.All(c => c != '@'))
            {
                LoggerUtil.LogInfo($"Your librarian is invalid. Librarian email is invalid.", MethodBase.GetCurrentMethod());
                return false;
            }

            if (librarian.Phone.IsNullOrEmpty())
            {
                LoggerUtil.LogInfo($"Your librarian is invalid. Librarian phone is null or empty.", MethodBase.GetCurrentMethod());
                return false;
            }

            if (librarian.Phone.Length != 10)
            {
                LoggerUtil.LogInfo($"Your librarian is invalid. Librarian phone has invalid length.", MethodBase.GetCurrentMethod());
                return false;
            }

            if (librarian.Phone.Any(c => !char.IsDigit(c)))
            {
                LoggerUtil.LogInfo($"Your librarian is invalid. Librarian phone is invalid.", MethodBase.GetCurrentMethod());
                return false;
            }

            if (librarian.Gender.IsNullOrEmpty())
            {
                LoggerUtil.LogInfo($"Your librarian is invalid. Librarian gerner is null or empty.", MethodBase.GetCurrentMethod());
                return false;
            }

            if (!(librarian.Gender.Equals("M") || librarian.Gender.Equals("F")))
            {
                LoggerUtil.LogInfo($"Your librarian is invalid. Librarian gender is invalid.", MethodBase.GetCurrentMethod());
                return false;
            }

            return this.librarianRepository.AddLibrarian(librarian);
        }

        /// <summary>
        /// Get librarian by email.
        /// </summary>
        /// <param name="email">The email.</param>
        /// <returns>An librarian.</returns>
        public Librarian GetLibrarian(string email)
        {
            if (email.IsNullOrEmpty())
            {
                LoggerUtil.LogInfo($"Param email is required.", MethodBase.GetCurrentMethod());
                return null;
            }

            return this.librarianRepository.GetLibrarian(email);
        }
    }
}