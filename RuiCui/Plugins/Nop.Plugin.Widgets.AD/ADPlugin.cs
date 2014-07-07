using System.Collections.Generic;
using System.IO;
using System.Web.Routing;
using Nop.Core;
using Nop.Core.Plugins;
using Nop.Services.Cms;
using Nop.Services.Configuration;
using Nop.Services.Localization;
using Nop.Services.Media;

namespace Nop.Plugin.Widgets.AD
{
    /// <summary>
    /// PLugin
    /// </summary>
    public class ADPlugin : BasePlugin, IWidgetPlugin
    {
        private readonly IPictureService _pictureService;
        private readonly ISettingService _settingService;
        private readonly IWebHelper _webHelper;

        public ADPlugin(IPictureService pictureService, 
            ISettingService settingService, IWebHelper webHelper)
        {
            this._pictureService = pictureService;
            this._settingService = settingService;
            this._webHelper = webHelper;
        }

        /// <summary>
        /// Gets widget zones where this widget should be rendered
        /// </summary>
        /// <returns>Widget zones</returns>
        public IList<string> GetWidgetZones()
        {
            return new List<string>() { "product_list_right" };
        }

        /// <summary>
        /// Gets a route for provider configuration
        /// </summary>
        /// <param name="actionName">Action name</param>
        /// <param name="controllerName">Controller name</param>
        /// <param name="routeValues">Route values</param>
        public void GetConfigurationRoute(out string actionName, out string controllerName, out RouteValueDictionary routeValues)
        {
            actionName = "Configure";
            controllerName = "AD";
            routeValues = new RouteValueDictionary() { { "Namespaces", "Nop.Plugin.Widgets.AD.Controllers" }, { "area", null } };
        }

        /// <summary>
        /// Gets a route for displaying widget
        /// </summary>
        /// <param name="widgetZone">Widget zone where it's displayed</param>
        /// <param name="actionName">Action name</param>
        /// <param name="controllerName">Controller name</param>
        /// <param name="routeValues">Route values</param>
        public void GetDisplayWidgetRoute(string widgetZone, out string actionName, out string controllerName, out RouteValueDictionary routeValues)
        {
            actionName = "";
            controllerName = "";
            routeValues = new RouteValueDictionary()
            {
                {"Namespaces", "Nop.Plugin.Widgets.AD.Controllers"},
                {"area", null},
                {"widgetZone", widgetZone}
            };
            switch (widgetZone)
            {
                case "product_list_right":
                    actionName = "PublicInfo";
                       controllerName = "AD";
                    break;
              
            }
         
        }
        
        /// <summary>
        /// Install plugin
        /// </summary>
        public override void Install()
        {
            //pictures
            var sampleImagesPath = _webHelper.MapPath("~/Plugins/Widgets.AD/Content/AD/sample-images/");


            //settings
            var settings = new ADSettings();
            _settingService.SaveSetting(settings);


            this.AddOrUpdatePluginLocaleResource("Plugins.Widgets.AD.Picture1", "Í¼Æ¬1");
            this.AddOrUpdatePluginLocaleResource("Plugins.Widgets.AD.Picture2", "Í¼Æ¬2");
            this.AddOrUpdatePluginLocaleResource("Plugins.Widgets.AD.Picture", "Í¼Æ¬");
            this.AddOrUpdatePluginLocaleResource("Plugins.Widgets.AD.Picture.Hint", "ÉÏ´«Í¼Æ¬");
            this.AddOrUpdatePluginLocaleResource("Plugins.Widgets.AD.Text", "±êÌâ");
            this.AddOrUpdatePluginLocaleResource("Plugins.Widgets.AD.Text.Hint", "");
            this.AddOrUpdatePluginLocaleResource("Plugins.Widgets.AD.Link", "URL");
            this.AddOrUpdatePluginLocaleResource("Plugins.Widgets.AD.Link.Hint", "");


            base.Install();
        }

        /// <summary>
        /// Uninstall plugin
        /// </summary>
        public override void Uninstall()
        {
            //settings
            _settingService.DeleteSetting<ADSettings>();

            //locales
            this.DeletePluginLocaleResource("Plugins.Widgets.AD.Picture1");
            this.DeletePluginLocaleResource("Plugins.Widgets.AD.Picture2");
            this.DeletePluginLocaleResource("Plugins.Widgets.AD.Picture");
            this.DeletePluginLocaleResource("Plugins.Widgets.AD.Picture.Hint");
            this.DeletePluginLocaleResource("Plugins.Widgets.AD.Text");
            this.DeletePluginLocaleResource("Plugins.Widgets.AD.Text.Hint");
            this.DeletePluginLocaleResource("Plugins.Widgets.AD.Link");
            this.DeletePluginLocaleResource("Plugins.Widgets.AD.Link.Hint");
            
            base.Uninstall();
        }
    }
}
