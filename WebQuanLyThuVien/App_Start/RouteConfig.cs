using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace WebQuanLyThuVien
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            routes.MapRoute(
                name: "Default",
                url: "{controller}/{action}/{id}",
                defaults: new { controller = "Home", action = "Index", id = UrlParameter.Optional },
                namespaces: new[] {"WebQuanLyThuVien.Controllers"} 
            );

            routes.MapRoute(
               name: "ThongTinSach",
               url: "{controller}/{action}/{id}",
               defaults: new { controller = "ThongTinSach", action = "ThongTinSach", id = UrlParameter.Optional }
           );
        }
    }
}
