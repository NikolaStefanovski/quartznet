using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Quartz.Examples.AspNetCore.Database;
using Quartz.Examples.AspNetCore.CRC_API;
using Quartz.Examples.AspNetCore.Mappers;
using Quartz.Impl;
using Quartz.Spi;
using SimpleInjector;
using SimpleInjector.Lifestyles;
using System.Data.Common;
//using System.ComponentModel;
using System.Reflection;

namespace Quartz.Examples.AspNetCore
{
    public class CRCJobFactory : IJobFactory
    {
        private readonly IServiceProvider serviceProvider;
        private readonly ILogger logger;

        public CRCJobFactory(IServiceProvider serviceProvider, ILogger<CRCJobFactory> logger)
        {
            this.serviceProvider = serviceProvider;
            this.logger = logger;
        }

        public IJob NewJob(TriggerFiredBundle bundle, IScheduler scheduler)
        {
            try
            {
                IJobDetail jobDetail = bundle.JobDetail;
                Type jobType = jobDetail.JobType;

                logger.LogDebug($"Producing instance of Job '{jobDetail.Key}', class={jobType.FullName}");

                var crcJobLogger = serviceProvider.GetRequiredService<ILogger<CRCJob>>();
                var crcAPI = serviceProvider.GetRequiredService<CRCAPI>();
                var dbContext = serviceProvider.CreateScope().ServiceProvider.GetService<CRCTestContext>();
                var mapper = serviceProvider.GetRequiredService<IMapper>();

#pragma warning disable CS8603 // Possible null reference return.
#pragma warning disable CS8604 // Possible null reference return.
                var job = new CRCJob(crcJobLogger, crcAPI, dbContext, mapper);
                return job;
#pragma warning disable CS8604 // Possible null reference return.
#pragma warning restore CS8603 // Possible null reference return.
            }
            catch (Exception ex)
            {
                logger.LogError(ex, ex.Message);
                throw new SchedulerException($"Problem instantiating class '{bundle.JobDetail.JobType.FullName}'", ex);
            }
        }

        public void ReturnJob(IJob job)
        {
            var disposable = job as IDisposable;
            disposable?.Dispose();
        }

        public static async Task RegisterQuartz(Container container, ILogger<CRCJobFactory> logger)
        {
            ISchedulerFactory schedulerFactory = new StdSchedulerFactory();
            IScheduler scheduler = await schedulerFactory.GetScheduler();
            IJobFactory jobFactory = new CRCJobFactory(container, logger);
            scheduler.JobFactory = jobFactory;

            container.RegisterInstance(schedulerFactory);
            container.RegisterInstance(jobFactory);
            container.RegisterInstance(scheduler);
            container.Register<CRCJob>();
        }
    }
}
