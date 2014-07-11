using Nop.Web.Framework.Mvc;
using System.Collections.Generic;

namespace Nop.Web.Models.Catalog
{
    public partial class SearchBoxModel : BaseNopModel
    {
        public bool AutoCompleteEnabled { get; set; }
        public bool ShowProductImagesInSearchAutoComplete { get; set; }
        public int SearchTermMinimumLength { get; set; }
        public List<string> Tags { get; set; }
    }
}