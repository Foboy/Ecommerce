using System.Web.Mvc;
using Nop.Core;
using Nop.Core.Caching;
using Nop.Plugin.Widgets.NivoSlider.Infrastructure.Cache;
using Nop.Plugin.Widgets.NivoSlider.Models;
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
using Nop.Web.Framework.Controllers;
using Nop.Web.Framework.Kendoui;
using Nop.Web.Framework.Mvc;
using Nop.Services.Vendors;
using AutoMapper;

namespace Nop.Plugin.Widgets.NivoSlider.Controllers
{
    public class IndexController : BasePluginController
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

        public IndexController(ICategoryService categoryService, ILocalizationService localizationService, IWorkContext workContext,
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
            obj._ConfigurationModel = getSliderModel();
            obj._ConfigurationIndexRightModel = getConfigurationIndexRightModel();
            obj._ConfigurationIndexSliderRootModel = getConfigurationIndexSliderRootModel();
            obj._ConfigurationHotModel = getHotModel();
            obj._ConfigurationNewModel = getNewModel();
            obj._ConfigurationLikeModel = getLikeModel();
            obj._ConfigurationStarModel = getStarModel();

            return View("Nop.Plugin.Widgets.NivoSlider.Views.WidgetsNivoSlider.index", obj);
        }
        private ConfigurationModel getSliderModel()
        {
            //load settings for a chosen store scope
            var storeScope = this.GetActiveStoreScopeConfiguration(_storeService, _workContext);
            var nivoSliderSettings = _settingService.LoadSetting<NivoSliderSettings>(storeScope);
            var model = new ConfigurationModel();
            model.Picture1Id = nivoSliderSettings.Picture1Id;
            model.Text1 = nivoSliderSettings.Text1;
            model.Link1 = nivoSliderSettings.Link1;
            model.Picture2Id = nivoSliderSettings.Picture2Id;
            model.Text2 = nivoSliderSettings.Text2;
            model.Link2 = nivoSliderSettings.Link2;
            model.Picture3Id = nivoSliderSettings.Picture3Id;
            model.Text3 = nivoSliderSettings.Text3;
            model.Link3 = nivoSliderSettings.Link3;
            model.Picture4Id = nivoSliderSettings.Picture4Id;
            model.Text4 = nivoSliderSettings.Text4;
            model.Link4 = nivoSliderSettings.Link4;
            model.Picture5Id = nivoSliderSettings.Picture5Id;
            model.Text5 = nivoSliderSettings.Text5;
            model.Link5 = nivoSliderSettings.Link5;
            model.ActiveStoreScopeConfiguration = storeScope;
            if (storeScope > 0)
            {
                model.Picture1Id_OverrideForStore = _settingService.SettingExists(nivoSliderSettings, x => x.Picture1Id, storeScope);
                model.Text1_OverrideForStore = _settingService.SettingExists(nivoSliderSettings, x => x.Text1, storeScope);
                model.Link1_OverrideForStore = _settingService.SettingExists(nivoSliderSettings, x => x.Link1, storeScope);
                model.Picture2Id_OverrideForStore = _settingService.SettingExists(nivoSliderSettings, x => x.Picture2Id, storeScope);
                model.Text2_OverrideForStore = _settingService.SettingExists(nivoSliderSettings, x => x.Text2, storeScope);
                model.Link2_OverrideForStore = _settingService.SettingExists(nivoSliderSettings, x => x.Link2, storeScope);
                model.Picture3Id_OverrideForStore = _settingService.SettingExists(nivoSliderSettings, x => x.Picture3Id, storeScope);
                model.Text3_OverrideForStore = _settingService.SettingExists(nivoSliderSettings, x => x.Text3, storeScope);
                model.Link3_OverrideForStore = _settingService.SettingExists(nivoSliderSettings, x => x.Link3, storeScope);
                model.Picture4Id_OverrideForStore = _settingService.SettingExists(nivoSliderSettings, x => x.Picture4Id, storeScope);
                model.Text4_OverrideForStore = _settingService.SettingExists(nivoSliderSettings, x => x.Text4, storeScope);
                model.Link4_OverrideForStore = _settingService.SettingExists(nivoSliderSettings, x => x.Link4, storeScope);
                model.Picture5Id_OverrideForStore = _settingService.SettingExists(nivoSliderSettings, x => x.Picture5Id, storeScope);
                model.Text5_OverrideForStore = _settingService.SettingExists(nivoSliderSettings, x => x.Text5, storeScope);
                model.Link5_OverrideForStore = _settingService.SettingExists(nivoSliderSettings, x => x.Link5, storeScope);
            }

            return model;
        }

        private ConfigurationIndexRightModel getConfigurationIndexRightModel()
        {
            //load settings for a chosen store scope
            var storeScope = this.GetActiveStoreScopeConfiguration(_storeService, _workContext);
            var nivoSliderSettings = _settingService.LoadSetting<NivoIndexRightSettings>(storeScope);
            var model = new ConfigurationIndexRightModel();
            model.Picture1Id = nivoSliderSettings.Picture1Id;
            model.Text1 = nivoSliderSettings.Text1;
            model.Link1 = nivoSliderSettings.Link1;
            model.Picture2Id = nivoSliderSettings.Picture2Id;
            model.Text2 = nivoSliderSettings.Text2;
            model.Link2 = nivoSliderSettings.Link2;
            model.Picture3Id = nivoSliderSettings.Picture3Id;
            model.Text3 = nivoSliderSettings.Text3;
            model.Link3 = nivoSliderSettings.Link3;
            model.ActiveStoreScopeConfiguration = storeScope;
            if (storeScope > 0)
            {
                model.Picture1Id_OverrideForStore = _settingService.SettingExists(nivoSliderSettings, x => x.Picture1Id, storeScope);
                model.Text1_OverrideForStore = _settingService.SettingExists(nivoSliderSettings, x => x.Text1, storeScope);
                model.Link1_OverrideForStore = _settingService.SettingExists(nivoSliderSettings, x => x.Link1, storeScope);
                model.Picture2Id_OverrideForStore = _settingService.SettingExists(nivoSliderSettings, x => x.Picture2Id, storeScope);
                model.Text2_OverrideForStore = _settingService.SettingExists(nivoSliderSettings, x => x.Text2, storeScope);
                model.Link2_OverrideForStore = _settingService.SettingExists(nivoSliderSettings, x => x.Link2, storeScope);
                model.Picture3Id_OverrideForStore = _settingService.SettingExists(nivoSliderSettings, x => x.Picture3Id, storeScope);
                model.Text3_OverrideForStore = _settingService.SettingExists(nivoSliderSettings, x => x.Text3, storeScope);
                model.Link3_OverrideForStore = _settingService.SettingExists(nivoSliderSettings, x => x.Link3, storeScope);
            }

            return model;
        }

        public ConfigurationIndexSliderRootModel getConfigurationIndexSliderRootModel()
        {
            //load settings for a chosen store scope
            var storeScope = this.GetActiveStoreScopeConfiguration(_storeService, _workContext);
            var nivoSliderSettings = _settingService.LoadSetting<NivoIndexSliderRootSettings>(storeScope);
            var model = new ConfigurationIndexSliderRootModel();
            model.Picture1Id = nivoSliderSettings.Picture1Id;
            model.Text1 = nivoSliderSettings.Text1;
            model.Link1 = nivoSliderSettings.Link1;
            model.Picture2Id = nivoSliderSettings.Picture2Id;
            model.Text2 = nivoSliderSettings.Text2;
            model.Link2 = nivoSliderSettings.Link2;
            model.Picture3Id = nivoSliderSettings.Picture3Id;
            model.Text3 = nivoSliderSettings.Text3;
            model.Link3 = nivoSliderSettings.Link3;
            model.ActiveStoreScopeConfiguration = storeScope;
            if (storeScope > 0)
            {
                model.Picture1Id_OverrideForStore = _settingService.SettingExists(nivoSliderSettings, x => x.Picture1Id, storeScope);
                model.Text1_OverrideForStore = _settingService.SettingExists(nivoSliderSettings, x => x.Text1, storeScope);
                model.Link1_OverrideForStore = _settingService.SettingExists(nivoSliderSettings, x => x.Link1, storeScope);
                model.Picture2Id_OverrideForStore = _settingService.SettingExists(nivoSliderSettings, x => x.Picture2Id, storeScope);
                model.Text2_OverrideForStore = _settingService.SettingExists(nivoSliderSettings, x => x.Text2, storeScope);
                model.Link2_OverrideForStore = _settingService.SettingExists(nivoSliderSettings, x => x.Link2, storeScope);
                model.Picture3Id_OverrideForStore = _settingService.SettingExists(nivoSliderSettings, x => x.Picture3Id, storeScope);
                model.Text3_OverrideForStore = _settingService.SettingExists(nivoSliderSettings, x => x.Text3, storeScope);
                model.Link3_OverrideForStore = _settingService.SettingExists(nivoSliderSettings, x => x.Link3, storeScope);
            }

            return  model;
        }

        private ConfigurationHotModel getHotModel()
        {
            //load settings for a chosen store scope
            var storeScope = this.GetActiveStoreScopeConfiguration(_storeService, _workContext);
            var nivoSliderSettings = _settingService.LoadSetting<NivoSliderHotSettings>(storeScope);
            var model = new ConfigurationHotModel();
            model.Picture1Id = nivoSliderSettings.Picture1Id;
            model.Text1 = nivoSliderSettings.Text1;
            model.Description1 = nivoSliderSettings.Description1;
            model.Link1 = nivoSliderSettings.Link1;
            model.Picture2Id = nivoSliderSettings.Picture2Id;
            model.Text2 = nivoSliderSettings.Text2;
            model.Description2 = nivoSliderSettings.Description2;
            model.Link2 = nivoSliderSettings.Link2;
            model.Picture3Id = nivoSliderSettings.Picture3Id;
            model.Text3 = nivoSliderSettings.Text3;
            model.Description3 = nivoSliderSettings.Description3;
            model.Link3 = nivoSliderSettings.Link3;
            model.Picture4Id = nivoSliderSettings.Picture4Id;
            model.Text4 = nivoSliderSettings.Text4;
            model.Description4 = nivoSliderSettings.Description4;
            model.Link4 = nivoSliderSettings.Link4;
            model.ActiveStoreScopeConfiguration = storeScope;
            if (storeScope > 0)
            {
                model.Picture1Id_OverrideForStore = _settingService.SettingExists(nivoSliderSettings, x => x.Picture1Id, storeScope);
                model.Text1_OverrideForStore = _settingService.SettingExists(nivoSliderSettings, x => x.Text1, storeScope);
                model.Description1_OverrideForStore = _settingService.SettingExists(nivoSliderSettings, x => x.Description1, storeScope);
                model.Link1_OverrideForStore = _settingService.SettingExists(nivoSliderSettings, x => x.Link1, storeScope);
                model.Picture2Id_OverrideForStore = _settingService.SettingExists(nivoSliderSettings, x => x.Picture2Id, storeScope);
                model.Text2_OverrideForStore = _settingService.SettingExists(nivoSliderSettings, x => x.Text2, storeScope);
                model.Description2_OverrideForStore = _settingService.SettingExists(nivoSliderSettings, x => x.Description2, storeScope);
                model.Link2_OverrideForStore = _settingService.SettingExists(nivoSliderSettings, x => x.Link2, storeScope);
                model.Picture3Id_OverrideForStore = _settingService.SettingExists(nivoSliderSettings, x => x.Picture3Id, storeScope);
                model.Text3_OverrideForStore = _settingService.SettingExists(nivoSliderSettings, x => x.Text3, storeScope);
                model.Description3_OverrideForStore = _settingService.SettingExists(nivoSliderSettings, x => x.Description3, storeScope);
                model.Link3_OverrideForStore = _settingService.SettingExists(nivoSliderSettings, x => x.Link3, storeScope);
                model.Picture4Id_OverrideForStore = _settingService.SettingExists(nivoSliderSettings, x => x.Picture4Id, storeScope);
                model.Text4_OverrideForStore = _settingService.SettingExists(nivoSliderSettings, x => x.Text4, storeScope);
                model.Description4_OverrideForStore = _settingService.SettingExists(nivoSliderSettings, x => x.Description4, storeScope);
                model.Link4_OverrideForStore = _settingService.SettingExists(nivoSliderSettings, x => x.Link4, storeScope);
            }

            return model;
        }

