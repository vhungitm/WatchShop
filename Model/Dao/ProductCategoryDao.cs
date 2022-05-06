using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Model.EF;
using PagedList;
using Model.ViewModel;

namespace Model.Dao
{
    public class ProductCategoryDao
    {
        WatchShop db = null;
        public ProductCategoryDao()
        {
            db = new WatchShop();
        }

        //Tìm theo id
        public ProductCategory GetByID(long id)
        {
            return db.ProductCategories.SingleOrDefault(x => x.ID == id);
        }

        public List<ProductCategory> ListAll()
        {
            return db.ProductCategories.Where(x => x.Status == true).OrderBy(x => x.DisplayOrder).ToList();
        }

        //Get danh sách danh mục có phân trang
        public IEnumerable<ProductCategoryViewModel> ListAllPaging(string searchString, int page, int pageSize)
        {
            var model = 
                from a in db.ProductCategories
                join b in db.ProductCategories
                on a.ParentID equals b.ID into g
                from c in g.DefaultIfEmpty()
                where (
                    string.IsNullOrEmpty(searchString) 
                        ? true 
                        : a.Name.Contains(searchString)
                )
                select new ProductCategoryViewModel
                {
                    ID = a.ID,
                    Name = a.Name,
                    MetaTitle = a.MetaTitle,
                    ParentID = a.ParentID,
                    ParentName = c.Name ?? string.Empty,
                    DisplayOrder = a.DisplayOrder,
                    CreatedDate = a.CreatedDate,
                    CreatedBy = a.CreatedBy,
                    ModifiedDate = a.ModifiedDate,
                    ModifiedBy = a.ModifiedBy,
                    MetaKeywords = a.MetaKeywords,
                    MetaDescriptions = a.MetaDescriptions,
                    Status = a.Status,
                    ShowOnHome = a.ShowOnHome
                };

            return model.OrderByDescending(x => x.CreatedDate).ToPagedList(page, pageSize);
        }

        //Thêm mới
        public bool Insert(ProductCategory Entity)
        {
            try
            {
                db.ProductCategories.Add(Entity);
                db.SaveChanges();
                return true;
            }
            catch (Exception) { return false; }
        }

        //Cập nhật
        public bool Update(ProductCategory NewEntity)
        {
            try
            {
                var Entity = db.ProductCategories.Find(NewEntity.ID);

                //Cập nhật
                Entity.Name = NewEntity.Name;
                Entity.MetaTitle = NewEntity.MetaTitle;
                Entity.ParentID = NewEntity.ParentID;
                Entity.DisplayOrder = NewEntity.DisplayOrder;
                Entity.ModifiedDate = NewEntity.ModifiedDate;
                Entity.ModifiedBy = NewEntity.ModifiedBy;
                Entity.MetaKeywords = NewEntity.MetaKeywords;
                Entity.MetaDescriptions = NewEntity.MetaDescriptions;
                Entity.Status = NewEntity.Status;
                Entity.ShowOnHome = NewEntity.ShowOnHome;

                db.SaveChanges(); //Lưu

                return true;

            }
            catch (Exception) { return false; }
        }
        //Xóa
        public bool Delete(long id)
        {
            try
            {
                var Entity = db.ProductCategories.SingleOrDefault(x => x.ID == id);
                db.ProductCategories.Remove(Entity);
                db.SaveChanges();

                return true;
            }
            catch (Exception) { return false; }
        }

        //Thay đổi trạng thái danh mục
        public bool ChangeStatus(long id)
        {
            var Entity = db.ProductCategories.SingleOrDefault(x => x.ID == id);
            Entity.Status = !Entity.Status;
            db.SaveChanges();

            return Entity.Status;
        }

        //Thay đổi trạng thái hiển thị trên trang chủ
        public bool ChangeShowOnHome(long id)
        {
            var Entity = db.ProductCategories.SingleOrDefault(x => x.ID == id);
            Entity.ShowOnHome = !Entity.ShowOnHome;
            db.SaveChanges();

            return Entity.ShowOnHome;
        }
    }
}
