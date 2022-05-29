

using System;

namespace Rhizosphere.Core;

public class FogMachine : IDisposable
{
    private readonly ILogger<FogMachine> _log;
    private readonly GpioController _controller;

    public FogMachine(ILogger<FogMachine> logger)
    {
        _log = logger;
        _controller = new GpioController(PinNumberingScheme.Board);
        _controller.OpenPin(32, PinMode.Output, PinValue.Low);
    }

    public bool IsRunning { get; private set; }

    public DateTime LatestStateChange { get; private set; } = DateTime.Now;

    public TimeSpan Run()
        => ChangeState(true);
    public TimeSpan Stop()
        => ChangeState(false);
        
    public TimeSpan ChangeState(bool state)
    {  
        if(IsRunning == state)
            return default;

        _controller.Write(32, state ? PinValue.High : PinValue.Low);

        IsRunning = state;
        var t0 = DateTime.Now;
        var ts = t0 - LatestStateChange;
        LatestStateChange = t0; 

        return ts;
    }

    public void Dispose()
    {
        _controller.Dispose();
    }
}