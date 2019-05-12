using System;
using System.Threading.Tasks;
using CloudinaryDotNet.Actions;

namespace CloudinaryDotNet
{
    /// <summary>
    /// Provides asynchronous methods for interaction with Cloudinary.
    /// </summary>
    public partial class Cloudinary
    {
        /// <summary>
        /// Uploads large file asynchronously.
        /// </summary>
        /// <param name="parameters">Parameters of file uploading.</param>
        /// <param name="bufferSize">Chunk (buffer) size (20 MB by default).</param>
        /// <returns>Results of image uploading.</returns>
        public Task<RawUploadResult> UploadLargeRawAsync(BasicRawUploadParams parameters, int bufferSize = 20 * 1024 * 1024)
        {
            return Task.Factory.StartNew((object o) =>
            {
                var t = (Tuple<BasicRawUploadParams, int>)o;
                return UploadLargeRaw(t.Item1, t.Item2);
            }, new Tuple<BasicRawUploadParams, int>(parameters, bufferSize));
        }

        /// <summary>
        /// Uploads a file to Cloudinary asynchronously.
        /// </summary>
        /// <param name="parameters">Parameters of file uploading.</param>
        /// <param name="type">The type ("raw" or "auto", last by default).</param>
        /// <returns>Results of image uploading.</returns>
        public Task<RawUploadResult> UploadAsync(RawUploadParams parameters, string type = "auto")
        {
            return Task.Factory.StartNew((object o) =>
            {
                var t = (Tuple<RawUploadParams, string>)o;
                return Upload(t.Item1, t.Item2);
            }, new Tuple<RawUploadParams, string>(parameters, type));
        }

        /// <summary>
        /// Uploads an image file to Cloudinary asynchronously.
        /// </summary>
        /// <param name="parameters">Parameters of image uploading.</param>
        /// <returns>Results of image uploading.</returns>
        public Task<ImageUploadResult> UploadAsync(ImageUploadParams parameters)
        {
            return CallAsync(Upload, parameters);
        }

        /// <summary>
        /// This method can be used to force refresh facebook and twitter profile pictures.
        /// The response of this method includes the image's version. Use this version to bypass previously
        /// cached CDN copies. Also it can be used to generate transformed versions of an uploaded image.
        /// This is useful when Strict Transformations are allowed for your account and you wish to create
        /// custom derived images for already uploaded images.
        /// </summary>
        /// <param name="parameters">The parameters.</param>
        public Task<ExplicitResult> ExplicitAsync(ExplicitParams parameters)
        {
            return CallAsync(Explicit, parameters);
        }

        /// <summary>
        /// Changes public identifier of a file asynchronously.
        /// </summary>
        /// <param name="parameters">Operation parameters.</param>
        /// <returns></returns>
        public Task<RenameResult> RenameAsync(RenameParams parameters)
        {
            return CallAsync(Rename, parameters);
        }

        /// <summary>
        /// Delete file from Cloudinary asynchronously.
        /// </summary>
        /// <param name="parameters">Parameters of resource deletion.</param>
        /// <returns>Results of deletion.</returns>
        public Task<DeletionResult> DestroyAsync(DeletionParams parameters)
        {
            return CallAsync(Destroy, parameters);
        }

        /// <summary>
        /// Generate an image of a given textual string asynchronously.
        /// </summary>
        /// <param name="parameters">Parameters of generating an image of a given textual string</param>
        /// <returns>Results of generating an image of a given textual string</returns>
        public Task<TextResult> TextAsync(TextParams parameters)
        {
            return CallAsync(Text, parameters);
        }

        /// <summary>
        /// Manage tag assignments asynchronously.
        /// </summary>
        /// <param name="parameters">Parameters of tag management</param>
        /// <returns>Results of tags management</returns>
        public Task<TagResult> TagAsync(TagParams parameters)
        {
            return CallAsync(Tag, parameters);
        }

        /// <summary>
        /// Lists resources asynchronously.
        /// </summary>
        /// <param name="parameters">The parameters.</param>
        public Task<ListResourcesResult> ListResourcesAsync(ListResourcesParams parameters)
        {
            return CallAsync(ListResources, parameters);
        }

        /// <summary>
        /// Gets a list of tags asynchronously.
        /// </summary>
        /// <param name="parameters"></param>
        /// <returns></returns>
        public Task<ListTagsResult> ListTagsAsync(ListTagsParams parameters)
        {
            return CallAsync(ListTags, parameters);
        }

