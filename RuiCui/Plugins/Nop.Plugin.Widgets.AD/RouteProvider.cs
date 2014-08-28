using System.Web.Mvc;
using System.Web.Routing;
using Nop.Web.Framework.Mvc.Routes;

namespace Nop.Plugin.Widgets.AD
{
    public partial class RouteProvider : IRouteProvider
    {
        public void RegisterRoutes(RouteCollection routes)
        {
            routes.MapRoute("Plugin.Widgets.AD.Configure",
                 "Plugins/Index/ConfigureAD",
                 new { controller = "Index", action = "Configure", },
                 new[] { "Nop.Plugin.Widgets.AD.Controllers" }
            );
           
        }
        public int Priority
        {
            get
            {
                return 0;
            }
        }
    }
}
