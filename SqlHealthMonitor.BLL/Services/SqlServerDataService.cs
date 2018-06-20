using AutoMapper;
using AutoMapper.QueryableExtensions;
using SqlHealthMonitor.BLL.Models;
using SqlHealthMonitor.DAL.Managers;
using SqlHealthMonitor.DAL.Models;
using SqlHealthMonitor.DAL.Models.Identity.UserLogin;
using SqlHealthMonitor.DAL.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace SqlHealthMonitor.BLL.Services
{
    public class SqlServerDataService : ServiceBase, ISqlServerDataService
    {
        private ISqlServerDataRepository _sqlServerDataRep;
        public SqlServerDataService(ISqlServerDataRepository sqlServerDataRep)
        {
            _sqlServerDataRep = sqlServerDataRep;
        }
        public List<SqlServerData> Read(Expression<Func<SqlServerData, bool>> predicate)
        {
            return _sqlServerDataRep.GetAll(predicate).ToList();
        }
        public List<SqlServerDataViewModel> Read<SqlServerDataViewModel>(Expression<Func<SqlServerData, bool>> predicate) 
        {
            var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMissingTypeMaps = true;
            });
        
            var query = _sqlServerDataRep.GetQueryable().Where(predicate);
            return query.ProjectTo<SqlServerDataViewModel>(config).ToList();

        }
        public SqlServerDataViewModel Create(SqlServerDataViewModel sqlServerView)
        {
            var config = new MapperConfiguration(cfg => {
                cfg.CreateMap<SqlServerData, SqlServerDataViewModel>(); ;
            });
            var sqlServer = config.CreateMapper().Map<SqlServerData>(sqlServerView);
            _sqlServerDataRep.Add(sqlServer);
            _sqlServerDataRep.Save();
            sqlServerView.SqlServerDataId = sqlServer.SqlServerDataId;
            return sqlServerView;
        }
        public SqlServerDataViewModel Update(SqlServerDataViewModel sqlServerView,string userId)
        {
            var config = new MapperConfiguration(cfg => {
                cfg.CreateMap<SqlServerDataViewModel, SqlServerData>()
                .ForMember(dest => dest.ApplicationUserId, opt => opt.MapFrom(src => userId));
            });
            var sqlServer = config.CreateMapper().Map<SqlServerData>(sqlServerView);
            _sqlServerDataRep.Update(sqlServer);
            _sqlServerDataRep.Save();
            return sqlServerView;
        }
        public void Delete(int sqlServerDataId, string UserId)
        {
             var itemToDelete=_sqlServerDataRep.GetSingle(x=>x.ApplicationUserId==UserId
             &&x.SqlServerDataId== sqlServerDataId);
           _sqlServerDataRep.Delete(itemToDelete);
            _sqlServerDataRep.Save();
        }
        protected override Type LogPrefix => GetType();
    }
}
