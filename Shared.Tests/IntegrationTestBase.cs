﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using CloudinaryDotNet.Actions;
using NUnit.Framework;

namespace CloudinaryDotNet.Test
{
    [TestFixture]
    public partial class IntegrationTestBase
    {
        protected const string CONFIG_PLACE = "appsettings.json";

        protected string m_suffix;

        protected string m_testImagePath;
        protected string m_testLargeImagePath;
        protected string m_testVideoPath;
        protected string m_testPdfPath;
        protected string m_testIconPath;

        protected string m_cloudName;
        protected string m_apiKey;
        protected string m_apiSecret;
        protected string m_apiBaseAddress;

        protected static string m_test_tag = "dotnet_tag";
        protected static string m_test_prefix = "dotnet_";

        protected const string TEST_MOVIE = "movie.mp4";
        protected const string TEST_IMAGE = "TestImage.jpg";
        protected const string TEST_LARGEIMAGE = "TestLargeImage.jpg";
        protected const string TEST_PDF = "multipage.pdf";
        protected const string TEST_FAVICON = "favicon.ico";

        protected const string FILE_FORMAT_PDF = "pdf";
        protected const string FILE_FORMAT_PNG = "png";
        protected const string FILE_FORMAT_JPG = "jpg";
        protected const string FILE_FORMAT_BMP = "bmp";
        protected const string FILE_FORMAT_GIF = "gif";
        protected const string FILE_FORMAT_MP4 = "mp4";
        protected const string FILE_FORMAT_ICO = "ico";
        protected const string FILE_FORMAT_ZIP = "zip";

        protected const string STORAGE_TYPE_UPLOAD = "upload";
        protected const string STORAGE_TYPE_PRIVATE = "private";

        protected const int TEST_PDF_PAGES_COUNT = 3;
        protected const int MAX_RESULTS = 500;

        protected const string TOKEN_KEY = "00112233FF99";
        protected const string TOKEN_ALT_KEY = "CCBB2233FF00";

        protected const string TRANSFORM_W_512 = "w_512";
        protected const string TRANSFORM_A_45 = "a_45";

        protected string m_apiTest;
        protected string m_apiTest1;
        protected string m_apiTest2;

        protected static string m_folderPrefix;
        protected string m_apiTag;

        protected const string m_simpleTransformationAsString = "c_scale,w_0.5";
        protected readonly Transformation m_simpleTransformation = new Transformation().Crop("scale").Width(0.5);

        protected const int m_resizeTransformationWidth = 512;
        protected const int m_resizeTransformationHeight = 512;

        protected const string m_resizeTransformationAsString = "h_512,w_512";
        protected readonly Transformation m_resizeTransformation = new Transformation().Width(m_resizeTransformationWidth).Height(m_resizeTransformationHeight);

        protected string m_updateTransformationAsString;
        protected Transformation m_updateTransformation;

        protected Transformation m_explicitTransformation;

        protected readonly Transformation m_explodeTransformation = new Transformation().Page("all");
        protected readonly Transformation m_simpleTransformationAngle = new Transformation().Angle(45);

        protected Account m_account;
        protected Cloudinary m_cloudinary;

        protected Dictionary<StorageType, List<string>> m_publicIdsToClear;
        protected List<object> m_transformationsToClear;
        protected List<string> m_presetsToClear;

        protected enum StorageType { text, sprite, multi, facebook, upload }

        private void Initialize(Assembly assembly)
        {
            Settings settings = new Settings(Path.GetDirectoryName(assembly.Location));
            m_cloudName = settings.CloudName;
            m_apiKey = settings.ApiKey;
            m_apiSecret = settings.ApiSecret;
            m_apiBaseAddress = settings.ApiBaseAddress;

            m_account = GetAccountInstance();
            m_cloudinary = GetCloudinaryInstance(m_account);

            SaveTestResources(assembly);

            m_publicIdsToClear = new Dictionary<StorageType, List<string>>
            {
                { StorageType.multi, new List<string>() },
                { StorageType.text, new List<string>() },
                { StorageType.sprite, new List<string>() },
                { StorageType.facebook, new List<string>() },
                { StorageType.upload, new List<string>() }
            };

            m_transformationsToClear = new List<object>();
            m_presetsToClear = new List<string>();

            InitializeUniqueNames(assembly.GetName().Name);
        }

