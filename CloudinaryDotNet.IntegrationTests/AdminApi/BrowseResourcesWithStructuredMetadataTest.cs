using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using CloudinaryDotNet.Actions;
using NUnit.Framework;

namespace CloudinaryDotNet.IntegrationTests.AdminApi
{
    public class BrowseResourcesWithStructuredMetadataTest: IntegrationTestBase
    {
        private const string MODERATION_MANUAL = "manual";
        private static string m_contextKey;
        private static string m_contextValue;
        private static string m_publicId;

        public override void Initialize()
        {
            base.Initialize();

            m_contextKey = $"{m_uniqueTestId}_context_key";
            m_contextValue = $"{m_uniqueTestId}_context_value";
            var context = $"{m_contextKey}={m_contextValue}";

            CreateMetadataField("resource_with_metadata", p => p.DefaultValue = p.Label);
            var uploadResult = UploadTestImageResource(p =>
            {
                p.Context = new StringDictionary(context);
                p.Moderation = MODERATION_MANUAL;
            });

            m_publicId = uploadResult.PublicId;
        }

        [Test, RetryWithDelay]
        public async Task TestListResources()
        {
            AssertHaveMetadata(await m_cloudinary.ListResourcesAsync(metadata: true));
            AssertHaveNoMetadata(await m_cloudinary.ListResourcesAsync(metadata: false));
        }

        [Test, RetryWithDelay]
        public async Task TestListResourcesByTag()
        {
            AssertHaveMetadata(await m_cloudinary.ListResourcesByTagAsync(m_apiTag, metadata: true));
            AssertHaveNoMetadata(await m_cloudinary.ListResourcesByTagAsync(m_apiTag, metadata: false));
        }

        [Test, RetryWithDelay]
        public async Task TestListResourcesByContext()
        {
            AssertHaveMetadata(await m_cloudinary.ListResourcesByContextAsync(m_contextKey, m_contextValue, metadata: true));
            AssertHaveNoMetadata(await m_cloudinary.ListResourcesByContextAsync(m_contextKey, m_contextValue, metadata: false));
        }

        [Test, RetryWithDelay]
        public async Task TestListResourcesByModerationStatus()
        {
            AssertHaveMetadata(await m_cloudinary.ListResourcesByModerationStatusAsync(MODERATION_MANUAL, ModerationStatus.Pending, metadata: true));
            AssertHaveNoMetadata(await m_cloudinary.ListResourcesByModerationStatusAsync(MODERATION_MANUAL, ModerationStatus.Pending, metadata: false));
        }

        [Test, RetryWithDelay]
        public async Task TestListResourcesByPrefix()
        {
            AssertHaveMetadata(await m_cloudinary.ListResourcesByPrefixAsync(m_publicId, metadata: true));
            AssertHaveNoMetadata(await m_cloudinary.ListResourcesByPrefixAsync(m_publicId, metadata: false));
        }

        [Test, RetryWithDelay]
        public async Task TestListResourcesByType()
        {
            AssertHaveMetadata(await m_cloudinary.ListResourcesByTypeAsync("upload", metadata: true));
            AssertHaveNoMetadata(await m_cloudinary.ListResourcesByTypeAsync("upload", metadata: false));
        }

        [Test, RetryWithDelay]
        public async Task TestListResourcesByPublicIds()
        {
            AssertHaveMetadata(await m_cloudinary.ListResourcesByPublicIdsAsync(new[] { m_publicId }, metadata: true));
            AssertHaveNoMetadata(await m_cloudinary.ListResourcesByPublicIdsAsync(new[] { m_publicId }, metadata: false));
        }

        private void AssertHaveMetadata(ListResourcesResult result) =>
            CommonAssertions(result, true);

        private void AssertHaveNoMetadata(ListResourcesResult result) =>
            CommonAssertions(result, false);

        private void CommonAssertions(ListResourcesResult result, bool shouldHaveMetadata)
        {
            Assert.IsNotNull(result);
            Assert.IsNotNull(result.Resources, result.Error?.Message);

            var resourceWithMetadata = result.Resources.FirstOrDefault(r => r.PublicId == m_publicId);
            Assert.IsNotNull(resourceWithMetadata);

            if (shouldHaveMetadata)
            {
                Assert.IsNotNull(resourceWithMetadata.MetadataFields);
            }
            else
            {
                Assert.IsNull(resourceWithMetadata.MetadataFields);
            }
        }
    }
}
