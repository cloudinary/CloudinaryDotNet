namespace CloudinaryDotNet
{
    using System.Threading;
    using System.Threading.Tasks;
    using CloudinaryDotNet.Actions;

    /// <summary>
    /// Part of Cloudinary .NET API main class, responsible for media analysis.
    /// </summary>
    public partial class Cloudinary
    {
        /// <summary>
        ///  Analyzes an asset with the requested analysis type asynchronously.
        /// </summary>
        /// <param name="parameters">Parameters of analysis.</param>
        /// <param name="cancellationToken">(Optional) Cancellation token.</param>
        /// <returns>Detailed analysis information.</returns>
        public Task<AnalyzeResult> AnalyzeAsync(AnalyzeParams parameters, CancellationToken? cancellationToken = null)
        {
            var url = m_api.ApiUrlV2.Add(Constants.ANALYSIS).Add(Constants.ANALYZE).Add(Constants.URI).BuildUrl();

            return CallAdminApiJsonAsync<AnalyzeResult>(HttpMethod.POST, url, parameters, cancellationToken);
        }

        /// <summary>
        ///  Analyzes an asset with the requested analysis type.
        /// </summary>
        /// <param name="parameters">Parameters of analysis.</param>
        /// <returns>Detailed analysis information .</returns>
        public AnalyzeResult Analyze(AnalyzeParams parameters) => AnalyzeAsync(parameters).GetAwaiter().GetResult();
    }
}
