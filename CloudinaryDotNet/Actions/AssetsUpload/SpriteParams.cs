namespace CloudinaryDotNet.Actions
{
    using System.Collections.Generic;

    /// <summary>
    /// Parameters of create sprite request.
    /// </summary>
    public class SpriteParams : MultiAssetsParams
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="SpriteParams"/> class.
        /// </summary>
        /// <param name="tag">The tag name assigned to images that we should merge into the sprite.</param>
        public SpriteParams(string tag)
            : base(tag)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="SpriteParams"/> class.
        /// </summary>
        /// <param name="urls">The sprite is created from all images with the urls specified.</param>
        public SpriteParams(List<string> urls)
            : base(urls)
        {
        }
    }
}
