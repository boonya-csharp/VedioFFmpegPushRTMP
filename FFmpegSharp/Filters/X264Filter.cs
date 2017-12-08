/*
 * resource:
 * https://trac.ffmpeg.org/wiki/Encode/H.264
 * http://mewiki.project357.com/wiki/X264_Settings
 */
using System.Text;

namespace FFmpegSharp.Filters
{
    public class X264Filter : FilterBase
    {
        /// <summary>
        /// change options to trade off compression efficiency against encoding speed.
        /// default: Medium
        /// </summary>
        public X264Preset Preset { get; set; }
        /// <summary>
        /// 编码X264
        /// set x264 to encode the movie in Constant Quantizer mode.
        /// range 0-54.
        /// a setting of 0 will produce lossless output.
        /// usually 21-26
        /// </summary>
        public int? ConstantQuantizer { get; set; }
        public int? MaxRate { get; set; }
        public int? MinRate { get; set; }

        //todo -movflags faststart

        public X264Filter()
        {
            Name = "X264";
            FilterType = FilterType.Video;
            Preset = X264Preset.Medium;
            Rank = 1;
        }

        public override string ToString()
        {
            var builder = new StringBuilder();

            builder.Append(" -c:v libx264");

            if (X264Preset.Medium != Preset)
            {
                var param = Preset.GetDescription();

                if (!string.IsNullOrWhiteSpace(param))
                {
                    builder.AppendFormat(" -preset {0}", param);
                }
            }

            if (null != ConstantQuantizer)
            {
                if (ConstantQuantizer < 0 || ConstantQuantizer > 51)
                {
                    ConstantQuantizer = 22;
                }

                builder.AppendFormat(" -qp {0}", ConstantQuantizer.Value);
            }

            return builder.ToString();
        }
    }
}