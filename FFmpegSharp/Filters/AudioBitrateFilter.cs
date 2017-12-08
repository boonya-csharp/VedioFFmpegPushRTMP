namespace FFmpegSharp.Filters
{
    /// <summary>
    /// audio bitrate filter
    /// </summary>
    public class AudioBitrateFilter : FilterBase
    {
        public int Rate { get; private set; }

        /// <summary>
        /// audio bitrate
        /// </summary>
        /// <param name="rate">(eg. 64k 96k 128k 192k 320k)</param>
        public AudioBitrateFilter(int rate)
        {
            Name = "AudioBitrate";
            FilterType = FilterType.Audio;
            Rate = rate;
            Rank = 8;
        }

        public override string ToString()
        {
            if (Rate > 320)
                Rate = 320;

            if (Rate < 64)
                Rate = 64;

            return string.Format(" -b:a {0}k", Rate);
        }
    }
}