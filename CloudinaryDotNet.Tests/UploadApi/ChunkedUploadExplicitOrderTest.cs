using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using CloudinaryDotNet.Actions;
using NUnit.Framework;

namespace CloudinaryDotNet.Tests.UploadApi
{
    public class ChunkedUploadExplicitOrderTest
    {
        private const string SuccessJson =
            "{\"public_id\":\"x\",\"version\":1,\"signature\":\"s\",\"width\":1,\"height\":1," +
            "\"format\":\"png\",\"resource_type\":\"image\",\"created_at\":\"2026-01-01T00:00:00Z\"," +
            "\"bytes\":1,\"type\":\"upload\",\"etag\":\"e\",\"url\":\"http://x\",\"secure_url\":\"https://x\"}";

        private static ImageUploadParams BuildParams(string uniqueUploadId = "uid-123")
        {
            return new ImageUploadParams
            {
                File = new FileDescription("chunk.bin", new MemoryStream(new byte[] { 1, 2, 3, 4 })),
                UniqueUploadId = uniqueUploadId,
            };
        }

        [Test]
        public async Task UploadChunk_WithPartNumberAndTotalParts_SendsExplicitOrderHeadersAndSkipsContentRange()
        {
            var cloudinary = new MockedCloudinary(SuccessJson);
            var uploadParams = BuildParams();
            uploadParams.PartNumber = 2;
            uploadParams.TotalParts = 3;

            await cloudinary.UploadChunkAsync(uploadParams);

            Assert.IsTrue(cloudinary.HttpRequestHeaders.Contains("X-Unique-Upload-Id"));
            Assert.AreEqual("uid-123", cloudinary.HttpRequestHeaders.GetValues("X-Unique-Upload-Id").First());
            Assert.IsTrue(cloudinary.HttpRequestHeaders.Contains("X-Upload-Part-Number"));
            Assert.AreEqual("2", cloudinary.HttpRequestHeaders.GetValues("X-Upload-Part-Number").First());
            Assert.IsTrue(cloudinary.HttpRequestHeaders.Contains("X-Upload-Total-Parts"));
            Assert.AreEqual("3", cloudinary.HttpRequestHeaders.GetValues("X-Upload-Total-Parts").First());

            Assert.IsNotNull(cloudinary.HttpContentHeaders);
            Assert.IsFalse(cloudinary.HttpContentHeaders.Contains("Content-Range"));
        }

        [Test]
        public async Task UploadChunk_WithoutPartNumber_StillSendsContentRange()
        {
            var cloudinary = new MockedCloudinary(SuccessJson);
            var uploadParams = BuildParams();

            await cloudinary.UploadChunkAsync(uploadParams);

            Assert.IsTrue(cloudinary.HttpRequestHeaders.Contains("X-Unique-Upload-Id"));
            Assert.IsFalse(cloudinary.HttpRequestHeaders.Contains("X-Upload-Part-Number"));
            Assert.IsFalse(cloudinary.HttpRequestHeaders.Contains("X-Upload-Total-Parts"));

            Assert.IsNotNull(cloudinary.HttpContentHeaders);
            Assert.IsTrue(cloudinary.HttpContentHeaders.Contains("Content-Range"));
        }

        [Test]
        public void UploadChunk_PartNumberWithoutUniqueUploadId_ThrowsArgumentException()
        {
            var cloudinary = new MockedCloudinary(SuccessJson);
            var uploadParams = BuildParams(uniqueUploadId: null);
            uploadParams.PartNumber = 1;

            Assert.ThrowsAsync<ArgumentException>(async () => await cloudinary.UploadChunkAsync(uploadParams));
        }

        [Test]
        public void UploadChunk_TotalPartsWithoutPartNumber_ThrowsArgumentException()
        {
            var cloudinary = new MockedCloudinary(SuccessJson);
            var uploadParams = BuildParams();
            uploadParams.TotalParts = 3;

            Assert.ThrowsAsync<ArgumentException>(async () => await cloudinary.UploadChunkAsync(uploadParams));
        }

        [Test]
        public void UploadChunk_PartNumberExceedsTotalParts_ThrowsArgumentException()
        {
            var cloudinary = new MockedCloudinary(SuccessJson);
            var uploadParams = BuildParams();
            uploadParams.PartNumber = 4;
            uploadParams.TotalParts = 3;

            Assert.ThrowsAsync<ArgumentException>(async () => await cloudinary.UploadChunkAsync(uploadParams));
        }
    }
}
