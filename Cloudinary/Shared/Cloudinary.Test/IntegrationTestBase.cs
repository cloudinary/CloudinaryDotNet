﻿using CloudinaryDotNet.Actions;
using Microsoft.Extensions.Configuration;
using Moq;
using NUnit.Framework;
using System;
using System.Linq;
using System.IO;
using System.Net.Http;
using System.Reflection;

namespace CloudinaryDotNet.Test
{
    [TestFixture]
    public class IntegrationTestBase
    {
        protected string m_testImagePath;
        protected string m_testLargeImagePath;
        protected string m_testVideoPath;
        protected string m_testPdfPath;
        protected string m_testIconPath;

        protected string m_cloudName;
        protected string m_apiKey;
        protected string m_apiSecret;
        protected string m_apiBaseAddress;

        protected const string TEST_TAG = "cloudinarydotnet_test";
        protected const string TEST_MOVIE = "movie.mp4";
        protected const string TEST_IMAGE = "TestImage.jpg";
        protected const string TEST_LARGEIMAGE = "TestLargeImage.jpg";
        protected const string TEST_PDF = "multipage.pdf";
        protected const string TEST_FAVICON = "favicon.ico";

        protected Account m_account;
        protected Cloudinary m_cloudinary;

        [OneTimeSetUp]
        public virtual void Initialize()
        {
            var builder = new ConfigurationBuilder();
            builder.SetBasePath(Directory.GetCurrentDirectory());
            builder.AddJsonFile("appsettings.json");
            var config = builder.Build();

            m_cloudName = config.GetSection("AccountSettings:CloudName").Value;
            m_apiKey = config.GetSection("AccountSettings:ApiKey").Value;
            m_apiSecret = config.GetSection("AccountSettings:ApiSecret").Value;
            m_apiBaseAddress = config.GetSection("AccountSettings:ApiBaseAddress").Value;

            m_testVideoPath = Path.Combine(Path.GetDirectoryName(typeof(IntegrationTestBase).GetTypeInfo().Assembly.Location), TEST_MOVIE);
            m_testImagePath = Path.Combine(Path.GetDirectoryName(typeof(IntegrationTestBase).GetTypeInfo().Assembly.Location), TEST_IMAGE);
            m_testLargeImagePath = Path.Combine(Path.GetDirectoryName(typeof(IntegrationTestBase).GetTypeInfo().Assembly.Location), TEST_LARGEIMAGE);
            m_testPdfPath = Path.Combine(Path.GetDirectoryName(typeof(IntegrationTestBase).GetTypeInfo().Assembly.Location), TEST_PDF);
            m_testIconPath = Path.Combine(Path.GetDirectoryName(typeof(IntegrationTestBase).GetTypeInfo().Assembly.Location), TEST_FAVICON);

            m_account = GetAccountInstance();
            m_cloudinary = GetCloudinaryInstance(m_account);

            SaveEmbeddedToDisk(TEST_IMAGE, m_testImagePath);
            SaveEmbeddedToDisk(TEST_LARGEIMAGE, m_testLargeImagePath);
            SaveEmbeddedToDisk(TEST_MOVIE, m_testVideoPath);
            SaveEmbeddedToDisk(TEST_FAVICON, m_testIconPath);
            SaveEmbeddedToDisk(TEST_PDF, m_testPdfPath);
        }

        private void SaveEmbeddedToDisk(string sourcePath, string destPath)
        {
            Assembly assembly = GetType().GetTypeInfo().Assembly;
            var resName = assembly.GetManifestResourceNames().FirstOrDefault(s => s.EndsWith(sourcePath));
            if (File.Exists(destPath) || string.IsNullOrEmpty(resName))
                return;

            Stream stream = assembly.GetManifestResourceStream(resName);
            using (FileStream fileStream = new FileStream(destPath, FileMode.CreateNew))
            {
                for (int i = 0; i < stream.Length; i++)
                    fileStream.WriteByte((byte)stream.ReadByte());
                fileStream.Flush();
            }

        }

        /// <summary>
        /// A convenient method for initialization of new Account instance with necessary checks
        /// </summary>
        /// <returns>New Account instance</returns>
        private Account GetAccountInstance()
        {
            Account account = new Account(m_cloudName, m_apiKey, m_apiSecret);

            if (String.IsNullOrEmpty(account.Cloud))
                Console.WriteLine("Cloud name must be specified in AccountSettings (appsettings.json)!");

            if (String.IsNullOrEmpty(account.ApiKey))
                Console.WriteLine("Cloudinary API key must be specified in AccountSettings (appsettings.json)!");

            if (String.IsNullOrEmpty(account.ApiSecret))
                Console.WriteLine("Cloudinary API secret must be specified in AccountSettings (appsettings.json)!");

            Assert.False(String.IsNullOrEmpty(account.Cloud));
            Assert.False(String.IsNullOrEmpty(account.ApiKey));
            Assert.False(String.IsNullOrEmpty(account.ApiSecret));
            return account;
        }

        /// <summary>
        /// A convenient method for initialization of new Coudinary instance with necessary checks
        /// </summary>
        /// <param name="account">Instance of Account</param>
        /// <returns>New Cloudinary instance</returns>
        public Cloudinary GetCloudinaryInstance(Account account)
        {
            Cloudinary cloudinary = new Cloudinary(account);
            if (!String.IsNullOrWhiteSpace(m_apiBaseAddress))
                cloudinary.Api.ApiBaseAddress = m_apiBaseAddress;
            return cloudinary;
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
        /// Get stream from mock request to Cloudinary API represented as String
        /// </summary>
        /// <param name="requestParams">Parameters for Cloudinary call</param>
        /// <param name="cloudinaryCall">Cloudinary call, e.g. "(cloudinaryInstance, params) => {return cloudinaryInstance.Text(params); }"</param>
        /// <returns></returns>
        protected string GetMockBodyOfCoudinaryRequest<TParams, TResult>(TParams requestParams, Func<Cloudinary, TParams, TResult> cloudinaryCall)
            where TParams : BaseParams
            where TResult : BaseResult
        {
            #region Mock infrastructure
            var mock = new Mock<HttpRequestMessage>();

            mock.CallBase = true;

            HttpRequestMessage request = null;
            Func<HttpRequestMessage> requestBuilder = () =>
            {
                request = mock.Object;
                return request;
            };
            #endregion

            Cloudinary fakeCloudinary = GetCloudinaryInstance(m_account);
            fakeCloudinary.Api.RequestBuilder = requestBuilder;

            try
            {
                cloudinaryCall(fakeCloudinary, requestParams);
            }
            // consciously return null in GetResponse() and extinguish the ArgumentNullException while parsing response, 'cause it's not in focus of current test
            catch (ArgumentNullException) { }

            MemoryStream stream = request.Content.ReadAsStreamAsync().Result as MemoryStream;
            request.Content.Dispose();
            request.Dispose();
            return System.Text.Encoding.UTF8.GetString(stream.ToArray());
        }

        protected long UnixTimeNow()
        {
            var timeSpan = (DateTime.UtcNow - new DateTime(1970, 1, 1, 0, 0, 0));
            return (long)timeSpan.TotalSeconds;
        }
    }
}
