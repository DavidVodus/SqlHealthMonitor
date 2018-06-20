using Castle.Windsor;
using SqlHealthMonitor.DAL.Managers;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace SqlHealthMonitor.BLL.Services
{
    public class CpuService : ServiceBase, ICpuService
    {
        public class CpuLoad
        {
            public int SqlServer { get; set; }
            public int Others { get; set; }
            public DateTime? EventTime { get ; set; }
            public string EventTimeText { get { return EventTime==null ? "NULL" : EventTime.Value.ToShortTimeString(); } set { } }
        }

        public static string SqlGetCpuStatus = @"
declare 
@ts_now bigint;
select @ts_now = ms_ticks from sys.dm_os_sys_info

select top (@NumberOfRecords)

    dateadd(ms, (y.[timestamp] -@ts_now), GETDATE()) as EventTime,
SQLProcessUtilization as SQLServer, 
--SystemIdle, 
100 - SystemIdle - SQLProcessUtilization as Others
from(
select
record.value('(./Record/@id)[1]', 'int') as record_id,  
record.value('(./Record/SchedulerMonitorEvent/SystemHealth/SystemIdle)[1]', 'int')  
as SystemIdle,  
record.value('(./Record/SchedulerMonitorEvent/SystemHealth/ProcessUtilization)[1]',  
'int') as SQLProcessUtilization,  
timestamp
from(
select timestamp, convert(xml, record) as record
from sys.dm_os_ring_buffers
where ring_buffer_type = N'RING_BUFFER_SCHEDULER_MONITOR'  
and record like '%<SystemHealth>%') as x  
) as y
order by record_id desc";
        private DbContext _dbContext;
        private ISqlServerDataService _sqlServerDataService;
        public CpuService(DbContext context,ISqlServerDataService sqlServerDataService) 
        {
        _sqlServerDataService = sqlServerDataService;;
        _dbContext = context;
        
        }
        public List<CpuLoad> GetCpuUsage(int SqlServerDataId,string currentUserId,int NumberOfRecords)
        {

          var sqlServer = _sqlServerDataService.Read
                (x => x.ApplicationUserId == currentUserId && x.SqlServerDataId == SqlServerDataId).SingleOrDefault();
               
           var dbContext=new DbContext(sqlServer.ConnectionString);
         
            var test = dbContext.Database.SqlQuery<CpuLoad>(SqlGetCpuStatus, new SqlParameter("NumberOfRecords", NumberOfRecords));
            return test.ToList<CpuLoad>();
        }
        protected override Type LogPrefix => GetType();
    }

}
