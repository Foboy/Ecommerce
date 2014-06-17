using System.Collections.Generic;
using System.IO;
using System.Web.Routing;
using Nop.Core;
using Nop.Core.Plugins;
using Nop.Services.Cms;
using Nop.Services.Configuration;
using Nop.Services.Localization;
using Nop.Services.Media;

namespace Nop.Plugin.Widgets.NivoSlider
{
    /// <summary>
    /// PLugin
    /// </summary>
    public class NivoSliderPlugin : BasePlugin, IWidgetPlugin
    {
        private readonly IPictureService _pictureService;
        private readonly ISettingService _settingService;
        private readonly IWebHelper _webHelper;

        public NivoSliderPlugin(IPictureService pictureService, 
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
            return new List<string>() { "home_page_top", "home_page_right", "home_page_root", "home_page_right2", "home_page_hot", "home_page_new", "home_page_star", "home_page_like" };
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
            controllerName = "Index";
            routeValues = new RouteValueDictionary() { { "Namespaces", "Nop.Plugin.Widgets.NivoSlider.Controllers" }, { "area", null } };
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
                {"Namespaces", "Nop.Plugin.Widgets.NivoSlider.Controllers"},
                {"area", null},
                {"widgetZone", widgetZone}
            };
            switch (widgetZone)
            {
                case "home_page_top":
                       actionName = "PublicInfoSlider";
                       controllerName = "Index";
                    break;
                case "home_page_right":
                    actionName = "PublicInfoIndexRight";
                    controllerName = "Index";
                    break;
                case "home_page_right2":
                    actionName = "PublicInfoIndexRight2";
                    controllerName = "Index";
                    break;
                case "home_page_root":
                    actionName = "PublicInfoRoot";
                    controllerName = "Index";
                    break;
                case "home_page_new":
                    actionName = "PublicInfoNew";
                    controllerName = "Index";
                    break;
                case "home_page_hot":
                    actionName = "PublicInfoHot";
                    controllerName = "Index";
                    break;
                case "home_page_star":
                    actionName = "PublicInfoStar";
                    controllerName = "Index";
                    break;
                case "home_page_like":
                    actionName = "PublicInfoLike";
                    controllerName = "Index";
                    break;
            }
         
        }
        
