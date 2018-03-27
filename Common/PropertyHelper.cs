using System;
using System.Collections.Generic;
using System.Linq;

namespace Common
{
   public static class PropertyHelper
    {
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
