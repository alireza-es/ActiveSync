using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml;
using System.Xml.Linq;
using ActiveSync.Core.ApplicationData;
using ActiveSync.Core.Constants;

namespace ActiveSync.Core.Helper
{
    public static class XElementExtensionMethods
    {
        public static IList<XElement> GetDescendants(this XElement element, string elementName, XNamespace nameSpace = null)
        {
            var childNameSpace = nameSpace ?? element.Name.Namespace;

            return element.Descendants(childNameSpace + elementName).ToList();
        }
        public static XElement GetElement(this XElement element, string elementName, XNamespace nameSpace = null)
        {
            var childNameSpace = nameSpace ?? element.Name.Namespace;

            return element.Element(childNameSpace + elementName);
        }
        public static string GetElementValueAsString(this XElement element, string elementName,
            XNamespace nameSpace = null, string defaultValue = "")
        {
            var childNameSpace = nameSpace ?? element.Name.Namespace;

            var childElement = element.Element(childNameSpace + elementName);

            return childElement != null ? childElement.Value : string.Empty;
        }

        public static bool GetElementValueAsBoolean(this XElement element, string elementName,
            XNamespace nameSpace = null, bool defaultValue = false)
        {
            var value = GetElementValueAsInt(element, elementName, nameSpace);

            return Convert.ToBoolean(value);
        }

        public static bool HasChildElement(this XElement element, string elementName,
            XNamespace nameSpace = null, bool defaultValue = false)
        {
            var childNameSpace = nameSpace ?? element.Name.Namespace;

            var childElement = element.Element(childNameSpace + elementName);

            return childElement != null;

        }

        public static int GetElementValueAsInt(this XElement element, string elementName,
            XNamespace nameSpace = null, int defaultValue = 0)
        {
            var value = GetElementValueAsString(element, elementName, nameSpace);

            return value != string.Empty ? Convert.ToInt32(value) : defaultValue;
        }

        public static int? GetElementValueAsNullableInt(this XElement element, string elementName,
            XNamespace nameSpace = null)
        {
            var childElement = element.GetElement(elementName, nameSpace);
            if (childElement == null)
                return null;

            return GetElementValueAsInt(element, elementName, nameSpace);

        }

        public static void UpdateEmailAppData(this XElement element, EmailAppData appData, string elementName)
        {
            if (element.HasChildElement(EmailStrings.Read, Namespaces.Email))
                appData.Email.Read = GetElementValueAsBoolean(element, EmailStrings.Read, Namespaces.Email);
            if (element.HasChildElement(EmailStrings.Subject, Namespaces.Email))
                appData.Email.Subject = GetElementValueAsString(element, EmailStrings.Subject, Namespaces.Email);
            if (element.HasChildElement(EmailStrings.Body, Namespaces.Email))
                appData.Email.Body = GetElementValueAsString(element, EmailStrings.Body, Namespaces.Email);
        }

