﻿using CloudinaryDotNet.Actions;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CloudinaryDotNet.IntegrationTest.AdminApi
{
    public class ManageStreamingProfilesTest : IntegrationTestBase
    {
        private List<string> m_streamingProfilesToClear;
        private readonly List<string> PREDEFINED_PROFILES =
            new List<string> { "4k", "full_hd", "hd", "sd", "full_hd_wifi", "full_hd_lean", "hd_lean" };

        private readonly string CREATE_STREAMING_PROFILE_SUFFIX = "create";

        private readonly Transformation PROFILE_TRANSFORMATION_1 = new Transformation()
            .Crop("limit").Width(1200).Height(1200).BitRate("5m");

        private readonly Transformation PROFILE_TRANSFORMATION_2 = new Transformation()
            .Crop("scale").Width(100).Height(200).BitRate("10m");

        private Task<StreamingProfileResult> CreateStreamingProfileWith2TransformsAsync(string name)
        {
            return m_cloudinary.CreateStreamingProfileAsync(
                new StreamingProfileCreateParams()
                {
                    Name = name,
                    Representations = new[] { PROFILE_TRANSFORMATION_1, PROFILE_TRANSFORMATION_2 }
                        .Select(t => new Representation { Transformation = t })
                        .ToList()
                });
        }

        private StreamingProfileResult CreateStreamingProfileWith2Transforms(string name)
        {
            return m_cloudinary.CreateStreamingProfile(
                new StreamingProfileCreateParams()
                {
                    Name = name,
                    Representations = new List<Representation>
                    {
                        new Representation { Transformation = PROFILE_TRANSFORMATION_1},
                        new Representation { Transformation = PROFILE_TRANSFORMATION_2}
                    }
                });
        }

        [OneTimeSetUp]
        public override void Initialize()
        {
            base.Initialize();
            m_streamingProfilesToClear = new List<string>();
        }

        [OneTimeTearDown]
        public override void Cleanup()
        {
            base.Cleanup();
            m_streamingProfilesToClear.ForEach(p => m_cloudinary.DeleteStreamingProfile(p));
        }

        private string GetUniqueStreamingProfileName(string suffix = "")
        {
            var name = $"{m_apiTest}_streaming_profile_{m_streamingProfilesToClear.Count + 1}";

            if (!string.IsNullOrEmpty(suffix))
                name = $"{name}_{suffix}";

            m_streamingProfilesToClear.Add(name);
            return name;
        }

        [Test, RetryWithDelay]
        public async Task TestCreateStreamingProfileAsync()
        {
            var name = GetUniqueStreamingProfileName(CREATE_STREAMING_PROFILE_SUFFIX);

            var result = await CreateStreamingProfileWith2TransformsAsync(name);

            AssertStreamingProfileWith2Transforms(result, name);
        }

        [Test, RetryWithDelay]
        public void TestCreateStreamingProfile()
        {
            string name = GetUniqueStreamingProfileName(CREATE_STREAMING_PROFILE_SUFFIX);

            var result = CreateStreamingProfileWith2Transforms(name);

            AssertStreamingProfileWith2Transforms(result, name);
        }

        private void AssertStreamingProfileWith2Transforms(StreamingProfileResult result, string name)
        {
            Assert.NotNull(result?.Data);
            Assert.AreEqual(name, result.Data.Name);
            Assert.AreEqual(2, result.Data.Representations.Count);
            Assert.AreEqual(PROFILE_TRANSFORMATION_1.ToString(), result.Data.Representations[0].Transformation.ToString());
            Assert.AreEqual(PROFILE_TRANSFORMATION_2.ToString(), result.Data.Representations[1].Transformation.ToString());
        }

        [Test, RetryWithDelay]
        public void TestGetStreamingProfile()
        {
            Assert.Throws<ArgumentException>(() => m_cloudinary.GetStreamingProfile(null));

            StreamingProfileResult result = m_cloudinary.GetStreamingProfile(PREDEFINED_PROFILES[0]);
            Assert.NotNull(result);
            Assert.NotNull(result.Data);
            Assert.IsTrue(result.Data.Predefined);
            Assert.AreEqual(PREDEFINED_PROFILES[0], result.Data.Name);
        }

        [Test, RetryWithDelay]
        public void TestListStreamingProfile()
        {
            StreamingProfileListResult profiles = m_cloudinary.ListStreamingProfiles();
            Assert.That(PREDEFINED_PROFILES, Is.SubsetOf(profiles.Data.Select(i => i.Name)));
        }

        [Test, RetryWithDelay]
        public void TestDeleteStreamingProfile()
        {
            string name = GetUniqueStreamingProfileName("delete");
            StreamingProfileResult result = CreateStreamingProfileWith2Transforms(name);

            Assert.NotNull(result);
            Assert.NotNull(result.Data);
            Assert.AreEqual(name, result.Data.Name);
            Assert.Throws<ArgumentException>(() => m_cloudinary.DeleteStreamingProfile(null));

            result = m_cloudinary.DeleteStreamingProfile(name);
            Assert.NotNull(result);
            Assert.AreEqual("deleted", result.Message);
        }

        [Test, RetryWithDelay]
        public void TestUpdateStreamingProfile()
        {
            string name = GetUniqueStreamingProfileName("update");
            string displayName = "The description of updated streaming profile";

            StreamingProfileResult result = CreateStreamingProfileWith2Transforms(name);

            Assert.NotNull(result);
            Assert.NotNull(result.Data);
            Assert.AreEqual(name, result.Data.Name);
            Assert.AreEqual(2, result.Data.Representations.Count);

            result = m_cloudinary.GetStreamingProfile(result.Data.Name);

            var representations = result.Data.Representations;
            Assert.AreEqual(2, representations.Count);

            representations.RemoveAt(1);
            representations[0].Transformation.Crop("limit").Width(800).Height(800).BitRate("5m");

            Assert.Throws<ArgumentException>(() =>
                m_cloudinary.UpdateStreamingProfile(null,
                    new StreamingProfileUpdateParams()
                    {
                        DisplayName = displayName,
                        Representations = representations
                    }));

            Assert.Throws<ArgumentNullException>(() => m_cloudinary.UpdateStreamingProfile(name, null));

            result = m_cloudinary.UpdateStreamingProfile(name,
                         new StreamingProfileUpdateParams()
                         {
                             DisplayName = displayName,
                             Representations = representations
                         });

            Assert.NotNull(result);
            Assert.AreEqual("updated", result.Message);
            Assert.NotNull(result.Data);
            Assert.AreEqual(displayName, result.Data.DisplayName);
            Assert.AreEqual(1, result.Data.Representations.Count);
            Assert.AreEqual(
                representations[0].Transformation.ToString(),
                result.Data.Representations[0].Transformation.ToString());
        }
    }
}
