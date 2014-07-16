using System.Data.Entity.ModelConfiguration;
using Nop.Core.Domain.Discounts;

namespace Nop.Data.Mapping.Discounts
{
    public partial class DiscountPackageProductsMap : EntityTypeConfiguration<DiscountPackageProducts>
    {
        public DiscountPackageProductsMap()
        {
            this.ToTable("DiscountPackageProducts");
            this.HasKey(c => c.Id);
        }
    }
}
