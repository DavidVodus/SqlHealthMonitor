using AutoMapper;
using Castle.Windsor;
using SqlHealthMonitor.BLL.Models;
using SqlHealthMonitor.DAL.Managers;
using SqlHealthMonitor.DAL.Repositories;
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



        private DbContext _dbContext;
        private ISqlServerDataRepository _sqlServerDataRepository;
        public DatabasesSizeService(DbContext context, ISqlServerDataRepository sqlServerDataRepository)
        {
            _sqlServerDataRepository = sqlServerDataRepository;
            _dbContext = context;

        }
       
        public List<SqlDatabasesSizeViewModel> Get(int sqlServerDataId, string currentUserId, List<int> databaseIds, string jtSorting)
        {

            var _config = new MapperConfiguration(cfg =>
            {

                cfg.CreateMissingTypeMaps = true;
                //exist just in ViewModel
                cfg.AddGlobalIgnore("SizeMB");
            });

            var mapper = new Mapper(_config);

            var sqlServer = _sqlServerDataRepository.GetQueryable().Where
              (x => x.ApplicationUserId == currentUserId && x.SqlServerDataId == sqlServerDataId).SingleOrDefault();
            IDatabaseSizeDataSource source = new DatabaseSizeDataSource(sqlServer.ConnectionString);
            return mapper.DefaultContext.Mapper.Map<List<SqlDatabasesSizeViewModel>>(source.Get(databaseIds,jtSorting));

        }
        protected override Type LogPrefix => GetType();
    }

}
