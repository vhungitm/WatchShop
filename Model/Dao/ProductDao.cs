﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Model.EF;
using PagedList; // Phân trang MVC
using Model.ViewModel;

namespace Model.Dao
{
    public class ProductDao
    {
        WatchShop db = null;
        public ProductDao()
        {
            db = new WatchShop();
        }

        // Tìm theo id
        public Product GetByID(long id)
        {
            return db.Products.SingleOrDefault(x => x.ID == id);
        }

        // get theo danh mục
        public IEnumerable<Product> GetByCategoryId(long categoryId, int page, int pageSize)
        {
            IQueryable<Product> Entity = db.Products;
            return Entity.Where(x => x.CategoryID == categoryId && x.Status == true).OrderByDescending(x => x.CreatedDate).ToPagedList(page,pageSize);
        }

        public List<string> ListName(string keyword)
        {
            return db.Products.Where(x => x.Name.Contains(keyword)).Select(x => x.Name).ToList();
        }

        public List<ProductViewModel> Search(string keyword, ref int totalRecord, int pageIndex = 1, int pageSize = 2)
        {
            totalRecord = db.Products.Where(x => x.Name == keyword).Count();
            var model = (from a in db.Products
                         join b in db.ProductCategories
                         on a.CategoryID equals b.ID
                         where a.Name.Contains(keyword)
                         select new
                         {
                             CateMetaTitle = b.MetaTitle,
                             CateName = b.Name,
                             CreatedDate = a.CreatedDate,
                             ID = a.ID,
                             Images = a.Image,
                             Name = a.Name,
                             MetaTitle = a.MetaTitle,
                             Price = a.Price,
                             PromotionPrice = a.PromotionPrice
                         }).AsEnumerable().Select(x => new ProductViewModel()
                         {
                             CateMetaTitle = x.MetaTitle,
                             CateName = x.Name,
                             CreatedDate = x.CreatedDate,
                             ID = x.ID,
                             Images = x.Images,
                             Name = x.Name,
                             MetaTitle = x.MetaTitle,
                             Price = x.Price,
                             PromotionPrice = x.PromotionPrice
                         });
            model.OrderByDescending(x => x.CreatedDate).Skip((pageIndex - 1) * pageSize).Take(pageSize);
            return model.ToList();
        }

        public List<Product> ListNewProduct(int top)
        {
            return db.Products.OrderByDescending(x => x.CreatedDate).Where(x => x.Status == true).Take(top).ToList();
        }

        public List<Product> ListFeatureProduct(int top)
        {
            return db.Products.Where(x => x.Status == true && x.TopHot != null && x.TopHot > DateTime.Now).OrderByDescending(x => x.CreatedDate).Take(top).ToList();
        }

        // Get danh sách danh mục có phân trang
        public IEnumerable<Product> ListAllPaging(string searchString, int page, int pageSize)
        {
            IQueryable<Product> Entity = db.Products;
            if (!string.IsNullOrEmpty(searchString))
            {
                Entity = Entity.Where(x => x.Name.Contains(searchString));
            }
            return Entity.OrderByDescending(x => x.CreatedDate).ToPagedList(page, pageSize);
        }

        //Thêm mới
        public bool Insert(Product Entity)
        {
            try
            {
                db.Products.Add(Entity);
                db.SaveChanges();
                return true;
            }
            catch (Exception) { return false; }
        }

        // Cập nhật
        public bool Update(Product NewEntity)
        {
            try
            {
                var Entity = db.Products.Find(NewEntity.ID);

                // Cập nhật
                Entity.Name = NewEntity.Name;
                Entity.MetaTitle = NewEntity.MetaTitle;
                Entity.Description = NewEntity.Description;
                Entity.Image = NewEntity.Image;
                Entity.MoreImages = NewEntity.MoreImages;
                Entity.Price = NewEntity.Price;
                Entity.PromotionPrice = NewEntity.PromotionPrice;
                Entity.Quantity = NewEntity.Quantity;
                Entity.CategoryID = NewEntity.CategoryID;
                Entity.Detail = NewEntity.Detail;
                Entity.Warranty = NewEntity.Warranty;
                Entity.ModifiedDate = NewEntity.ModifiedDate;
                Entity.ModifiedBy = NewEntity.ModifiedBy;
                Entity.MetaDescriptions = NewEntity.MetaDescriptions;
                Entity.MetaKeywords = NewEntity.MetaKeywords;
                Entity.Status = NewEntity.Status;
                Entity.TopHot = NewEntity.TopHot;

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
                var Entity = db.Products.SingleOrDefault(x => x.ID == id);
                db.Products.Remove(Entity);
                db.SaveChanges();

                return true;
            }
            catch (Exception) { return false; }
        }

        // Thay đổi trạng thái danh mục
        public bool ChangeStatus(long id)
        {
            var Entity = db.Products.SingleOrDefault(x => x.ID == id);
            Entity.Status = !Entity.Status;
            db.SaveChanges();

            return Entity.Status;
        }
        public int GetQuantity(long id)
        {
            return db.Products.Find(id).Quantity;
        }
        public bool ReduceQuantity(long id, int quantity)
        {
            try
            {
                var Entity = db.Products.Find(id);
                Entity.Quantity -= quantity;
                db.SaveChanges();
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }
    }
}
