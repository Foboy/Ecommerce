using System.Web.Mvc;
using Nop.Core;
using Nop.Core.Caching;
using Nop.Plugin.Widgets.AD.Infrastructure.Cache;
using Nop.Plugin.Widgets.AD.Models;
using Nop.Services.Configuration;
using Nop.Services.Media;
using Nop.Services.Stores;
using Nop.Web.Framework.Controllers;
using Nop.Web.Framework.Kendoui;
using Nop.Core.Domain.Catalog;
using Nop.Services.Catalog;
using Nop.Services.Localization;
using System.Collections.Generic;
using System.Linq;
using Nop.Web.Framework;
using Nop.Web.Framework.Mvc;
using Nop.Services.Vendors;
using AutoMapper;

namespace Nop.Plugin.Widgets.AD.Controllers
{
    public class ADController : BasePluginController
    {
          private readonly IWorkContext _workContext;
        private readonly IStoreContext _storeContext;
        private readonly IStoreService _storeService;
        private readonly IPictureService _pictureService;
        private readonly ISettingService _settingService;
        private readonly ICacheManager _cacheManager;

        private readonly ICategoryService _categoryService;
        private readonly ILocalizationService _localizationService;
        private readonly IProductService _productService;
        private readonly IManufacturerService _manufacturerService;
        private readonly IVendorService _vendorService;

        public ADController(ICategoryService categoryService, ILocalizationService localizationService, IWorkContext workContext,
              IManufacturerService manufacturerService,
            IVendorService vendorService,
            IProductService productService,
            IStoreContext storeContext,
            IStoreService storeService, 
            IPictureService pictureService,
            ISettingService settingService,
            ICacheManager cacheManager)
        {
            this._manufacturerService = manufacturerService;
            this._categoryService = categoryService;
            this._localizationService = localizationService;
            this._productService = productService;
            this._workContext = workContext;
            this._storeContext = storeContext;
            this._storeService = storeService;
            this._pictureService = pictureService;
            this._settingService = settingService;
            this._cacheManager = cacheManager;
            this._vendorService = vendorService;
        }

        protected string GetPictureUrl(int pictureId)
        {
            string cacheKey = string.Format(ModelCacheEventConsumer.PICTURE_URL_MODEL_KEY, pictureId);
            return _cacheManager.Get(cacheKey, () =>
            {
                var url = _pictureService.GetPictureUrl(pictureId, showDefaultPicture: false);
                //little hack here. nulls aren't cacheable so set it to ""
                if (url == null)
                    url = "";

                return url;
            });
        }
        #region getModel

        [AdminAuthorize]
        [ChildActionOnly]
        public ActionResult Configure()
        {
            IndexModel obj = new IndexModel();
            obj._ConfigurationModel = getADModel();

            return View("Nop.Plugin.Widgets.AD.Views.WidgetsAD.index", obj);
        }
        private ConfigurationModel getADModel()
        {
            //load settings for a chosen store scope
            var storeScope = this.GetActiveStoreScopeConfiguration(_storeService, _workContext);
            var ADSettings = _settingService.LoadSetting<ADSettings>(storeScope);
            var model = new ConfigurationModel();
            model.Picture1Id = ADSettings.Picture1Id;
            model.Text1 = ADSettings.Text1;
            model.Link1 = ADSettings.Link1;
            model.Picture2Id = ADSettings.Picture2Id;
            model.Text2 = ADSettings.Text2;
            model.Link2 = ADSettings.Link2;
            model.ActiveStoreScopeConfiguration = storeScope;
            if (storeScope > 0)
            {
                model.Picture1Id_OverrideForStore = _settingService.SettingExists(ADSettings, x => x.Picture1Id, storeScope);
                model.Text1_OverrideForStore = _settingService.SettingExists(ADSettings, x => x.Text1, storeScope);
                model.Link1_OverrideForStore = _settingService.SettingExists(ADSettings, x => x.Link1, storeScope);
                model.Picture2Id_OverrideForStore = _settingService.SettingExists(ADSettings, x => x.Picture2Id, storeScope);
                model.Text2_OverrideForStore = _settingService.SettingExists(ADSettings, x => x.Text2, storeScope);
                model.Link2_OverrideForStore = _settingService.SettingExists(ADSettings, x => x.Link2, storeScope);
            }

            return model;
        }



