using System;
using System.IO;
using System.Reflection;

namespace FFmpegSharp
{
    /// <summary>
    /// base config
    /// </summary>
    public sealed class Config
    {
        /// <summary>
        /// the path for ffmpeg.exe
        ///  </summary>
        /// <remarks>
        /// default:{Host runtime Directory}/external/ffmpeg/{OS architecture(for 32bit is x86,for 64bit is x64)}/ffmpeg.exe
        /// </remarks>
        public string FFmpegPath { get; set; }

        /// <summary>
        /// the path for ffprobe.exe
        ///  </summary>
        /// <remarks>
        /// default:{Host runtime Directory}/external/ffmpeg/{OS architecture(for 32bit is x86,for 64bit is x64)}/ffprobe.exe
        /// </remarks>
        public string FFprobePath { get; set; }



        /// <summary>
        /// FFplayPath路径读取
        /// </summary>
        public string FFplayPath { get; set; }

        private Config()
        {
            var currentDir = new FileInfo(Uri.UnescapeDataString(new Uri(Assembly.GetExecutingAssembly().CodeBase).AbsolutePath));
            //var appPath = currentDir.DirectoryName;
            var arch = Environment.Is64BitOperatingSystem ? "x64" : "x86";
            FFmpegPath = Path.Combine(currentDir.DirectoryName, "external", "ffmpeg", arch, "ffmpeg.exe");
            FFprobePath = Path.Combine(currentDir.DirectoryName, "external", "ffmpeg", arch, "ffprobe.exe");
            FFplayPath = Path.Combine(currentDir.DirectoryName, "external", "ffmpeg", arch, "ffplay.exe");
        }

        /// <summary>
        /// The single instance for FFmpegConfg
        /// </summary>
        public static Config Instance
        {
            get { return ConfigInstance.instance; }
        }

        /// <summary>
        /// default output directory path;
        /// </summary>
        /// <remarks>
        /// it will not work when it's null or empty
        /// </remarks>
        public string OutputPath { get; set; }

        /// <summary>
        /// nested class for single instance
        /// </summary>
        class ConfigInstance
        {
            internal static readonly Config instance = new Config();

            static ConfigInstance()
            {

            }
        }
    }
}