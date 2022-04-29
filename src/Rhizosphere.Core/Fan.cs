

namespace Rhizosphere.Core;

public class Fan : IDisposable
{
    private readonly ILogger<Fan> _log;
    private readonly GpioController _controller;

    public Fan(ILogger<Fan> logger)
    {
        _log = logger;
        _controller = new GpioController();
        _controller.OpenPin(32, PinMode.Output, PinValue.High);
    }

    public bool IsRunning { get; private set; }

    public bool Run()
        => ChangeState(true);
    public bool Stop()
        => ChangeState(false);

    public bool ChangeState(bool state)
    {
        _controller.Write(32, state ? PinValue.Low : PinValue.High);

        IsRunning = state;

        return state;
    }

    public void Dispose()
    {
        _controller.Dispose();
    }
}