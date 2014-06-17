using Nop.Core;
using Nop.Core.Domain.Customers;
using Nop.Core.Plugins;
using Nop.Plugin.ExternalAuth.QQ.Models;
using Nop.Services.Authentication.External;
using Nop.Services.Configuration;
using Nop.Services.Security;
using Nop.Services.Stores;
using Nop.Web.Framework.Controllers;
using System;
using System.Web.Mvc;
using QConnectSDK.Context;
using QConnectSDK.Api;
using QConnectSDK.Config;

namespace Nop.Plugin.ExternalAuth.QQ.Controllers
{
    public class ExternalAuthQQController : BasePluginController
    {
        private readonly ISettingService _settingService;
        private readonly IPermissionService _permissionService;
        private readonly IOpenAuthenticationService _openAuthenticationService;
        private readonly ExternalAuthenticationSettings _externalAuthenticationSettings;
        private readonly IStoreContext _storeContext;
        private readonly IStoreService _storeService;
        private readonly IWorkContext _workContext;
        private readonly IPluginFinder _pluginFinder;

        public ExternalAuthQQController(ISettingService settingService,
            IPermissionService permissionService,
            IOpenAuthenticationService openAuthenticationService,
            ExternalAuthenticationSettings externalAuthenticationSettings,
            IStoreContext storeContext,
            IStoreService storeService,
            IWorkContext workContext,
            IPluginFinder pluginFinder)
        {
            this._settingService = settingService;
            this._permissionService = permissionService;
            this._openAuthenticationService = openAuthenticationService;
            this._externalAuthenticationSettings = externalAuthenticationSettings;
            this._storeContext = storeContext;
            this._storeService = storeService;
            this._workContext = workContext;
            this._pluginFinder = pluginFinder;
        }

        [AdminAuthorize]
        [ChildActionOnly]
        public ActionResult Configure()
        {
            if (!_permissionService.Authorize(StandardPermissionProvider.ManageExternalAuthenticationMethods))
                return Content("Access denied");

            //load settings for a chosen store scope
            var storeScope = this.GetActiveStoreScopeConfiguration(_storeService, _workContext);
            var qqExternalAuthSettings = _settingService.LoadSetting<QQExternalAuthSettings>(storeScope);

            var model = new ConfigurationModel();
            model.AppKey = qqExternalAuthSettings.AppKey;
            model.AppSecret = qqExternalAuthSettings.AppSecret;
            model.CallBackURI = qqExternalAuthSettings.CallBackURI;
            model.AuthorizeURL = qqExternalAuthSettings.AuthorizeURL;
            model.ActiveStoreScopeConfiguration = storeScope;
            if (storeScope > 0)
            {
                model.AppKey_OverrideForStore = _settingService.SettingExists(qqExternalAuthSettings, x => x.AppKey, storeScope);
                model.AppSecret_OverrideForStore = _settingService.SettingExists(qqExternalAuthSettings, x => x.AppSecret, storeScope);
                model.CallBackURI_OverrideForStore = _settingService.SettingExists(qqExternalAuthSettings, x => x.CallBackURI, storeScope);
                model.AuthorizeURL_OverrideForStore = _settingService.SettingExists(qqExternalAuthSettings, x => x.AuthorizeURL, storeScope);
            }

            return View("Nop.Plugin.ExternalAuth.QQ.Views.ExternalAuthQQ.Configure", model);
        }

        [HttpPost]
        [AdminAuthorize]
        [ChildActionOnly]
        public ActionResult Configure(ConfigurationModel model)
        {
            if (!_permissionService.Authorize(StandardPermissionProvider.ManageExternalAuthenticationMethods))
                return Content("Access denied");

            if (!ModelState.IsValid)
                return Configure();

            //load settings for a chosen store scope
            var storeScope = this.GetActiveStoreScopeConfiguration(_storeService, _workContext);
            var qqExternalAuthSettings = _settingService.LoadSetting<QQExternalAuthSettings>(storeScope);

            //save settings
            qqExternalAuthSettings.AppKey = model.AppKey;
            qqExternalAuthSettings.AppSecret = model.AppSecret;
            qqExternalAuthSettings.CallBackURI = model.CallBackURI;
            qqExternalAuthSettings.AuthorizeURL = model.AuthorizeURL;

            /* We do not clear cache after each setting update.
             * This behavior can increase performance because cached settings will not be cleared 
             * and loaded from database after each update */
            if (model.AppKey_OverrideForStore || storeScope == 0)
                _settingService.SaveSetting(qqExternalAuthSettings, x => x.AppKey, storeScope, false);
            else if (storeScope > 0)
                _settingService.DeleteSetting(qqExternalAuthSettings, x => x.AppKey, storeScope);

            if (model.AppSecret_OverrideForStore || storeScope == 0)
                _settingService.SaveSetting(qqExternalAuthSettings, x => x.AppSecret, storeScope, false);
            else if (storeScope > 0)
                _settingService.DeleteSetting(qqExternalAuthSettings, x => x.AppSecret, storeScope);
            if (model.CallBackURI_OverrideForStore || storeScope == 0)
                _settingService.SaveSetting(qqExternalAuthSettings, x => x.CallBackURI, storeScope, false);
            else if (storeScope > 0)
                _settingService.DeleteSetting(qqExternalAuthSettings, x => x.CallBackURI, storeScope);
            if (model.AuthorizeURL_OverrideForStore || storeScope == 0)
                _settingService.SaveSetting(qqExternalAuthSettings, x => x.AuthorizeURL, storeScope, false);
            else if (storeScope > 0)
                _settingService.DeleteSetting(qqExternalAuthSettings, x => x.AuthorizeURL, storeScope);

            //now clear settings cache
            _settingService.ClearCache();

            return Configure();
        }

