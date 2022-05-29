

using System;
using Microsoft.Extensions.Options;

namespace Rhizosphere.Core;

public class Device<T> : IDisposable
    where T : DeviceOptions
{
    private readonly ILogger<Device<T>> _log;
    private readonly IOptionsMonitor<T> _optionsDelegate;
    private readonly GpioController _controller;

    public Device(ILogger<Device<T>> logger, IOptionsMonitor<T> optionsDelegate)
    {
        _log = logger;
        _optionsDelegate = optionsDelegate;
        _controller = new GpioController(PinNumberingScheme.Board);
        _controller.OpenPin
        (
            _optionsDelegate.CurrentValue.PinNumber, 
            PinMode.Output, 
            _optionsDelegate.CurrentValue.NormallyOpen ? PinValue.High : PinValue.Low
        );
    }

    public bool IsRunning { get; private set; }

    public DateTime LatestStateChange { get; private set; } = DateTime.Now;

    public DeviceOptions DeviceOptions {get;}
     => _optionsDelegate.CurrentValue;

    public TimeSpan Run()
        => ChangeState(true);
    public TimeSpan Stop()
        => ChangeState(false);
        
    private PinValue GetPinValue(bool state, bool normallyOpen)
    {
        if(state)
            return normallyOpen ? PinValue.Low : PinValue.High;
        else
            return normallyOpen ? PinValue.High : PinValue.Low;
    }

    public TimeSpan ChangeState(bool state)
    {  
        if(IsRunning == state)
            return default;

        
        var pinValue = GetPinValue(state, _optionsDelegate.CurrentValue.NormallyOpen);


        _controller.Write
        (
            _optionsDelegate.CurrentValue.PinNumber, 
            pinValue
        );

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