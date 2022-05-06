using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Model.EF;
using Model.ViewModel;

namespace Model.Dao
{
    public class StatisticDao
    {
        WatchShop db = null;
        public StatisticDao()
        {
            db = new WatchShop();
        }


        public enum FilterType
        {
            Quantity, Price, Day, Month, Year
        }
        // Sản phẩm mua nhiều nhất
        public List<ProductViewModel> TopProduct(DateTime date, FilterType timeType, FilterType topType, int top)
        {
            var Order = from a in db.Orders
                        join b in db.OrderDetails on a.ID equals b.OrderID
                        select new
                        {
                            ProductID = b.ProductID,
                            Quantity = b.Quantity,
                            Price = b.Price,
                            CreatedDate = a.CreatedDate,
                        };

            // Thời gian
            if (timeType == FilterType.Month)
            {
                Order = Order.Where(x => x.CreatedDate.Value.Month == date.Month && x.CreatedDate.Value.Year == date.Year);
            }
            else if (timeType == FilterType.Year)
            {
                Order = Order.Where(x => x.CreatedDate.Value.Year == date.Year);
            }

            var Entity = from a in Order
                         group a by a.ProductID into g
                         select new ProductViewModel
                         {
                             ID = g.Key,
                             Name = (from gg in g join product in db.Products on gg.ProductID equals product.ID select product.Name).FirstOrDefault(),
                             Image = (from gg in g join product in db.Products on gg.ProductID equals product.ID select product.Image).FirstOrDefault(),
                             Quantity = g.Sum(x => x.Quantity),
                             Price = g.Sum(x => x.Price),
                             CreatedDate = (from gg in g select gg.CreatedDate).FirstOrDefault(),
                         };


            // Số lượng/Tổng tiền
            if (topType == FilterType.Quantity)
            {
                Entity = Entity.OrderByDescending(x => x.Quantity);
            }
            else if (topType == FilterType.Price)
            {
                Entity = Entity.OrderByDescending(x => x.Quantity);
            }

            return Entity.Take(top).ToList();
        }

        public List<Decimal> GetPrice(DateTime date, FilterType typeTime)
        {
            var Order = from a in db.Orders
                        join b in db.OrderDetails on a.ID equals b.OrderID
                        select new
                        {
                            ID = a.ID,
                            Price = b.Price,
                            CreatedDate = a.CreatedDate,
                        };
            List<decimal> Price = new List<decimal>();
            if (typeTime == FilterType.Month)
            {
                Order = Order.Where(x => x.CreatedDate.Value.Month == date.Month && x.CreatedDate.Value.Year == date.Year);

                int Day = DateTime.DaysInMonth(date.Year, date.Month);
                for (int i = 1; i <= Day; i++)
                {
                    Price.Add(0);
                    var listDayPrice = Order.Where(x => x.CreatedDate.Value.Day == i).ToList();
                    foreach (var j in listDayPrice)
                    {
                        Price[i - 1] += j.Price;
                    }
                }
            }
            else if (typeTime == FilterType.Year){
                Order = Order.Where(x => x.CreatedDate.Value.Year == date.Year);
                for (int i = 1; i < 13; i++)
                {
                    Price.Add(0);
                    var listMonthPrice = Order.Where(x => x.CreatedDate.Value.Month == i).ToList();
                    foreach (var j in listMonthPrice)
                    {
                        Price[i - 1] += j.Price;
                    }
                }
            }
            return Price;
        }
    }
}