        private ConfigurationNewModel getNewModel()
        {
            //load settings for a chosen store scope
            var storeScope = this.GetActiveStoreScopeConfiguration(_storeService, _workContext);
            var nivoSliderSettings = _settingService.LoadSetting<NivoSliderNewSettings>(storeScope);
            var model = new ConfigurationNewModel();
            model.Picture1Id = nivoSliderSettings.Picture1Id;
            model.Text1 = nivoSliderSettings.Text1;
            model.Link1 = nivoSliderSettings.Link1;
            model.Picture2Id = nivoSliderSettings.Picture2Id;
            model.Text2 = nivoSliderSettings.Text2;
            model.Link2 = nivoSliderSettings.Link2;
            model.Picture3Id = nivoSliderSettings.Picture3Id;
            model.Text3 = nivoSliderSettings.Text3;
            model.Link3 = nivoSliderSettings.Link3;
            model.Picture4Id = nivoSliderSettings.Picture4Id;
            model.Text4 = nivoSliderSettings.Text4;
            model.Link4 = nivoSliderSettings.Link4;
            model.Picture5Id = nivoSliderSettings.Picture5Id;
            model.Text5 = nivoSliderSettings.Text5;
            model.Link5 = nivoSliderSettings.Link5;
            model.Picture6Id = nivoSliderSettings.Picture6Id;
            model.Text6 = nivoSliderSettings.Text6;
            model.Link6 = nivoSliderSettings.Link6;
            model.Picture7Id = nivoSliderSettings.Picture7Id;
            model.Text7 = nivoSliderSettings.Text7;
            model.Link7 = nivoSliderSettings.Link7;
            model.Picture8Id = nivoSliderSettings.Picture8Id;
            model.Text8 = nivoSliderSettings.Text8;
            model.Link8 = nivoSliderSettings.Link8;
            model.ActiveStoreScopeConfiguration = storeScope;
            if (storeScope > 0)
            {
                model.Picture1Id_OverrideForStore = _settingService.SettingExists(nivoSliderSettings, x => x.Picture1Id, storeScope);
                model.Text1_OverrideForStore = _settingService.SettingExists(nivoSliderSettings, x => x.Text1, storeScope);
                model.Link1_OverrideForStore = _settingService.SettingExists(nivoSliderSettings, x => x.Link1, storeScope);
                model.Picture2Id_OverrideForStore = _settingService.SettingExists(nivoSliderSettings, x => x.Picture2Id, storeScope);
                model.Text2_OverrideForStore = _settingService.SettingExists(nivoSliderSettings, x => x.Text2, storeScope);
                model.Link2_OverrideForStore = _settingService.SettingExists(nivoSliderSettings, x => x.Link2, storeScope);
                model.Picture3Id_OverrideForStore = _settingService.SettingExists(nivoSliderSettings, x => x.Picture3Id, storeScope);
                model.Text3_OverrideForStore = _settingService.SettingExists(nivoSliderSettings, x => x.Text3, storeScope);
                model.Link3_OverrideForStore = _settingService.SettingExists(nivoSliderSettings, x => x.Link3, storeScope);
                model.Picture4Id_OverrideForStore = _settingService.SettingExists(nivoSliderSettings, x => x.Picture4Id, storeScope);
                model.Text4_OverrideForStore = _settingService.SettingExists(nivoSliderSettings, x => x.Text4, storeScope);
                model.Link4_OverrideForStore = _settingService.SettingExists(nivoSliderSettings, x => x.Link4, storeScope);
                model.Picture5Id_OverrideForStore = _settingService.SettingExists(nivoSliderSettings, x => x.Picture5Id, storeScope);
                model.Text5_OverrideForStore = _settingService.SettingExists(nivoSliderSettings, x => x.Text5, storeScope);
                model.Link5_OverrideForStore = _settingService.SettingExists(nivoSliderSettings, x => x.Link5, storeScope);
                model.Picture6Id_OverrideForStore = _settingService.SettingExists(nivoSliderSettings, x => x.Picture6Id, storeScope);
                model.Text6_OverrideForStore = _settingService.SettingExists(nivoSliderSettings, x => x.Text6, storeScope);
                model.Link6_OverrideForStore = _settingService.SettingExists(nivoSliderSettings, x => x.Link6, storeScope);
                model.Picture7Id_OverrideForStore = _settingService.SettingExists(nivoSliderSettings, x => x.Picture7Id, storeScope);
                model.Text7_OverrideForStore = _settingService.SettingExists(nivoSliderSettings, x => x.Text7, storeScope);
                model.Link7_OverrideForStore = _settingService.SettingExists(nivoSliderSettings, x => x.Link7, storeScope);
                model.Picture8Id_OverrideForStore = _settingService.SettingExists(nivoSliderSettings, x => x.Picture8Id, storeScope);
                model.Text8_OverrideForStore = _settingService.SettingExists(nivoSliderSettings, x => x.Text8, storeScope);
                model.Link8_OverrideForStore = _settingService.SettingExists(nivoSliderSettings, x => x.Link8, storeScope);
            }

            return model;
        }

        private ConfigurationLikeModel getLikeModel()
        {
            //load settings for a chosen store scope
            var storeScope = this.GetActiveStoreScopeConfiguration(_storeService, _workContext);
            var nivoSliderSettings = _settingService.LoadSetting<NivoSliderLikeSettings>(storeScope);
            var model = new ConfigurationLikeModel();
            model.Picture1Id = nivoSliderSettings.Picture1Id;
            model.Text1 = nivoSliderSettings.Text1;
            model.Link1 = nivoSliderSettings.Link1;
            model.Picture2Id = nivoSliderSettings.Picture2Id;
            model.Text2 = nivoSliderSettings.Text2;
            model.Link2 = nivoSliderSettings.Link2;
            model.Picture3Id = nivoSliderSettings.Picture3Id;
            model.Text3 = nivoSliderSettings.Text3;
            model.Link3 = nivoSliderSettings.Link3;
            model.Picture4Id = nivoSliderSettings.Picture4Id;
            model.Text4 = nivoSliderSettings.Text4;
            model.Link4 = nivoSliderSettings.Link4;
            model.Picture5Id = nivoSliderSettings.Picture5Id;
            model.Text5 = nivoSliderSettings.Text5;
            model.Link5 = nivoSliderSettings.Link5;
            model.Picture6Id = nivoSliderSettings.Picture6Id;
            model.Text6 = nivoSliderSettings.Text6;
            model.Link6 = nivoSliderSettings.Link6;
            model.Picture7Id = nivoSliderSettings.Picture7Id;
            model.Text7 = nivoSliderSettings.Text7;
            model.Link7 = nivoSliderSettings.Link7;
            model.Picture8Id = nivoSliderSettings.Picture8Id;
            model.Text8 = nivoSliderSettings.Text8;
            model.Link8 = nivoSliderSettings.Link8;
            model.ActiveStoreScopeConfiguration = storeScope;
            if (storeScope > 0)
            {
                model.Picture1Id_OverrideForStore = _settingService.SettingExists(nivoSliderSettings, x => x.Picture1Id, storeScope);
                model.Text1_OverrideForStore = _settingService.SettingExists(nivoSliderSettings, x => x.Text1, storeScope);
                model.Link1_OverrideForStore = _settingService.SettingExists(nivoSliderSettings, x => x.Link1, storeScope);
                model.Picture2Id_OverrideForStore = _settingService.SettingExists(nivoSliderSettings, x => x.Picture2Id, storeScope);
                model.Text2_OverrideForStore = _settingService.SettingExists(nivoSliderSettings, x => x.Text2, storeScope);
                model.Link2_OverrideForStore = _settingService.SettingExists(nivoSliderSettings, x => x.Link2, storeScope);
                model.Picture3Id_OverrideForStore = _settingService.SettingExists(nivoSliderSettings, x => x.Picture3Id, storeScope);
                model.Text3_OverrideForStore = _settingService.SettingExists(nivoSliderSettings, x => x.Text3, storeScope);
                model.Link3_OverrideForStore = _settingService.SettingExists(nivoSliderSettings, x => x.Link3, storeScope);
                model.Picture4Id_OverrideForStore = _settingService.SettingExists(nivoSliderSettings, x => x.Picture4Id, storeScope);
                model.Text4_OverrideForStore = _settingService.SettingExists(nivoSliderSettings, x => x.Text4, storeScope);
                model.Link4_OverrideForStore = _settingService.SettingExists(nivoSliderSettings, x => x.Link4, storeScope);
                model.Picture5Id_OverrideForStore = _settingService.SettingExists(nivoSliderSettings, x => x.Picture5Id, storeScope);
                model.Text5_OverrideForStore = _settingService.SettingExists(nivoSliderSettings, x => x.Text5, storeScope);
                model.Link5_OverrideForStore = _settingService.SettingExists(nivoSliderSettings, x => x.Link5, storeScope);
                model.Picture6Id_OverrideForStore = _settingService.SettingExists(nivoSliderSettings, x => x.Picture6Id, storeScope);
                model.Text6_OverrideForStore = _settingService.SettingExists(nivoSliderSettings, x => x.Text6, storeScope);
                model.Link6_OverrideForStore = _settingService.SettingExists(nivoSliderSettings, x => x.Link6, storeScope);
                model.Picture7Id_OverrideForStore = _settingService.SettingExists(nivoSliderSettings, x => x.Picture7Id, storeScope);
                model.Text7_OverrideForStore = _settingService.SettingExists(nivoSliderSettings, x => x.Text7, storeScope);
                model.Link7_OverrideForStore = _settingService.SettingExists(nivoSliderSettings, x => x.Link7, storeScope);
                model.Picture8Id_OverrideForStore = _settingService.SettingExists(nivoSliderSettings, x => x.Picture8Id, storeScope);
                model.Text8_OverrideForStore = _settingService.SettingExists(nivoSliderSettings, x => x.Text8, storeScope);
                model.Link8_OverrideForStore = _settingService.SettingExists(nivoSliderSettings, x => x.Link8, storeScope);
            }

            return model;
        }

