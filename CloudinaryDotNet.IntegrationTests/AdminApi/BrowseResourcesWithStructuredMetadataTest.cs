using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using CloudinaryDotNet.Actions;
using NUnit.Framework;

namespace CloudinaryDotNet.IntegrationTests.AdminApi
{
    public class BrowseResourcesWithStructuredMetadataTest : IntegrationTestBase
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
            var @params = new ListResourcesParams
            {
            };
            await AssertMetadataFlagBehavior(@params);
        }

        [Test, RetryWithDelay]
        public async Task TestListResourcesByTag()
        {
            var @params = new ListResourcesByTagParams
            {
                Tag = m_apiTag,
            };
            await AssertMetadataFlagBehavior(@params);
        }

        [Test, RetryWithDelay]
        public async Task TestListResourcesByModerationStatus()
        {
            var @params = new ListResourcesByModerationParams
            {
                ModerationStatus = ModerationStatus.Pending,
                ModerationKind = MODERATION_MANUAL,
            };
            await AssertMetadataFlagBehavior(@params);
        }

        [Test, RetryWithDelay]
        public async Task TestListResourcesByPrefix()
        {
            var @params = new ListResourcesByPrefixParams
            {
                Type = "upload",
                Prefix = m_test_prefix,
            };
            await AssertMetadataFlagBehavior(@params);
        }

        private async Task AssertMetadataFlagBehavior(ListResourcesParams @params)
        {
            @params.Metadata = true;
            CommonAssertions(await m_cloudinary.ListResourcesAsync(@params), true);

            @params.Metadata = false;
            CommonAssertions(await m_cloudinary.ListResourcesAsync(@params), false);
        }

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
