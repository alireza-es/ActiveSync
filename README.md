# ActiveSync framework in .Net
<p>
ActiveSync is an open-source framework to synchronize ActiveSync compatible devices such as mobile phones (Android, iOS, Windows Phone, Symbian), tablets, Outlook, etc. </p>
<p>I wrote it from scratch and I hope it would be the primary and robust implementation of Active Sync protocol in .Net. </p>

<p>
About 5 years ago, I had a project to synchronize an office automation system with mobile. The requirement was that users want to access their contacts and inboxes in mobile. </p>
<p>We did not want to implement multiple apps for different types of mobile platforms like Android, iOS, Windows Phone, etc. Therefore, we started to research and finally, we decided to implement a standard protocol to support synchronization of items in any device.</p>
<p>I started to learn about Microsoft Active Sync protocol. This protocol is used in Microsoft Exchange too. At that time, there were no library in .net and I had to implement it from scratch. I try to design a framework to support ActiveSync in .Net and it can be use in anywhere you need!</p>
<p>
You can easily have your own implementation of active sync. Just implement these services:
<UL>
  <li><b>IAuthenticationService:</b> To authenticate users
    <ul>
      <li>Authenticate</li>
    </ul>  
  </li>
  <li><b>IFolderService:</b> To support synchronization of folders
    <ul>
      <li>CreateFolder</li>
      <li>UpdateFolder</li>
      <li>DeleteFolder</li>
      <li>GetFolder</li>
      <li>GetAllFolders</li>
      <li>EmptyFolderContents</li>
    </ul>  
  </li>

  <li><b>IContactService:</b> To support synchronization of contacts
    <ul>
      <li>GetContacts</li>
      <li>FetchContact</li>
      <li>UpdateContact</li>
      <li>AddContact</li>
      <li>DeleteContact</li>
    </ul>  
  </li>

  <li><b>IEmailService:</b> To support synchronization of emails (or messages in your domain)
    <ul>
      <li>SendMail</li>
      <li>GetEmails</li>
      <li>ReadMail</li>
      <li>UnReadMail</li>
      <li>EditMail</li>
      <li>ReplyEmail</li>
      <li>ForwardEmail</li>
      <li>FetchEmail</li>
      <li>MoveConversation</li>
    </ul>  
  </li>

</UL>
</p>
<p>
  Now I am creating a new version of ActiveSync in .Net Core and I appreciate any contribution to develop this framework.
</p>
