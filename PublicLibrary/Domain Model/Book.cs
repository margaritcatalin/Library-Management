using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using NUnit.Framework;
using PublicLibrary.Domain_Model;

namespace PublicLibrary.Data_Mapper
{
    public class Book
    {
        public int Id { get; set; }

        [Index("Id",IsUnique = true)]
        [StringLength(450)]

        public string Name { get; set; }

        public virtual ICollection<Category> Categories { get; set; }

        public virtual ICollection<Edition> Editions { get; set; }

        public virtual ICollection<Author> Authors { get; set; }

        public virtual ICollection<BorrowedBook> BorrowedBooks { get; set; }

    }
}