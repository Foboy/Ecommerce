using System.Data.Entity.ModelConfiguration;
using Nop.Core.Domain.Catalog;

namespace Nop.Data.Mapping.Catalog
{
    public partial class ProductQuestionMap : EntityTypeConfiguration<ProductQuestion>
    {
        public ProductQuestionMap()
        {
            this.ToTable("ProductQuestion");
            this.HasKey(pr => pr.Id);

        }
    }
}