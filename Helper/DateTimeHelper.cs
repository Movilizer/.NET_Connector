using System;
using System.Globalization;

namespace MWS.Helper
{
    public static class DateTimeHelper
    {
        public static int GetYear() => CultureInfo.CurrentCulture.Calendar.GetYear(DateTime.Now);

        public static int GetWeekOfYear() => CultureInfo.CurrentCulture.Calendar.GetWeekOfYear(DateTime.Now, CalendarWeekRule.FirstDay, DayOfWeek.Monday) - 1;

        public static string FormatDate(DateTime dt) => dt.ToString("d", CultureInfo.CurrentCulture.DateTimeFormat);

        public static DateTime ToDateTime(long timestamp)
        {
            DateTime converted = new DateTime(1970, 1, 1, 0, 0, 0, 0);

            //add the timestamp to the value
            DateTime newDateTime = converted.AddSeconds(timestamp / 1000);

            return newDateTime.ToLocalTime();
        }

        public static string GetTimestamp(this DateTime value, bool forFilename)
        {
            if (forFilename)
            {
                return value.ToString("yyyy-MM-ddTHH.mm.ss.ffffZ");
            }
            else
            {
                return value.ToString("yyyy-MM-ddTHH:mm:ss.ffffZ");
            }
        }

        public static string GetTimestamp(bool forFilename) => GetTimestamp(DateTime.Now, forFilename);
    }
}
