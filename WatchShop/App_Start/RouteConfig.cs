using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace WatchShop
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            routes.IgnoreRoute("{*botdetect}",
            new { botdetect = @"(.*)BotDetectCaptcha\.ashx" });

            

            routes.MapRoute(
                name: "Login",
                url: "Dang-nhap",
                defaults: new { controller = "User", action = "Login", id = UrlParameter.Optional },
                namespaces: new[] { "WatchShop.Controllers" }
            );

            routes.MapRoute(
                name: "All Product",
                url: "San-pham",
                defaults: new { controller = "Product", action = "Index", id = UrlParameter.Optional },
                namespaces: new[] { "WatchShop.Controllers" }
            );
            routes.MapRoute(
                name: "Product Category",
                url: "San-pham/{metatitle}-{CategoryId}",
                defaults: new { controller = "Product", action = "ProductCategory", id = UrlParameter.Optional },
                namespaces: new[] { "WatchShop.Controllers" }
            );
            routes.MapRoute(
                name: "Product Detail",
                url: "Chi-tiet/{metatitle}-{Id}",
                defaults: new { controller = "Product", action = "Detail", id = UrlParameter.Optional },
                namespaces: new[] { "WatchShop.Controllers" }
            );

            routes.MapRoute(
                name: "Contact",
                url: "Lien-he",
                defaults: new { controller = "Contact", action = "Index", id = UrlParameter.Optional },
                namespaces: new[] { "WatchShop.Controllers" }
            );

            routes.MapRoute(
                name: "Register",
                url: "Dang-ky",
                defaults: new { controller = "User", action = "Register", id = UrlParameter.Optional },
                namespaces: new[] { "WatchShop.Controllers" }
            );

            routes.MapRoute(
                name: "Content",
                url: "Tin-tuc",
                defaults: new { controller = "Content", action = "Index", id = UrlParameter.Optional },
                namespaces: new[] { "WatchShop.Controllers" }
            );

            routes.MapRoute(
                name: "Content Detail",
                url: "Tin-tuc/{MetaTitle}-{id}",
                defaults: new { controller = "Content", action = "Detail", id = UrlParameter.Optional },
                namespaces: new[] { "WatchShop.Controllers" }
            );

            routes.MapRoute(
             name: "Search",
             url: "tim-kiem",
             defaults: new { controller = "Product", action = "Search", id = UrlParameter.Optional },
             namespaces: new[] { "WatchShop.Controllers" }
            );
            routes.MapRoute(
              name: "Cart",
              url: "gio-hang",
              defaults: new { controller = "Cart", action = "Index", id = UrlParameter.Optional },
              namespaces: new[] { "WatchShop.Controllers" }
            );

            routes.MapRoute(
              name: "Add Cart",
              url: "them-gio-hang",
              defaults: new { controller = "Cart", action = "AddItem", id = UrlParameter.Optional },
              namespaces: new[] { "WatchShop.Controllers" }
            );

            routes.MapRoute(
              name: "Payment",
              url: "thanh-toan",
              defaults: new { controller = "Cart", action = "Payment", id = UrlParameter.Optional },
              namespaces: new[] { "WatchShop.Controllers" }
            );

            routes.MapRoute(
              name: "Payment Success",
              url: "hoan-thanh",
              defaults: new { controller = "Cart", action = "Success", id = UrlParameter.Optional },
              namespaces: new[] { "WatchShop.Controllers" }
            );
            routes.MapRoute(
              name: "Payment Error",
              url: "loi-thanh-toan",
              defaults: new { controller = "Cart", action = "Error", id = UrlParameter.Optional },
              namespaces: new[] { "WatchShop.Controllers" }
            );

            routes.MapRoute(
                name: "Default",
                url: "{controller}/{action}/{id}",
                defaults: new { controller = "Home", action = "Index", id = UrlParameter.Optional },
                namespaces: new[] { "WatchShop.Controllers" }
            );
        }
    }
}
