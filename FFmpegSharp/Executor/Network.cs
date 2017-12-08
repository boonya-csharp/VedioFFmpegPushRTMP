using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FFmpegSharp.Codes;
using FFmpegSharp.Filters;
using FFmpegSharp.Media;
using System.Collections;
using FFmpegSharp.Directshow;
using System.Runtime.InteropServices;

namespace FFmpegSharp.Executor
{
    public class Network
    {
        private readonly List<FilterBase> _filters;

        private string _source;
        private string _dest;
        private TargetType _sourceType;
        private TargetType _destType;
        /// <summary> list of installed video devices. </summary>
        private ArrayList capDevices;
        private string _dsshow;
        private string _dsshowname;

        public Network()
        {
            _sourceType = TargetType.Default;
            _destType = TargetType.Default;
            _filters = new List<FilterBase>();
        }

        public Network WithFilter(FilterBase filter)
        {
            if (_filters.Any(x => x.Name.Equals(filter.Name, StringComparison.OrdinalIgnoreCase)))
            {
                var old = _filters.First(x => x.Name.Equals(filter.Name, StringComparison.OrdinalIgnoreCase));
                _filters.Remove(old);
            }

            _filters.Add(filter);
            return this;
        }

        public Network WithSource(string source)
        {
            _sourceType = GetTargetType(source);
            _source = source;

            return this;
        }

        public Network WithDest(string dest)
        {
            _destType = GetTargetType(dest);
            _dest = dest;

            return this;
        }

        public static Network Create()
        {
            return new Network();
        }

        /// <summary>
        /// 推流到RTMP服务器
        /// </summary>
        public void Push()
        {
            Validate();

            if (_destType != TargetType.Live)
            {
                throw new ApplicationException("当推流到RTMP服务器的时候，源类型必须是'RtmpType.Live'类型.");
            }

            //参数为false的时候则为推流
            //true为获取实时设备图像
            var @params = GetParams(false);
            if (@params != "")
            {
                Processor.FFmpeg(@params);
            }
            else
            {
                Console.WriteLine("There is no stream or devices input/output,please check again!");
            }
        }

        /// <summary>
        /// 把流从RTMP服务器拉取--读取视频数据 ==pull a stream from rtmp server
        /// </summary>
        public void Pull()
        {
            Validate();

            if (!TestRtmpServer(_source, true))
                throw new ApplicationException("RTMP服务器发送错误.");

            if (_sourceType != TargetType.Live)
            {
                throw new ApplicationException("必须是RTMP服务器.");
            }
            //参数为true的时候则为读取视频流
            var @params = GetParams(false);

            Processor.FFmpeg(@params);
        }


        /// <summary>
        /// 检测输出输入源以及过滤器
        /// </summary>
        private void Validate()
        {
            if (_sourceType == TargetType.Default)
                throw new ApplicationException("源错误.请输入源！");

            if (_destType == TargetType.Default)
                throw new ApplicationException("dest错误.请输入一个dest");

            var supportFilters = new[] { "Resize", "Segment", "X264", "AudioRate", "AudioBitrate" };

            if (_filters.Any(x => !supportFilters.Contains(x.Name)))
            {
                throw new ApplicationException(string.Format("过滤器不支持，过滤器只支持:{0} 类型",
                    supportFilters.Aggregate(string.Empty, (current, filter) => current + (filter + ",")).TrimEnd(new[] { ',' })));
            }
        }

        private static TargetType GetTargetType(string target)
        {
            if (target.StartsWith("rtmp://", StringComparison.OrdinalIgnoreCase) || target == "")
            {
                //获取服务器当前流的状态
                return TargetType.Live;
            }
            else if (File.Exists(target))
            {
                //判断是否为本地文件
                return TargetType.File;
            }
            else
            {
                return TargetType.Directshow;
            }


            throw new ApplicationException("源错误，未知的访问源.");
        }


        private static CodeBase GuessCode(string filePath)
        {
            var codes = new Dictionary<string, CodeBase>
            {
                {"MP3", new Mp3()},
                {"MP4", new Mp4()},
                {"M4A", new M4A()},
                {"FLV", new Flv()}
            };

            var ext = Path.GetExtension(filePath);

            if (string.IsNullOrEmpty(ext))
                throw new ApplicationException(string.Format("can't guess the code from Path :'{0}'", filePath));

            var key = ext.TrimStart(new[] { '.' });

            if (codes.Keys.Any(x => x.Equals(key, StringComparison.OrdinalIgnoreCase)))
            {
                return codes[key];
            }

            throw new ApplicationException(string.Format("not support file extension :{0}", key.ToLower()));
        }

