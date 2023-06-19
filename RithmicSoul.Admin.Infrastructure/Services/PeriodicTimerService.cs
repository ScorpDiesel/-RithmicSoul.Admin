using System.Timers;
using Timer = System.Timers.Timer;

namespace RithmicSoul.Admin.Infrastructure.Services;

public class JobExecutedEventArgs : EventArgs { }


public class PeriodicTimerService : IDisposable
{
    public event EventHandler<JobExecutedEventArgs> JobExecuted;
    private Timer _timer;
    bool _running;
    
    void OnJobExecuted()
    {
        JobExecuted?.Invoke(this, new JobExecutedEventArgs());
    }
    

    public async Task StartExecutingAsync()
    {
        if (_running) return;
        _timer = new Timer();
        _timer.Interval = 1000;  
        _timer.Elapsed += HandleTimer;
        _timer.AutoReset = true;
        _timer.Enabled = true;
        _running = true;
    }
    void HandleTimer(object source, ElapsedEventArgs e)
    {
        OnJobExecuted();
    }

    public void Dispose()
    {
        if (_running)
        {
            _timer = null;
        }
    }
}