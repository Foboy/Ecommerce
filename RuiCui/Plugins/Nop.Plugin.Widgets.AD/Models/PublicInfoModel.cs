using Nop.Web.Framework.Mvc;

namespace Nop.Plugin.Widgets.AD.Models
{
    public class PublicInfoModel : BaseNopModel
    {
        public string Picture1Url { get; set; }
        public string Text1 { get; set; }
        public string Link1 { get; set; }

        public string Picture2Url { get; set; }
        public string Text2 { get; set; }
        public string Link2 { get; set; }

    }
}