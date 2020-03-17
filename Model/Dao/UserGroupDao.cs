using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Model.EF;

namespace Model.Dao
{
    public class UserGroupDao
    {
        WatchShop db = null;
        public UserGroupDao()
        {
            db = new WatchShop();
        }
        public List<UserGroup> ListAll()
        {
            return db.UserGroups.ToList();
        }
    }
}
