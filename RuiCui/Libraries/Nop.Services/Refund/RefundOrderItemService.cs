using Nop.Core.Data;
using Nop.Core.Domain.Refund;
using Nop.Services.Events;
using System;
using System.Collections.Generic;
using System.Linq;
namespace Nop.Services.Refund
{
   public class RefundOrderItemService:IRefundOrderItemService
    {

        private readonly IRepository<RefundOrderItem> _refundOrderItemRepository;
        private readonly IEventPublisher _eventPublisher;
        public RefundOrderItemService(IRepository<RefundOrderItem> refundOrderItemRepository, IEventPublisher eventPublisher)
        {
            this._refundOrderItemRepository = refundOrderItemRepository;
            this._eventPublisher = eventPublisher;
        }
        public virtual void InsertRefundOrderItem(RefundOrderItem refundOrderItem)
        {
            if (refundOrderItem == null)
                throw new ArgumentNullException("refundOrderItem");

            _refundOrderItemRepository.Insert(refundOrderItem);
            _eventPublisher.EntityInserted(refundOrderItem);
        }

        public virtual void UpdateRefundOrderItem(RefundOrderItem refundOrderItem)
        {
            if (refundOrderItem == null)
                throw new ArgumentNullException("refundOrderItem");

            _refundOrderItemRepository.Update(refundOrderItem);

            //event notification
            _eventPublisher.EntityUpdated(refundOrderItem);
        }

        public virtual void DeleteRefundOrderItem(RefundOrderItem refundOrderItem)
        {
            if (refundOrderItem == null)
                throw new ArgumentNullException("orderItem");

            _refundOrderItemRepository.Delete(refundOrderItem);

            //event notification
            _eventPublisher.EntityDeleted(refundOrderItem);
        }

        public virtual IList<RefundOrderItem> GetAllRefundOrderItems(int orderId)
        {
            return _refundOrderItemRepository.Table.Where(o => o.OrderId == orderId).ToList();
        }

        public virtual RefundOrderItem GetRefundOrderItemById(int refundOrderItemId)
        {
            throw new NotImplementedException();
        }
    }
}
