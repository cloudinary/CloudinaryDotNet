using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Reflection;
using System.Threading.Tasks;
using CloudinaryDotNet.Actions;
using NUnit.Framework;
using NUnit.Framework.Interfaces;

#pragma warning disable 0618
namespace CloudinaryDotNet.IntegrationTests
{
    [TestFixture]
    public partial class IntegrationTestBase
    {
        protected const string CONFIG_PLACE = "appsettings.json";

        protected string m_suffix;

        protected string m_testImagePath;
        protected string m_testUnicodeImagePath;
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
        protected const string TEST_UNICODE_IMAGE_NAME = "TestüniNämeLögö";
        protected const string TEST_UNICODE_IMAGE = "TestüniNämeLögö.jpg";
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
        internal IAdminApi m_adminApi;

        protected Dictionary<StorageType, List<string>> m_publicIdsToClear;
        protected List<object> m_transformationsToClear;
        protected List<string> m_presetsToClear;
        protected List<string> FoldersToClear;
        protected List<string> m_metadataFieldsToClear;

        protected enum StorageType { text, sprite, multi, facebook, upload }

        protected string m_uniqueTestId;

        [OneTimeSetUp]
        public virtual void Initialize()
        {
            var assembly = typeof(IntegrationTestBase).GetTypeInfo().Assembly;
            Settings settings = new Settings(Path.GetDirectoryName(assembly.Location));
            m_cloudName = settings.CloudName;
            m_apiKey = settings.ApiKey;
            m_apiSecret = settings.ApiSecret;
            m_apiBaseAddress = settings.ApiBaseAddress;

            m_account = GetAccountInstance();
            m_cloudinary = GetCloudinaryInstance(m_account);
            m_adminApi = GetAdminApiInstance(m_account);

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
            FoldersToClear = new List<string>();
            m_metadataFieldsToClear = new List<string>();

            InitializeUniqueNames();
        }

        protected virtual string GetMethodTag([System.Runtime.CompilerServices.CallerMemberName]string memberName = "")
        {
            return $"{m_apiTag}_{memberName}";
        }

        internal Func<Account, IAdminApi> AdminApiFactory = (account) => new CloudinaryDotNet.AdminApi(account);

        protected void InitializeUniqueNames()
        {
            m_suffix = GetTaggedRandomValue();
            m_apiTest = m_test_prefix + m_suffix;
            m_apiTest1 = m_apiTest + "_1";
            m_apiTest2 = m_apiTest + "_2";
            m_folderPrefix = $"{m_test_prefix}test_folder_{m_suffix}";
            m_apiTag = $"{m_test_tag}{m_suffix}_api";
            m_uniqueTestId = $"{m_test_tag}_{m_suffix}";

            m_updateTransformationAsString = "c_scale,l_text:Arial_60:" + m_suffix + "_update,w_100";
            m_updateTransformation = new Transformation().Width(100).Crop("scale").Overlay(new TextLayer().Text(m_suffix + "_update").FontFamily("Arial").FontSize(60));
            m_explicitTransformation = new Transformation().Width(100).Crop("scale").FetchFormat("png").Overlay(new TextLayer().Text(m_suffix).FontFamily("Arial").FontSize(60));

            AddCreatedTransformation(m_simpleTransformation, m_resizeTransformation, m_updateTransformation, m_updateTransformationAsString,
                m_explicitTransformation, m_explodeTransformation, m_simpleTransformationAngle);
        }

        public static string GetTaggedRandomValue()
        {
            var assembly = typeof(IntegrationTestBase).Assembly;
            var result = assembly.GetName().Name.Replace('.', '_');
            var appveyorJobId = Environment.GetEnvironmentVariable("APPVEYOR_JOB_ID");
            result += $"{appveyorJobId}_{new Random().Next(100000, 999999).ToString()}";
            return result;
        }