        public static void UpdateContactAppData(this XElement element, ContactAppData appData, string elementName)
        {
            //ContactsStrings.Title, Contact.Title, Namespaces.PoomContacts);
            //ContactsStrings.FirstName, Contact.FirstName, Namespaces.PoomContacts);
            //ContactsStrings.MiddleName, Contact.MiddleName, Namespaces.PoomContacts);
            //ContactsStrings.LastName, Contact.LastName, Namespaces.PoomContacts);
            //ContactsStrings.FileAs, Contact.FileAs, Namespaces.PoomContacts);
            //ContactsStrings.Email1Address, Contact.Email1Address, Namespaces.PoomContacts);
            //ContactsStrings.Email2Address, Contact.Email2Address, Namespaces.PoomContacts);
            //ContactsStrings.Email3Address, Contact.Email3Address, Namespaces.PoomContacts);
            //ContactsStrings.MobilePhoneNumber, Contact.MobilePhoneNumber, Namespaces.PoomContacts);
            //ContactsStrings.HomeAddressCity, Contact.HomeAddressCity, Namespaces.PoomContacts);
            //ContactsStrings.HomeAddressCountry, Contact.HomeAddressCountry, Namespaces.PoomContacts);
            //ContactsStrings.HomeFaxNumber, Contact.HomeFaxNumber, Namespaces.PoomContacts);
            //ContactsStrings.HomePhoneNumber, Contact.HomePhoneNumber, Namespaces.PoomContacts);
            //ContactsStrings.Home2PhoneNumber, Contact.Home2PhoneNumber, Namespaces.PoomContacts);
            //ContactsStrings.HomeAddressPostalCode, Contact.HomeAddressPostalCode, Namespaces.PoomContacts);
            //ContactsStrings.HomeAddressState, Contact.HomeAddressState, Namespaces.PoomContacts);
            //ContactsStrings.HomeAddressStreet, Contact.HomeAddressStreet, Namespaces.PoomContacts);
            //ContactsStrings.AssistantName, Contact.AssistantName, Namespaces.PoomContacts);
            //ContactsStrings.AssistantPhoneNumber, Contact.AssistantPhoneNumber, Namespaces.PoomContacts);
            //ContactsStrings.Business2PhoneNumber, Contact.Business2PhoneNumber, Namespaces.PoomContacts);
            //ContactsStrings.BusinessAddressCity, Contact.BusinessAddressCity, Namespaces.PoomContacts);
            //ContactsStrings.BusinessPhoneNumber, Contact.BusinessPhoneNumber, Namespaces.PoomContacts);
            //ContactsStrings.Webpage, Contact.WebPage, Namespaces.PoomContacts);
            //ContactsStrings.BusinessAddressCountry, Contact.BusinessAddressCountry, Namespaces.PoomContacts);
            //ContactsStrings.Department, Contact.Department, Namespaces.PoomContacts);
            //ContactsStrings.BusinessFaxNumber, Contact.BusinessFaxNumber, Namespaces.PoomContacts);
            //ContactsStrings.Alias, Contact.Alias, Namespaces.PoomContacts);
            //ContactsStrings.Suffix, Contact.Suffix, Namespaces.PoomContacts);
            //ContactsStrings.CompanyName, Contact.CompanyName, Namespaces.PoomContacts);
            //ContactsStrings.OtherAddressCity, Contact.OtherAddressCity, Namespaces.PoomContacts);
            //ContactsStrings.OtherAddressCountry, Contact.OtherAddressCountry, Namespaces.PoomContacts);
            //ContactsStrings.CarPhoneNumber, Contact.CarPhoneNumber, Namespaces.PoomContacts);
            //ContactsStrings.OtherAddressPostalCode, Contact.OtherAddressPostalCode, Namespaces.PoomContacts);
            //ContactsStrings.OtherAddressState, Contact.OtherAddressState, Namespaces.PoomContacts);
            //ContactsStrings.OtherAddressStreet, Contact.OtherAddressStreet, Namespaces.PoomContacts);
            //ContactsStrings.PagerNumber, Contact.PagerNumber, Namespaces.PoomContacts);
            //ContactsStrings.BusinessAddressPostalCode, Contact.BusinessAddressPostalCode, Namespaces.PoomContacts);
            //ContactsStrings.Spouse, Contact.Spouse, Namespaces.PoomContacts);
            //ContactsStrings.BusinessAddressState, Contact.BusinessAddressState, Namespaces.PoomContacts);
            //ContactsStrings.BusinessAddressStreet, Contact.BusinessAddressStreet, Namespaces.PoomContacts);
            //ContactsStrings.JobTitle, Contact.JobTitle, Namespaces.PoomContacts);
            //ContactsStrings.YomiFirstName, Contact.YomiFirstName, Namespaces.PoomContacts);
            //ContactsStrings.YomiLastName, Contact.YomiLastName, Namespaces.PoomContacts);
            //ContactsStrings.YomiCompanyName, Contact.YomiCompanyName, Namespaces.PoomContacts);
            //ContactsStrings.OfficeLocation, Contact.OfficeLocation, Namespaces.PoomContacts);
            //ContactsStrings.RadioPhoneNumber, Contact.RadioPhoneNumber, Namespaces.PoomContacts);
            //ContactsStrings.Picture, Contact.Picture, Namespaces.PoomContacts);
            if (element.HasChildElement(ContactsStrings.FirstName, Namespaces.PoomContacts))
                appData.Contact.FirstName = element.GetElementValueAsString(ContactsStrings.FirstName,
                    Namespaces.PoomContacts);

            if (element.HasChildElement(ContactsStrings.LastName, Namespaces.PoomContacts))
                appData.Contact.LastName = element.GetElementValueAsString(ContactsStrings.LastName,
                    Namespaces.PoomContacts);

            if (element.HasChildElement(ContactsStrings.MobilePhoneNumber, Namespaces.PoomContacts))
                appData.Contact.MobilePhoneNumber = element.GetElementValueAsString(ContactsStrings.MobilePhoneNumber,
                    Namespaces.PoomContacts);

            if (element.HasChildElement(ContactsStrings.BusinessPhoneNumber, Namespaces.PoomContacts))
                appData.Contact.BusinessPhoneNumber =
                    element.GetElementValueAsString(ContactsStrings.BusinessPhoneNumber,
                        Namespaces.PoomContacts);

            if (element.HasChildElement(ContactsStrings.FileAs, Namespaces.PoomContacts))
                appData.Contact.FileAs = element.GetElementValueAsString(ContactsStrings.FileAs,
                    Namespaces.PoomContacts);

            if (element.HasChildElement(ContactsStrings.Email1Address, Namespaces.PoomContacts))
                appData.Contact.Email1Address = element.GetElementValueAsString(ContactsStrings.Email1Address,
                    Namespaces.PoomContacts);
        }
    }

