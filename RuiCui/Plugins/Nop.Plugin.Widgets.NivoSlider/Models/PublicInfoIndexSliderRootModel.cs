using Nop.Web.Framework.Mvc;

namespace Nop.Plugin.Widgets.NivoSlider.Models
{
    public class PublicInfoIndexSliderRootModel : BaseNopModel
    {
        public string Picture1Url { get; set; }
        public string Text1 { get; set; }
        public string Link1 { get; set; }

        public string Picture2Url { get; set; }
        public string Text2 { get; set; }
        public string Link2 { get; set; }

        public string Picture3Url { get; set; }
        public string Text3 { get; set; }
        public string Link3 { get; set; }

    }
}