// <copyright file="ReaderService.cs" company="Transilvania University of Brasov">
// Margarit Marian Catalin
// </copyright>

namespace LibraryManagement.BusinessLayer
{
    using System;
    using System.Collections.Generic;
    using System.Configuration;
    using System.Data.Entity;
    using System.Linq;
    using System.Reflection;
    using Castle.Core.Internal;
    using LibraryManagement.DataMapper;
    using LibraryManagement.DomainModel;

    /// <summary>
    /// The reader service.
    /// </summary>
    public class ReaderService
    {
        private readonly ReaderRepository readerRepository;

        /// <summary>
        /// Initializes a new instance of the <see cref="ReaderService"/> class.
        /// </summary>
        /// <param name="readerRepository">The reader repository.</param>
        public ReaderService(ReaderRepository readerRepository)
        {
            this.readerRepository = readerRepository;
        }

        /// <summary>
        /// Get reader by email and phone.
        /// </summary>
        /// <param name="email">The email.</param>
        /// <param name="phone">The phone.</param>
        /// <returns>A reader.</returns>
        public Reader GetReader(string email, string phone)
        {
            if (email.IsNullOrEmpty() && phone.IsNullOrEmpty())
            {
                LoggerUtil.LogInfo($"Params email and phone is required.", MethodBase.GetCurrentMethod());
                return null;
            }

            return this.readerRepository.GetReader(email, phone);
        }

        /// <summary>
        /// Add a new reader.
        /// </summary>
        /// <param name="reader">The reader.</param>
        /// <returns>If reader was created.</returns>
        public bool AddReader(Reader reader)
        {
            if (reader == null)
            {
                LoggerUtil.LogInfo($"Your reader is invalid. Reader is null.", MethodBase.GetCurrentMethod());
                return false;
            }

            if (reader.FirstName.IsNullOrEmpty())
            {
                LoggerUtil.LogInfo($"Your reader is invalid. Reader firstName is null or empty.", MethodBase.GetCurrentMethod());
                return false;
            }

            if ((reader.FirstName.Length < 3) || (reader.FirstName.Length > 80))
            {
                LoggerUtil.LogInfo($"Your reader is invalid. Reader firstName has invalid length.", MethodBase.GetCurrentMethod());
                return false;
            }

            if (reader.FirstName.Any(c => !(char.IsLetter(c) || char.IsWhiteSpace(c))))
            {
                LoggerUtil.LogInfo($"Your reader is invalid. Reader firstName is invalid.", MethodBase.GetCurrentMethod());
                return false;
            }

            if (char.IsLower(reader.FirstName.First()))
            {
                LoggerUtil.LogInfo($"Your reader is invalid. Reader firstName is need to start with uppercase.", MethodBase.GetCurrentMethod());
                return false;
            }

            if (reader.LastName.IsNullOrEmpty())
            {
                LoggerUtil.LogInfo($"Your reader is invalid. Reader lastNameIsNull or empty.", MethodBase.GetCurrentMethod());
                return false;
            }

            if ((reader.LastName.Length < 3) || (reader.LastName.Length > 80))
            {
                LoggerUtil.LogInfo($"Your reader is invalid. Reader lastName has a invalid length.", MethodBase.GetCurrentMethod());
                return false;
            }

            if (reader.LastName.Any(c => !(char.IsLetter(c) || char.IsWhiteSpace(c))))
            {
                LoggerUtil.LogInfo($"Your reader is invalid. Reader lastName is invalid.", MethodBase.GetCurrentMethod());
                return false;
            }

            if (char.IsLower(reader.LastName.First()))
            {
                LoggerUtil.LogInfo($"Your reader is invalid. Reader lastName is need to start with upper case.", MethodBase.GetCurrentMethod());
                return false;
            }

            if (reader.Address.IsNullOrEmpty())
            {
                LoggerUtil.LogInfo($"Your reader is invalid. Reader address is null or empty.", MethodBase.GetCurrentMethod());
                return false;
            }

            if ((reader.Address.Length < 3) || (reader.Address.Length > 120))
            {
                LoggerUtil.LogInfo($"Your reader is invalid. Reader address has invalid length.", MethodBase.GetCurrentMethod());
                return false;
            }

            if (reader.Address.Any(c => !(char.IsLetterOrDigit(c) || char.IsWhiteSpace(c) || (c == '.') || (c == ','))))
            {
                LoggerUtil.LogInfo($"Your reader is invalid. Reader address is invalid.", MethodBase.GetCurrentMethod());
                return false;
            }

            var hasEmailOrPhone = false;
            hasEmailOrPhone = !(reader.Email.IsNullOrEmpty() && reader.Phone.IsNullOrEmpty());
            if (!hasEmailOrPhone)
            {
                LoggerUtil.LogInfo($"Your reader is invalid. Reader email or phone is null or empty.", MethodBase.GetCurrentMethod());
                return false;
            }

            if (!reader.Email.IsNullOrEmpty())
            {
                if ((reader.Email.Length < 10) || (reader.Email.Length > 150))
                {
                    LoggerUtil.LogInfo($"Your reader is invalid. Reader email has invalid length.", MethodBase.GetCurrentMethod());
                    return false;
                }

                if (reader.Email.Any(
                    c => !(char.IsLetterOrDigit(c) || (c == '@') || (c == '.') || (c == '_') || (c == '-'))))
                {
                    LoggerUtil.LogInfo($"Your reader is invalid. Reader email is invalid.", MethodBase.GetCurrentMethod());
                    return false;
                }

                if (reader.Email.All(c => c != '@'))
                {
                    LoggerUtil.LogInfo($"Your reader is invalid. Reader email is invalid.", MethodBase.GetCurrentMethod());
                    return false;
                }
            }

            if (!reader.Phone.IsNullOrEmpty())
            {
                if (reader.Phone.Length != 10)
                {
                    LoggerUtil.LogInfo($"Your reader is invalid. Reader phone number has invalid length.", MethodBase.GetCurrentMethod());
                    return false;
                }

                if (reader.Phone.Any(c => !char.IsDigit(c)))
                {
                    LoggerUtil.LogInfo($"Your reader is invalid. Reader phone number is invalid.", MethodBase.GetCurrentMethod());
                    return false;
                }
            }

            if (reader.Extensions == null)
            {
                LoggerUtil.LogInfo($"Your reader is invalid. Reader extensions is null.", MethodBase.GetCurrentMethod());
                return false;
            }

            if (reader.Gender.IsNullOrEmpty())
            {
                LoggerUtil.LogInfo($"Your reader is invalid. Reader gender is null or empty.", MethodBase.GetCurrentMethod());
                return false;
            }

            if (!(reader.Gender.Equals("M") || reader.Gender.Equals("F")))
            {
                LoggerUtil.LogInfo($"Your reader is invalid. Reader gender is invalid.", MethodBase.GetCurrentMethod());
                return false;
            }

            return this.readerRepository.AddReader(reader);
        }

