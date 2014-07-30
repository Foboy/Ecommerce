using FluentValidation.Attributes;
using Nop.Admin.Validators.Orders;
using Nop.Core.Domain.Refund;
using Nop.Web.Framework.Mvc;
using System;
using System.Collections.Generic;
using System.Web.Mvc;
namespace Nop.Admin.Models.Orders
{
    [Validator(typeof(RefundOrderValidator))]
    public partial class RefundOrderModel : BaseNopEntityModel
    {
        public RefundOrderModel()
        {
            SelectList = new List<SelectListItem>();
        }
        public override int Id { get; set; }

        public string ReasonForRefund { get; set; }

        public int OrderId { get; set; }

        public string ChoseOrderItemIds { get; set; }

        public IList<SelectListItem> SelectList { get; set; }


        public DateTime CreateTime { get; set; }

        public int RefundOrderId { get; set; }

        public decimal RefundAmount { get; set; }

        public string OperatorEmail { get; set; }

        public int OperatorId { get; set; }
    }
}