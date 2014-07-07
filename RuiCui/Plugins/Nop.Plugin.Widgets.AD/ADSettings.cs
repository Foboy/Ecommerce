
using Nop.Core.Configuration;

namespace Nop.Plugin.Widgets.AD
{
    public class ADSettings : ISettings
    {
        public int Picture1Id { get; set; }
        public string Text1 { get; set; }
        public string Link1 { get; set; }

        public int Picture2Id { get; set; }
        public string Text2 { get; set; }
        public string Link2 { get; set; }


    }
}