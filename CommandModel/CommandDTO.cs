using System;

namespace CommandModel
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
        public string[] FileNames { get; set; }
        public string Folder { get; set; }
        public string Uidl { get; set; }

    }
}
