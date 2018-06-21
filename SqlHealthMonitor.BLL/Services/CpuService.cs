using AutoMapper;
using Castle.Windsor;
using Common;
using SqlHealthMonitor.BLL.Models;
using SqlHealthMonitor.DAL.Managers;
using SqlHealthMonitor.DAL.Models;
using SqlHealthMonitor.DAL.Repositories;
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
   
        private DbContext _dbContext;
        private ISqlServerDataRepository _sqlServerDataRepository;
        public CpuService(DbContext context, ISqlServerDataRepository sqlServerDataRepository) 
        {
            _sqlServerDataRepository = sqlServerDataRepository;
            _dbContext = context;
        
        }
        public List<CpuLoadViewModel> Get(int sqlServerDataId,string currentUserId,int numberOfRecords)
        {
            var _config = new MapperConfiguration(cfg =>
            {

                cfg.CreateMissingTypeMaps = true;
                cfg.AddGlobalIgnore("EventTimeText");
            });

            var mapper = new Mapper(_config);
          
            var sqlServer = _sqlServerDataRepository.GetQueryable().Where
              (x => x.ApplicationUserId == currentUserId && x.SqlServerDataId == sqlServerDataId).SingleOrDefault();
            ICpuDataSource source = new CpuDataSource(sqlServer.ConnectionString);
            return  mapper.DefaultContext.Mapper.Map<List<CpuLoadViewModel>>(source.Get(numberOfRecords));

        }
        protected override Type LogPrefix => GetType();
    }

}
