// <copyright file="ReaderService.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace PublicLibrary.BusinessLayer
{
    using System;
    using System.Collections.Generic;
    using System.Configuration;
    using System.Data.Entity;
    using System.Linq;
    using Castle.Core.Internal;
    using PublicLibrary.Data_Mapper;
    using PublicLibrary.Domain_Model;

    /// <summary>
    /// The reader service.
    /// </summary>
    public class ReaderService
    {
        private ReaderRepository readerRepository;

        /// <summary>
        /// Initializes a new instance of the <see cref="ReaderService"/> class.
        /// </summary>
        /// <param name="readerRepository">The reader repository.</param>
        public ReaderService(ReaderRepository readerRepository)
        {
            this.readerRepository = readerRepository;
        }

        /// <summary>
        /// Get Reader by email and phone number.
        /// </summary>
        /// <param name="email">The email.</param>
        /// <param name="phone">The phone number.</param>
        /// <returns>The reader.</returns>
        public Reader GetReader(string email, string phone)
        {
            return this.readerRepository.GetReader(email, phone);
        }

        /// <summary>
        /// Create a new reader.
        /// </summary>
        /// <param name="reader">The new reader.</param>
        /// <returns>If reader was created.</returns>
        public bool AddReader(Reader reader)
        {
            if (reader == null)
            {
                LoggerUtil.LogInfo($"The reader is null.");
                return false;
            }

            if (reader.FirstName.IsNullOrEmpty())
            {
                LoggerUtil.LogInfo($"The reader first name is null or empty.");
                return false;
            }

            if (reader.FirstName.Length < 3 || reader.FirstName.Length > 80)
            {
                LoggerUtil.LogInfo($"The reader first name is invalid.");
                return false;
            }

            if (reader.FirstName.Any(c => !(char.IsLetter(c) || char.IsWhiteSpace(c))))
            {
                LoggerUtil.LogInfo($"The reader first name is invalid.");
                return false;
            }

            if (char.IsLower(reader.FirstName.First()))
            {
                LoggerUtil.LogInfo($"The reader first name was started with lower case.");
                return false;
            }

            if (reader.LastName.IsNullOrEmpty())
            {
                LoggerUtil.LogInfo($"The reader last name is null or empty.");
                return false;
            }

            if (reader.LastName.Length < 3 || reader.LastName.Length > 80)
            {
                LoggerUtil.LogInfo($"The reader last name is invalid.");
                return false;
            }

            if (reader.LastName.Any(c => !(char.IsLetter(c) || char.IsWhiteSpace(c))))
            {
                LoggerUtil.LogInfo($"The reader last name is invalid.");
                return false;
            }

            if (char.IsLower(reader.LastName.First()))
            {
                LoggerUtil.LogInfo($"The reader last name was started with lower case.");
                return false;
            }

            if (reader.Address.IsNullOrEmpty())
            {
                LoggerUtil.LogInfo($"The reader address is null or empty.");
                return false;
            }

            if (reader.Address.Length < 3 || reader.Address.Length > 120)
            {
                LoggerUtil.LogInfo($"The reader address is invalid.");
                return false;
            }

            if (reader.Address.Any(c => !(char.IsLetterOrDigit(c) || char.IsWhiteSpace(c) || c == '.' || c == ',')))
            {
                LoggerUtil.LogInfo($"The reader address is invalid.");
                return false;
            }

            bool hasEmailOrPhone = false;
            hasEmailOrPhone = !(reader.Email.IsNullOrEmpty() && reader.Phone.IsNullOrEmpty());
            if (!hasEmailOrPhone)
            {
                LoggerUtil.LogInfo($"The reader doesn't have email or phone number.");
                return false;
            }

            if (!reader.Email.IsNullOrEmpty())
            {
                if (reader.Email.Length < 10 || reader.Email.Length > 150)
                {
                    LoggerUtil.LogInfo($"The reader email is invalid.");
                    return false;
                }

                if (reader.Email.Any(c => !(char.IsLetterOrDigit(c) || c == '@' || c == '.' || c == '_' || c == '-' )))
                {
                    LoggerUtil.LogInfo($"The reader email is invalid.");
                    return false;
                }

                if (reader.Email.All(c => c != '@'))
                {
                    LoggerUtil.LogInfo($"The reader email is invalid.");
                    return false;
                }
            }

            if (!reader.Phone.IsNullOrEmpty())
            {
                if (reader.Phone.Length != 10)
                {
                    LoggerUtil.LogInfo($"The reader phone number is invalid.");
                    return false;
                }

                if (reader.Phone.Any(c => !char.IsDigit(c)))
                {
                    LoggerUtil.LogInfo($"The reader phone number is invalid.");
                    return false;
                }
            }

            if (reader.Extensions == null)
            {
                LoggerUtil.LogInfo($"The reader extensions is null.");
                return false;
            }

            return this.readerRepository.AddReader(reader);
        }

        /// <summary>
        /// Check if can borrow books.
        /// </summary>
        /// <param name="books">The books.</param>
        /// <param name="reader">The reader.</param>
        /// <param name="employee">The employee.</param>
        /// <param name="dateOfBorrowing">The date of borrowing.</param>
        /// <returns>If reader can borrow books.</returns>
        public bool CanBorrowBooks(List<Book> books, Reader reader, Employee employee, DateTime dateOfBorrowing)
        {
            reader = this.readerRepository.GetReader(reader.Email, reader.Phone);
            bool isEmployee = this.IsEmployee(reader);
            if (!this.CheckNumberOfBorrowedBooksInPeriod(books, reader, isEmployee, dateOfBorrowing))
            {
                LoggerUtil.LogInfo($"The reader borrowed to many books.");
                return false;
            }

            if (!this.CheckBooksDifferentCategories(books, isEmployee))
            {
                LoggerUtil.LogInfo($"The reader tried to borrow a book from different categories.");
                return false;
            }

            if (!this.CheckBooksForSameCategories(books, reader, isEmployee, dateOfBorrowing))
            {
                LoggerUtil.LogInfo($"The reader tried to borrow a book from same category.");
                return false;
            }

            if (!this.CheckSameBookDelta(books, reader, isEmployee, dateOfBorrowing))
            {
                LoggerUtil.LogInfo($"The reader tried to borrow a same book.");
                return false;
            }

            if (!this.CheckNumberOfBooksPerDay(books, reader, isEmployee, dateOfBorrowing))
            {
                LoggerUtil.LogInfo($"The reader has to many books borrowed.");
                return false;
            }

            return true;
        }

        /// <summary>
        ///  Add a new extension.
        /// </summary>
        /// <param name="reader">The reader.</param>
        /// <param name="bookWithdrawal">The book wihdrawl.</param>
        /// <returns>If extension was added.</returns>
        public bool AddExtension(Reader reader, BookWithdrawal bookWithdrawal)
        {
            if (reader == null)
            {
                return false;
            }

            reader = this.readerRepository.GetReader(reader.Email, reader.Phone);
            if (!this.CheckNumberOfExtensions(reader, this.IsEmployee(reader), DateTime.Now))
            {
                return false;
            }

            return this.readerRepository.AddExtension(reader,bookWithdrawal);
        }

        /// <summary>
        /// Check if Reader is employee.
        /// </summary>
        /// <param name="reader">The reader.</param>
        /// <returns>If is employee.</returns>
        public bool IsEmployee(Reader reader)
        {
            return this.readerRepository.IsEmployee(reader);
        }

        /// <summary>
        /// Get employee by reader.
        /// </summary>
        /// <param name="reader">The reader.</param>
        /// <returns>The employee.</returns>
        public Employee GetEmployee(Reader reader)
        {
            return this.readerRepository.GetEmployeeFromReader(reader);
        }

        private Category GoToTopParent(Category category)
        {
            while (category.ParentCategory != null)
            {
                category = category.ParentCategory;
            }

            return category;
        }

        private bool CheckNumberOfExtensions(Reader reader, bool isEmployee, DateTime dateTime)
        {
            int lIM = int.Parse(ConfigurationManager.AppSettings["LIM"]);
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
            int nCZ = int.Parse(ConfigurationManager.AppSettings["NCZ"]);
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
            int pERSIMP = int.Parse(ConfigurationManager.AppSettings["PERSIMP"]);
            var bookWithdrawalsForToday = employee.BookWithdrawals.Where(bw => DbFunctions.DiffDays(dateTime, bw.Date) < 1);
            var numberOfLendedBooks = bookWithdrawalsForToday.SelectMany(bw => bw.BorrowedBooks).Count();
            return numberOfLendedBooks + currentWithdrawBooks <= pERSIMP;
        }
        private bool CheckBooksDifferentCategories(List<Book> books, bool isEmployee)
        {
            int c = int.Parse(ConfigurationManager.AppSettings["C"]);

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

        private bool CheckBooksForSameCategories(IEnumerable<Book> books, Reader reader, bool isEmployee, DateTime dateTime)
        {

            int d = int.Parse(ConfigurationManager.AppSettings["D"]);
            int l = int.Parse(ConfigurationManager.AppSettings["L"]);
            if (isEmployee)
            {
                d *= 2;
            }

            var borrowedBooks = this.readerRepository.GetBooksWithdrawalWithinPeriod((int)(30.436875f * l), reader, dateTime);
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
            int dELTA = int.Parse(ConfigurationManager.AppSettings["DELTA"]);
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

        private bool CheckNumberOfBorrowedBooksInPeriod(List<Book> books, Reader reader, bool isEmployee, DateTime dateTime)
        {
            int nMC = int.Parse(ConfigurationManager.AppSettings["NMC"]);
            int pER = int.Parse(ConfigurationManager.AppSettings["PER"]);
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
    }
}