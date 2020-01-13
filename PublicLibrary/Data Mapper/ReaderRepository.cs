using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using PublicLibrary.Domain_Model;

namespace PublicLibrary.Data_Mapper
{
    public class ReaderRepository
    {
        private readonly LibraryDb _libraryDb;

        public ReaderRepository(LibraryDb libraryDb)
        {
            _libraryDb = libraryDb;
        }

        public Reader GetReader(string email,string phone)
        {
            if (!string.IsNullOrEmpty(email))
            {
                return _libraryDb.Readers.FirstOrDefault(r => r.Email.Equals(email));
            }
            if (!string.IsNullOrEmpty(phone))
            {
                return _libraryDb.Readers.FirstOrDefault(r => r.Phone.Equals(phone));
            }
            LoggerUtil.LogInfo($"Reader not found in db with email or phone : {email} {phone}");
            return null;
        }

        public List<Book> GetBooksWithdrawalWithinPeriod(int days, Reader reader, DateTime dateTime)
        {

            return _libraryDb.BookWithdrawals.Include(bw => bw.BorrowedBooks)
                .Include(bw => bw.BorrowedBooks.Select(bb => bb.Book)).Where(bw =>
                    bw.Reader.Id.Equals(reader.Id) &&
                    DbFunctions.DiffDays(bw.Date,
                        dateTime) <
                    days)
                .SelectMany(bw => bw.BorrowedBooks).Include(bb => bb.Book)
                .Select(bb => bb.Book)
                .ToList();


        }

        public bool AddExtension(Reader reader,BookWithdrawal bookWithdrawal)
        {
            bookWithdrawal.Extensions.Add(new Extension { Date = DateTime.Now,Reader = reader});
            return _libraryDb.SaveChanges()!=0;
        }

        public bool AddReader(Reader reader)
        {
            _libraryDb.Readers.Add(reader);
            var successful = _libraryDb.SaveChanges()!=0;
            if(successful)LoggerUtil.LogInfo($"Reader added successfully : {reader.FirstName} {reader.LastName}");
            else LoggerUtil.LogError($"Reader failed to add to db : {reader.FirstName} {reader.LastName}");
            return successful;
        }

        public Employee GetEmployeeFromReader(Reader reader)
        {
            return _libraryDb.Employees.FirstOrDefault(e => e.Email.Equals(reader.Email));
        }
        public bool IsEmployee(Reader reader)
        {
            return GetEmployeeFromReader(reader) != null;
        }

        public Employee GetEmployee(Employee employee)
        {
            var employeeFromDefault = _libraryDb.Employees.FirstOrDefault(e => e.Email.Equals(employee.Email));
            if(employeeFromDefault==null)LoggerUtil.LogInfo($"Employee not found in db with name : {employee.FirstName} {employee.LastName}");
            return employee;
        }
    }
}