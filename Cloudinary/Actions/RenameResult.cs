using System.Net;

namespace CloudinaryDotNet.Actions
{
    /// <summary>
    /// Results of resource renaming
    /// </summary>
    public class RenameResult : GetResourceResult
    {
        /// <summary>
        /// Parses HTTP response and creates new instance of this class
        /// </summary>
        /// <param name="response">HTTP response</param>
        /// <returns>New instance of this class</returns>
        internal static new RenameResult Parse(HttpWebResponse response)
        {
            return Parse<RenameResult>(response);
        }
    }
}
