using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using PublicLibrary.Domain_Model;

namespace PublicLibrary.Data_Mapper
{
    public class Employee
    {
        public int Id { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        [Index(IsUnique = true)]
        [StringLength(450)]
        public string Email { get; set; }

        public virtual ICollection<BookWithdrawal> BookWithdrawals { get; set; }
    }
}