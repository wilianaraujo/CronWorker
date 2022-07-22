using CronWorker.Configuration;
using CronWorker.Workers;
using Microsoft.Extensions.DependencyInjection;
using System.Diagnostics;

namespace CronWorker.Extensions
{
    public static class JobExtensions
    {
        public static IServiceCollection AddCronJob<T>(this IServiceCollection services, Action<IScheduleConfig<T>> options) where T : ServiceWorker
        {
            if (options == null)
            {
                Debug.WriteLine("Parâmetros de configuração não definidos para o job. O job não será ativado.");
                return services;
            }
            var config = new ScheduleConfig<T>();
            options.Invoke(config);

            services.AddSingleton<IScheduleConfig<T>>(config);
            services.AddHostedService<T>();

            return services;
        }
    }
}
