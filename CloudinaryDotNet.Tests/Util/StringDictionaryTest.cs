using NUnit.Framework;
using System;
using System.Collections.Generic;

namespace CloudinaryDotNet.Tests.Util
{
    [TestFixture]
    public class StringDictionaryTest
    {
       [Test]
        public void TestStringDictionaryAddListValue()
        {
            var sd = new StringDictionary();

            sd.Add("k1", new List<string>{"v11", "v12"});
            sd.Add("k2", new List<string>{"v21", "v22"});

            Assert.AreEqual("k1=[\"v11\",\"v12\"]|k2=[\"v21\",\"v22\"]", Utils.SafeJoin("|", sd.SafePairs));
        }

        [Test]
        public void TestStringDictionaryAddListValueSpecialCharacters()
        {
            var sd = new StringDictionary();

            sd.Add("k1", new List<string>{"v11|=\"'!@#$%^*({}[]"});

            Assert.AreEqual(@"k1=[""v11\|\=\""'!@#$%^*({}[]""]", Utils.SafeJoin("|", sd.SafePairs));
        }
    }
}
