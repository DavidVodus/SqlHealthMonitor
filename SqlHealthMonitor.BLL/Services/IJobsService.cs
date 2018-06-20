using System.Collections.Generic;
using static SqlHealthMonitor.BLL.Services.JobsService;

namespace SqlHealthMonitor.BLL.Services
{
    public interface IJobsService
    {
         List<Jobs> GetJobs(int SqlServerDataId, string currentUserId);
    }
}