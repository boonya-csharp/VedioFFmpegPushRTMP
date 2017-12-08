namespace FFmpegSharp.Filters
{
    public class VideoRateFilter : FilterBase
    {
        public int Rate { get; private set; }

        /// <summary>
        /// set frame rate
        /// </summary>
        /// <param name="rate">Hz value, fraction or abbreviation</param>
        public VideoRateFilter(int rate)
        {
            Name = "VideoRate";
            FilterType = FilterType.Video;
            Rate = rate;
            Rank = 6;
        }

        public override string ToString()
        {
            return string.Concat(" -r ", Rate);
        }
    }
}