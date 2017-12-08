using System;

namespace FFmpegSharp.Media
{
    public class StreamInfoBase
    {
        public string CodecName { get; set; }
        public TimeSpan Duration { get; set; }
    }
}