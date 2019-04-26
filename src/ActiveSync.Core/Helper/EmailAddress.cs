namespace ActiveSync.Core.Helper
{
    public class EmailAddress
    {
        public string DisplayName { get; set; }

        public string Address { get; set; }

        public override string ToString()
        {
            return string.IsNullOrEmpty(DisplayName) ? Address : 
                string.Format("{0}<{1}>", DisplayName, Address);
        }
    }
}
