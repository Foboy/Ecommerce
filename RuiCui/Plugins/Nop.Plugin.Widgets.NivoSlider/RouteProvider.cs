using System.Web.Mvc;
using System.Web.Routing;
using Nop.Web.Framework.Mvc.Routes;

namespace Nop.Plugin.Shipping.ByWeight
{
    public partial class RouteProvider : IRouteProvider
    {
        public void RegisterRoutes(RouteCollection routes)
        {
            routes.MapRoute("Plugin.Widgets.NivoSlider.ConfigureSlider",
                 "Plugins/Index/ConfigureSlider",
                 new { controller = "Index", action = "ConfigureSlider", },
                 new[] { "Nop.Plugin.Widgets.NivoSlider.Controllers" }
            );
            routes.MapRoute("Plugin.Widgets.NivoSlider.ConfigureIndexRight",
                 "Plugins/Index/ConfigureIndexRight",
                 new { controller = "Index", action = "ConfigureIndexRight", },
                 new[] { "Nop.Plugin.Widgets.NivoSlider.Controllers" }
            );
            routes.MapRoute("Plugin.Widgets.NivoSlider.ConfigureRoot",
     "Plugins/Index/ConfigureRoot",
     new { controller = "Index", action = "ConfigureRoot", },
     new[] { "Nop.Plugin.Widgets.NivoSlider.Controllers" }
);
            routes.MapRoute("Plugin.Widgets.NivoSlider.ProductAddPopup",
     "Plugins/Index/ProductAddPopup",
     new { controller = "Index", action = "ProductAddPopup", },
     new[] { "Nop.Plugin.Widgets.NivoSlider.Controllers" }
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
