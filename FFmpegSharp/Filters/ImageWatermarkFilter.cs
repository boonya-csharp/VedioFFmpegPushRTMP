using System;
using System.Drawing;
using System.IO;
using System.Text;

namespace FFmpegSharp.Filters
{
    public class ImageWatermarkFilter : FilterBase
    {
        public string ImageFile { get; private set; }

        public WatermarkPosition Position { get; private set; }
        public Point Offset { get; private set; }

        public ImageWatermarkFilter(string imageFile, WatermarkPosition position, Point offset)
        {
            Name = "ImageWatermark";
            FilterType = FilterType.Video;
            ImageFile = imageFile;
            Position = position;
            Offset = offset;
            Rank = 0;
        }

        public ImageWatermarkFilter(string imageFile, WatermarkPosition position)
        {
            Name = "ImageWatermark";
            FilterType = FilterType.Video;
            ImageFile = imageFile;
            Position = position;
            Offset = new Point(10, 10);
            Rank = 0;
        }

        public override string ToString()
        {
            if (!File.Exists(ImageFile))
                throw new ApplicationException("image file not exists.");

            var builder = new StringBuilder();

            string overlayFormat;

            switch (Position)
            {
                case WatermarkPosition.TopLeft:
                    overlayFormat = "{0}:{1}";
                    break;
                case WatermarkPosition.TopRight:
                    overlayFormat = "main_w-overlay_w-{0}:{1}";
                    break;
                case WatermarkPosition.BottomLeft:
                    overlayFormat = "{0}:main_h-overlay_h-{1}";
                    break;
                case WatermarkPosition.BottomRight:
                    overlayFormat = "main_w-overlay_w-{0}:main_h-overlay_h-{1}";
                    break;
                case WatermarkPosition.Center:
                    overlayFormat = "(main_w-overlay_w)/2-{0}:(main_h-overlay_h)/2-{1}";
                    break;
                case WatermarkPosition.MiddleLeft:
                    overlayFormat = "{0}:(main_h-overlay_h)/2-{1}";
                    break;
                case WatermarkPosition.MiddleRight:
                    overlayFormat = "main_w-overlay_w-{0}:(main_h-overlay_h)/2-{1}";
                    break;
                case WatermarkPosition.CenterTop:
                    overlayFormat = "(main_w-overlay_w)/2-{0}:{1}";
                    break;
                case WatermarkPosition.CenterBottom:
                    overlayFormat = "(main_w-overlay_w)/2-{0}:main_h-overlay_h-{1}";
                    break;

                default:
                    throw new ArgumentException("unknown wartermark position");

            }

            var overlayPostion = String.Format(overlayFormat, Offset.X, Offset.Y);

            builder.AppendFormat(" -vf \"movie=\\'{0}\\' [watermark]; [in][watermark] overlay={1} [out]\"",
                ImageFile.Replace("\\", "\\\\"), overlayPostion);

            return builder.ToString();
        }
    }
}