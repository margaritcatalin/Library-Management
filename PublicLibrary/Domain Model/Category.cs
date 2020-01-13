using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using PublicLibrary.Data_Mapper;

namespace PublicLibrary.Domain_Model
{
    public class Category
    {
        public int Id { get; set; }

        [Index(IsUnique = true)]
        [StringLength(450)]
        public string Name { get; set; }

        public virtual ICollection<Book> Books { get; set; }

        public Category ParentCategory { get; set; }
    }
}