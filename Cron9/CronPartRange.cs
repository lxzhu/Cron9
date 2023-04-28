using System;
using System.Text.RegularExpressions;

namespace Cron9
{
    public class CronPartRange
    {
        private readonly bool _full;
        private readonly int _min;
        private readonly int _max;
        private readonly string _expression;
        public string Expression => _expression;
        public static readonly Regex RangeRegex = new Regex(@"^([+-]?\d+)(-([+-]?\d+))?$");
        public CronPartRange(string expression)
        {
            _expression = expression;
            if (expression == "*")
            {
                _full = true;
            }
            else
            {
                var match = RangeRegex.Match(expression);
                if (!match.Success)
                {
                    throw new ArgumentException("Range expression does not match regex", nameof(expression));
                }

                _min = int.Parse(match.Groups[1].Value);
                if (match.Groups[3].Success)
                {
                    _max = int.Parse(match.Groups[3].Value);
                }
                else
                {
                    _max = _min;
                }
            }
        }
        public bool IsFull => _full;
        public int Min => _min;
        public int Max => _max;
        public bool InRange(int value) => _full || (value >= _min && value <= _max);
        
        public override string ToString()
        {
           return IsFull?"*":Min==Max?$"{Min}":$"{Min}-{Max}";
        }
    }
}