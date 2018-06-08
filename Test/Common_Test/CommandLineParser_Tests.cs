using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using FluentAssertions;
using System.Linq;

namespace HisRoyalRedness.com.Tests
{
    [TestClass]
    public class CommandLineParser_Test
    {
        [DataTestMethod]
        [DataRow(false, "-1, /2, \\3", "1,2,3")]
        public void TestSwitchPrefixes(bool caseSensitive, string argumentList, string expectedList)
        {
            var cmdLine = new CommandLineParser(caseSensitive, argumentList.ListToArray(), CommandLineParser.DEFAULT_SWITCH_PREFIXES);
            var expectedSwitches = expectedList.ListToArray();
            cmdLine.Switches.Should().HaveCount(expectedSwitches.Length);
            cmdLine.Switches.Should().Contain(expectedSwitches);
        }

    }

    internal static class TestExtensions
    {
        internal static string[] ListToArray(this string list)
            => list.Split(',').Select(a => a.Trim()).ToArray();
    }
}