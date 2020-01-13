using System.Data;
using System.ComponentModel.DataAnnotations.Schema;

namespace PublicLibrary.Domain_Model
{
    public class BookStock
    {
        public int Id { get; set; }

        public int Amount { get; set; }

        public int LectureRoomAmount { get; set; }

    }
}