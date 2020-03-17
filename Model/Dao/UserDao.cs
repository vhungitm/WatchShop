using Model.EF;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PagedList;
using Common;
namespace Model.Dao
{
    public class UserDao
    {
        WatchShop db = null;
        public UserDao()
        {
            db = new WatchShop();
        }
        public IEnumerable<User> ListAllPaging(string searchString, int page, int pageSize)
        {
            IQueryable<User> Entity = db.Users;
            if (!string.IsNullOrEmpty(searchString))
            {
                Entity = Entity.Where(x => x.Name.Contains(searchString));
            }
            return Entity.OrderByDescending(x => x.CreatedDate).ToPagedList(page, pageSize);
        }
        public User GetByUsername(string username)
        {
            return db.Users.SingleOrDefault(x => x.Username == username);
        }
        public int Login(string username, string password, bool isLoginAdmin = false)
        {
            var result = db.Users.SingleOrDefault(x => x.Username == username);
            if (result == null)
            {
                return 0; //Tài khoản không tồn tại
            }
            else
            {
                if (isLoginAdmin == true)
                {
                    if (result.GroupID == CommonConstants.ADMIN_GROUP || result.GroupID == CommonConstants.MOD_GROUP)
                    {
                        if (!result.Status)
                        {
                            return -1; //Tài khoản bị khóa
                        }
                        else
                        {
                            if (result.Password == password)
                            {
                                return 1; //Tài khoản chính xác
                            }
                            else
                            {
                                return -2; //Sai mật khẩu
                            }
                        }
                    }
                    else
                    {
                        return -3; // Không có quyền truy cập
                    }
                }
                else
                {
                    if (!result.Status)
                    {
                        return -1; //Tài khoản bị khóa
                    }
                    else
                    {
                        if (result.Password == password)
                        {
                            return 1; //Tài khoản chính xác
                        }
                        else
                        {
                            return -2; //Sai mật khẩu
                        }
                    }
                }
            }
        }

        public bool InsertForFaceBook(User Entity)
        {
            var user = db.Users.SingleOrDefault(x => x.Username == Entity.Username);
            if (user == null)
            {
                db.Users.Add(Entity);
                db.SaveChanges();
                return true;
            }
            else
            {
                return false;
            }
        }

        public bool Insert(User user)
        {
            try
            {
                db.Users.Add(user);
                db.SaveChanges();
                return true;
            }
            catch (Exception) { return false; }
        }
        public bool Delete(long id)
        {
            try
            {
                var Entity = db.Users.Find(id);
                db.Users.Remove(Entity);
                db.SaveChanges();
                return true;
            }
            catch (Exception e) { return false; }
        }
        public User GetById(long id)
        {
            return db.Users.Find(id);
        }
        public bool Update(User NewEntity)
        {
            try
            {
                var Entity = db.Users.Find(NewEntity.ID);
                Entity.Password = NewEntity.Password;
                Entity.Name = NewEntity.Name;
                Entity.GroupID = NewEntity.GroupID;
                Entity.Gender = NewEntity.Gender;
                Entity.DateOfBirth = NewEntity.DateOfBirth;
                Entity.Address = NewEntity.Address;
                Entity.Email = NewEntity.Email;
                Entity.Phone = NewEntity.Phone;
                Entity.Status = NewEntity.Status;
                Entity.ModifiedBy = NewEntity.ModifiedBy;
                Entity.ModifiedDate = NewEntity.ModifiedDate;
                db.SaveChanges();
                return true;
            }
            catch (Exception e) { return false; }
        }
        public bool ChangStatus(long id)
        {
            var Entity = db.Users.Find(id);
            Entity.Status = !Entity.Status;
            db.SaveChanges();

            return Entity.Status;
        }

        public bool CheckUsername(string Username)
        {
            return db.Users.Count(x => x.Username == Username) > 0;
        }

        public bool CheckEmail(string Email)
        {
            return db.Users.Count(x => x.Email == Email) > 0;
        }

        public List<string> GetListCredential(string username)
        {
            var user = db.Users.Single(x => x.Username == username);
            var data = (from a in db.Credentials
                        join b in db.UserGroups on a.UserGroupID equals b.ID
                        join c in db.Roles on a.RoleID equals c.ID
                        where b.ID == user.GroupID
                        select new
                        {
                            RoleID = a.RoleID,
                            UserGroupID = a.UserGroupID,
                        }).AsEnumerable().Select(x => new Credential()
                        {
                            RoleID = x.RoleID,
                            UserGroupID = x.UserGroupID,

                        });
            return data.Select(x => x.RoleID).ToList();
        }
    }
}
