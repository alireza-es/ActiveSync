using System.Globalization;
using System.IO;
using System.Xml;
using ActiveSync.Core.Constants;
using ActiveSync.Core.Helper;
using ActiveSync.SyncContract.Syncables;

namespace ActiveSync.Core.ApplicationData
{
    public class ContactAppData : AppData
    {
        public ContactAppData()
        {
            Contact = new SyncableContact();
        }
        public ContactAppData(SyncableContact contact)
        {
            Contact = contact;
        }
        public SyncableContact Contact { get; set; }
        public override XmlNode GetAsXmlNode(XmlDocument xmlDocument)
        {
            if (Contact == null)
                return null;

            var applicationDataNode = xmlDocument.CreateElement(AirSyncStrings.ApplicationData, Namespaces.AirSync);

            if (Contact.Anniversary.HasValue)
                applicationDataNode.AppendValueNode(ContactsStrings.Anniversary, Contact.Anniversary.Value.ToString("yyyy-MM-ddThh:mm:ss.fffZ", CultureInfo.InvariantCulture), Namespaces.PoomContacts);

            if (Contact.Birthday.HasValue)
                applicationDataNode.AppendValueNode(ContactsStrings.Birthday, Contact.Birthday.Value.ToString("yyyy-MM-ddThh:mm:ss.fffZ", CultureInfo.InvariantCulture), Namespaces.PoomContacts);

            if (Contact.WeightedRank.HasValue)
                applicationDataNode.AppendValueNode(ContactsStrings.WeightedRank, Contact.WeightedRank.Value.ToString(), Namespaces.PoomContacts);

            applicationDataNode.AppendValueNodeIfNotNull(ContactsStrings.Title, Contact.Title, Namespaces.PoomContacts);
            applicationDataNode.AppendValueNodeIfNotNull(ContactsStrings.FirstName, Contact.FirstName, Namespaces.PoomContacts);
            applicationDataNode.AppendValueNodeIfNotNull(ContactsStrings.MiddleName, Contact.MiddleName, Namespaces.PoomContacts);
            applicationDataNode.AppendValueNodeIfNotNull(ContactsStrings.LastName, Contact.LastName, Namespaces.PoomContacts);
            applicationDataNode.AppendValueNodeIfNotNull(ContactsStrings.FileAs, Contact.FileAs, Namespaces.PoomContacts);
            applicationDataNode.AppendValueNodeIfNotNull(ContactsStrings.Email1Address, Contact.Email1Address, Namespaces.PoomContacts);
            applicationDataNode.AppendValueNodeIfNotNull(ContactsStrings.Email2Address, Contact.Email2Address, Namespaces.PoomContacts);
            applicationDataNode.AppendValueNodeIfNotNull(ContactsStrings.Email3Address, Contact.Email3Address, Namespaces.PoomContacts);
            applicationDataNode.AppendValueNodeIfNotNull(ContactsStrings.MobilePhoneNumber, Contact.MobilePhoneNumber, Namespaces.PoomContacts);
            applicationDataNode.AppendValueNodeIfNotNull(ContactsStrings.HomeAddressCity, Contact.HomeAddressCity, Namespaces.PoomContacts);
            applicationDataNode.AppendValueNodeIfNotNull(ContactsStrings.HomeAddressCountry, Contact.HomeAddressCountry, Namespaces.PoomContacts);
            applicationDataNode.AppendValueNodeIfNotNull(ContactsStrings.HomeFaxNumber, Contact.HomeFaxNumber, Namespaces.PoomContacts);
            applicationDataNode.AppendValueNodeIfNotNull(ContactsStrings.HomePhoneNumber, Contact.HomePhoneNumber, Namespaces.PoomContacts);
            applicationDataNode.AppendValueNodeIfNotNull(ContactsStrings.Home2PhoneNumber, Contact.Home2PhoneNumber, Namespaces.PoomContacts);
            applicationDataNode.AppendValueNodeIfNotNull(ContactsStrings.HomeAddressPostalCode, Contact.HomeAddressPostalCode, Namespaces.PoomContacts);
            applicationDataNode.AppendValueNodeIfNotNull(ContactsStrings.HomeAddressState, Contact.HomeAddressState, Namespaces.PoomContacts);
            applicationDataNode.AppendValueNodeIfNotNull(ContactsStrings.HomeAddressStreet, Contact.HomeAddressStreet, Namespaces.PoomContacts);
            applicationDataNode.AppendValueNodeIfNotNull(ContactsStrings.AssistantName, Contact.AssistantName, Namespaces.PoomContacts);
            applicationDataNode.AppendValueNodeIfNotNull(ContactsStrings.AssistantPhoneNumber, Contact.AssistantPhoneNumber, Namespaces.PoomContacts);
            applicationDataNode.AppendValueNodeIfNotNull(ContactsStrings.Business2PhoneNumber, Contact.Business2PhoneNumber, Namespaces.PoomContacts);
            applicationDataNode.AppendValueNodeIfNotNull(ContactsStrings.BusinessAddressCity, Contact.BusinessAddressCity, Namespaces.PoomContacts);
            applicationDataNode.AppendValueNodeIfNotNull(ContactsStrings.BusinessPhoneNumber, Contact.BusinessPhoneNumber, Namespaces.PoomContacts);
            applicationDataNode.AppendValueNodeIfNotNull(ContactsStrings.Webpage, Contact.WebPage, Namespaces.PoomContacts);
            applicationDataNode.AppendValueNodeIfNotNull(ContactsStrings.BusinessAddressCountry, Contact.BusinessAddressCountry, Namespaces.PoomContacts);
            applicationDataNode.AppendValueNodeIfNotNull(ContactsStrings.Department, Contact.Department, Namespaces.PoomContacts);
            applicationDataNode.AppendValueNodeIfNotNull(ContactsStrings.BusinessFaxNumber, Contact.BusinessFaxNumber, Namespaces.PoomContacts);
            applicationDataNode.AppendValueNodeIfNotNull(ContactsStrings.Alias, Contact.Alias, Namespaces.PoomContacts);
            applicationDataNode.AppendValueNodeIfNotNull(ContactsStrings.Suffix, Contact.Suffix, Namespaces.PoomContacts);
            applicationDataNode.AppendValueNodeIfNotNull(ContactsStrings.CompanyName, Contact.CompanyName, Namespaces.PoomContacts);
            applicationDataNode.AppendValueNodeIfNotNull(ContactsStrings.OtherAddressCity, Contact.OtherAddressCity, Namespaces.PoomContacts);
            applicationDataNode.AppendValueNodeIfNotNull(ContactsStrings.OtherAddressCountry, Contact.OtherAddressCountry, Namespaces.PoomContacts);
            applicationDataNode.AppendValueNodeIfNotNull(ContactsStrings.CarPhoneNumber, Contact.CarPhoneNumber, Namespaces.PoomContacts);
            applicationDataNode.AppendValueNodeIfNotNull(ContactsStrings.OtherAddressPostalCode, Contact.OtherAddressPostalCode, Namespaces.PoomContacts);
            applicationDataNode.AppendValueNodeIfNotNull(ContactsStrings.OtherAddressState, Contact.OtherAddressState, Namespaces.PoomContacts);
            applicationDataNode.AppendValueNodeIfNotNull(ContactsStrings.OtherAddressStreet, Contact.OtherAddressStreet, Namespaces.PoomContacts);
            applicationDataNode.AppendValueNodeIfNotNull(ContactsStrings.PagerNumber, Contact.PagerNumber, Namespaces.PoomContacts);
            applicationDataNode.AppendValueNodeIfNotNull(ContactsStrings.BusinessAddressPostalCode, Contact.BusinessAddressPostalCode, Namespaces.PoomContacts);
            applicationDataNode.AppendValueNodeIfNotNull(ContactsStrings.Spouse, Contact.Spouse, Namespaces.PoomContacts);
            applicationDataNode.AppendValueNodeIfNotNull(ContactsStrings.BusinessAddressState, Contact.BusinessAddressState, Namespaces.PoomContacts);
            applicationDataNode.AppendValueNodeIfNotNull(ContactsStrings.BusinessAddressStreet, Contact.BusinessAddressStreet, Namespaces.PoomContacts);
            applicationDataNode.AppendValueNodeIfNotNull(ContactsStrings.JobTitle, Contact.JobTitle, Namespaces.PoomContacts);
            applicationDataNode.AppendValueNodeIfNotNull(ContactsStrings.YomiFirstName, Contact.YomiFirstName, Namespaces.PoomContacts);
            applicationDataNode.AppendValueNodeIfNotNull(ContactsStrings.YomiLastName, Contact.YomiLastName, Namespaces.PoomContacts);
            applicationDataNode.AppendValueNodeIfNotNull(ContactsStrings.YomiCompanyName, Contact.YomiCompanyName, Namespaces.PoomContacts);
            applicationDataNode.AppendValueNodeIfNotNull(ContactsStrings.OfficeLocation, Contact.OfficeLocation, Namespaces.PoomContacts);
            applicationDataNode.AppendValueNodeIfNotNull(ContactsStrings.RadioPhoneNumber, Contact.RadioPhoneNumber, Namespaces.PoomContacts);
            applicationDataNode.AppendValueNodeIfNotNull(ContactsStrings.Picture, Contact.Picture, Namespaces.PoomContacts);

            if (Contact.Categories != null && Contact.Categories.Count > 0)
            {
                var categoriesNode = applicationDataNode.AppendContainerNode(ContactsStrings.Categories, Namespaces.PoomContacts);
                foreach (var category in Contact.Categories)
                    categoriesNode.AppendValueNode(ContactsStrings.Category, category, Namespaces.PoomContacts);
            }

            if (Contact.Children != null && Contact.Children.Count > 0)
            {
                var childrenNode = applicationDataNode.AppendContainerNode(ContactsStrings.Children,
                    Namespaces.PoomContacts);
                foreach (var child in Contact.Children)
                    childrenNode.AppendValueNode(ContactsStrings.Child, child, Namespaces.PoomContacts);

            }
            applicationDataNode.AppendValueNodeIfNotNull(Contacts2Strings.CustomerId, Contact.CustomerId, Namespaces.Contacts2);
            applicationDataNode.AppendValueNodeIfNotNull(Contacts2Strings.GovernmentId, Contact.GovernmentId, Namespaces.Contacts2);
            applicationDataNode.AppendValueNodeIfNotNull(Contacts2Strings.IMAddress, Contact.IMAddress, Namespaces.Contacts2);
            applicationDataNode.AppendValueNodeIfNotNull(Contacts2Strings.IMAddress2, Contact.IMAddress2, Namespaces.Contacts2);
            applicationDataNode.AppendValueNodeIfNotNull(Contacts2Strings.IMAddress3, Contact.IMAddress3, Namespaces.Contacts2);
            applicationDataNode.AppendValueNodeIfNotNull(Contacts2Strings.ManagerName, Contact.ManagerName, Namespaces.Contacts2);
            applicationDataNode.AppendValueNodeIfNotNull(Contacts2Strings.CompanyMainPhone, Contact.CompanyMainPhone, Namespaces.Contacts2);
            applicationDataNode.AppendValueNodeIfNotNull(Contacts2Strings.AccountName, Contact.AccountName, Namespaces.Contacts2);
            applicationDataNode.AppendValueNodeIfNotNull(Contacts2Strings.NickName, Contact.NickName, Namespaces.Contacts2);
            applicationDataNode.AppendValueNodeIfNotNull(Contacts2Strings.IMAddress2, Contact.IMAddress2, Namespaces.Contacts2);
            applicationDataNode.AppendValueNodeIfNotNull(Contacts2Strings.MMS, Contact.MMS, Namespaces.Contacts2);

            var bodyNode = applicationDataNode.AppendContainerNode(AirSyncBaseStrings.Body, Namespaces.AirSyncBase);
            bodyNode.AppendValueNode(AirSyncBaseStrings.Type, "3", Namespaces.AirSyncBase);
            bodyNode.AppendValueNode(AirSyncBaseStrings.EstimatedDataSize, "5500", Namespaces.AirSyncBase);
            bodyNode.AppendValueNode(AirSyncBaseStrings.Truncated, "1", Namespaces.AirSyncBase);

            applicationDataNode.AppendValueNode(AirSyncBaseStrings.NativeBodyType, "3", Namespaces.AirSyncBase);

            return applicationDataNode;

        }

        protected override string[] GetHashKeys()
        {
            return new string[]
            {
                Contact.FirstName,
                Contact.LastName,
                Contact.Email1Address,
                Contact.CompanyName,
                Contact.BusinessPhoneNumber,
                Contact.MobilePhoneNumber
            };
        }

        private string GetXmlAsString(XmlDocument xDoc)
        {
            var sw = new StringWriter();
            var xmlw = new XmlTextWriter(sw) { Formatting = Formatting.Indented };
            xDoc.WriteTo(xmlw);
            xmlw.Flush();

            return sw.ToString();
        }

    }
}
