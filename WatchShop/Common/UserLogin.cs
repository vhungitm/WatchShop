using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WatchShop.Common
{
    [Serializable]
    public class UserLogin
    {
        public long UserID { set; get; }
        public string Username { set; get; }
        public string Name { set; get; }

        public string GroupID { set; get; }
    }
}