        /// <summary>
        /// Gets a list of transformations asynchronously.
        /// </summary>
        /// <param name="parameters">Parameters of the request for a list of transformation.</param>
        /// <returns>Parsed list of transformations details.</returns>
        public Task<ListTransformsResult> ListTransformationsAsync(ListTransformsParams parameters)
        {
            return CallAsync(ListTransformations, parameters);
        }

        /// <summary>
        /// Gets details of a single transformation asynchronously.
        /// </summary>
        /// <param name="parameters">Parameters of the request of transformation details.</param>
        /// <returns>Parsed details of a single transformation.</returns>
        public Task<GetTransformResult> GetTransformAsync(GetTransformParams parameters)
        {
            return CallAsync(GetTransform, parameters);
        }

        /// <summary>
        /// Updates details of an existing resource asynchronously.
        /// </summary>
        /// <param name="parameters">Parameters to update details of an existing resource.</param>
        /// <returns>Parsed response of the detailed resource information.</returns>
        public Task<GetResourceResult> UpdateResourceAsync(UpdateParams parameters)
        {
            return CallAsync(UpdateResource, parameters);
        }

        /// <summary>
        /// Gets details of the requested resource as well as all its derived resources asynchronously.
        /// </summary>
        /// <param name="parameters">Parameters of the request of resource.</param>
        /// <returns>Parsed response with the detailed resource information.</returns>
        public Task<GetResourceResult> GetResourceAsync(GetResourceParams parameters)
        {
            return CallAsync(GetResource, parameters);
        }

        /// <summary>
        /// Deletes all derived resources with the given parameters asynchronously.
        /// </summary>
        /// <param name="parameters">Parameters to delete derived resources.</param>
        /// <returns>Parsed result of deletion derived resources.</returns>
        public Task<DelDerivedResResult> DeleteDerivedResourcesAsync(DelDerivedResParams parameters)
        {
            return CallAsync(DeleteDerivedResources, parameters);
        }

        /// <summary>
        /// Deletes all resources with parameters asynchronously.
        /// </summary>
        /// <param name="parameters">Parameters for deletion resources.</param>
        /// <returns>Parsed result of deletion resources.</returns>
        public Task<DelResResult> DeleteResourcesAsync(DelResParams parameters)
        {
            return CallAsync(DeleteResources, parameters);
        }

        /// <summary>
        /// Updates Cloudinary transformation resource asynchronously.
        /// </summary>
        /// <param name="parameters">Parameters for transformation update.</param>
        /// <returns>Parsed response after transformation manipulation.</returns>
        public Task<UpdateTransformResult> UpdateTransformAsync(UpdateTransformParams parameters)
        {
            return CallAsync(UpdateTransform, parameters);
        }

        /// <summary>
        /// Creates Cloudinary transformation resource asynchronously.
        /// </summary>
        /// <param name="parameters">Parameters of the new transformation.</param>
        /// <returns>Parsed response after transformation manipulation.</returns>
        public Task<TransformResult> CreateTransformAsync(CreateTransformParams parameters)
        {
            return CallAsync(CreateTransform, parameters);
        }

        /// <summary>
        /// Eagerly generate sprites asynchronously.
        /// </summary>
        /// <param name="parameters">Parameters for sprite generation.</param>
        /// <returns>Result of sprite generation.</returns>
        public Task<SpriteResult> MakeSpriteAsync(SpriteParams parameters)
        {
            return CallAsync(MakeSprite, parameters);
        }

        /// <summary>
        /// Allows multi transformation
        /// </summary>
        /// <param name="parameters">Parameters of operation</param>
        /// <returns>Result of operation</returns>
        public Task<MultiResult> MultiAsync(MultiParams parameters)
        {
            return CallAsync(Multi, parameters);
        }

        /// <summary>
        /// Explode multipage document to single pages
        /// </summary>
        /// <param name="parameters">Parameters of explosion</param>
        /// <returns>Result of operation</returns>
        public Task<ExplodeResult> ExplodeAsync(ExplodeParams parameters)
        {
            return CallAsync(Explode, parameters);
        }

        /// <summary>
        /// Gets the Cloudinary account usage details asynchronously.
        /// </summary>
        /// <returns>Result of operation</returns>
        public Task<UsageResult> GetUsageAsync()
        {
            return Task.Factory.StartNew<UsageResult>(GetUsage);
        }

        private Task<TRes> CallAsync<TParams, TRes>(Func<TParams, TRes> f, TParams @params)
        {
            return Task.Factory.StartNew((object o) =>
            {
                return f(@params);
            }, @params);
        }
    }
}

