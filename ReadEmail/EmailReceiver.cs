using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReadEmail
{
    public class EmailReceiver
    {
        public void Do()
        {
            var appsettings = new Settings();
            Chilkat.MailMan mailman = new Chilkat.MailMan();

            bool success = mailman.UnlockComponent(appsettings.chilkatMailKey);
            if (success != true)
            {
                Console.WriteLine(mailman.LastErrorText);
                return;
            }

            //  Set the POP3 server's hostname
            mailman.MailHost = appsettings.mailServer;

            //  Set the POP3 login/password.
            mailman.PopUsername = appsettings.mailAccount;
            mailman.PopPassword = appsettings.mailPassword;

            int numMessages;

            numMessages = mailman.GetMailboxCount();
            int i;
            if (numMessages > 0)
            {

                Chilkat.Email email = null;
                for (i = 1; i <= numMessages; i++)
                {

                    email = mailman.FetchByMsgnum(i);
                    if (email == null)
                    {
                        Console.WriteLine(mailman.LastErrorText);
                        return;
                    }
                    else {
                        int numAttached;
                        numAttached = email.NumAttachments;
                        string fileName = string.Empty;

                        if (numAttached > 0)
                        {
                            success = email.SaveAllAttachments("myAttachments");
                            if (success != true)
                            {
                                Console.WriteLine(email.LastErrorText);

                            }
                            for (int j = 0; j <= email.NumAttachments - 1; j++)
                            {
                                Console.WriteLine(email.GetAttachmentFilename(j));

                            }
                            fileName = email.GetAttachmentFilename(0);
                        }
                        Console.WriteLine(email.From + ": " + email.Subject + fileName + "\n");

                    }

                }

            }
        }
    }
}
