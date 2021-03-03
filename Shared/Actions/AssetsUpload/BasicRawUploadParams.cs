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
        public virtual ResourceType ResourceType
        {
            get { return Actions.ResourceType.Raw; }
        }

        /// <summary>
        /// Gets or sets file name to override an original file name.
        /// </summary>
        public string FilenameOverride { get; set; }

        /// <summary>
        /// Validate object model.
        /// </summary>
        public override void Check()
        {
            if (File == null)
            {
                throw new ArgumentException("File must be specified in UploadParams!");
            }

            if (!File.IsRemote && File.Stream == null && string.IsNullOrEmpty(File.FilePath))
            {
                throw new ArgumentException("File is not ready!");
            }

            if (string.IsNullOrEmpty(File.FileName))
            {
                throw new ArgumentException("File name must be specified in UploadParams!");
            }
        }

        /// <summary>
        /// Map object model to dictionary of parameters in cloudinary notation.
        /// </summary>
        /// <returns>Sorted dictionary of parameters.</returns>
        public override SortedDictionary<string, object> ToParamsDictionary()
        {
            var dict = base.ToParamsDictionary();

            AddParam(dict, "public_id", PublicId);
            AddParam(dict, "type", Type);
            AddParam(dict, "filename_override", FilenameOverride);

            if (Backup.HasValue)
            {
                AddParam(dict, "backup", Backup.Value);
            }

            return dict;
        }
    }
}