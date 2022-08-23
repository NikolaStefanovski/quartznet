using AutoMapper;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Quartz.Examples.AspNetCore.Database;
using Quartz.Examples.AspNetCore.Database.Models;
using Quartz.Examples.AspNetCore.CRC_API;

namespace Quartz.Examples.AspNetCore
{
    [DisallowConcurrentExecution]
    public class CRCJob : IJob, IDisposable
    {
        private readonly ILogger Logger;
        private readonly CRCAPI API;
        private readonly CRCTestContext DBContext;
        private readonly IMapper Mapper;

        public CRCJob(ILogger<CRCJob> logger, CRCAPI api, CRCTestContext dbContext, IMapper mapper)
        {
            this.Logger = logger;
            this.API = api;
            this.DBContext = dbContext;
            this.Mapper = mapper;
        }

        public async Task Execute(IJobExecutionContext context)
        {
            Logger.LogInformation(context.JobDetail.Key + " job executing, triggered by " + context.Trigger.Key);

            // get employees
            var employeesResult = API.GetEmployees();

            if (employeesResult != null && employeesResult.Value != null)
            {
                DBContext.EmployeeExtraFields.RemoveRange(DBContext.EmployeeExtraFields);
                DBContext.Employees.RemoveRange(DBContext.Employees);
                DBContext.SaveChanges();

                foreach (var employee in employeesResult.Value)
                {
                    DBContext.Employees.Add(Mapper.Map<Employee>(employee));
                }

                DBContext.SaveChanges();
            }

            // get employees
            var employeesPresenceResult = API.GetEmployeePresences();

            if (employeesPresenceResult != null && employeesPresenceResult.Value != null)
            {
                DBContext.EmployeePresences.RemoveRange(DBContext.EmployeePresences);
                DBContext.SaveChanges();

                foreach (var employeePresence in employeesPresenceResult.Value)
                {
                    DBContext.EmployeePresences.Add(Mapper.Map<EmployeePresence>(employeePresence));
                }

                DBContext.SaveChanges();
            }

            Logger.LogInformation("CRC job executed");
            await Task.Delay(TimeSpan.FromSeconds(10));
        }

        public void Dispose()
        {
            Logger.LogInformation("CRC job disposing");
        }
    }
}
