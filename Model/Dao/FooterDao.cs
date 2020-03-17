using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Model.EF;

namespace Model.Dao
{
    public class FooterDao
    {
        WatchShop db = null;
        public FooterDao()
        {
            db = new WatchShop();
        }
        public Footer GetFooter()
        {
            return db.Footers.Find(1);
        }
        public bool Update(Footer NewEntity)
        {
            try
            {
                var Entity = db.Footers.Find(1);
                Entity.Content = NewEntity.Content;
                Entity.Status = NewEntity.Status;

                db.SaveChanges();
                return true;
            }
            catch (Exception) { return false; }
         }
    }
}
