using CloudinaryDotNet.Actions;
using NUnit.Framework;

namespace CloudinaryDotNet.Tests.AdminApi
{
    /// <summary>
    /// Verifies the wrap-around sentinel is handled across real result types, end to end
    /// through the deserialization pipeline rather than against the converter in isolation.
    /// </summary>
    [TestFixture]
    public class SafeIntConverterCoverageTest
    {
        private const string Wrapped = "4294967295";

        // ---------------------------------------------------------------
        // The originally reported failure
        // ---------------------------------------------------------------
        [Test]
        public void TestReportedPagesFailureNoLongerThrows()
        {
            // Reproduces the payload from the incident: a document whose page count
            // arrived as an unsigned -1 and discarded the entire response.
            var json = @"{
              ""asset_id"": ""8abd06560fc75b3bbe80299b988035b0"",
              ""public_id"": ""sample_document"",
              ""format"": ""pdf"",
              ""resource_type"": ""image"",
              ""type"": ""upload"",
              ""bytes"": 582249,
              ""width"": 1000,
              ""height"": 688,
              ""pages"": " + Wrapped + @"
            }";

            var result = new MockedCloudinary(json).GetResource("sample_document");

            Assert.NotNull(result);
            Assert.AreEqual(SafeIntConverter.UnknownValue, result.Pages);

            // Everything alongside the bad field must survive.
            Assert.AreEqual("sample_document", result.PublicId);
            Assert.AreEqual("pdf", result.Format);
            Assert.AreEqual(1000, result.Width);
            Assert.AreEqual(688, result.Height);
            Assert.AreEqual(582249, result.Bytes);
        }

        // ---------------------------------------------------------------
        // GetResourceResult
        // ---------------------------------------------------------------
        [Test]
        public void TestGetResourceIntFieldsAreCovered()
        {
            var json = @"{
              ""public_id"": ""sample"",
              ""width"": " + Wrapped + @",
              ""height"": " + Wrapped + @",
              ""pages"": 3
            }";

            var result = new MockedCloudinary(json).GetResource("sample");

