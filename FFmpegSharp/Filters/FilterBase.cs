using System;
using FFmpegSharp.Media;

namespace FFmpegSharp.Filters
{
    public abstract class FilterBase : IFilter
    {
        public MediaStream Source = null;

        public int Rank { get; protected set; }
        public string Name { get; protected set; }
        protected FilterType FilterType { get; set; }

        protected FilterBase()
        {
            Rank = 9;
        }

        protected virtual void ValidateVideoStream()
        {
            if(null == Source)
                throw new ApplicationException("source file is null.");

            if(null == Source.VideoInfo)
                //视频流
                throw new ApplicationException("non video stream found in source file.");
        }

        protected virtual void ValidateAudioStream()
        {
            if (null == Source)
                throw new ApplicationException("source file is null.");

            if (null == Source.AudioInfo)
                //音频流
                throw new ApplicationException("non audio stream found in source file.");
        }

        public string Execute()
        {
            switch (FilterType)
            {
                case FilterType.Audio:
                    ValidateAudioStream();
                    break;
                case FilterType.Video:
                    ValidateVideoStream();
                    break;
                default:
                    throw new ApplicationException("unknown filter type.");
            }
            return ToString();
        }
    }
}
