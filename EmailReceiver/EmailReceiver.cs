using System;
using Chilkat;
using CommandModel;
using MailBot.Utility;

namespace EmailService
{
    public delegate void FoundEmailEventHandler(object sender, MyEmailEventArgs e);
    public class EmailReceiver
    {
        public event FoundEmailEventHandler FoundEmail;
        Chilkat.MailMan mailMan = new Chilkat.MailMan();
        public EmailReceiver()
        {
            var appsettings = new Settings();
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
        }
        public void Scan()
        {
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
                    ProcessValidEmail(email);
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
            //if (email.From.ToUpper().Contains("EZITSOL.COM"))
                return true;
            //return false;
        }

        public void ProcessValidEmail(Chilkat.Email email)
        {
            //Raise event
            var emailEventArgs = new MyEmailEventArgs();
            emailEventArgs.From = EmailUtility.CleanEmailAddress( email.From);
            emailEventArgs.Subject = email.Subject;

            OnFoundEmail(emailEventArgs);



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
