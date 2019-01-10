using NUnit.Framework;
using NUnit.Framework.Constraints;
using System;
using System.Collections.Generic;

namespace CloudinaryDotNet.Test
{
    [TestFixture]
    public partial class ApiTest
    {
        [Test]
        public void TestRadius()
        {
            var radiusTestValues = new Dictionary<Radius, string>
            {
                {new Radius(10), "10"},
                {new Radius("10"), "10"},
                {new Radius("$v"), "$v"},
                {new Radius("10:20"), "10:20"},
                
                {new Radius(10, 20), "10:20"},
                {new Radius(10, 20, 30), "10:20:30"},
                {new Radius("10:20:$v:40"), "10:20:$v:40"},
                {new Radius(10, 20, "$v", 40), "10:20:$v:40"},
                
                {new Radius(new[] {10, 20, 30}), "10:20:30"},
                {new Radius(new List<object> {10, 20, "$v"}), "10:20:$v"},
                {new Radius(new object[] {10, 20, "$v", 40}), "10:20:$v:40"},
                {new Radius(new string[] {"10:20"}), "10:20"},
                {new Radius(new List<string> {"10:20:$v:40"}), "10:20:$v:40"},
                
            };

            foreach (var test in radiusTestValues)
                Assert.AreEqual(test.Value, test.Key.ToString());

            ExactTypeConstraint ThrowsArgumentNullException() => Throws.TypeOf<ArgumentNullException>();
            ExactTypeConstraint ThrowsArgumentException() => Throws.TypeOf<ArgumentException>();

            Assert.That(() => new Radius(null), ThrowsArgumentNullException());
            Assert.That(() => new Radius(null, 10), ThrowsArgumentNullException());
            Assert.That(() => new Radius(null, 10, 20), ThrowsArgumentNullException());
            Assert.That(() => new Radius(null, 10, 20, 30), ThrowsArgumentNullException());

            Assert.That(() => new Radius(new List<object> { 1, 2, 3, 4, 5 }), ThrowsArgumentException());
            Assert.That(() => new Radius(new List<object>()), ThrowsArgumentException());

        }

        [Test]
        public void TestRadiusClone()
        {
            const string radiusStr = "10:20.5:$v:40";
            var radius = new Radius(10, 20.5f, "$v", 40);
            var cloned = radius.Clone();
            Assert.AreEqual(radiusStr, radius.ToString());
            Assert.AreEqual(radiusStr, cloned.ToString());
        }
    }
}
