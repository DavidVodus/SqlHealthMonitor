using System;

namespace Common
{
    /// <summary>
    ///     Convert between unix(long) and standart DateTime
    /// </summary>
    public static class DateTimeHelper
    {
        public static int DateTimeToUnixTimeStamp(DateTime date)
        {
            var dateTimeOffset = new DateTimeOffset(date);
            return Convert.ToInt32(dateTimeOffset.ToUnixTimeSeconds());
        }

        public static DateTime UnixTimeStampToDateTime(long unixTimeStamp)
        {
            var localDateTimeOffset = DateTimeOffset.FromUnixTimeSeconds(unixTimeStamp)
                .DateTime.ToLocalTime();
            return localDateTimeOffset;
        }
    }
}