        #endregion

        #region AD

        [HttpPost]
        [AdminAuthorize]
        public ActionResult ConfigureAD(ConfigurationModel model)
        {
            //load settings for a chosen store scope
            var storeScope = this.GetActiveStoreScopeConfiguration(_storeService, _workContext);
            var ADSettings = _settingService.LoadSetting<ADSettings>(storeScope);
            ADSettings.Picture1Id = model.Picture1Id;
            ADSettings.Text1 = model.Text1;
            ADSettings.Link1 = model.Link1;
            ADSettings.Picture2Id = model.Picture2Id;
            ADSettings.Text2 = model.Text2;
            ADSettings.Link2 = model.Link2;
          

            /* We do not clear cache after each setting update.
             * This behavior can increase performance because cached settings will not be cleared 
             * and loaded from database after each update */
            if (model.Picture1Id_OverrideForStore || storeScope == 0)
                _settingService.SaveSetting(ADSettings, x => x.Picture1Id, storeScope, false);
            else if (storeScope > 0)
                _settingService.DeleteSetting(ADSettings, x => x.Picture1Id, storeScope);

            if (model.Text1_OverrideForStore || storeScope == 0)
                _settingService.SaveSetting(ADSettings, x => x.Text1, storeScope, false);
            else if (storeScope > 0)
                _settingService.DeleteSetting(ADSettings, x => x.Text1, storeScope);

            if (model.Link1_OverrideForStore || storeScope == 0)
                _settingService.SaveSetting(ADSettings, x => x.Link1, storeScope, false);
            else if (storeScope > 0)
                _settingService.DeleteSetting(ADSettings, x => x.Link1, storeScope);

            if (model.Picture2Id_OverrideForStore || storeScope == 0)
                _settingService.SaveSetting(ADSettings, x => x.Picture2Id, storeScope, false);
            else if (storeScope > 0)
                _settingService.DeleteSetting(ADSettings, x => x.Picture2Id, storeScope);

            if (model.Text2_OverrideForStore || storeScope == 0)
                _settingService.SaveSetting(ADSettings, x => x.Text2, storeScope, false);
            else if (storeScope > 0)
                _settingService.DeleteSetting(ADSettings, x => x.Text2, storeScope);

            if (model.Link2_OverrideForStore || storeScope == 0)
                _settingService.SaveSetting(ADSettings, x => x.Link2, storeScope, false);
            else if (storeScope > 0)
                _settingService.DeleteSetting(ADSettings, x => x.Link2, storeScope);

          

            //now clear settings cache
            _settingService.ClearCache();

            Response.Redirect("/Admin/Widget/ConfigureWidget?systemName=Widgets.AD");
            return null;
        }

     
        public ActionResult PublicInfo(string widgetZone)
        {
            var nivoSliderSettings = _settingService.LoadSetting<ADSettings>(_storeContext.CurrentStore.Id);

            var model = new PublicInfoModel();
            model.Picture1Url = GetPictureUrl(nivoSliderSettings.Picture1Id);
            model.Text1 = nivoSliderSettings.Text1;
            model.Link1 = nivoSliderSettings.Link1;

            model.Picture2Url = GetPictureUrl(nivoSliderSettings.Picture2Id);
            model.Text2 = nivoSliderSettings.Text2;
            model.Link2 = nivoSliderSettings.Link2;


            //if (string.IsNullOrEmpty(model.Picture1Url) && string.IsNullOrEmpty(model.Picture2Url) &&
            //    string.IsNullOrEmpty(model.Picture3Url) && string.IsNullOrEmpty(model.Picture4Url) &&
            //    string.IsNullOrEmpty(model.Picture5Url))
            //    //no pictures uploaded
            //    return Content("");


            return View("Nop.Plugin.Widgets.AD.Views.WidgetsAD.PublicInfo", model);
        }
        #endregion Slider

    }
}