        /// <summary>
        /// Check if can borrow the book.
        /// </summary>
        /// <param name="books">The books.</param>
        /// <param name="reader">The reader.</param>
        /// <param name="librarian">The librarian.</param>
        /// <param name="dateOfRent">Date of renting.</param>
        /// <returns>If reader can borrow the books.</returns>
        public bool CanRentBooks(List<Book> books, Reader reader, Librarian librarian, DateTime dateOfRent)
        {
            reader = this.readerRepository.GetReader(reader.Email, reader.Phone);
            var isLibrarian = this.IsLibrarian(reader);
            if (!this.CheckNumberOfRentedBooksInPeriod(books, reader, isLibrarian, dateOfRent))
            {
                LoggerUtil.LogInfo($"The reader rented too many books.", MethodBase.GetCurrentMethod());
                return false;
            }

            if (!this.CheckBooksDifferentCategories(books, isLibrarian))
            {
                LoggerUtil.LogInfo($"The reader has books from too many domains.", MethodBase.GetCurrentMethod());
                return false;
            }

            if (!this.CheckBooksForSameCategories(books, reader, isLibrarian, dateOfRent))
            {
                LoggerUtil.LogInfo($"The reader has too many books from same domains.", MethodBase.GetCurrentMethod());
                return false;
            }

            if (!this.CheckSameBookDelta(books, reader, isLibrarian, dateOfRent))
            {
                LoggerUtil.LogInfo($"The reader rented too many books in same period.", MethodBase.GetCurrentMethod());
                return false;
            }

            if (!this.CheckNumberOfBooksPerDay(books, reader, isLibrarian, dateOfRent))
            {
                LoggerUtil.LogInfo($"The reader rented too many books in same day.", MethodBase.GetCurrentMethod());
                return false;
            }

            return true;
        }

        /// <summary>
        /// Add a new extension.
        /// </summary>
        /// <param name="reader">The reader.</param>
        /// <param name="bookWithdrawal">The book withdrawl.</param>
        /// <returns>If extension was added.</returns>
        public bool AddExtension(Reader reader, BookWithdrawal bookWithdrawal)
        {
            if (reader == null)
            {
                LoggerUtil.LogInfo($"Reader is null.", MethodBase.GetCurrentMethod());
                return false;
            }

            reader = this.readerRepository.GetReader(reader.Email, reader.Phone);
            if (!this.CheckNumberOfExtensions(reader, this.IsLibrarian(reader), DateTime.Now))
            {
                LoggerUtil.LogInfo($"Reader has too many extensions.", MethodBase.GetCurrentMethod());
                return false;
            }

            return this.readerRepository.AddExtension(reader, bookWithdrawal);
        }

        /// <summary>
        /// Check if reader is librarian.
        /// </summary>
        /// <param name="reader">The reader.</param>
        /// <returns>If he's eployee.</returns>
        public bool IsLibrarian(Reader reader)
        {
            return this.readerRepository.IsLibrarian(reader);
        }

