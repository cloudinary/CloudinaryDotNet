using System.Collections.Generic;
using NUnit.Framework;

namespace CloudinaryDotNet.IntegrationTest.AdminApi
{
    public class UsageReportTest: IntegrationTestBase
    {
        [Test]
        public void TestUsage()
        {
            UploadTestResource(); // making sure at least one resource exists

            var result = m_cloudinary.GetUsage();

            var plans = new List<string>() { "Free", "Advanced" };

            Assert.True(plans.Contains(result.Plan));
            Assert.True(result.Resources > 0);
            Assert.True(result.Objects.Used < result.Objects.Limit);
            Assert.True(result.Bandwidth.Used < result.Bandwidth.Limit);

        }
    }
}