        protected void InitializeUniqueNames(string assemblyName)
        {
            string appveyorJobId = Environment.GetEnvironmentVariable("APPVEYOR_JOB_ID");
            m_suffix = assemblyName.Replace('.', '_');
            m_suffix += String.IsNullOrEmpty(appveyorJobId) ? new Random().Next(100000, 999999).ToString() : appveyorJobId;
            m_apiTest = m_test_prefix + m_suffix;
            m_apiTest1 = m_apiTest + "_1";
            m_apiTest2 = m_apiTest + "_2";
            m_folderPrefix = $"test_folder_{m_suffix}";
            m_apiTag = $"{m_test_tag}{m_suffix}_api";

            m_updateTransformationAsString = "c_scale,l_text:Arial_60:" + m_suffix + "_update,w_100";
            m_updateTransformation = new Transformation().Width(100).Crop("scale").Overlay(new TextLayer().Text(m_suffix + "_update").FontFamily("Arial").FontSize(60));
            m_explicitTransformation = new Transformation().Width(100).Crop("scale").Overlay(new TextLayer().Text(m_suffix).FontFamily("Arial").FontSize(60));

            AddCreatedTransformation(m_simpleTransformation, m_resizeTransformation, m_updateTransformation, m_updateTransformationAsString,
                m_explicitTransformation, m_explodeTransformation, m_simpleTransformationAngle);
        }

        private void SaveTestResources(Assembly assembly)
        {
            string filePrefix = Path.GetDirectoryName(assembly.Location);
            m_testVideoPath = Path.Combine(filePrefix, TEST_MOVIE);
            m_testImagePath = Path.Combine(filePrefix, TEST_IMAGE);
            m_testLargeImagePath = Path.Combine(filePrefix, TEST_LARGEIMAGE);
            m_testPdfPath = Path.Combine(filePrefix, TEST_PDF);
            m_testIconPath = Path.Combine(filePrefix, TEST_FAVICON);

            SaveEmbeddedToDisk(assembly, TEST_IMAGE, m_testImagePath);
            SaveEmbeddedToDisk(assembly, TEST_LARGEIMAGE, m_testLargeImagePath);
            SaveEmbeddedToDisk(assembly, TEST_MOVIE, m_testVideoPath);
            SaveEmbeddedToDisk(assembly, TEST_FAVICON, m_testIconPath);
            SaveEmbeddedToDisk(assembly, TEST_PDF, m_testPdfPath);
        }

        private void SaveEmbeddedToDisk(Assembly assembly, string sourcePath, string destPath)
        {
            var resName = assembly.GetManifestResourceNames().FirstOrDefault(s => s.EndsWith(sourcePath));
            if (File.Exists(destPath) || string.IsNullOrEmpty(resName))
                return;

            Stream stream = assembly.GetManifestResourceStream(resName);
            using (FileStream fileStream = new FileStream(destPath, FileMode.CreateNew))
            {
                stream.CopyTo(fileStream);
            }
        }

        /// <summary>
        /// A convenience method for uploading an image before testing
        /// </summary>
        /// <param name="id">The ID of the resource</param>
        /// <returns>The upload results</returns>
        protected ImageUploadResult UploadTestResource(String id = null)
        {
            if (String.IsNullOrEmpty(id))
            {
                id = GetUniquePublicId();
            }

            var uploadParams = new ImageUploadParams()
            {
                File = new FileDescription(m_testImagePath),
                PublicId = id,
                Tags = m_apiTag
            };
            return m_cloudinary.Upload(uploadParams);
        }

        /// <summary>
        /// A convenience method for deleting an image in the test
        /// </summary>
        /// <param name="id">The ID of the image to delete</param>
        /// <returns>The results of the deletion</returns>
        protected DelResResult DeleteTestResource(String id)
        {
            return m_cloudinary.DeleteResources(id);
        }

        /// <summary>
        /// A convenient method for initialization of new Account instance with necessary checks
        /// </summary>
        /// <returns>New Account instance</returns>
        private Account GetAccountInstance()
        {
            Account account = new Account(m_cloudName, m_apiKey, m_apiSecret);

            if (String.IsNullOrEmpty(account.Cloud))
                Console.WriteLine($"Cloud name must be specified in {CONFIG_PLACE}!");

            if (String.IsNullOrEmpty(account.ApiKey))
                Console.WriteLine($"Cloudinary API key must be specified in {CONFIG_PLACE}!");

            if (String.IsNullOrEmpty(account.ApiSecret))
                Console.WriteLine($"Cloudinary API secret must be specified in {CONFIG_PLACE}!");

            Assert.IsFalse(String.IsNullOrEmpty(account.Cloud));
            Assert.IsFalse(String.IsNullOrEmpty(account.ApiKey));
            Assert.IsFalse(String.IsNullOrEmpty(account.ApiSecret));
            return account;
        }

