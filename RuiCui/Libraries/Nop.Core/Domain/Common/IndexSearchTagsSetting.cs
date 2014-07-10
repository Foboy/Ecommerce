
using Nop.Core.Configuration;

namespace Nop.Core.Domain.Common
{
    public class IndexSearchTagsSetting : ISettings
    {
        public string FirstTag { get; set; }
        public string SecondTag { get; set; }
        public string ThirdTag { get; set; }

        
    }
}