        /// <summary>
        /// Install plugin
        /// </summary>
        public override void Install()
        {
            //pictures
            var sampleImagesPath = _webHelper.MapPath("~/Plugins/Widgets.NivoSlider/Content/nivoslider/sample-images/");


            //settings
            var settings = new NivoSliderSettings()
            {
                Picture1Id = _pictureService.InsertPicture(File.ReadAllBytes(sampleImagesPath + "banner1.jpg"), "image/pjpeg", "banner_1", true).Id,
                Text1 = "",
                Link1 = _webHelper.GetStoreLocation(false),
                Picture2Id = _pictureService.InsertPicture(File.ReadAllBytes(sampleImagesPath + "banner2.jpg"), "image/pjpeg", "banner_2", true).Id,
                Text2 = "",
                Link2 = _webHelper.GetStoreLocation(false),
                Picture3Id = _pictureService.InsertPicture(File.ReadAllBytes(sampleImagesPath + "banner3.jpg"), "image/pjpeg", "banner_3", true).Id,
                Text3 = "",
                Link3 = _webHelper.GetStoreLocation(false),
                //Picture4Id = _pictureService.InsertPicture(File.ReadAllBytes(sampleImagesPath + "banner4.jpg"), "image/pjpeg", "banner_4", true).Id,
                //Text4 = "",
                //Link4 = _webHelper.GetStoreLocation(false),
            };
            _settingService.SaveSetting(settings);


    
            //NivoIndexRightSettings
            var settingsIndexRight = new NivoIndexRightSettings()
            {
                Picture1Id = _pictureService.InsertPicture(File.ReadAllBytes(sampleImagesPath + "banner1.jpg"), "image/pjpeg", "banner_1", true).Id,
                Text1 = "",
                Link1 = _webHelper.GetStoreLocation(false),
                Picture2Id = _pictureService.InsertPicture(File.ReadAllBytes(sampleImagesPath + "banner2.jpg"), "image/pjpeg", "banner_2", true).Id,
                Text2 = "",
                Link2 = _webHelper.GetStoreLocation(false),
                Picture3Id = _pictureService.InsertPicture(File.ReadAllBytes(sampleImagesPath + "banner3.jpg"), "image/pjpeg", "banner_3", true).Id,
                Text3 = "",
                Link3 = _webHelper.GetStoreLocation(false),
                //Picture4Id = _pictureService.InsertPicture(File.ReadAllBytes(sampleImagesPath + "banner4.jpg"), "image/pjpeg", "banner_4", true).Id,
                //Text4 = "",
                //Link4 = _webHelper.GetStoreLocation(false),
            };
            _settingService.SaveSetting(settingsIndexRight);


     
            //NivoIndexSliderRootSettings
            var settingsNivoIndexSliderRoot = new NivoIndexSliderRootSettings()
            {
                Picture1Id = _pictureService.InsertPicture(File.ReadAllBytes(sampleImagesPath + "banner1.jpg"), "image/pjpeg", "banner_1", true).Id,
                Text1 = "",
                Link1 = _webHelper.GetStoreLocation(false),
                Picture2Id = _pictureService.InsertPicture(File.ReadAllBytes(sampleImagesPath + "banner2.jpg"), "image/pjpeg", "banner_2", true).Id,
                Text2 = "",
                Link2 = _webHelper.GetStoreLocation(false),
                Picture3Id = _pictureService.InsertPicture(File.ReadAllBytes(sampleImagesPath + "banner3.jpg"), "image/pjpeg", "banner_3", true).Id,
                Text3 = "",
                Link3 = _webHelper.GetStoreLocation(false),
                //Picture4Id = _pictureService.InsertPicture(File.ReadAllBytes(sampleImagesPath + "banner4.jpg"), "image/pjpeg", "banner_4", true).Id,
                //Text4 = "",
                //Link4 = _webHelper.GetStoreLocation(false),
            };
            _settingService.SaveSetting(settingsNivoIndexSliderRoot);

            var settingNivoSliderHotSettings = new NivoSliderHotSettings();
            _settingService.SaveSetting(settingNivoSliderHotSettings);

            var settingNivoSliderNewSettings = new NivoSliderNewSettings();
            _settingService.SaveSetting(settingNivoSliderNewSettings);

            var settingNivoSliderStarSettings = new NivoSliderStarSettings();
            _settingService.SaveSetting(settingNivoSliderStarSettings);

            var settingNivoSliderLikeSettings = new NivoSliderLikeSettings();
            _settingService.SaveSetting(settingNivoSliderLikeSettings);


            this.AddOrUpdatePluginLocaleResource("Plugins.Widgets.NivoSlider.Picture1", "×ó 1");
            this.AddOrUpdatePluginLocaleResource("Plugins.Widgets.NivoSlider.Picture2", "Picture 2");
            this.AddOrUpdatePluginLocaleResource("Plugins.Widgets.NivoSlider.Picture3", "Picture 3");
            this.AddOrUpdatePluginLocaleResource("Plugins.Widgets.NivoSlider.Picture4", "Picture 4");
            this.AddOrUpdatePluginLocaleResource("Plugins.Widgets.NivoSlider.Picture5", "Picture 5");
            this.AddOrUpdatePluginLocaleResource("Plugins.Widgets.NivoSlider.Picture", "Picture");
            this.AddOrUpdatePluginLocaleResource("Plugins.Widgets.NivoSlider.Picture.Hint", "Upload picture.");
            this.AddOrUpdatePluginLocaleResource("Plugins.Widgets.NivoSlider.Text", "Comment");
            this.AddOrUpdatePluginLocaleResource("Plugins.Widgets.NivoSlider.Text.Hint", "Enter comment for picture. Leave empty if you don't want to display any text.");
            this.AddOrUpdatePluginLocaleResource("Plugins.Widgets.NivoSlider.Link", "URL");
            this.AddOrUpdatePluginLocaleResource("Plugins.Widgets.NivoSlider.Link.Hint", "Enter URL. Leave empty if you don't want this picture to be clickable.");


            base.Install();
        }

        /// <summary>
        /// Uninstall plugin
        /// </summary>
        public override void Uninstall()
        {
            //settings
            _settingService.DeleteSetting<NivoSliderSettings>();
            _settingService.DeleteSetting<NivoIndexRightSettings>();
            _settingService.DeleteSetting<NivoIndexSliderRootSettings>();
            _settingService.DeleteSetting<NivoSliderHotSettings>();
            _settingService.DeleteSetting<NivoSliderLikeSettings>();
            _settingService.DeleteSetting<NivoSliderNewSettings>();
            _settingService.DeleteSetting<NivoSliderStarSettings>();

            //locales
            this.DeletePluginLocaleResource("Plugins.Widgets.NivoSlider.Picture1");
            this.DeletePluginLocaleResource("Plugins.Widgets.NivoSlider.Picture2");
            this.DeletePluginLocaleResource("Plugins.Widgets.NivoSlider.Picture3");
            this.DeletePluginLocaleResource("Plugins.Widgets.NivoSlider.Picture4");
            this.DeletePluginLocaleResource("Plugins.Widgets.NivoSlider.Picture5");
            this.DeletePluginLocaleResource("Plugins.Widgets.NivoSlider.Picture");
            this.DeletePluginLocaleResource("Plugins.Widgets.NivoSlider.Picture.Hint");
            this.DeletePluginLocaleResource("Plugins.Widgets.NivoSlider.Text");
            this.DeletePluginLocaleResource("Plugins.Widgets.NivoSlider.Text.Hint");
            this.DeletePluginLocaleResource("Plugins.Widgets.NivoSlider.Link");
            this.DeletePluginLocaleResource("Plugins.Widgets.NivoSlider.Link.Hint");
            
            base.Uninstall();
        }
    }
}
