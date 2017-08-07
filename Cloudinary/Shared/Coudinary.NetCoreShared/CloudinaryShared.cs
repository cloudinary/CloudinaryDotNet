using CloudinaryDotNet;
using CloudinaryDotNet.Actions;
using Coudinary.NetCoreShared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CloudinaryShared.Core
{
    public abstract class CloudinaryShared<TApi> where TApi: ApiShared
    {
        public const string CF_SHARED_CDN = "d3jpl91pxevbkh.cloudfront.net";
        public const string OLD_AKAMAI_SHARED_CDN = "cloudinary-a.akamaihd.net";
        public const string AKAMAI_SHARED_CDN = "res.cloudinary.com";
        public const string SHARED_CDN = AKAMAI_SHARED_CDN;
        protected const string RESOURCE_TYPE_IMAGE = "image";
        protected const string ACTION_GENERATE_ARCHIVE = "generate_archive";
        protected static Random m_random = new Random();

        protected TApi m_api;
        
        /// <summary>
        /// API object that used by this instance
        /// </summary>
        public TApi Api
        {
            get { return m_api; }
        }

        public Search Search()
        {
            return new Search(m_api);
        }

        /// <summary>
        /// Gets URL to download private image
        /// </summary>
        /// <param name="publicId">The image public ID.</param>
        /// <param name="attachment">Whether to download image as attachment (optional).</param>
        /// <param name="format">Format to download (optional).</param>
        /// <param name="type">The type (optional).</param>
        /// <returns></returns>
        /// <exception cref="System.ArgumentException">publicId can't be null</exception>
        public string DownloadPrivate(string publicId, bool? attachment = null, string format = "", string type = "")
        {
            if (String.IsNullOrEmpty(publicId))
                throw new ArgumentException("publicId");

            UrlBuilder urlBuilder = new UrlBuilder(
               m_api.ApiUrlV
               .ResourceType(RESOURCE_TYPE_IMAGE)
               .Action("download")
               .BuildUrl());

            var parameters = new SortedDictionary<string, object>();

            parameters.Add("public_id", publicId);

            if (!String.IsNullOrEmpty(format))
                parameters.Add("format", format);

            if (attachment != null)
                parameters.Add("attachment", (bool)attachment ? "true" : "false");

            if (!String.IsNullOrEmpty(type))
                parameters.Add("type", type);

            return GetDownloadUrl(urlBuilder, parameters);
        }

        /// <summary>
        /// Gets URL to download tag cloud as ZIP package
        /// </summary>
        /// <param name="tag">The tag.</param>
        /// <param name="transform">The transformation.</param>
        /// <returns></returns>
        /// <exception cref="System.ArgumentException">Tag should be specified!</exception>
        public string DownloadZip(string tag, Transformation transform)
        {
            if (String.IsNullOrEmpty(tag))
                throw new ArgumentException("Tag should be specified!");

            UrlBuilder urlBuilder = new UrlBuilder(
               m_api.ApiUrlV
               .ResourceType(RESOURCE_TYPE_IMAGE)
               .Action("download_tag.zip")
               .BuildUrl());

            var parameters = new SortedDictionary<string, object>();

            parameters.Add("tag", tag);

            if (transform != null)
                parameters.Add("transformation", transform.Generate());

            return GetDownloadUrl(urlBuilder, parameters);
        }

        private string RandomPublicId()
        {
            byte[] buffer = new byte[8];
            m_random.NextBytes(buffer);
            return string.Concat(buffer.Select(x => x.ToString("X2")).ToArray());
        }

        private string GetUploadMappingUrl()
        {
            return m_api.ApiUrlV.
                ResourceType("upload_mappings").
                BuildUrl();
        }
        
        private string GetUploadMappingUrl(UploadMappingParams parameters)
        {
            var uri = GetUploadMappingUrl();
            return new UrlBuilder(uri, parameters.ToParamsDictionary()).ToString();
        }
        
        /// <summary>
        ///  Return Url on archive file
        /// </summary>
        /// <param name="parameters">Parameters of generated archive</param>
        /// <returns>Url on archive file</returns>
        public string DownloadArchiveUrl(ArchiveParams parameters)
        {
            parameters.Mode(ArchiveCallMode.Download);

            UrlBuilder urlBuilder = new UrlBuilder(
                m_api.ApiUrlV.
                ResourceType(RESOURCE_TYPE_IMAGE).
                Action(ACTION_GENERATE_ARCHIVE).
                BuildUrl());

            return GetDownloadUrl(urlBuilder, parameters.ToParamsDictionary());
        }

        private static void AppendScriptLine(StringBuilder sb, string dir, string script)
        {
            sb.Append("<script src=\"");
            sb.Append(dir);

            if (!dir.EndsWith("/") && !dir.EndsWith("\\"))
                sb.Append("/");

            sb.Append(script);

            sb.AppendLine("\"></script>");
        }

        private string GetDownloadUrl(UrlBuilder builder, IDictionary<string, object> parameters)
        {
            m_api.FinalizeUploadParameters(parameters);
            builder.SetParameters(parameters);
            return builder.ToString();
        }

        private static void ResetInternalFileDescription(FileDescription file, int bufferSize = Int32.MaxValue)
        {
            file.BufferLength = bufferSize;
            file.EOF = false;
            file.BytesSent = 0;
        }

        /// <summary>
        /// Publish resources by prefix.
        /// </summary>
        /// <param name="prefix">The prefix for publishing resources.</param>
        /// <param name="publishResourceParams">Parameters for publishing of resources.</param>
        /// <returns></returns>
        public PublishResourceResult PublishResourceByPrefix(string prefix, PublishResourceParams publishResourceParams)
        {
            return PublishResource("prefix", prefix, publishResourceParams);
        }

        /// <summary>
        /// Publish resources by tag.
        /// </summary>
        /// <param name="prefix">The tag for publishing resources.</param>
        /// <param name="publishResourceParams">Parameters for publishing of resources.</param>
        /// <returns></returns>
        public PublishResourceResult PublishResourceByTag(string tag, PublishResourceParams publishResourceParams)
        {
            return PublishResource("tag", tag, publishResourceParams);
        }

        public PublishResourceResult PublishResourceByIds(string tag, PublishResourceParams publishResourceParams)
        {
            return PublishResource(string.Empty, string.Empty, publishResourceParams);
        }

        private PublishResourceResult PublishResource(string byKey, string value, PublishResourceParams publishResourceParams)
        {
            Url url = m_api.ApiUrlV
                .Add("resources")
                .Add(publishResourceParams.ResourceType.ToString().ToLower())
                .Add("publish_resources");

            if(!string.IsNullOrWhiteSpace(byKey) && !string.IsNullOrWhiteSpace(value))
                publishResourceParams.AddCustomParam(byKey, value);

            object response = m_api.InternalCall(HttpMethod.POST, url.BuildUrl(), publishResourceParams.ToParamsDictionary(), null);

            return PublishResourceResult.Parse(response);

        }

        private UpdateResourceAccessModeResult UpdateResourceAccessMode(string byKey, string value, UpdateResourceAccessModeParams updateResourceAccessModeParams)
        {

           Url url = m_api.ApiUrlV
                .Add(Constants.RESOURCES_API_URL)
                .Add(updateResourceAccessModeParams.ResourceType.ToString().ToLower())
                .Add(updateResourceAccessModeParams.Type)
                .Add(Constants.UPDATE_ACESS_MODE);

            if(!string.IsNullOrWhiteSpace(byKey) && !string.IsNullOrWhiteSpace(value))
                updateResourceAccessModeParams.AddCustomParam(byKey, value);

            object response = m_api.InternalCall(HttpMethod.POST, url.BuildUrl(), updateResourceAccessModeParams.ToParamsDictionary(), null);

            return UpdateResourceAccessModeResult.Parse(response);
        }
        
        public UpdateResourceAccessModeResult UpdateResourceAccessModeByTag(string tag, UpdateResourceAccessModeParams updateResourceAccessModeParams)
        {
            return UpdateResourceAccessMode(Constants.TAG_PARAM_NAME, tag, updateResourceAccessModeParams);
        }

        public UpdateResourceAccessModeResult UpdateResourceAccessModeByPrefix(string prefix, UpdateResourceAccessModeParams updateResourceAccessModeParams)
        {
            return UpdateResourceAccessMode(Constants.PREFIX_PARAM_NAME, prefix, updateResourceAccessModeParams);
        }

        public UpdateResourceAccessModeResult UpdateResourceAccessModeByIds(UpdateResourceAccessModeParams updateResourceAccessModeParams)
        {
            return UpdateResourceAccessMode(string.Empty, string.Empty, updateResourceAccessModeParams);
        }

        /// <summary>
        /// Manage tag assignments
        /// </summary>
        /// <param name="parameters">Parameters of tag management</param>
        /// <returns>Results of tags management</returns>
        public TagResult Tag(TagParams parameters)
        {
            string uri = m_api.ApiUrlImgUpV.Action(Constants.TAGS_MANGMENT).BuildUrl();

            object response = m_api.InternalCall(HttpMethod.POST, uri, parameters.ToParamsDictionary(), null);

            TagResult result = TagResult.Parse(response);
            return result;
        }

        /// <summary>
        /// Manage context assignments
        /// </summary>
        /// <param name="parameters">Parameters of context management</param>
        /// <returns>Results of contexts management</returns>
        public ContextResult Context(ContextParams parameters)
        {
            string uri = m_api.ApiUrlImgUpV.Action(Constants.CONTEXT_MANAGMENT).BuildUrl();

            object response = m_api.InternalCall(HttpMethod.POST, uri, parameters.ToParamsDictionary(), null);

            ContextResult result = ContextResult.Parse(response);
            return result;
        }

        public DelDerivedResResult DeleteDerivedResourcesByTransform(DelDerivedResParams parameters)
        {
            UrlBuilder urlBuilder = new UrlBuilder(
                m_api.ApiUrlV.
                Add("derived_resources").
                BuildUrl(),
                parameters.ToParamsDictionary());

            object response = m_api.InternalCall(HttpMethod.DELETE, urlBuilder.ToString(), null, null);

            DelDerivedResResult result = DelDerivedResResult.Parse(response);
            return result;
        }

        public AuthToken GetToken(string key)
        {
            return new AuthToken(key);
        }
    }
}
