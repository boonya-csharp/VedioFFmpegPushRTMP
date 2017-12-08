using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using FFmpegSharp.Codes;
using FFmpegSharp.Executor;
using FFmpegSharp.Filters;
using FFmpegSharp.Media;
using System.Diagnostics;

namespace SampleApp
{
    class Program
    {
        static void Main(string[] args)
        {
            var currentDir =
                new FileInfo(Uri.UnescapeDataString(new Uri(Assembly.GetExecutingAssembly().CodeBase).AbsolutePath));
            var appPath = currentDir.DirectoryName;

            if (string.IsNullOrWhiteSpace(appPath))
                throw new ApplicationException("app path not found.");

            var inputPath = Path.Combine(appPath, "Surrender.MP4");//需要推流的视频
            var outputPath = Path.Combine(appPath, Guid.NewGuid().ToString());//视频读取之后的缓存地址
            var image = Path.Combine(appPath, "logo.png");

            //Console.WriteLine("开始推流中...");

            //Console.WriteLine("正在点播中...");
           Console.WriteLine("正在直播中...");


            #region 视频编码方法格式转换(**已注释**)
            //视频编码
            //Encoder.Create()
            //    .WidthInput(inputPath)
            //    .WithFilter(new X264Filter { Preset = X264Preset.Faster, ConstantQuantizer = 18 })
            //    .WithFilter(new ImageWatermarkFilter(image, WatermarkPosition.TopRight))
            //    .WithFilter(new ResizeFilter(Resolution.X720P))
            //    .WithFilter(new SnapshotFilter(Path.Combine(appPath, "output", "output.png"), 320, 180, 10))//with snapshot
            //    .To<Mp4>(outputPath)//编码完成之后保存的地址
            //    .Execute();

            #endregion




            #region RTMP推流(**已成功推流至服务器,推流本地视频**)
            //Network.Create()
            //    .WithSource(inputPath)//inputPath可以改成获取设备的视频流
            //    .WithDest("rtmp://192.168.61.128/live/livestream")//可以根据自己的需求更新RTMP服务器地址
            //    .WithFilter(new X264Filter { ConstantQuantizer = 20 })
            //    .WithFilter(new ResizeFilter(Resolution.X360P))
            //    .Push();

            #endregion


            #region RTMP推流(**自动获取设备并推流至服务器**)
            Network.Create()
                .WithSource("Directshow")//inputPath可以改成获取设备的视频流
                                         // .WithDest("rtmp://192.168.61.128/live/livestream")//可以根据自己的需求更新RTMP服务器地址
                .WithDest("rtmp://172.16.20.10:1935/live")
                .WithFilter(new X264Filter { ConstantQuantizer = 20 })
                .WithFilter(new ResizeFilter(Resolution.X720P))
                .Push();

            #endregion





            #region 从RTMP服务器接收流(录屏功能,支持以后需要播放)

            //Network.Create()
            //    .WithSource("rtmp://192.168.61.128/live/livestream")//inputPath可以改成获取设备的视频流
            //    .WithDest(inputPath)//这个路径可以自由更改，如果是直播就不需要使用这个路径，直接读取流至播放器播放实时接收即可。
            //    .WithFilter(new X264Filter { ConstantQuantizer = 20 })
            //    .WithFilter(new ResizeFilter(Resolution.X720P))
            //    .Pull();



            //var ffPlay = Path.Combine(AssemblyDirectory, "ffplay.exe");
            //Process.Start(ffPlay, "rtmp://192.168.61.128/live/livestream");

            #endregion
            Console.WriteLine("直播结束.\n 按任意键进行退出！");

            Console.ReadKey();
        }


        //public static string AssemblyDirectory
        //{
        //    get
        //    {
        //        string codeBase = Assembly.GetExecutingAssembly().CodeBase;
        //        var uri = new UriBuilder(codeBase);
        //        string path = Uri.UnescapeDataString(uri.Path);
        //        return Path.GetDirectoryName(path);
        //    }
        //}
    }
}
