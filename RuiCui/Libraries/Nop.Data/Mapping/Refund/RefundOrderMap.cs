
using Nop.Core.Domain.Refund;
using System.Data.Entity.ModelConfiguration;


namespace Nop.Data.Mapping.Refund
{
    public partial class RefundOrderMap : EntityTypeConfiguration<RefundOrder>
    {
      public RefundOrderMap()
      {
          this.ToTable("RefundOrder");
          this.HasKey(on => on.Id);
          this.Property(on => on.OrderId).IsRequired();
          this.Property(on => on.OperatorId).IsRequired();
          this.Property(on => on.OperatorEmail);
          this.Property(on => on.RefundAmount).HasPrecision(18, 4);
          this.Property(on => on.ReasonForRefund);
      }
    }
}
