using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CloudinaryDotNet.Actions;
using NUnit.Framework;

namespace CloudinaryDotNet.IntegrationTests.AdminApi
{
    public class ReorderMetadataTest : IntegrationTestBase
    {
        private List<string> m_metaDataFields;

        [OneTimeSetUp]
        public override void Initialize()
        {
            base.Initialize();

            m_metaDataFields = Enumerable.Range(0, 3)
                .Select(i => $"metadata_field_reoder_test_{m_uniqueTestId}_{i}")
                .ToList();

            m_metaDataFields.ForEach(f => CreateMetadataFieldForTest<StringMetadataFieldCreateParams, string>(f));
        }

        [OneTimeTearDown]
        public override void Cleanup()
        {
            m_metaDataFields.ForEach(p => m_cloudinary.DeleteMetadataField(p));
            base.Cleanup();
        }

        [Test, RetryWithDelay]
        public async Task TestReorderMetadataFieldsLabelAsc()
        {
            var r = await m_cloudinary.ReorderMetadataFieldsAsync(MetadataReorderBy.Label, MetadataReorderDirection.Asc);
            CollectionAssert.IsOrdered(r.MetadataFields.Select(_ => _.Label));
        }

        [Test, RetryWithDelay]
        public async Task TestReorderMetadataFieldsLabelDesc()
        {
            var r = await m_cloudinary.ReorderMetadataFieldsAsync(MetadataReorderBy.Label, MetadataReorderDirection.Desc);
            CollectionAssert.IsOrdered(r.MetadataFields.Select(_ => _.Label).Reverse());
        }

        [Test, RetryWithDelay]
        public async Task TestReorderMetadataFieldsExternalIdDesc()
        {
            var r = await m_cloudinary.ReorderMetadataFieldsAsync(MetadataReorderBy.ExternalId, MetadataReorderDirection.Desc);
            CollectionAssert.IsOrdered(r.MetadataFields.Select(_ => _.ExternalId).Reverse());
        }

        [Test, RetryWithDelay]
        public async Task TestReorderMetadataFieldsExternalIdNoDirection()
        {
            var r = await m_cloudinary.ReorderMetadataFieldsAsync(MetadataReorderBy.ExternalId);
            CollectionAssert.IsOrdered(r.MetadataFields.Select(_ => _.ExternalId));
        }

        [Test, RetryWithDelay]
        public void TestReorderMetadataFieldsExternalIdAscSync()
        {
            var r = m_cloudinary.ReorderMetadataFields(MetadataReorderBy.ExternalId, MetadataReorderDirection.Asc);
            CollectionAssert.IsOrdered(r.MetadataFields.Select(_ => _.Label));
        }
    }
}

