using SqlHealthMonitor.DAL.Models;
using System.Collections.Generic;

namespace SqlHealthMonitor.DAL.Repositories
{
    public interface IDatabaseSizeDataSource
    {
        /// <summary>
        /// Get data from Sql server,related to Size of databases in it.
        /// </summary>
        /// <param name="databaseIds">list of databaseIds that goes to the Where clausule of Sql query, that select
        /// just only these Id from database</param>
        /// <param name="jtSorting">if jtSorting isnt null,add Order by clause to query string</param>
        /// <returns></returns>
        List<SqlDatabasesSize> Get(List<int> databaseIds, string jtSorting);
    }
}