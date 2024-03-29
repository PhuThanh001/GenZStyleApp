using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GenZStyleAPP.BAL.DTOs.Authencications
{
    public static class SettedUpDateTime
    {
        public static DateTime GetCurrentVietNamTime()
        {
            TimeZoneInfo vietnamTimeZone = TimeZoneInfo.FindSystemTimeZoneById(
                "SE Asia Standard Time"
            );
            DateTime currentUtcTime = DateTime.UtcNow;
            return TimeZoneInfo.ConvertTimeFromUtc(currentUtcTime, vietnamTimeZone);
        }

        public static DateTime GetCurrentVietNamTimeWithDateOnly()
        {
            TimeZoneInfo vietnamTimeZone = TimeZoneInfo.FindSystemTimeZoneById(
                "SE Asia Standard Time"
            );
            DateTime currentUtcTime = DateTime.UtcNow;
            DateTime dateTime = TimeZoneInfo.ConvertTimeFromUtc(currentUtcTime, vietnamTimeZone);
            return new DateTime(dateTime.Year, dateTime.Month, dateTime.Day);
        }
    }
}
