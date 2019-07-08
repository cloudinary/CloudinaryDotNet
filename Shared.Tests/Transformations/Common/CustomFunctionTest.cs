using NUnit.Framework;

namespace CloudinaryDotNet.Test.Transformations.Common
{
    [TestFixture]
    public class CustomFunctionTest
    {
        [Test]
        public void TestCustomFunction()
        {
            var customFunc = new Transformation().CustomFunction(CustomFunction.Wasm("blur_wasm")).Generate();

            Assert.AreEqual("fn_wasm:blur_wasm", customFunc);

            customFunc = new Transformation()
                .CustomFunction(
                    CustomFunction.Remote("https://df34ra4a.execute-api.us-west-2.amazonaws.com/default/cloudinaryFunction"))
                .Generate();

            Assert.AreEqual(
                "fn_remote:aHR0cHM6Ly9kZjM0cmE0YS5leGVjdXRlLWFwaS51cy13ZXN0LTIuYW1hem9uYXdzLmNvbS9kZWZhdWx0L2Nsb3VkaW5hcnlGdW5jdGlvbg==",
                customFunc);

            customFunc = new Transformation()
                .CustomFunction(null)
                .Generate();
            Assert.AreEqual("", customFunc);
        }

        [Test]
        public void TestCustomPreFunction()
        {
            var customFunc = new Transformation().CustomPreFunction(CustomFunction.Wasm("blur_wasm")).Generate();

            Assert.AreEqual("fn_pre:wasm:blur_wasm", customFunc);

            customFunc = new Transformation()
                .CustomPreFunction(
                    CustomFunction.Remote("https://df34ra4a.execute-api.us-west-2.amazonaws.com/default/cloudinaryFunction"))
                .Generate();

            Assert.AreEqual(
                "fn_pre:remote:aHR0cHM6Ly9kZjM0cmE0YS5leGVjdXRlLWFwaS51cy13ZXN0LTIuYW1hem9uYXdzLmNvbS9kZWZhdWx0L2Nsb3VkaW5hcnlGdW5jdGlvbg==",
                customFunc);

            customFunc = new Transformation()
                .CustomPreFunction(null)
                .Generate();
            Assert.AreEqual("", customFunc);
        }
    }
}
