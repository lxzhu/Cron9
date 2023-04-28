using System;
using System.Collections.Generic;
using System.Text;

namespace Cron9
{
    public class CronPart
    {
        public CronPartKind Kind { get; private set; }
        public CronPartRange Range { get; private set; }
        public int Every { get; private set; }

        public CronPart(CronPartKind kind, string expression)
        {
            this.Kind = kind;
            var parts = expression.Split('/');
            if (parts.Length == 1)
            {
                this.Range = new CronPartRange(parts[0]);
                this.Every = 1;

            }else if (parts.Length == 2)
            {
                this.Range = new CronPartRange(parts[0]);
                this.Every = int.Parse(parts[1]);
            }
            else throw new ArgumentException($"{kind} expression {expression} is invalid",
                nameof(expression));
        }

        public override string ToString()
        {
            return Every == 1 ? $"{Kind}:{Range}" : $"{Kind}:{Range}/{Every}";
        }
    }
}
