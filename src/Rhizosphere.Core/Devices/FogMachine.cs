namespace Rhizosphere.Core;

public class FogMachineOptions : DeviceOptions
{
    
}

public class FogMachine : Device<FogMachineOptions>
{
    public FogMachine(ILogger<Device<FogMachineOptions>> logger, IOptionsMonitor<FogMachineOptions> optionsDelegate)
     : base(logger, optionsDelegate)
    {
    }
}