using System;
using System.Configuration;

namespace ReadEmail
{
    public class Settings
    {
        public string chilkatMailKey { get; set; }
        public string mailServer { get; set; }
        public string mailAccount { get; set; }
        public string mailPassword { get; set; }
        public string testProp { get; set; }
        public Settings()
        {
            var appSettings = ConfigurationManager.AppSettings;
            chilkatMailKey = appSettings["ChilkatMailKey"] ?? "30-day trial";
            mailServer = appSettings["mailServer"] ?? "Not Found";
            mailAccount = appSettings["mailAccount"] ?? "Not Found";
            mailPassword = appSettings["mailPassword"] ?? "Not Found";
            testProp = appSettings["TestProp"] ?? "Not Found";
        }
            
    }
}
