using System;
using System.Collections;
using System.Collections.Generic;
using System.Net;
using CloudinaryDotNet.Actions;

namespace CloudinaryDotNet
{
    /// <summary>
    /// Main class of cloudinary .NET API
    /// </summary>
    public class Cloudinary
    {
        public const string CF_SHARED_CDN = "d3jpl91pxevbkh.cloudfront.net";
        public const string AKAMAI_SHARED_CDN = "cloudinary-a.akamaihd.net";
        public const string SHARED_CDN = AKAMAI_SHARED_CDN;


        Api m_api;

        /// <summary>
        /// Default parameterless constructor.
        /// Assumes that environment variable CLOUDINARY_URL is set.
        /// </summary>
        public Cloudinary()
        {
            m_api = new Api();
        }

        /// <summary>
        /// Parameterized constructor
        /// </summary>
        /// <param name="cloudinaryUrl">Cloudinary URL</param>
        public Cloudinary(string cloudinaryUrl)
        {
            m_api = new Api(cloudinaryUrl);
        }

        /// <summary>
        /// Parameterized constructor
        /// </summary>
        /// <param name="account">Cloudinary account</param>
        public Cloudinary(Account account)
        {
            m_api = new Api(account);
        }

        /// <summary>
        /// API object that used by this instance
        /// </summary>
        public Api Api
        {
            get { return m_api; }
            set { m_api = value; }
        }

        public ExplicitResult Explicit(ExplicitParams parameters)
        {
            string uri = m_api.ApiUrlImgUpV.Action("explicit").BuildUrl();

            using (HttpWebResponse response = m_api.Call(HttpMethod.POST, uri, parameters.ToParamsDictionary(), null))
            {
                return ExplicitResult.Parse(response);
            }
        }

