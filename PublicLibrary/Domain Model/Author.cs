using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PublicLibrary.Data_Mapper
{
    public class Author
    {
        public int Id { get; set; }

        [Index(IsUnique = true)]
        [StringLength(450)]
        public string Name { get; set; }

        public ICollection<Book> Books { get; set; }


        public override string ToString()
        {
            return Name;
        }
    }
}