        private ConfigurationStarModel getStarModel()
        {
            //load settings for a chosen store scope
            var storeScope = this.GetActiveStoreScopeConfiguration(_storeService, _workContext);
            var nivoSliderSettings = _settingService.LoadSetting<NivoSliderStarSettings>(storeScope);
            var model = new ConfigurationStarModel();
            model.Picture1Id = nivoSliderSettings.Picture1Id;
            model.Text1 = nivoSliderSettings.Text1;
            model.Link1 = nivoSliderSettings.Link1;
            model.Picture2Id = nivoSliderSettings.Picture2Id;
            model.Text2 = nivoSliderSettings.Text2;
            model.Link2 = nivoSliderSettings.Link2;
            model.Picture3Id = nivoSliderSettings.Picture3Id;
            model.Text3 = nivoSliderSettings.Text3;
            model.Link3 = nivoSliderSettings.Link3;
            model.Picture4Id = nivoSliderSettings.Picture4Id;
            model.Text4 = nivoSliderSettings.Text4;
            model.Link4 = nivoSliderSettings.Link4;
            model.Picture5Id = nivoSliderSettings.Picture5Id;
            model.Text5 = nivoSliderSettings.Text5;
            model.Link5 = nivoSliderSettings.Link5;
            model.Picture6Id = nivoSliderSettings.Picture6Id;
            model.Text6 = nivoSliderSettings.Text6;
            model.Link6 = nivoSliderSettings.Link6;
            model.Picture7Id = nivoSliderSettings.Picture7Id;
            model.Text7 = nivoSliderSettings.Text7;
            model.Link7 = nivoSliderSettings.Link7;
            model.Picture8Id = nivoSliderSettings.Picture8Id;
            model.Text8 = nivoSliderSettings.Text8;
            model.Link8 = nivoSliderSettings.Link8;
            model.ActiveStoreScopeConfiguration = storeScope;
            if (storeScope > 0)
            {
                model.Picture1Id_OverrideForStore = _settingService.SettingExists(nivoSliderSettings, x => x.Picture1Id, storeScope);
                model.Text1_OverrideForStore = _settingService.SettingExists(nivoSliderSettings, x => x.Text1, storeScope);
                model.Link1_OverrideForStore = _settingService.SettingExists(nivoSliderSettings, x => x.Link1, storeScope);
                model.Picture2Id_OverrideForStore = _settingService.SettingExists(nivoSliderSettings, x => x.Picture2Id, storeScope);
                model.Text2_OverrideForStore = _settingService.SettingExists(nivoSliderSettings, x => x.Text2, storeScope);
                model.Link2_OverrideForStore = _settingService.SettingExists(nivoSliderSettings, x => x.Link2, storeScope);
                model.Picture3Id_OverrideForStore = _settingService.SettingExists(nivoSliderSettings, x => x.Picture3Id, storeScope);
                model.Text3_OverrideForStore = _settingService.SettingExists(nivoSliderSettings, x => x.Text3, storeScope);
                model.Link3_OverrideForStore = _settingService.SettingExists(nivoSliderSettings, x => x.Link3, storeScope);
                model.Picture4Id_OverrideForStore = _settingService.SettingExists(nivoSliderSettings, x => x.Picture4Id, storeScope);
                model.Text4_OverrideForStore = _settingService.SettingExists(nivoSliderSettings, x => x.Text4, storeScope);
                model.Link4_OverrideForStore = _settingService.SettingExists(nivoSliderSettings, x => x.Link4, storeScope);
                model.Picture5Id_OverrideForStore = _settingService.SettingExists(nivoSliderSettings, x => x.Picture5Id, storeScope);
                model.Text5_OverrideForStore = _settingService.SettingExists(nivoSliderSettings, x => x.Text5, storeScope);
                model.Link5_OverrideForStore = _settingService.SettingExists(nivoSliderSettings, x => x.Link5, storeScope);
                model.Picture6Id_OverrideForStore = _settingService.SettingExists(nivoSliderSettings, x => x.Picture6Id, storeScope);
                model.Text6_OverrideForStore = _settingService.SettingExists(nivoSliderSettings, x => x.Text6, storeScope);
                model.Link6_OverrideForStore = _settingService.SettingExists(nivoSliderSettings, x => x.Link6, storeScope);
                model.Picture7Id_OverrideForStore = _settingService.SettingExists(nivoSliderSettings, x => x.Picture7Id, storeScope);
                model.Text7_OverrideForStore = _settingService.SettingExists(nivoSliderSettings, x => x.Text7, storeScope);
                model.Link7_OverrideForStore = _settingService.SettingExists(nivoSliderSettings, x => x.Link7, storeScope);
                model.Picture8Id_OverrideForStore = _settingService.SettingExists(nivoSliderSettings, x => x.Picture8Id, storeScope);
                model.Text8_OverrideForStore = _settingService.SettingExists(nivoSliderSettings, x => x.Text8, storeScope);
                model.Link8_OverrideForStore = _settingService.SettingExists(nivoSliderSettings, x => x.Link8, storeScope);
            }

            return model;
        }


        #endregion

        #region Slider

        [HttpPost]
        [AdminAuthorize]
        public ActionResult ConfigureSlider(ConfigurationModel model)
        {
            //load settings for a chosen store scope
            var storeScope = this.GetActiveStoreScopeConfiguration(_storeService, _workContext);
            var nivoSliderSettings = _settingService.LoadSetting<NivoSliderSettings>(storeScope);
            nivoSliderSettings.Picture1Id = model.Picture1Id;
            nivoSliderSettings.Text1 = model.Text1;
            nivoSliderSettings.Link1 = model.Link1;
            nivoSliderSettings.Picture2Id = model.Picture2Id;
            nivoSliderSettings.Text2 = model.Text2;
            nivoSliderSettings.Link2 = model.Link2;
            nivoSliderSettings.Picture3Id = model.Picture3Id;
            nivoSliderSettings.Text3 = model.Text3;
            nivoSliderSettings.Link3 = model.Link3;
            nivoSliderSettings.Picture4Id = model.Picture4Id;
            nivoSliderSettings.Text4 = model.Text4;
            nivoSliderSettings.Link4 = model.Link4;
            nivoSliderSettings.Picture5Id = model.Picture5Id;
            nivoSliderSettings.Text5 = model.Text5;
            nivoSliderSettings.Link5 = model.Link5;

            /* We do not clear cache after each setting update.
             * This behavior can increase performance because cached settings will not be cleared 
             * and loaded from database after each update */
            if (model.Picture1Id_OverrideForStore || storeScope == 0)
                _settingService.SaveSetting(nivoSliderSettings, x => x.Picture1Id, storeScope, false);
            else if (storeScope > 0)
                _settingService.DeleteSetting(nivoSliderSettings, x => x.Picture1Id, storeScope);

            if (model.Text1_OverrideForStore || storeScope == 0)
                _settingService.SaveSetting(nivoSliderSettings, x => x.Text1, storeScope, false);
            else if (storeScope > 0)
                _settingService.DeleteSetting(nivoSliderSettings, x => x.Text1, storeScope);

            if (model.Link1_OverrideForStore || storeScope == 0)
                _settingService.SaveSetting(nivoSliderSettings, x => x.Link1, storeScope, false);
            else if (storeScope > 0)
                _settingService.DeleteSetting(nivoSliderSettings, x => x.Link1, storeScope);

            if (model.Picture2Id_OverrideForStore || storeScope == 0)
                _settingService.SaveSetting(nivoSliderSettings, x => x.Picture2Id, storeScope, false);
            else if (storeScope > 0)
                _settingService.DeleteSetting(nivoSliderSettings, x => x.Picture2Id, storeScope);

            if (model.Text2_OverrideForStore || storeScope == 0)
                _settingService.SaveSetting(nivoSliderSettings, x => x.Text2, storeScope, false);
            else if (storeScope > 0)
                _settingService.DeleteSetting(nivoSliderSettings, x => x.Text2, storeScope);

            if (model.Link2_OverrideForStore || storeScope == 0)
                _settingService.SaveSetting(nivoSliderSettings, x => x.Link2, storeScope, false);
            else if (storeScope > 0)
                _settingService.DeleteSetting(nivoSliderSettings, x => x.Link2, storeScope);

            if (model.Picture3Id_OverrideForStore || storeScope == 0)
                _settingService.SaveSetting(nivoSliderSettings, x => x.Picture3Id, storeScope, false);
            else if (storeScope > 0)
                _settingService.DeleteSetting(nivoSliderSettings, x => x.Picture3Id, storeScope);

            if (model.Text3_OverrideForStore || storeScope == 0)
                _settingService.SaveSetting(nivoSliderSettings, x => x.Text3, storeScope, false);
            else if (storeScope > 0)
                _settingService.DeleteSetting(nivoSliderSettings, x => x.Text3, storeScope);

            if (model.Link3_OverrideForStore || storeScope == 0)
                _settingService.SaveSetting(nivoSliderSettings, x => x.Link3, storeScope, false);
            else if (storeScope > 0)
                _settingService.DeleteSetting(nivoSliderSettings, x => x.Link3, storeScope);

            if (model.Picture4Id_OverrideForStore || storeScope == 0)
                _settingService.SaveSetting(nivoSliderSettings, x => x.Picture4Id, storeScope, false);
            else if (storeScope > 0)
                _settingService.DeleteSetting(nivoSliderSettings, x => x.Picture4Id, storeScope);

            if (model.Text4_OverrideForStore || storeScope == 0)
                _settingService.SaveSetting(nivoSliderSettings, x => x.Text4, storeScope, false);
            else if (storeScope > 0)
                _settingService.DeleteSetting(nivoSliderSettings, x => x.Text4, storeScope);

            if (model.Link4_OverrideForStore || storeScope == 0)
                _settingService.SaveSetting(nivoSliderSettings, x => x.Link4, storeScope, false);
            else if (storeScope > 0)
                _settingService.DeleteSetting(nivoSliderSettings, x => x.Link4, storeScope);

            if (model.Picture5Id_OverrideForStore || storeScope == 0)
                _settingService.SaveSetting(nivoSliderSettings, x => x.Picture5Id, storeScope, false);
            else if (storeScope > 0)
                _settingService.DeleteSetting(nivoSliderSettings, x => x.Picture5Id, storeScope);

            if (model.Text5_OverrideForStore || storeScope == 0)
                _settingService.SaveSetting(nivoSliderSettings, x => x.Text5, storeScope, false);
            else if (storeScope > 0)
                _settingService.DeleteSetting(nivoSliderSettings, x => x.Text5, storeScope);

            if (model.Link5_OverrideForStore || storeScope == 0)
                _settingService.SaveSetting(nivoSliderSettings, x => x.Link5, storeScope, false);
            else if (storeScope > 0)
                _settingService.DeleteSetting(nivoSliderSettings, x => x.Link5, storeScope);

            //now clear settings cache
            _settingService.ClearCache();

            Response.Redirect("/Admin/Widget/ConfigureWidget?systemName=Widgets.NivoSlider");
            return null;
        }

     
        public ActionResult PublicInfoSlider(string widgetZone)
        {
            var nivoSliderSettings = _settingService.LoadSetting<NivoSliderSettings>(_storeContext.CurrentStore.Id);

            var model = new PublicInfoModel();
            model.Picture1Url = GetPictureUrl(nivoSliderSettings.Picture1Id);
            model.Text1 = nivoSliderSettings.Text1;
            model.Link1 = nivoSliderSettings.Link1;

            model.Picture2Url = GetPictureUrl(nivoSliderSettings.Picture2Id);
            model.Text2 = nivoSliderSettings.Text2;
            model.Link2 = nivoSliderSettings.Link2;

            model.Picture3Url = GetPictureUrl(nivoSliderSettings.Picture3Id);
            model.Text3 = nivoSliderSettings.Text3;
            model.Link3 = nivoSliderSettings.Link3;

            model.Picture4Url = GetPictureUrl(nivoSliderSettings.Picture4Id);
            model.Text4 = nivoSliderSettings.Text4;
            model.Link4 = nivoSliderSettings.Link4;

            model.Picture5Url = GetPictureUrl(nivoSliderSettings.Picture5Id);
            model.Text5 = nivoSliderSettings.Text5;
            model.Link5 = nivoSliderSettings.Link5;

            //if (string.IsNullOrEmpty(model.Picture1Url) && string.IsNullOrEmpty(model.Picture2Url) &&
            //    string.IsNullOrEmpty(model.Picture3Url) && string.IsNullOrEmpty(model.Picture4Url) &&
            //    string.IsNullOrEmpty(model.Picture5Url))
            //    //no pictures uploaded
            //    return Content("");


            return View("Nop.Plugin.Widgets.NivoSlider.Views.WidgetsNivoSlider.PublicInfo", model);
        }
        #endregion Slider

