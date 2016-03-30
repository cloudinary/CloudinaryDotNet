using Newtonsoft.Json.Linq;

namespace CloudinaryDotNet
{
    /// <summary>
    /// Allows to generate images for your responsive website in various width dimensions, 
    /// and to define the minimum file size step (performance budget)
    /// </summary>
    public class ResponsiveBreakpoint : JObject
    {
        public ResponsiveBreakpoint()
        {
            Add("create_derived", true);
        }

        /// <summary>
        /// Get value of the create_derived flag
        /// </summary>
        public bool IsCreateDerived()
        {
            return GetValue("create_derived").Value<bool>();
        }

        /// <summary>
        /// Set create_derived flag.
        /// If create_derived flag is enabled, the derived images don't need to be regenerated when first accessed by your users
        /// </summary>
        public ResponsiveBreakpoint CreateDerived(bool createDerived)
        {
            this["create_derived"] = createDerived;
            return this;
        }

        /// <summary>
        /// Set transformation to apply on the original image
        /// </summary>
        public ResponsiveBreakpoint Transformation(Transformation transformation)
        {
            this["transformation"] = transformation.ToString();
            return this;
        }

        /// <summary>
        /// Get maximal width in pixels
        /// </summary>
        /// <returns></returns>
        public int GetMaxWidth()
        {
            return Value<int>("max_width");
        }

        /// <summary>
        /// Maximal boundary of Width
        /// </summary>
        /// <param name="maxWidth">Maximal width in pixels</param>
        /// <returns></returns>
        public ResponsiveBreakpoint MaxWidth(int maxWidth)
        {
            this["max_width"] = maxWidth;
            return this;
        }

        /// <summary>
        /// Get minimal Width in pixels
        /// </summary>
        public int GetMinWidth()
        {
            return Value<int>("min_width");
        }

        /// <summary>
        /// Minimal boundary of Width
        /// </summary>
        /// <param name="minWidth">Minimal width in pixels</param>
        public ResponsiveBreakpoint MinWidth(int minWidth)
        {
            this["min_width"] = minWidth;
            return this;
        }

        /// <summary>
        /// Get minimal file size step
        /// </summary>
        public int BytesStep()
        {
            return Value<int>("bytes_step");
        }

        /// <summary>
        /// Minimal file size step to generate images
        /// </summary>
        /// <param name="bytesStep">File size step in bytes</param>
        /// <returns></returns>
        public ResponsiveBreakpoint BytesStep(int bytesStep)
        {
            this["bytes_step"] = bytesStep;
            return this;
        }

        /// <summary>
        /// Get maximum number of images to generate
        /// </summary>
        public int MaxImages()
        {
            return Value<int>("max_images");
        }

        /// <summary>
        /// Maximum number of images to generate
        /// </summary>
        public ResponsiveBreakpoint MaxImages(int maxImages)
        {
            this["max_images"] = maxImages;
            return this;
        }
    }
}