    public static class XmlNodeExtensionMethods
    {
        public static void AppendValueNodeIfNotNull(this XmlNode element, string elementName, string value,
            string nameSpace = null)
        {
            if (value != null)
                element.AppendValueNode(elementName, value, nameSpace);
        }
        public static void AppendValueNode(this XmlNode element, string elementName, string value,
            string nameSpace = null)
        {
            if (!(element is XmlDocument) && element.OwnerDocument == null)
                throw new ArgumentNullException("Missing XmlDocument.");

            var xmlDocument = element is XmlDocument ? (XmlDocument)element : element.OwnerDocument;

            var childNameSpace = nameSpace ?? element.NamespaceURI;

            var childNode = xmlDocument.CreateElement(elementName, childNameSpace);
            childNode.InnerText = value;

            element.AppendChild(childNode);
        }
        public static void AppendSelfClosingNode(this XmlNode element, string elementName, string nameSpace = null)
        {
            if (!(element is XmlDocument) && element.OwnerDocument == null)
                throw new ArgumentNullException("Missing XmlDocument.");

            var xmlDocument = element is XmlDocument ? (XmlDocument)element : element.OwnerDocument;

            var childNameSpace = nameSpace ?? element.NamespaceURI;

            var childNode = xmlDocument.CreateElement(elementName, childNameSpace);
            childNode.IsEmpty = true;
            element.AppendChild(childNode);
        }
        public static XmlNode AppendContainerNode(this XmlNode element, string elementName, string nameSpace = null, bool addXmlns = false)
        {
            if (!(element is XmlDocument) && element.OwnerDocument == null)
                throw new ArgumentNullException("Missing XmlDocument.");

            var xmlDocument = element is XmlDocument ? (XmlDocument)element : element.OwnerDocument;

            var childNameSpace = nameSpace ?? element.NamespaceURI;
            var childNode = xmlDocument.CreateElement(elementName, childNameSpace);
            if (addXmlns)
                childNode.SetAttribute("xmlns", nameSpace);

            element.AppendChild(childNode);

            return childNode;
        }
    }

    public static class XmlDocumentExtensionMethods
    {
        public static void CreateDecleration(this XmlDocument xmlDocument, params NameSpaceInfo[] nameSpaceInfos)
        {
            var syncXmlDeclare = xmlDocument.CreateXmlDeclaration("1.0", "utf-8", null);
            xmlDocument.InsertBefore(syncXmlDeclare, null);

            foreach (var nameSpaceInfo in nameSpaceInfos)
            {
                var xmlAttribute = xmlDocument.CreateAttribute("xmlns:" + nameSpaceInfo.Prefix);
                xmlAttribute.Value = nameSpaceInfo.Name;
            }
        }
    }
}