        #region IndexRight

      
        [HttpPost]
        [AdminAuthorize]
        public ActionResult ConfigureIndexRight(ConfigurationIndexRightModel model)
        {
            //load settings for a chosen store scope
            var storeScope = this.GetActiveStoreScopeConfiguration(_storeService, _workContext);
            var nivoSliderSettings = _settingService.LoadSetting<NivoIndexRightSettings>(storeScope);
            nivoSliderSettings.Picture1Id = model.Picture1Id;
            nivoSliderSettings.Text1 = model.Text1;
            nivoSliderSettings.Link1 = model.Link1;
            nivoSliderSettings.Picture2Id = model.Picture2Id;
            nivoSliderSettings.Text2 = model.Text2;
            nivoSliderSettings.Link2 = model.Link2;
            nivoSliderSettings.Picture3Id = model.Picture3Id;
            nivoSliderSettings.Text3 = model.Text3;
            nivoSliderSettings.Link3 = model.Link3;

            /* We do not clear cache after each setting update.
             * This behavior can increase performance because cached settings will not be cleared 
             * and loaded from database after each update */
            if (model.Picture1Id_OverrideForStore || storeScope == 0)
                _settingService.SaveSetting(nivoSliderSettings, x => x.Picture1Id, storeScope, false);
            else if (storeScope > 0)
                _settingService.DeleteSetting(nivoSliderSettings, x => x.Picture1Id, storeScope);

            if (model.Text1_OverrideForStore || storeScope == 0)
                _settingService.SaveSetting(nivoSliderSettings, x => x.Text1, storeScope, false);
            else if (storeScope > 0)
                _settingService.DeleteSetting(nivoSliderSettings, x => x.Text1, storeScope);

            if (model.Link1_OverrideForStore || storeScope == 0)
                _settingService.SaveSetting(nivoSliderSettings, x => x.Link1, storeScope, false);
            else if (storeScope > 0)
                _settingService.DeleteSetting(nivoSliderSettings, x => x.Link1, storeScope);

            if (model.Picture2Id_OverrideForStore || storeScope == 0)
                _settingService.SaveSetting(nivoSliderSettings, x => x.Picture2Id, storeScope, false);
            else if (storeScope > 0)
                _settingService.DeleteSetting(nivoSliderSettings, x => x.Picture2Id, storeScope);

            if (model.Text2_OverrideForStore || storeScope == 0)
                _settingService.SaveSetting(nivoSliderSettings, x => x.Text2, storeScope, false);
            else if (storeScope > 0)
                _settingService.DeleteSetting(nivoSliderSettings, x => x.Text2, storeScope);

            if (model.Link2_OverrideForStore || storeScope == 0)
                _settingService.SaveSetting(nivoSliderSettings, x => x.Link2, storeScope, false);
            else if (storeScope > 0)
                _settingService.DeleteSetting(nivoSliderSettings, x => x.Link2, storeScope);

            if (model.Picture3Id_OverrideForStore || storeScope == 0)
                _settingService.SaveSetting(nivoSliderSettings, x => x.Picture3Id, storeScope, false);
            else if (storeScope > 0)
                _settingService.DeleteSetting(nivoSliderSettings, x => x.Picture3Id, storeScope);

            if (model.Text3_OverrideForStore || storeScope == 0)
                _settingService.SaveSetting(nivoSliderSettings, x => x.Text3, storeScope, false);
            else if (storeScope > 0)
                _settingService.DeleteSetting(nivoSliderSettings, x => x.Text3, storeScope);

            if (model.Link3_OverrideForStore || storeScope == 0)
                _settingService.SaveSetting(nivoSliderSettings, x => x.Link3, storeScope, false);
            else if (storeScope > 0)
                _settingService.DeleteSetting(nivoSliderSettings, x => x.Link3, storeScope);



            //now clear settings cache
            _settingService.ClearCache();

            Response.Redirect("/Admin/Widget/ConfigureWidget?systemName=Widgets.NivoSlider");
            return null;
        }

        [ChildActionOnly]
        public ActionResult PublicInfoIndexRightTop(string widgetZone)
        {
            var nivoSliderSettings = _settingService.LoadSetting<NivoIndexRightSettings>(_storeContext.CurrentStore.Id);

            var model = new PublicInfoIndexRightModel();
            model.Picture1Url = GetPictureUrl(nivoSliderSettings.Picture1Id);
            model.Text1 = nivoSliderSettings.Text1;
            model.Link1 = nivoSliderSettings.Link1;



            //if (string.IsNullOrEmpty(model.Picture1Url))
            //    //no pictures uploaded
            //    return Content("");


            return View("Nop.Plugin.Widgets.NivoSlider.Views.WidgetsNivoSlider.PublicInfoIndexRightTop", model);
        }
        [ChildActionOnly]
        public ActionResult PublicInfoIndexRight2(string widgetZone)
        {
            var nivoSliderSettings = _settingService.LoadSetting<NivoIndexRightSettings>(_storeContext.CurrentStore.Id);

            var model = new PublicInfoIndexRightModel();

            model.Picture2Url = GetPictureUrl(nivoSliderSettings.Picture2Id);
            model.Text2 = nivoSliderSettings.Text2;
            model.Link2 = nivoSliderSettings.Link2;


            //if ( string.IsNullOrEmpty(model.Picture2Url))
            //    //no pictures uploaded
            //    return Content("");


            return View("Nop.Plugin.Widgets.NivoSlider.Views.WidgetsNivoSlider.PublicInfoIndexRight2", model);
        }
        #endregion indexright

        #region Root

       

        [HttpPost]
        [AdminAuthorize]
        public ActionResult ConfigureRoot(ConfigurationIndexSliderRootModel model)
        {
            //load settings for a chosen store scope
            var storeScope = this.GetActiveStoreScopeConfiguration(_storeService, _workContext);
            var nivoSliderSettings = _settingService.LoadSetting<NivoIndexSliderRootSettings>(storeScope);
            nivoSliderSettings.Picture1Id = model.Picture1Id;
            nivoSliderSettings.Text1 = model.Text1;
            nivoSliderSettings.Link1 = model.Link1;
            nivoSliderSettings.Picture2Id = model.Picture2Id;
            nivoSliderSettings.Text2 = model.Text2;
            nivoSliderSettings.Link2 = model.Link2;
            nivoSliderSettings.Picture3Id = model.Picture3Id;
            nivoSliderSettings.Text3 = model.Text3;
            nivoSliderSettings.Link3 = model.Link3;

            /* We do not clear cache after each setting update.
             * This behavior can increase performance because cached settings will not be cleared 
             * and loaded from database after each update */
            if (model.Picture1Id_OverrideForStore || storeScope == 0)
                _settingService.SaveSetting(nivoSliderSettings, x => x.Picture1Id, storeScope, false);
            else if (storeScope > 0)
                _settingService.DeleteSetting(nivoSliderSettings, x => x.Picture1Id, storeScope);

            if (model.Text1_OverrideForStore || storeScope == 0)
                _settingService.SaveSetting(nivoSliderSettings, x => x.Text1, storeScope, false);
            else if (storeScope > 0)
                _settingService.DeleteSetting(nivoSliderSettings, x => x.Text1, storeScope);

            if (model.Link1_OverrideForStore || storeScope == 0)
                _settingService.SaveSetting(nivoSliderSettings, x => x.Link1, storeScope, false);
            else if (storeScope > 0)
                _settingService.DeleteSetting(nivoSliderSettings, x => x.Link1, storeScope);

            if (model.Picture2Id_OverrideForStore || storeScope == 0)
                _settingService.SaveSetting(nivoSliderSettings, x => x.Picture2Id, storeScope, false);
            else if (storeScope > 0)
                _settingService.DeleteSetting(nivoSliderSettings, x => x.Picture2Id, storeScope);

            if (model.Text2_OverrideForStore || storeScope == 0)
                _settingService.SaveSetting(nivoSliderSettings, x => x.Text2, storeScope, false);
            else if (storeScope > 0)
                _settingService.DeleteSetting(nivoSliderSettings, x => x.Text2, storeScope);

            if (model.Link2_OverrideForStore || storeScope == 0)
                _settingService.SaveSetting(nivoSliderSettings, x => x.Link2, storeScope, false);
            else if (storeScope > 0)
                _settingService.DeleteSetting(nivoSliderSettings, x => x.Link2, storeScope);

            if (model.Picture3Id_OverrideForStore || storeScope == 0)
                _settingService.SaveSetting(nivoSliderSettings, x => x.Picture3Id, storeScope, false);
            else if (storeScope > 0)
                _settingService.DeleteSetting(nivoSliderSettings, x => x.Picture3Id, storeScope);

            if (model.Text3_OverrideForStore || storeScope == 0)
                _settingService.SaveSetting(nivoSliderSettings, x => x.Text3, storeScope, false);
            else if (storeScope > 0)
                _settingService.DeleteSetting(nivoSliderSettings, x => x.Text3, storeScope);

            if (model.Link3_OverrideForStore || storeScope == 0)
                _settingService.SaveSetting(nivoSliderSettings, x => x.Link3, storeScope, false);
            else if (storeScope > 0)
                _settingService.DeleteSetting(nivoSliderSettings, x => x.Link3, storeScope);



            //now clear settings cache
            _settingService.ClearCache();

            Response.Redirect("/Admin/Widget/ConfigureWidget?systemName=Widgets.NivoSlider");
            return null;
        }

        [ChildActionOnly]
        public ActionResult PublicInfoRoot(string widgetZone)
        {
            var nivoSliderSettings = _settingService.LoadSetting<NivoIndexSliderRootSettings>(_storeContext.CurrentStore.Id);

            var model = new PublicInfoIndexSliderRootModel();
            model.Picture1Url = GetPictureUrl(nivoSliderSettings.Picture1Id);
            model.Text1 = nivoSliderSettings.Text1;
            model.Link1 = nivoSliderSettings.Link1;

            model.Picture2Url = GetPictureUrl(nivoSliderSettings.Picture2Id);
            model.Text2 = nivoSliderSettings.Text2;
            model.Link2 = nivoSliderSettings.Link2;

            model.Picture3Url = GetPictureUrl(nivoSliderSettings.Picture3Id);
            model.Text3 = nivoSliderSettings.Text3;
            model.Link3 = nivoSliderSettings.Link3;



            //if (string.IsNullOrEmpty(model.Picture1Url) && string.IsNullOrEmpty(model.Picture2Url) &&
            //    string.IsNullOrEmpty(model.Picture3Url))
            //    //no pictures uploaded
            //    return Content("");


            return View("Nop.Plugin.Widgets.NivoSlider.Views.WidgetsNivoSlider.PublicInfoIndexSliderRoot", model);
        }

        #endregion root

        #region Hot

