using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Model.EF;

namespace Model.Dao
{
    public class ContactDAO
    {
        WatchShop db = null;
        public ContactDAO()
        {
            db = new WatchShop();
        }
        public Contact GetContact()
        {
            return db.Contacts.Find(1);
        }
        public bool Update(Contact NewEntity)
        {
            try
            {
                var Entity = db.Contacts.Find(1);
                Entity.Content = NewEntity.Content;
                Entity.Status = NewEntity.Status;

                db.SaveChanges();
                return true;
            }
            catch (Exception) { return false; }
        }
    }
}
