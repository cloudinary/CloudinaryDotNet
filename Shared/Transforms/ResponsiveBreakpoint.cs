using Newtonsoft.Json.Linq;

namespace CloudinaryDotNet
{
    /// <summary>
    /// Allows to generate images for your responsive website in various width dimensions, 
    /// and to define the minimum file size step (performance budget).
    /// </summary>
    public class ResponsiveBreakpoint : JObject
    {
        private const string CREATE_DERIVED = "create_derived";
        private const string TRANSFORMATION = "transformation";
        private const string MAX_WIDTH = "max_width";
        private const string MIN_WIDTH = "min_width";
        private const string BYTES_STEP = "bytes_step";
        private const string MAX_IMAGES = "max_images";
        private const string FORMAT = "format";

        /// <summary>
        /// Instantiates the <see cref="ResponsiveBreakpoint"/> object.
        /// </summary>
        public ResponsiveBreakpoint()
        {
            Add(CREATE_DERIVED, true);
        }

        /// <summary>
        /// Get value of the create_derived flag.
        /// </summary>
        public bool IsCreateDerived()
        {
            return GetValue(CREATE_DERIVED).Value<bool>();
        }

        /// <summary>
        /// Set create_derived flag.
        /// If true, create and keep the derived images of the selected breakpoints during the API call. 
        /// If false, images generated during the analysis process are thrown away.
        /// </summary>
        public ResponsiveBreakpoint CreateDerived(bool createDerived)
        {
            this[CREATE_DERIVED] = createDerived;
            return this;
        }

        /// <summary>
        /// The base transformation to first apply to the image before finding the best breakpoints.
        /// </summary>
        public ResponsiveBreakpoint Transformation(Transformation transformation)
        {
            this[TRANSFORMATION] = transformation.ToString();
            return this;
        }

        /// <summary>
        /// Get maximal width in pixels
        /// </summary>
        public int MaxWidth()
        {
            return Value<int>(MAX_WIDTH);
        }

        /// <summary>
        /// The maximum width needed for this image. If specifying a width bigger than the original image, the width
        /// of the original image is used instead. Default: 1000.
        /// </summary>
        /// <param name="maxWidth">Maximum width in pixels.</param>
        public ResponsiveBreakpoint MaxWidth(int maxWidth)
        {
            this[MAX_WIDTH] = maxWidth;
            return this;
        }

        /// <summary>
        /// Get minimum width in pixels.
        /// </summary>
        public int MinWidth()
        {
            return Value<int>(MIN_WIDTH);
        }

        /// <summary>
        /// Set the minimum width needed for this image. Default: 50.
        /// </summary>
        /// <param name="minWidth">Minimum width in pixels.</param>
        public ResponsiveBreakpoint MinWidth(int minWidth)
        {
            this[MIN_WIDTH] = minWidth;
            return this;
        }

        /// <summary>
        /// Get minimum number of bytes between two consecutive breakpoints (images).
        /// </summary>
        public int BytesStep()
        {
            return Value<int>(BYTES_STEP);
        }

        /// <summary>
        /// Set the minimum number of bytes between two consecutive breakpoints (images). Default: 20000.
        /// </summary>
        /// <param name="bytesStep">File size step in bytes</param>
        /// <returns></returns>
        public ResponsiveBreakpoint BytesStep(int bytesStep)
        {
            this[BYTES_STEP] = bytesStep;
            return this;
        }

        /// <summary>
        /// Get maximum number of breakpoints(images) to find.
        /// </summary>
        public int MaxImages()
        {
            return Value<int>(MAX_IMAGES);
        }

        /// <summary>
        /// Set the maximum number of breakpoints to find, between 3 and 200. This means that there might be size
        /// differences bigger than the given bytes_step value between consecutive images. Default: 20.
        /// </summary>
        public ResponsiveBreakpoint MaxImages(int maxImages)
        {
            this[MAX_IMAGES] = maxImages;
            return this;
        }

        /// <summary>
        /// Get the file extension of the derived resources to the format indicated.
        /// </summary>
        public string Format()
        {
            return Value<string>(FORMAT);
        }

        /// <summary>
        /// Sets the file extension of the derived resources to the format indicated.
        /// </summary>
        public ResponsiveBreakpoint Format(string format)
        {
            this[FORMAT] = format;
            return this;
        }
    }
}
