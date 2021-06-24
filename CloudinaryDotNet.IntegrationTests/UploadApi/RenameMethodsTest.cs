using System.Net;
using System.Threading.Tasks;
using CloudinaryDotNet.Actions;
using NUnit.Framework;

namespace CloudinaryDotNet.IntegrationTests.UploadApi
{
    public class RenameMethodsTest : IntegrationTestBase
    {
        private static string m_context;

        public override void Initialize()
        {
            base.Initialize();

            var contextKey = $"{m_uniqueTestId}_context_key";
            var contextValue = $"{m_uniqueTestId}_context_value";
            m_context = $"{contextKey}={contextValue}";
        }

        [Test, RetryWithDelay]
        public async Task TestRename()
        {
            var toPublicId = GetUniquePublicId();

            var uploadParams = new ImageUploadParams()
            {
                File = new FileDescription(m_testImagePath),
                Tags = m_apiTag
            };
            var uploadResult1 = await m_cloudinary.UploadAsync(uploadParams);

            uploadParams.File = new FileDescription(m_testIconPath);
            var uploadResult2 = await m_cloudinary.UploadAsync(uploadParams);

            var renameResult = await m_cloudinary.RenameAsync(uploadResult1.PublicId, toPublicId);
            Assert.AreEqual(HttpStatusCode.OK, renameResult.StatusCode, renameResult.Error?.Message);

            var getResult = await m_cloudinary.GetResourceAsync(toPublicId);
            Assert.NotNull(getResult);

            renameResult = await m_cloudinary.RenameAsync(uploadResult2.PublicId, toPublicId);
            Assert.True(renameResult.StatusCode == HttpStatusCode.BadRequest, renameResult.Error?.Message);

            await m_cloudinary.RenameAsync(uploadResult2.PublicId, toPublicId, true);

            getResult = await m_cloudinary.GetResourceAsync(toPublicId);
            Assert.NotNull(getResult);
            Assert.AreEqual(FILE_FORMAT_ICO, getResult.Format, getResult.Error?.Message);
        }

        [Test, RetryWithDelay]
        public async Task TestRenameToType()
        {
            string publicId = GetUniquePublicId();
            string newPublicId = GetUniquePublicId();

            var uploadParams = new ImageUploadParams()
            {
                PublicId = publicId,
                File = new FileDescription(m_testImagePath),
                Tags = m_apiTag,
                Type = STORAGE_TYPE_UPLOAD
            };

            var uploadResult = await m_cloudinary.UploadAsync(uploadParams);
            Assert.AreEqual(uploadResult.StatusCode, HttpStatusCode.OK, uploadResult.Error?.Message);

            RenameParams renameParams = new RenameParams(publicId, newPublicId)
            {
                ToType = STORAGE_TYPE_UPLOAD
            };

            var renameResult = await m_cloudinary.RenameAsync(renameParams);
            Assert.AreEqual(renameResult.StatusCode, HttpStatusCode.OK, renameResult.Error?.Message);
            Assert.AreEqual(renameResult.Type, STORAGE_TYPE_UPLOAD);
            Assert.AreEqual(renameResult.PublicId, newPublicId);
        }

        [Test]
        public async Task TestRenameReturnsContext()
        {
            string publicId = GetUniquePublicId();
            string newPublicId = GetUniquePublicId();

            await UploadImage(publicId);

            var renameResult = await m_cloudinary.RenameAsync(publicId, newPublicId, context: true);
            Assert.IsTrue(renameResult.Context.HasValues);

            renameResult = await m_cloudinary.RenameAsync(newPublicId, publicId);
            Assert.IsNull(renameResult.Context);
        }

        [Test]
        public async Task TestRenameReturnsMetadata()
        {
            string publicId = GetUniquePublicId();
            string newPublicId = GetUniquePublicId();

            await UploadImage(publicId, true);

            var renameResult = await m_cloudinary.RenameAsync(publicId, newPublicId, metadata: true);
            Assert.IsTrue(renameResult.MetadataFields.HasValues);

            renameResult = await m_cloudinary.RenameAsync(newPublicId, publicId);
            Assert.IsNull(renameResult.MetadataFields);
        }

        private async Task UploadImage(string publicId, bool withMetadata = false)
        {
            if (withMetadata)
            {
                CreateMetadataField("rename_with_metadata", p => p.DefaultValue = p.Label);
            }

            var uploadParams = new ImageUploadParams()
            {
                PublicId = publicId,
                File = new FileDescription(m_testImagePath),
                Tags = m_apiTag,
                Type = STORAGE_TYPE_UPLOAD,
                Context = new StringDictionary(m_context),
            };

            var uploadResult = await m_cloudinary.UploadAsync(uploadParams);
            Assert.AreEqual(uploadResult.StatusCode, HttpStatusCode.OK, uploadResult.Error?.Message);
        }
    }
}
