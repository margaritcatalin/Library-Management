using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PublicLibrary.BusinessLayer;
using PublicLibrary.Data_Mapper;
using PublicLibrary.Domain_Model;

namespace PublicLibrary
{
    class Program
    {
        static void Main(string[] args)
        {
/*
            LibraryDb libraryDb = new LibraryDb();
            BookRepository bookRepository = new BookRepository(libraryDb);
            EmployeeRepository employeeRepository = new EmployeeRepository(libraryDb);
            ReaderRepository readerRepository = new ReaderRepository(libraryDb);
            CategoriesService categoriesService = new CategoriesService();
            BookService bookService = new BookService(bookRepository, categoriesService,readerRepository);
            ReaderService readerService = new ReaderService(readerRepository);
            EmployeeService employeeService = new EmployeeService(employeeRepository);
            Category parentCategory = new Category { Name = "Manuale" };

            BookStock bookStock = new BookStock
            {
                Amount = 100,
                LectureRoomAmount = 10
            };
            BookStock bookStock2 = new BookStock
            {
                Amount = 100,
                LectureRoomAmount = 10
            };
            var edition = new Edition
            {
                Name = "Teora",
                BookType = "Hardcover",
                Pages = 256,
                BookStock = bookStock
            };
            var edition2 = new Edition
            {
                Name = "All",
                BookType = "Hardcover",
                Pages = 256,
                BookStock = bookStock2
            };

            Book book = new Book
            {
                Name = "Moara cu noroc",
                Authors = new List<Author>
                {
                    new Author
                    {
                        Name = "Ioan Slavici"
                    }
                },
                Editions = new List<Edition>
                {
                    edition
                },
                Categories = new List<Category>
                {
                    new Category
                    {
                        Name = "Carti scolare",
                        ParentCategory = parentCategory
                    }
                }
            };

            Book book2 = new Book
            {
                Name = "Padurea spanzuratilor",
                Authors = new List<Author>
                {
                    new Author
                    {
                        Name = "Lucian Blaga"
                    }
                },
                Editions = new List<Edition>
                {
                    edition2
                },
                Categories = new List<Category>
                {
                    new Category
                    {
                        Name = "",
                        ParentCategory = parentCategory
                    }
                }
            };

            var reader = new Reader
            {
                FirstName = "Mama",
                LastName = "ta",
                Email = "Mathaiimprejur"
            };
            BorrowedBook borrowedBook = new BorrowedBook
            {
                Book = book,
                Edition = edition,
                BookWithdrawal = new BookWithdrawal
                {
                    Date = DateTime.Now,
                    Reader = reader
                }
            };


            readerService.AddReader(reader);
            /*libraryDb.BorrowedBooks.Add(borrowedBook);#1#

            bookService.CreateBook(book);
            bookService.CreateBook(book2);
            var canBorrowBook  =  bookService.CanBorrowBook(book2.Name, edition2.Name);
            var employee = new Employee()
            {
                Email = "olaf.gigel@yahoo.com",
                FirstName = "Olaf",
                LastName = "Gigel"
            };
            employeeService.AddEmployee(employee);
            bookService.BorrowBooks(new List<Tuple<string, string>>
            {
                new Tuple<string, string>(book.Name,edition.Name),
                new Tuple<string, string>(book2.Name,edition2.Name),
            }, reader,employee);

            List<Book> bookList = new List<Book>
            {
                book,book2
            };
            readerService.CanBorrowBooks(bookList, reader, employee);
            Console.ReadLine();
*/

        }
    }
}