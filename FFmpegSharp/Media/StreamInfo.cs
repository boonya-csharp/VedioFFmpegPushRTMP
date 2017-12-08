using System;
using Newtonsoft.Json;

namespace FFmpegSharp.Media
{
    [JsonObject]
    internal class StreamInfo
    {
        [JsonProperty("codec_long_name")]
        public string CodecName { get; set; }
        [JsonProperty("width")]
        public int Width { get; set; }
        [JsonProperty("height")]
        public int Height { get; set; }
        [JsonProperty("codec_type")]
        public string Type { get; set; }
        [JsonProperty("duration")]
        public TimeSpan Duration { get; set; }
        [JsonProperty("channels")]
        public int Channels { get; set; }
    } 
}