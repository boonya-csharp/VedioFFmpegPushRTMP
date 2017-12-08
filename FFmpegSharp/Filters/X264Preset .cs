using System.ComponentModel;

namespace FFmpegSharp.Filters
{
    public enum X264Preset 
    {
        [Description("ultrafast")]
        Ultrafast,
        [Description("superfast")]
        Superfast,
        [Description("veryfast")]
        Veryfast,
        [Description("faster")]
        Faster,
        [Description("fast")]
        Fast,
        [Description("medium")]
        Medium,
        [Description("slow")]
        Slow,
        [Description("slower")]
        Slower,
        [Description("veryslow")]
        Veryslow,
        [Description("placebo")]
        Placebo
    }
}