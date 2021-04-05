using System.Collections.Generic;
using CloudinaryDotNet.Actions;
using NUnit.Framework;

#pragma warning disable 0618
namespace CloudinaryDotNet.IntegrationTests.UploadApi
{
    public class ContextMethodsTest : IntegrationTestBase
    {
        [Test, RetryWithDelay]
        public void TestAddContext()
        {
            var publicId = GetUniquePublicId();

            var uploadParams = new ImageUploadParams()
            {
                File = new FileDescription(m_testImagePath),
                PublicId = publicId,
                Overwrite = true,
                Type = STORAGE_TYPE_UPLOAD,
                Tags = m_apiTag
            };

            m_cloudinary.Upload(uploadParams);

            List<string> pIds = new List<string> { publicId };

            ContextResult contextResult = m_cloudinary.Context(new ContextParams()
            {
                Command = ContextCommand.Add,
                PublicIds = pIds,
                Type = STORAGE_TYPE_UPLOAD,
                Context = "TestContext",
                ResourceType = ResourceType.Image
            });

            Assert.True(contextResult.PublicIds.Length > 0, contextResult.Error?.Message);

            m_cloudinary.GetResource(new GetResourceParams(pIds[0])
            {
                PublicId = pIds[0],
                Type = STORAGE_TYPE_UPLOAD,
                ResourceType = ResourceType.Image
            });
        }

        [Test, RetryWithDelay]
        public void TestContextEscaping()
        {
            var context = new StringDictionary();
            context.Add("key", "val=ue");

            var uploadParams = new ImageUploadParams { Context = context };
            Assert.AreEqual(@"key=val\=ue", uploadParams.ToParamsDictionary()["context"]);

            context.Add(@"hello=world|2", "goodbye|wo=rld2");

            var contextParams = new ContextParams()
            {
                Context = @"val\=ue",
                ContextDict = context
            };

            Assert.AreEqual(@"key=val\=ue|hello\=world\|2=goodbye\|wo\=rld2|val\=ue", contextParams.ToParamsDictionary()["context"]);
        }

        [Test, RetryWithDelay]
        public void TestContext()
        {
            //should allow sending context

            var uploadParams = new ImageUploadParams()
            {
                File = new FileDescription(m_testImagePath),
                PublicId = GetUniquePublicId(),
                Context = new StringDictionary("key=value", "key2=value2"),
                Tags = m_apiTag
            };

            var uploaded = m_cloudinary.Upload(uploadParams);

            var res = m_cloudinary.GetResource(uploaded.PublicId);

            Assert.AreEqual("value", res?.Context["custom"]?["key"]?.ToString(), res.Error?.Message);
            Assert.AreEqual("value2", res?.Context["custom"]?["key2"]?.ToString());
        }
    }
}
#pragma warning restore 0618
