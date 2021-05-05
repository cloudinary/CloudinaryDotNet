namespace CloudinaryDotNet
{
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;
    using System.Threading;
    using System.Threading.Tasks;
    using CloudinaryDotNet.Actions;

    /// <summary>
    /// Main class of Cloudinary .NET API.
    /// </summary>
    [SuppressMessage("StyleCop.CSharp.NamingRules", "SA1310:FieldNamesMustNotContainUnderscore", Justification = "Reviewed.")]
    public partial class Cloudinary
    {
        /// <summary>
        /// Lists resource types asynchronously.
        /// </summary>
        /// <param name="cancellationToken">(Optional) Cancellation token.</param>
        /// <returns>Parsed list of resource types.</returns>
        public Task<ListResourceTypesResult> ListResourceTypesAsync(CancellationToken? cancellationToken = null)
        {
            return CallAdminApiAsync<ListResourceTypesResult>(GetResourcesUrl().BuildUrl(), null, cancellationToken);
        }

        /// <summary>
        /// Lists resource types.
        /// </summary>
        /// <returns>Parsed list of resource types.</returns>
        public ListResourceTypesResult ListResourceTypes()
        {
            return ListResourceTypesAsync().GetAwaiter().GetResult();
        }

        /// <summary>
        /// Lists resources asynchronously asynchronously.
        /// </summary>
        /// <param name="nextCursor">Starting position.</param>
        /// <param name="tags">Whether to include tags in result.</param>
        /// <param name="context">Whether to include context in result.</param>
        /// <param name="moderations">Whether to include moderation status in result.</param>
        /// <param name="cancellationToken">(Optional) Cancellation token.</param>
        /// <returns>Parsed result of the resources listing.</returns>
        public Task<ListResourcesResult> ListResourcesAsync(
            string nextCursor = null,
            bool tags = true,
            bool context = true,
            bool moderations = true,
            CancellationToken? cancellationToken = null)
        {
            var listResourcesParams = new ListResourcesParams()
            {
                NextCursor = nextCursor,
                Tags = tags,
                Context = context,
                Moderations = moderations,
            };
            return ListResourcesAsync(listResourcesParams, cancellationToken);
        }

        /// <summary>
        /// Lists resources.
        /// </summary>
        /// <param name="nextCursor">Starting position.</param>
        /// <param name="tags">Whether to include tags in result.</param>
        /// <param name="context">Whether to include context in result.</param>
        /// <param name="moderations">Whether to include moderation status in result.</param>
        /// <returns>Parsed result of the resources listing.</returns>
        public ListResourcesResult ListResources(
            string nextCursor = null,
            bool tags = true,
            bool context = true,
            bool moderations = true)
        {
            return ListResourcesAsync(nextCursor, tags, context, moderations)
                .GetAwaiter().GetResult();
        }

        /// <summary>
        /// Lists resources of specified type asynchronously.
        /// </summary>
        /// <param name="type">Resource type.</param>
        /// <param name="nextCursor">Starting position.</param>
        /// <param name="cancellationToken">(Optional) Cancellation token.</param>
        /// <returns>Parsed result of the resources listing.</returns>
        public Task<ListResourcesResult> ListResourcesByTypeAsync(string type, string nextCursor = null, CancellationToken? cancellationToken = null)
        {
            return ListResourcesAsync(new ListResourcesParams() { Type = type, NextCursor = nextCursor }, cancellationToken);
        }

        /// <summary>
        /// Lists resources of specified type.
        /// </summary>
        /// <param name="type">Resource type.</param>
        /// <param name="nextCursor">Starting position.</param>
        /// <returns>Parsed result of the resources listing.</returns>
        public ListResourcesResult ListResourcesByType(string type, string nextCursor = null)
        {
            return ListResourcesByTypeAsync(type, nextCursor).GetAwaiter().GetResult();
        }

        /// <summary>
        /// Lists resources by prefix asynchronously.
        /// </summary>
        /// <param name="prefix">Public identifier prefix.</param>
        /// <param name="type">Resource type.</param>
        /// <param name="nextCursor">Starting position.</param>
        /// <param name="cancellationToken">(Optional) Cancellation token.</param>
        /// <returns>Parsed result of the resources listing.</returns>
        public Task<ListResourcesResult> ListResourcesByPrefixAsync(
            string prefix,
            string type = "upload",
            string nextCursor = null,
            CancellationToken? cancellationToken = null)
        {
            var listResourcesByPrefixParams = new ListResourcesByPrefixParams()
            {
                Type = type,
                Prefix = prefix,
                NextCursor = nextCursor,
            };
            return ListResourcesAsync(listResourcesByPrefixParams, cancellationToken);
        }

        /// <summary>
        /// Lists resources by prefix.
        /// </summary>
        /// <param name="prefix">Public identifier prefix.</param>
        /// <param name="type">Resource type.</param>
        /// <param name="nextCursor">Starting position.</param>
        /// <returns>Parsed result of the resources listing.</returns>
        public ListResourcesResult ListResourcesByPrefix(string prefix, string type = "upload", string nextCursor = null)
        {
            return ListResourcesByPrefixAsync(prefix, type, nextCursor)
                .GetAwaiter().GetResult();
        }

        /// <summary>
        /// Lists resources by prefix asynchronously.
        /// </summary>
        /// <param name="prefix">Public identifier prefix.</param>
        /// <param name="tags">Whether to include tags in result.</param>
        /// <param name="context">Whether to include context in result.</param>
        /// <param name="moderations">If true, include moderation status for each resource.</param>
        /// <param name="type">Resource type.</param>
        /// <param name="nextCursor">Starting position.</param>
        /// <param name="cancellationToken">(Optional) Cancellation token.</param>
        /// <returns>Parsed result of the resources listing.</returns>
        public Task<ListResourcesResult> ListResourcesByPrefixAsync(
            string prefix,
            bool tags,
            bool context,
            bool moderations,
            string type = "upload",
            string nextCursor = null,
            CancellationToken? cancellationToken = null)
        {
            var listResourcesByPrefixParams = new ListResourcesByPrefixParams()
            {
                Tags = tags,
                Context = context,
                Moderations = moderations,
                Type = type,
                Prefix = prefix,
                NextCursor = nextCursor,
            };
            return ListResourcesAsync(listResourcesByPrefixParams, cancellationToken);
        }

        /// <summary>
        /// Lists resources by prefix.
        /// </summary>
        /// <param name="prefix">Public identifier prefix.</param>
        /// <param name="tags">Whether to include tags in result.</param>
        /// <param name="context">Whether to include context in result.</param>
        /// <param name="moderations">If true, include moderation status for each resource.</param>
        /// <param name="type">Resource type.</param>
        /// <param name="nextCursor">Starting position.</param>
        /// <returns>Parsed result of the resources listing.</returns>
        public ListResourcesResult ListResourcesByPrefix(string prefix, bool tags, bool context, bool moderations, string type = "upload", string nextCursor = null)
        {
            return ListResourcesByPrefixAsync(prefix, tags, context, moderations, type, nextCursor)
                .GetAwaiter().GetResult();
        }

        /// <summary>
        /// Lists resources by tag asynchronously.
        /// </summary>
        /// <param name="tag">The tag.</param>
        /// <param name="nextCursor">Starting position.</param>
        /// <param name="cancellationToken">(Optional) Cancellation token.</param>
        /// <returns>Parsed result of the resources listing.</returns>
        public Task<ListResourcesResult> ListResourcesByTagAsync(string tag, string nextCursor = null, CancellationToken? cancellationToken = null)
        {
            var listResourcesByTagParams = new ListResourcesByTagParams()
            {
                Tag = tag,
                NextCursor = nextCursor,
            };
            return ListResourcesAsync(listResourcesByTagParams, cancellationToken);
        }

        /// <summary>
        /// Lists resources by tag.
        /// </summary>
        /// <param name="tag">The tag.</param>
        /// <param name="nextCursor">Starting position.</param>
        /// <returns>Parsed result of the resources listing.</returns>
        public ListResourcesResult ListResourcesByTag(string tag, string nextCursor = null)
        {
            return ListResourcesByTagAsync(tag, nextCursor).GetAwaiter().GetResult();
        }

        /// <summary>
        /// Returns resources with specified public identifiers asynchronously.
        /// </summary>
        /// <param name="publicIds">Public identifiers.</param>
        /// <param name="cancellationToken">(Optional) Cancellation token.</param>
        /// <returns>Parsed result of the resources listing.</returns>
        public Task<ListResourcesResult> ListResourcesByPublicIdsAsync(IEnumerable<string> publicIds, CancellationToken? cancellationToken = null)
        {
            var listSpecificResourcesParams = new ListSpecificResourcesParams()
            {
                PublicIds = new List<string>(publicIds),
            };
            return ListResourcesAsync(listSpecificResourcesParams, cancellationToken);
        }

        /// <summary>
        /// Returns resources with specified public identifiers.
        /// </summary>
        /// <param name="publicIds">Public identifiers.</param>
        /// <returns>Parsed result of the resources listing.</returns>
        public ListResourcesResult ListResourcesByPublicIds(IEnumerable<string> publicIds)
        {
            return ListResourcesByPublicIdsAsync(publicIds)
                .GetAwaiter().GetResult();
        }

        /// <summary>
        /// Returns resources with specified public identifiers asynchronously.
        /// </summary>
        /// <param name="publicIds">Public identifiers.</param>
        /// <param name="tags">Whether to include tags in result.</param>
        /// <param name="context">Whether to include context in result.</param>
        /// <param name="moderations">Whether to include moderation status in result.</param>
        /// <param name="cancellationToken">(Optional) Cancellation token.</param>
        /// <returns>Parsed result of the resources listing.</returns>
        public Task<ListResourcesResult> ListResourceByPublicIdsAsync(
            IEnumerable<string> publicIds,
            bool tags,
            bool context,
            bool moderations,
            CancellationToken? cancellationToken = null)
        {
            var listSpecificResourcesParams = new ListSpecificResourcesParams()
            {
                PublicIds = new List<string>(publicIds),
                Tags = tags,
                Context = context,
                Moderations = moderations,
            };
            return ListResourcesAsync(listSpecificResourcesParams, cancellationToken);
        }

        /// <summary>
        /// Returns resources with specified public identifiers.
        /// </summary>
        /// <param name="publicIds">Public identifiers.</param>
        /// <param name="tags">Whether to include tags in result.</param>
        /// <param name="context">Whether to include context in result.</param>
        /// <param name="moderations">Whether to include moderation status in result.</param>
        /// <returns>Parsed result of the resources listing.</returns>
        public ListResourcesResult ListResourceByPublicIds(IEnumerable<string> publicIds, bool tags, bool context, bool moderations)
        {
            return ListResourceByPublicIdsAsync(publicIds, tags, context, moderations)
                .GetAwaiter().GetResult();
        }

        /// <summary>
        /// Lists resources by moderation status asynchronously.
        /// </summary>
        /// <param name="kind">The moderation kind.</param>
        /// <param name="status">The moderation status.</param>
        /// <param name="tags">Whether to include tags in result.</param>
        /// <param name="context">Whether to include context in result.</param>
        /// <param name="moderations">Whether to include moderation status in result.</param>
        /// <param name="nextCursor">The next cursor.</param>
        /// <param name="cancellationToken">(Optional) Cancellation token.</param>
        /// <returns>Parsed result of the resources listing.</returns>
        public Task<ListResourcesResult> ListResourcesByModerationStatusAsync(
            string kind,
            ModerationStatus status,
            bool tags = true,
            bool context = true,
            bool moderations = true,
            string nextCursor = null,
            CancellationToken? cancellationToken = null)
        {
            var listResourcesByModerationParams = new ListResourcesByModerationParams()
            {
                ModerationKind = kind,
                ModerationStatus = status,
                Tags = tags,
                Context = context,
                Moderations = moderations,
                NextCursor = nextCursor,
            };
            return ListResourcesAsync(listResourcesByModerationParams, cancellationToken);
        }

        /// <summary>
        /// Lists resources by moderation status.
        /// </summary>
        /// <param name="kind">The moderation kind.</param>
        /// <param name="status">The moderation status.</param>
        /// <param name="tags">Whether to include tags in result.</param>
        /// <param name="context">Whether to include context in result.</param>
        /// <param name="moderations">Whether to include moderation status in result.</param>
        /// <param name="nextCursor">The next cursor.</param>
        /// <returns>Parsed result of the resources listing.</returns>
        public ListResourcesResult ListResourcesByModerationStatus(
            string kind,
            ModerationStatus status,
            bool tags = true,
            bool context = true,
            bool moderations = true,
            string nextCursor = null)
        {
            return ListResourcesByModerationStatusAsync(kind, status, tags, context, moderations, nextCursor)
                .GetAwaiter().GetResult();
        }

        /// <summary>
        /// List resources by context metadata keys and values asynchronously.
        /// </summary>
        /// <param name="key">Only resources with the given key should be returned.</param>
        /// <param name="value">When provided should only return resources with this given value for the context key.
        /// When not provided, return all resources for which the context key exists.</param>
        /// <param name="tags">If true, include list of tag names assigned for each resource.</param>
        /// <param name="context">If true, include context assigned to each resource.</param>
        /// <param name="nextCursor">The next cursor.</param>
        /// <param name="cancellationToken">(Optional) Cancellation token.</param>
        /// <returns>Parsed result of the resources listing.</returns>
        public Task<ListResourcesResult> ListResourcesByContextAsync(
            string key,
            string value = "",
            bool tags = false,
            bool context = false,
            string nextCursor = null,
            CancellationToken? cancellationToken = null)
        {
            var listResourcesByContextParams = new ListResourcesByContextParams()
            {
                Key = key,
                Value = value,
                Tags = tags,
                Context = context,
                NextCursor = nextCursor,
            };
            return ListResourcesAsync(listResourcesByContextParams, cancellationToken);
        }

        /// <summary>
        /// List resources by context metadata keys and values.
        /// </summary>
        /// <param name="key">Only resources with the given key should be returned.</param>
        /// <param name="value">When provided should only return resources with this given value for the context key.
        /// When not provided, return all resources for which the context key exists.</param>
        /// <param name="tags">If true, include list of tag names assigned for each resource.</param>
        /// <param name="context">If true, include context assigned to each resource.</param>
        /// <param name="nextCursor">The next cursor.</param>
        /// <returns>Parsed result of the resources listing.</returns>
        public ListResourcesResult ListResourcesByContext(string key, string value = "", bool tags = false, bool context = false, string nextCursor = null)
        {
            return ListResourcesByContextAsync(key, value, tags, context, nextCursor).GetAwaiter().GetResult();
        }

        /// <summary>
        /// Gets a list of resources asynchronously.
        /// </summary>
        /// <param name="parameters">Parameters to list resources.</param>
        /// <param name="cancellationToken">(Optional) Cancellation token.</param>
        /// <returns>Parsed result of the resources listing.</returns>
        public Task<ListResourcesResult> ListResourcesAsync(ListResourcesParams parameters, CancellationToken? cancellationToken = null)
        {
            var url = GetListResourcesUrl(parameters);
            return CallAdminApiAsync<ListResourcesResult>(url, parameters, cancellationToken);
        }

        /// <summary>
        /// Gets a list of resources.
        /// </summary>
        /// <param name="parameters">Parameters to list resources.</param>
        /// <returns>Parsed result of the resources listing.</returns>
        public ListResourcesResult ListResources(ListResourcesParams parameters)
        {
            return ListResourcesAsync(parameters).GetAwaiter().GetResult();
        }

        private string GetListResourcesUrl(ListResourcesParams parameters)
        {
            var url = GetResourcesUrl().Add(ApiShared.GetCloudinaryParam(parameters.ResourceType));

            switch (parameters)
            {
                case ListResourcesByTagParams tagParams when !string.IsNullOrEmpty(tagParams.Tag):
                    url
                        .Add("tags")
                        .Add(tagParams.Tag);

                    break;
                case ListResourcesByModerationParams modParams when !string.IsNullOrEmpty(modParams.ModerationKind):
                    url
                        .Add("moderations")
                        .Add(modParams.ModerationKind)
                        .Add(Api.GetCloudinaryParam(modParams.ModerationStatus));

                    break;
                case ListResourcesByContextParams _:
                    url.Add("context");

                    break;
            }

            var urlBuilder = new UrlBuilder(
                url.BuildUrl(),
                parameters.ToParamsDictionary());

            var s = urlBuilder.ToString();
            return s;
        }

        private Url GetResourcesUrl() => GetApiUrlV().ResourceType("resources");

        private Task<T> CallAdminApiAsync<T>(
                string url,
                BaseParams parameters,
                CancellationToken? cancellationToken)
            where T : BaseResult, new()
        {
            return m_api.CallApiAsync<T>(
                            HttpMethod.GET,
                            url,
                            parameters,
                            null,
                            null,
                            cancellationToken);
        }
    }
}
