using SqlHealthMonitor.DAL.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SqlHealthMonitor.DAL.Repositories
{
   public class DatabaseSizeDataSource : IDatabaseSizeDataSource
    {
        private DbContext _dbContext;
    
        public DatabaseSizeDataSource(string connectionString)
        {
            _dbContext = new DbContext(connectionString);

        }
        private string GenerateSql(List<int> DatabaseIds)
        {
            var array = DatabaseIds.Select(x => x.ToString());
            var databaseIdString = string.Join(",", array);
            string databasesSizeQuery = "SELECT [DatabaseId] = Max(database_id)," +
           "[DatabaseName] = DB_NAME(database_id)," +
       "[Type] = CASE WHEN Type_Desc = 'ROWS' THEN 'Data'" +
                     "WHEN Type_Desc = 'LOG'  THEN 'Log'" +
                     "ELSE Type_Desc END," +
         "[Size] =  SUM(Size) " +
"FROM sys.master_files " +
(DatabaseIds.Count() <= 0 ? "" : "WHERE database_id IN(" + databaseIdString + ")") +
"GROUP BY      GROUPING SETS" +
             "(" +
                     "(DB_NAME(database_id), Type_Desc)," +
                     "(DB_NAME(database_id))" +
             ")";
            return databasesSizeQuery;
        }
        public List<SqlDatabasesSize> Get(List<int> databaseIds, string jtSorting)
        {
            if (!string.IsNullOrEmpty(jtSorting))
                jtSorting = jtSorting.Replace("SizeMB", "Size");
           return _dbContext.Database.SqlQuery<SqlDatabasesSize>(GenerateSql(databaseIds) + (string.IsNullOrEmpty(jtSorting) ? "" : "ORDER BY " + jtSorting)).ToList();
          

        }
    }
}
