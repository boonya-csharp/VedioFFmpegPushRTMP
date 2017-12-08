using System;

namespace FFmpegSharp.Filters
{
    /// <summary>
    /// audio channel select filter
    /// </summary>
    public class AudioChannelFilter : FilterBase
    {
        public int Channel { get; private set; }

        public AudioChannelFilter(int channel)
        {
            Name = "AudioChannel";
            FilterType = FilterType.Audio;
            Channel = channel;
            Rank = 8;
        }

        public override string ToString()
        {
            if(Channel > Source.AudioInfo.Channels)
                throw new ApplicationException(string.Format("there only {0} channels in audio stream.", Source.AudioInfo.Channels));

            return string.Concat(" -ac ", Channel);
        }
    }
}