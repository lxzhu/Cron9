using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cron9.Tests
{
    public static class StringDateTimeExtension
    {
        public static DateTime ToDateTime(this string input,string format) => DateTime.ParseExact(input, format, System.Globalization.CultureInfo.InvariantCulture);
    }
}
