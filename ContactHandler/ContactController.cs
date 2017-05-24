using System;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using CommandModel;
using ICommandHandler;
using System.Text.RegularExpressions;
using System.IO;

namespace ContactHandler
{
    public class ContactController: ICommandHandler.ICommandHandler
    {
        private ContactContext db = new ContactContext();

        public event PostBack OnPostBack;

        void PostProcessCommand(MyEmailEventArgs e)
        {
            if (OnPostBack != null)
                OnPostBack(this, e);
        }
         public IQueryable<Contact> GetContacts()
        {
            return db.Contacts;
        }

        public Contact GetContact(int id)
        {
            Contact contact = db.Contacts.Find(id);
            if (contact == null)
            {
                return null;
            }

            return contact;
        }

        public bool UpdateContact(int id, Contact contact)
        {

            if (id != contact.Id)
            {
                return false;
            }

            db.Entry(contact).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ContactExists(id))
                {
                    return false;
                }
                else
                {
                    throw;
                }
            }

            return true;
        }

        public void CreateContact(Contact contact)
        {
            db.Contacts.Add(contact);
            db.SaveChanges();
        }

        public bool DeleteContact(int id)
        {
            Contact contact = db.Contacts.Find(id);
            if (contact == null)
            {
                return false;
            }

            db.Contacts.Remove(contact);
            db.SaveChanges();

            return true;
        }

        private bool ContactExists(int id)
        {
            return db.Contacts.Count(e => e.Id == id) > 0;
        }
        private bool ContactExists(string name)
        {
            return db.Contacts.Count(e => e.Name == name) > 0;
        }
        public void Process(MyEmailEventArgs e)
        {
            //using (StreamWriter writetext = new StreamWriter("emailContent.txt"))
            //{
            //    writetext.WriteLine(e.Content);
            //}
            CreateContact(ParseEmailContent(e.Content));

            PostProcessCommand(e);
            Console.WriteLine("-------------------------------");
        }

        public bool CanProcess(MyEmailEventArgs e)
        {
            //Implement logic of command
            return (e.Subject.Contains("Updated Card"));
        }
        string StripHTMLTag(string html)
        {
            return Regex.Replace(html, "<.*?>", String.Empty);
        }
        Contact ParseEmailContent(string content)
        {
            string[] stringSeparators = new string[] { "<br>" };
            string[] lines = content.Split(stringSeparators, StringSplitOptions.None);
            Contact contact = new Contact();
            foreach (string line in lines)
            {
                // Console.WriteLine(StripHTMLTag(line));
                string[] sub = StripHTMLTag(line).Split(':');
                if (sub[0] == "Name")
                    contact.Name = sub[1];
                if (sub[0] == "Work" && sub[1].Contains("@"))
                    contact.Email = sub[1];
                if (sub[0] == "Work" && !sub[1].Contains("@"))
                    contact.Tel = sub[1];
                if (sub[0].Contains("Fax"))
                    contact.Fax = sub[1];
                if (sub[0].Contains("Address"))
                    contact.Address1 = sub[1];
            }
            return contact;
        }
    }

}
