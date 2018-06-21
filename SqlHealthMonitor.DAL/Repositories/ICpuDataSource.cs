using SqlHealthMonitor.DAL.Models;
using System.Collections.Generic;

namespace SqlHealthMonitor.DAL.Repositories
{
    public interface ICpuDataSource
    {
        /// <summary>
        /// Get data from Sql server,related to Cpu processor load of Sql server .
        /// </summary>
        /// <param name="numberOfRecords">number of records back to history ,scale is the one minute</param>
        /// <returns></returns>
        IList<CpuLoad> Get(int numberOfRecords);
    }
}