// <copyright file="ReaderRepository.cs" company="Transilvania University of Brasov">
// Margarit Marian Catalin
// </copyright>

namespace LibraryManagement.DataMapper
{
    using System;
    using System.Collections.Generic;
    using System.Data.Entity;
    using System.Linq;
    using System.Reflection;
    using LibraryManagement.DomainModel;

    /// <summary>
    /// The reader repository.
    /// </summary>
    public class ReaderRepository
    {
        private readonly LibraryDbContext libraryContext;

        /// <summary>
        /// Initializes a new instance of the <see cref="ReaderRepository"/> class.
        /// </summary>
        /// <param name="libraryContext">The library db manager.</param>
        public ReaderRepository(LibraryDbContext libraryContext)
        {
            this.libraryContext = libraryContext;
        }

        /// <summary>
        /// Get a reader by email and phone.
        /// </summary>
        /// <param name="email">The email.</param>
        /// <param name="phone">The phone.</param>
        /// <returns>A reader.</returns>
        public Reader GetReader(string email, string phone)
        {
            if (!string.IsNullOrEmpty(email))
            {
                return this.libraryContext.Readers.FirstOrDefault(r => r.Email.Equals(email));
            }

            if (!string.IsNullOrEmpty(phone))
            {
                return this.libraryContext.Readers.FirstOrDefault(r => r.Phone.Equals(phone));
            }

            LoggerUtil.LogInfo($"Reader not found in db with email or phone : {email} {phone}", MethodBase.GetCurrentMethod());
            return null;
        }

        /// <summary>
        /// Get books by withdrawl period.
        /// </summary>
        /// <param name="days">Number of days.</param>
        /// <param name="reader">The reader.</param>
        /// <param name="dateTime">The date.</param>
        /// <returns>The books list.</returns>
        public List<Book> GetBooksWithdrawalWithinPeriod(int days, Reader reader, DateTime dateTime)
        {
            return this.libraryContext.BookWithdrawals.Include(bw => bw.RentedBooks)
                .Include(bw => bw.RentedBooks.Select(bb => bb.Book)).Where(
                    bw => bw.Reader.Id.Equals(reader.Id) && (DbFunctions.DiffDays(bw.Date, dateTime) < days))
                .SelectMany(bw => bw.RentedBooks).Include(bb => bb.Book).Select(bb => bb.Book).ToList();
        }

        /// <summary>
        /// Add a new extension.
        /// </summary>
        /// <param name="reader">The reader.</param>
        /// <param name="bookWithdrawal">The book withdrawl.</param>
        /// <returns>if extension was added.</returns>
        public bool AddExtension(Reader reader, BookWithdrawal bookWithdrawal)
        {
            bookWithdrawal.Extensions.Add(new Extension { Date = DateTime.Now, Reader = reader });
            return this.libraryContext.SaveChanges() != 0;
        }

        /// <summary>
        /// Create a new reader.
        /// </summary>
        /// <param name="reader">The new reader.</param>
        /// <returns>If reader was added.</returns>
        public bool AddReader(Reader reader)
        {
            this.libraryContext.Readers.Add(reader);
            var successful = this.libraryContext.SaveChanges() != 0;
            if (successful)
            {
                LoggerUtil.LogInfo($"Reader added successfully : {reader.FirstName} {reader.LastName}", MethodBase.GetCurrentMethod());
            }
            else
            {
                LoggerUtil.LogError($"Reader failed to add to db : {reader.FirstName} {reader.LastName}", MethodBase.GetCurrentMethod());
            }

            return successful;
        }

        /// <summary>
        /// Get librarian by reader.
        /// </summary>
        /// <param name="reader">The reader.</param>
        /// <returns>An librarian.</returns>
        public Librarian GetLibrarianFromReader(Reader reader)
        {
            return this.libraryContext.Librarians.FirstOrDefault(e => e.Email.Equals(reader.Email));
        }

        /// <summary>
        /// Check if reader is librarian.
        /// </summary>
        /// <param name="reader">The reader.</param>
        /// <returns>If is librarian.</returns>
        public bool IsLibrarian(Reader reader)
        {
            return this.GetLibrarianFromReader(reader) != null;
        }

        /// <summary>
        /// Get same librarian.
        /// </summary>
        /// <param name="librarian">The librarian.</param>
        /// <returns>An librarian.</returns>
        public Librarian GetLibrarian(Librarian librarian)
        {
            var librarianFromDefault = this.libraryContext.Librarians.FirstOrDefault(e => e.Email.Equals(librarian.Email));
            if (librarianFromDefault == null)
            {
                LoggerUtil.LogInfo($"Librarian not found in db with name : {librarian.FirstName} {librarian.LastName}", MethodBase.GetCurrentMethod());
            }

            return librarian;
        }
    }
}