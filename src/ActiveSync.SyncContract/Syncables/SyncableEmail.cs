using System;
using System.Collections.Generic;

namespace ActiveSync.SyncContract.Syncables
{
    public class SyncableEmail
    {
        /// <summary>
        /// Email Server Id
        /// </summary>
        public string Id { get; set; }

        public string FolderId { get; set; }


        /// <summary>
        /// The To element is an optional element that specifies the list of primary recipients (1) of a message.
        /// The value of this element contains one or more e-mail addresses. If there are multiple e-mail addresses, they are separated by commas.
        /// </summary>
        public string To { get; set; }
        /// <summary>
        /// The Cc element is an optional element that specifies the list of secondary recipients (1) of a message.
        /// If there are multiple e-mail addresses, they are separated by commas.
        /// </summary>
        public string Cc { get; set; }
        /// <summary>
        /// The From element is an optional element that specifies the e-mail address of the message sender.
        /// </summary>
        public string From { get; set; }
        /// <summary>
        /// the Subject element is an optional element that specifies the subject of the e-mail message.
        /// 
        /// </summary>
        public string Subject { get; set; }
        /// <summary>
        /// The content of the Data element is returned as a string in the format that is specified by the Type element
        /// </summary>
        public string Body { get; set; }  
        /// <summary>
        /// The ReplyTo element is an optional element that specifies the e-mail address(es) to which replies will be addressed by default.
        /// If there are multiple e-mail addresses, they are separated by a semi-colon.
        /// </summary>
        public string ReplyTo { get; set; }
        /// <summary>
        /// The DateReceived specifies the date and time the message was received by the current recipient (1).
        /// </summary>
        public DateTime DateReceived { get; set; }
        /// <summary>
        /// The DisplayTo element is an optional element that specifies the e-mail addresses of the primary recipients (1) of this message.
        /// The value of this element contains one or more display names. If there are multiple display names, they are separated by semi-colons.
        /// </summary>
        public string DisplayTo { get; set; }
        /// <summary>
        /// The ThreadTopic element is an optional element that specifies the topic used for conversation threading.
        /// </summary>
        public string ThreadTopic { get; set; }
        /// <summary>
        /// The Importance element is an optional element that specifies the importance of the message, as assigned by the sender.
        /// </summary>
        public EmailImportance Importance { get; set; }
        /// <summary>
        /// The Read element is an optional element that specifies whether the e-mail message has been viewed by the current recipient (1).
        /// </summary>
        public bool Read { get; set; }
        /// <summary>
        /// The Categories specifies a collection of user-selected categories assigned to the e-mail message.
        /// </summary>
        public List<string> Categories { get; set; }
        public string ConversationId { get; set; }
        public string ConversationIndex { get; set; }
        /// <summary>
        /// The LastVerbExecuted element is an optional element that indicates the last action, such as reply or forward, that was taken on the message.
        /// The client SHOULD use the value of this element to display the icon related to the message.
        /// </summary>
        public LastVerbExecutedOnEmail LastVerbExecuted { get; set; }
        /// <summary>
        /// The LastVerbExecutionTime element is an optional element that indicates the date and time when the action specified by the LastVerbExecuted element was performed on the message.
        /// </summary>
        public DateTime? LastVerbExecutionTime { get; set; }
        /// <summary>
        /// The ReceivedAsBcc element is an optional element that indicates to the user that they are a blind carbon copy (Bcc) recipient on the email.
        /// </summary>
        public bool ReceivedAsBcc { get; set; }
        /// <summary>
        /// The Sender element is an optional element that indicates the message was not sent from the user identified by the From element 
        /// The value of the Sender element is an e-mail address
        /// This element is set by the server and is read-only on the client.
        /// </summary>
        public string Sender { get; set; }
        /// <summary>
        /// The AccountId element is an optional element that specifies a unique identifier for the account that received a message.
        /// </summary>
        public Guid AccountId { get; set; }

    }

    public enum LastVerbExecutedOnEmail : byte
    {
        Unknown = 0,
        ReplyToSender = 1,
        ReplyToAll = 2,
        Forward = 3
    }

    public enum EmailImportance:byte
    {
        /// <summary>
        /// اولویت پایین
        /// </summary>
        Low = 0,
        /// <summary>
        /// معمولی - پیش فرض
        /// </summary>
        Normal = 1,
        /// <summary>
        /// اولویت بالا
        /// </summary>
        High = 2
    }
}
