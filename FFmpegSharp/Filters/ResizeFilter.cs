using System.Collections.Generic;
using System.Drawing;
using FFmpegSharp.Utils;

namespace FFmpegSharp.Filters
{
    /// <summary>
    /// video resize filter
    /// </summary>
    public class ResizeFilter : FilterBase
    {
        private static readonly Dictionary<Resolution, Size> Sizes = new Dictionary<Resolution, Size>
        {
            {Resolution.X240P, new Size(424, 240)},
            {Resolution.X360P, new Size(640, 360)},
            {Resolution.X480P, new Size(848, 480)},
            {Resolution.X720P, new Size(1280, 720)},
            {Resolution.X1080P, new Size(1920, 1080)},
        };

        public Resolution Resolution { get; private set; }


        public ResizeFilter(Resolution resolution = Resolution.X480P)
        {
            Name = "Resize";
            FilterType = FilterType.Video;
            Resolution = resolution;

        }

        public override string ToString()
        {
            var size = Sizes[Resolution];

            return string.Format(" -s {0}x{1} ", size.Width, size.Height);
        }
    }
}