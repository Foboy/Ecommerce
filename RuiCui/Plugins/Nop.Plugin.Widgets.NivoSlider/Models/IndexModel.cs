using Nop.Web.Framework.Mvc;

namespace Nop.Plugin.Widgets.NivoSlider.Models
{
    public class IndexModel : BaseNopModel
    {
        public ConfigurationModel _ConfigurationModel { get; set; }
        public ConfigurationIndexRightModel _ConfigurationIndexRightModel { get; set; }
        public ConfigurationIndexSliderRootModel _ConfigurationIndexSliderRootModel { get; set; }
        public ConfigurationHotModel _ConfigurationHotModel { get; set; }
        public ConfigurationNewModel _ConfigurationNewModel { get; set; }
        public ConfigurationLikeModel _ConfigurationLikeModel { get; set; }
        public ConfigurationStarModel _ConfigurationStarModel { get; set; }
    }
}