        private string GetParams(bool is_push)
        {
            var builder = new StringBuilder();
            int showflag = 0;

            if (_sourceType == TargetType.File)
            {
                builder.Append(" -re -i ");
                builder.Append(_source);
            }

            if (_sourceType == TargetType.Live)
            {
                builder.Append(" -i");

                builder.AppendFormat(
                    -1 < _source.IndexOf("live=1", StringComparison.OrdinalIgnoreCase) ? " {0} live=1" : " {0}",
                    _source);
            }

            if (_sourceType == TargetType.Directshow)
            {

                //如果输入是设备开始获取设备名称
                if (DsDev.GetDevicesOfCat(FilterCategory.VideoInputDevice, out capDevices, out _dsshow))
                {
                    //获取正常
                    _dsshowname = "\"" + _dsshow + "\"";

                    _dest = " -f dshow -i video=" + _dsshowname + " -vcodec libx264 -preset:v ultrafast -tune:v zerolatency -f flv " + _dest + "";
                    builder.Append(_dest);
                }
                else
                {
                    Console.WriteLine("No video capture devices found!");
                    showflag = 1;
                    builder.Clear();
                }

            }

            if (_sourceType != TargetType.Directshow)
            {

                if (_sourceType == TargetType.File)
                {
                    if (_filters.Any(x => x.Name.Equals("Segment", StringComparison.OrdinalIgnoreCase)))
                    {
                        var filter = _filters.First(x => x.Name.Equals("Segment", StringComparison.OrdinalIgnoreCase));
                        _filters.Remove(filter);
                    }
                }


                if (!_filters.Any(x => x.Name.Equals("x264", StringComparison.OrdinalIgnoreCase)))
                {
                    builder.Append(" -vcodec copy");
                }

                if (_destType == TargetType.Live)
                {
                    if (!_filters.Any(x => x.Name.Equals("AudioRate", StringComparison.OrdinalIgnoreCase)))
                    {
                        _filters.Add(new AudioRatelFilter(44100));
                    }

                    if (!_filters.Any(x => x.Name.Equals("AudioBitrate", StringComparison.OrdinalIgnoreCase)))
                    {
                        _filters.Add(new AudioBitrateFilter(128));
                    }
                }


                if (_sourceType != TargetType.Directshow)
                {
                    var sourcefile = new MediaStream(_source, is_push);
                    foreach (var filter in _filters.OrderBy(x => x.Rank))
                    {
                        filter.Source = sourcefile;
                        builder.Append(filter.Execute());
                    }
                }

                if (_destType == TargetType.File)
                {
                    var dir = Path.GetDirectoryName(_dest);

                    if (string.IsNullOrWhiteSpace(dir))
                        throw new ApplicationException("output directory error.");

                    var fileName = Path.GetFileNameWithoutExtension(_dest);

                    if (string.IsNullOrWhiteSpace(fileName))
                        throw new ApplicationException("output filename is null");

                    var code = GuessCode(_dest);

                    if (!_filters.Any(x => x.Name.Equals("Segment", StringComparison.OrdinalIgnoreCase)))
                    {
                        // out%d.mp4
                        builder.AppendFormat(" {0}{1}%d{2}", dir, fileName, code.Extension);
                    }
                }

                if (_destType == TargetType.Live)
                {
                    builder.Append(" -f flv ");
                    builder.Append(_dest);
                }


            }

            return builder.ToString();
        }

        /// <summary>
        /// 测试RTMP服务器是否响应
        /// </summary>
        /// <param name="server"></param>
        /// <returns></returns>
        private bool TestRtmpServer(string server, bool is_push)
        {
            var val = false;

            var @params = string.Format(
                -1 < _source.IndexOf("live=1", StringComparison.OrdinalIgnoreCase) ? "\" -i {0} live=1\"" : "\" -i {0}\"",
                server);

            //try 10 times
            var i = 0;
            do
            {
                try
                {
                    var message = Processor.FFprobe(@params, is_push,
                        id => Task.Run(async () =>
                        {
                            await Task.Delay(1000);

                            /*
                             * if rtmp is alive but no current stream output, 如果RTMP是启动状态但是没有当前输出流
                             * FFmpeg will  be in a wait state forerver. FFMPEG将会一直在等待状态
                             * so after 1s, kill the process. 最后关闭进程
                             */

                            try
                            {
                                var p = Process.GetProcessById(id);
                                //when the process was exited will throw a excetion. 
                                //当进程还存在的时候将会抛出异常
                                p.Kill();
                            }
                            catch (Exception)
                            {
                            }

                        }));

                    if (message.Equals("error", StringComparison.OrdinalIgnoreCase))
                    {
                        throw new ApplicationException("error");
                    }

                    val = true;
                    i = 10;
                }
                catch (Exception)
                {
                    i += 1;
                }

            } while (i < 10);

            return val;
        }

        private static string GetFriendlyName(UCOMIMoniker mon)
        {
            object bagObj = null;
            IPropertyBag bag = null;
            try
            {
                Guid bagId = typeof(IPropertyBag).GUID;
                mon.BindToStorage(null, null, ref bagId, out bagObj);
                bag = (IPropertyBag)bagObj;
                object val = "";
                int hr = bag.Read("FriendlyName", ref val, IntPtr.Zero);
                if (hr != 0)
                    Marshal.ThrowExceptionForHR(hr);
                string ret = val as string;
                if ((ret == null) || (ret.Length < 1))
                    throw new NotImplementedException("Device FriendlyName");
                return ret;
            }
            catch (Exception)
            {
                return null;
            }
            finally
            {
                bag = null;
                if (bagObj != null)
                    Marshal.ReleaseComObject(bagObj); bagObj = null;
            }
        }
    }
}