using Nop.Core.Domain.Orders;
using Nop.Services.Logging;
using Nop.Services.Tasks;
using System;

namespace Nop.Services.Orders
{
    /// <summary>
    /// Represents a task for sending CancleOrder
    /// </summary>
    public partial class CancleOrderTask : ITask
    {
        private readonly IOrderService _orderService;

        private readonly ILogger _logger;

        private readonly IOrderProcessingService _orderProcessingService;


        public CancleOrderTask(IOrderService orderService,
            ILogger logger,
            IOrderProcessingService orderProcessingService)
        {

            this._orderService = orderService;
            this._logger = logger;
            this._orderProcessingService = orderProcessingService;
        }
        public virtual void Execute()
        {
            try
            {
                var orderList = _orderService.SearchOrders();
                foreach (var item in orderList)
                {
                    if (item.OrderStatus == OrderStatus.Pending&&DateTime.UtcNow.AddDays(-7)>item.CreatedOnUtc)
                    {
                            var order = _orderService.GetOrderById(item.Id);
                            if (order != null)
                            {
                                _orderProcessingService.CancelOrder(order, true);
                            }
                    }
                }
            }
            catch (Exception exc)
            {
                _logger.Error(string.Format("Error cancleorder. {0}", exc.Message), exc);
            }
            finally
            {
                //queuedEmail.SentTries = queuedEmail.SentTries + 1;
                //_queuedEmailService.UpdateQueuedEmail(queuedEmail);
               
            }
        }
    }
}
