using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Model.EF;
using PagedList; //Phân trang MVC

namespace Model.Dao
{
    public class MenuTypeDao
    {
        WatchShop db = null;
        public MenuTypeDao()
        {
            db = new WatchShop();
        }

        // Tìm theo id
        public MenuType GetByID(int id)
        {
            return db.MenuTypes.SingleOrDefault(x => x.ID == id);
        }

        
        // Get all
        public List<MenuType> ListAll()
        {
            return db.MenuTypes.ToList();
        }

        // Get danh sách có phân trang
        public IEnumerable<MenuType> ListAllPaging(string searchString, int page, int pageSize)
        {
            IQueryable<MenuType> Entity = db.MenuTypes;
            if (!string.IsNullOrEmpty(searchString))
            {
                Entity = Entity.Where(x => x.Name.Contains(searchString));
            }
            return Entity.OrderBy(x => x.ID).ToPagedList(page, pageSize);
        }

        // Thêm mới
        public bool Insert(MenuType Entity)
        {
            try
            {
                db.MenuTypes.Add(Entity);
                db.SaveChanges();
                return true;
            }
            catch (Exception) { return false; }
        }

        // Cập nhật
        public bool Update(MenuType NewEntity)
        {
            try
            {
                var Entity = db.MenuTypes.Find(NewEntity.ID);

                // Cập nhật
                Entity.Name = NewEntity.Name;

                db.SaveChanges(); // Lưu

                return true;

            }
            catch (Exception) { return false; }
        }
        // Xóa
        public bool Delete(int id)
        {
            try
            {
                var Entity = db.MenuTypes.SingleOrDefault(x => x.ID == id);
                db.MenuTypes.Remove(Entity);
                db.SaveChanges();

                return true;
            }
            catch (Exception) { return false; }
        }
    }
}
