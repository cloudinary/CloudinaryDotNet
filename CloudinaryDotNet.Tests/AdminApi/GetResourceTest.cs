using CloudinaryDotNet.Actions;
using NUnit.Framework;

namespace CloudinaryDotNet.Tests.AdminApi
{
    [TestFixture]
    public class GetResourceTest
    {
        private const string CommonJsonPrefix = @"
        {
          ""asset_id"": ""8abd06560fc75b3bbe80299b988035b0"",
          ""public_id"": ""REPLACE_PUBLIC_ID"",
          ""format"": ""jpg"",
          ""version"": 1739308959,
          ""resource_type"": ""image"",
          ""type"": ""upload"",
          ""created_at"": ""2025-02-11T21:22:39Z"",
          ""bytes"": 582249,
          ""width"": 1000,
          ""height"": 688,
          ""url"": ""http://res.cloudinary.com/demo/image/upload/v1739308959/REPLACE_PUBLIC_ID.jpg"",
          ""secure_url"": ""https://res.cloudinary.com/demo/image/upload/v1739308959/REPLACE_PUBLIC_ID.jpg"",
          ""moderation"": [
        ";

        private const string CommonJsonSuffix = @"
          ]
        }
        ";

        // "response" is an object containing "moderation_labels"
        private const string LabelsModerationBlock = @"
            {
              ""response"": {
                ""moderation_labels"": [
                  {
                    ""confidence"": 94.9907455444336,
                    ""name"": ""Suggestive"",
                    ""parent_name"": """"
                  },
                  {
                    ""confidence"": 94.9907455444336,
                    ""name"": ""Female Swimwear Or Underwear"",
                    ""parent_name"": ""Suggestive""
                  }
                ]
              },
              ""status"": ""rejected"",
              ""kind"": ""aws_rek"",
              ""updated_at"": ""2017-08-03T08:26:58Z""
            }
        ";

        // "response" is an array of objects (duplicates)
        private const string DuplicatesModerationBlock = @"
            {
              ""kind"": ""duplicate"",
              ""status"": ""rejected"",
              ""response"": [
                {
                  ""public_id"": ""duplicate_id"",
                  ""confidence"": 1.0
                }
              ],
              ""updated_at"": ""2025-02-07T08:30:29Z""
            }
        ";

        [Test]
        public void TestGetResourceModerationResponse_WithLabels()
        {
            var responseData = CommonJsonPrefix
                .Replace("REPLACE_PUBLIC_ID", "mhor383ejnw0j6iokmlh")
                + LabelsModerationBlock
                + CommonJsonSuffix;

            var localCloudinaryMock = new MockedCloudinary(responseData);
            var result = localCloudinaryMock.GetResource("mhor383ejnw0j6iokmlh");

            Assert.NotNull(result);
            Assert.AreEqual("mhor383ejnw0j6iokmlh", result.PublicId);
            Assert.NotNull(result.Moderation);
            Assert.IsNotEmpty(result.Moderation);

            var firstModeration = result.Moderation[0];
            Assert.AreEqual("aws_rek", firstModeration.Kind);
            Assert.AreEqual(ModerationStatus.Rejected, firstModeration.Status);
            Assert.NotNull(firstModeration.Response.ModerationLabels);
            Assert.IsNotEmpty(firstModeration.Response.ModerationLabels);
            Assert.AreEqual("Suggestive", firstModeration.Response.ModerationLabels[0].Name);
            Assert.AreEqual(94.9907455444336f, firstModeration.Response.ModerationLabels[0].Confidence);
        }

        [Test]
        public void TestGetResourceModerationResponse_WithDuplicates()
        {
            var responseData = CommonJsonPrefix
                .Replace("REPLACE_PUBLIC_ID", "duplicate_id")
                + DuplicatesModerationBlock
                + CommonJsonSuffix;

            var localCloudinaryMock = new MockedCloudinary(responseData);
            var result = localCloudinaryMock.GetResource("duplicate_id");

            Assert.NotNull(result);
            Assert.AreEqual("duplicate_id", result.PublicId);
            Assert.NotNull(result.Moderation);
            Assert.IsNotEmpty(result.Moderation);

            var firstModeration = result.Moderation[0];
            Assert.AreEqual("duplicate", firstModeration.Kind);
            Assert.AreEqual(ModerationStatus.Rejected, firstModeration.Status);
            Assert.NotNull(firstModeration.Response.ModerationLabels);
            Assert.IsNotEmpty(firstModeration.Response.ModerationLabels);
            Assert.AreEqual("duplicate_id", firstModeration.Response.ModerationLabels[0].PublicId);
            Assert.AreEqual(1.0f, firstModeration.Response.ModerationLabels[0].Confidence);
        }
    }
}
