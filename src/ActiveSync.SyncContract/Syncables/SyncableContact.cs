using System;
using System.Collections.Generic;

namespace ActiveSync.SyncContract.Syncables
{
    public class SyncableContact
    {
        public string Id { get; set; }
        public DateTime? Anniversary { get; set; }
        public string AssistantName { get; set; }
        public string AssistantPhoneNumber { get; set; }
        public DateTime? Birthday { get; set; }
        public string Business2PhoneNumber { get; set; }
        public string BusinessAddressCity { get; set; }
        public string BusinessPhoneNumber { get; set; }
        public string WebPage { get; set; }
        public string BusinessAddressCountry { get; set; }
        public string Department { get; set; }
        public string Email1Address { get; set; }
        public string Email2Address { get; set; }
        public string Email3Address { get; set; }
        public string BusinessFaxNumber { get; set; }
        public string FileAs { get; set; }
        public string Alias { get; set; }
        public int? WeightedRank { get; set; }
        public string FirstName { get; set; }
        public string MiddleName { get; set; }
        public string HomeAddressCity { get; set; }
        public string HomeAddressCountry { get; set; }
        public string HomeFaxNumber { get; set; }
        public string HomePhoneNumber { get; set; }
        public string Home2PhoneNumber { get; set; }
        public string HomeAddressPostalCode { get; set; }
        public string HomeAddressState { get; set; }
        public string HomeAddressStreet { get; set; }
        public string MobilePhoneNumber { get; set; }
        public string Suffix { get; set; }
        public string CompanyName { get; set; }
        public string OtherAddressCity { get; set; }
        public string OtherAddressCountry { get; set; }
        public string CarPhoneNumber { get; set; }
        public string OtherAddressPostalCode { get; set; }
        public string OtherAddressState { get; set; }
        public string OtherAddressStreet { get; set; }
        public string PagerNumber { get; set; }
        public string Title { get; set; }
        public string BusinessAddressPostalCode { get; set; }
        public string LastName { get; set; }
        public string Spouse { get; set; }
        public string BusinessAddressState { get; set; }
        public string BusinessAddressStreet { get; set; }
        public string JobTitle { get; set; }
        public string YomiFirstName { get; set; }
        public string YomiLastName { get; set; }
        public string YomiCompanyName { get; set; }
        public string OfficeLocation { get; set; }
        public string RadioPhoneNumber { get; set; }
        public string Picture { get; set; }
        public List<string> Categories { get; set; }
        public List<string> Children { get; set; }
        
        //Contact2
        public string CustomerId { get; set; }
        public string GovernmentId { get; set; }
        public string IMAddress { get; set; }
        public string IMAddress2 { get; set; }
        public string IMAddress3 { get; set; }
        public string ManagerName { get; set; }
        public string CompanyMainPhone { get; set; }
        public string AccountName { get; set; }
        public string NickName { get; set; }
        public string MMS { get; set; }

    }
}
