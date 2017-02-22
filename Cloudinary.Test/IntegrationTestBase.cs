using Cloudinary.Test.Properties;
using CloudinaryDotNet.Actions;
using Moq;
using NUnit.Framework;
using System;
using System.IO;
using System.Linq;
using System.Net;
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

        protected const string TEST_TAG = "cloudinarydotnet_test";

        protected Account m_account;
        protected Cloudinary m_cloudinary;

        [TestFixtureSetUp]
        public virtual void Initialize()
        {
            m_account = GetAccountInstance();
            m_cloudinary = GetCloudinaryInstance(m_account);

            m_testVideoPath = Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), "movie.mp4");
            m_testImagePath = Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), "TestImage.jpg");
            m_testLargeImagePath = Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), "TestLargeImage.jpg");
            m_testPdfPath = Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), "multipage.pdf");
            m_testIconPath = Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), "favicon.ico");

            Resources.TestImage.Save(m_testImagePath);
            Resources.TestLargeImage.Save(m_testLargeImagePath);
            File.WriteAllBytes(m_testPdfPath, Resources.multipage);
            File.WriteAllBytes(m_testVideoPath, Resources.movie);

            using (Stream s = new FileStream(m_testIconPath, FileMode.Create, FileAccess.Write, FileShare.None))
            {
                Resources.favicon.Save(s);
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
        protected void DeleteTestResource(String id)
        {
            DelResResult delResult = m_cloudinary.DeleteResources(id);
            Assert.AreEqual(delResult.StatusCode, HttpStatusCode.OK);
            Assert.IsNull(delResult.Error);
            Assert.AreEqual("deleted", delResult.Deleted[id]);
        }
        protected void DeleteTestResource(ResourceType resourceType, String id)
        {
            DeleteTestResource(resourceType, false, id);
        }
        protected void DeleteTestResource(ResourceType resourceType, bool isPrivate, String id)
        {
            DelResResult delResult = m_cloudinary.DeleteResources(resourceType, isPrivate, id);
            Assert.AreEqual(delResult.StatusCode, HttpStatusCode.OK);
            Assert.IsNull(delResult.Error);
            Assert.AreEqual("deleted", delResult.Deleted[id]);
        }
        protected void DeleteTestResource(SpecialImageType specialImageType, String id)
        {
            DelResResult delResult = m_cloudinary.DeleteResources(specialImageType, id);
            Assert.AreEqual(delResult.StatusCode, HttpStatusCode.OK);
            Assert.IsNull(delResult.Error);
            Assert.AreEqual("deleted", delResult.Deleted[id]);
        }


        /// <summary>
        /// A convenient method for initialization of new Account instance with necessary checks
        /// </summary>
        /// <returns>New Account instance</returns>
        private Account GetAccountInstance()
        {
            Account account = new Account(
                Settings.Default.CloudName,
                Settings.Default.ApiKey,
                Settings.Default.ApiSecret);

            if (String.IsNullOrEmpty(account.Cloud))
                Console.WriteLine("Cloud name must be specified in test configuration (app.config)!");

            if (String.IsNullOrEmpty(account.ApiKey))
                Console.WriteLine("Cloudinary API key must be specified in test configuration (app.config)!");

            if (String.IsNullOrEmpty(account.ApiSecret))
                Console.WriteLine("Cloudinary API secret must be specified in test configuration (app.config)!");

            Assert.IsFalse(String.IsNullOrEmpty(account.Cloud));
            Assert.IsFalse(String.IsNullOrEmpty(account.ApiKey));
            Assert.IsFalse(String.IsNullOrEmpty(account.ApiSecret));
            return account;
        }

        /// <summary>
        /// A convenient method for initialization of new Coudinary instance with necessary checks
        /// </summary>
        /// <param name="account">Instance of Account</param>
        /// <returns>New Cloudinary instance</returns>
        protected Cloudinary GetCloudinaryInstance(Account account)
        {
            Cloudinary cloudinary = new Cloudinary(account);
            if (!String.IsNullOrWhiteSpace(Settings.Default.ApiBaseAddress))
                cloudinary.Api.ApiBaseAddress = Settings.Default.ApiBaseAddress;
            return cloudinary;
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
            HttpWebRequest request = null;
            return GetMockBodyOfCoudinaryRequest(requestParams, cloudinaryCall, out request);
        }

        /// <summary>
        /// Get stream represented as String and request object from mock request to Cloudinary API
        /// </summary>
        /// <param name="requestParams">Parameters for Cloudinary call</param>
        /// <param name="cloudinaryCall">Cloudinary call, e.g. "(cloudinaryInstance, params) => {return cloudinaryInstance.Text(params); }"</param>
        /// <param name="request">HttpWebRequest object as out parameter for further analyze of properties</param>
        /// <returns></returns>
        protected string GetMockBodyOfCoudinaryRequest<TParams, TResult>(TParams requestParams, Func<Cloudinary, TParams, TResult> cloudinaryCall, out HttpWebRequest request)
            where TParams : BaseParams
            where TResult : BaseResult
        {
            #region Mock infrastructure
            var mock = new Mock<HttpWebRequest>();

            mock.Setup(x => x.GetRequestStream()).Returns(new MemoryStream());
            mock.Setup(x => x.GetResponse()).Returns((WebResponse)null);
            mock.CallBase = true;

            HttpWebRequest localRequest = null;
            Func<string, HttpWebRequest> requestBuilder = (x) =>
            {
                localRequest = mock.Object;
                localRequest.Headers = new WebHeaderCollection();
                return localRequest;
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

            MemoryStream stream = localRequest.GetRequestStream() as MemoryStream;
            request = localRequest;
            return System.Text.Encoding.Default.GetString(stream.ToArray());
        }

        protected long UnixTimeNow()
        {
            var timeSpan = (DateTime.UtcNow - new DateTime(1970, 1, 1, 0, 0, 0));
            return (long)timeSpan.TotalSeconds;
        }
    }
}
