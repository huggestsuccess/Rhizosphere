

using System;
using Microsoft.Extensions.Options;

namespace Rhizosphere.Core;


public class FanOptions : DeviceOptions
{
    
}
public class Fan : Device<FanOptions>
{
    public Fan(ILogger<Device<FanOptions>> logger, IOptionsMonitor<FanOptions> optionsDelegate)
     : base(logger, optionsDelegate)
    {
    }
}