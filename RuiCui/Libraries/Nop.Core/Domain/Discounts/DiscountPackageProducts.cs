using System;
using System.Collections.Generic;
using Nop.Core.Domain.Catalog;
using Nop.Core.Domain.Orders;

namespace Nop.Core.Domain.Discounts
{
    public partial class DiscountPackageProducts : BaseEntity
    {
        public int DiscountId { get; set; }

        public int ShoppingCartItemId { get; set; }

        public int DisplayOrder { get; set; }
    }
}
