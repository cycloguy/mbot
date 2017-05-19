using ContactHandler;
using EmailModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EmailService
{

    public class Worker
    {
        List<ICommandHandler.ICommandHandler> controllers = new List<ICommandHandler.ICommandHandler>();
        EmailReceiver reader;
        public Worker()
        {
            var controller = new ContactController();
            controller.OnPostBack += OnPostBack;
            controllers.Add(controller);
            //var controller2 = new ContactController2();
            //controller2.OnPostBack += OnPostBack;
            //controllers.Add(controller2);

            reader = new EmailReceiver();
            reader.FoundEmail += Rearder_FoundEmail;
        }
        public void Execute()
        {
            reader.Do();
        }
        private void OnPostBack(object sender, MyEmailEventArgs e)
        {
            Console.WriteLine(e.Subject);
        }

        private void Rearder_FoundEmail(object sender, MyEmailEventArgs e)
        {
            if (controllers.All(s => !s.CanProcess(e)))
            {
                //invalidcontroller.process(e);
            }
            else
                controllers.Where(s => s.CanProcess(e))
                    .ToList()
                    .ForEach(d => d.Process(e));
        }
    }
}
