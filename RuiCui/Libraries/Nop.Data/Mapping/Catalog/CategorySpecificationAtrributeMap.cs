using System.Data.Entity.ModelConfiguration;
using Nop.Core.Domain.Catalog;

namespace Nop.Data.Mapping.Catalog
{
    public partial class CategorySpecificationAtrributeMap : EntityTypeConfiguration<CategorySpecificationAtrribute>
    {

        public CategorySpecificationAtrributeMap()
        {
            this.ToTable("CategorySpecificationAtrribute");
            this.HasKey(sao => sao.Id);
            this.Property(sao => sao.AllowFiltering).IsRequired();
            this.Property(sao => sao.CategoryId).IsRequired();
            this.Property(sao => sao.SpecificationAttributeId).IsRequired();
        }
    }
}
