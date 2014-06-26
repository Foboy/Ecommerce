using System.Collections.Generic;
using Nop.Core.Domain.Localization;
using Nop.Core.Domain.Stores;
using Nop.Core.Domain.Security;
using Nop.Core.Domain.Seo;


namespace Nop.Core.Domain.Catalog
{
    public partial class CategorySpecificationAtrribute: BaseEntity, ILocalizedEntity
    {
        public int SpecificationAttributeId { get; set; }

        public int CategoryId { get; set; }

        public bool AllowFiltering { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the entity has been deleted
        /// </summary>
        public bool Deleted { get; set; }
    }
}
