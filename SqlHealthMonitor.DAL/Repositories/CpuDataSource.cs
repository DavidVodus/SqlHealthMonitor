using SqlHealthMonitor.DAL.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SqlHealthMonitor.DAL.Repositories
{
    public class CpuDataSource : ICpuDataSource
    {
        private static string _sqlGetCpuStatusQuery = @"
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
        private DbContext _dbContext=null;
        public CpuDataSource(string connectionString)
        {
             _dbContext = new DbContext(connectionString);

        }
        public  IList<CpuLoad> Get(int numberOfRecords)
        {
            return _dbContext.Database.SqlQuery<CpuLoad>(_sqlGetCpuStatusQuery, new SqlParameter("NumberOfRecords", numberOfRecords)).ToList();
        }
      
    }
}
