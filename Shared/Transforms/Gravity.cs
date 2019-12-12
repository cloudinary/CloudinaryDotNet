namespace CloudinaryDotNet
{
    /// <summary>
    /// The gravity values.
    /// </summary>
    public static class Gravity
    {
        /// <summary>
        /// The center of the image. Default.
        /// </summary>
        public const string Center = "center";

        /// <summary>
        /// North west corner (top left).
        /// </summary>
        public const string NorthWest = "north_west";

        /// <summary>
        /// North center part (top center).
        /// </summary>
        public const string North = "north";

        /// <summary>
        /// North east corner (top right).
        /// </summary>
        public const string NorthEast = "north_east";

        /// <summary>
        /// Middle west part (left).
        /// </summary>
        public const string West = "west";

        /// <summary>
        /// Middle east part (right).
        /// </summary>
        public const string East = "east";

        /// <summary>
        /// South west corner (bottom left).
        /// </summary>
        public const string SouthWest = "south_west";

        /// <summary>
        /// South center part (bottom center).
        /// </summary>
        public const string South = "south";

        /// <summary>
        /// South east corner (bottom right).
        /// </summary>
        public const string SouthEast = "south_east";

        /// <summary>
        /// An intelligent algorithm analyzes and prioritizes the most prominent elements of the image to include.
        /// </summary>
        public const string Auto = "auto";

        /// <summary>
        /// Set the crop's center of gravity to the given x and y coordinates.
        /// </summary>
        public const string XYCenter = "xy_center";

        /// <summary>
        /// Automatically detect the largest face in an image and aim to make it the center of the cropped image.
        /// Alternatively, use face coordinates specified by API if available.
        /// Defaults to the 'north' gravity if no face was detected.
        /// </summary>
        public const string Face = "face";

        /// <summary>
        /// Same as the 'face' gravity, but defaults to 'center' gravity instead of 'north' if no face is detected.
        /// </summary>
        public const string FaceCenter = "face:center";

        /// <summary>
        /// Same as the 'face' gravity, but defaults to 'auto' gravity instead of 'north' if no face is detected.
        /// </summary>
        public const string FaceAuto = "face:auto";

        /// <summary>
        /// Automatically detect multiple faces in an image and aim to make them the center of the cropped image.
        /// </summary>
        public const string Faces = "faces";

        /// <summary>
        /// Same as the 'faces' gravity, but defaults to 'center' gravity instead of 'north' if no faces are detected.
        /// </summary>
        public const string FacesCenter = "faces:center";

        /// <summary>
        /// Same as the 'faces' gravity, but defaults to 'auto' gravity instead of 'north' if no faces are detected.
        /// </summary>
        public const string FacesAuto = "faces:auto";

        /// <summary>
        /// Automatically detect the largest body in an image and aim to make it the center of the cropped image.
        /// Defaults to the 'north' gravity if no body was detected.
        /// </summary>
        public const string Body = "body";

        /// <summary>
        /// Automatically detect the largest body in an image and aim to make it the center of the cropped image.
        /// Defaults to the 'face' gravity if a full body was not detected.
        /// </summary>
        public const string BodyFace = "body:face";

        /// <summary>
        /// Use liquid rescaling to change the aspect ratio of an image while retaining all important content and
        /// avoiding unnatural distortions. For more details and guidelines, see liquid gravity.
        /// </summary>
        public const string Liquid = "liquid";

        /// <summary>
        /// Detect all text elements in an image using the `OCR Text Detection and Extraction` add-on and use the
        /// detected bounding box coordinates as the basis of the transformation.
        /// </summary>
        public const string OcrText = "ocr_text";

        /// <summary>
        /// Automatically detect the largest face in an image with the `Advanced Facial Attribute Detection` add-on
        /// and make it the focus of the transformation.
        /// </summary>
        public const string AdvFace = "adv_face";

        /// <summary>
        /// Automatically detect all the faces in an image with the `Advanced Facial Attribute Detection` add-on and
        /// make them the focus of the transformation.
        /// </summary>
        public const string AdvFaces = "adv_faces";

        /// <summary>
        /// Automatically detect the largest pair of eyes in an image with the `Advanced Facial Attribute Detection`
        /// add-on and make them the focus of the transformation.
        /// </summary>
        public const string AdvEyes = "adv_eyes";

        /// <summary>
        /// Use custom coordinates that were specified by the upload or admin API and aim to make it the center
        /// of the cropped image. Defaults to 'center' gravity if no custom coordinates are available.
        /// </summary>
        public const string Custom = "custom";

        /// <summary>
        /// Same as the 'custom' gravity, but defaults to 'face' gravity if no custom coordinates are available.
        /// </summary>
        public const string CustomFace = "custom:face";

        /// <summary>
        /// Same as the 'custom' gravity, but defaults to 'faces' gravity if no custom coordinates are available.
        /// </summary>
        public const string CustomFaces = "custom:faces";

        /// <summary>
        /// Same as the 'custom' gravity, but defaults to 'adv_face' gravity if no custom coordinates are available.
        /// </summary>
        public const string CustomAdvFace = "custom:adv_face";

        /// <summary>
        /// Same as the 'custom' gravity, but defaults to 'adv_faces' gravity if no custom coordinates are available.
        /// </summary>
        public const string CustomAdvFaces = "custom:adv_faces";
    }
}
