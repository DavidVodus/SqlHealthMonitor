using System.Linq;
using SqlHealthMonitor.BLL.Models;

namespace SqlHealthMonitor.BLL.Services
{
  public  interface ILogService
    {
        IQueryable<LogViewModel> GetLogs();
    }
}
