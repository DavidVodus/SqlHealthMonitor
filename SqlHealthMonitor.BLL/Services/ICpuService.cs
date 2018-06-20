

using System.Collections.Generic;

namespace SqlHealthMonitor.BLL.Services
{
    public interface ICpuService
    {
        /// Get data from Sql server,related to cpu load,and return JSON 
        /// [{"SqlServer":0,"Others":1,"EventTime":"\/Date(1526887626807)\/","EventTimeText":"9:27"}]
        /// six records is returned from Sql and each record contains time record of CPU load from now up to 6 minutes to the past
        List< CpuService.CpuLoad> GetCpuUsage(int SqlServerDataId,string currentUserId,int NumberOfREcords);
    }
}