using System;
using System.Diagnostics;
using System.IO;
//OS support:Windows 7, Windows Server 2003, Windows Server 2008, Windows Vista, Windows XP
//Windows 10 integrated this  DirectX sdk in C:\Windows\Microsoft.NET\DirectX for Managed Code
using Microsoft.DirectX.DirectSound;
using System.Runtime.InteropServices;
using System.Reflection;
/// <summary>
/// FFmpeg 音视频录制
/// DirectX SDK 下载地址 https://www.microsoft.com/en-us/download/details.aspx?id=6812
/// </summary>
namespace FFmpegSharp.Utils
{
    public class Recorder
    {
        [DllImport("kernel32.dll")]
        static extern bool GenerateConsoleCtrlEvent(int dwCtrlEvent, int dwProcessGroupId);

        [DllImport("kernel32.dll")]
        static extern bool SetConsoleCtrlHandler(IntPtr handlerRoutine, bool add);

        [DllImport("kernel32.dll")]
        static extern bool AttachConsole(int dwProcessId);

        [DllImport("kernel32.dll")]
        static extern bool FreeConsole();


        // ffmpeg进程
        static Process p = new Process();

        // ffmpeg.exe实体文件路径
        static string ffmpegPath = AppDomain.CurrentDomain.BaseDirectory + "external\\ffmpeg\\x64\\ffmpeg.exe";

        /// <summary>
        /// 获取声音输入设备列表
        /// </summary>
        /// <returns>声音输入设备列表</returns>
        /// OS support: Windows 7, Windows Server 2003, Windows Server 2008, Windows Vista, Windows XP
        /// Windows 10 integrated this  DirectX sdk in C:\Windows\Microsoft.NET\DirectX for Managed Code
        public static CaptureDevicesCollection GetAudioList()
        {
            CaptureDevicesCollection collection = new CaptureDevicesCollection();

            return collection;
        }

        /// <summary>
        /// 功能: 开始录制
        /// </summary>
        public static void Start(string audioDevice, string outFilePath)
        {
            if (File.Exists(outFilePath))
            {
                File.Delete(outFilePath);
            }

            /*
             * 转码，
             * 视频录制设备：gdigrab；
             * 录制对象：桌面；
             * 音频录制方式：dshow；
             * 视频编码格式：h.264；*/
            ProcessStartInfo startInfo = new ProcessStartInfo(ffmpegPath);
            startInfo.WindowStyle = ProcessWindowStyle.Normal;
            startInfo.UseShellExecute = false;// 必须设置使用shell否则异常
            startInfo.Arguments = "-f gdigrab -framerate 15 -i desktop -f dshow -i audio=\"" + audioDevice + "\" -vcodec libx264 -preset:v ultrafast -tune:v zerolatency -acodec libmp3lame \"" + outFilePath + "\"";

            p.StartInfo = startInfo;

            p.Start();
        }

        /// <summary>
        /// 功能: 停止录制
        /// </summary>
        public static void Stop()
        {
            AttachConsole(p.Id);
            SetConsoleCtrlHandler(IntPtr.Zero, true);
            GenerateConsoleCtrlEvent(0, 0);
            FreeConsole();
        }
    }
}
