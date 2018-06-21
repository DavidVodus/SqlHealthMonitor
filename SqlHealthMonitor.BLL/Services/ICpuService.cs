

using SqlHealthMonitor.BLL.Models;
using System.Collections.Generic;

namespace SqlHealthMonitor.BLL.Services
{
    public interface ICpuService
    {
        /// <summary>
        ///Get data from Sql server,related to cpu load of Sql server.
        /// it uses CpuDataSource and SqlServerRepository
        /// </summary>
        /// <param name="sqlServerDataId">id of sql record where connectionString is stored</param>
        /// <param name="currentUserId">logged user id</param>
        /// <param name="numberOfREcords">each records represents cpu load in one minutes span </param>
        /// <returns></returns>
        List<CpuLoadViewModel> Get(int sqlServerDataId,string currentUserId,int numberOfREcords);
    }
}