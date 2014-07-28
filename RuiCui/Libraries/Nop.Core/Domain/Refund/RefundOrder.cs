
using System;
namespace Nop.Core.Domain.Refund
{
   public partial class RefundOrder: BaseEntity
    {
        public int OrderId { get; set; }

        public decimal RefundAmount { get; set; }

        public int OperatorId { get; set; }

        public string OperatorEmail { get; set; }

        public DateTime? OperateTime { get; set; }

        public string ReasonForRefund { get; set; }
    }
}
