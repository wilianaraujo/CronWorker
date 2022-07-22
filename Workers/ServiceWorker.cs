using Cronos;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CronWorker.Workers
{
    public abstract class ServiceWorker : IHostedService, IDisposable
    {
        IEnumerable<CronExpression> _cronExpressions;
        TimeZoneInfo _timeZone;

        public DateTimeOffset NextRun { private set; get; } 

        System.Timers.Timer _timer;

        public ServiceWorker(IEnumerable<string> cronExpressions, TimeZoneInfo timezone)
        {
            _cronExpressions = cronExpressions.Select(e => CronExpression.Parse(e, CronFormat.IncludeSeconds));
            _timeZone = timezone;
        }

        public void Dispose()
        {
            _timer?.Dispose();  
        }

        public virtual async Task StartAsync(CancellationToken cancellationToken)
        {
            await ScheduleJob(cancellationToken);
        }

        public virtual async Task StopAsync(CancellationToken cancellationToken)
        {
            _timer?.Stop();
            await Task.CompletedTask;
        }

        public virtual async Task ScheduleJob(CancellationToken cancellationToken)
        {
            var next = _cronExpressions.Select(e => e.GetNextOccurrence(DateTimeOffset.Now, _timeZone))
                                       .Where(e => e.HasValue)
                                       .Select(e => e.Value)
                                       .OrderBy(e => e)
                                       .FirstOrDefault();

            if (next != null)
            {
                NextRun = next;

                var delay = next - DateTimeOffset.Now;

                if(delay.TotalMilliseconds <= 0)
                {
                    await ScheduleJob(cancellationToken);
                }
                _timer = new System.Timers.Timer(delay.TotalMilliseconds);

                _timer.Elapsed += async (sender, args) =>
                {
                    _timer.Dispose();
                    _timer = null;

                    if (!cancellationToken.IsCancellationRequested)
                    {
                        await DoWork(cancellationToken);

                        await ScheduleJob(cancellationToken);
                    }
                };
                _timer.Start();
            }

            await Task.CompletedTask;  
        }

        public virtual async Task DoWork(CancellationToken cancellationToken)
        {
            await Task.Delay(5000, cancellationToken);
        }
    }
}
