using Model.EF;
using System;
using System.Collections.Generic;
using System.Linq;
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
            return db.Users.SingleOrDefault(x => x.Username == username || x.Phone == username || x.Email == username);
        }

        public User GetByPhone(string phone)
        {
            return db.Users.SingleOrDefault(x => x.Phone == phone);
        }

        public User GetByEmail(string email)
        {
            return db.Users.SingleOrDefault(x => x.Email == email);
        }

        public Response Login(Login model)
        {
            var password = Encryptor.MD5Hash(model.Password);
            var result = db.Users.SingleOrDefault(x => 
                (x.Username == model.Username || x.Email == model.Username || x.Phone == model.Username) &&
                (x.Password == password) &&
                (model.GroupID == CommonConstants.ADMIN_GROUP ? (x.GroupID == model.GroupID) : true)
            );
            var response = new Response();

            if (result != null)
            {
                if (result.Status)
                {
                    response.Status = true;
                    response.Message = "Đăng nhập thành công!";

                    return response;
                }
                else
                {
                    response.Status = false;
                    response.Message = "Tài khoản của bạn đã bị khóa! Vui lòng liên hệ với Admin để được mở lại!";

                    return response;
                }
            }
            else
            {
                response.Status = false;
                response.Message = "Sai tên tài khoản hoặc mật khẩu!";

                return response;
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
            catch { return false; }
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
                Entity.Password = NewEntity.Password != null ? NewEntity.Password : Entity.Password;
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
            catch { return false; }
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
