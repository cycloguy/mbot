using ContactHandler;
using System;
using System.Collections.Generic;
using System.Linq;
using NonCommandHandler;
using CommandModel;

namespace EmailService
{

    public class Worker
    { 
        List<ICommandHandler.ICommandHandler> controllers = new List<ICommandHandler.ICommandHandler>();
        EmailReceiver reader = new EmailReceiver();
        NonCommandController nonCommandControlder = new NonCommandController();
        public Worker()
        {
            var controller = new ContactController();
            controller.OnPostBack += OnPostBack;
            controllers.Add(controller);

            
            nonCommandControlder.OnPostBack += OnPostBack;
            reader.FoundEmail += Rearder_FoundEmail;
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
