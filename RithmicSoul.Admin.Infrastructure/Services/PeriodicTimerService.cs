using System.Timers;
using Timer = System.Timers.Timer;

namespace RithmicSoul.Admin.Infrastructure.Services;

public class JobExecutedEventArgs : EventArgs { }


public class PeriodicTimerService : IDisposable
{
    public event EventHandler<JobExecutedEventArgs> JobExecuted;
    private Timer _Timer;
    bool _Running;
    
    void OnJobExecuted()
    {
        JobExecuted?.Invoke(this, new JobExecutedEventArgs());
    }
    

    public async Task StartExecutingAsync()
    {
        if (_Running) return;
        _Timer = new Timer();
        _Timer.Interval = 1000;  
        _Timer.Elapsed += HandleTimer;
        _Timer.AutoReset = true;
        _Timer.Enabled = true;
        _Running = true;
    }
    void HandleTimer(object source, ElapsedEventArgs e)
    {
        OnJobExecuted();
    }

    public void Dispose()
    {
        if (_Running)
        {
            _Timer = null;
        }
    }
}