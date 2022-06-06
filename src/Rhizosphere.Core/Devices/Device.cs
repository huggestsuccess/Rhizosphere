namespace Rhizosphere.Core;

public abstract class Device
{

    public bool IsRunning { get; protected set; }

    public DateTime LatestStateChange { get; protected set; } = DateTime.Now;

    public Uptime LatestUptime { get; protected set; } =  new();

    public Uptime Uptime
    {
        get=>  new Uptime(LatestUptime, LatestStateChange, DateTime.Now, IsRunning);
    }
    

    public abstract DeviceOptions DeviceOptions { get; }
}

public class Device<T> : Device, IDisposable
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

    public override DeviceOptions DeviceOptions
    {
        get => _optionsDelegate.CurrentValue;
    }

    public TimeSpan Run()
        => ChangeState(true);
    public TimeSpan Stop()
        => ChangeState(false);

    private PinValue GetPinValue(bool state, bool normallyOpen)
    {
        if (state)
            return normallyOpen ? PinValue.Low : PinValue.High;
        else
            return normallyOpen ? PinValue.High : PinValue.Low;
    }

    public TimeSpan ChangeState(bool state)
    {
        if (IsRunning == state)
            return default;

        if(!state)
            LatestUptime = Uptime;

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