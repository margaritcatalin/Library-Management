using System;

namespace PublicLibrary.Domain_Model
{
    public class Extension
    {
        public int Id { get; set; }

        public Reader Reader { get; set; }

        public DateTime Date { get; set; }
        public BookWithdrawal BookWithdrawal { get; set; }

    }
}