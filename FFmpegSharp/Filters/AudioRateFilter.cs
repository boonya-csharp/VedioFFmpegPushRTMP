namespace FFmpegSharp.Filters
{
    /// <summary>
    /// audio rate filter
    /// </summary>
    public class AudioRatelFilter : FilterBase
    {
        public int Rate { get; private set; }

        /// <summary>
        /// audio sampling rate (in Hz)
        /// </summary>
        /// <param name="rate">in flv not above 44100</param>
        public AudioRatelFilter(int rate)
        {
            Name = "AudioRate";
            FilterType = FilterType.Audio;
            Rate = rate;
            Rank = 8;
        }

        public override string ToString()
        {
            return string.Concat(" -ar ", Rate);
        }
    }
}