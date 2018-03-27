using System;

namespace SqlHealthMonitor.DAL.Managers
{
    public class DataBaseTypeAttribute : Attribute
    {
        public DataBaseTypeAttribute(Type type)
        {
            Type = type;
        }

        public Type Type { get; set; }
    }
}