        /// <summary>
        /// Upload an image file to cloudinary
        /// </summary>
        /// <param name="parameters">Parameters for uploading image to cloudinary</param>
        /// <returns>Results of image uploading</returns>
        public ImageUploadResult Upload(ImageUploadParams parameters)
        {
            string uri = m_api.ApiUrlImgUpV.BuildUrl();

            using (HttpWebResponse response = m_api.Call(HttpMethod.POST, uri, parameters.ToParamsDictionary(), parameters.File))
            {
                return ImageUploadResult.Parse(response);
            }
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
               .ResourceType("image")
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
        /// <returns></returns>
        /// <exception cref="System.ArgumentException">tag should be specified</exception>
        public string DownloadZip(string tag, Transformation transform)
        {
            if (String.IsNullOrEmpty(tag))
                throw new ArgumentException("tag should be specified");

            UrlBuilder urlBuilder = new UrlBuilder(
               m_api.ApiUrlV
               .ResourceType("image")
               .Action("download_tag.zip")
               .BuildUrl());

            var parameters = new SortedDictionary<string, object>();

            parameters.Add("tag", tag);

            if (transform != null)
                parameters.Add("transformation", transform.Generate());

            return GetDownloadUrl(urlBuilder, parameters);
        }

        public UsageResult GetUsage()
        {
            string uri = m_api.ApiUrlV.Action("usage").BuildUrl();

            using (HttpWebResponse response = m_api.Call(HttpMethod.GET, uri, null, null))
            {
                return UsageResult.Parse(response);
            }
        }

        /// <summary>
        /// Upload a file to cloudinary
        /// </summary>
        /// <param name="parameters">Parameters of uploading a file to cloudinary</param>
        /// <param name="type">The type ("raw" or "auto", last by default).</param>
        /// <returns>
        /// Results of file uploading
        /// </returns>
        public RawUploadResult Upload(RawUploadParams parameters, string type = "auto")
        {
            string uri = m_api.ApiUrlImgUpV.ResourceType(type).BuildUrl();

            using (HttpWebResponse response = m_api.Call(HttpMethod.POST, uri, parameters.ToParamsDictionary(), parameters.File))
            {
                return RawUploadResult.Parse(response);
            }
        }

        public RenameResult Rename(string fromPublicId, string toPublicId, bool overwrite = false)
        {
            return Rename(new RenameParams(fromPublicId, toPublicId) { Overwrite = overwrite });
        }

        public RenameResult Rename(RenameParams parameters)
        {
            string uri = m_api.ApiUrlImgUpV.Action("rename").BuildUrl();

            using (HttpWebResponse response = m_api.Call(HttpMethod.POST, uri, parameters.ToParamsDictionary(), null))
            {
                return RenameResult.Parse(response);
            }
        }

        /// <summary>
        /// Delete file from cloudinary
        /// </summary>
        /// <param name="parameters">Parameters for deletion of resource from cloudinary</param>
        /// <returns>Results of deletion</returns>
        public DeletionResult Destroy(DeletionParams parameters)
        {
            string uri = m_api.ApiUrlImgUpV.ResourceType(
                Api.GetCloudinaryParam<ResourceType>(parameters.ResourceType)).
                Action("destroy").BuildUrl();

            using (HttpWebResponse response = m_api.Call(HttpMethod.POST, uri, parameters.ToParamsDictionary(), null))
            {
                DeletionResult result = DeletionResult.Parse(response);
                return result;
            }
        }

        /// <summary>
        /// Generate an image of a given textual string
        /// </summary>
        /// <param name="text">Text to draw</param>
        /// <returns>Results of generating an image of a given textual string</returns>
        public TextResult Text(string text)
        {
            return Text(new TextParams(text));
        }

        /// <summary>
        /// Generate an image of a given textual string
        /// </summary>
        /// <param name="parameters">Parameters of generating an image of a given textual string</param>
        /// <returns>Results of generating an image of a given textual string</returns>
        public TextResult Text(TextParams parameters)
        {
            string uri = m_api.ApiUrlImgUpV.Action("text").BuildUrl();

            using (HttpWebResponse response = m_api.Call(HttpMethod.POST, uri, parameters.ToParamsDictionary(), null))
            {
                TextResult result = TextResult.Parse(response);
                return result;
            }
        }

        /// <summary>
        /// Manage tag assignments
        /// </summary>
        /// <param name="parameters">Parameters of tag management</param>
        /// <returns>Results of tags management</returns>
        public TagResult Tag(TagParams parameters)
        {
            string uri = m_api.ApiUrlImgUpV.Action("tags").BuildUrl();

            using (HttpWebResponse response = m_api.Call(HttpMethod.POST, uri, parameters.ToParamsDictionary(), null))
            {
                TagResult result = TagResult.Parse(response);
                return result;
            }
        }

        public ListResourceTypesResult ListResourceTypes()
        {
            using (HttpWebResponse response = m_api.Call(
                HttpMethod.GET, m_api.ApiUrlV.Add("resources").BuildUrl(), null, null))
            {
                ListResourceTypesResult result = ListResourceTypesResult.Parse(response);
                return result;
            }
        }

        public ListResourcesResult ListResources()
        {
            return ListResources(new ListResourcesParams());
        }

        public ListResourcesResult ListResources(string nextCursor)
        {
            return ListResources(new ListResourcesParams() { NextCursor = nextCursor });
        }

        public ListResourcesResult ListResourcesByType(string type, string nextCursor)
        {
            return ListResources(new ListResourcesParams() { Type = type, NextCursor = nextCursor });
        }

        public ListResourcesResult ListResourcesByPrefix(string type, string prefix, string nextCursor)
        {
            return ListResources(new ListResourcesParams()
            {
                Type = type,
                Prefix = prefix,
                NextCursor = nextCursor
            });
        }

        public ListResourcesResult ListResourcesByTag(string tag, string nextCursor)
        {
            return ListResources(new ListResourcesParams()
            {
                Tag = tag,
                NextCursor = nextCursor
            });
        }

        public ListResourcesResult ListResources(ListResourcesParams parameters)
        {
            UrlBuilder urlBuilder = new UrlBuilder(
                m_api.ApiUrlV.
                ResourceType("resources").
                Add(Api.GetCloudinaryParam<ResourceType>(parameters.ResourceType)).
                Add(!String.IsNullOrEmpty(parameters.Tag) ? String.Format("tags/{0}", parameters.Tag) : String.Empty).
                BuildUrl());

            foreach (var param in parameters.ToParamsDictionary())
            {
                urlBuilder.QueryString[param.Key] = param.Value.ToString();
            }

            using (HttpWebResponse response = m_api.Call(
                HttpMethod.GET, urlBuilder.ToString(), null, null))
            {
                ListResourcesResult result = ListResourcesResult.Parse(response);
                return result;
            }
        }

        public ListTagsResult ListTags()
        {
            return ListTags(new ListTagsParams());
        }

        public ListTagsResult ListTagsByPrefix(string prefix)
        {
            return ListTags(new ListTagsParams() { Prefix = prefix });
        }

        public ListTagsResult ListTags(ListTagsParams parameters)
        {
            UrlBuilder urlBuilder = new UrlBuilder(
                m_api.ApiUrlV.
                ResourceType("tags").
                Add(Api.GetCloudinaryParam<ResourceType>(parameters.ResourceType)).
                BuildUrl());

            foreach (var param in parameters.ToParamsDictionary())
            {
                urlBuilder.QueryString[param.Key] = param.Value.ToString();
            }

            using (HttpWebResponse response = m_api.Call(
                HttpMethod.GET, urlBuilder.ToString(), null, null))
            {
                ListTagsResult result = ListTagsResult.Parse(response);
                return result;
            }
        }

        public ListTransformsResult ListTransformations()
        {
            return ListTransformations(new ListTransformsParams());
        }

        public ListTransformsResult ListTransformations(ListTransformsParams parameters)
        {
            UrlBuilder urlBuilder = new UrlBuilder(
                m_api.ApiUrlV.
                ResourceType("transformations").
                BuildUrl());

            foreach (var param in parameters.ToParamsDictionary())
            {
                urlBuilder.QueryString[param.Key] = param.Value.ToString();
            }

            using (HttpWebResponse response = m_api.Call(
                HttpMethod.GET, urlBuilder.ToString(), null, null))
            {
                ListTransformsResult result = ListTransformsResult.Parse(response);
                return result;
            }
        }

        public GetTransformResult GetTransform(string transform)
        {
            return GetTransform(new GetTransformParams() { Transformation = transform });
        }

        public GetTransformResult GetTransform(GetTransformParams parameters)
        {
            UrlBuilder urlBuilder = new UrlBuilder(
                m_api.ApiUrlV.
                ResourceType("transformations").
                Add(parameters.Transformation).
                BuildUrl());

            foreach (var param in parameters.ToParamsDictionary())
            {
                urlBuilder.QueryString[param.Key] = param.Value.ToString();
            }

            using (HttpWebResponse response = m_api.Call(
                HttpMethod.GET, urlBuilder.ToString(), null, null))
            {
                GetTransformResult result = GetTransformResult.Parse(response);
                return result;
            }
        }

        public GetResourceResult GetResource(string publicId)
        {
            return GetResource(new GetResourceParams(publicId));
        }

        public GetResourceResult GetResource(GetResourceParams parameters)
        {
            UrlBuilder urlBuilder = new UrlBuilder(
                m_api.ApiUrlV.
                ResourceType("resources").
                Add(Api.GetCloudinaryParam<ResourceType>(parameters.ResourceType)).
                Add(parameters.Type).Add(parameters.PublicId).
                BuildUrl());

            foreach (var param in parameters.ToParamsDictionary())
            {
                urlBuilder.QueryString[param.Key] = param.Value.ToString();
            }

            using (HttpWebResponse response = m_api.Call(
                HttpMethod.GET, urlBuilder.ToString(), null, null))
            {
                GetResourceResult result = GetResourceResult.Parse(response);
                return result;
            }
        }

        public DelDerivedResResult DeleteDerivedResources(params string[] ids)
        {
            DelDerivedResParams p = new DelDerivedResParams();
            p.DerivedResources.AddRange(ids);
            return DeleteDerivedResources(p);
        }

        public DelDerivedResResult DeleteDerivedResources(DelDerivedResParams parameters)
        {
            UrlBuilder urlBuilder = new UrlBuilder(
                m_api.ApiUrlV.
                Add("derived_resources").
                BuildUrl());

            foreach (var param in parameters.ToParamsDictionary())
            {
                if (param.Value is IEnumerable<string>)
                {
                    foreach (var item in (IEnumerable)param.Value)
                    {
                        urlBuilder.QueryString.Add(String.Format("{0}[]", param.Key), item.ToString());
                    }
                }
                else
                {
                    urlBuilder.QueryString[param.Key] = param.Value.ToString();
                }
            }

            using (HttpWebResponse response = m_api.Call(
                HttpMethod.DELETE, urlBuilder.ToString(), null, null))
            {
                DelDerivedResResult result = DelDerivedResResult.Parse(response);
                return result;
            }
        }

        public DelResResult DeleteResources(params string[] publicIds)
        {
            DelResParams p = new DelResParams();
            p.PublicIds.AddRange(publicIds);
            return DeleteResources(p);
        }

        public DelResResult DeleteResourcesByPrefix(string prefix)
        {
            DelResParams p = new DelResParams() { Prefix = prefix };
            return DeleteResources(p);
        }

        public DelResResult DeleteResourcesByTag(string tag)
        {
            DelResParams p = new DelResParams() { Tag = tag };
            return DeleteResources(p);
        }

        public DelResResult DeleteResources(DelResParams parameters)
        {
            Url url = m_api.ApiUrlV.
                Add("resources").
                Add(Api.GetCloudinaryParam<ResourceType>(parameters.ResourceType));

            if (String.IsNullOrEmpty(parameters.Tag))
            {
                url = url.Add(parameters.Type);
            }
            else
            {
                url = url.Add("tags").Add(parameters.Tag);
            }

            UrlBuilder urlBuilder = new UrlBuilder(url.BuildUrl());

            foreach (var param in parameters.ToParamsDictionary())
            {
                if (param.Value is IEnumerable<string>)
                {
                    foreach (var item in (IEnumerable)param.Value)
                    {
                        urlBuilder.QueryString.Add(String.Format("{0}[]", param.Key), item.ToString());
                    }
                }
                else
                {
                    urlBuilder.QueryString[param.Key] = param.Value.ToString();
                }
            }

            using (HttpWebResponse response = m_api.Call(
                HttpMethod.DELETE, urlBuilder.ToString(), null, null))
            {
                DelResResult result = DelResResult.Parse(response);
                return result;
            }
        }

        public UpdateTransformResult UpdateTransform(UpdateTransformParams parameters)
        {
            UrlBuilder urlBuilder = new UrlBuilder(
                m_api.ApiUrlV.
                ResourceType("transformations").
                Add(parameters.Transformation).
                BuildUrl());

            foreach (var param in parameters.ToParamsDictionary())
            {
                urlBuilder.QueryString[param.Key] = param.Value.ToString();
            }

            using (HttpWebResponse response = m_api.Call(
                HttpMethod.PUT, urlBuilder.ToString(), null, null))
            {
                UpdateTransformResult result = UpdateTransformResult.Parse(response);
                return result;
            }
        }

        public TransformResult CreateTransform(CreateTransformParams parameters)
        {
            UrlBuilder urlBuilder = new UrlBuilder(
                m_api.ApiUrlV.
                ResourceType("transformations").
                Add(parameters.Name).
                BuildUrl());

            urlBuilder.QueryString["transformation"] = parameters.Transform.Generate();

            using (HttpWebResponse response = m_api.Call(
                HttpMethod.POST, urlBuilder.ToString(), null, null))
            {
                TransformResult result = TransformResult.Parse(response);
                return result;
            }
        }

        public TransformResult DeleteTransform(string transformName)
        {
            UrlBuilder urlBuilder = new UrlBuilder(
                m_api.ApiUrlV.
                ResourceType("transformations").
                Add(transformName).
                BuildUrl());

            using (HttpWebResponse response = m_api.Call(
                HttpMethod.DELETE, urlBuilder.ToString(), null, null))
            {
                TransformResult result = TransformResult.Parse(response);
                return result;
            }
        }

        /// <summary>
        /// Eagerly generate sprites
        /// </summary>
        /// <param name="parameters">Parameters for sprite generation</param>
        /// <returns>Result of sprite generation</returns>
        public SpriteResult MakeSprite(SpriteParams parameters)
        {
            UrlBuilder urlBuilder = new UrlBuilder(
                m_api.ApiUrlImgUpV.
                Action("sprite").
                BuildUrl());

            using (HttpWebResponse response = m_api.Call(
                HttpMethod.POST, urlBuilder.ToString(), parameters.ToParamsDictionary(), null))
            {
                SpriteResult result = SpriteResult.Parse(response);
                return result;
            }
        }

        /// <summary>
        /// Allows multi transformation
        /// </summary>
        /// <param name="parameters">Parameters of operation</param>
        /// <returns>Result of operation</returns>
        public MultiResult Multi(MultiParams parameters)
        {
            UrlBuilder urlBuilder = new UrlBuilder(
                m_api.ApiUrlImgUpV.
                Action("multi").
                BuildUrl());

            using (HttpWebResponse response = m_api.Call(
                HttpMethod.POST, urlBuilder.ToString(), parameters.ToParamsDictionary(), null))
            {
                MultiResult result = MultiResult.Parse(response);
                return result;
            }
        }

        /// <summary>
        /// Explode multipage document to single pages
        /// </summary>
        /// <param name="parameters">Parameters of explosion</param>
        /// <returns>Result of operation</returns>
        public ExplodeResult Explode(ExplodeParams parameters)
        {
            UrlBuilder urlBuilder = new UrlBuilder(
                m_api.ApiUrlImgUpV.
                Action("explode").
                BuildUrl());

            using (HttpWebResponse response = m_api.Call(
                HttpMethod.POST, urlBuilder.ToString(), parameters.ToParamsDictionary(), null))
            {
                ExplodeResult result = ExplodeResult.Parse(response);
                return result;
            }
        }

        private string GetDownloadUrl(UrlBuilder builder, IDictionary<string, object> parameters)
        {
            m_api.FinalizeUploadParameters(parameters);

            foreach (var param in parameters)
            {
                builder.QueryString[param.Key] = param.Value.ToString();
            }

            return builder.ToString();
        }
    }
}