        [HttpPost]
        [AdminAuthorize]
        public ActionResult ConfigureHot(ConfigurationHotModel model)
        {
            //load settings for a chosen store scope
            var storeScope = this.GetActiveStoreScopeConfiguration(_storeService, _workContext);
            var nivoSliderSettings = _settingService.LoadSetting<NivoSliderHotSettings>(storeScope);
            nivoSliderSettings.Picture1Id = model.Picture1Id;
            nivoSliderSettings.Text1 = model.Text1;
            nivoSliderSettings.Description1 = model.Description1;
            nivoSliderSettings.Link1 = model.Link1;
            nivoSliderSettings.Picture2Id = model.Picture2Id;
            nivoSliderSettings.Text2 = model.Text2;
            nivoSliderSettings.Description2 = model.Description2;
            nivoSliderSettings.Link2 = model.Link2;
            nivoSliderSettings.Picture3Id = model.Picture3Id;
            nivoSliderSettings.Text3 = model.Text3;
            nivoSliderSettings.Description3 = model.Description3;
            nivoSliderSettings.Link3 = model.Link3;
            nivoSliderSettings.Picture4Id = model.Picture4Id;
            nivoSliderSettings.Text4 = model.Text4;
            nivoSliderSettings.Description4 = model.Description4;
            nivoSliderSettings.Link4 = model.Link4;

            /* We do not clear cache after each setting update.
             * This behavior can increase performance because cached settings will not be cleared 
             * and loaded from database after each update */
            if (model.Picture1Id_OverrideForStore || storeScope == 0)
                _settingService.SaveSetting(nivoSliderSettings, x => x.Picture1Id, storeScope, false);
            else if (storeScope > 0)
                _settingService.DeleteSetting(nivoSliderSettings, x => x.Picture1Id, storeScope);

            if (model.Text1_OverrideForStore || storeScope == 0)
                _settingService.SaveSetting(nivoSliderSettings, x => x.Text1, storeScope, false);
            else if (storeScope > 0)
                _settingService.DeleteSetting(nivoSliderSettings, x => x.Text1, storeScope);

            if (model.Description1_OverrideForStore || storeScope == 0)
                _settingService.SaveSetting(nivoSliderSettings, x => x.Description1, storeScope, false);
            else if (storeScope > 0)
                _settingService.DeleteSetting(nivoSliderSettings, x => x.Description1, storeScope);

            if (model.Link1_OverrideForStore || storeScope == 0)
                _settingService.SaveSetting(nivoSliderSettings, x => x.Link1, storeScope, false);
            else if (storeScope > 0)
                _settingService.DeleteSetting(nivoSliderSettings, x => x.Link1, storeScope);

            if (model.Picture2Id_OverrideForStore || storeScope == 0)
                _settingService.SaveSetting(nivoSliderSettings, x => x.Picture2Id, storeScope, false);
            else if (storeScope > 0)
                _settingService.DeleteSetting(nivoSliderSettings, x => x.Picture2Id, storeScope);

            if (model.Text2_OverrideForStore || storeScope == 0)
                _settingService.SaveSetting(nivoSliderSettings, x => x.Text2, storeScope, false);
            else if (storeScope > 0)
                _settingService.DeleteSetting(nivoSliderSettings, x => x.Text2, storeScope);

            if (model.Description2_OverrideForStore || storeScope == 0)
                _settingService.SaveSetting(nivoSliderSettings, x => x.Description2, storeScope, false);
            else if (storeScope > 0)
                _settingService.DeleteSetting(nivoSliderSettings, x => x.Description2, storeScope);

            if (model.Link2_OverrideForStore || storeScope == 0)
                _settingService.SaveSetting(nivoSliderSettings, x => x.Link2, storeScope, false);
            else if (storeScope > 0)
                _settingService.DeleteSetting(nivoSliderSettings, x => x.Link2, storeScope);

            if (model.Picture3Id_OverrideForStore || storeScope == 0)
                _settingService.SaveSetting(nivoSliderSettings, x => x.Picture3Id, storeScope, false);
            else if (storeScope > 0)
                _settingService.DeleteSetting(nivoSliderSettings, x => x.Picture3Id, storeScope);

            if (model.Text3_OverrideForStore || storeScope == 0)
                _settingService.SaveSetting(nivoSliderSettings, x => x.Text3, storeScope, false);
            else if (storeScope > 0)
                _settingService.DeleteSetting(nivoSliderSettings, x => x.Text3, storeScope);

            if (model.Description3_OverrideForStore || storeScope == 0)
                _settingService.SaveSetting(nivoSliderSettings, x => x.Description3, storeScope, false);
            else if (storeScope > 0)
                _settingService.DeleteSetting(nivoSliderSettings, x => x.Description3, storeScope);

            if (model.Link3_OverrideForStore || storeScope == 0)
                _settingService.SaveSetting(nivoSliderSettings, x => x.Link3, storeScope, false);
            else if (storeScope > 0)
                _settingService.DeleteSetting(nivoSliderSettings, x => x.Link3, storeScope);

            if (model.Picture4Id_OverrideForStore || storeScope == 0)
                _settingService.SaveSetting(nivoSliderSettings, x => x.Picture4Id, storeScope, false);
            else if (storeScope > 0)
                _settingService.DeleteSetting(nivoSliderSettings, x => x.Picture4Id, storeScope);

            if (model.Text4_OverrideForStore || storeScope == 0)
                _settingService.SaveSetting(nivoSliderSettings, x => x.Text4, storeScope, false);
            else if (storeScope > 0)
                _settingService.DeleteSetting(nivoSliderSettings, x => x.Text4, storeScope);

            if (model.Description4_OverrideForStore || storeScope == 0)
                _settingService.SaveSetting(nivoSliderSettings, x => x.Description4, storeScope, false);
            else if (storeScope > 0)
                _settingService.DeleteSetting(nivoSliderSettings, x => x.Description4, storeScope);

            if (model.Link4_OverrideForStore || storeScope == 0)
                _settingService.SaveSetting(nivoSliderSettings, x => x.Link4, storeScope, false);
            else if (storeScope > 0)
                _settingService.DeleteSetting(nivoSliderSettings, x => x.Link4, storeScope);

            //now clear settings cache
            _settingService.ClearCache();

            Response.Redirect("/Admin/Widget/ConfigureWidget?systemName=Widgets.NivoSlider");
            return null;
        }


        public ActionResult PublicInfoHot(string widgetZone)
        {
            var nivoSliderSettings = _settingService.LoadSetting<NivoSliderHotSettings>(_storeContext.CurrentStore.Id);

            var model = new PublicInfoHotModel();
            model.Picture1Url = GetPictureUrl(nivoSliderSettings.Picture1Id);
            model.Text1 = nivoSliderSettings.Text1;
            model.Description1 = nivoSliderSettings.Description1;
            model.Link1 = nivoSliderSettings.Link1;

            model.Picture2Url = GetPictureUrl(nivoSliderSettings.Picture2Id);
            model.Text2 = nivoSliderSettings.Text2;
            model.Description2 = nivoSliderSettings.Description2;
            model.Link2 = nivoSliderSettings.Link2;

            model.Picture3Url = GetPictureUrl(nivoSliderSettings.Picture3Id);
            model.Text3 = nivoSliderSettings.Text3;
            model.Description3 = nivoSliderSettings.Description3;
            model.Link3 = nivoSliderSettings.Link3;

            model.Picture4Url = GetPictureUrl(nivoSliderSettings.Picture4Id);
            model.Text4 = nivoSliderSettings.Text4;
            model.Description4 = nivoSliderSettings.Description4;
            model.Link4 = nivoSliderSettings.Link4;


            //if (string.IsNullOrEmpty(model.Picture1Url) && string.IsNullOrEmpty(model.Picture2Url) &&
            //    string.IsNullOrEmpty(model.Picture3Url) && string.IsNullOrEmpty(model.Picture4Url) &&
            //    string.IsNullOrEmpty(model.Picture5Url))
            //    //no pictures uploaded
            //    return Content("");


            return View("Nop.Plugin.Widgets.NivoSlider.Views.WidgetsNivoSlider.PublicInfoHot", model);
        }
        #endregion Hot

        #region New

