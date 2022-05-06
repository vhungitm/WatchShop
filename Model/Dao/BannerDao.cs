using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Model.EF;
using PagedList; // Phân trang MVC

namespace Model.Dao
{
    public class BannerDao
    {
        WatchShop db = null;
        public BannerDao()
        {
            db = new WatchShop();
        }

        //Tìm theo id
        public Banner GetByID(long id)
        {
            return db.Banners.SingleOrDefault(x => x.ID == id);
        }

        public List<Banner> ListAll()
        {
            return db.Banners.Where(x => x.Status == true).OrderBy(x => x.DisplayOrder).ToList();
        }

        public IEnumerable<Banner> ListAllPaging(string searchString, int page, int pageSize)
        {
            IQueryable<Banner> Entity = db.Banners;
            if (!string.IsNullOrEmpty(searchString))
            {
                Entity = Entity.Where(x => x.Title.Contains(searchString));
            }
            return Entity.OrderByDescending(x => x.DisplayOrder).ToPagedList(page, pageSize);
        }

        // Thêm mới
        public bool Insert(Banner Entity)
        {
            try
            {
                db.Banners.Add(Entity);
                db.SaveChanges();
                return true;
            }
            catch (Exception) { return false; }
        }

        // Cập nhật
        public bool Update(Banner NewEntity)
        {
            try
            {
                var Entity = db.Banners.Find(NewEntity.ID);

                // Cập nhật
                Entity.Title = NewEntity.Title;
                Entity.Image = NewEntity.Image;
                Entity.Link = NewEntity.Link;
                Entity.DisplayOrder = NewEntity.DisplayOrder;
                Entity.Status = NewEntity.Status;

                db.SaveChanges(); //Lưu

                return true;

            }
            catch (Exception) { return false; }
        }
        // Xóa
        public bool Delete(long id)
        {
            try
            {
                var Entity = db.Banners.SingleOrDefault(x => x.ID == id);
                db.Banners.Remove(Entity);
                db.SaveChanges();

                return true;
            }
            catch (Exception) { return false; }
        }

        // Thay đổi trạng thái
        public bool ChangeStatus(long id)
        {
            var Entity = db.Banners.SingleOrDefault(x => x.ID == id);
            Entity.Status = !Entity.Status;
            db.SaveChanges();

            return Entity.Status;
        }
    }
}
