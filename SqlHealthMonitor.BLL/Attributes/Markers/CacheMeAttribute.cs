using System;
namespace SqlHealthMonitor.BLL.Attributes.Markers
{
    /// <summary>
    /// marking class gives setting to a method that is Cached
    /// </summary>
    public class CacheMeAttribute : Attribute
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="durationInMinutes">slidingExpiration is setted by this param</param>
        public CacheMeAttribute(int durationInMinutes = 60)
        {
            DurationInMinutes = durationInMinutes;
        }
        public int DurationInMinutes { get; set; }
    }
}