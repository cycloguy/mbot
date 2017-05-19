using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EmailModel;
using ICommandHandler;
namespace ContactHandler
{
    public class ContactController: ICommandHandler.ICommandHandler
    {
        private ContactContext db = new ContactContext();

        public event PostBack OnPostBack;

        public ContactController()
        {
                        
        }

        void PostCommand(MyEmailEventArgs e)
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

        public Contact CreateContact(Contact contact)
        {
            db.Contacts.Add(contact);
            db.SaveChanges();

            return contact;
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


        public void Process(MyEmailEventArgs e)
        {
            if (e.Subject.Length > 0)
            {
                Console.WriteLine(e.From);
                PostCommand(e);
                Console.WriteLine("-------------------------------");
            }
        }
    }
}
