using System;
using System.Collections.Generic;
using System.Text;
using CloudinaryDotNet.Actions;
using NUnit.Framework;
using SystemHttp = System.Net.Http;

namespace CloudinaryDotNet.Tests.AdminApi
{
    [TestFixture]
    public class RelatedAssetsTest
    {
        private const string MockedResponse = @"
            {
              ""failed"": [
                    {
                        ""message"": ""resource does not exist"",
                        ""code"": ""non_existing_ids"",
                        ""asset"": ""image/upload/test_id2"",
                        ""status"": 404
                    }
                ],
                ""success"": [
                    {
                        ""message"": ""success"",
                        ""code"": ""success_ids"",
                        ""asset"": ""raw/upload/test_id3"",
                        ""status"": 200
                    }
                ]
            }";
        private readonly string _mockedAssetResponse = new StringBuilder(MockedResponse)
            .Replace(TestIds[0], TestAssetIds[0])
            .Replace(TestIds[1], TestAssetIds[1])
            .ToString();

        private static readonly List<string> TestIds = new List<string>
        {
            "image/upload/" + TestConstants.TestPublicId2,
            "raw/upload/" + TestConstants.TestPublicId3
        };

        private static readonly List<string> TestAssetIds = new List<string>()
        {
            TestConstants.TestAssetId2,
            TestConstants.TestAssetId3
        };

        [Test]
        public void TestAddRelatedResources()
        {
            var cloudinary = new MockedCloudinary(MockedResponse);

            var result = cloudinary.AddRelatedResources( new AddRelatedResourcesParams()
            {
                PublicId = TestConstants.TestPublicId,
                AssetsToRelate = TestIds
            });

            cloudinary.AssertHttpCall(SystemHttp.HttpMethod.Post, "resources/related_assets/image/upload/" + TestConstants.TestPublicId);

            Assert.AreEqual(TestIds, cloudinary.RequestJson()["assets_to_relate"]?.Values<string>());

            Assert.NotNull(result);

            Assert.Positive(result.Success.Count);
            Assert.AreEqual("success", result.Success[0].Message);
            Assert.AreEqual("success_ids", result.Success[0].Code);
            Assert.AreEqual(TestIds?[1], result.Success[0].Asset);
            Assert.AreEqual(200, result.Success[0].Status);

            Assert.Positive(result.Failed.Count);
            Assert.AreEqual("resource does not exist", result.Failed[0].Message);
            Assert.AreEqual("non_existing_ids", result.Failed[0].Code);
            Assert.AreEqual(TestIds?[0], result.Failed[0].Asset);
            Assert.AreEqual(404, result.Failed[0].Status);
        }

        [Test]
        public void TestAddRelatedResourcesByAssetIds()
        {
            var cloudinary = new MockedCloudinary(_mockedAssetResponse);

            var result = cloudinary.AddRelatedResourcesByAssetIds( new AddRelatedResourcesByAssetIdsParams()
            {
                AssetId = TestConstants.TestAssetId,
                AssetsToRelate = TestAssetIds
            });

            cloudinary.AssertHttpCall(SystemHttp.HttpMethod.Post, "resources/related_assets/" + TestConstants.TestAssetId);

            Assert.AreEqual(TestAssetIds, cloudinary.RequestJson()["assets_to_relate"]?.Values<string>());

            Assert.NotNull(result);

            Assert.Positive(result.Success.Count);
            Assert.AreEqual("success", result.Success[0].Message);
            Assert.AreEqual("success_ids", result.Success[0].Code);
            Assert.AreEqual(TestAssetIds?[1], result.Success[0].Asset);
            Assert.AreEqual(200, result.Success[0].Status);

            Assert.Positive(result.Failed.Count);
            Assert.AreEqual("resource does not exist", result.Failed[0].Message);
            Assert.AreEqual("non_existing_ids", result.Failed[0].Code);
            Assert.AreEqual(TestAssetIds?[0], result.Failed[0].Asset);
            Assert.AreEqual(404, result.Failed[0].Status);
        }

        [Test]
        public void TestDeleteRelatedResources()
        {
            var cloudinary = new MockedCloudinary(MockedResponse);

            var result = cloudinary.DeleteRelatedResources( new DeleteRelatedResourcesParams()
            {
                PublicId = TestConstants.TestPublicId,
                AssetsToUnrelate = TestIds
            });

            cloudinary.AssertHttpCall(
                SystemHttp.HttpMethod.Delete,
                "resources/related_assets/image/upload/" + TestConstants.TestPublicId,
                 $"?assets_to_unrelate[]={Uri.EscapeDataString(TestIds[0])}" +
                 $"&assets_to_unrelate[]={Uri.EscapeDataString(TestIds[1])}"
                );

            Assert.NotNull(result);

            Assert.Positive(result.Success.Count);
            Assert.AreEqual("success", result.Success[0].Message);
            Assert.AreEqual("success_ids", result.Success[0].Code);
            Assert.AreEqual(TestIds[1], result.Success[0].Asset);
            Assert.AreEqual(200, result.Success[0].Status);

            Assert.Positive(result.Failed.Count);
            Assert.AreEqual("resource does not exist", result.Failed[0].Message);
            Assert.AreEqual("non_existing_ids", result.Failed[0].Code);
            Assert.AreEqual(TestIds[0], result.Failed[0].Asset);
            Assert.AreEqual(404, result.Failed[0].Status);
        }

        [Test]
        public void TestDeleteRelatedResourcesByAssetIds()
        {
            var cloudinary = new MockedCloudinary(_mockedAssetResponse);

            var result = cloudinary.DeleteRelatedResourcesByAssetIds( new DeleteRelatedResourcesByAssetIdsParams()
            {
                AssetId = TestConstants.TestAssetId,
                AssetsToUnrelate = TestAssetIds
            });

            cloudinary.AssertHttpCall(
                SystemHttp.HttpMethod.Delete,
                "resources/related_assets/" + TestConstants.TestAssetId,
                $"?assets_to_unrelate[]={TestAssetIds[0]}&assets_to_unrelate[]={TestAssetIds[1]}"
                );

            Assert.NotNull(result);

            Assert.Positive(result.Success.Count);
            Assert.AreEqual("success", result.Success[0].Message);
            Assert.AreEqual("success_ids", result.Success[0].Code);
            Assert.AreEqual(TestAssetIds[1], result.Success[0].Asset);
            Assert.AreEqual(200, result.Success[0].Status);

            Assert.Positive(result.Failed.Count);
            Assert.AreEqual("resource does not exist", result.Failed[0].Message);
            Assert.AreEqual("non_existing_ids", result.Failed[0].Code);
            Assert.AreEqual(TestAssetIds[0], result.Failed[0].Asset);
            Assert.AreEqual(404, result.Failed[0].Status);
        }
    }
}
