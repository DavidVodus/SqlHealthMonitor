using SqlHealthMonitor.DAL.Models;
using System.Collections.Generic;

namespace SqlHealthMonitor.DAL.Repositories
{
    public interface IJobsDataSource
    {
        /// <summary>
        ///  Get data from Sql server,related to the Jobs in it.
        /// </summary>
        /// <param name="jtSorting">if jtSorting isnt null,add Order by clause to query string</param>
        /// <returns></returns>
        List<SqlJobs> Get(string jtSorting);
    }
}