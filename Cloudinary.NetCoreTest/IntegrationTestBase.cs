using System.Reflection;
using NUnit.Framework;

namespace CloudinaryDotNet.Test
{
    [TestFixture]
    public partial class IntegrationTestBase
    {

        [OneTimeSetUp]
        public virtual void Initialize()
        {
            Initialize(typeof(IntegrationTestBase).GetTypeInfo().Assembly);
        }

    }
}
