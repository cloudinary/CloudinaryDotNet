using NUnit.Framework;
using CloudinaryDotNet.HtmlTags;

namespace CloudinaryDotNet.Test
{
    [TestFixture]
    class HtmlTagTest : IntegrationTestBase
    {
        [Test]
        public void SimpleHtmlTag()
        {
            var tag = new HtmlTag("dummy");
            Assert.AreEqual("<dummy></dummy>", tag.ToString());
        }

        [Test]
        public void VoidHtmlTag()
        {
            var tag = new HtmlTag("img");
            Assert.AreEqual("<img>", tag.ToString());
        }

        [Test]
        public void HtmlTagWithAttributes()
        {
            var tag = new HtmlTag("dummy").Attr("name", "value");
            Assert.AreEqual("<dummy name=\"value\"></dummy>", tag.ToString());
            tag.Attr("name2", "value2");
            Assert.AreEqual("<dummy name=\"value\" name2=\"value2\"></dummy>", tag.ToString());
            tag.Attr("name3", "value_'with'_\"quotes\"");
            Assert.AreEqual("<dummy name=\"value\" name2=\"value2\" name3=\"value_&#39;with&#39;_&quot;quotes&quot;\"></dummy>", tag.ToString());
        }

        [Test]
        public void HtmlTagWithClass()
        {
            var tag = new HtmlTag("dummy");

            // Add a simple class
            tag.Class("class0");
            Assert.AreEqual("<dummy class=\"class0\"></dummy>", tag.ToString());

            // Add multiple classes
            tag.Class("class1 class2");
            Assert.AreEqual("<dummy class=\"class0 class1 class2\"></dummy>", tag.ToString());

            //Add a class using Attr
            tag.Attr("class", "class3");
            Assert.AreEqual("<dummy class=\"class0 class1 class2 class3\"></dummy>", tag.ToString());

            // Add multiple classes using Attr
            tag.Attr("class", "class4 class5");
            Assert.AreEqual("<dummy class=\"class0 class1 class2 class3 class4 class5\"></dummy>", tag.ToString());

            // Add duplicate class, should be ignored
            tag.Class("class0");
            Assert.AreEqual("<dummy class=\"class0 class1 class2 class3 class4 class5\"></dummy>", tag.ToString());
        }
    }
}
