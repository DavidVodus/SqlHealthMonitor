using System.Collections.Generic;

namespace SqlHealthMonitor.BLL.Services
{
    public interface IDatabasesSizeService
    {
        /// Get data from Sql server,related to Size of databases in it,and return JSON 
        /// [{"DatabaseId":0,"DatabaseName":"something","Type":"Data","Size":"900"}]
        List<DatabasesSizeService.DatabasesSize> GetDatabasesSize(int SqlServerDataId, string currentUserId, List<int> DatabaseIds,string jtSorting);
    }
}