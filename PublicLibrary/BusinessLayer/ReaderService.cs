using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.Entity;
using System.Linq;
using Castle.Core.Internal;
using PublicLibrary.Data_Mapper;
using PublicLibrary.Domain_Model;

namespace PublicLibrary.BusinessLayer
{
    public class ReaderService
    {
        public ReaderService(ReaderRepository readerRepository)
        {
            this._readerRepository = readerRepository;
        }
        private ReaderRepository _readerRepository;

        public Reader GetReader(string email, string phone)
        {
            return _readerRepository.GetReader(email, phone);
        }

        public bool AddReader(Reader reader)
        {
            if (reader == null) return false;
            if (reader.FirstName.IsNullOrEmpty()) return false;
            if (reader.FirstName.Length < 3 || reader.FirstName.Length > 80) return false;
            if (reader.FirstName.Any(c => !(char.IsLetter(c) || char.IsWhiteSpace(c)))) return false;
            if (char.IsLower(reader.FirstName.First())) return false;

            if (reader.LastName.IsNullOrEmpty()) return false;
            if (reader.LastName.Length < 3 || reader.LastName.Length > 80) return false;
            if (reader.LastName.Any(c => !(char.IsLetter(c) || char.IsWhiteSpace(c)))) return false;
            if (char.IsLower(reader.LastName.First())) return false;


            if (reader.Address.IsNullOrEmpty()) return false;
            if (reader.Address.Length < 3 || reader.Address.Length > 120) return false;
            if (reader.Address.Any(c => !(char.IsLetterOrDigit(c) || char.IsWhiteSpace(c) || c=='.'|| c==','))) return false;

            bool hasEmailOrPhone = false;
            hasEmailOrPhone = !(reader.Email.IsNullOrEmpty() && reader.Phone.IsNullOrEmpty());
            if (!hasEmailOrPhone) return false;
            if (!reader.Email.IsNullOrEmpty())
            {
                if (reader.Email.Length < 10 || reader.Email.Length > 150) return false;
                if (reader.Email.Any(c => !(char.IsLetterOrDigit(c) || c=='@' || c=='.'|| c=='_'|| c=='-' ))) return false;
                if (reader.Email.All(c => c != '@')) return false;
            }

            if (!reader.Phone.IsNullOrEmpty())
            {
                if (reader.Phone.Length != 10) return false;
                if (reader.Phone.Any(c => !(char.IsDigit(c)))) return false;
            }

            if (reader.Extensions == null) return false;

            return _readerRepository.AddReader(reader);
        }

        public bool CanBorrowBooks(List<Book> books,Reader reader,Employee employee,DateTime dateOfBorrowing)
        {
            reader = _readerRepository.GetReader(reader.Email, reader.Phone);
            bool isEmployee = IsEmployee(reader);
            if (!CheckNumberOfBorrowedBooksInPeriod(books, reader, isEmployee,dateOfBorrowing)) return false;
            if (!CheckBooksDifferentCategories(books, isEmployee)) return false;
            if (!CheckBooksForSameCategories(books,reader, isEmployee,dateOfBorrowing)) return false;
            if (!CheckSameBookDelta(books, reader, isEmployee, dateOfBorrowing)) return false;
            if (!CheckNumberOfBooksPerDay(books, reader, isEmployee, dateOfBorrowing)) return false;

            return true;
        }

        private bool CheckNumberOfBorrowedBooksInPeriod(List<Book> books, Reader reader,bool isEmployee,DateTime dateTime)
        {
            int NMC = int.Parse(ConfigurationManager.AppSettings["NMC"]);
            int PER = int.Parse(ConfigurationManager.AppSettings["PER"]);
            if (isEmployee)
            {
                NMC *= 2;
                PER /= 2;
            }
            var borrowedBooks = _readerRepository.GetBooksWithdrawalWithinPeriod(PER, reader, dateTime);
            var numberOfBorrowedBooks = borrowedBooks.Count;

            if (numberOfBorrowedBooks + books.Count > NMC) return false;
            return true;
        }

        private static bool CheckBooksDifferentCategories(List<Book> books,bool isEmployee)
        {
            int C = int.Parse(ConfigurationManager.AppSettings["C"]);
            if (isEmployee)
            {
                C *= 2;
            }
            if (books.Count >= C)
            {
                return false;
            }

            // Alba ca zapada ( A,B) , Matematici fundamentale (A,C,D) - > (A,B,A,C,D) -> (A,B,C,D) -> 4
            if (books.Count > 3)
            {
                var distinctCategories = books.SelectMany(b => b.Categories).Distinct().Count();
                if (distinctCategories < 2) return false;
            }

            return true;
        }

