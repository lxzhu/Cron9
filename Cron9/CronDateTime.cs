using System;
using System.Collections.Generic;
using System.Text;

namespace Cron9
{
    public static class CronDateTime
    {
        public static DateTime Reset0(this DateTime input) => new DateTime(input.Year, input.Month, input.Day, input.Hour, input.Minute, input.Second, 0);
        public static DateTime Reset1(this DateTime input) => new DateTime(input.Year, input.Month, input.Day, input.Hour, input.Minute, 0, 0);
        public static DateTime Reset2(this DateTime input) => new DateTime(input.Year, input.Month, input.Day, input.Hour, 0, 0, 0);
        public static DateTime Reset3(this DateTime input) => new DateTime(input.Year, input.Month, input.Day, 0, 0, 0, 0);
        public static DateTime Reset4(this DateTime input) => new DateTime(input.Year, input.Month, 1, 0, 0, 0, 0);
        public static DateTime Reset5(this DateTime input) => new DateTime(input.Year, 1, 1, 0, 0, 0, 0);

        public static DateTime AddPart(this DateTime input, CronPartKind kind, int n)
        {
            if (n == 0) return input;
            switch (kind)
            {
                case CronPartKind.Millisecond:
                    return input.AddMilliseconds(n);
                case CronPartKind.Second:
                    return input.AddSeconds(n).Reset0();
                case CronPartKind.Minute:
                    return input.AddMinutes(n).Reset1();
                case CronPartKind.Hour:
                    return input.AddHours(n).Reset2();
                case CronPartKind.DayOfMonth:
                    return input.AddDays(n).Reset3();
                case CronPartKind.DayOfWeek:
                    return input.AddDays(n).Reset3();
                case CronPartKind.WeekOfMonth:
                    return input.AddDays(n * 7).Reset3();
                case CronPartKind.WeekOfYear:
                    return input.AddDays(n * 7).Reset3();
                case CronPartKind.Month:
                    return input.AddMonths(n).Reset4();
                case CronPartKind.Year:
                    return input.AddYears(n).Reset5();
                default:
                    return input;
            }
        }

        public static int GetPart(this DateTime input, CronPartKind kind)
        {
            switch (kind)
            {
                case CronPartKind.Millisecond:
                    return input.Millisecond;
                case CronPartKind.Second:
                    return input.Second;
                case CronPartKind.Minute:
                    return input.Minute;
                case CronPartKind.Hour:
                    return input.Hour;
                case CronPartKind.DayOfMonth:
                    return input.Day;
                case CronPartKind.DayOfWeek:
                    return (int)input.DayOfWeek;
                case CronPartKind.WeekOfMonth:
                    return input.Day / 7 + 1;
                case CronPartKind.WeekOfYear:
                    return input.DayOfYear / 7 + 1;
                case CronPartKind.Month:
                    return input.Month;
                case CronPartKind.Year:
                    return input.Year;
                default:
                    throw new ArgumentException($"Invalid CronPartKind {kind}", nameof(kind));

            }
        }

        public static (int, int) GetFullRange(this DateTime input, CronPartKind kind)
        {
            switch (kind)
            {
                case CronPartKind.Millisecond:
                    return (input.Millisecond, 999);
                case CronPartKind.Second:
                    return (input.Second, 59);
                case CronPartKind.Minute:
                    return (input.Minute, 59);
                case CronPartKind.Hour:
                    return (input.Hour, 23);
                case CronPartKind.DayOfMonth:
                    return (input.Day, DateTime.DaysInMonth(input.Year, input.Month));
                case CronPartKind.DayOfWeek:
                    return ((int)input.DayOfWeek, 6);
                case CronPartKind.WeekOfMonth:
                    return (input.Day / 7 + 1, 5);
                case CronPartKind.WeekOfYear:
                    return (input.DayOfYear / 7 + 1, 52);
                case CronPartKind.Month:
                    return (input.Month, 12);
                case CronPartKind.Year:
                    return (input.Year, 9998);
                default:
                    throw new ArgumentException($"Invalid CronPartKind {kind}", nameof(kind));
            }
        }
    }

}