        /// <summary>
        /// Get an librarian by reader.
        /// </summary>
        /// <param name="reader">The reader.</param>
        /// <returns>An librarian.</returns>
        public Librarian GetLibrarian(Reader reader)
        {
            return this.readerRepository.GetLibrarianFromReader(reader);
        }

        private bool CheckNumberOfRentedBooksInPeriod(
            List<Book> books,
            Reader reader,
            bool isLibrarian,
            DateTime dateTime)
        {
            var nMC = int.Parse(ConfigurationManager.AppSettings["NMC"]);
            var pER = int.Parse(ConfigurationManager.AppSettings["PER"]);
            if (isLibrarian)
            {
                nMC *= 2;
                pER /= 2;
            }

            var rentedBooks = this.readerRepository.GetBooksWithdrawalWithinPeriod(pER, reader, dateTime);
            var numberOfRentedBooks = rentedBooks.Count;

            if (numberOfRentedBooks + books.Count > nMC)
            {
                return false;
            }

            return true;
        }

        private bool CheckBooksDifferentCategories(List<Book> books, bool isLibrarian)
        {
            var c = int.Parse(ConfigurationManager.AppSettings["C"]);
            if (isLibrarian)
            {
                c *= 2;
            }

            if (books.Count >= c)
            {
                return false;
            }

            if (books.Count > 3)
            {
                var distinctCategories = books.SelectMany(b => b.Categories).Distinct().Count();
                if (distinctCategories < 2)
                {
                    return false;
                }
            }

            return true;
        }

        private bool CheckBooksForSameCategories(
            IEnumerable<Book> books,
            Reader reader,
            bool isLibrarian,
            DateTime dateTime)
        {
            var d = int.Parse(ConfigurationManager.AppSettings["D"]);
            var l = int.Parse(ConfigurationManager.AppSettings["L"]);
            if (isLibrarian)
            {
                d *= 2;
            }

            var rentedBooks =
                this.readerRepository.GetBooksWithdrawalWithinPeriod((int)(30.436875f * l), reader, dateTime);
            rentedBooks = rentedBooks.Union(books).ToList();

            var distinctCat = rentedBooks.SelectMany(b => b.Categories).Select(c => this.GoToTopParent(c));
            var groupedCategories = distinctCat.GroupBy(dc => dc.Id);
            foreach (var groupedDomain in groupedCategories)
            {
                var numberOfBooks = groupedDomain.Count();
                if (numberOfBooks > d)
                {
                    return false;
                }
            }

            return true;
        }

        private bool CheckSameBookDelta(List<Book> books, Reader reader, bool isLibrarian, DateTime dateTime)
        {
            var dELTA = int.Parse(ConfigurationManager.AppSettings["DELTA"]);
            if (isLibrarian)
            {
                dELTA /= 2;
            }

            List<Book> rentedBooks;
            rentedBooks = this.readerRepository.GetBooksWithdrawalWithinPeriod(dELTA, reader, dateTime);
            foreach (var book in books)
            {
                if (rentedBooks.FirstOrDefault(bb => bb.Name.Equals(book.Name)) != null)
                {
                    return false;
                }
            }

            return true;
        }

        private bool CheckNumberOfExtensions(Reader reader, bool isLibrarian, DateTime dateTime)
        {
            var lIM = int.Parse(ConfigurationManager.AppSettings["LIM"]);
            if (isLibrarian)
            {
                lIM *= 2;
            }

            var numberOfExtensions = reader.Extensions.Count(e => (dateTime - e.Date).TotalDays < 90);

            if (numberOfExtensions > lIM)
            {
                return false;
            }

            return true;
        }

        private bool CheckNumberOfBooksPerDay(List<Book> books, Reader reader, bool isLibrarian, DateTime dateTime)
        {
            var nCZ = int.Parse(ConfigurationManager.AppSettings["NCZ"]);
            if (isLibrarian)
            {
                return this.CheckNumberOfLendedBooks(this.GetLibrarian(reader), books.Count, dateTime);
            }

            List<Book> rentedBooks;
            int numberOfRentedBooks;
            rentedBooks = this.readerRepository.GetBooksWithdrawalWithinPeriod(1, reader, dateTime);
            numberOfRentedBooks = rentedBooks.Count;

            if (numberOfRentedBooks + books.Count > nCZ)
            {
                return false;
            }

            return true;
        }

        private bool CheckNumberOfLendedBooks(Librarian librarian, int currentWithdrawBooks, DateTime dateTime)
        {
            var pERSIMP = int.Parse(ConfigurationManager.AppSettings["PERSIMP"]);
            var bookWithdrawalsForToday =
                librarian.BookWithdrawals.Where(bw => DbFunctions.DiffDays(dateTime, bw.Date) < 1);
            var numberOfLendedBooks = bookWithdrawalsForToday.SelectMany(bw => bw.RentedBooks).Count();
            return numberOfLendedBooks + currentWithdrawBooks <= pERSIMP;
        }

        private Domain GoToTopParent(Domain domain)
        {
            while (domain.ParentDomain != null)
            {
                domain = domain.ParentDomain;
            }

            return domain;
        }
    }
}