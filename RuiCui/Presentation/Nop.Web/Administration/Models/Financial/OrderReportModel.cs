using Nop.Web.Framework;
using Nop.Web.Framework.Mvc;
using System;
using System.ComponentModel.DataAnnotations;

namespace Nop.Admin.Models.Orders
{
    public partial class OrderReportModel : BaseNopModel
    {


        [NopResourceDisplayName("Admin.Orders.List.OrderStatus")]
        public int OrderStatusId { get; set; }

        [UIHint("DateNullable")]
        public DateTime? StartDate { get; set; }

        [UIHint("DateNullable")]
        public DateTime? EndDate { get; set; }
    }
}