using Nop.Core.Domain.Refund;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nop.Services.Refund
{
   public partial interface IRefundOrderItemService
    {
       void InsertRefundOrderItem(RefundOrderItem refundOrderItem);

       void UpdateRefundOrderItem(RefundOrderItem refundOrderItem);

       void DeleteRefundOrderItem(RefundOrderItem refundOrderItem);

       IList<RefundOrderItem> GetAllRefundOrderItems(int orderId);

       RefundOrderItem GetRefundOrderItemById(int refundOrderItemId);
    }
}
