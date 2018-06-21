using SqlHealthMonitor.BLL.Models;
using SqlHealthMonitor.DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace SqlHealthMonitor.BLL.Services
{
    public interface ISqlServerDataService
    {
      /// <summary>
      /// Get SqlServerData and transform them to ViewModel
      /// by using automapper
      /// </summary>
      /// <typeparam name="SqlServerDataViewModel"></typeparam>
      /// <param name="predicate"></param>
      /// <returns></returns>
        List<SqlServerDataViewModel> Read<SqlServerDataViewModel>(Expression<Func<SqlServerData, bool>> predicate);
        /// <summary>
        /// Get SqlServerData
        /// </summary>
        /// <param name="predicate"></param>
        /// <returns></returns>
        List<SqlServerData> Read(Expression<Func<SqlServerData, bool>> predicate);
        /// <summary>
        /// Transform from VIewModel to EF entity,return ViewModel with new Id in Database
        /// </summary>
        /// <param name="sqlServer"></param>
        /// <returns></returns>
       SqlServerDataViewModel Create(SqlServerDataViewModel sqlServer);
        /// <summary>
        /// Delete record with regards to logged user
        /// </summary>
        /// <param name="sqlServerDataId"></param>
        /// <param name="UserId"></param>
        void Delete(int sqlServerDataId, string UserId);
        SqlServerDataViewModel Update(SqlServerDataViewModel sqlServer,string UserId);
    }
}