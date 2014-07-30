using Nop.Core.Domain.Refund;
using System.Data.Entity.ModelConfiguration;

namespace Nop.Data.Mapping.Refund
{
    public partial class RefundOrderItemMap : EntityTypeConfiguration<RefundOrderItem>
    {
        public RefundOrderItemMap()
        {
            this.ToTable("RefundOrderItem");
            this.HasKey(on => on.Id);
            this.Property(on => on.OrderId).IsRequired();
            this.Property(on => on.OrderItemId).IsRequired();
        }
    }
}
