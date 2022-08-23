using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Quartz.Examples.AspNetCore.CRC_API;
using Quartz.Examples.AspNetCore.CRC_API.Models;

namespace Quartz.Examples.AspNetCore
{
    public class CRCJob : IJob, IDisposable
    {
        private readonly ILogger<ExampleJob> Logger;
        private readonly CRCAPI API;
        private readonly DbContext DBContext;

        public CRCJob(ILogger<ExampleJob> logger, CRCAPI api, DbContext dbContext)
        {
            this.Logger = logger;
            this.API = api;
            this.DBContext = dbContext;
        }

        public async Task Execute(IJobExecutionContext context)
        {
            var result = API.GetEmployees();

            if (result != null && result.Value != null)
            {
                foreach (var employee in result.Value)
                {

                    DBContext.Add(employee);
                }

                DBContext.SaveChanges();
            }

            Logger.LogInformation("CRC job executed");
            await Task.Yield();
        }

        public void Dispose()
        {
            Logger.LogInformation("CRC job disposing");
        }
    }
}
