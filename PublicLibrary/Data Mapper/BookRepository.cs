using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using PublicLibrary.Domain_Model;

namespace PublicLibrary.Data_Mapper
{
    public class BookRepository
    {
        private readonly LibraryDb _libraryDb;

        public BookRepository(LibraryDb libraryDb)
        {
            _libraryDb = libraryDb;
        }

        public bool AddBook(Book book)
        {
            book.Authors = book.Authors.Select(a =>
            {
                var dba = _libraryDb.Authors.FirstOrDefault(dbA=> dbA.Name.Equals(a.Name));
                if (dba == null) return a;
                else return dba;
            }).ToList();

            book.Categories = book.Categories.Select(c =>
            {
                var dbc = _libraryDb.Categories.FirstOrDefault(dbC => dbC.Name.Equals(c.Name));
                if (dbc == null) return c;
                else return dbc;
            }).ToList();

            _libraryDb.Books.Add(book);
            return _libraryDb.SaveChanges() !=0;
        }

        public Book GetBook(string bookName)
        {
            var result = _libraryDb.Books.FirstOrDefault(b => b.Name.Equals(bookName));
            return result;
        }

        public Edition GetEdition(string bookName, string editionName)
        {
            var edition = _libraryDb.Editions.Include(e => e.BookStock)
                .Include(e => e.BorrowedBooks)
                .Include(e => e.BorrowedBooks.Select(bb => bb.BookWithdrawal))
                .First(e => e.Name.Equals(editionName) && e.Book.Name.Equals(bookName));

            return edition;
        }

        public List<Edition> GetEditions(List<Borrowing> editions)
        {
            return editions.Select(e => GetEdition(e.BookName, e.EditionName)).ToList();
        }


        public BookWithdrawal BorrowBooks(List<Borrowing> editionsList,Reader reader, Employee employee)
        {
            List<Edition> editions = GetEditions(editionsList);

            BookWithdrawal bookWithdrawal = new BookWithdrawal
            {
                BorrowedBooks = editions.Select(e=> new BorrowedBook
                {
                    Book = e.Book,
                    Edition = e

                }).ToList(),
                Date = DateTime.Now,
                Reader = reader,
                Employee = employee,
                Extensions = new List<Extension>()
            };

            _libraryDb.BookWithdrawals.Add(bookWithdrawal);
            if (_libraryDb.SaveChanges() == 0) return null;
            return bookWithdrawal;
        }

        public Book GetBook(Book bookToGet)
        {
            return _libraryDb.Books.FirstOrDefault(b => b.Name.Equals(bookToGet.Name));
        }

        public bool AddEdition(Book book, Edition edition)
        {
            book.Editions.Add(edition);
            return _libraryDb.SaveChanges() != 0;
        }
    }
}