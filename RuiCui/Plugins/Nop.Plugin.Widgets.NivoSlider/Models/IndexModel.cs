using Nop.Web.Framework.Mvc;

namespace Nop.Plugin.Widgets.NivoSlider.Models
{
    public class IndexModel : BaseNopModel
    {
        public int IndexId { get; set; }
        public ConfigurationModel _ConfigurationModel { get; set; }
        public ConfigurationIndexRightModel _ConfigurationIndexRightModel { get; set; }
        public ConfigurationIndexSliderRootModel _ConfigurationIndexSliderRootModel { get; set; }
    }
}