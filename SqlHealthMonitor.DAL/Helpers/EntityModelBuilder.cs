using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using SqlHealthMonitor.DAL.Managers;


namespace SqlHealthMonitor.DAL.Helpers
{
  public static class EntityModelBuilder
    {
     

        /// <summary>
        /// return DisplayName annotation, if it's set
        /// return property name if not
        /// </summary>
        /// <param name="property"></param>
        /// <returns></returns>
        public static string GetDisplayName(PropertyInfo property)
        {
            if (property.GetCustomAttributes(typeof(DisplayNameAttribute), true).Cast<DisplayNameAttribute>().Any())
                return property.GetCustomAttributes(typeof(DisplayNameAttribute), true).Cast<DisplayNameAttribute>().Single().DisplayName;
            else
                return property.Name;

        }
    }
}
