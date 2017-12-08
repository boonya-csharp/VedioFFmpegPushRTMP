namespace FFmpegSharp.Filters
{
    public class VideoBitrateFilter : FilterBase
    {
        public float Bitrate { get; private set; }

        /// <summary>
        /// set frame rate
        /// </summary>
        /// <param name="bitrate">Hz value, fraction or abbreviation(eg. 64k)</param>
        public VideoBitrateFilter(float bitrate)
        {
            Name = "VideoBitrate";
            FilterType = FilterType.Video;
            Bitrate = bitrate;
            Rank = 6;
        }

        public override string ToString()
        {
            return string.Concat(" -b:v ", Bitrate);
        }
    }
}