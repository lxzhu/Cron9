using System;
using System.Diagnostics;
using System.Globalization;
using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Cron9.Tests
{
    [TestClass]
    public class Cron9Test
    {
        
       
        [TestMethod]
        [DataRow("0 0 1 1 *","2023-01-02","2024-01-01","yyyy-MM-dd")]
        public void TestCron5(string expr,string from,string expect,string format)
        {
            var fromDateTime = from.ToDateTime(format);
            var expectDateTime = expect.ToDateTime(format);
            var cron = Cron.Cron5(expr);
            var next = cron.Next(fromDateTime);
            next.Should().Be(expectDateTime);
        }

        [TestMethod]
        [DataRow("0 0 * * * * * 1-4 * *","2023-04-28","2023-05-01","yyyy-MM-dd")]
        public void TestCron9(string expr,string from,string expect,string format)
        {
            var fromDateTime=from.ToDateTime(format);
            var expectDateTime=expect.ToDateTime(format);
            var cron = Cron.Cron9(expr);
            var next = cron.Next(fromDateTime);
            next.Should().Be(expectDateTime);
        }
    }
}