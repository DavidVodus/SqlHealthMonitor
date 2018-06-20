using Castle.Windsor;
using SqlHealthMonitor.DAL.Managers;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace SqlHealthMonitor.BLL.Services
{
    public class DatabasesSizeService : ServiceBase, IDatabasesSizeService
    {
        public class DatabasesSize
        {
            public int DatabaseId { get; set; }
            public string DatabaseName { get; set; }
            public string Type { get; set; }
            public int Size { get; set; }
            public double SizeMB { get { return ((Size * 8) / 1024.0); } set { } }
        }

//        public static string databasesSizeQuery = @"

//SELECT 
//[Database Id]= Max(database_id),
//[Database Name] = DB_NAME(database_id),
//       [Type] = CASE WHEN Type_Desc = 'ROWS' THEN 'Data'
//                     WHEN Type_Desc = 'LOG'  THEN 'Log'
//                     ELSE Type_Desc END,
//      --[Size] =  CAST( ((SUM(Size)* 8) / 1024.0) AS DECIMAL(18,2) )
//	     [Size] =  SUM(Size)
//FROM   sys.master_files
//-- Uncomment if you need to query for a particular database
//WHERE    database_id not IN (select Id from @databasesList)
//GROUP BY      GROUPING SETS
//              (
//                     (DB_NAME(database_id), Type_Desc),
//                     (DB_NAME(database_id))
//              )
//ORDER BY      DB_NAME(database_id), Type_Desc DESC";
        private DbContext _dbContext;
        private ISqlServerDataService _sqlServerDataService;
        public DatabasesSizeService(DbContext context,ISqlServerDataService sqlServerDataService) 
        {
        _sqlServerDataService = sqlServerDataService;;
        _dbContext = context;
        
        }
        private string GenerateSql(List<int> DatabaseIds)
        {
            var array = DatabaseIds.Select(x => x.ToString());
           var databaseIdString= string.Join(",", array);
            string  databasesSizeQuery=  "SELECT [DatabaseId] = Max(database_id)," +
           "[DatabaseName] = DB_NAME(database_id),"+
       "[Type] = CASE WHEN Type_Desc = 'ROWS' THEN 'Data'"+
                     "WHEN Type_Desc = 'LOG'  THEN 'Log'"+
                     "ELSE Type_Desc END,"+
         "[Size] =  SUM(Size) "+
"FROM sys.master_files "+
(DatabaseIds.Count()<=0  ? "" : "WHERE database_id IN("+ databaseIdString + ")")+
"GROUP BY      GROUPING SETS"+
             "("+
                     "(DB_NAME(database_id), Type_Desc),"+
                     "(DB_NAME(database_id))"+
             ")";
            return databasesSizeQuery;
        }
        public List<DatabasesSize> GetDatabasesSize(int SqlServerDataId,string currentUserId, List<int> DatabaseIds,string jtSorting)
        {
            if (!string.IsNullOrEmpty(jtSorting))
                jtSorting= jtSorting.Replace("SizeMB","Size");
          var sqlServer = _sqlServerDataService.Read
                (x => x.ApplicationUserId == currentUserId && x.SqlServerDataId == SqlServerDataId).SingleOrDefault();
               
           var dbContext=new DbContext(sqlServer.ConnectionString);
            var test = dbContext.Database.SqlQuery<DatabasesSize>(GenerateSql(DatabaseIds)+ (string.IsNullOrEmpty(jtSorting) ? "": "ORDER BY " + jtSorting));
            return test.ToList<DatabasesSize>();
          
        }
        protected override Type LogPrefix => GetType();
    }

}
