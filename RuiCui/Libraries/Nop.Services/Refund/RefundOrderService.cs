using Nop.Core;
using Nop.Core.Data;
using Nop.Core.Domain.Refund;
using Nop.Services.Events;
using System;
using System.Collections.Generic;
using System.Linq;


namespace Nop.Services.Refund
{
    public class RefundOrderService : IRefundOrderService
    {
        private readonly IRepository<RefundOrder> _refundOrderRepository;
        private readonly IEventPublisher _eventPublisher;
        public RefundOrderService(IRepository<RefundOrder> refundOrderRepository, IEventPublisher eventPublisher)
        {
            this._refundOrderRepository = refundOrderRepository;
            this._eventPublisher = eventPublisher;
        }

        public virtual void InsertRefundOrder(RefundOrder refundOrder)
        {
            if (refundOrder == null)
                throw new ArgumentNullException("refundOrder");

            _refundOrderRepository.Insert(refundOrder);
            _eventPublisher.EntityInserted(refundOrder);
        }

        public virtual void UpdateRefundOrder(RefundOrder refundOrder)
        {
            if (refundOrder == null)
                throw new ArgumentNullException("refundOrder");

            _refundOrderRepository.Update(refundOrder);

            //event notification
            _eventPublisher.EntityUpdated(refundOrder);
        }

        public virtual void DeleteRefundOrder(RefundOrder refundOrder)
        {
            throw new NotImplementedException();
        }

        public virtual IList<RefundOrder> GetAllRefundOrders()
        {
            return _refundOrderRepository.Table.ToList();
        }

        public virtual RefundOrder GetRefundOrderById(int refundOrderId)
        {
            if (refundOrderId == 0)
                return null;

            return _refundOrderRepository.GetById(refundOrderId);
        }


        public virtual IPagedList<RefundOrder> GetAllRefundOrders(DateTime? createTime, int? orderId, int pageIndex, int pageSize)
        {
            throw new NotImplementedException();
        }


        public virtual RefundOrder GetRefundOrderByOrderId(int OrderId)
        {
            if (OrderId == 0)
            {
                return null;
            }
            var query = from o in _refundOrderRepository.Table
                        where o.OrderId == OrderId
                        select o;
            var refundOrder = query.FirstOrDefault();
            return refundOrder;
        }
    }
}

