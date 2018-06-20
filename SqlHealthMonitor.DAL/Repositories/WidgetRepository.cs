using SqlHealthMonitor.DAL.Models.Widgets;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SqlHealthMonitor.DAL.Repositories
{
    public class WidgetRepository : RepositoryBase<WidgetBase>, IWidgetRepository
    {
        public WidgetRepository(DbContext context) : base(context)
        {
        }

        protected override Type LogPrefix => GetType();
    }
}
