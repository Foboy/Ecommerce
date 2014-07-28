using System;


namespace Nop.Core.Domain.Refund
{
   public partial class RefundOrderItem:BaseEntity
    {
       public int OrderId { get; set; }
       public int OrderItemId { get; set; }
    }
}