        /// <summary>
        /// A convenient method for initialization of new Cloudinary instance with necessary checks
        /// </summary>
        /// <param name="account">Instance of Account</param>
        /// <returns>New Cloudinary instance</returns>
        protected Cloudinary GetCloudinaryInstance(Account account)
        {
            Cloudinary cloudinary = new Cloudinary(account);
            if(!string.IsNullOrWhiteSpace(m_apiBaseAddress))
                cloudinary.Api.ApiBaseAddress = m_apiBaseAddress;
            return cloudinary;
        }

        protected long UnixTimeNow()
        {
            var timeSpan = (DateTime.UtcNow - new DateTime(1970, 1, 1, 0, 0, 0));
            return (long)timeSpan.TotalMilliseconds;
        }

        protected IEnumerable<Resource> GetAllResults(Func<String, ListResourcesResult> list)
        {
            ListResourcesResult current = list(null);
            IEnumerable<Resource> resources = current.Resources;
            for (; resources != null && current.NextCursor != null; current = list(current.NextCursor))
            {
                resources = resources.Concat(current.Resources);
            }
            return resources;
        }

        #region Unique PublicId's
        protected string GetUniquePublicId()
        {
            return GetUniquePublicId(StorageType.upload);
        }

        protected virtual string GetUniquePublicId(StorageType storageType, string suffix = "")
        {
            var publicId = $"{m_apiTest}_{m_publicIdsToClear[storageType].Count + 1}_{suffix}";
            AddCreatedPublicId(storageType, publicId);
            return publicId;
        }

        protected void AddCreatedPublicId(StorageType storageType, string publicId)
        {
            if (!string.IsNullOrEmpty(publicId))
                m_publicIdsToClear[storageType].Add(publicId);
        }
        #endregion

        #region Unique TransformationNames

        protected virtual string GetUniqueTransformationName(string suffix = "")
        {
            var transformationName = $"{m_apiTest}_transformation_{m_transformationsToClear.Count + 1}_{suffix}";
            AddCreatedTransformation(transformationName);
            return transformationName;
        }

        protected void AddCreatedTransformation(object transformation)
        {
            if (transformation != null)
                m_transformationsToClear.Add(transformation);
        }

        protected void AddCreatedTransformation(params object[] transformations)
        {
            if (transformations != null && transformations.Length > 0)
                m_transformationsToClear.AddRange(transformations);
        }
        #endregion

        #region Unique UploadPresetNames

        protected virtual string GetUniquePresetName(string suffix = "")
        {
            var presetName = $"{m_apiTest}_upload_preset_{m_presetsToClear.Count + 1}_{suffix}";
            m_presetsToClear.Add(presetName);
            return presetName;
        }

        #endregion

        private int GetUniqueNumber()
        {
            return Guid.NewGuid().GetHashCode();
        }

        [OneTimeTearDown]
        public virtual void Cleanup()
        {
            foreach (ResourceType resourceType in Enum.GetValues(typeof(ResourceType)))
            {
                m_cloudinary.DeleteResources(new DelResParams() { Tag = m_apiTag, ResourceType = resourceType });
            }

            m_cloudinary.DeleteResources(new DelResParams() { Tag = m_apiTag, ResourceType = ResourceType.Raw, Type = STORAGE_TYPE_PRIVATE });

            foreach (var prefix in new[]{ m_folderPrefix, m_apiTest })
            {
                m_cloudinary.DeleteResourcesByPrefix(prefix);
            }

            foreach (var item in m_publicIdsToClear)
            {
                if (item.Value.Count == 0)
                    continue;

                m_cloudinary.DeleteResources(new DelResParams()
                {
                    Type = item.Key.ToString(),
                    PublicIds = item.Value,
                    ResourceType = ResourceType.Image
                });
            }

            m_transformationsToClear.ForEach(t => m_cloudinary.DeleteTransform(t.ToString()));
            m_presetsToClear.ForEach(p => m_cloudinary.DeleteUploadPreset(p));
        }
    }
}
