namespace LibraryManagement.Util
{
    public class Role
    {
        private Role(string value)
        {
            Value = value;
        }

        public string Value { get; set; }

        public static Role Seller
        {
            get { return new Role("Seller"); }
        }

        public static Role Buyer
        {
            get { return new Role("Buyer"); }
        }
    }
}