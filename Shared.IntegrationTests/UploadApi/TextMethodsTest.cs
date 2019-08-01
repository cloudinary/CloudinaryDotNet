﻿using CloudinaryDotNet.Actions;
using NUnit.Framework;
using System.Threading.Tasks;

namespace CloudinaryDotNet.IntegrationTest.UploadApi
{
    public class TextMethodsTest : IntegrationTestBase
    {
        [Test]
        public void TestEnglishText()
        {
            TextParams tParams = new TextParams("Sample text.")
            {
                Background = "red",
                FontStyle = "italic",
                PublicId = GetUniquePublicId(StorageType.text)
            };

            TextResult textResult = m_cloudinary.Text(tParams);

            Assert.IsTrue(textResult.Width > 0);
            Assert.IsTrue(textResult.Height > 0);
        }

        [Test]
        public async Task TestEnglishTextAsync()
        {
            var tParams = new TextParams("Sample text.")
            {
                Background = "red",
                FontStyle = "italic",
                PublicId = GetUniqueAsyncPublicId(StorageType.text)
            };

            var textResult = await m_cloudinary.TextAsync(tParams);

            Assert.IsTrue(textResult.Width > 0);
            Assert.IsTrue(textResult.Height > 0);
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
