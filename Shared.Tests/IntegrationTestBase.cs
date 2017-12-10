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
        protected const string m_config_place = "appsettings.json";

        protected string m_testImagePath;
        protected string m_testLargeImagePath;
        protected string m_testVideoPath;
        protected string m_testPdfPath;
        protected string m_testIconPath;
        protected string m_appveyor_job_id;
        protected string m_suffix;

        protected string m_cloudName;
        protected string m_apiKey;
        protected string m_apiSecret;
        protected string m_apiBaseAddress;

        protected string m_test_tag = "cloudinarydotnet_test";

        protected const string TEST_MOVIE = "movie.mp4";
        protected const string TEST_IMAGE = "TestImage.jpg";
        protected const string TEST_LARGEIMAGE = "TestLargeImage.jpg";
        protected const string TEST_PDF = "multipage.pdf";
        protected const string TEST_FAVICON = "favicon.ico";

        protected const string TOKEN_KEY = "00112233FF99";
        protected const string TOKEN_ALT_KEY = "CCBB2233FF00";

        protected Account m_account;
        protected Cloudinary m_cloudinary;

        private void Initialize(Assembly assembly)
        {
            Settings settings = new Settings(Path.GetDirectoryName(assembly.Location));
            m_cloudName = settings.CloudName;
            m_apiKey = settings.ApiKey;
            m_apiSecret = settings.ApiSecret;
            m_apiBaseAddress = settings.ApiBaseAddress;

            m_appveyor_job_id = Environment.GetEnvironmentVariable("APPVEYOR_JOB_ID");
            m_suffix = String.IsNullOrEmpty(m_appveyor_job_id) ? new Random().Next(100000, 999999).ToString() : m_appveyor_job_id;
            m_test_tag += m_suffix;

            m_account = GetAccountInstance();
            m_cloudinary = GetCloudinaryInstance(m_account);

            SaveTestResources(assembly);
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
                Tags = "test"
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
                Console.WriteLine($"Cloud name must be specified in {m_config_place}!");

            if (String.IsNullOrEmpty(account.ApiKey))
                Console.WriteLine($"Cloudinary API key must be specified in {m_config_place}!");

            if (String.IsNullOrEmpty(account.ApiSecret))
                Console.WriteLine($"Cloudinary API secret must be specified in {m_config_place}!");

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
            return (long)timeSpan.TotalSeconds;
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

        [OneTimeTearDown]
        public void Cleanup()
        {
            string publicId = string.Format("TestForTagSearch_{0}", m_suffix);
            DelResResult delResult = m_cloudinary.DeleteResources(new string[] { publicId });
            publicId = string.Concat(m_suffix, "_TestForTagSearch");
            delResult = m_cloudinary.DeleteResources(new string[] { publicId });
            publicId = string.Concat(m_suffix, "_TestForSearch");
            delResult = m_cloudinary.DeleteResources(new string[] { publicId });
            m_cloudinary.DeleteResourcesByTag(m_test_tag);
        }
    }
}
