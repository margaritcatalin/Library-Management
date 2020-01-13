// <copyright file="ReaderRepository.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace PublicLibrary.Data_Mapper
{
    using System;
    using System.Collections.Generic;
    using System.Data.Entity;
    using System.Linq;
    using PublicLibrary.Domain_Model;

    /// <summary>
    /// The reader repository.
    /// </summary>
    public class ReaderRepository
    {
        private readonly LibraryDb libraryDb;

        /// <summary>
        /// Initializes a new instance of the <see cref="ReaderRepository"/> class.
        /// </summary>
        /// <param name="libraryDb">The database connection.</param>
        public ReaderRepository(LibraryDb libraryDb)
        {
            this.libraryDb = libraryDb;
        }

        /// <summary>
        /// Get reader by email and phone.
        /// </summary>
        /// <param name="email">The email.</param>
        /// <param name="phone">The phone.</param>
        /// <returns>A reader.</returns>
        public Reader GetReader(string email, string phone)
        {
            if (!string.IsNullOrEmpty(email))
            {
                return this.libraryDb.Readers.FirstOrDefault(r => r.Email.Equals(email));
            }

            if (!string.IsNullOrEmpty(phone))
            {
                return this.libraryDb.Readers.FirstOrDefault(r => r.Phone.Equals(phone));
            }

            LoggerUtil.LogInfo($"Reader not found in db with email or phone : {email} {phone}");
            return null;
        }

        /// <summary>
        /// Get books borrowed for a specific period.
        /// </summary>
        /// <param name="days">Number of days.</param>
        /// <param name="reader">The reader.</param>
        /// <param name="dateTime">The date time.</param>
        /// <returns>The books list.</returns>
        public List<Book> GetBooksWithdrawalWithinPeriod(int days, Reader reader, DateTime dateTime)
        {

            return this.libraryDb.BookWithdrawals.Include(bw => bw.BorrowedBooks)
                .Include(bw => bw.BorrowedBooks.Select(bb => bb.Book)).Where(bw =>
                    bw.Reader.Id.Equals(reader.Id) &&
                    DbFunctions.DiffDays(
                        bw.Date,
                        dateTime) <
                    days)
                .SelectMany(bw => bw.BorrowedBooks).Include(bb => bb.Book)
                .Select(bb => bb.Book)
                .ToList();


        }

        /// <summary>
        /// Add a new extensions.
        /// </summary>
        /// <param name="reader">The reader.</param>
        /// <param name="bookWithdrawal">The bookWihdrawl.</param>
        /// <returns>If extension was added.</returns>
        public bool AddExtension(Reader reader,BookWithdrawal bookWithdrawal)
        {
            bookWithdrawal.Extensions.Add(new Extension { Date = DateTime.Now,Reader = reader});
            return this.libraryDb.SaveChanges() != 0;
        }

        /// <summary>
        /// Add a new reader.
        /// </summary>
        /// <param name="reader">The reader.</param>
        /// <returns>If reader was added.</returns>
        public bool AddReader(Reader reader)
        {
            this.libraryDb.Readers.Add(reader);
            var successful = this.libraryDb.SaveChanges() != 0;
            if (successful)
            {
                LoggerUtil.LogInfo($"Reader added successfully : {reader.FirstName} {reader.LastName}");
            }
            else
            {
                LoggerUtil.LogError($"Reader failed to add to db : {reader.FirstName} {reader.LastName}");
            }

            return successful;
        }

        /// <summary>
        /// Get employee by reader.
        /// </summary>
        /// <param name="reader">The reader.</param>
        /// <returns>A emoloyee.</returns>
        public Employee GetEmployeeFromReader(Reader reader)
        {
            return this.libraryDb.Employees.FirstOrDefault(e => e.Email.Equals(reader.Email));
        }

        /// <summary>
        /// Check if is employee.
        /// </summary>
        /// <param name="reader">The reader.</param>
        /// <returns>If reader is employee.</returns>
        public bool IsEmployee(Reader reader)
        {
            return this.GetEmployeeFromReader(reader) != null;
        }

        /// <summary>
        /// Get an employee.
        /// </summary>
        /// <param name="employee">The employee.</param>
        /// <returns>An employee.</returns>
        public Employee GetEmployee(Employee employee)
        {
            var employeeFromDefault = this.libraryDb.Employees.FirstOrDefault(e => e.Email.Equals(employee.Email));
            if (employeeFromDefault == null)
            {
                LoggerUtil.LogInfo($"Employee not found in db with name : {employee.FirstName} {employee.LastName}");
            }

            return employee;
        }
    }
}