using System.Reflection;
using NUnit.Framework;

namespace CloudinaryDotNet.Test
{
    [TestFixture]
    public partial class IntegrationTestBase
    {
        [OneTimeSetUp]
        public virtual void Initialize()
        {
            Initialize(Assembly.GetExecutingAssembly());
        }

        ///// <summary>
        ///// Get stream from mock request to Cloudinary API represented as String
        ///// </summary>
        ///// <param name="requestParams">Parameters for Cloudinary call</param>
        ///// <param name="cloudinaryCall">Cloudinary call, e.g. "(cloudinaryInstance, params) => {return cloudinaryInstance.Text(params); }"</param>
        ///// <returns></returns>
        //protected string GetMockBodyOfCloudinaryRequest<TParams, TResult>(TParams requestParams, Func<Cloudinary, TParams, TResult> cloudinaryCall)
        //    where TParams : BaseParams
        //    where TResult : BaseResult
        //{
        //    HttpWebRequest request = null;
        //    return GetMockBodyOfCloudinaryRequest(requestParams, cloudinaryCall, out request);
        //}

        ///// <summary>
        ///// Get stream represented as String and request object from mock request to Cloudinary API
        ///// </summary>
        ///// <param name="requestParams">Parameters for Cloudinary call</param>
        ///// <param name="cloudinaryCall">Cloudinary call, e.g. "(cloudinaryInstance, params) => {return cloudinaryInstance.Text(params); }"</param>
        ///// <param name="request">HttpWebRequest object as out parameter for further analyze of properties</param>
        ///// <returns></returns>
        //protected string GetMockBodyOfCloudinaryRequest<TParams, TResult>(TParams requestParams, Func<Cloudinary, TParams, TResult> cloudinaryCall, out HttpWebRequest request)
        //    where TParams : BaseParams
        //    where TResult : BaseResult
        //{
        //    #region Mock infrastructure
        //    var mock = new Mock<HttpWebRequest>();

        //    mock.Setup(x => x.GetRequestStream()).Returns(new MemoryStream());
        //    mock.Setup(x => x.GetResponse()).Returns((WebResponse)null);
        //    mock.CallBase = true;

        //    HttpWebRequest localRequest = null;
        //    Func<string, HttpWebRequest> requestBuilder = (x) =>
        //    {
        //        localRequest = mock.Object;
        //        localRequest.Headers = new WebHeaderCollection();
        //        return localRequest;
        //    };
        //    #endregion

        //    Cloudinary fakeCloudinary = GetCloudinaryInstance(m_account);
        //    fakeCloudinary.Api.RequestBuilder = requestBuilder;

        //    try
        //    {
        //        cloudinaryCall(fakeCloudinary, requestParams);
        //    }
        //    // consciously return null in GetResponse() and extinguish the ArgumentNullException while parsing response, 'cause it's not in focus of current test
        //    catch (ArgumentNullException) { }

        //    MemoryStream stream = localRequest.GetRequestStream() as MemoryStream;
        //    request = localRequest;
        //    return System.Text.Encoding.Default.GetString(stream.ToArray());
        //}

    }
}
