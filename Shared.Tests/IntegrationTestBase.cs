using System;
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

        protected static string m_test_tag = "net_tag";
        protected static string m_test_prefix = "net_";

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

        protected const string m_simpleTransformationName = "c_scale,w_2.0";
        protected readonly Transformation m_simpleTransformation = new Transformation().Crop("scale").Width(2.0);
        protected const string m_resizeTransformationName = "w_512,h_512";
        protected readonly Transformation m_resizeTransformation = new Transformation().Width(512).Height(512);
        protected string m_updateTransformationName;
        protected Transformation m_updateTransformation;
        protected Transformation m_explicitTransformation;
        protected readonly Transformation m_explodeTransformation = new Transformation().Page("all");
        protected readonly Transformation m_simpleTransformationAngle = new Transformation().Angle(45);

        protected Account m_account;
        protected Cloudinary m_cloudinary;

        protected Dictionary<ResourceParameterType, List<string>> m_publicIdsToClear;

        protected enum ResourceParameterType { text, sprite, multi, facebook, upload}

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

            InitializeUniqueNames(assembly.GetName().Name);

            m_publicIdsToClear = new Dictionary<ResourceParameterType, List<string>>
            {
                { ResourceParameterType.multi, new List<string>() },
                { ResourceParameterType.text, new List<string>() },
                { ResourceParameterType.sprite, new List<string>() },
                { ResourceParameterType.facebook, new List<string>() },
                { ResourceParameterType.upload, new List<string>() }
            };

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
            m_updateTransformationName = "c_scale,l_text:Arial_60:" + m_suffix + "_update,w_100";
            m_updateTransformation = new Transformation().Width(100).Crop("scale").Overlay(new TextLayer().Text(m_suffix + "_update").FontFamily("Arial").FontSize(60));
            m_explicitTransformation = new Transformation().Width(100).Crop("scale").Overlay(new TextLayer().Text(m_suffix).FontFamily("Arial").FontSize(60));
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
        protected ImageUploadResult UploadTestResource(String id)
        {
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

        protected string GetUniquePublicId()
        {
            return GetUniquePublicId(ResourceParameterType.upload);
        }

        protected virtual string GetUniquePublicId(ResourceParameterType actionType, string suffix="")
        {
            var publicId = $"{m_apiTest}_{m_publicIdsToClear[actionType].Count + 1}{suffix}";

            m_publicIdsToClear[actionType].Add(publicId);

            return publicId;
        }

        protected void AddCreatedPublicId(ResourceParameterType actionType, string publicId)
        {
            if (!string.IsNullOrEmpty(publicId))
                m_publicIdsToClear[actionType].Add(publicId);
        }

        [OneTimeTearDown]
        public virtual void Cleanup()
        {
            m_cloudinary.DeleteResources(new DelResParams() { Tag = m_apiTag, ResourceType = ResourceType.Image });
            m_cloudinary.DeleteResources(new DelResParams() { Tag = m_apiTag, ResourceType = ResourceType.Raw });
            m_cloudinary.DeleteResources(new DelResParams() { Tag = m_apiTag, ResourceType = ResourceType.Video });
            m_cloudinary.DeleteResources(
                new DelResParams() { Tag = m_apiTag, ResourceType = ResourceType.Raw, Type = "private" });
            m_cloudinary.DeleteResourcesByPrefix(m_folderPrefix);
            m_cloudinary.DeleteResourcesByPrefix(m_apiTest);
            m_cloudinary.DeleteResourcesByPrefix(m_apiTag);

            foreach (var item in m_publicIdsToClear)
            {
                m_cloudinary.DeleteResources(new DelResParams()
                {
                    Type = item.Key.ToString(),
                    PublicIds = item.Value,
                    ResourceType = ResourceType.Image
                });
            }
        }
    }
}
