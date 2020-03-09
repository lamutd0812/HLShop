using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace HLShop.Web
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            //BotDetect requests must not be routed
            routes.IgnoreRoute("{*botdetect}", new { botdetect = @"(.*)BotDetectCaptcha\.ashx" });

            //routes.MapRoute(
            //    name: "About",
            //    url: "gioi-thieu.html",
            //    defaults: new { controller = "About", action = "Index", id = UrlParameter.Optional },
            //    namespaces:new string[] { "HLShop.Web.Controllers" }
            //);

            //login
            routes.MapRoute(
                name: "Login",
                url: "dang-nhap.html",
                defaults: new { controller = "Account", action = "Login", id = UrlParameter.Optional },
                namespaces: new string[] { "HLShop.Web.Controllers" }
            );

            //register account
            routes.MapRoute(
                name: "Register",
                url: "dang-ky.html",
                defaults: new { controller = "Account", action = "Index", id = UrlParameter.Optional },
                namespaces: new string[] { "HLShop.Web.Controllers" }
            );

            //recover
            routes.MapRoute(
                name: "Recover",
                url: "quen-mat-khau.html",
                defaults: new { controller = "Account", action = "RecoverPassword", id = UrlParameter.Optional },
                namespaces: new string[] { "HLShop.Web.Controllers" }
            );

            //change password
            routes.MapRoute(
                name: "ChangePassword",
                url: "doi-mat-khau.hmtl",
                defaults: new { controller = "Account", action = "ChangePassword", id = UrlParameter.Optional },
                namespaces: new string[] { "HLShop.Web.Controllers" }
            );

            // cart
            routes.MapRoute(
                name: "Cart",
                url: "gio-hang.html",
                defaults: new { controller = "Cart", action = "Index", id = UrlParameter.Optional },
                namespaces: new string[] { "HLShop.Web.Controllers" }
            );

            // checkout
            routes.MapRoute(
                name: "Checkout",
                url: "thanh-toan.html",
                defaults: new { controller = "Cart", action = "Checkout", id = UrlParameter.Optional },
                namespaces: new string[] { "HLShop.Web.Controllers" }
            );

            // product category
            routes.MapRoute(
                name: "Product Category",
                url: "{alias}.pc-{id}.html",
                defaults: new { controller = "Product", action = "Category", id = UrlParameter.Optional },
                namespaces: new string[] { "HLShop.Web.Controllers" }
            );

            // all product
            routes.MapRoute(
                name: "All Product",
                url: "san-pham.html",
                defaults: new { controller = "Product", action = "GetAllProduct", id = UrlParameter.Optional },
                namespaces: new string[] { "HLShop.Web.Controllers" }
            );

            // product detail 
            routes.MapRoute(
                name: "Product",
                url: "{alias}.p-{productId}.html",
                defaults: new { controller = "Product", action = "Detail", productId = UrlParameter.Optional },
                namespaces: new string[] { "HLShop.Web.Controllers" }
            );

            // search
            routes.MapRoute(
                name: "Search",
                url: "tim-kiem.html",
                defaults: new { controller = "Product", action = "Search",id = UrlParameter.Optional },
                namespaces: new string[] { "HLShop.Web.Controllers" }
            );

            //taglist
            routes.MapRoute(
                name: "TagList",
                url: "tag/{tagId}.html",
                defaults: new { controller = "Product", action = "GetListProductByTag", tagId = UrlParameter.Optional },
                namespaces: new string[] { "HLShop.Web.Controllers" }
            );

            // other pages
            routes.MapRoute(
                name: "Page",
                url: "trang/{alias}.html",
                defaults: new { controller = "Page", action = "Index", alias= UrlParameter.Optional },
                namespaces: new string[] { "HLShop.Web.Controllers" }
            );

            //trang contact
            routes.MapRoute(
                name: "Contact",
                url: "lien-he.html",
                defaults: new { controller = "Contact", action = "Index", id = UrlParameter.Optional },
                namespaces: new string[] { "HLShop.Web.Controllers" }
            );

            // default
            routes.MapRoute(
                name: "Default",
                url: "{controller}/{action}/{id}",
                defaults: new { controller = "Home", action = "Index", id = UrlParameter.Optional },
                namespaces: new string[] { "HLShop.Web.Controllers" }
            );
        }
    }
}
