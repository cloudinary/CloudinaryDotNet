namespace CloudinaryDotNet
{
    /// <summary>
    /// Represent cloudinary configuration object.
    /// </summary>
    public static  class CloudinaryConfiguration
    {
        //General configuration
        public static string CloudName = string.Empty;
        public static string ApiKey = string.Empty;
        public static string ApiSecret = string.Empty;

        //Auth token configuration
        public static AuthToken AuthToken = null;

        //Tests configuration
        public static bool IsTestMode = false;
        public static string TestTagName = string.Empty;
    }
}