        private bool CheckBooksForSameCategories(IEnumerable<Book> books, Reader reader,bool isEmployee,DateTime dateTime)
        {

            int D = int.Parse(ConfigurationManager.AppSettings["D"]);
            int L = int.Parse(ConfigurationManager.AppSettings["L"]);
            if (isEmployee)
            {
                D *= 2;
            }


            var borrowedBooks = _readerRepository.GetBooksWithdrawalWithinPeriod((int)(30.436875f * L), reader,dateTime);
            borrowedBooks = borrowedBooks.Union(books).ToList();
            // A A A C D C F F G H G -> (A A A) ,(C C), (F F) (
            var distinctCat = borrowedBooks.SelectMany(b => b.Categories).Select(c => GoToTopParent(c));
            var groupedCategories = distinctCat.GroupBy(dc => dc.Id);
            foreach (var groupedCategory in groupedCategories)
            {
                var numberOfBooks = groupedCategory.Count();
                if (numberOfBooks > D)
                {
                    return false;
                }
            }

            return true;
        }

        private bool CheckSameBookDelta(List<Book> books, Reader reader, bool isEmployee,DateTime dateTime)
        {
            int DELTA = int.Parse(ConfigurationManager.AppSettings["DELTA"]);
            if (isEmployee) DELTA /= 2;
            List<Book> borrowedBooks;
            borrowedBooks = _readerRepository.GetBooksWithdrawalWithinPeriod(DELTA, reader, dateTime);
            foreach (var book in books)
            {
                if (borrowedBooks.FirstOrDefault(bb => bb.Name.Equals(book.Name)) != null)
                {
                    return false;
                }
            }

            return true;
        }

        private static bool CheckNumberOfExtensions(Reader reader,bool isEmployee,DateTime dateTime)
        {
            int LIM = int.Parse(ConfigurationManager.AppSettings["LIM"]);
            if (isEmployee) LIM *= 2;
            var numberOfExtensions = reader.Extensions.Count(e => (dateTime- e.Date).TotalDays < 90);

            if (numberOfExtensions > LIM) return false;
            return true;
        }

        private bool CheckNumberOfBooksPerDay(List<Book> books, Reader reader,bool isEmployee,DateTime dateTime)
        {
            int NCZ = int.Parse(ConfigurationManager.AppSettings["NCZ"]);
            if (isEmployee)
            {
                return CheckNumberOfLendedBooks(GetEmployee(reader), books.Count,dateTime);
            }
            List<Book> borrowedBooks;
            int numberOfBorrowedBooks;
            borrowedBooks = _readerRepository.GetBooksWithdrawalWithinPeriod(1, reader, dateTime);
            numberOfBorrowedBooks = borrowedBooks.Count;

            if (numberOfBorrowedBooks + books.Count > NCZ) return false;
            return true;
        }

        private bool CheckNumberOfLendedBooks(Employee employee, int currentWithdrawBooks,DateTime dateTime)
        {
            int PERSIMP = int.Parse(ConfigurationManager.AppSettings["PERSIMP"]);
            var bookWithdrawalsForToday = employee.BookWithdrawals.Where(bw => DbFunctions.DiffDays(dateTime, bw.Date)<1);
            var numberOfLendedBooks = bookWithdrawalsForToday.SelectMany(bw => bw.BorrowedBooks).Count();
            return numberOfLendedBooks + currentWithdrawBooks <= PERSIMP;
        }

        private Category GoToTopParent(Category category)
        {
            while (category.ParentCategory!=null)
            {
                category = category.ParentCategory;
            }
            return category;
        }

        public bool AddExtension(Reader reader,BookWithdrawal bookWithdrawal)
        {
            if (reader == null) return false;
            reader = _readerRepository.GetReader(reader.Email, reader.Phone);
            if (!CheckNumberOfExtensions(reader, IsEmployee(reader), DateTime.Now)) return false;


            return _readerRepository.AddExtension(reader,bookWithdrawal);
        }

        public bool IsEmployee(Reader reader)
        {
            return _readerRepository.IsEmployee(reader);
        }

        public Employee GetEmployee(Reader reader)
        {
            return _readerRepository.GetEmployeeFromReader(reader);
        }
    }
}