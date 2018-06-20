using AutoMapper;
using AutoMapper.QueryableExtensions;
using Common;
using SqlHealthMonitor.BLL.Models;
using SqlHealthMonitor.DAL.Models.Widgets;
using SqlHealthMonitor.DAL.Repositories;
using System;
using System.Collections.Generic;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace SqlHealthMonitor.BLL.Services
{
    public class WidgetService : ServiceBase, IWidgetService
    {
        private IWidgetRepository _widgetRep;
        private ISqlServerDataRepository _sqlServerDataRep;
        private MapperConfiguration _widgetViewModelToWidgetBase;
        public WidgetService(IWidgetRepository widgetRep, ISqlServerDataRepository sqlServerDataRep)
        {
            _sqlServerDataRep = sqlServerDataRep;
            _widgetRep = widgetRep;
            _widgetViewModelToWidgetBase = new MapperConfiguration(cfg =>
            {

                cfg.CreateMap<WidgetViewModelBase, WidgetBase>();
                cfg.CreateMissingTypeMaps = true;
            });
        }
     
       
        public List<WidgetViewModelBase> Read(Expression<Func<WidgetBase, bool>> predicate)
        {
            List<WidgetViewModelBase> list = new List<WidgetViewModelBase>();
           
            foreach (var item in _widgetRep.GetAll(predicate))
            {
                var transformed = item.Transform<WidgetViewModelBase>();
                if (transformed.GetType().Name.Contains("ViewModel"))
                transformed.Type = (WidgetType)Enum.Parse(typeof(WidgetType), item.GetType().Name);
                else
                    throw new ArgumentException("the type name of the widget isnt the same as WidgetType enum");
                list.Add(transformed);
            }
            return list;
        }
      
        public List<T> Read<T,EfType>(Expression<Func<EfType, bool>> predicate) where T : WidgetViewModelBase
        {
            var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<WidgetBase, T>();
                cfg.CreateMap<EfType, CpuWidgetViewModel>();
            });
            var query= _widgetRep.GetQueryable().OfType<EfType>().Where(predicate).ProjectTo<T>(config).ToList();
            return query;
        }
      
        public WidgetViewModelBase Create(WidgetViewModelBase widgetView)
        {
         
            var widgetType = TypeHelper.FindType(widgetView.Type.ToString());
            WidgetBase widget =(WidgetBase)Activator.CreateInstance(widgetType);
            _widgetViewModelToWidgetBase.CreateMapper().Map(widgetView,widget);
            _widgetRep.Add(widget);
            _widgetRep.Save();
            widgetView.WidgetId = widget.WidgetId;
            return widgetView;
        }
        public WidgetViewModelBase Update(WidgetViewModelBase widgetView, string userId)
        {
            widgetView.ApplicationUserId = userId;
            WidgetBase widget = null;
            var widgetType = TypeHelper.FindType(widgetView.Type.ToString());
                widget = widgetView.Transform<WidgetBase>();
            _widgetRep.Update(widget);
            _widgetRep.Save();
            return null;
        }
        public void Delete(int widgetId, string UserId)
        {
            var itemToDelete = _widgetRep.GetSingle(x => x.ApplicationUserId == UserId
              && x.WidgetId == widgetId);
            _widgetRep.Delete(itemToDelete);
            _widgetRep.Save();
        }
    
        protected override Type LogPrefix => GetType();
    }
}
