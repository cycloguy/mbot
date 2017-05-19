using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ICommandHandler;
using CommandModel;
using MailBot.Utility;

namespace NonCommandHandler
{
    public class NonCommandController : ICommandHandler.ICommandHandler
    {
        public event PostBack OnPostBack;
        Chilkat.MailMan mailMan = new Chilkat.MailMan();
        public NonCommandController()
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
        public bool CanProcess(MyEmailEventArgs e)
        {
            throw new NotImplementedException();
        }

        public void Process(MyEmailEventArgs e)
        {
            throw new NotImplementedException();
        }
    }
}
