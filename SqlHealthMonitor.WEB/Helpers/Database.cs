using Castle.Windsor;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace SqlHealthMonitor.Helpers
{
    public static class Database
    {
        /// <summary>
        /// Get DbContext from Ioc container stored in appinstance 
        /// </summary>
        /// <returns></returns>
        public static DbContext GetDbContext()
        {
            var container = (HttpContext.Current.ApplicationInstance as IContainerAccessor).Container;
            return container.Resolve<DbContext>();
        }
    }
}