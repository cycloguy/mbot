using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Chilkat;
using EmailModel;

namespace EmailService
{
    public delegate void FoundEmailEventHandler(object sender, MyEmailEventArgs e);
    public class EmailReceiver
    {
        public event FoundEmailEventHandler FoundEmail;
        public void Do()
        {
            var appsettings = new Settings();
            Chilkat.MailMan mailMan = new Chilkat.MailMan();

            bool success = mailMan.UnlockComponent(appsettings.chilkatMailKey);
            if (success != true)
            {
                Console.WriteLine(mailMan.LastErrorText);
                return;
            }

            //  Set the POP3 server's hostname
            mailMan.MailHost = appsettings.mailServer;

            //  Set the POP3 login/password.
            mailMan.PopUsername = appsettings.mailAccount;
            mailMan.PopPassword = appsettings.mailPassword;

            int numMessages = mailMan.GetMailboxCount();
            int i;
            if (numMessages == 0)
                return;
            Chilkat.Email email = null;
            //get 1
            numMessages = 1;
            for (i = 1; i <= numMessages; i++)
            {

                email = mailMan.FetchByMsgnum(i);
                if (email == null)
                {
                    Console.WriteLine(mailMan.LastErrorText);
                    return;
                }
                if (ValidateCommand(email))
                {
                    TriggerEvent(email);
                }
                else
                {
                    ProcessInvalidEmail(email);
                }
            }

        }

        private void ProcessInvalidEmail(Email email)
        {
            throw new NotImplementedException();
        }

        private bool ValidateCommand(Email email)
        {
            return true;
        }

        public void TriggerEvent(Chilkat.Email email)
        {
            //Raise event
            var emailEventArgs = new MyEmailEventArgs();
            emailEventArgs.From = ReadEmail.Utility.CleanEmailAddress( email.From);
            emailEventArgs.Subject = email.Subject;
          
            FoundEmail(this, emailEventArgs);



            //int numAttached;
            //bool success;
            
            //numAttached = email.NumAttachments;
            //if (numAttached > 0)
            //{
            //    success = email.SaveAllAttachments(email.Uidl + "myAttachments");
            //    if (success != true)
            //    {
            //        Console.WriteLine(email.LastErrorText);
            //    }
            //}
        }
        public virtual void OnFoundEmail(MyEmailEventArgs e)
        {
            if (FoundEmail != null)
                FoundEmail(this, e);
        }
    }
}
