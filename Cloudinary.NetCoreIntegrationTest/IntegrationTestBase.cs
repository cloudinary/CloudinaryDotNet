using System.Reflection;
using NUnit.Framework;

namespace CloudinaryDotNet.IntegrationTest
{
    [TestFixture]
    public partial class IntegrationTestBase
    {

        [OneTimeSetUp]
        public virtual void Initialize()
        {
            Initialize(typeof(IntegrationTestBase).GetTypeInfo().Assembly);
        }

        protected virtual string GetMethodTag([System.Runtime.CompilerServices.CallerMemberName]string memberName = "")
        {
            return $"{m_apiTag}_{memberName}";
        }

    }
}
