

using System.Collections.Generic;

namespace SqlHealthMonitor.BLL.Services
{
    public interface ICpuService
    {
        List< CpuService.CpuLoad> GetCpuUsage();
    }
}