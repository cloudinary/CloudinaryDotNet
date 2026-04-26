namespace CloudinaryDotNet.Actions
{
    using System;
    using System.Collections.Generic;

    /// <summary>
    /// Base parameters of file upload request.
    /// </summary>
    public class BasicRawUploadParams : BaseParams
    {
        /// <summary>
        /// Gets or sets either the actual data of the image or an HTTP URL of a public image on the Internet.
        /// </summary>
        public FileDescription File { get; set; }

        /// <summary>
        /// Gets or sets the identifier that is used for accessing the uploaded resource.
        /// A randomly generated ID is assigned if not specified.
        /// </summary>
        public string PublicId { get; set; }

        /// <summary>
        /// Gets or sets the identifier that is used to provide context and to improve the SEO of an assets filename in the delivery URL.
        /// It does not impact the location where the asset is stored.
        /// </summary>
        public string PublicIdPrefix { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether to backup the uploaded file. Overrides the default backup settings of your account.
        /// </summary>
        public bool? Backup { get; set; }

        /// <summary>
        /// Gets or sets privacy mode of the file. Valid values: 'upload' and 'authenticated'. Default: 'upload'.
        /// </summary>
        public string Type { get; set; }

        /// <summary>
        /// Gets the 'raw' type of file you are uploading.
        /// </summary>
        public virtual ResourceType ResourceType => ResourceType.Raw;

        /// <summary>
        /// Gets or sets file name to override an original file name.
        /// </summary>
        public string FilenameOverride { get; set; }

        /// <summary>
        /// Gets or sets upload timestamp.
        /// </summary>
        public DateTime Timestamp { get; set; }

        /// <summary>
        /// Gets or sets upload signature.
        /// </summary>
        public string Signature { get; set; }

        /// <summary>
        /// Gets or sets unique upload ID.
        /// </summary>
        public string UniqueUploadId { get; set; }

        /// <summary>
        /// Gets or sets the 1-based part number for this chunk in explicit-order chunked uploads.
        /// When set, the SDK sends the X-Upload-Part-Number header and skips Content-Range,
        /// allowing chunks of uneven sizes. Requires <see cref="UniqueUploadId"/>.
        /// </summary>
        public int? PartNumber { get; set; }

        /// <summary>
        /// Gets or sets the total number of parts for this upload. Required when
        /// <see cref="PartNumber"/> is set; must be the same value on every chunk for the
        /// same upload.
        /// </summary>
        public int? TotalParts { get; set; }

        /// <summary>
        /// Validate object model.
        /// </summary>
        public override void Check()
        {
            if (File == null)
            {
                throw new ArgumentException("File must be specified in UploadParams!");
            }

            if (!File.Chunked && !File.IsRemote && File.Stream == null && string.IsNullOrEmpty(File.FilePath))
            {
                throw new ArgumentException("File is not ready!");
            }

            if (string.IsNullOrEmpty(File.FileName))
            {
                throw new ArgumentException("File name must be specified in UploadParams!");
            }

            if (PartNumber.HasValue && string.IsNullOrEmpty(UniqueUploadId))
            {
                throw new ArgumentException("UniqueUploadId is required when PartNumber is set.");
            }

            if (TotalParts.HasValue && !PartNumber.HasValue)
            {
                throw new ArgumentException("PartNumber is required when TotalParts is set.");
            }

            if (PartNumber.HasValue && PartNumber.Value < 1)
            {
                throw new ArgumentException("PartNumber must be >= 1.");
            }

            if (TotalParts.HasValue && (TotalParts.Value < 1 || TotalParts.Value > 10000))
            {
                throw new ArgumentException("TotalParts must be between 1 and 10000.");
            }

            if (PartNumber.HasValue && TotalParts.HasValue && PartNumber.Value > TotalParts.Value)
            {
                throw new ArgumentException("PartNumber cannot exceed TotalParts.");
            }
        }

        /// <summary>
        /// Map object model to dictionary of parameters in cloudinary notation.
        /// </summary>
        /// <returns>Sorted dictionary of parameters.</returns>
        public override SortedDictionary<string, object> ToParamsDictionary()
        {
            var dict = base.ToParamsDictionary();

            AddParam(dict, "file", File);
            AddParam(dict, "public_id", PublicId);
            AddParam(dict, "public_id_prefix", PublicIdPrefix);
            AddParam(dict, "type", Type);
            AddParam(dict, "filename_override", FilenameOverride);
            if (Timestamp != DateTime.MinValue)
            {
                AddParam(dict, "timestamp", Utils.ToUnixTimeSeconds(Timestamp));
            }

            AddParam(dict, "signature", Signature);

            if (Backup.HasValue)
            {
                AddParam(dict, "backup", Backup.Value);
            }

            return dict;
        }
    }
}
