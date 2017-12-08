using System;
using System.Drawing;
using System.IO;
using System.Text;
using FFmpegSharp.Utils;

namespace FFmpegSharp.Filters
{
    public class SnapshotFilter : FilterBase
    {
        public int? Number { get; private set; }
        public TimeSpan? Time { get; private set; }
        public string OutputPath { get; private set; }
        public int Width { get; private set; }
        public int Height { get; private set; }
        public ResizeType ResizeType { get; private set; }

        /// <summary>
        /// create multiple snapshot.
        /// </summary>
        /// <param name="outputPath"></param>
        /// <param name="width"></param>
        /// <param name="height"></param>
        /// <param name="number">number of snapshot</param>
        /// <param name="type"></param>
        public SnapshotFilter(string outputPath, int width,int height, int number = 1, ResizeType type = ResizeType.Scale)
        {
            
            Name = "Snapshot";
            FilterType = FilterType.Video;
            OutputPath = outputPath;
            Width = width;
            Height = height;
            Rank = 6;
            ResizeType = type;
            if (number <= 1)
            {
                Number = null;
                Time = new TimeSpan(0, 0, 0, 1);
            }
            else
            {
                Number = number;
                Time = null;  
            }
        }

        /// <summary>
        /// create single snapshot at the fixed time.
        /// </summary>
        /// <param name="outputPath"></param>
        /// <param name="width"></param>
        /// <param name="height"></param>
        /// <param name="time"></param>
        /// <param name="type"></param>
        public SnapshotFilter(string outputPath, int width, int height, TimeSpan time, ResizeType type = ResizeType.Scale)
        {
            Name = "Snapshot";
            FilterType = FilterType.Video;
            OutputPath = outputPath;
            Width = width;
            Height = height;
            Time = time;
            ResizeType = type;
            Number = null;
            Rank = 6;
        }


        public override string ToString()
        {
            //ffmpeg -i test.mp4  -f image2 -ss 00:00:14.435 -s 960*550 -vframes 1 out.jpg
            //ffmpeg -i test.mp4 -ss 00:00:14.435 -s 960*550 -f image2 -vframes 1 out.jpg
            //ffmpeg -i input.flv -f image2 -vf fps=fps=1 out%d.png

            var builder = new StringBuilder();

            builder.Append(" -i ");
            builder.Append(Source.FilePath);
            builder.Append(" -f image2");

            if (ResizeType == ResizeType.Scale)
            {
                var sourceSize = new Size(Source.VideoInfo.Width, Source.VideoInfo.Height);
                var resultSize = SizeUtils.CalculateOutSize(sourceSize, Width, Height);

                builder.AppendFormat(" -s {0}x{1}", resultSize.Width, resultSize.Height);
            }
            else
            {
                builder.AppendFormat(" -s {0}x{1}", Width, Height);
            }

            var dir = Path.GetDirectoryName(OutputPath);

            if (string.IsNullOrEmpty(dir))
                throw new ApplicationException("snapshot outpath directory is null.");

            if (!Directory.Exists(dir))
            {
                Directory.CreateDirectory(dir);
            }

            var filename = Path.GetFileNameWithoutExtension(OutputPath);

            if (string.IsNullOrEmpty(filename))
                throw new ApplicationException("snapshot outpath filename is null.");

            var ext = Path.GetExtension(OutputPath);

            if (string.IsNullOrEmpty(ext))
                ext = ".jpg";


            if (null != Time)
            {
                builder.AppendFormat(" -ss {0} -vframes 1", Time.Value);
                builder.Append(" ");
                builder.Append(OutputPath);
            }

            if (null != Number)
            {
                var duration = Source.VideoInfo.Duration;

                var num = duration.TotalSeconds/(Number.Value - 1);

                builder.AppendFormat(" -vf fps=fps=1/{0} {1}\\{2}%d{3}", Math.Floor(num), dir, filename, ext);
            }

            return builder.ToString();
        }
    }
}