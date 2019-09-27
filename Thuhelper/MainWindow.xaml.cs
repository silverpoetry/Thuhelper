


using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Timers;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Media.Animation;
using System.Windows.Interop;
using System.IO;

namespace Thuhelper
{
    public struct Rect
    {
        public int Left;
        public int Top;
        public int Right;
        public int Bottom;
    }
    class WindowHelpser
    {
        [DllImport("user32.dll", CharSet = CharSet.Auto, ExactSpelling = true)]
        public static extern IntPtr GetForegroundWindow();
        [DllImport("user32.dll")]
        public static extern int GetWindowTextLength(IntPtr hWnd);
        [DllImport("User32.dll", CharSet = CharSet.Auto)]
        public static extern int GetWindowText(IntPtr hWnd, StringBuilder text, int nMaxCount);


        [DllImport("user32.dll")]
        private static extern int GetWindowRect(IntPtr hwnd, out Rect lpRect);


        public static string GetForeWinText()
        {
            IntPtr hWnd = GetForegroundWindow();
            int length = GetWindowTextLength(hWnd);
            StringBuilder windowName = new StringBuilder(length + 1);
            GetWindowText(hWnd, windowName, windowName.Capacity);
            return windowName.ToString();
            //"Program Manager"
            //"Mainwindow"
        }

        public static IntPtr GetForeWindowHwnd()
        {
            IntPtr hWnd = GetForegroundWindow();

            return hWnd;
        }
        public static int GetWindowHeight()
        {
            GetWindowRect(GetForeWindowHwnd(), out Rect r);
            return r.Bottom - r.Top;
        }




    }

    struct Event
    {
      public  string Name;
        public int Xingqi;
        public string Classroom;
        public DateTime StartTime;
        public DateTime EndTime;


    }
    


    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow : Window
    {

        Event[] events= new Event[1000];

       
        public MainWindow()
        {
            InitializeComponent();
            
           
        }

        [DllImport("user32", EntryPoint = "SetWindowLong")]
        public static extern int SetWindowLongA(int hwnd, int nIndex, int dwNewLong);

        private void Grid_Loaded(object sender, RoutedEventArgs e)
        {

            #region Layout
            this.Left = System.Windows.SystemParameters.PrimaryScreenWidth - this.ActualWidth - 10;
            this.Top = 10;

            const int WS_EX_TOOLWINDOW = 0x00000080;
            SetWindowLongA(new WindowInteropHelper(this).Handle.ToInt32(), -20, WS_EX_TOOLWINDOW);



            #endregion
            #region 加载
            TimeSpan tt = DateTime.Now - new DateTime(2019, 9, 8);
            this.txt1.Text = "第" + ((int)tt.TotalDays / 7 + 1).ToString() + "周";


            Timer t = new Timer(100);
            t.Elapsed += (a, b) => {
                #region 失败的尝试
                //      string s = GetForeWinText();
                //;
                //      if (s == "MainWindow" || s == "Program Manager"||s=="") { this.Dispatcher.Invoke(() => { txt1.Text =this.IsVisible.ToString();   }); }
                //      else
                //      {

                //          this.Dispatcher.Invoke(() =>
                //          {
                //              txt1.Text = this.IsVisible.ToString();
                //              this.Topmost = false;

                //          });
                //      }
                #endregion

                this.Dispatcher.Invoke(
                    () =>
                    {

                        txttime.Text = DateTime.Now.ToShortDateString();

                        if (WindowHelpser.GetForeWinText().Trim() == "" && WindowHelpser.GetWindowHeight() > 500)
                        {
                           // System.Threading.Thread.Sleep(300);
                           if (WindowHelpser.GetForeWinText().Trim() == "")
                            {
                                this.Show();
                                this.Topmost = true;
                                this.Topmost = false;
                            }

                        }




                    }
                    );




            };
            t.Start();
            #endregion
            var  lst= File.ReadLines("info.thu") ;
            string[] s = lst.ToArray();
            int n = 0;
            for (int i = 0; i < s.Length; i+=6)
            {
                events[++n].Name = s[i];
                events[n].Xingqi = int.Parse(s[i + 1]);
                
                events[n].StartTime= events[n].StartTime.AddHours (int.Parse(s[i + 2].Split(':')[0]));
                events[n].StartTime= events[n].StartTime.AddMinutes(int.Parse(s[i + 2].Split(':')[1]));
                events[n].EndTime= events[n].EndTime.AddHours(int.Parse(s[i + 3].Split(':')[0]));
                events[n].EndTime=events[n].EndTime.AddMinutes(int.Parse(s[i + 3].Split(':')[1]));
                events[n].Classroom = s[i + 4];
            
            }

        }
        bool state = true;
        private void Txt1_MouseUp(object sender, MouseButtonEventArgs e)
        {
            if (state)
            {
                DoubleAnimation D = new DoubleAnimation(0, 1, new Duration(TimeSpan.FromMilliseconds(200)));
                // D.From = grd1.Width;
                grd1.BeginAnimation(Grid.OpacityProperty, D);
            }
            else
            {
                DoubleAnimation D = new DoubleAnimation(1, 0, new Duration(TimeSpan.FromMilliseconds(200)));
                // D.From = grd1.Width;
                grd1.BeginAnimation(Grid.OpacityProperty, D);

            }
            state = !state;

           
        }
    }
}
