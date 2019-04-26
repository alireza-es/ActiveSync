using System;
using System.Collections.Generic;

namespace ActiveSync.Core.Requests.Handlers.ItemOperations
{
    public class FetchItemOperation
    {
        public string Store { get; set; }
        public string ServerId { get; set; }
        public string CollectionId { get; set; }
        public Uri LinkId { get; set; }
        public string LongId { get; set; }
        public string FileReference { get; set; }
        public List<FetchItemOperationOption> FetchItemOperationOptions { get; set; }
        public string RemoveRightsManagementProtection { get; set; }
        
    }

    public class FetchItemOperationOption
    {
        public List<Schema> Schemata { get; set; }
        public string Range { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
        public eMIMESendSupport MIMESupport { get; set; }
        public string BodyPreference { get; set; }
        public string BodyPartPreference { get; set; }
        public string RightsManagementSupport { get; set; }
    }

    public class Schema
    {

    }
}
