using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace Thuhelper
{
    /// <summary>
    /// ScreenScene.xaml 的交互逻辑
    /// </summary>
    public partial class ScreenScene : Window
    {
        public ScreenScene()
        {
            InitializeComponent();
        }
       
        public static string DoGetRequestSendData(string url,string type)
        {
            HttpWebRequest hwRequest = null;
            HttpWebResponse hwResponse = null;
            string strResult = string.Empty;
            try
            {
                hwRequest = (System.Net.HttpWebRequest)WebRequest.Create(url);
                //hwRequest.Timeout = 30000;
                hwRequest.Method = "GET";
                hwRequest.ContentType = type;

            }
            catch (System.Exception err)
            {
            }
            try
            {
                hwResponse = (HttpWebResponse)hwRequest.GetResponse();
                StreamReader srReader = new StreamReader(hwResponse.GetResponseStream(), Encoding.UTF8);

                strResult = srReader.ReadToEnd();
                srReader.Close();
                hwResponse.Close();
            }
            catch (System.Exception err)
            {
            }
            return strResult;
        }

        public static string SaveImageFromWeb(string imgUrl, string path, string fileName)
        {
            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }
            string imgName = imgUrl.ToString().Substring(imgUrl.ToString().LastIndexOf("/") + 1);
            string defaultType = ".jpg";
            string[] imgTypes = new string[] { ".jpg", ".jpeg", ".png", ".gif", ".bmp" };
            string imgType = imgUrl.ToString().Substring(imgUrl.ToString().LastIndexOf("."));
            string imgPath = "";
            foreach (string it in imgTypes)
            {
                if (imgType.ToLower().Equals(it))
                    break;
                if (it.Equals(".bmp"))
                    imgType = defaultType;
            }

            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(imgUrl);
            request.UserAgent = "Mozilla/6.0 (MSIE 6.0; Windows NT 5.1; Natas.Robot)";
            request.Timeout = 3000;

            WebResponse response = request.GetResponse();
            Stream stream = response.GetResponseStream();

            if (response.ContentType.ToLower().StartsWith("image/"))
            {
                byte[] arrayByte = new byte[1024];
                int imgLong = (int)response.ContentLength;
                int l = 0;

                if (fileName == "")
                    fileName = imgName;
               
                FileStream fso = new FileStream(path+fileName+imgType, FileMode.Create);
                while (l < imgLong)
                {
                    int i = stream.Read(arrayByte, 0, 1024);
                    fso.Write(arrayByte, 0, i);
                    l += i;
                }

                fso.Close();
                stream.Close();
                response.Close();
                imgPath = fileName + imgType;
                return imgPath;
            }
            else
            {
                return "";
            }
        }
        string[] files;
        int nowflag;
        private void TextBox_Loaded(object sender, RoutedEventArgs e)
        {
           float rate = (int)Graphics.FromHwnd(IntPtr.Zero).DpiX / 96;
            this.Width = System.Windows.Forms.Screen.PrimaryScreen.Bounds.Width / rate;
            this.Height = System.Windows.Forms.Screen.PrimaryScreen.Bounds.Height / rate;
           // img.Source = screenscource;
            img.Height = System.Windows.Forms.Screen.PrimaryScreen.Bounds.Height / rate;


            if (File.Exists("a.txt")) File.WriteAllText("a.txt", DateTime.Now.ToString("yy-mm-dd"));
            if (!(File.ReadAllText("a.txt")== DateTime.Now.ToString("yy-mm-dd")))
            {
          
            for (int i = 0; i < 1; i++)
            {
                string url = @"https://cn.bing.com/HPImageArchive.aspx?format=js&idx=" + i.ToString() + "&n=1&pid=hp";
                url = DoGetRequestSendData(url, "application/x-www-form-urlencoded");
                JObject j = (JObject)JsonConvert.DeserializeObject(url);
                j = (JObject)((JArray)j["images"])[0];
                string u = "http://cn.bing.com" + j["url"];
                SaveImageFromWeb(u, "image\\", $"{Guid.NewGuid().ToString()}");
                
            }

            }
            files = Directory.GetFiles(System.Windows.Forms.Application.StartupPath+ "\\image");
           var t =  new System.Windows.Forms.Timer();
            t.Interval=60000;
            t.Tick +=
              (asda, basd) =>
              {
                  BitmapImage b = new BitmapImage();
                  b.BeginInit();
                  b.UriSource = new Uri(files[nowflag],UriKind.Absolute);
                  b.EndInit();
                  img.Source = b;
                  nowflag++;
                  if (nowflag == files.Length) nowflag = 0;
              };

            t.Start();
            BitmapImage bb = new BitmapImage();
            bb.BeginInit();
            bb.UriSource = new Uri(files[nowflag], UriKind.Absolute);
            bb.EndInit();
            img.Source = bb;
            nowflag++;
            if (nowflag == files.Length) nowflag = 0;
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            //this.Left = 0.0;
           // this.Top = 0.0;
           // this.WindowStyle = WindowStyle.None;
            //this.Width = System.Windows.SystemParameters.FullPrimaryScreenWidth;
          //  this.Height = System.Windows.SystemParameters.PrimaryScreenHeight;


        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
           
        }
    }
}
