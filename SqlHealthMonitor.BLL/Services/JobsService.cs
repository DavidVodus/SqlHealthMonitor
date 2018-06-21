using AutoMapper;
using SqlHealthMonitor.BLL.Models;
using SqlHealthMonitor.DAL.Repositories;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SqlHealthMonitor.BLL.Services
{
    public class JobsService : ServiceBase, IJobsService
    {
       

       
        private DbContext _dbContext;
        private ISqlServerDataRepository _sqlServerDataRepository;
        public JobsService(DbContext context, ISqlServerDataRepository sqlServerDataRepository)
        {
            _sqlServerDataRepository = sqlServerDataRepository;
            _dbContext = context;

        }
        public List<SqlJobsViewModel> Get(int sqlServerDataId, string currentUserId, string jtSorting)
        {

            var _config = new MapperConfiguration(cfg =>
            {

                cfg.CreateMissingTypeMaps = true;
            });

            var mapper = new Mapper(_config);

            var sqlServer = _sqlServerDataRepository.GetQueryable().Where
              (x => x.ApplicationUserId == currentUserId && x.SqlServerDataId == sqlServerDataId).SingleOrDefault();
            IJobsDataSource source = new JobsDataSource(sqlServer.ConnectionString);
            return mapper.DefaultContext.Mapper.Map<List<SqlJobsViewModel>>(source.Get(jtSorting));

        }
        protected override Type LogPrefix => GetType();
    }

}