        [HttpPost]
        [AdminAuthorize]
        public ActionResult ConfigureNew(ConfigurationNewModel model)
        {
            //load settings for a chosen store scope
            var storeScope = this.GetActiveStoreScopeConfiguration(_storeService, _workContext);
            var nivoSliderSettings = _settingService.LoadSetting<NivoSliderNewSettings>(storeScope);
            nivoSliderSettings.Picture1Id = model.Picture1Id;
            nivoSliderSettings.Text1 = model.Text1;
            nivoSliderSettings.Link1 = model.Link1;
            nivoSliderSettings.Picture2Id = model.Picture2Id;
            nivoSliderSettings.Text2 = model.Text2;
            nivoSliderSettings.Link2 = model.Link2;
            nivoSliderSettings.Picture3Id = model.Picture3Id;
            nivoSliderSettings.Text3 = model.Text3;
            nivoSliderSettings.Link3 = model.Link3;
            nivoSliderSettings.Picture4Id = model.Picture4Id;
            nivoSliderSettings.Text4 = model.Text4;
            nivoSliderSettings.Link4 = model.Link4;
            nivoSliderSettings.Picture5Id = model.Picture5Id;
            nivoSliderSettings.Text5 = model.Text5;
            nivoSliderSettings.Link5 = model.Link5;
            nivoSliderSettings.Picture6Id = model.Picture6Id;
            nivoSliderSettings.Text6 = model.Text6;
            nivoSliderSettings.Link6 = model.Link6;
            nivoSliderSettings.Picture7Id = model.Picture7Id;
            nivoSliderSettings.Text7 = model.Text7;
            nivoSliderSettings.Link7 = model.Link7;
            nivoSliderSettings.Picture8Id = model.Picture8Id;
            nivoSliderSettings.Text8 = model.Text8;
            nivoSliderSettings.Link8 = model.Link8;

            /* We do not clear cache after each setting update.
             * This behavior can increase performance because cached settings will not be cleared 
             * and loaded from database after each update */
            if (model.Picture1Id_OverrideForStore || storeScope == 0)
                _settingService.SaveSetting(nivoSliderSettings, x => x.Picture1Id, storeScope, false);
            else if (storeScope > 0)
                _settingService.DeleteSetting(nivoSliderSettings, x => x.Picture1Id, storeScope);

            if (model.Text1_OverrideForStore || storeScope == 0)
                _settingService.SaveSetting(nivoSliderSettings, x => x.Text1, storeScope, false);
            else if (storeScope > 0)
                _settingService.DeleteSetting(nivoSliderSettings, x => x.Text1, storeScope);

            if (model.Link1_OverrideForStore || storeScope == 0)
                _settingService.SaveSetting(nivoSliderSettings, x => x.Link1, storeScope, false);
            else if (storeScope > 0)
                _settingService.DeleteSetting(nivoSliderSettings, x => x.Link1, storeScope);

            if (model.Picture2Id_OverrideForStore || storeScope == 0)
                _settingService.SaveSetting(nivoSliderSettings, x => x.Picture2Id, storeScope, false);
            else if (storeScope > 0)
                _settingService.DeleteSetting(nivoSliderSettings, x => x.Picture2Id, storeScope);

            if (model.Text2_OverrideForStore || storeScope == 0)
                _settingService.SaveSetting(nivoSliderSettings, x => x.Text2, storeScope, false);
            else if (storeScope > 0)
                _settingService.DeleteSetting(nivoSliderSettings, x => x.Text2, storeScope);

            if (model.Link2_OverrideForStore || storeScope == 0)
                _settingService.SaveSetting(nivoSliderSettings, x => x.Link2, storeScope, false);
            else if (storeScope > 0)
                _settingService.DeleteSetting(nivoSliderSettings, x => x.Link2, storeScope);

            if (model.Picture3Id_OverrideForStore || storeScope == 0)
                _settingService.SaveSetting(nivoSliderSettings, x => x.Picture3Id, storeScope, false);
            else if (storeScope > 0)
                _settingService.DeleteSetting(nivoSliderSettings, x => x.Picture3Id, storeScope);

            if (model.Text3_OverrideForStore || storeScope == 0)
                _settingService.SaveSetting(nivoSliderSettings, x => x.Text3, storeScope, false);
            else if (storeScope > 0)
                _settingService.DeleteSetting(nivoSliderSettings, x => x.Text3, storeScope);

            if (model.Link3_OverrideForStore || storeScope == 0)
                _settingService.SaveSetting(nivoSliderSettings, x => x.Link3, storeScope, false);
            else if (storeScope > 0)
                _settingService.DeleteSetting(nivoSliderSettings, x => x.Link3, storeScope);

            if (model.Picture4Id_OverrideForStore || storeScope == 0)
                _settingService.SaveSetting(nivoSliderSettings, x => x.Picture4Id, storeScope, false);
            else if (storeScope > 0)
                _settingService.DeleteSetting(nivoSliderSettings, x => x.Picture4Id, storeScope);

            if (model.Text4_OverrideForStore || storeScope == 0)
                _settingService.SaveSetting(nivoSliderSettings, x => x.Text4, storeScope, false);
            else if (storeScope > 0)
                _settingService.DeleteSetting(nivoSliderSettings, x => x.Text4, storeScope);

            if (model.Link4_OverrideForStore || storeScope == 0)
                _settingService.SaveSetting(nivoSliderSettings, x => x.Link4, storeScope, false);
            else if (storeScope > 0)
                _settingService.DeleteSetting(nivoSliderSettings, x => x.Link4, storeScope);

            if (model.Picture5Id_OverrideForStore || storeScope == 0)
                _settingService.SaveSetting(nivoSliderSettings, x => x.Picture5Id, storeScope, false);
            else if (storeScope > 0)
                _settingService.DeleteSetting(nivoSliderSettings, x => x.Picture5Id, storeScope);

            if (model.Text5_OverrideForStore || storeScope == 0)
                _settingService.SaveSetting(nivoSliderSettings, x => x.Text5, storeScope, false);
            else if (storeScope > 0)
                _settingService.DeleteSetting(nivoSliderSettings, x => x.Text5, storeScope);

            if (model.Link5_OverrideForStore || storeScope == 0)
                _settingService.SaveSetting(nivoSliderSettings, x => x.Link5, storeScope, false);
            else if (storeScope > 0)
                _settingService.DeleteSetting(nivoSliderSettings, x => x.Link5, storeScope);

            //6
            if (model.Picture6Id_OverrideForStore || storeScope == 0)
                _settingService.SaveSetting(nivoSliderSettings, x => x.Picture6Id, storeScope, false);
            else if (storeScope > 0)
                _settingService.DeleteSetting(nivoSliderSettings, x => x.Picture6Id, storeScope);

            if (model.Text6_OverrideForStore || storeScope == 0)
                _settingService.SaveSetting(nivoSliderSettings, x => x.Text6, storeScope, false);
            else if (storeScope > 0)
                _settingService.DeleteSetting(nivoSliderSettings, x => x.Text6, storeScope);

            if (model.Link6_OverrideForStore || storeScope == 0)
                _settingService.SaveSetting(nivoSliderSettings, x => x.Link6, storeScope, false);
            else if (storeScope > 0)
                _settingService.DeleteSetting(nivoSliderSettings, x => x.Link6, storeScope);
            //7
            if (model.Picture7Id_OverrideForStore || storeScope == 0)
                _settingService.SaveSetting(nivoSliderSettings, x => x.Picture7Id, storeScope, false);
            else if (storeScope > 0)
                _settingService.DeleteSetting(nivoSliderSettings, x => x.Picture7Id, storeScope);

            if (model.Text7_OverrideForStore || storeScope == 0)
                _settingService.SaveSetting(nivoSliderSettings, x => x.Text7, storeScope, false);
            else if (storeScope > 0)
                _settingService.DeleteSetting(nivoSliderSettings, x => x.Text7, storeScope);

            if (model.Link7_OverrideForStore || storeScope == 0)
                _settingService.SaveSetting(nivoSliderSettings, x => x.Link7, storeScope, false);
            else if (storeScope > 0)
                _settingService.DeleteSetting(nivoSliderSettings, x => x.Link7, storeScope);
            //8
            if (model.Picture8Id_OverrideForStore || storeScope == 0)
                _settingService.SaveSetting(nivoSliderSettings, x => x.Picture8Id, storeScope, false);
            else if (storeScope > 0)
                _settingService.DeleteSetting(nivoSliderSettings, x => x.Picture8Id, storeScope);

            if (model.Text8_OverrideForStore || storeScope == 0)
                _settingService.SaveSetting(nivoSliderSettings, x => x.Text8, storeScope, false);
            else if (storeScope > 0)
                _settingService.DeleteSetting(nivoSliderSettings, x => x.Text8, storeScope);

            if (model.Link8_OverrideForStore || storeScope == 0)
                _settingService.SaveSetting(nivoSliderSettings, x => x.Link8, storeScope, false);
            else if (storeScope > 0)
                _settingService.DeleteSetting(nivoSliderSettings, x => x.Link8, storeScope);

            //now clear settings cache
            _settingService.ClearCache();

            Response.Redirect("/Admin/Widget/ConfigureWidget?systemName=Widgets.NivoSlider");
            return null;
        }


        public ActionResult PublicInfoNew(string widgetZone)
        {
            var nivoSliderSettings = _settingService.LoadSetting<NivoSliderNewSettings>(_storeContext.CurrentStore.Id);

            var model = new PublicInfoNewModel();
            model.Picture1Url = GetPictureUrl(nivoSliderSettings.Picture1Id);
            model.Text1 = nivoSliderSettings.Text1;
            model.Link1 = nivoSliderSettings.Link1;

            model.Picture2Url = GetPictureUrl(nivoSliderSettings.Picture2Id);
            model.Text2 = nivoSliderSettings.Text2;
            model.Link2 = nivoSliderSettings.Link2;

            model.Picture3Url = GetPictureUrl(nivoSliderSettings.Picture3Id);
            model.Text3 = nivoSliderSettings.Text3;
            model.Link3 = nivoSliderSettings.Link3;

            model.Picture4Url = GetPictureUrl(nivoSliderSettings.Picture4Id);
            model.Text4 = nivoSliderSettings.Text4;
            model.Link4 = nivoSliderSettings.Link4;

            model.Picture5Url = GetPictureUrl(nivoSliderSettings.Picture5Id);
            model.Text5 = nivoSliderSettings.Text5;
            model.Link5 = nivoSliderSettings.Link5;

            model.Picture6Url = GetPictureUrl(nivoSliderSettings.Picture6Id);
            model.Text6 = nivoSliderSettings.Text6;
            model.Link6 = nivoSliderSettings.Link6;

            model.Picture7Url = GetPictureUrl(nivoSliderSettings.Picture7Id);
            model.Text7 = nivoSliderSettings.Text7;
            model.Link7 = nivoSliderSettings.Link7;

            model.Picture8Url = GetPictureUrl(nivoSliderSettings.Picture8Id);
            model.Text8 = nivoSliderSettings.Text8;
            model.Link8 = nivoSliderSettings.Link8;

            //if (string.IsNullOrEmpty(model.Picture1Url) && string.IsNullOrEmpty(model.Picture2Url) &&
            //    string.IsNullOrEmpty(model.Picture3Url) && string.IsNullOrEmpty(model.Picture4Url) &&
            //    string.IsNullOrEmpty(model.Picture5Url))
            //    //no pictures uploaded
            //    return Content("");


            return View("Nop.Plugin.Widgets.NivoSlider.Views.WidgetsNivoSlider.PublicInfoNew", model);
        }
        #endregion New

        #region Star

