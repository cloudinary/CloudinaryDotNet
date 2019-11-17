using CloudinaryDotNet.Actions;
using NUnit.Framework;
using System.Threading.Tasks;

namespace CloudinaryDotNet.IntegrationTest.UploadApi
{
    public class TextMethodsTest : IntegrationTestBase
    {
        [Test]
        public void TestEnglishText()
        {
            var textParams = GetEnglishTextParams();

            var textResult = m_cloudinary.Text(textParams);

            AssertEnglishText(textResult);
        }

        [Test]
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
            Assert.Greater(result.Width, 0);
            Assert.Greater(result.Height, 0);
        }

        [Test]
        public void TestRussianText()
        {
            TextParams tParams = new TextParams("Пример текста.")
            {
                PublicId = GetUniquePublicId(StorageType.text)
            };

            TextResult textResult = m_cloudinary.Text(tParams);

            Assert.AreEqual(100, textResult.Width);
            Assert.AreEqual(13, textResult.Height);
        }
    }
}
