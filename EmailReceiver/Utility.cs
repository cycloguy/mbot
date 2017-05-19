using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace ReadEmail
{
    class Utility
    {
        public static string CleanEmailAddress(string emailAddresses)
        {
            string data = emailAddresses; //read File 
                                                        //instantiate with this pattern 
            Regex emailRegex = new Regex(@"\w+([-+.]\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*",
                RegexOptions.IgnoreCase);
            //find items that matches with our pattern
            MatchCollection emailMatches = emailRegex.Matches(data);

            string cleanedEmailAddresses = string.Empty;

            foreach (Match emailMatch in emailMatches)
            {
                cleanedEmailAddresses += emailMatch.Value + ";";
            }
            if (cleanedEmailAddresses.Length > 0)
                cleanedEmailAddresses = cleanedEmailAddresses.Substring(0, cleanedEmailAddresses.Length - 1);
            return cleanedEmailAddresses;
        }
    }
}
