using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Common
{
   public static class PropertyHelper
    {
     
        /// <summary>
        /// Transform object from something to somethingViewModel with all properties and vice versa.
        /// If object derives from base type and the object is presented by base type ,
        /// it iterates all of its properties and put them into the counterpart class represented by ViewModel and Vice Versa
        /// Important!Each model must have its counterpart as example somethingViewModel must have something in an Assembly and vice versa
        /// </summary>
        /// <typeparam name="T">returned type</typeparam>
        /// <param name="myobj"></param>
        /// <param name="excludedProperties"></param>
        /// <returns></returns>
        public static T Transform<T>(this object myobj, List<string> excludedProperties = null)
        {
            var objectType = myobj.GetType();
            string targetTypeName = objectType.Name;
            //from Ef to ViewModel classes
            if (typeof(T).Name.Contains("ViewModel"))
                targetTypeName = objectType.Name + "ViewModel";
            //from ViewModel to Ef classes
            else if (objectType.Name.Contains("ViewModel"))
                targetTypeName = objectType.Name.Replace("ViewModel", "");
            else
                throw new ArgumentException("T type neither contain string 'viewModel' nor myobj contains string 'viewModel'");
            var targetType= TypeHelper.FindType(targetTypeName);
            var targetObj = Activator.CreateInstance(targetType);
            //var targetObj = targetInstance.Unwrap();
           
            // Find common members by name
            var sourceMembers = from source in objectType.GetMembers().ToList()
                                where source.MemberType == MemberTypes.Property
                                select source;
            var targetMembers = from source in targetType.GetMembers().ToList()
                                where source.MemberType == MemberTypes.Property
                                select source;
            var commonMembers = targetMembers.Where(memberInfo => sourceMembers.Select(c => c.Name)
                .ToList().Contains(memberInfo.Name)).ToList();

            // Remove unwanted members
            if(excludedProperties!=null)
            commonMembers.RemoveAll(x => excludedProperties.Contains(x.Name));

            foreach (var memberInfo in commonMembers)
            {
                if (!((PropertyInfo)memberInfo).CanWrite) continue;

                var targetProperty = targetType.GetProperty(memberInfo.Name);
                if (targetProperty == null) continue;

                var sourceProperty = myobj.GetType().GetProperty(memberInfo.Name);
                if (sourceProperty == null) continue;

                // Check source and target types are the same
                if (sourceProperty.PropertyType.Name != targetProperty.PropertyType.Name) continue;

                var value = myobj.GetType().GetProperty(memberInfo.Name)?.GetValue(myobj, null);
                if (value == null) continue;

                // Set the value
                targetProperty.SetValue(targetObj, value, null);
            }

            return (T)targetObj;
        }
        public static object GetValueFromComplexType(string accessor,object source)
        {
            object value = null;
            if (!accessor.Contains("."))
            {
                value = source.GetType().GetProperty(accessor).GetValue(source);
            }
            else
            {
                var splitedAccessor = accessor.Split('.');
                object traversedSource = source.GetType().GetProperty(splitedAccessor.First()).GetValue(source);
                string joinedAccessor = string.Join(".", splitedAccessor.Skip(1).ToArray());
            value=  GetValueFromComplexType(joinedAccessor, traversedSource);
            }
            return value;



        }
        public static List<KeyValuePair<int,string>> EnumToSelectList(Type enumType)
        {
            return Enum
                .GetValues(enumType)
                .Cast<int>()
                .Select(i => new KeyValuePair<int, string>(i, Enum.GetName(enumType, i))
                )
                .ToList();
        }
    }
}
