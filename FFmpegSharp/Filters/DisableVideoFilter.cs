namespace FFmpegSharp.Filters
{
    /// <summary>
    /// audio channel select filter
    /// </summary>
    public class DisableVideoFilter : FilterBase
    {

        public DisableVideoFilter()
        {
            Name = "DisableVideo";
            FilterType = FilterType.Video;
            Rank = 8;
        }

        public override string ToString()
        {
            return " -vn";
        }
    }
}