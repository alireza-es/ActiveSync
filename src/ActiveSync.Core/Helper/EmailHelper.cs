using System.Collections.Generic;
using System.Linq;

namespace ActiveSync.Core.Helper
{
    public static class EmailHelper
    {
        public static List<EmailAddress> ToEmailAddress(string email)
        {
            var addressList = new List<EmailAddress>();
            if (string.IsNullOrEmpty(email))
                return addressList;

            var addresses = email.Split(new char[] { ',' });
            foreach (var address in addresses)
            {
                var emailFields = email.Split(new char[] { ' ', '\"', '"', '<', '>' });
                var mail = emailFields.FirstOrDefault(m => m.Contains("@"));
                var displayName = "";                
                if (email.StartsWith("<"))
                {
                    
                }
                else
                {

                    for (var row = 0; row < emailFields.Length - 3; row++)
                    {
                        if (!string.IsNullOrEmpty(emailFields[row]))
                            displayName = emailFields[row];
                    }
                }
                addressList.Add(
                    new EmailAddress
                    {
                        DisplayName = displayName,
                        Address = mail
                    });
            }
            
            return addressList;
        }
    }
}
