using ContactHandler;
using EmailModel;
using System.Timers;
using System.ServiceProcess;
using System;

namespace EmailService
{
    partial class WinService : ServiceBase
    {
        private static System.Timers.Timer timer;
        private Worker worker;
        public WinService()
        {
            InitializeComponent();
            worker = new 
                Worker();
        }

        protected override void OnStart(string[] args)
        {
            timer = new System.Timers.Timer(10000); //1 minute
            timer.Elapsed += new ElapsedEventHandler(OnTimerEvent);
            timer.Enabled = true;

        }

        private void OnTimerEvent(object sender, ElapsedEventArgs e)
        {
            Console.WriteLine("The Elapsed event was raised at {0}", e.SignalTime);
            worker.Execute();
        }

        protected override void OnStop()
        {

        }

    
    }
}
