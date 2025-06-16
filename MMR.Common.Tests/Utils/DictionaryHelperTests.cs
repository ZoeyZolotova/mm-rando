using NUnit.Framework;
using NUnit.Framework.Legacy;
using MMR.Common.Utils;
using System.Collections.Generic;

namespace MMR.Common.Tests.Utils
{
    public class DictionaryHelperTests
    {
        [SetUp]
        public void Setup()
        {
        }

        [Test]
        public void Test1()
        {
            var commandArgument = "-a a -b b -c c c -d";
            var args = commandArgument.Split(' ');
            var result = DictionaryHelper.FromProgramArguments(args);

            CollectionAssert.AreEqual(new List<string> { "-a", "-b", "-c", "-d" }, result.Keys);
            CollectionAssert.AreEqual(new List<string> { "a" }, result["-a"]);
            CollectionAssert.AreEqual(new List<string> { "b" }, result["-b"]);
            CollectionAssert.AreEqual(new List<string> { "c", "c" }, result["-c"]);
            CollectionAssert.AreEqual(new List<string> { }, result["-d"]);
        }

        [Test]
        public void TestQuotes()
        {
            var commandArgument = "-a \"\" -b \"hello\" -c \"hello world\" -d \"\"\"";
            var args = commandArgument.Split(' ');
            var result = DictionaryHelper.FromProgramArguments(args);

            CollectionAssert.AreEqual(new List<string> { "-a", "-b", "-c", "-d" }, result.Keys);
            CollectionAssert.AreEqual(new List<string> { "" }, result["-a"]);
            CollectionAssert.AreEqual(new List<string> { "hello" }, result["-b"]);
            CollectionAssert.AreEqual(new List<string> { "hello world" }, result["-c"]);
            CollectionAssert.AreEqual(new List<string> { "\"" }, result["-d"]);
        }
    }
}
