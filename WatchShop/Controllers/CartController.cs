using Model.Dao;
using Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using System.Web.Script.Serialization;
using Model.EF;
using Common;

namespace WatchShop.Controllers
{
    public class CartController : BaseController{
        // GET: Cart
        private string CartSession = "CartSession";
        public JsonResult DeleteAll()
        {
            Session[CartSession] = null;

            SetAlert("Xóa giỏ hàng thành công!", AlertType.Success);
            return Json(new
            {
                status = true
            });
        }

        public JsonResult Delete(long id)
        {
            try
            {
                var sessionCart = (List<CartItem>)Session[CartSession];

                var ProductName = sessionCart.Find(x => x.Product.ID == id).Product.Name;
                sessionCart.RemoveAll(x => x.Product.ID == id);
                Session[CartSession] = sessionCart;

                SetAlert("Xóa thành công sản phẩm " + ProductName + " khỏi giỏ hàng!", AlertType.Success);
                return Json(new
                {
                    status = true
                });
            }
            catch (Exception) {
                return Json(new
                {
                    status = false
                });
            }
        }
        public JsonResult Update(string cartModel)
        {
            string errorMess = "";

            var jsonCart = new JavaScriptSerializer().Deserialize<List<CartItem>>(cartModel);
            var sessionCart = (List<CartItem>)Session[CartSession];

            foreach (var item in sessionCart)
            {
                var jsonItem = jsonCart.SingleOrDefault(x => x.Product.ID == item.Product.ID);
                if (jsonItem != null)
                {
                    if (jsonItem.Quantity < item.Product.Quantity)
                    {
                        item.Quantity = jsonItem.Quantity;
                    }
                    else {
                        errorMess += ", " + item.Product.Name;
                    }
                }
            }

            // Thông báo
            if (errorMess != "")
            {
                SetAlert("Số lượng sản phẩm " + errorMess.Substring(2) + " đã đặt vượt quá số lượng sản phẩm trong kho", AlertType.Error);
            }
            else
            {
                SetAlert("Cập nhật giỏ hàng thành công!", AlertType.Success);
            }

            Session[CartSession] = sessionCart;
            return Json(new
            {
                status = true
            });
        }
        public ActionResult Index()
        {
            var cart = Session[CartSession];
            var list = new List<CartItem>();
            if (cart != null)
            {
                list = (List<CartItem>)cart;
            }
            return View(list);
        }
        public ActionResult AddItem(long productId, int quantity)
        {
            var product = new ProductDao().GetByID(productId);
            var cart = Session[CartSession];
            if (cart != null)
            {
                var list = (List<CartItem>)cart;
                if (list.Exists(x => x.Product.ID == productId))
                {

                    foreach (var item in list)
                    {
                        if (item.Product.ID == productId)
                        {
                            item.Quantity += quantity;
                        }
                    }
                }
                else
                {
                    //tạo mới đối tượng cart item
                    var item = new CartItem();
                    item.Product = product;
                    item.Quantity = quantity;
                    list.Add(item);
                }
                //Gán vào session
                Session[CartSession] = list;
            }
            else
            {
                //tạo mới đối tượng cart item
                var item = new CartItem();
                item.Product = product;
                item.Quantity = quantity;
                var list = new List<CartItem>();
                list.Add(item);
                //Gán vào session
                Session[CartSession] = list;
            }
            return RedirectToAction("Index");
        }
        [HttpGet]
        public ActionResult Payment()
        {
            var user = Session[CommonConstants.USER_SESSION];
            var cart = Session[CartSession];

            if (cart != null)
            {
                if (user == null)
                {
                    return RedirectToAction("Login", "User");
                }
                else
                {
                    var list = (List<CartItem>)cart;

                    return View(list);
                }    
            }

            return Redirect("/404");
        }

        [HttpPost]
        public ActionResult Payment(string shipName, string mobile, string address, string email)
        {
            var productDao = new ProductDao();

            var order = new Order();
            order.CreatedDate = DateTime.Now;
            order.ShipAddress = address;
            order.ShipMobile = mobile;
            order.ShipName = shipName;
            order.ShipEmail = email;

            var session = (User)Session[Common.CommonConstants.USER_SESSION];
            if (session != null)
                order.CustomerID = session.ID;
            try
            {
                var id = new OrderDao().Insert(order);
                var cart = (List<CartItem>)Session[CartSession];
                var detailDao = new Model.Dao.OrderDetailDao();
                decimal total = 0;
                foreach (var item in cart)
                {
                    var orderDetail = new OrderDetail();
                    orderDetail.ProductID = item.Product.ID;
                    orderDetail.OrderID = id;

                    orderDetail.Quantity = item.Quantity;
                    if(!productDao.ReduceQuantity(orderDetail.ProductID, orderDetail.Quantity)){
                        return Redirect("/404/Index.html");
                    }

                    orderDetail.Price = (item.Product.PromotionPrice == 0) ? (item.Product.Price * item.Quantity) : (item.Product.PromotionPrice * item.Quantity);
                    detailDao.Insert(orderDetail);
                    total += orderDetail.Price;
                }

                Session[CartSession] = null;
            }
            catch
            {

                return Redirect("/loi-thanh-toan");
            }
            return Redirect("/hoan-thanh");
        }

        public ActionResult Success()
        {
            return View();
        }

        public ActionResult Error()
        {
            return View();
        }
    }
}