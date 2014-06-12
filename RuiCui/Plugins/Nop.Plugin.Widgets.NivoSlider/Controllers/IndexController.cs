using System.Web.Mvc;
using Nop.Core;
using Nop.Core.Caching;
using Nop.Plugin.Widgets.NivoSlider.Infrastructure.Cache;
using Nop.Plugin.Widgets.NivoSlider.Models;
using Nop.Services.Configuration;
using Nop.Services.Media;
using Nop.Services.Stores;
using Nop.Web.Framework.Controllers;

namespace Nop.Plugin.Widgets.NivoSlider.Controllers
{
    public class IndexController : BasePluginController
    {
        [AdminAuthorize]
        [ChildActionOnly]
        public ActionResult Configure()
        {
            return View("Nop.Plugin.Widgets.NivoSlider.Views.index");
        }

      
    }
}