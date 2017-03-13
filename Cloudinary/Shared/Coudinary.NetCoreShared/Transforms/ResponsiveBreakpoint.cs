using Newtonsoft.Json.Linq;

namespace CloudinaryDotNet
{
    /// <summary>
    /// Allows to generate images for your responsive website in various width dimensions, 
    /// and to define the minimum file size step (performance budget)
    /// </summary>
    public class ResponsiveBreakpoint : JObject
    {
        private const string CREATE_DERIVED = "create_derived";
        private const string TRANSFORMATION = "transformation";
        private const string MAX_WIDTH = "max_width";
        private const string MIN_WIDTH = "min_width";
        private const string BYTES_STEP = "bytes_step";
        private const string MAX_IMAGES = "max_images";

        public ResponsiveBreakpoint()
        {
            Add(CREATE_DERIVED, true);
        }

        /// <summary>
        /// Get value of the create_derived flag
        /// </summary>
        public bool IsCreateDerived()
        {
            return GetValue(CREATE_DERIVED).Value<bool>();
        }

        /// <summary>
        /// Set create_derived flag.
        /// If create_derived flag is enabled, the derived images don't need to be regenerated when first accessed by your users
        /// </summary>
        public ResponsiveBreakpoint CreateDerived(bool createDerived)
        {
            this[CREATE_DERIVED] = createDerived;
            return this;
        }

        /// <summary>
        /// Set transformation to apply on the original image
        /// </summary>
        public ResponsiveBreakpoint Transformation(Transformation transformation)
        {
            this[TRANSFORMATION] = transformation.ToString();
            return this;
        }

        /// <summary>
        /// Get maximal width in pixels
        /// </summary>
        /// <returns></returns>
        public int MaxWidth()
        {
            return Value<int>(MAX_WIDTH);
        }

        /// <summary>
        /// Set maximal boundary of Width
        /// </summary>
        /// <param name="maxWidth">Maximal width in pixels</param>
        /// <returns></returns>
        public ResponsiveBreakpoint MaxWidth(int maxWidth)
        {
            this[MAX_WIDTH] = maxWidth;
            return this;
        }

        /// <summary>
        /// Get minimal Width in pixels
        /// </summary>
        public int MinWidth()
        {
            return Value<int>(MIN_WIDTH);
        }

        /// <summary>
        /// Set minimal boundary of Width
        /// </summary>
        /// <param name="minWidth">Minimal width in pixels</param>
        public ResponsiveBreakpoint MinWidth(int minWidth)
        {
            this[MIN_WIDTH] = minWidth;
            return this;
        }

        /// <summary>
        /// Get minimal file size step
        /// </summary>
        public int BytesStep()
        {
            return Value<int>(BYTES_STEP);
        }

        /// <summary>
        /// Set minimal file size step to generate images
        /// </summary>
        /// <param name="bytesStep">File size step in bytes</param>
        /// <returns></returns>
        public ResponsiveBreakpoint BytesStep(int bytesStep)
        {
            this[BYTES_STEP] = bytesStep;
            return this;
        }

        /// <summary>
        /// Get maximal number of images to generate
        /// </summary>
        public int MaxImages()
        {
            return Value<int>(MAX_IMAGES);
        }

        /// <summary>
        /// Set maximum number of images to generate
        /// </summary>
        public ResponsiveBreakpoint MaxImages(int maxImages)
        {
            this[MAX_IMAGES] = maxImages;
            return this;
        }
    }
}