        private void SaveTestResources(Assembly assembly)
        {
            string filePrefix = Path.GetDirectoryName(assembly.Location);
            m_testVideoPath = Path.Combine(filePrefix, TEST_MOVIE);
            m_testImagePath = Path.Combine(filePrefix, TEST_IMAGE);
            m_testUnicodeImagePath = Path.Combine(filePrefix, TEST_UNICODE_IMAGE);
            m_testLargeImagePath = Path.Combine(filePrefix, TEST_LARGEIMAGE);
            m_testPdfPath = Path.Combine(filePrefix, TEST_PDF);
            m_testIconPath = Path.Combine(filePrefix, TEST_FAVICON);

            SaveEmbeddedToDisk(assembly, TEST_IMAGE, m_testImagePath);
            SaveEmbeddedToDisk(assembly, TEST_IMAGE, m_testUnicodeImagePath);
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
        /// A convenient method for uploading an image before testing.
        /// </summary>
        /// <param name="setParamsAction">Action to set custom upload parameters.</param>
        /// <param name="storageType">The storage type of the asset.</param>
        /// <returns>The upload result.</returns>
        protected ImageUploadResult UploadTestImageResource(
            Action<ImageUploadParams> setParamsAction = null,
            StorageType storageType = StorageType.upload)
        {
            var uploadParams = new ImageUploadParams();

            setParamsAction?.Invoke(uploadParams);

            PopulateMissingRawUploadParams(uploadParams, false, storageType);

            return m_cloudinary.Upload(uploadParams);
        }

        /// <summary>
        /// A convenient method for uploading a video before testing.
        /// </summary>
        /// <param name="setParamsAction">Action to set custom upload parameters.</param>
        /// <returns>The upload result.</returns>
        protected VideoUploadResult UploadTestVideoResource(
            Action<VideoUploadParams> setParamsAction = null,
            StorageType storageType = StorageType.upload)
        {
            var uploadParams = new VideoUploadParams();

            setParamsAction?.Invoke(uploadParams);

            uploadParams.File = uploadParams.File ?? new FileDescription(m_testVideoPath);
            PopulateMissingRawUploadParams(uploadParams, false, storageType);

            return m_cloudinary.Upload(uploadParams);
        }

        /// <summary>
        /// A convenient method for uploading an image before testing asynchronously.
        /// </summary>
        /// <param name="setParamsAction">Action to set custom upload parameters.</param>
        /// <param name="storageType">The storage type of the asset.</param>
        /// <returns>The upload result.</returns>
        protected Task<ImageUploadResult> UploadTestImageResourceAsync(
            Action<ImageUploadParams> setParamsAction = null,
            StorageType storageType = StorageType.upload)
        {
            var uploadParams = new ImageUploadParams();

            setParamsAction?.Invoke(uploadParams);

            PopulateMissingRawUploadParams(uploadParams, true, storageType);

            return m_cloudinary.UploadAsync(uploadParams);
        }

        /// <summary>
        /// A convenient method for uploading a raw resource before testing.
        /// </summary>
        /// <param name="setParamsAction">Action to set custom upload parameters.</param>
        /// <param name="type">The type ("raw" or "auto", last by default).</param>
        /// <param name="storageType">The storage type of the asset.</param>
        /// <returns>The upload result.</returns>
        protected RawUploadResult UploadTestRawResource(
            Action<RawUploadParams> setParamsAction = null,
            string type = "auto",
            StorageType storageType = StorageType.upload)
        {
            var uploadParams = new RawUploadParams();

            setParamsAction?.Invoke(uploadParams);

            PopulateMissingRawUploadParams(uploadParams, false, storageType);

            return m_cloudinary.Upload(uploadParams, type);
        }

        /// <summary>
        /// A convenient method for uploading a raw resource before testing asynchronously.
        /// </summary>
        /// <param name="setParamsAction">Action to set custom upload parameters.</param>
        /// <param name="type">The type ("raw" or "auto", last by default).</param>
        /// <param name="storageType">The storage type of the asset.</param>
        /// <returns>The upload result.</returns>
        protected Task<RawUploadResult> UploadTestRawResourceAsync(
            Action<RawUploadParams> setParamsAction = null,
            string type = "auto",
            StorageType storageType = StorageType.upload)
        {
            var uploadParams = new RawUploadParams();

            setParamsAction?.Invoke(uploadParams);

            PopulateMissingRawUploadParams(uploadParams, true, storageType);

            return m_cloudinary.UploadAsync(uploadParams, type);
        }
        /// <summary>
        /// A convenient method for creating a structured metadata field before testing.
        /// </summary>
        /// <param name="fieldLabelSuffix">The distinguishable suffix.</param>
        /// <returns>The ExternalId of the structured metadata field.</returns>
        protected string CreateMetadataField(string fieldLabelSuffix)
        {
            var metadataLabel = GetUniqueMetadataFieldLabel(fieldLabelSuffix);
            var metadataParameters = new StringMetadataFieldCreateParams(metadataLabel);
            var metadataResult = m_cloudinary.AddMetadataField(metadataParameters);

            Assert.NotNull(metadataResult, metadataResult.Error?.Message);

            var metadataFieldId = metadataResult.ExternalId;
            if (!string.IsNullOrEmpty(metadataFieldId))
                m_metadataFieldsToClear.Add(metadataFieldId);

            return metadataFieldId;
        }

        private void PopulateMissingRawUploadParams(RawUploadParams uploadParams, bool isAsync, StorageType storageType = StorageType.upload)
        {
            uploadParams.File = uploadParams.File ?? new FileDescription(m_testImagePath);
            uploadParams.PublicId = uploadParams.PublicId ??
                (isAsync ? GetUniqueAsyncPublicId(storageType) : GetUniquePublicId(storageType));
            uploadParams.Tags = uploadParams.Tags ?? m_apiTag;
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
                cloudinary.SetApiBaseAddress(m_apiBaseAddress);

            return cloudinary;
        }

        /// <summary>
        /// A convenient method for initialization of new Cloudinary instance with necessary checks
        /// </summary>
        /// <param name="account">Instance of Account</param>
        /// <returns>New Cloudinary instance</returns>
        protected IAdminApi GetAdminApiInstance(Account account)
        {
            var adminApi = new CloudinaryDotNet.AdminApi(account);
            if(!string.IsNullOrWhiteSpace(m_apiBaseAddress))
                adminApi.SetApiBaseAddress(m_apiBaseAddress);

            return adminApi;
        }

        /// <summary>
        /// A convenient method for initialization of new Cloudinary instance with necessary checks
        /// </summary>
        /// <param name="adminApi">Instance of IAdminApi</param>
        /// <returns>New Cloudinary instance</returns>
        internal IAdminApi GetAdminApiInstance(IAdminApi adminApi)
        {
            if(!string.IsNullOrWhiteSpace(m_apiBaseAddress))
                adminApi.SetApiBaseAddress(m_apiBaseAddress);

            return adminApi;
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

        protected bool UrlExists(string url)
        {
            var request = WebRequest.Create(new Uri(url));
            request.Method = "HEAD";
            using (var response = (HttpWebResponse)request.GetResponseAsync().Result)
            {
                return response.StatusCode == HttpStatusCode.OK;
            }
        }

        #region Unique PublicId's

        protected virtual string GetUniquePublicId(StorageType storageType = StorageType.upload, string suffix = "")
        {
            var publicId = $"{m_apiTest}_{m_publicIdsToClear[storageType].Count + 1}_{suffix}";
            AddCreatedPublicId(storageType, publicId);
            return publicId;
        }

        protected virtual string GetUniqueAsyncPublicId(StorageType storageType = StorageType.upload)
        {
            return GetUniquePublicId(storageType, "ASYNC");
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

        #region Unique Folders

        protected string GetUniqueFolder(string suffix = "", string subFolders = "")
        {
            string[] folders = {$"{m_folderPrefix}_{FoldersToClear.Count + 1}_{suffix}", subFolders};
            var fullPath = string.Join("/", folders.Where(f=> !string.IsNullOrEmpty(f)));
            FoldersToClear.Add(fullPath);

            return fullPath;
        }

        #endregion

        protected string GetUniqueMetadataFieldLabel(string suffix = "")
        {
            var label = $"{m_apiTest}_meta_data_label_{m_metadataFieldsToClear.Count + 1}";
            if (!string.IsNullOrEmpty(suffix))
                label = $"{label}_{suffix}";
            return label;
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
            FoldersToClear.ForEach(f => m_cloudinary.DeleteFolder(f));
            m_metadataFieldsToClear.ForEach(p => m_cloudinary.DeleteMetadataField(p));
        }
    }

    public class IgnoreAddonAttribute : Attribute, ITestAction
    {
        private readonly string _mAddonName;

        public IgnoreAddonAttribute(string name)
        {
            _mAddonName = (name ?? string.Empty).ToLower();
        }

        public ActionTargets Targets { get; private set; }

        public void AfterTest(ITest test) { }

        public void BeforeTest(ITest test)
        {
            var environmentVariable = Environment.GetEnvironmentVariable("CLD_TEST_ADDONS");
            var addonsList = (environmentVariable ?? string.Empty).ToLower()
                .Split(',')
                .Select(addon => addon.Trim())
                .ToList();

            var allTestsShouldRun = addonsList.Count == 1 && addonsList[0] == "all";
            var addonNotInList = !addonsList.Contains(_mAddonName);
            var noAddonsDefined = !addonsList.Any();

            if (noAddonsDefined || addonNotInList && !allTestsShouldRun)
            {
                Assert.Ignore(
                    $"Please enable {_mAddonName} plugin in your account and set CLD_TEST_ADDONS environment variable");
            }
        }
    }

    public class IgnoreFeatureAttribute : Attribute, ITestAction
    {
        private readonly string _mFeatureName;

        public IgnoreFeatureAttribute(string name)
        {
            _mFeatureName = name;
        }

        public ActionTargets Targets { get; private set; }

        public void AfterTest(ITest test) { }

        public void BeforeTest(ITest test)
        {
            var featuresToRun = Environment.GetEnvironmentVariable("CLD_TEST_FEATURES");
            if (string.IsNullOrEmpty(featuresToRun) ||
                !featuresToRun.Contains(_mFeatureName) &&
                !featuresToRun.ToLower().Equals("all"))
            {
                Assert.Ignore(
                    $"Please enable {_mFeatureName} feature in your account and set CLD_TEST_FEATURES environment variable");
            }
        }
    }

    public static class CloudinaryAssert
    {
        public static void StringsAreEqualIgnoringCaseAndChars(string expected, string actual, string chars)
        {
            expected = expected.Replace(chars.ToCharArray(), "");
            actual = actual.Replace(chars.ToCharArray(), "");

            StringAssert.AreEqualIgnoringCase(expected, actual);
        }

        private static string Replace(this string s, char[] oldChars, string newChar)
        {
            return string.Join(newChar, s.Split(oldChars, StringSplitOptions.RemoveEmptyEntries));
        }

        public static void AccessibilityAnalysisNotEmpty(AccessibilityAnalysis accessibilityAnalysisResult)
        {
            Assert.IsNotNull(accessibilityAnalysisResult);
            Assert.GreaterOrEqual(accessibilityAnalysisResult.ColorblindAccessibilityScore, 0);

            Assert.IsNotNull(accessibilityAnalysisResult.ColorblindAccessibilityAnalysis);
            Assert.GreaterOrEqual(accessibilityAnalysisResult.ColorblindAccessibilityAnalysis.DistinctColors, 0);
            Assert.GreaterOrEqual(accessibilityAnalysisResult.ColorblindAccessibilityAnalysis.DistinctEdges, 0);
            Assert.GreaterOrEqual(accessibilityAnalysisResult.ColorblindAccessibilityAnalysis.MostIndistinctPair.Length, 0);
        }
    }

}
#pragma warning restore 0618
