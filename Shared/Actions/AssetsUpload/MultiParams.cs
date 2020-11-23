namespace CloudinaryDotNet.Actions
{
    using System.Collections.Generic;

    /// <summary>
    /// Parameters of request to create a single animated GIF file from a group of images.
    /// </summary>
    public class MultiParams : MultiAssetsParams
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="MultiParams"/> class.
        /// </summary>
        /// <param name="tag">The animated GIF is created from all images with this tag.</param>
        public MultiParams(string tag)
            : base(tag)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="MultiParams"/> class.
        /// </summary>
        /// <param name="urls">The animated GIF is created from all images with the urls specified.</param>
        public MultiParams(List<string> urls)
            : base(urls)
        {
        }
    }
}