        [ChildActionOnly]
        public ActionResult PublicInfo()
        {
            return View("Nop.Plugin.ExternalAuth.QQ.Views.ExternalAuthQQ.PublicInfo");
        }

        [NonAction]
        private ActionResult LoginInternal(string returnUrl, bool verifyResponse)
        {
            var processor = _openAuthenticationService.LoadExternalAuthenticationMethodBySystemName("ExternalAuth.QQ");
            if (processor == null ||
                !processor.IsMethodActive(_externalAuthenticationSettings) ||
                !processor.PluginDescriptor.Installed ||
                !_pluginFinder.AuthenticateStore(processor.PluginDescriptor, _storeContext.CurrentStore.Id))
                throw new NopException("QQ模块没有被装载");

            var viewModel = new LoginModel();
            TryUpdateModel(viewModel);
            var context = new QzoneContext(string.Empty, new QQConnectConfig("204134", "4c46ac3122a0ccb5ebf14b2a77de3516", "http://www.win8charm.com/account/QQCallback.aspx", "https://graph.qq.com/oauth2.0/authorize"));
            string state = Guid.NewGuid().ToString().Replace("-", "");
            string scope = "get_user_info,add_share,list_album,upload_pic,check_page_fans,add_t,add_pic_t,del_t,get_repost_list,get_info,get_other_info,get_fanslist,get_idolist,add_idol,del_idol,add_one_blog,add_topic,get_tenpay_addr";
            var authenticationUrl = context.GetAuthorizationUrl(state, scope);
            //request token, request token secret 需要保存起来
            //在demo演示中，直接保存在全局变量中.真实情况需要网站自己处理
            Session["requeststate"] = state;
            //Response.Redirect(authenticationUrl);
            return new RedirectResult(authenticationUrl);
            //var result = _oAuthProviderFacebookAuthorizer.Authorize(returnUrl, verifyResponse);
            //switch (result.AuthenticationStatus)
            //{
            //    case OpenAuthenticationStatus.Error:
            //        {
            //            if (!result.Success)
            //                foreach (var error in result.Errors)
            //                    ExternalAuthorizerHelper.AddErrorsToDisplay(error);

            //            return new RedirectResult(Url.LogOn(returnUrl));
            //        }
            //    case OpenAuthenticationStatus.AssociateOnLogon:
            //        {
            //            return new RedirectResult(Url.LogOn(returnUrl));
            //        }
            //    case OpenAuthenticationStatus.AutoRegisteredEmailValidation:
            //        {
            //            //result
            //            return RedirectToRoute("RegisterResult", new { resultId = (int)UserRegistrationType.EmailValidation });
            //        }
            //    case OpenAuthenticationStatus.AutoRegisteredAdminApproval:
            //        {
            //            return RedirectToRoute("RegisterResult", new { resultId = (int)UserRegistrationType.AdminApproval });
            //        }
            //    case OpenAuthenticationStatus.AutoRegisteredStandard:
            //        {
            //            return RedirectToRoute("RegisterResult", new { resultId = (int)UserRegistrationType.Standard });
            //        }
            //    default:
            //        break;
            //}

            //if (result.Result != null) return result.Result;
            //return HttpContext.Request.IsAuthenticated ? new RedirectResult(!string.IsNullOrEmpty(returnUrl) ? returnUrl : "~/") : new RedirectResult(Url.LogOn(returnUrl));
        }

        public ActionResult Login(string returnUrl)
        {
            return LoginInternal(returnUrl, false);
        }

        public ActionResult LoginCallback(string returnUrl)
        {
            return LoginInternal(returnUrl, true);
        }
    }
}
