using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PublicLibrary.Domain_Model
{
    public class Reader
    {
        public int Id { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string Address { get; set; }

        [Index(IsUnique = true)]
        [StringLength(450)]
        public string Phone { get; set; }
        [Index(IsUnique = true)]
        [StringLength(450)]
        public string Email { get; set; }

        public virtual ICollection<BookWithdrawal> BookWithdrawals { get; set; }

        public virtual ICollection<Extension> Extensions { get; set; }
    }
}