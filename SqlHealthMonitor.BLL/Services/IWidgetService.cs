using SqlHealthMonitor.BLL.Models;
using SqlHealthMonitor.DAL.Models.Widgets;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace SqlHealthMonitor.BLL.Services
{
    public interface IWidgetService
    {
        /// <summary>
        /// Read EF widget entities based on predicate,transform them into widgetViewModeBase with repsect to 
        /// All properties and derived types from EF widget entity
        /// </summary>
        /// <param name="predicate"></param>
        /// <returns></returns>
        List<WidgetViewModelBase> Read(Expression<Func<WidgetBase, bool>> predicate);
        /// <summary>
        /// Select only widget based on EfType and predicate then transform them to ViewModel of T
        /// with respect to All properties
        /// </summary>
        /// <typeparam name="T">Transformed returned type</typeparam>
        /// <typeparam name="EfType">Entity Framework Type</typeparam>
        /// <param name="predicate"></param>
        /// <returns></returns>
        List<T> Read<T, EfType>(Expression<Func<EfType, bool>> predicate) where T : WidgetViewModelBase;
        /// <summary>
        ///  Transform from VIewModel to EF entity,return ViewModel with new Id in Database
        /// </summary>
        /// <param name="sqlServer"></param>
        /// <returns></returns>
        WidgetViewModelBase Create(WidgetViewModelBase sqlServer);
        void Delete(int sqlServerDataId, string UserId);
        /// <summary>
        ///  Transform from VIewModel to EF entity and update record in the Database
        /// </summary>
        /// <param name="sqlServer"></param>
        /// <param name="UserId"></param>
        /// <returns></returns>
        WidgetViewModelBase Update(WidgetViewModelBase sqlServer, string UserId);
    }
}