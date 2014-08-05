using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Net;
using System.ServiceModel.Syndication;
using System.Web.Mvc;
using System.Xml;
using Nop.Admin.Infrastructure.Cache;
using Nop.Admin.Models.Home;
using Nop.Core;
using Nop.Core.Caching;
using Nop.Core.Domain.Common;
using Nop.Services.Configuration;
using Nop.Web.Framework.Kendoui;
using Nop.Admin.Models.Orders;
using Nop.Services.Security;
using Nop.Services.Helpers;
using Nop.Core.Domain.Orders;
using Nop.Core.Domain.Payments;
using Nop.Services.Orders;
using Nop.Services.Catalog;
using Nop.Core.Domain.Shipping;
using Nop.Services.Localization;

namespace Nop.Admin.Controllers
{
    public class FinancialController : BaseAdminController
    {
        #region Fields
        private readonly IStoreContext _storeContext;
        private readonly CommonSettings _commonSettings;
        private readonly ISettingService _settingService;
        private readonly IWorkContext _workContext;
        private readonly ICacheManager _cacheManager;
        private readonly IPermissionService _permissionService;
        private readonly IDateTimeHelper _dateTimeHelper;
        private readonly IOrderReportService _orderReportService;
        private readonly IProductService _productService;
        private readonly IPriceFormatter _priceFormatter;
        private readonly IOrderService _orderService;
        private readonly ILocalizationService _localizationService;

        #endregion

        #region Ctor

        public FinancialController(IStoreContext storeContext,
              IOrderService orderService,
             IPermissionService permissionService,
               IDateTimeHelper dateTimeHelper,
                IOrderReportService orderReportService,
                 ILocalizationService localizationService,
              IProductService productService,
              IPriceFormatter priceFormatter,
            CommonSettings commonSettings,
            ISettingService settingService,
            IWorkContext workContext,
            ICacheManager cacheManager)
        {
            this._localizationService = localizationService;
            this._orderService = orderService;
            this._storeContext = storeContext;
            this._commonSettings = commonSettings;
            this._settingService = settingService;
            this._workContext = workContext;
            this._cacheManager = cacheManager;
            this._permissionService = permissionService;
            this._dateTimeHelper = dateTimeHelper;
            this._orderReportService = orderReportService;
            this._productService = productService;
            this._priceFormatter = priceFormatter;
        }

        #endregion

    
        public ActionResult OrderReport()
        {

            return View();
        }

        #region Methods
        

         class orderAddressReport
        {
            public string Name { get; set; }
            public DateTime CreatedOnUtc { get; set; }
            public decimal OrderTotal { get; set; }
        }
         class orderAddressRes
         {
             public string name { get; set; }
             public string value { get; set; }
         }
         [HttpPost]
         public ActionResult GetOrderReportListByAddress(DateTime? createdFromUtc = null, DateTime? createdToUtc = null)
        {
            var orderList = _orderService.SearchOrders();
            var query = from o in orderList
                        select new orderAddressReport
                        {
                            Name = o.ShippingAddress.Country.Name,
                            CreatedOnUtc = o.CreatedOnUtc,
                            OrderTotal = o.OrderTotal
                        };


            if (createdFromUtc.HasValue)
                query = query.Where(o => createdFromUtc.Value <= o.CreatedOnUtc);
            if (createdToUtc.HasValue)
                query = query.Where(o => createdToUtc.Value >= o.CreatedOnUtc);

            var q1 = from oq in query
                     group oq by oq.Name
                         into result
                         select new object[]
                          {
                              new{name = "云南"},
                                 new{ name = result.Key,
                                  value = result.Sum(o => o.OrderTotal)
                                 }
                             
                          };
            var q2 = from oq in query
                     group oq by oq.Name
                         into result
                         select new
                         {
                             name = result.Key,
                             value = result.Sum(o => o.OrderTotal)

                         };
            var max = q2.OrderByDescending(o => o.value).First();
            var res = new { q1 = q1, q2 = q2,max=max };

            return Json(res);
            
        }

