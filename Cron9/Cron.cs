using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace Cron9
{
    /// <summary>
    /// * => millisecond
    /// * => second
    /// * => minute
    /// * => hour
    /// * => day of month
    /// * => day of week
    /// * => week of month
    /// * => month
    /// * => week of year
    /// * => year
    /// </summary>
    public class Cron
    {
        private readonly string _expression;
        private readonly CronPart[] _parts;
        private readonly CronPartKind _logLevel;

        public Cron(string expression, CronPartKind logLevel = CronPartKind.Year)
        {
            _expression = expression;
            var expressionParts = expression.Split(' ');
            if (expressionParts.Length != 10)
            {
                throw new ArgumentException("Cron expression must have 10 parts", nameof(expression));
            }
            this._parts = new CronPart[10];

            for (var i = 0; i < expressionParts.Length; i++)
            {
                _parts[i] = new CronPart((CronPartKind)(i), expressionParts[i]);
            }
            _logLevel = logLevel;
        }

        /// <summary>
        /// minute, hour, day of month, month, year
        /// </summary>
        /// <param name="expression"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentException"></exception>
        public static Cron Cron5(string expression)
        {
            var parts = expression.Split(' ');
            if (parts.Length != 5)
                throw new ArgumentException("Cron expression must have 5 parts", nameof(expression));

            return new Cron($"0 0 {parts[0]} {parts[1]} {parts[2]} {parts[3]} {parts[4]} * * *");
        }

        /// <summary>
        /// second,minute, hour, day of month, month, year
        /// </summary>
        /// <param name="expression"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentException"></exception>
        public static Cron Cron6(string expression)
        {
            var parts = expression.Split(' ');
            if (parts.Length != 6)
                throw new ArgumentException("Cron expression must have 5 parts", nameof(expression));

            return new Cron($"0 {parts[0]} {parts[1]} {parts[2]} {parts[3]} {parts[4]} {parts[5]} * * *");
        }

        /// <summary>
        /// millisecond,second,minute, hour, day of month, month, year
        /// </summary>
        /// <param name="expression"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentException"></exception>
        public static Cron Cron7(string expression)
        {
            var parts = expression.Split(' ');
            if (parts.Length != 6)
                throw new ArgumentException("Cron expression must have 5 parts", nameof(expression));

            return new Cron($"{parts[0]} {parts[1]} {parts[2]} {parts[3]} {parts[4]} {parts[5]} {parts[6]} ** * *");
        }

        public static Cron Cron9(string expression) => new Cron(expression);

        public static bool TryParse(string expression, out Cron result)
        {
            result = null;

            try
            {
                result = new Cron(expression);
            }
            catch (Exception ex)
            {
                return false;
            }
            return true;
        }

        public DateTime Next(DateTime from)
        {
            return Next(CronPartKind.Year, from.AddMilliseconds(1), out DateTime result) ? result : DateTime.MinValue;
        }

        private bool Next(CronPartKind kind, DateTime from, out DateTime result)
        {
            var test = result = from;
            var part = _parts[(int)kind];
            bool isInRange = part.Range.InRange(test.GetPart(kind));
            bool isNextLevelInRange = kind == CronPartKind.Millisecond || Next(kind - 1, test, out result);
            bool hasNext = true;
            while (hasNext && (!isInRange || !MeetWeekParts(test, kind) || !isNextLevelInRange))
            {
                hasNext = NextInRange(test, part, out test);
                if (hasNext)
                {
                    isInRange = part.Range.InRange(test.GetPart(kind));
                    isNextLevelInRange = kind == CronPartKind.Millisecond || Next(kind - 1, test, out result);
                }
            }
            return WriteTestCase(result, kind, hasNext);

        }

        private bool NextInRange(DateTime input, CronPart part, out DateTime result)
        {
            var (min, max) = input.GetFullRange(part.Kind);
            min = Math.Max(min + 1, part.Range.Min);
            max = Math.Min(max, part.Range.Max);
            var test = input.AddPart(part.Kind, min - input.GetPart(part.Kind));
            for (var index = min; index <= max; index++)
            {
                if (part.Range.InRange(test.GetPart(part.Kind)))
                    break;
                test = test.AddPart(part.Kind, 1);
            }
            result = part.Range.InRange(test.GetPart(part.Kind)) ? test : DateTime.MinValue;
            return result != DateTime.MinValue;
        }

        private bool IsPartInRange(DateTime input, CronPartKind kind) =>
            _parts[(int)kind].Range.InRange(input.GetPart(kind));

        private bool MeetWeekParts(DateTime input, CronPartKind kind)
        {
            if (kind != CronPartKind.DayOfMonth)
                return true;
            var meetAllWeekChecks = IsPartInRange(input, CronPartKind.DayOfWeek) 
                                    && IsPartInRange(input, CronPartKind.WeekOfMonth) 
                                    && IsPartInRange(input, CronPartKind.WeekOfYear);

            return WriteTestCase(input, kind, meetAllWeekChecks, true);
        }

        private bool WriteTestCase(DateTime test, CronPartKind kind, bool isNext, bool isWeekTests = false)
        {
            if (kind == _logLevel)
            {
                var expression = isWeekTests ? $"{_parts[7]} {_parts[8]} {_parts[9]}" : _expression;
                var logging = isNext ? $"{test} meets {expression}" : $"{test} fails {expression}";
                Console.WriteLine(logging);
            }
            return isNext;
        }
        public IEnumerable<DateTime> Next(DateTime from, int count)
        {
            for (int i = 0; i < count; i++)
            {
                from = Next(from);
                yield return from;
            }
        }
    }
}
