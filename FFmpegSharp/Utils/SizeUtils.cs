using System.Drawing;

namespace FFmpegSharp.Utils
{
    internal class SizeUtils
    {
        /// <summary>
        /// calculate out size in scale
        /// </summary>
        /// <param name="source">source size</param>
        /// <param name="maxWidth">max width</param>
        /// <param name="maxHeight">max height</param>
        /// <returns></returns>
        internal static Size CalculateOutSize(Size source, int maxWidth, int maxHeight)
        {
            var result = new Size();

            double sourceScale = (double)source.Width / source.Height;

            double calculatedScale = (double)maxWidth / maxHeight;

            if (sourceScale < calculatedScale)
            {
                result.Width = (source.Width * maxHeight) / source.Height;
                result.Height = maxHeight;
            }
            else
            {
                result.Width = maxWidth;
                result.Height = (source.Height * maxWidth) / source.Width;
            }

            return result;
        }  
    }
}