# ActiveSync framework in .Net
A pure and from scratch implementation of Active Sync protocol in .Net
About 5 years ago, I had a project to synchronize an office automation system with mobile. The requirement was that users want to access their contacts and inboxes in mobile. 
We did not want to implement multiple apps for different types of mobile platforms like Android, iOS, Windows Phone, etc. Therefore, we started to research and finally, we decided to implement a standard protocol to support synchronization of items in any device.
I started to learn about Microsoft Active Sync protocol. This protocol is used in Microsoft Exchange too. At that time, there were no library in .net and I had to implement it from scratch. I try to design a framework to support ActiveSync in .Net and it can be use in anywhere you need!

You can easily have your own implementation of active sync. Just implement these services:
<UL>
  <li><b>IFolderService:</b> To support synchronization of folders</li>
  <li><b>IContactService:</b> To support synchronization of contacts</li>
  <li><b>IEmailService:</b> To support synchronization of emails (or messages in your domain)</li>
</UL>
