using SqlHealthMonitor.BLL.Models;
using System.Collections.Generic;

namespace SqlHealthMonitor.BLL.Services
{
    public interface IDatabasesSizeService
    {
        /// <summary>
        /// 
        /// Get data from Sql server,related to Size of databases in it
        /// it uses DatabaseSizeuDataSource and SqlServerRepository
        /// </summary>
        /// <param name="sqlServerDataId">id of sql record where connectionString is stored</param>
        /// <param name="currentUserId">logged user id</param>
        /// <param name="databaseIds">list of databaseIds that goes to the Where clausule of Sql query, that select
        /// just only these Id from database</param>
        /// <param name="jtSorting">if jtSorting isnt null,add Order by clause to query string</param>
        /// <returns></returns>
        List<SqlDatabasesSizeViewModel> Get(int sqlServerDataId, string currentUserId, List<int> databaseIds,string jtSorting);
    }
}