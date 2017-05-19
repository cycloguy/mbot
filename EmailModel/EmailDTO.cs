using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EmailModel
{
    public class MyEmailEventArgs: EventArgs
    {
        public string From { get; set; }
        public string To { get; set; }
        public string Cc { get; set; }
        public string Bcc { get; set; }
        public string Subject { get; set; }
        public string Content { get; set; }
        public bool HasAttachment { get; set; }
        public string FileName { get; set; }
        public string Folder { get; set; }
        public int Uidl { get; set; }

    }
}