        [HttpPost]
        [AdminAuthorize]
        public ActionResult ConfigureStar(ConfigurationStarModel model)
        {
            //load settings for a chosen store scope
            var storeScope = this.GetActiveStoreScopeConfiguration(_storeService, _workContext);
            var nivoSliderSettings = _settingService.LoadSetting<NivoSliderStarSettings>(storeScope);
            nivoSliderSettings.Picture1Id = model.Picture1Id;
            nivoSliderSettings.Text1 = model.Text1;
            nivoSliderSettings.Link1 = model.Link1;
            nivoSliderSettings.Picture2Id = model.Picture2Id;
            nivoSliderSettings.Text2 = model.Text2;
            nivoSliderSettings.Link2 = model.Link2;
            nivoSliderSettings.Picture3Id = model.Picture3Id;
            nivoSliderSettings.Text3 = model.Text3;
            nivoSliderSettings.Link3 = model.Link3;
            nivoSliderSettings.Picture4Id = model.Picture4Id;
            nivoSliderSettings.Text4 = model.Text4;
            nivoSliderSettings.Link4 = model.Link4;
            nivoSliderSettings.Picture5Id = model.Picture5Id;
            nivoSliderSettings.Text5 = model.Text5;
            nivoSliderSettings.Link5 = model.Link5;
            nivoSliderSettings.Picture6Id = model.Picture6Id;
            nivoSliderSettings.Text6 = model.Text6;
            nivoSliderSettings.Link6 = model.Link6;
            nivoSliderSettings.Picture7Id = model.Picture7Id;
            nivoSliderSettings.Text7 = model.Text7;
            nivoSliderSettings.Link7 = model.Link7;
            nivoSliderSettings.Picture8Id = model.Picture8Id;
            nivoSliderSettings.Text8 = model.Text8;
            nivoSliderSettings.Link8 = model.Link8;

            /* We do not clear cache after each setting update.
             * This behavior can increase performance because cached settings will not be cleared 
             * and loaded from database after each update */
            if (model.Picture1Id_OverrideForStore || storeScope == 0)
                _settingService.SaveSetting(nivoSliderSettings, x => x.Picture1Id, storeScope, false);
            else if (storeScope > 0)
                _settingService.DeleteSetting(nivoSliderSettings, x => x.Picture1Id, storeScope);

            if (model.Text1_OverrideForStore || storeScope == 0)
                _settingService.SaveSetting(nivoSliderSettings, x => x.Text1, storeScope, false);
            else if (storeScope > 0)
                _settingService.DeleteSetting(nivoSliderSettings, x => x.Text1, storeScope);

            if (model.Link1_OverrideForStore || storeScope == 0)
                _settingService.SaveSetting(nivoSliderSettings, x => x.Link1, storeScope, false);
            else if (storeScope > 0)
                _settingService.DeleteSetting(nivoSliderSettings, x => x.Link1, storeScope);

            if (model.Picture2Id_OverrideForStore || storeScope == 0)
                _settingService.SaveSetting(nivoSliderSettings, x => x.Picture2Id, storeScope, false);
            else if (storeScope > 0)
                _settingService.DeleteSetting(nivoSliderSettings, x => x.Picture2Id, storeScope);

            if (model.Text2_OverrideForStore || storeScope == 0)
                _settingService.SaveSetting(nivoSliderSettings, x => x.Text2, storeScope, false);
            else if (storeScope > 0)
                _settingService.DeleteSetting(nivoSliderSettings, x => x.Text2, storeScope);

            if (model.Link2_OverrideForStore || storeScope == 0)
                _settingService.SaveSetting(nivoSliderSettings, x => x.Link2, storeScope, false);
            else if (storeScope > 0)
                _settingService.DeleteSetting(nivoSliderSettings, x => x.Link2, storeScope);

            if (model.Picture3Id_OverrideForStore || storeScope == 0)
                _settingService.SaveSetting(nivoSliderSettings, x => x.Picture3Id, storeScope, false);
            else if (storeScope > 0)
                _settingService.DeleteSetting(nivoSliderSettings, x => x.Picture3Id, storeScope);

            if (model.Text3_OverrideForStore || storeScope == 0)
                _settingService.SaveSetting(nivoSliderSettings, x => x.Text3, storeScope, false);
            else if (storeScope > 0)
                _settingService.DeleteSetting(nivoSliderSettings, x => x.Text3, storeScope);

            if (model.Link3_OverrideForStore || storeScope == 0)
                _settingService.SaveSetting(nivoSliderSettings, x => x.Link3, storeScope, false);
            else if (storeScope > 0)
                _settingService.DeleteSetting(nivoSliderSettings, x => x.Link3, storeScope);

            if (model.Picture4Id_OverrideForStore || storeScope == 0)
                _settingService.SaveSetting(nivoSliderSettings, x => x.Picture4Id, storeScope, false);
            else if (storeScope > 0)
                _settingService.DeleteSetting(nivoSliderSettings, x => x.Picture4Id, storeScope);

            if (model.Text4_OverrideForStore || storeScope == 0)
                _settingService.SaveSetting(nivoSliderSettings, x => x.Text4, storeScope, false);
            else if (storeScope > 0)
                _settingService.DeleteSetting(nivoSliderSettings, x => x.Text4, storeScope);

            if (model.Link4_OverrideForStore || storeScope == 0)
                _settingService.SaveSetting(nivoSliderSettings, x => x.Link4, storeScope, false);
            else if (storeScope > 0)
                _settingService.DeleteSetting(nivoSliderSettings, x => x.Link4, storeScope);

            if (model.Picture5Id_OverrideForStore || storeScope == 0)
                _settingService.SaveSetting(nivoSliderSettings, x => x.Picture5Id, storeScope, false);
            else if (storeScope > 0)
                _settingService.DeleteSetting(nivoSliderSettings, x => x.Picture5Id, storeScope);

            if (model.Text5_OverrideForStore || storeScope == 0)
                _settingService.SaveSetting(nivoSliderSettings, x => x.Text5, storeScope, false);
            else if (storeScope > 0)
                _settingService.DeleteSetting(nivoSliderSettings, x => x.Text5, storeScope);

            if (model.Link5_OverrideForStore || storeScope == 0)
                _settingService.SaveSetting(nivoSliderSettings, x => x.Link5, storeScope, false);
            else if (storeScope > 0)
                _settingService.DeleteSetting(nivoSliderSettings, x => x.Link5, storeScope);

            //6
            if (model.Picture6Id_OverrideForStore || storeScope == 0)
                _settingService.SaveSetting(nivoSliderSettings, x => x.Picture6Id, storeScope, false);
            else if (storeScope > 0)
                _settingService.DeleteSetting(nivoSliderSettings, x => x.Picture6Id, storeScope);

            if (model.Text6_OverrideForStore || storeScope == 0)
                _settingService.SaveSetting(nivoSliderSettings, x => x.Text6, storeScope, false);
            else if (storeScope > 0)
                _settingService.DeleteSetting(nivoSliderSettings, x => x.Text6, storeScope);

            if (model.Link6_OverrideForStore || storeScope == 0)
                _settingService.SaveSetting(nivoSliderSettings, x => x.Link6, storeScope, false);
            else if (storeScope > 0)
                _settingService.DeleteSetting(nivoSliderSettings, x => x.Link6, storeScope);
            //7
            if (model.Picture7Id_OverrideForStore || storeScope == 0)
                _settingService.SaveSetting(nivoSliderSettings, x => x.Picture7Id, storeScope, false);
            else if (storeScope > 0)
                _settingService.DeleteSetting(nivoSliderSettings, x => x.Picture7Id, storeScope);

            if (model.Text7_OverrideForStore || storeScope == 0)
                _settingService.SaveSetting(nivoSliderSettings, x => x.Text7, storeScope, false);
            else if (storeScope > 0)
                _settingService.DeleteSetting(nivoSliderSettings, x => x.Text7, storeScope);

            if (model.Link7_OverrideForStore || storeScope == 0)
                _settingService.SaveSetting(nivoSliderSettings, x => x.Link7, storeScope, false);
            else if (storeScope > 0)
                _settingService.DeleteSetting(nivoSliderSettings, x => x.Link7, storeScope);
            //8
            if (model.Picture8Id_OverrideForStore || storeScope == 0)
                _settingService.SaveSetting(nivoSliderSettings, x => x.Picture8Id, storeScope, false);
            else if (storeScope > 0)
                _settingService.DeleteSetting(nivoSliderSettings, x => x.Picture8Id, storeScope);

            if (model.Text8_OverrideForStore || storeScope == 0)
                _settingService.SaveSetting(nivoSliderSettings, x => x.Text8, storeScope, false);
            else if (storeScope > 0)
                _settingService.DeleteSetting(nivoSliderSettings, x => x.Text8, storeScope);

            if (model.Link8_OverrideForStore || storeScope == 0)
                _settingService.SaveSetting(nivoSliderSettings, x => x.Link8, storeScope, false);
            else if (storeScope > 0)
                _settingService.DeleteSetting(nivoSliderSettings, x => x.Link8, storeScope);

            //now clear settings cache
            _settingService.ClearCache();

            Response.Redirect("/Admin/Widget/ConfigureWidget?systemName=Widgets.NivoSlider");
            return null;
        }


        public ActionResult PublicInfoStar(string widgetZone)
        {
            var nivoSliderSettings = _settingService.LoadSetting<NivoSliderStarSettings>(_storeContext.CurrentStore.Id);

            var model = new PublicInfoStarModel();
            model.Picture1Url = GetPictureUrl(nivoSliderSettings.Picture1Id);
            model.Text1 = nivoSliderSettings.Text1;
            model.Link1 = nivoSliderSettings.Link1;

            model.Picture2Url = GetPictureUrl(nivoSliderSettings.Picture2Id);
            model.Text2 = nivoSliderSettings.Text2;
            model.Link2 = nivoSliderSettings.Link2;

            model.Picture3Url = GetPictureUrl(nivoSliderSettings.Picture3Id);
            model.Text3 = nivoSliderSettings.Text3;
            model.Link3 = nivoSliderSettings.Link3;

            model.Picture4Url = GetPictureUrl(nivoSliderSettings.Picture4Id);
            model.Text4 = nivoSliderSettings.Text4;
            model.Link4 = nivoSliderSettings.Link4;

            model.Picture5Url = GetPictureUrl(nivoSliderSettings.Picture5Id);
            model.Text5 = nivoSliderSettings.Text5;
            model.Link5 = nivoSliderSettings.Link5;

            model.Picture6Url = GetPictureUrl(nivoSliderSettings.Picture6Id);
            model.Text6 = nivoSliderSettings.Text6;
            model.Link6 = nivoSliderSettings.Link6;

            model.Picture7Url = GetPictureUrl(nivoSliderSettings.Picture7Id);
            model.Text7 = nivoSliderSettings.Text7;
            model.Link7 = nivoSliderSettings.Link7;

            model.Picture8Url = GetPictureUrl(nivoSliderSettings.Picture8Id);
            model.Text8 = nivoSliderSettings.Text8;
            model.Link8 = nivoSliderSettings.Link8;

            //if (string.IsNullOrEmpty(model.Picture1Url) && string.IsNullOrEmpty(model.Picture2Url) &&
            //    string.IsNullOrEmpty(model.Picture3Url) && string.IsNullOrEmpty(model.Picture4Url) &&
            //    string.IsNullOrEmpty(model.Picture5Url))
            //    //no pictures uploaded
            //    return Content("");


            return View("Nop.Plugin.Widgets.NivoSlider.Views.WidgetsNivoSlider.PublicInfoStar", model);
        }
        #endregion Star

        #region Like

