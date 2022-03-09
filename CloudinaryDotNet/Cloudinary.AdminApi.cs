namespace CloudinaryDotNet
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;
    using System.Globalization;
    using System.Threading;
    using System.Threading.Tasks;
    using CloudinaryDotNet.Actions;

    /// <summary>
    /// Main class of Cloudinary .NET API.
    /// </summary>
    [SuppressMessage("StyleCop.CSharp.NamingRules", "SA1310:FieldNamesMustNotContainUnderscore", Justification = "Reviewed.")]
    public partial class Cloudinary : ICloudinaryAdmin
    {
        private readonly ICloudinaryAdmin cloudinaryAdmin;

        /// <inheritdoc />
        public Search Search()
        {
            return cloudinaryAdmin.Search();
        }

        /// <inheritdoc />
        public Task<ListResourceTypesResult> ListResourceTypesAsync(CancellationToken? cancellationToken = null)
        {
            return cloudinaryAdmin.ListResourceTypesAsync(cancellationToken);
        }

        /// <inheritdoc />
        public ListResourceTypesResult ListResourceTypes()
        {
            return cloudinaryAdmin.ListResourceTypes();
        }

        /// <inheritdoc />
        public Task<ListResourcesResult> ListResourcesAsync(string nextCursor = null, bool tags = true, bool context = true, bool moderations = true, CancellationToken? cancellationToken = null)
        {
            return cloudinaryAdmin.ListResourcesAsync(nextCursor, tags, context, moderations, cancellationToken);
        }

        /// <inheritdoc />
        public Task<ListResourcesResult> ListResourcesAsync(ListResourcesParams parameters, CancellationToken? cancellationToken = null)
        {
            return cloudinaryAdmin.ListResourcesAsync(parameters, cancellationToken);
        }

        /// <inheritdoc />
        public ListResourcesResult ListResources(string nextCursor = null, bool tags = true, bool context = true, bool moderations = true)
        {
            return cloudinaryAdmin.ListResources(nextCursor, tags, context, moderations);
        }

        /// <inheritdoc />
        public ListResourcesResult ListResources(ListResourcesParams parameters)
        {
            return cloudinaryAdmin.ListResources(parameters);
        }

        /// <inheritdoc />
        public Task<ListResourcesResult> ListResourcesByTypeAsync(string type, string nextCursor = null, CancellationToken? cancellationToken = null)
        {
            return cloudinaryAdmin.ListResourcesByTypeAsync(type, nextCursor, cancellationToken);
        }

        /// <inheritdoc />
        public ListResourcesResult ListResourcesByType(string type, string nextCursor = null)
        {
            return cloudinaryAdmin.ListResourcesByType(type, nextCursor);
        }

        /// <inheritdoc />
        public Task<ListResourcesResult> ListResourcesByPrefixAsync(string prefix, string type = "upload", string nextCursor = null, CancellationToken? cancellationToken = null)
        {
            return cloudinaryAdmin.ListResourcesByPrefixAsync(prefix, type, nextCursor, cancellationToken);
        }

        /// <inheritdoc />
        public Task<ListResourcesResult> ListResourcesByPrefixAsync(string prefix, bool tags, bool context, bool moderations, string type = "upload", string nextCursor = null, CancellationToken? cancellationToken = null)
        {
            return cloudinaryAdmin.ListResourcesByPrefixAsync(prefix, tags, context, moderations, type, nextCursor, cancellationToken);
        }

        /// <inheritdoc />
        public ListResourcesResult ListResourcesByPrefix(string prefix, string type = "upload", string nextCursor = null)
        {
            return cloudinaryAdmin.ListResourcesByPrefix(prefix, type, nextCursor);
        }

        /// <inheritdoc />
        public ListResourcesResult ListResourcesByPrefix(string prefix, bool tags, bool context, bool moderations, string type = "upload", string nextCursor = null)
        {
            return cloudinaryAdmin.ListResourcesByPrefix(prefix, tags, context, moderations, type, nextCursor);
        }

        /// <inheritdoc />
        public Task<ListResourcesResult> ListResourcesByTagAsync(string tag, string nextCursor = null, CancellationToken? cancellationToken = null)
        {
            return cloudinaryAdmin.ListResourcesByTagAsync(tag, nextCursor, cancellationToken);
        }

        /// <inheritdoc />
        public ListResourcesResult ListResourcesByTag(string tag, string nextCursor = null)
        {
            return cloudinaryAdmin.ListResourcesByTag(tag, nextCursor);
        }

        /// <inheritdoc />
        public Task<ListResourcesResult> ListResourcesByPublicIdsAsync(IEnumerable<string> publicIds, CancellationToken? cancellationToken = null)
        {
            return cloudinaryAdmin.ListResourcesByPublicIdsAsync(publicIds, cancellationToken);
        }

        /// <inheritdoc />
        public ListResourcesResult ListResourcesByPublicIds(IEnumerable<string> publicIds)
        {
            return cloudinaryAdmin.ListResourcesByPublicIds(publicIds);
        }

        /// <inheritdoc />
        public Task<ListResourcesResult> ListResourceByPublicIdsAsync(IEnumerable<string> publicIds, bool tags, bool context, bool moderations, CancellationToken? cancellationToken = null)
        {
            return cloudinaryAdmin.ListResourceByPublicIdsAsync(publicIds, tags, context, moderations, cancellationToken);
        }

        /// <inheritdoc />
        public ListResourcesResult ListResourceByPublicIds(IEnumerable<string> publicIds, bool tags, bool context, bool moderations)
        {
            return cloudinaryAdmin.ListResourceByPublicIds(publicIds, tags, context, moderations);
        }

        /// <inheritdoc />
        public Task<ListResourcesResult> ListResourcesByModerationStatusAsync(string kind, ModerationStatus status, bool tags = true, bool context = true, bool moderations = true, string nextCursor = null, CancellationToken? cancellationToken = null)
        {
            return cloudinaryAdmin.ListResourcesByModerationStatusAsync(kind, status, tags, context, moderations, nextCursor, cancellationToken);
        }

        /// <inheritdoc />
        public ListResourcesResult ListResourcesByModerationStatus(string kind, ModerationStatus status, bool tags = true, bool context = true, bool moderations = true, string nextCursor = null)
        {
            return cloudinaryAdmin.ListResourcesByModerationStatus(kind, status, tags, context, moderations, nextCursor);
        }

        /// <inheritdoc />
        public Task<ListResourcesResult> ListResourcesByContextAsync(string key, string value = "", bool tags = false, bool context = false, string nextCursor = null, CancellationToken? cancellationToken = null)
        {
            return cloudinaryAdmin.ListResourcesByContextAsync(key, value, tags, context, nextCursor, cancellationToken);
        }

        /// <inheritdoc />
        public ListResourcesResult ListResourcesByContext(string key, string value = "", bool tags = false, bool context = false, string nextCursor = null)
        {
            return cloudinaryAdmin.ListResourcesByContext(key, value, tags, context, nextCursor);
        }

        /// <inheritdoc />
        public Task<PublishResourceResult> PublishResourceByPrefixAsync(string prefix, PublishResourceParams parameters, CancellationToken? cancellationToken)
        {
            return cloudinaryAdmin.PublishResourceByPrefixAsync(prefix, parameters, cancellationToken);
        }

        /// <inheritdoc />
        public PublishResourceResult PublishResourceByPrefix(string prefix, PublishResourceParams parameters)
        {
            return cloudinaryAdmin.PublishResourceByPrefix(prefix, parameters);
        }

        /// <inheritdoc />
        public Task<PublishResourceResult> PublishResourceByTagAsync(string tag, PublishResourceParams parameters, CancellationToken? cancellationToken = null)
        {
            return cloudinaryAdmin.PublishResourceByTagAsync(tag, parameters, cancellationToken);
        }

        /// <inheritdoc />
        public PublishResourceResult PublishResourceByTag(string tag, PublishResourceParams parameters)
        {
            return cloudinaryAdmin.PublishResourceByTag(tag, parameters);
        }

        /// <inheritdoc />
        public Task<PublishResourceResult> PublishResourceByIdsAsync(string tag, PublishResourceParams parameters, CancellationToken? cancellationToken)
        {
            return cloudinaryAdmin.PublishResourceByIdsAsync(tag, parameters, cancellationToken);
        }

        /// <inheritdoc />
        public PublishResourceResult PublishResourceByIds(string tag, PublishResourceParams parameters)
        {
            return cloudinaryAdmin.PublishResourceByIds(tag, parameters);
        }

        /// <inheritdoc />
        public Task<UpdateResourceAccessModeResult> UpdateResourceAccessModeByTagAsync(string tag, UpdateResourceAccessModeParams parameters, CancellationToken? cancellationToken = null)
        {
            return cloudinaryAdmin.UpdateResourceAccessModeByTagAsync(tag, parameters, cancellationToken);
        }

        /// <inheritdoc />
        public UpdateResourceAccessModeResult UpdateResourceAccessModeByTag(string tag, UpdateResourceAccessModeParams parameters)
        {
            return cloudinaryAdmin.UpdateResourceAccessModeByTag(tag, parameters);
        }

        /// <inheritdoc />
        public Task<UpdateResourceAccessModeResult> UpdateResourceAccessModeByPrefixAsync(string prefix, UpdateResourceAccessModeParams parameters, CancellationToken? cancellationToken = null)
        {
            return cloudinaryAdmin.UpdateResourceAccessModeByPrefixAsync(prefix, parameters, cancellationToken);
        }

        /// <inheritdoc />
        public UpdateResourceAccessModeResult UpdateResourceAccessModeByPrefix(string prefix, UpdateResourceAccessModeParams parameters)
        {
            return cloudinaryAdmin.UpdateResourceAccessModeByPrefix(prefix, parameters);
        }

        /// <inheritdoc />
        public Task<UpdateResourceAccessModeResult> UpdateResourceAccessModeByIdsAsync(UpdateResourceAccessModeParams parameters, CancellationToken? cancellationToken = null)
        {
            return cloudinaryAdmin.UpdateResourceAccessModeByIdsAsync(parameters, cancellationToken);
        }

        /// <inheritdoc />
        public UpdateResourceAccessModeResult UpdateResourceAccessModeByIds(UpdateResourceAccessModeParams parameters)
        {
            return cloudinaryAdmin.UpdateResourceAccessModeByIds(parameters);
        }

        /// <inheritdoc />
        public Task<DelDerivedResResult> DeleteDerivedResourcesByTransformAsync(DelDerivedResParams parameters, CancellationToken? cancellationToken = null)
        {
            return cloudinaryAdmin.DeleteDerivedResourcesByTransformAsync(parameters, cancellationToken);
        }

        /// <inheritdoc />
        public DelDerivedResResult DeleteDerivedResourcesByTransform(DelDerivedResParams parameters)
        {
            return cloudinaryAdmin.DeleteDerivedResourcesByTransform(parameters);
        }

        /// <inheritdoc />
        public Task<GetFoldersResult> RootFoldersAsync(GetFoldersParams parameters = null, CancellationToken? cancellationToken = null)
        {
            return cloudinaryAdmin.RootFoldersAsync(parameters, cancellationToken);
        }

        /// <inheritdoc />
        public GetFoldersResult RootFolders(GetFoldersParams parameters = null)
        {
            return cloudinaryAdmin.RootFolders(parameters);
        }

        /// <inheritdoc />
        public Task<GetFoldersResult> SubFoldersAsync(string folder, CancellationToken? cancellationToken = null)
        {
            return cloudinaryAdmin.SubFoldersAsync(folder, cancellationToken);
        }

        /// <inheritdoc />
        public Task<GetFoldersResult> SubFoldersAsync(string folder, GetFoldersParams parameters, CancellationToken? cancellationToken = null)
        {
            return cloudinaryAdmin.SubFoldersAsync(folder, parameters, cancellationToken);
        }

        /// <inheritdoc />
        public GetFoldersResult SubFolders(string folder, GetFoldersParams parameters = null)
        {
            return cloudinaryAdmin.SubFolders(folder, parameters);
        }

        /// <inheritdoc />
        public Task<DeleteFolderResult> DeleteFolderAsync(string folder, CancellationToken? cancellationToken = null)
        {
            return cloudinaryAdmin.DeleteFolderAsync(folder, cancellationToken);
        }

        /// <inheritdoc />
        public DeleteFolderResult DeleteFolder(string folder)
        {
            return cloudinaryAdmin.DeleteFolder(folder);
        }

        /// <inheritdoc />
        public CreateFolderResult CreateFolder(string folder)
        {
            return cloudinaryAdmin.CreateFolder(folder);
        }

        /// <inheritdoc />
        public Task<CreateFolderResult> CreateFolderAsync(string folder, CancellationToken? cancellationToken = null)
        {
            return cloudinaryAdmin.CreateFolderAsync(folder, cancellationToken);
        }

        /// <inheritdoc />
        public Task<UploadPresetResult> CreateUploadPresetAsync(UploadPresetParams parameters, CancellationToken? cancellationToken = null)
        {
            return cloudinaryAdmin.CreateUploadPresetAsync(parameters, cancellationToken);
        }

        /// <inheritdoc />
        public UploadPresetResult CreateUploadPreset(UploadPresetParams parameters)
        {
            return cloudinaryAdmin.CreateUploadPreset(parameters);
        }

        /// <inheritdoc />
        public Task<UploadPresetResult> UpdateUploadPresetAsync(UploadPresetParams parameters, CancellationToken? cancellationToken = null)
        {
            return cloudinaryAdmin.UpdateUploadPresetAsync(parameters, cancellationToken);
        }

        /// <inheritdoc />
        public UploadPresetResult UpdateUploadPreset(UploadPresetParams parameters)
        {
            return cloudinaryAdmin.UpdateUploadPreset(parameters);
        }

        /// <inheritdoc />
        public Task<GetUploadPresetResult> GetUploadPresetAsync(string name, CancellationToken? cancellationToken = null)
        {
            return cloudinaryAdmin.GetUploadPresetAsync(name, cancellationToken);
        }

        /// <inheritdoc />
        public GetUploadPresetResult GetUploadPreset(string name)
        {
            return cloudinaryAdmin.GetUploadPreset(name);
        }

        /// <inheritdoc />
        public Task<ListUploadPresetsResult> ListUploadPresetsAsync(string nextCursor = null, CancellationToken? cancellationToken = null)
        {
            return cloudinaryAdmin.ListUploadPresetsAsync(nextCursor, cancellationToken);
        }

        /// <inheritdoc />
        public Task<ListUploadPresetsResult> ListUploadPresetsAsync(ListUploadPresetsParams parameters, CancellationToken? cancellationToken = null)
        {
            return cloudinaryAdmin.ListUploadPresetsAsync(parameters, cancellationToken);
        }

        /// <inheritdoc />
        public ListUploadPresetsResult ListUploadPresets(string nextCursor = null)
        {
            return cloudinaryAdmin.ListUploadPresets(nextCursor);
        }

        /// <inheritdoc />
        public ListUploadPresetsResult ListUploadPresets(ListUploadPresetsParams parameters)
        {
            return cloudinaryAdmin.ListUploadPresets(parameters);
        }

        /// <inheritdoc />
        public Task<DeleteUploadPresetResult> DeleteUploadPresetAsync(string name, CancellationToken? cancellationToken = null)
        {
            return cloudinaryAdmin.DeleteUploadPresetAsync(name, cancellationToken);
        }

        /// <inheritdoc />
        public DeleteUploadPresetResult DeleteUploadPreset(string name)
        {
            return cloudinaryAdmin.DeleteUploadPreset(name);
        }

        /// <inheritdoc />
        public Task<UsageResult> GetUsageAsync(DateTime? date, CancellationToken? cancellationToken = null)
        {
            return cloudinaryAdmin.GetUsageAsync(date, cancellationToken);
        }

        /// <inheritdoc />
        public Task<UsageResult> GetUsageAsync(CancellationToken? cancellationToken = null)
        {
            return cloudinaryAdmin.GetUsageAsync(cancellationToken);
        }

        /// <inheritdoc />
        public UsageResult GetUsage(DateTime? date = null)
        {
            return cloudinaryAdmin.GetUsage(date);
        }

        /// <inheritdoc />
        public Task<ListTagsResult> ListTagsAsync(CancellationToken? cancellationToken = null)
        {
            return cloudinaryAdmin.ListTagsAsync(cancellationToken);
        }

        /// <inheritdoc />
        public Task<ListTagsResult> ListTagsAsync(ListTagsParams parameters, CancellationToken? cancellationToken = null)
        {
            return cloudinaryAdmin.ListTagsAsync(parameters, cancellationToken);
        }

        /// <inheritdoc />
        public ListTagsResult ListTags()
        {
            return cloudinaryAdmin.ListTags();
        }

        /// <inheritdoc />
        public ListTagsResult ListTags(ListTagsParams parameters)
        {
            return cloudinaryAdmin.ListTags(parameters);
        }

        /// <inheritdoc />
        public Task<ListTagsResult> ListTagsByPrefixAsync(string prefix, CancellationToken? cancellationToken = null)
        {
            return cloudinaryAdmin.ListTagsByPrefixAsync(prefix, cancellationToken);
        }

        /// <inheritdoc />
        public ListTagsResult ListTagsByPrefix(string prefix)
        {
            return cloudinaryAdmin.ListTagsByPrefix(prefix);
        }

        /// <inheritdoc />
        public Task<ListTransformsResult> ListTransformationsAsync(CancellationToken? cancellationToken = null)
        {
            return cloudinaryAdmin.ListTransformationsAsync(cancellationToken);
        }

        /// <inheritdoc />
        public Task<ListTransformsResult> ListTransformationsAsync(ListTransformsParams parameters, CancellationToken? cancellationToken = null)
        {
            return cloudinaryAdmin.ListTransformationsAsync(parameters, cancellationToken);
        }

        /// <inheritdoc />
        public ListTransformsResult ListTransformations()
        {
            return cloudinaryAdmin.ListTransformations();
        }

        /// <inheritdoc />
        public ListTransformsResult ListTransformations(ListTransformsParams parameters)
        {
            return cloudinaryAdmin.ListTransformations(parameters);
        }

        /// <inheritdoc />
        public Task<GetTransformResult> GetTransformAsync(string transform, CancellationToken? cancellationToken = null)
        {
            return cloudinaryAdmin.GetTransformAsync(transform, cancellationToken);
        }

        /// <inheritdoc />
        public Task<GetTransformResult> GetTransformAsync(GetTransformParams parameters, CancellationToken? cancellationToken = null)
        {
            return cloudinaryAdmin.GetTransformAsync(parameters, cancellationToken);
        }

        /// <inheritdoc />
        public GetTransformResult GetTransform(string transform)
        {
            return cloudinaryAdmin.GetTransform(transform);
        }

        /// <inheritdoc />
        public GetTransformResult GetTransform(GetTransformParams parameters)
        {
            return cloudinaryAdmin.GetTransform(parameters);
        }

        /// <inheritdoc />
        public Task<GetResourceResult> UpdateResourceAsync(string publicId, ModerationStatus moderationStatus, CancellationToken? cancellationToken = null)
        {
            return cloudinaryAdmin.UpdateResourceAsync(publicId, moderationStatus, cancellationToken);
        }

        /// <inheritdoc />
        public Task<GetResourceResult> UpdateResourceAsync(UpdateParams parameters, CancellationToken? cancellationToken = null)
        {
            return cloudinaryAdmin.UpdateResourceAsync(parameters, cancellationToken);
        }

        /// <inheritdoc />
        public GetResourceResult UpdateResource(string publicId, ModerationStatus moderationStatus)
        {
            return cloudinaryAdmin.UpdateResource(publicId, moderationStatus);
        }

        /// <inheritdoc />
        public GetResourceResult UpdateResource(UpdateParams parameters)
        {
            return cloudinaryAdmin.UpdateResource(parameters);
        }

        /// <inheritdoc />
        public Task<GetResourceResult> GetResourceAsync(string publicId, CancellationToken? cancellationToken = null)
        {
            return cloudinaryAdmin.GetResourceAsync(publicId, cancellationToken);
        }

        /// <inheritdoc />
        public Task<GetResourceResult> GetResourceAsync(GetResourceParams parameters, CancellationToken? cancellationToken = null)
        {
            return cloudinaryAdmin.GetResourceAsync(parameters, cancellationToken);
        }

        /// <inheritdoc />
        public GetResourceResult GetResource(string publicId)
        {
            return cloudinaryAdmin.GetResource(publicId);
        }

        /// <inheritdoc />
        public GetResourceResult GetResource(GetResourceParams parameters)
        {
            return cloudinaryAdmin.GetResource(parameters);
        }

        /// <inheritdoc />
        public Task<DelDerivedResResult> DeleteDerivedResourcesAsync(params string[] ids)
        {
            return cloudinaryAdmin.DeleteDerivedResourcesAsync(ids);
        }

        /// <inheritdoc />
        public Task<DelDerivedResResult> DeleteDerivedResourcesAsync(DelDerivedResParams parameters, CancellationToken? cancellationToken = null)
        {
            return cloudinaryAdmin.DeleteDerivedResourcesAsync(parameters, cancellationToken);
        }

        /// <inheritdoc />
        public DelDerivedResResult DeleteDerivedResources(params string[] ids)
        {
            return cloudinaryAdmin.DeleteDerivedResources(ids);
        }

        /// <inheritdoc />
        public DelDerivedResResult DeleteDerivedResources(DelDerivedResParams parameters)
        {
            return cloudinaryAdmin.DeleteDerivedResources(parameters);
        }

        /// <inheritdoc />
        public Task<DelResResult> DeleteResourcesAsync(ResourceType type, params string[] publicIds)
        {
            return cloudinaryAdmin.DeleteResourcesAsync(type, publicIds);
        }

        /// <inheritdoc />
        public Task<DelResResult> DeleteResourcesAsync(params string[] publicIds)
        {
            return cloudinaryAdmin.DeleteResourcesAsync(publicIds);
        }

        /// <inheritdoc />
        public Task<DelResResult> DeleteResourcesAsync(DelResParams parameters, CancellationToken? cancellationToken = null)
        {
            return cloudinaryAdmin.DeleteResourcesAsync(parameters, cancellationToken);
        }

        /// <inheritdoc />
        public DelResResult DeleteResources(ResourceType type, params string[] publicIds)
        {
            return cloudinaryAdmin.DeleteResources(type, publicIds);
        }

        /// <inheritdoc />
        public DelResResult DeleteResources(params string[] publicIds)
        {
            return cloudinaryAdmin.DeleteResources(publicIds);
        }

        /// <inheritdoc />
        public DelResResult DeleteResources(DelResParams parameters)
        {
            return cloudinaryAdmin.DeleteResources(parameters);
        }

        /// <inheritdoc />
        public Task<DelResResult> DeleteResourcesByPrefixAsync(string prefix, CancellationToken? cancellationToken = null)
        {
            return cloudinaryAdmin.DeleteResourcesByPrefixAsync(prefix, cancellationToken);
        }

        /// <inheritdoc />
        public Task<DelResResult> DeleteResourcesByPrefixAsync(string prefix, bool keepOriginal, string nextCursor, CancellationToken? cancellationToken = null)
        {
            return cloudinaryAdmin.DeleteResourcesByPrefixAsync(prefix, keepOriginal, nextCursor, cancellationToken);
        }

        /// <inheritdoc />
        public DelResResult DeleteResourcesByPrefix(string prefix)
        {
            return cloudinaryAdmin.DeleteResourcesByPrefix(prefix);
        }

        /// <inheritdoc />
        public DelResResult DeleteResourcesByPrefix(string prefix, bool keepOriginal, string nextCursor)
        {
            return cloudinaryAdmin.DeleteResourcesByPrefix(prefix, keepOriginal, nextCursor);
        }

        /// <inheritdoc />
        public Task<DelResResult> DeleteResourcesByTagAsync(string tag, CancellationToken? cancellationToken = null)
        {
            return cloudinaryAdmin.DeleteResourcesByTagAsync(tag, cancellationToken);
        }

        /// <inheritdoc />
        public Task<DelResResult> DeleteResourcesByTagAsync(string tag, bool keepOriginal, string nextCursor, CancellationToken? cancellationToken = null)
        {
            return cloudinaryAdmin.DeleteResourcesByTagAsync(tag, keepOriginal, nextCursor, cancellationToken);
        }

        /// <inheritdoc />
        public DelResResult DeleteResourcesByTag(string tag)
        {
            return cloudinaryAdmin.DeleteResourcesByTag(tag);
        }

        /// <inheritdoc />
        public DelResResult DeleteResourcesByTag(string tag, bool keepOriginal, string nextCursor)
        {
            return cloudinaryAdmin.DeleteResourcesByTag(tag, keepOriginal, nextCursor);
        }

        /// <inheritdoc />
        public Task<DelResResult> DeleteAllResourcesAsync(CancellationToken? cancellationToken = null)
        {
            return cloudinaryAdmin.DeleteAllResourcesAsync(cancellationToken);
        }

        /// <inheritdoc />
        public Task<DelResResult> DeleteAllResourcesAsync(bool keepOriginal, string nextCursor, CancellationToken? cancellationToken = null)
        {
            return cloudinaryAdmin.DeleteAllResourcesAsync(keepOriginal, nextCursor, cancellationToken);
        }

        /// <inheritdoc />
        public DelResResult DeleteAllResources()
        {
            return cloudinaryAdmin.DeleteAllResources();
        }

        /// <inheritdoc />
        public DelResResult DeleteAllResources(bool keepOriginal, string nextCursor)
        {
            return cloudinaryAdmin.DeleteAllResources(keepOriginal, nextCursor);
        }

        /// <inheritdoc />
        public Task<RestoreResult> RestoreAsync(params string[] publicIds)
        {
            return cloudinaryAdmin.RestoreAsync(publicIds);
        }

        /// <inheritdoc />
        public Task<RestoreResult> RestoreAsync(RestoreParams parameters, CancellationToken? cancellationToken = null)
        {
            return cloudinaryAdmin.RestoreAsync(parameters, cancellationToken);
        }

        /// <inheritdoc />
        public RestoreResult Restore(params string[] publicIds)
        {
            return cloudinaryAdmin.Restore(publicIds);
        }

        /// <inheritdoc />
        public RestoreResult Restore(RestoreParams parameters)
        {
            return cloudinaryAdmin.Restore(parameters);
        }

        /// <inheritdoc />
        public Task<UploadMappingResults> UploadMappingsAsync(UploadMappingParams parameters, CancellationToken? cancellationToken = null)
        {
            return cloudinaryAdmin.UploadMappingsAsync(parameters, cancellationToken);
        }

        /// <inheritdoc />
        public UploadMappingResults UploadMappings(UploadMappingParams parameters)
        {
            return cloudinaryAdmin.UploadMappings(parameters);
        }

        /// <inheritdoc />
        public Task<UploadMappingResults> UploadMappingAsync(string folder, CancellationToken? cancellationToken = null)
        {
            return cloudinaryAdmin.UploadMappingAsync(folder, cancellationToken);
        }

        /// <inheritdoc />
        public UploadMappingResults UploadMapping(string folder)
        {
            return cloudinaryAdmin.UploadMapping(folder);
        }

        /// <inheritdoc />
        public Task<UploadMappingResults> CreateUploadMappingAsync(string folder, string template, CancellationToken? cancellationToken = null)
        {
            return cloudinaryAdmin.CreateUploadMappingAsync(folder, template, cancellationToken);
        }

        /// <inheritdoc />
        public UploadMappingResults CreateUploadMapping(string folder, string template)
        {
            return cloudinaryAdmin.CreateUploadMapping(folder, template);
        }

        /// <inheritdoc />
        public Task<UploadMappingResults> UpdateUploadMappingAsync(string folder, string newTemplate, CancellationToken? cancellationToken = null)
        {
            return cloudinaryAdmin.UpdateUploadMappingAsync(folder, newTemplate, cancellationToken);
        }

        /// <inheritdoc />
        public UploadMappingResults UpdateUploadMapping(string folder, string newTemplate)
        {
            return cloudinaryAdmin.UpdateUploadMapping(folder, newTemplate);
        }

        /// <inheritdoc />
        public Task<UploadMappingResults> DeleteUploadMappingAsync(CancellationToken? cancellationToken = null)
        {
            return cloudinaryAdmin.DeleteUploadMappingAsync(cancellationToken);
        }

        /// <inheritdoc />
        public Task<UploadMappingResults> DeleteUploadMappingAsync(string folder, CancellationToken? cancellationToken = null)
        {
            return cloudinaryAdmin.DeleteUploadMappingAsync(folder, cancellationToken);
        }

        /// <inheritdoc />
        public UploadMappingResults DeleteUploadMapping()
        {
            return cloudinaryAdmin.DeleteUploadMapping();
        }

        /// <inheritdoc />
        public UploadMappingResults DeleteUploadMapping(string folder)
        {
            return cloudinaryAdmin.DeleteUploadMapping(folder);
        }

        /// <inheritdoc />
        public Task<UpdateTransformResult> UpdateTransformAsync(UpdateTransformParams parameters, CancellationToken? cancellationToken = null)
        {
            return cloudinaryAdmin.UpdateTransformAsync(parameters, cancellationToken);
        }

        /// <inheritdoc />
        public UpdateTransformResult UpdateTransform(UpdateTransformParams parameters)
        {
            return cloudinaryAdmin.UpdateTransform(parameters);
        }

        /// <inheritdoc />
        public Task<TransformResult> CreateTransformAsync(CreateTransformParams parameters, CancellationToken? cancellationToken = null)
        {
            return cloudinaryAdmin.CreateTransformAsync(parameters, cancellationToken);
        }

        /// <inheritdoc />
        public TransformResult CreateTransform(CreateTransformParams parameters)
        {
            return cloudinaryAdmin.CreateTransform(parameters);
        }

        /// <inheritdoc />
        public Task<TransformResult> DeleteTransformAsync(string transformName, CancellationToken? cancellationToken = null)
        {
            return cloudinaryAdmin.DeleteTransformAsync(transformName, cancellationToken);
        }

        /// <inheritdoc />
        public TransformResult DeleteTransform(string transformName)
        {
            return cloudinaryAdmin.DeleteTransform(transformName);
        }
    }
}
