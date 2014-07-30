using Nop.Core;
using Nop.Core.Domain.Refund;
using System;
using System.Collections.Generic;
namespace Nop.Services.Refund
{
   public partial interface IRefundOrderService
    {
       void InsertRefundOrder(RefundOrder refundOrder);
       void UpdateRefundOrder(RefundOrder refundOrder);

       /// <summary>
       /// Deletes a RefundOrder 
       /// </summary>
       /// <param name="refundOrder">refundOrder</param>
       void DeleteRefundOrder(RefundOrder refundOrder);

       /// <summary>
       /// Gets all refundOrder 
       /// </summary>
       /// <returns>refundOrder collection</returns>
       IList<RefundOrder> GetAllRefundOrders();

       /// <summary>
       /// Gets a refundOrder 
       /// </summary>
       /// <param name="refundOrderId">refundOrder  identifier</param>
       /// <returns>RefundOrder </returns>
       RefundOrder GetRefundOrderById(int refundOrderId);

       RefundOrder GetRefundOrderByOrderId(int OrderId);

       /// <summary>
       /// Gets all RefundOrder
       /// </summary>
       /// <param name="createTime">createTime</param>
       /// <param name="orderId">orderId</param>
       /// <param name="pageIndex">Page index</param>
       /// <param name="pageSize">Page size</param>
       /// <returns>RefundOrder collection</returns>
       IPagedList<RefundOrder> GetAllRefundOrders(DateTime? createTime,  int? orderId, int pageIndex, int pageSize);
    }
}