        [HttpPost]
        public ActionResult GetOrderReportList(DataSourceRequest command, OrderReportModel model)
        {
            DateTime? startDateValue = (model.StartDate == null) ? null
                           : (DateTime?)_dateTimeHelper.ConvertToUtcTime(model.StartDate.Value, _dateTimeHelper.CurrentTimeZone);

            DateTime? endDateValue = (model.EndDate == null) ? null
                            : (DateTime?)_dateTimeHelper.ConvertToUtcTime(model.EndDate.Value, _dateTimeHelper.CurrentTimeZone).AddDays(1);

            
            OrderStatus? orderStatus = model.OrderStatusId > 0 ? (OrderStatus?)(model.OrderStatusId) : null;
            orderStatus = OrderStatus.Complete;
            //load orders
            var orders = _orderService.SearchOrders(0, 0, 0, 0, 0,
                startDateValue, endDateValue, orderStatus,
                null, null,null, null,
                command.Page - 1, command.PageSize);
            var gridModel = new DataSourceResult
            {
                Data = orders.Select(x =>
                {
                    return new OrderModel()
                    {
                        Id = x.Id,
                        OrderTotal = _priceFormatter.FormatPrice(x.OrderTotal, true, false),
                        OrderStatus = x.OrderStatus.GetLocalizedEnum(_localizationService, _workContext),
                        CustomerEmail = x.BillingAddress.Email,
                        CustomerFullName = string.Format("{0} {1}", x.BillingAddress.FirstName, x.BillingAddress.LastName),
                        CreatedOn = _dateTimeHelper.ConvertToUserTime(x.CreatedOnUtc, DateTimeKind.Utc)
                    };
                }),
                Total = orders.TotalCount,
            };

            //summary report
            var reportSummary = _orderReportService.GetOrderAverageReportLine(0,
                0, orderStatus, null, null,
                startDateValue, endDateValue,null);


            gridModel.ExtraData = new OrderAggreratorModel()
            {
                aggregatortotal = _priceFormatter.FormatPrice(reportSummary.SumOrders, true, false),
                avgOrderTotal = _priceFormatter.FormatPrice(reportSummary.SumOrders / gridModel.Total, true, false)
            };

            return Json(gridModel);
        }

        public ActionResult ShippingReport()
        {
            return View();
        }

        [HttpPost]
        public ActionResult GetOrderShipReportList(DataSourceRequest command, OrderReportModel model)
        {
            DateTime? startDateValue = (model.StartDate == null) ? null
                           : (DateTime?)_dateTimeHelper.ConvertToUtcTime(model.StartDate.Value, _dateTimeHelper.CurrentTimeZone);

            DateTime? endDateValue = (model.EndDate == null) ? null
                            : (DateTime?)_dateTimeHelper.ConvertToUtcTime(model.EndDate.Value, _dateTimeHelper.CurrentTimeZone).AddDays(1);


            OrderStatus? orderStatus = model.OrderStatusId > 0 ? (OrderStatus?)(model.OrderStatusId) : null;
            orderStatus = OrderStatus.Complete;
            //load orders
            var orders = _orderService.SearchOrders(0, 0, 0, 0, 0,
                startDateValue, endDateValue, orderStatus,
                null, null, null, null,
                command.Page - 1, command.PageSize);
            var gridModel = new DataSourceResult
            {
                Data = orders.Select(x =>
                {
                    return new OrderModel()
                    {
                        Id = x.Id,
                        OrderShippingExclTax = _priceFormatter.FormatPrice(x.OrderShippingExclTax, true, false),
                        OrderStatus = x.OrderStatus.GetLocalizedEnum(_localizationService, _workContext),
                        CustomerEmail = x.BillingAddress.Email,
                        CustomerFullName = string.Format("{0} {1}", x.BillingAddress.FirstName, x.BillingAddress.LastName),
                        CreatedOn = _dateTimeHelper.ConvertToUserTime(x.CreatedOnUtc, DateTimeKind.Utc)
                    };
                }),
                Total = orders.TotalCount,
            };
            //summary report
            var reportSummary = _orderReportService.GetOrderAverageReportLine(0,
                0, orderStatus, null, null,
                startDateValue, endDateValue, null);


            gridModel.ExtraData = new OrderAggreratorModel()
            {
                aggregatorshipping = _priceFormatter.FormatShippingPrice(reportSummary.SumShippingExclTax, true),
                avgShipping = _priceFormatter.FormatPrice(reportSummary.SumShippingExclTax / gridModel.Total, true, false)
            };

            return Json(gridModel);
        }
        #endregion
    }
}