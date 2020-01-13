using PublicLibrary.Data_Mapper;

namespace PublicLibrary.Domain_Model
{
    public class BorrowedBook
    {
        public int Id { get; set; }

        public Book Book { get; set; }

        public BookWithdrawal BookWithdrawal { get; set; }

        public Edition Edition { get; set; }
    }
}