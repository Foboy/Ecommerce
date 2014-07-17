using System.Web.Mvc;
using Nop.Web.Framework;
using Nop.Web.Framework.Mvc;
using System.Collections.Generic;

namespace Nop.Admin.Models.Catalog
{
    public partial class CategorySpecificationAtrributeModel : BaseNopModel
    {
        public CategorySpecificationAtrributeModel()
        {
            Options = new List<SelectListItem>();
        }
        public string Name { get; set; }
        public IList<SelectListItem> Options { get; set; }
    }
}