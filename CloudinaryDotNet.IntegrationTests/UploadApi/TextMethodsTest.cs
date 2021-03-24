using CloudinaryDotNet.Actions;
using NUnit.Framework;
using System.Threading.Tasks;

namespace CloudinaryDotNet.IntegrationTests.UploadApi
{
    public class TextMethodsTest : IntegrationTestBase
    {
        [Test, RetryWithDelay]
        public void TestEnglishText()
        {
            var textParams = GetEnglishTextParams();

            var textResult = m_cloudinary.Text(textParams);

            AssertEnglishText(textResult);
        }

        [Test, RetryWithDelay]
        public async Task TestEnglishTextAsync()
        {
            var textParams = GetEnglishTextParams();

            var textResult = await m_cloudinary.TextAsync(textParams);

            AssertEnglishText(textResult);
        }

        private TextParams GetEnglishTextParams()
        {
            return new TextParams("Sample text.")
            {
                Background = "red",
                FontStyle = "italic",
                PublicId = GetUniqueAsyncPublicId(StorageType.text)
            };
        }

        private void AssertEnglishText(TextResult result)
        {
            Assert.Greater(result.Width, 0, result.Error?.Message);
            Assert.Greater(result.Height, 0);

            Assert.NotNull(result.PublicId);
            Assert.NotNull(result.Version);
            Assert.NotNull(result.Signature);
            Assert.AreEqual(ResourceType.Image, result.ResourceType);
            Assert.NotNull(result.Format);
            Assert.NotNull(result.CreatedAt);
            Assert.NotZero(result.Bytes);
            Assert.NotNull(result.Type);
            Assert.NotNull(result.Placeholder);
            Assert.NotNull(result.Url);
            Assert.NotNull(result.SecureUrl);
        }

        [Test, RetryWithDelay]
        public void TestRussianText()
        {
            TextParams tParams = new TextParams("Пример текста.")
            {
                PublicId = GetUniquePublicId(StorageType.text)
            };

            TextResult textResult = m_cloudinary.Text(tParams);

            Assert.AreEqual(100, textResult.Width, textResult.Error?.Message);
            Assert.AreEqual(13, textResult.Height);
        }
    }
}