            Assert.AreEqual(SafeIntConverter.UnknownValue, result.Width);
            Assert.AreEqual(SafeIntConverter.UnknownValue, result.Height);
            Assert.AreEqual(3, result.Pages);
        }

        [Test]
        public void TestGetResourceLongFieldIsCovered()
        {
            var json = @"{ ""public_id"": ""sample"", ""bytes"": " + Wrapped + @" }";

            var result = new MockedCloudinary(json).GetResource("sample");

            Assert.AreEqual(SafeIntConverter.UnknownValue, result.Bytes);
        }

        [Test]
        public void TestGetResourceLongFieldRetainsLargeValues()
        {
            // A genuine 8 GB asset must survive: only the sentinel is meaningless.
            var json = @"{ ""public_id"": ""sample"", ""bytes"": 8589934592 }";

            var result = new MockedCloudinary(json).GetResource("sample");

            Assert.AreEqual(8589934592L, result.Bytes);
        }

        [Test]
        public void TestGetResourceOrdinaryValuesAreUnaffected()
        {
            var json = @"{
              ""public_id"": ""sample"",
              ""width"": 1920,
              ""height"": 1080,
              ""pages"": 7,
              ""bytes"": 2048
            }";

            var result = new MockedCloudinary(json).GetResource("sample");

            Assert.AreEqual(1920, result.Width);
            Assert.AreEqual(1080, result.Height);
            Assert.AreEqual(7, result.Pages);
            Assert.AreEqual(2048, result.Bytes);
        }

        // ---------------------------------------------------------------
        // Nested and collection result types
        // ---------------------------------------------------------------
        [Test]
        public void TestNestedResourceIntsAreCovered()
        {
            var json = @"{
              ""resources"": [
                { ""public_id"": ""a"", ""width"": " + Wrapped + @", ""height"": 200 }
              ]
            }";

            var result = new MockedCloudinary(json).ListResources();

            Assert.IsNotEmpty(result.Resources);
            Assert.AreEqual(SafeIntConverter.UnknownValue, result.Resources[0].Width);
            Assert.AreEqual(200, result.Resources[0].Height);
        }

        [Test]
        public void TestOneBadResourceDoesNotDiscardTheList()
        {
            var json = @"{
              ""resources"": [
                { ""public_id"": ""good"", ""width"": 100, ""height"": 100 },
                { ""public_id"": ""bad"", ""width"": " + Wrapped + @", ""height"": 100 },
                { ""public_id"": ""also_good"", ""width"": 300, ""height"": 300 }
              ]
            }";

            var result = new MockedCloudinary(json).ListResources();

            Assert.AreEqual(3, result.Resources.Length);
            Assert.AreEqual(100, result.Resources[0].Width);
            Assert.AreEqual(SafeIntConverter.UnknownValue, result.Resources[1].Width);
            Assert.AreEqual(300, result.Resources[2].Width);
        }

        [Test]
        public void TestSearchResourceIntsAreCovered()
        {
            var json = @"{
              ""total_count"": 1,
              ""resources"": [
                { ""public_id"": ""a"", ""pixels"": " + Wrapped + @", ""pages"": " + Wrapped + @" }
              ]
            }";

            var result = new MockedCloudinary(json).Search().Execute();

            Assert.IsNotEmpty(result.Resources);
            Assert.AreEqual(SafeIntConverter.UnknownValue, result.Resources[0].Pixels);
            Assert.AreEqual(SafeIntConverter.UnknownValue, result.Resources[0].Pages);
        }

        [Test]
        public void TestSearchTotalCountIsCovered()
        {
            var json = @"{ ""total_count"": " + Wrapped + @", ""resources"": [] }";

            var result = new MockedCloudinary(json).Search().Execute();

            Assert.AreEqual(SafeIntConverter.UnknownValue, result.TotalCount);
        }

        // ---------------------------------------------------------------
        // Upload results
        // ---------------------------------------------------------------
        [Test]
        public void TestImageUploadPagesIsCovered()
        {
            var json = @"{
              ""public_id"": ""uploaded"",
              ""width"": 800,
              ""height"": 600,
              ""pages"": " + Wrapped + @"
            }";

            var result = new MockedCloudinary(json).Upload(new ImageUploadParams
            {
                File = new FileDescription("http://example.com/sample.pdf"),
            });

            Assert.AreEqual(SafeIntConverter.UnknownValue, result.Pages);
            Assert.AreEqual(800, result.Width);
            Assert.AreEqual(600, result.Height);
        }

        // ---------------------------------------------------------------
        // Fractional values, which the usage endpoint genuinely returns
        // ---------------------------------------------------------------
        [Test]
        public void TestFractionalUsageValueIsTruncated()
        {
            // 'credits.usage' is fractional but binds to a long; the previous
            // binder truncated it and that behaviour must be preserved.
            var json = @"{
              ""plan"": ""Basic"",
              ""credits"": { ""usage"": 1.47 },
              ""objects"": { ""usage"": 1217216 }
            }";

            var result = new MockedCloudinary(json).GetUsage();

            Assert.AreEqual(1L, result.Credits.Used);
            Assert.AreEqual(1217216L, result.Objects.Used);
        }

        [Test]
        public void TestUsageDictionaryValuesAreCovered()
        {
            var json = @"{
              ""plan"": ""Basic"",
              ""media_limits"": {
                ""image_max_size_bytes"": 157286400,
                ""video_max_size_bytes"": 3145728000,
                ""raw_max_size_bytes"": " + Wrapped + @"
              }
            }";

            var result = new MockedCloudinary(json).GetUsage();

            Assert.AreEqual(157286400L, result.MediaLimits["image_max_size_bytes"]);
            Assert.AreEqual(3145728000L, result.MediaLimits["video_max_size_bytes"]);
            Assert.AreEqual(SafeIntConverter.UnknownValue, result.MediaLimits["raw_max_size_bytes"]);
        }

        // ---------------------------------------------------------------
        // Genuinely bad data must still be reported
        // ---------------------------------------------------------------
        [Test]
        public void TestOutOfRangeIntStillFails()
        {
            var json = @"{ ""public_id"": ""sample"", ""pages"": 4294967296 }";

            Assert.That(
                () => new MockedCloudinary(json).GetResource("sample"),
                Throws.Exception.Message.Contains("Failed to deserialize response"));
        }

        [Test]
        public void TestValueBeyondLongRangeStillFails()
        {
            var json = @"{ ""public_id"": ""sample"", ""pages"": 18446744073709551615 }";

            Assert.That(
                () => new MockedCloudinary(json).GetResource("sample"),
                Throws.Exception.Message.Contains("Failed to deserialize response"));
        }
    }
}
