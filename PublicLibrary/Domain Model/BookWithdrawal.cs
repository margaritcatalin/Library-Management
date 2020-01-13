using System;
using System.Collections.Generic;
using PublicLibrary.Data_Mapper;

namespace PublicLibrary.Domain_Model
{
    public class BookWithdrawal
    {
        public int Id { get; set; }

        public DateTime Date { get; set; }

        public Reader Reader { get; set; }

        public ICollection<BorrowedBook> BorrowedBooks { get; set; }
        public ICollection<Extension> Extensions { get; set; }

        public Employee Employee { get; set; }
    }
}