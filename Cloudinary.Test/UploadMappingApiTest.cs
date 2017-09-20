using CloudinaryDotNet.Actions;
using NUnit.Framework;
using System;

namespace CloudinaryDotNet.Test
{
    public class UploadMappingTest : IntegrationTestBase
    {
        string[] FOLDERS = { "api_test_upload_mapping_000", "api_test_upload_mapping_001", "api_test_upload_mapping_002"};
        const string TEMPLATE = "http://upload.wikimedia.org/wikipedia";
        const string NEW_TEMPLATE = "http://res.cloudinary.com";

        public override void Initialize()
        {
            base.Initialize();

            try
            {
                m_cloudinary.DeleteUploadMapping(FOLDERS[0]);
                m_cloudinary.DeleteUploadMapping(FOLDERS[1]);
                m_cloudinary.DeleteUploadMapping(FOLDERS[2]);
            }
            catch (Exception) { }
        }

        [Test]
        public void TestUploadMapping()
        {
            UploadMappingResults result;
            result = m_cloudinary.CreateUploadMapping(FOLDERS[0], TEMPLATE);
            StringAssert.AreEqualIgnoringCase("created", result.Message);

            result = m_cloudinary.UploadMapping(FOLDERS[0]);
            Assert.AreEqual(1, result.Mappings.Count);
            Assert.AreEqual(TEMPLATE, result.Mappings[FOLDERS[0]]);

            result = m_cloudinary.UpdateUploadMapping(FOLDERS[0], NEW_TEMPLATE);
            StringAssert.AreEqualIgnoringCase("updated", result.Message);

            result = m_cloudinary.UploadMapping(FOLDERS[0]);
            Assert.AreEqual(1, result.Mappings.Count);
            Assert.AreEqual(NEW_TEMPLATE, result.Mappings[FOLDERS[0]]);

            result = m_cloudinary.UploadMappings(new UploadMappingParams());
            Assert.IsTrue(result.Mappings.ContainsKey(FOLDERS[0]));
            Assert.IsTrue(result.Mappings.ContainsValue(NEW_TEMPLATE));

            result = m_cloudinary.DeleteUploadMapping(FOLDERS[0]);
            StringAssert.AreEqualIgnoringCase("deleted", result.Message);
        }

        [Test]
        public void TestUploadMappingNextCursor()
        {
            UploadMappingResults result;
            string templateSuffix = "_test";

            result = m_cloudinary.CreateUploadMapping(FOLDERS[1], TEMPLATE + templateSuffix);
            StringAssert.AreEqualIgnoringCase("created", result.Message);
            result = m_cloudinary.CreateUploadMapping(FOLDERS[2], TEMPLATE + templateSuffix);
            StringAssert.AreEqualIgnoringCase("created", result.Message);

            var uploadMappingParams = new UploadMappingParams()
            {
                MaxResults = 1,
                Template = templateSuffix
            };

            //get the first upload mapping of two created with given template
            UploadMappingResults results1 = m_cloudinary.UploadMappings(uploadMappingParams);
            Assert.IsNotNull(results1.NextCursor);
            Assert.IsNull(results1.Error);
            Assert.AreEqual(1, results1.Mappings.Count);

            //get the second upload mapping of two created with given template
            uploadMappingParams.NextCursor = results1.NextCursor;
            UploadMappingResults results2 = m_cloudinary.UploadMappings(uploadMappingParams);
            Assert.IsNull(results2.Error);
            Assert.AreEqual(1, results2.Mappings.Count);
        }
    }
}
