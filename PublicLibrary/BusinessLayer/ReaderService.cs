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
    using Castle.Core.Internal;
    using LibraryManagement.Data_Mapper;
    using LibraryManagement.Domain_Model;

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
                LoggerUtil.LogInfo($"Params email and phone is required.");
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
                LoggerUtil.LogInfo($"Your reader is invalid. Reader is null.");
                return false;
            }

            if (reader.FirstName.IsNullOrEmpty())
            {
                LoggerUtil.LogInfo($"Your reader is invalid. Reader firstName is null or empty.");
                return false;
            }

            if ((reader.FirstName.Length < 3) || (reader.FirstName.Length > 80))
            {
                LoggerUtil.LogInfo($"Your reader is invalid. Reader firstName has invalid length.");
                return false;
            }

            if (reader.FirstName.Any(c => !(char.IsLetter(c) || char.IsWhiteSpace(c))))
            {
                LoggerUtil.LogInfo($"Your reader is invalid. Reader firstName is invalid.");
                return false;
            }

            if (char.IsLower(reader.FirstName.First()))
            {
                LoggerUtil.LogInfo($"Your reader is invalid. Reader firstName is need to start with uppercase.");
                return false;
            }

            if (reader.LastName.IsNullOrEmpty())
            {
                LoggerUtil.LogInfo($"Your reader is invalid. Reader lastNameIsNull or empty.");
                return false;
            }

            if ((reader.LastName.Length < 3) || (reader.LastName.Length > 80))
            {
                LoggerUtil.LogInfo($"Your reader is invalid. Reader lastName has a invalid length.");
                return false;
            }

            if (reader.LastName.Any(c => !(char.IsLetter(c) || char.IsWhiteSpace(c))))
            {
                LoggerUtil.LogInfo($"Your reader is invalid. Reader lastName is invalid.");
                return false;
            }

            if (char.IsLower(reader.LastName.First()))
            {
                LoggerUtil.LogInfo($"Your reader is invalid. Reader lastName is need to start with upper case.");
                return false;
            }

            if (reader.Address.IsNullOrEmpty())
            {
                LoggerUtil.LogInfo($"Your reader is invalid. Reader address is null or empty.");
                return false;
            }

            if ((reader.Address.Length < 3) || (reader.Address.Length > 120))
            {
                LoggerUtil.LogInfo($"Your reader is invalid. Reader address has invalid length.");
                return false;
            }

            if (reader.Address.Any(c => !(char.IsLetterOrDigit(c) || char.IsWhiteSpace(c) || (c == '.') || (c == ','))))
            {
                LoggerUtil.LogInfo($"Your reader is invalid. Reader address is invalid.");
                return false;
            }

            var hasEmailOrPhone = false;
            hasEmailOrPhone = !(reader.Email.IsNullOrEmpty() && reader.Phone.IsNullOrEmpty());
            if (!hasEmailOrPhone)
            {
                LoggerUtil.LogInfo($"Your reader is invalid. Reader email or phone is null or empty.");
                return false;
            }

            if (!reader.Email.IsNullOrEmpty())
            {
                if ((reader.Email.Length < 10) || (reader.Email.Length > 150))
                {
                    LoggerUtil.LogInfo($"Your reader is invalid. Reader email has invalid length.");
                    return false;
                }

                if (reader.Email.Any(
                    c => !(char.IsLetterOrDigit(c) || (c == '@') || (c == '.') || (c == '_') || (c == '-'))))
                {
                    LoggerUtil.LogInfo($"Your reader is invalid. Reader email is invalid.");
                    return false;
                }

                if (reader.Email.All(c => c != '@'))
                {
                    LoggerUtil.LogInfo($"Your reader is invalid. Reader email is invalid.");
                    return false;
                }
            }

            if (!reader.Phone.IsNullOrEmpty())
            {
                if (reader.Phone.Length != 10)
                {
                    LoggerUtil.LogInfo($"Your reader is invalid. Reader phone number has invalid length.");
                    return false;
                }

                if (reader.Phone.Any(c => !char.IsDigit(c)))
                {
                    LoggerUtil.LogInfo($"Your reader is invalid. Reader phone number is invalid.");
                    return false;
                }
            }

            if (reader.Extensions == null)
            {
                LoggerUtil.LogInfo($"Your reader is invalid. Reader extensions is null.");
                return false;
            }

            if (reader.Gender.IsNullOrEmpty())
            {
                LoggerUtil.LogInfo($"Your reader is invalid. Reader gender is null or empty.");
                return false;
            }

            if (!(reader.Gender.Equals("M") || reader.Gender.Equals("F")))
            {
                LoggerUtil.LogInfo($"Your reader is invalid. Reader gender is invalid.");
                return false;
            }

            return this.readerRepository.AddReader(reader);
        }

        /// <summary>
        /// Check if can borrow the book.
        /// </summary>
        /// <param name="books">The books.</param>
        /// <param name="reader">The reader.</param>
        /// <param name="employee">The employee.</param>
        /// <param name="dateOfBorrowing">Date of borrowing.</param>
        /// <returns>If reader can borrow the books.</returns>
        public bool CanBorrowBooks(List<Book> books, Reader reader, Employee employee, DateTime dateOfBorrowing)
        {
            reader = this.readerRepository.GetReader(reader.Email, reader.Phone);
            var isEmployee = this.IsEmployee(reader);
            if (!this.CheckNumberOfBorrowedBooksInPeriod(books, reader, isEmployee, dateOfBorrowing))
            {
                LoggerUtil.LogInfo($"The reader borrowed too many books.");
                return false;
            }

            if (!this.CheckBooksDifferentCategories(books, isEmployee))
            {
                LoggerUtil.LogInfo($"The reader has books from too many categories.");
                return false;
            }

            if (!this.CheckBooksForSameCategories(books, reader, isEmployee, dateOfBorrowing))
            {
                LoggerUtil.LogInfo($"The reader has too many books from same categories.");
                return false;
            }

            if (!this.CheckSameBookDelta(books, reader, isEmployee, dateOfBorrowing))
            {
                LoggerUtil.LogInfo($"The reader borrowed too many books in same period.");
                return false;
            }

            if (!this.CheckNumberOfBooksPerDay(books, reader, isEmployee, dateOfBorrowing))
            {
                LoggerUtil.LogInfo($"The reader borrowed too many books in same day.");
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
                LoggerUtil.LogInfo($"Reader is null.");
                return false;
            }

            reader = this.readerRepository.GetReader(reader.Email, reader.Phone);
            if (!this.CheckNumberOfExtensions(reader, this.IsEmployee(reader), DateTime.Now))
            {
                LoggerUtil.LogInfo($"Reader has too many extensions.");
                return false;
            }

            return this.readerRepository.AddExtension(reader, bookWithdrawal);
        }

        /// <summary>
        /// Check if reader is employee.
        /// </summary>
        /// <param name="reader">The reader.</param>
        /// <returns>If he's eployee.</returns>
        public bool IsEmployee(Reader reader)
        {
            return this.readerRepository.IsEmployee(reader);
        }

        /// <summary>
        /// Get an employee by reader.
        /// </summary>
        /// <param name="reader">The reader.</param>
        /// <returns>An employee.</returns>
        public Employee GetEmployee(Reader reader)
        {
            return this.readerRepository.GetEmployeeFromReader(reader);
        }

        private bool CheckNumberOfBorrowedBooksInPeriod(
            List<Book> books,
            Reader reader,
            bool isEmployee,
            DateTime dateTime)
        {
            var nMC = int.Parse(ConfigurationManager.AppSettings["NMC"]);
            var pER = int.Parse(ConfigurationManager.AppSettings["PER"]);
            if (isEmployee)
            {
                nMC *= 2;
                pER /= 2;
            }

            var borrowedBooks = this.readerRepository.GetBooksWithdrawalWithinPeriod(pER, reader, dateTime);
            var numberOfBorrowedBooks = borrowedBooks.Count;

            if (numberOfBorrowedBooks + books.Count > nMC)
            {
                return false;
            }

            return true;
        }

        private bool CheckBooksDifferentCategories(List<Book> books, bool isEmployee)
        {
            var c = int.Parse(ConfigurationManager.AppSettings["C"]);
            if (isEmployee)
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
            bool isEmployee,
            DateTime dateTime)
        {
            var d = int.Parse(ConfigurationManager.AppSettings["D"]);
            var l = int.Parse(ConfigurationManager.AppSettings["L"]);
            if (isEmployee)
            {
                d *= 2;
            }

            var borrowedBooks =
                this.readerRepository.GetBooksWithdrawalWithinPeriod((int)(30.436875f * l), reader, dateTime);
            borrowedBooks = borrowedBooks.Union(books).ToList();

            var distinctCat = borrowedBooks.SelectMany(b => b.Categories).Select(c => this.GoToTopParent(c));
            var groupedCategories = distinctCat.GroupBy(dc => dc.Id);
            foreach (var groupedCategory in groupedCategories)
            {
                var numberOfBooks = groupedCategory.Count();
                if (numberOfBooks > d)
                {
                    return false;
                }
            }

            return true;
        }

        private bool CheckSameBookDelta(List<Book> books, Reader reader, bool isEmployee, DateTime dateTime)
        {
            var dELTA = int.Parse(ConfigurationManager.AppSettings["DELTA"]);
            if (isEmployee)
            {
                dELTA /= 2;
            }

            List<Book> borrowedBooks;
            borrowedBooks = this.readerRepository.GetBooksWithdrawalWithinPeriod(dELTA, reader, dateTime);
            foreach (var book in books)
            {
                if (borrowedBooks.FirstOrDefault(bb => bb.Name.Equals(book.Name)) != null)
                {
                    return false;
                }
            }

            return true;
        }

        private bool CheckNumberOfExtensions(Reader reader, bool isEmployee, DateTime dateTime)
        {
            var lIM = int.Parse(ConfigurationManager.AppSettings["LIM"]);
            if (isEmployee)
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

        private bool CheckNumberOfBooksPerDay(List<Book> books, Reader reader, bool isEmployee, DateTime dateTime)
        {
            var nCZ = int.Parse(ConfigurationManager.AppSettings["NCZ"]);
            if (isEmployee)
            {
                return this.CheckNumberOfLendedBooks(this.GetEmployee(reader), books.Count, dateTime);
            }

            List<Book> borrowedBooks;
            int numberOfBorrowedBooks;
            borrowedBooks = this.readerRepository.GetBooksWithdrawalWithinPeriod(1, reader, dateTime);
            numberOfBorrowedBooks = borrowedBooks.Count;

            if (numberOfBorrowedBooks + books.Count > nCZ)
            {
                return false;
            }

            return true;
        }

        private bool CheckNumberOfLendedBooks(Employee employee, int currentWithdrawBooks, DateTime dateTime)
        {
            var pERSIMP = int.Parse(ConfigurationManager.AppSettings["PERSIMP"]);
            var bookWithdrawalsForToday =
                employee.BookWithdrawals.Where(bw => DbFunctions.DiffDays(dateTime, bw.Date) < 1);
            var numberOfLendedBooks = bookWithdrawalsForToday.SelectMany(bw => bw.BorrowedBooks).Count();
            return numberOfLendedBooks + currentWithdrawBooks <= pERSIMP;
        }

        private Category GoToTopParent(Category category)
        {
            while (category.ParentCategory != null)
            {
                category = category.ParentCategory;
            }

            return category;
        }
    }
}