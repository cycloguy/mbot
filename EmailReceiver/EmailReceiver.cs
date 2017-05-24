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
        Chilkat.MailMan mailMan;
        public EmailReceiver(Chilkat.MailMan mMan)
        {
            mailMan = mMan;
        }
        public void Scan()
        {
            Chilkat.StringArray saUidls = null;
            //  Get the complete list of UIDLs
            saUidls = mailMan.GetUidls();
            if (saUidls == null)
            {
                return;
            }
            //  Get the 10 most recent UIDLs
            //  The 1st email is the oldest, the last email is the newest
            //  (usually)
            int i;
            int n;
            int startIdx;
            int bundleSize = 3;
            n = saUidls.Count;
            startIdx = n > bundleSize ? n - bundleSize : 0;

            Chilkat.StringArray saUidls2 = new Chilkat.StringArray();
            for (i = startIdx; i <= n - 1; i++)
            {
                saUidls2.Append(saUidls.GetString(i));
            }
            //  Download in full the 10 most recent emails:
            Chilkat.EmailBundle bundle = null;

            bundle = mailMan.FetchMultiple(saUidls2);
            if (bundle == null)
            {
                Console.WriteLine(mailMan.LastErrorText);
                return;
            }

            Chilkat.Email email = null;
            for (i = 0; i <= bundle.MessageCount - 1; i++)
            {
                email = bundle.GetEmail(i);
                Console.WriteLine(email.From);
                Console.WriteLine(email.Subject);
                Console.WriteLine("----");
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
            emailEventArgs.Uidl = email.Uidl;
            emailEventArgs.Content = email.Body;
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