        [HttpPost]
        [AdminAuthorize]
        public ActionResult ConfigureLike(ConfigurationLikeModel model)
        {
            //load settings for a chosen store scope
            var storeScope = this.GetActiveStoreScopeConfiguration(_storeService, _workContext);
            var nivoSliderSettings = _settingService.LoadSetting<NivoSliderLikeSettings>(storeScope);
            nivoSliderSettings.Picture1Id = model.Picture1Id;
            nivoSliderSettings.Text1 = model.Text1;
            nivoSliderSettings.Link1 = model.Link1;
            nivoSliderSettings.Picture2Id = model.Picture2Id;
            nivoSliderSettings.Text2 = model.Text2;
            nivoSliderSettings.Link2 = model.Link2;
            nivoSliderSettings.Picture3Id = model.Picture3Id;
            nivoSliderSettings.Text3 = model.Text3;
            nivoSliderSettings.Link3 = model.Link3;
            nivoSliderSettings.Picture4Id = model.Picture4Id;
            nivoSliderSettings.Text4 = model.Text4;
            nivoSliderSettings.Link4 = model.Link4;
            nivoSliderSettings.Picture5Id = model.Picture5Id;
            nivoSliderSettings.Text5 = model.Text5;
            nivoSliderSettings.Link5 = model.Link5;
            nivoSliderSettings.Picture6Id = model.Picture6Id;
            nivoSliderSettings.Text6 = model.Text6;
            nivoSliderSettings.Link6 = model.Link6;
            nivoSliderSettings.Picture7Id = model.Picture7Id;
            nivoSliderSettings.Text7 = model.Text7;
            nivoSliderSettings.Link7 = model.Link7;
            nivoSliderSettings.Picture8Id = model.Picture8Id;
            nivoSliderSettings.Text8 = model.Text8;
            nivoSliderSettings.Link8 = model.Link8;

            /* We do not clear cache after each setting update.
             * This behavior can increase performance because cached settings will not be cleared 
             * and loaded from database after each update */
            if (model.Picture1Id_OverrideForStore || storeScope == 0)
                _settingService.SaveSetting(nivoSliderSettings, x => x.Picture1Id, storeScope, false);
            else if (storeScope > 0)
                _settingService.DeleteSetting(nivoSliderSettings, x => x.Picture1Id, storeScope);

            if (model.Text1_OverrideForStore || storeScope == 0)
                _settingService.SaveSetting(nivoSliderSettings, x => x.Text1, storeScope, false);
            else if (storeScope > 0)
                _settingService.DeleteSetting(nivoSliderSettings, x => x.Text1, storeScope);

            if (model.Link1_OverrideForStore || storeScope == 0)
                _settingService.SaveSetting(nivoSliderSettings, x => x.Link1, storeScope, false);
            else if (storeScope > 0)
                _settingService.DeleteSetting(nivoSliderSettings, x => x.Link1, storeScope);

            if (model.Picture2Id_OverrideForStore || storeScope == 0)
                _settingService.SaveSetting(nivoSliderSettings, x => x.Picture2Id, storeScope, false);
            else if (storeScope > 0)
                _settingService.DeleteSetting(nivoSliderSettings, x => x.Picture2Id, storeScope);

            if (model.Text2_OverrideForStore || storeScope == 0)
                _settingService.SaveSetting(nivoSliderSettings, x => x.Text2, storeScope, false);
            else if (storeScope > 0)
                _settingService.DeleteSetting(nivoSliderSettings, x => x.Text2, storeScope);

            if (model.Link2_OverrideForStore || storeScope == 0)
                _settingService.SaveSetting(nivoSliderSettings, x => x.Link2, storeScope, false);
            else if (storeScope > 0)
                _settingService.DeleteSetting(nivoSliderSettings, x => x.Link2, storeScope);

            if (model.Picture3Id_OverrideForStore || storeScope == 0)
                _settingService.SaveSetting(nivoSliderSettings, x => x.Picture3Id, storeScope, false);
            else if (storeScope > 0)
                _settingService.DeleteSetting(nivoSliderSettings, x => x.Picture3Id, storeScope);

            if (model.Text3_OverrideForStore || storeScope == 0)
                _settingService.SaveSetting(nivoSliderSettings, x => x.Text3, storeScope, false);
            else if (storeScope > 0)
                _settingService.DeleteSetting(nivoSliderSettings, x => x.Text3, storeScope);

            if (model.Link3_OverrideForStore || storeScope == 0)
                _settingService.SaveSetting(nivoSliderSettings, x => x.Link3, storeScope, false);
            else if (storeScope > 0)
                _settingService.DeleteSetting(nivoSliderSettings, x => x.Link3, storeScope);

            if (model.Picture4Id_OverrideForStore || storeScope == 0)
                _settingService.SaveSetting(nivoSliderSettings, x => x.Picture4Id, storeScope, false);
            else if (storeScope > 0)
                _settingService.DeleteSetting(nivoSliderSettings, x => x.Picture4Id, storeScope);

            if (model.Text4_OverrideForStore || storeScope == 0)
                _settingService.SaveSetting(nivoSliderSettings, x => x.Text4, storeScope, false);
            else if (storeScope > 0)
                _settingService.DeleteSetting(nivoSliderSettings, x => x.Text4, storeScope);

            if (model.Link4_OverrideForStore || storeScope == 0)
                _settingService.SaveSetting(nivoSliderSettings, x => x.Link4, storeScope, false);
            else if (storeScope > 0)
                _settingService.DeleteSetting(nivoSliderSettings, x => x.Link4, storeScope);

            if (model.Picture5Id_OverrideForStore || storeScope == 0)
                _settingService.SaveSetting(nivoSliderSettings, x => x.Picture5Id, storeScope, false);
            else if (storeScope > 0)
                _settingService.DeleteSetting(nivoSliderSettings, x => x.Picture5Id, storeScope);

            if (model.Text5_OverrideForStore || storeScope == 0)
                _settingService.SaveSetting(nivoSliderSettings, x => x.Text5, storeScope, false);
            else if (storeScope > 0)
                _settingService.DeleteSetting(nivoSliderSettings, x => x.Text5, storeScope);

            if (model.Link5_OverrideForStore || storeScope == 0)
                _settingService.SaveSetting(nivoSliderSettings, x => x.Link5, storeScope, false);
            else if (storeScope > 0)
                _settingService.DeleteSetting(nivoSliderSettings, x => x.Link5, storeScope);

            //6
            if (model.Picture6Id_OverrideForStore || storeScope == 0)
                _settingService.SaveSetting(nivoSliderSettings, x => x.Picture6Id, storeScope, false);
            else if (storeScope > 0)
                _settingService.DeleteSetting(nivoSliderSettings, x => x.Picture6Id, storeScope);

            if (model.Text6_OverrideForStore || storeScope == 0)
                _settingService.SaveSetting(nivoSliderSettings, x => x.Text6, storeScope, false);
            else if (storeScope > 0)
                _settingService.DeleteSetting(nivoSliderSettings, x => x.Text6, storeScope);

            if (model.Link6_OverrideForStore || storeScope == 0)
                _settingService.SaveSetting(nivoSliderSettings, x => x.Link6, storeScope, false);
            else if (storeScope > 0)
                _settingService.DeleteSetting(nivoSliderSettings, x => x.Link6, storeScope);
            //7
            if (model.Picture7Id_OverrideForStore || storeScope == 0)
                _settingService.SaveSetting(nivoSliderSettings, x => x.Picture7Id, storeScope, false);
            else if (storeScope > 0)
                _settingService.DeleteSetting(nivoSliderSettings, x => x.Picture7Id, storeScope);

            if (model.Text7_OverrideForStore || storeScope == 0)
                _settingService.SaveSetting(nivoSliderSettings, x => x.Text7, storeScope, false);
            else if (storeScope > 0)
                _settingService.DeleteSetting(nivoSliderSettings, x => x.Text7, storeScope);

            if (model.Link7_OverrideForStore || storeScope == 0)
                _settingService.SaveSetting(nivoSliderSettings, x => x.Link7, storeScope, false);
            else if (storeScope > 0)
                _settingService.DeleteSetting(nivoSliderSettings, x => x.Link7, storeScope);
            //8
            if (model.Picture8Id_OverrideForStore || storeScope == 0)
                _settingService.SaveSetting(nivoSliderSettings, x => x.Picture8Id, storeScope, false);
            else if (storeScope > 0)
                _settingService.DeleteSetting(nivoSliderSettings, x => x.Picture8Id, storeScope);

            if (model.Text8_OverrideForStore || storeScope == 0)
                _settingService.SaveSetting(nivoSliderSettings, x => x.Text8, storeScope, false);
            else if (storeScope > 0)
                _settingService.DeleteSetting(nivoSliderSettings, x => x.Text8, storeScope);

            if (model.Link8_OverrideForStore || storeScope == 0)
                _settingService.SaveSetting(nivoSliderSettings, x => x.Link8, storeScope, false);
            else if (storeScope > 0)
                _settingService.DeleteSetting(nivoSliderSettings, x => x.Link8, storeScope);

            //now clear settings cache
            _settingService.ClearCache();

            Response.Redirect("/Admin/Widget/ConfigureWidget?systemName=Widgets.NivoSlider");
            return null;
        }


        public ActionResult PublicInfoLike(string widgetZone)
        {
            var nivoSliderSettings = _settingService.LoadSetting<NivoSliderLikeSettings>(_storeContext.CurrentStore.Id);

            var model = new PublicInfoLikeModel();
            model.Picture1Url = GetPictureUrl(nivoSliderSettings.Picture1Id);
            model.Text1 = nivoSliderSettings.Text1;
            model.Link1 = nivoSliderSettings.Link1;

            model.Picture2Url = GetPictureUrl(nivoSliderSettings.Picture2Id);
            model.Text2 = nivoSliderSettings.Text2;
            model.Link2 = nivoSliderSettings.Link2;

            model.Picture3Url = GetPictureUrl(nivoSliderSettings.Picture3Id);
            model.Text3 = nivoSliderSettings.Text3;
            model.Link3 = nivoSliderSettings.Link3;

            model.Picture4Url = GetPictureUrl(nivoSliderSettings.Picture4Id);
            model.Text4 = nivoSliderSettings.Text4;
            model.Link4 = nivoSliderSettings.Link4;

            model.Picture5Url = GetPictureUrl(nivoSliderSettings.Picture5Id);
            model.Text5 = nivoSliderSettings.Text5;
            model.Link5 = nivoSliderSettings.Link5;

            model.Picture6Url = GetPictureUrl(nivoSliderSettings.Picture6Id);
            model.Text6 = nivoSliderSettings.Text6;
            model.Link6 = nivoSliderSettings.Link6;

            model.Picture7Url = GetPictureUrl(nivoSliderSettings.Picture7Id);
            model.Text7 = nivoSliderSettings.Text7;
            model.Link7 = nivoSliderSettings.Link7;

            model.Picture8Url = GetPictureUrl(nivoSliderSettings.Picture8Id);
            model.Text8 = nivoSliderSettings.Text8;
            model.Link8 = nivoSliderSettings.Link8;

            //if (string.IsNullOrEmpty(model.Picture1Url) && string.IsNullOrEmpty(model.Picture2Url) &&
            //    string.IsNullOrEmpty(model.Picture3Url) && string.IsNullOrEmpty(model.Picture4Url) &&
            //    string.IsNullOrEmpty(model.Picture5Url))
            //    //no pictures uploaded
            //    return Content("");


            return View("Nop.Plugin.Widgets.NivoSlider.Views.WidgetsNivoSlider.PublicInfoLike", model);
        }
        #endregion Like

        #region ProductAddPopup

        public ActionResult ProductAddPopup(int categoryId)
        {

            var model = new CategoryModel.AddCategoryProductModel();
            //categories
            model.AvailableCategories.Add(new SelectListItem() { Text = _localizationService.GetResource("Admin.Common.All"), Value = "0" });
            var categories = _categoryService.GetAllCategories(showHidden: true);
            foreach (var c in categories)
                model.AvailableCategories.Add(new SelectListItem() { Text = c.GetFormattedBreadCrumb(categories), Value = c.Id.ToString() });

            //manufacturers
            model.AvailableManufacturers.Add(new SelectListItem() { Text = _localizationService.GetResource("Admin.Common.All"), Value = "0" });
            foreach (var m in _manufacturerService.GetAllManufacturers(showHidden: true))
                model.AvailableManufacturers.Add(new SelectListItem() { Text = m.Name, Value = m.Id.ToString() });

            //stores
            model.AvailableStores.Add(new SelectListItem() { Text = _localizationService.GetResource("Admin.Common.All"), Value = "0" });
            foreach (var s in _storeService.GetAllStores())
                model.AvailableStores.Add(new SelectListItem() { Text = s.Name, Value = s.Id.ToString() });

            //vendors
            model.AvailableVendors.Add(new SelectListItem() { Text = _localizationService.GetResource("Admin.Common.All"), Value = "0" });
            foreach (var v in _vendorService.GetAllVendors(0, int.MaxValue, true))
                model.AvailableVendors.Add(new SelectListItem() { Text = v.Name, Value = v.Id.ToString() });

            //product types
            model.AvailableProductTypes = ProductType.SimpleProduct.ToSelectList(false).ToList();
            model.AvailableProductTypes.Insert(0, new SelectListItem() { Text = _localizationService.GetResource("Admin.Common.All"), Value = "0" });

           // Response.Redirect("/Admin/Widget/ConfigureWidget?systemName=Widgets.NivoSlider");
            return this.View("Nop.Plugin.Widgets.NivoSlider.Views.WidgetsNivoSlider.ProductAddPopup", model);
        }

        [HttpPost]
        public ActionResult ProductAddPopupList(DataSourceRequest command, CategoryModel.AddCategoryProductModel model)
        {

            var gridModel = new DataSourceResult();
            var products = _productService.SearchProducts(
                categoryIds: new List<int>() { model.SearchCategoryId },
                manufacturerId: model.SearchManufacturerId,
                storeId: model.SearchStoreId,
                vendorId: model.SearchVendorId,
                productType: model.SearchProductTypeId > 0 ? (ProductType?)model.SearchProductTypeId : null,
                keywords: model.SearchProductName,
                pageIndex: command.Page - 1,
                pageSize: command.PageSize,
                showHidden: true
                );
            gridModel.Data = products.Select(x =>ToModel(x));
            gridModel.Total = products.TotalCount;

            return Json(gridModel);
        }
        public static ProductModel ToModel( Product entity)
        {
            return Mapper.Map<Product, ProductModel>(entity);
        }
        [HttpPost]
        [FormValueRequired("save")]
        public ActionResult ProductAddPopup(string btnId, string formId, CategoryModel.AddCategoryProductModel model)
        {

            if (model.SelectedProductIds != null)
            {
                foreach (int id in model.SelectedProductIds)
                {
                    var product = _productService.GetProductById(id);
                    if (product != null)
                    {
                        var existingProductCategories = _categoryService.GetProductCategoriesByCategoryId(model.CategoryId, 0, int.MaxValue, true);
                        if (existingProductCategories.FindProductCategory(id, model.CategoryId) == null)
                        {
                            _categoryService.InsertProductCategory(
                                new ProductCategory()
                                {
                                    CategoryId = model.CategoryId,
                                    ProductId = id,
                                    IsFeaturedProduct = false,
                                    DisplayOrder = 1
                                });
                        }
                    }
                }
            }

            ViewBag.RefreshPage = true;
            ViewBag.btnId = btnId;
            ViewBag.formId = formId;
            return View("Nop.Plugin.Widgets.NivoSlider.Views.WidgetsNivoSlider.ProductAddPopup", model);
        }
        #endregion
    }
}