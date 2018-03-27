using System;
using System.Linq;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using SqlHealthMonitor.BLL.Models;
using SqlHealthMonitor.DAL.Models;
using SqlHealthMonitor.DAL.Repositories;

namespace SqlHealthMonitor.BLL.Services
{
   public class LogService: ServiceBase , ILogService
    {
        private MapperConfiguration _config;


        private ILogRepository _logRep;

        public LogService(ILogRepository logRep)
        {
           
                   _logRep = logRep;


                _config = new MapperConfiguration(cfg =>
                {
                    cfg.CreateMap<Log, LogViewModel>();
                    cfg.CreateMissingTypeMaps = true;
                });
            }

        protected override Type LogPrefix => GetType();

        public IQueryable<LogViewModel> GetLogs()
        {
            var tabLogs = _logRep.GetQueryable();
              return tabLogs.ProjectTo<LogViewModel>(_config);
        }
    }
}
