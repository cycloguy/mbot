using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ContactHandler;
using EmailModel;


namespace EmailService
{
    class Program
    {
        static ICommandHandler.ICommandHandler controller = new ContactController();
        static void Main(string[] args)
        {
            controller.OnPostBack += OnPostBack;
                var rearder = new EmailReceiver();
            rearder.FoundEmail += Rearder_FoundEmail;
            rearder.Do();
            Console.ReadLine();
        }

        private static void OnPostBack(object sender, MyEmailEventArgs e)
        {
            Console.WriteLine(e.Subject);
        }

        private static void Rearder_FoundEmail(object sender, MyEmailEventArgs e)
        {
            controller.Process(e);
        }
    }
}
