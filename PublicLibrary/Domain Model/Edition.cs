using System.Collections.Generic;
using NUnit.Framework.Constraints;
using PublicLibrary.Data_Mapper;

namespace PublicLibrary.Domain_Model
{
    public class Edition
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public int Pages { get; set; }

        public string BookType { get; set; }

        public Book Book { get; set; }

        public virtual ICollection<BorrowedBook> BorrowedBooks { get; set; }

        public BookStock BookStock { get; set; }

        public override string ToString()
        {
            return Name;
        }
    }
}