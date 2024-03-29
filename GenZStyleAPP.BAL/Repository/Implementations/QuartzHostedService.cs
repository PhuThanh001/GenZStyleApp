using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Hosting;
using Quartz;

namespace GenZStyleAPP.BAL.Repository.Implementations
{
    public class QuartzHostedService : IHostedService
    {
        private readonly IScheduler _scheduler;

        public QuartzHostedService(IScheduler scheduler)
        {
            _scheduler = scheduler;
        }

        public Task StartAsync(CancellationToken cancellationToken)
        {
            // Bạn cần thêm công việc CheckAndDeleteJob vào Scheduler ở đây
            Console.WriteLine("QuartzHostedService is starting...");
            // Thêm công việc CheckAndDeleteJob vào Scheduler
            var job = JobBuilder.Create<CheckAndDeleteJob>()
                .WithIdentity("CheckAndDeleteJob")
                .Build();

            var trigger = TriggerBuilder.Create()
                .WithIdentity("CheckAndDeleteTrigger")
                .StartNow()
                .WithSimpleSchedule(x => x.WithIntervalInMinutes(1).RepeatForever())
                .Build();

            _scheduler.ScheduleJob(job, trigger);

            // Khởi động Scheduler
            return _scheduler.Start(cancellationToken);
        }

        public async Task StopAsync(CancellationToken cancellationToken)
        {
            // Dừng Scheduler khi ứng dụng tắt
            await _scheduler.Shutdown(cancellationToken);
        }
    }



}
