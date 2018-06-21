using SqlHealthMonitor.BLL.Models;
using SqlHealthMonitor.DAL.Models;
using System.Collections.Generic;
using static SqlHealthMonitor.BLL.Services.JobsService;

namespace SqlHealthMonitor.BLL.Services
{
    public interface IJobsService
    {
        /// <summary>
        /// Get data from Sql server,related to Sql Jobs in it.
        /// it uses JobsDataSource and SqlServerRepository
        /// </summary>
        /// <param name="sqlServerDataId">id of sql record where connectionString is stored</param>
        /// <param name="currentUserId">logged user id</param>
        /// <param name="jtSorting">if jtSorting isnt null,add Order by clause to query string</param>
        /// <returns></returns>
        List<SqlJobsViewModel> Get(int sqlServerDataId, string currentUserId,string jtSorting);
    }
}