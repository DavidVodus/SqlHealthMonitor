using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SqlHealthMonitor.BLL;
using SqlHealthMonitor.DAL.Models.Widgets;
using System.Reflection;
using System.Linq;
using SqlHealthMonitor.BLL.Models;
using Common;
using System.Resources;
using System.Collections;
using System.Data.Entity;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace SqlHealthMonitor
{
    [TestClass]
    public class UnitTest1
    {
       
        [TestMethod]
        public void TestMethod1()
        {
            List<int> DatabaseIds=new List<int>{ 1,1,2,5};
            var u = DatabaseIds.Select(x=>x.ToString());
            string.Join(",", u);
              //var widget= new CpuWidget { WidgetId = 5 };
              // WidgetBase widgetBase = widget;

            var widget = new CpuWidgetViewModel { WidgetId = 5 };
            WidgetViewModelBase widgetBase = widget;
            var test = widgetBase.Transform<WidgetBase>();

            var objectType = widgetBase.GetType();
            var widgetTypeName=widgetBase.GetType().Name;
            widgetTypeName = widgetTypeName + "ViewModel";
            var assembly = System.Reflection.Assembly.GetExecutingAssembly().GetName().Name;
            assembly = "SqlHealthMonitor.BLL";
            var targetInstance = Activator.CreateInstance(assembly, "SqlHealthMonitor.BLL.Models."+ widgetTypeName);
            var targetObj = targetInstance.Unwrap();
            var targetType = targetObj.GetType();

            // Find common members by name
            var sourceMembers = from source in objectType.GetMembers().ToList()
                                where source.MemberType == MemberTypes.Property
                                select source;
            var targetMembers = from source in targetType.GetMembers().ToList()
                                where source.MemberType == MemberTypes.Property
                                select source;
            var commonMembers = targetMembers.Where(memberInfo => sourceMembers.Select(c => c.Name)
                .ToList().Contains(memberInfo.Name)).ToList();
          
            foreach (var memberInfo in commonMembers)
            {
                if (!((PropertyInfo)memberInfo).CanWrite) continue;

                var targetProperty = targetType.GetProperty(memberInfo.Name);
                if (targetProperty == null) continue;

                var sourceProperty = widgetBase.GetType().GetProperty(memberInfo.Name);
                if (sourceProperty == null) continue;

                // Check source and target types are the same
                if (sourceProperty.PropertyType.Name != targetProperty.PropertyType.Name) continue;

                var value = widgetBase.GetType().GetProperty(memberInfo.Name)?.GetValue(widgetBase, null);
                if (value == null) continue;

                // Set the value
                targetProperty.SetValue(targetObj, value, null);
            }


        }
    }
}
