using System;
using Nop.Core.Configuration;

namespace Nop.Core.Domain.Common
{
    public class VIPConsumerValveSettings : ISettings
    {
        public int VIPConditionValue { get; set; }
    }
}
