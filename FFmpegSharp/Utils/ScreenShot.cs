using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
/// <summary>
/// flv视频截图工具
/// </summary>
namespace FFmpegSharp.Utils
{
    class ScreenShot: System.Web.UI.Page
    {
        //文件路径
        public static string ffmpegtool = "ffmpeg/ffmpeg.exe";
        public static string mencodertool = "mencoder/mencoder.exe";
        public static string flvtool = "flvtool/flvtool2.exe";//flv标记工具
        public static string upFile = "UpFiles" + "/";//上传文件夹
        public static string imgFile = "ImgFile" + "/";//图片文件夹
        public static string playFile = "PlayFiles" + "/";//flv文件夹
        public static string xmlFile = "xmlFiles" + "/";//xml文件夹
        public static string sizeOfImg = "240x180";//图片的宽与高
        public static string widthOfFile = "400";//flv文件的宽度
        public static string heightOfFile = "350";//flv文件的高度
        //public static string ffmpegtool = ConfigurationManager.AppSettings["ffmpeg"];
        //public static string mencodertool = ConfigurationManager.AppSettings["mencoder"];
        //public static string upFile = ConfigurationManager.AppSettings["upfile"] + "/";
        //public static string imgFile = ConfigurationManager.AppSettings["imgfile"] + "/";
        //public static string playFile = ConfigurationManager.AppSettings["playfile"] + "/";
        //文件图片大小
        //public static string sizeOfImg = ConfigurationManager.AppSettings["CatchFlvImgSize"];
        //文件大小
        //public static string widthOfFile = ConfigurationManager.AppSettings["widthSize"];
        //public static string heightOfFile = ConfigurationManager.AppSettings["heightSize"];
        //获取文件的名字
        private System.Timers.Timer myTimer = new System.Timers.Timer(3000);//记时器
        public static string flvName = "";
        public static string imgName = "";
        public static string flvXml = "";
        public static int pId = 0;
        public static string GetFileName(string fileName)
        {
            int i = fileName.LastIndexOf("\\") + 1;
    
            string Name = fileName.Substring(i);
            return Name;
        }
        //获取文件扩展名
        public static string GetExtension(string fileName)
        {
            int i = fileName.LastIndexOf(".") + 1;
            string Name = fileName.Substring(i);
            return Name;
        }
        //
        #region //运行FFMpeg的视频解码，(这里是绝对路径)
        /// <summary>
        /// 转换文件并保存在指定文件夹下面(这里是绝对路径)
        /// </summary>
        /// <param name="fileName">上传视频文件的路径（原文件）</param>
        /// <param name="playFile">转换后的文件的路径（网络播放文件）</param>
        /// <param name="imgFile">从视频文件中抓取的图片路径</param>
        /// <returns>成功:返回图片虚拟地址;   失败:返回空字符串</returns>
        public void ChangeFilePhy(string fileName, string playFile, string imgFile)
        {
            //取得ffmpeg.exe的路径,路径配置在Web.Config中,如:<add   key="ffmpeg"   value="E:aspx1ffmpeg.exe"   /> 
            string ffmpeg = Server.MapPath(ScreenShot.ffmpegtool);
            if ((!System.IO.File.Exists(ffmpeg)) || (!System.IO.File.Exists(fileName)))
            {
                return;
            }
            //获得图片和(.flv)文件相对路径/最后存储到数据库的路径,如:/Web/User1/00001.jpg 
            string flv_file = System.IO.Path.ChangeExtension(playFile, ".flv");
            //截图的尺寸大小,配置在Web.Config中,如:<add   key="CatchFlvImgSize"   value="240x180"   /> 
            string FlvImgSize = ScreenShot.sizeOfImg;
            System.Diagnostics.ProcessStartInfo FilestartInfo = new System.Diagnostics.ProcessStartInfo(ffmpeg);
            FilestartInfo.WindowStyle = System.Diagnostics.ProcessWindowStyle.Hidden;
            FilestartInfo.Arguments = " -i " + fileName + " -ab 56 -ar 22050 -b 500 -r 15 -s " + widthOfFile + "x" + heightOfFile + " " + flv_file;
            //ImgstartInfo.Arguments = "   -i   " + fileName + "   -y   -f   image2   -t   0.05   -s   " + FlvImgSize + "   " + flv_img;
            try
            {
                //转换
                System.Diagnostics.Process.Start(FilestartInfo);
                //截图
                CatchImg(fileName, imgFile);
                //System.Diagnostics.Process.Start(ImgstartInfo);
            }
            catch
            {
            }
        }
        #endregion
        #region 截图
        public string CatchImg(string fileName, string imgFile)
        {
            //
            string ffmpeg = Server.MapPath(ScreenShot.ffmpegtool);
            //
            string flv_img = imgFile + ".jpg";
            //
            string FlvImgSize = ScreenShot.sizeOfImg;
            //
            System.Diagnostics.ProcessStartInfo ImgstartInfo = new System.Diagnostics.ProcessStartInfo(ffmpeg);
            ImgstartInfo.WindowStyle = System.Diagnostics.ProcessWindowStyle.Hidden;
            //
            ImgstartInfo.Arguments = "   -i   " + fileName + "  -y  -f  image2   -ss 2 -vframes 1  -s   " + FlvImgSize + "   " + flv_img;
            try
            {
                System.Diagnostics.Process.Start(ImgstartInfo);
            }
            catch
            {
                return "";
            }
            //
            catchFlvTool(fileName);
            if (System.IO.File.Exists(flv_img))
            {
                return flv_img;
            }
            return "";
        }
        #endregion
        #region //运行FFMpeg的视频解码，(这里是(虚拟)相对路径)
        /// <summary>
        /// 转换文件并保存在指定文件夹下面(这里是相对路径)
        /// </summary>
        /// <param name="fileName">上传视频文件的路径（原文件）</param>
        /// <param name="playFile">转换后的文件的路径（网络播放文件）</param>
        /// <param name="imgFile">从视频文件中抓取的图片路径</param>
        /// <returns>成功:返回图片虚拟地址;   失败:返回空字符串</returns>
        public void ChangeFileVir(string fileName, string playFile, string imgFile)
        {
            //取得ffmpeg.exe的路径,路径配置在Web.Config中,如:<add   key="ffmpeg"   value="E:\aspx1\ffmpeg.exe"   /> 
            string ffmpeg = Server.MapPath(ScreenShot.ffmpegtool);
            if ((!System.IO.File.Exists(ffmpeg)) || (!System.IO.File.Exists(fileName)))
            {
                return;
            }
            //获得图片和(.flv)文件相对路径/最后存储到数据库的路径,如:/Web/User1/00001.jpg 
            string flv_img = System.IO.Path.ChangeExtension(Server.MapPath(imgFile), ".jpg");
            string flv_file = System.IO.Path.ChangeExtension(Server.MapPath(playFile), ".flv");
            //截图的尺寸大小,配置在Web.Config中,如:<add   key="CatchFlvImgSize"   value="240x180"   /> 
            string FlvImgSize = ScreenShot.sizeOfImg;
            System.Diagnostics.ProcessStartInfo FilestartInfo = new System.Diagnostics.ProcessStartInfo(ffmpeg);
            FilestartInfo.WindowStyle = System.Diagnostics.ProcessWindowStyle.Hidden;
            //此处组合成ffmpeg.exe文件需要的参数即可,此处命令在ffmpeg   0.4.9调试通过
            //ffmpeg -i F:\01.wmv -ab 56 -ar 22050 -b 500 -r 15 -s 320x240 f:\test.flv
            FilestartInfo.Arguments = " -i " + fileName + " -ab 56 -ar 22050 -b 500 -r 15 -s " + widthOfFile + "x" + heightOfFile + " " + flv_file;
            try
            {
                System.Diagnostics.Process ps = new System.Diagnostics.Process();
                ps.StartInfo = FilestartInfo;
                ps.Start();
                Session.Add("ProcessID", ps.Id);
                Session.Add("flv", flv_file);
                Session.Add("img", imgFile);
                myTimer.Elapsed += new System.Timers.ElapsedEventHandler(myTimer_Test);
                myTimer.Enabled = true;
            }
            catch
            {
            }
        }
        #endregion
        #region //运行mencoder的视频解码器转换(这里是(绝对路径))
        public void MChangeFilePhy(string vFileName, string playFile, string imgFile)
        {
            string tool = Server.MapPath(ScreenShot.mencodertool);
            //string mplaytool = Server.MapPath(PublicMethod.ffmpegtool);
            if ((!System.IO.File.Exists(tool)) || (!System.IO.File.Exists(vFileName)))
            {
                return;
            }
            string flv_file = System.IO.Path.ChangeExtension(playFile, ".flv");
            //截图的尺寸大小,配置在Web.Config中,如:<add   key="CatchFlvImgSize"   value="240x180"   /> 
            string FlvImgSize = ScreenShot.sizeOfImg;
            System.Diagnostics.ProcessStartInfo FilestartInfo = new System.Diagnostics.ProcessStartInfo(tool);
            FilestartInfo.WindowStyle = System.Diagnostics.ProcessWindowStyle.Hidden;
            FilestartInfo.Arguments = " " + vFileName + " -o " + flv_file + " -of lavf -lavfopts i_certify_that_my_video_stream_does_not_use_b_frames -oac mp3lame -lameopts abr:br=56 -ovc lavc -lavcopts vcodec=flv:vbitrate=200:mbd=2:mv0:trell:v4mv:cbp:last_pred=1:dia=-1:cmp=0:vb_strategy=1 -vf scale=" + widthOfFile + ":" + heightOfFile + " -ofps 12 -srate 22050";
            try
            {
                System.Diagnostics.Process ps = new System.Diagnostics.Process();
                ps.StartInfo = FilestartInfo;
                ps.Start();
                Session.Add("ProcessID", ps.Id);
                Session.Add("flv", flv_file);
                Session.Add("img", imgFile);
                //pId = ps.Id;
                //flvName = flv_file;
                //imgName = imgFile;
                myTimer.Elapsed += new System.Timers.ElapsedEventHandler(myTimer_Test);
                myTimer.Enabled = true;
            }
            catch
            {
            }
        }
        /// <summary>
        /// 记时器功能，自动保存截图
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void myTimer_Test(object sender, System.Timers.ElapsedEventArgs e)
        {
            if (!object.Equals(null, Session["ProcessID"]))
            {
                try
                {
                    System.Diagnostics.Process prs = System.Diagnostics.Process.GetProcessById(int.Parse(Session["ProcessID"].ToString()));
                    if (prs.HasExited)
                    {
                        CatchImg(Session["flv"].ToString(), Session["img"].ToString());
                        catchFlvTool(Session["flv"].ToString());
                        myTimer.Enabled = false;
                        myTimer.Close();
                        myTimer.Dispose();
                        Session.Abandon();
                    }
                }
                catch
                {
                    CatchImg(Session["flv"].ToString(), Session["img"].ToString());
                    catchFlvTool(Session["flv"].ToString());
                    myTimer.Enabled = false;
                    myTimer.Close();
                    myTimer.Dispose();
                    Session.Abandon();
                }
            }
        }
        #endregion
        public string catchFlvTool(string fileName)
        {
            //
            string flvtools = Server.MapPath(ScreenShot.flvtool);
            //
            string flv_xml = fileName.Replace(".flv", ".xml").Replace(ScreenShot.upFile.Replace("/", ""), ScreenShot.xmlFile.Replace("/", ""));
            //
            System.Diagnostics.ProcessStartInfo ImgstartInfo = new System.Diagnostics.ProcessStartInfo(flvtools);
            ImgstartInfo.WindowStyle = System.Diagnostics.ProcessWindowStyle.Hidden;
            //
            ImgstartInfo.Arguments = "   " + fileName + "   -UPx   " + fileName + "  >  " + flv_xml;
            try
            {
                System.Diagnostics.Process.Start(ImgstartInfo);
            }
            catch
            {
                return "";
            }
            //
            if (System.IO.File.Exists(flv_xml))
            {
                return flv_xml;
            }
            return "";
        }
    }
}
