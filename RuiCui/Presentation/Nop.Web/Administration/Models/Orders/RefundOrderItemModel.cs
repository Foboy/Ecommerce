using FluentValidation.Attributes;
using Nop.Web.Framework.Mvc;
namespace Nop.Admin.Models.Orders
{

    public partial class RefundOrderItemModel : BaseNopModel
    {
        public int OrderId { get; set; }

        public int OrderItemId { get; set; }


        public string ProductName { get; set; }
    }
}