using ContactHandler;
using System;
using System.Collections.Generic;
using System.Linq;
using NonCommandHandler;
using CommandModel;
using MailBot.Utility;

namespace EmailService
{

    public class Worker
    { 
        List<ICommandHandler.ICommandHandler> controllers = new List<ICommandHandler.ICommandHandler>();
        EmailReceiver reader;
        NonCommandController nonCommandControlder = new NonCommandController();
        Chilkat.MailMan mailMan = new Chilkat.MailMan();
        public Worker()
        {
            InitEmail();
            reader = new EmailReceiver(mailMan);

            var controller = new ContactController();
            controller.OnPostBack += OnPostBack;
            controllers.Add(controller);

            //var controller2 = new ForwardController(mailMan);
            //controller2.OnPostBack += OnPostBack;
            //controllers.Add(controller2);

            //nonCommandControlder.OnPostBack += OnPostBack;
            reader.FoundEmail += Rearder_FoundEmail;
        }

        private void InitEmail()
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
            mailMan.SmtpHost = appsettings.mailServer;
            //  Set the POP3 login/password.
            mailMan.SmtpUsername = appsettings.mailAccount;
            mailMan.SmtpPassword = appsettings.mailPassword;
            //  Set the SMTP login/password.
            mailMan.PopUsername = appsettings.mailAccount;
            mailMan.PopPassword = appsettings.mailPassword;
           
        }
        public void Execute()
        {
            reader.Scan();
        }
        private void OnPostBack(object sender, MyEmailEventArgs e)
        {
            Console.WriteLine(e.Subject);
        }

        private void Rearder_FoundEmail(object sender, MyEmailEventArgs e)
        {
            if (controllers.All(s => !s.CanProcess(e)))
            {
                nonCommandControlder.Process(e);
            }
            else
                controllers.Where(s => s.CanProcess(e))
                    .ToList()
                    .ForEach(d => d.Process(e));
        }
    }
}
