namespace CloudinaryDotNet
{
    /// <summary>
    /// Represents property of the overlay parameter ( l_subtitles: in URLs), 
    /// followed by the SRT file name (including the .srt extension).
    /// </summary>
    public class SubtitlesLayer : TextLayer
    {
        public SubtitlesLayer()
        {
            m_resourceType = "subtitles";
        }
    }
}
