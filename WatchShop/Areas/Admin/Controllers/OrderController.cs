using System.Web.Mvc;
using Model.Dao;
using WatchShop.Utils;

namespace WatchShop.Areas.Admin.Controllers
{
    public class OrderController : BaseController
    {
        [HasCredential(RoleID = "VIEW_ORDER")]
        public ActionResult Index(string searchString, int page = 1, int pageSize = 5)
        {
            var entities = new OrderDao().ListAllPaging(searchString, page, pageSize);
            ViewBag.SearchString = searchString;

            return View(entities);
        }

        // Thay đổi trạng thái thanh toán
        public ActionResult ChangeStatus(long id)
        {
            var result = new OrderDao().ChangeStatus(id);
            if (result)
            {
                SetAlert("Thay đổi trạng thái đơn hàng thành công!", AlertType.Success);
            }
            else
            {
                SetAlert("Thay đổi trạng thái đơn hàng không thành công!", AlertType.Error);
            }
            return Redirect("/Admin/Order/Index");
        }
	}
}