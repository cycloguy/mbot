using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ICommandHandler;
using CommandModel;
using MailBot.Utility;
using Chilkat;

namespace NonCommandHandler
{
    public class ForwardController : ICommandHandler.ICommandHandler
    {
        public event PostBack OnPostBack;
        Chilkat.MailMan mailMan;
        public ForwardController(Chilkat.MailMan mMan)
        {
            mailMan = mMan;
        }
        public bool CanProcess(MyEmailEventArgs e)
        {
            if (e.From.ToUpper().Contains("ACOM.COM"))
                return true;
            return false;
        }
        void PostProcessCommand(MyEmailEventArgs e)
        {
            if (OnPostBack != null)
                OnPostBack(this, e);
        }
        public void Process(MyEmailEventArgs e)
        {

            Console.WriteLine("Forwarding message {0}!!!", e.Subject);
            Chilkat.Email mail = mailMan.FetchEmail(e.Uidl);
            if (mail == null)
            {
                Console.WriteLine("Cannot get the remail {0}", e.Uidl);
                return;
            }
            Chilkat.Email forwardMail = mail.CreateForward();
            forwardMail.From = "thao.vo@ezitsol.com";
            forwardMail.AddTo("Thao Vo", "votthao@yahoo.com");
            bool success = mailMan.SendEmail(forwardMail);
            if (success == false)
                Console.WriteLine(mailMan.LastErrorText);

            PostProcessCommand(e);
        }
    }
}
