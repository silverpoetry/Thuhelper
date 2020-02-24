


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
   

    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow : Window
    {

        Event[] events=new Event[1000];
        System.Windows.Forms.Timer timer = new System.Windows.Forms.Timer() { Interval = 300 };
        bool fucked = false;
       
  
        public MainWindow()
        {
            InitializeComponent();
            timer.Tick += (a, b) =>
            {
                timer.Stop();
                fucked = true;
                new Snapshotbutton().Show();
                
                // Txt1_MouseUp(null, null);
                //  TImer t = new TImer();
                //t.Show();



            };
           
        }
        long getTimeTicks()
        {
            return DateTime.Now.Ticks/ 10000;
        }






        [DllImport("user32", EntryPoint = "SetWindowLong")]
        public static extern int SetWindowLongA(int hwnd, int nIndex, int dwNewLong);
        private HotKey _hotkey, _hotkey2;
        private void Grid_Loaded(object sender, RoutedEventArgs e)
        {
            _hotkey = new HotKey(ModifierKeys.Control| ModifierKeys.Alt, System.Windows.Forms.Keys.A, this);
            _hotkey.HotKeyPressed += (k) =>new  ScreenCapture().Show();

            _hotkey = new HotKey(ModifierKeys.Control, System.Windows.Forms.Keys.L, this);
            _hotkey.HotKeyPressed += (k) => new ScreenScene().Show();

            #region Layout
            
            this.Left = System.Windows.SystemParameters.PrimaryScreenWidth - this.ActualWidth - 10;
            this.Top = 10;

            const int WS_EX_TOOLWINDOW = 0x00000080;
            SetWindowLongA(new WindowInteropHelper(this).Handle.ToInt32(), -20, WS_EX_TOOLWINDOW);



            #endregion
            #region 加载
            TimeSpan tt = DateTime.Now - new DateTime(2019, 9, 8);
            this.txt1.Text = "第" + ((int)tt.TotalDays / 7 + 1).ToString() + "周";
            grd1.Opacity = 0;
            scv.ScrollToVerticalOffset (btn_timer.Height);

            Timer t = new Timer(500);
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

            var ee = (from a in events where getxq(a.Xingqi) == (int)(DateTime.Now.DayOfWeek) select a);

            foreach (var item in ee)
            {
                EveItem eee = new EveItem();
                eee.Eventname = item.Name;
                eee.Eventtime = $"{item.StartTime.ToString("hh:mm")}~{item.EndTime.ToString("hh:mm")}";
                stk.Children.Add(eee);
                eee.Classroom = item.Classroom;
            }
            
        }
        bool state = true;
        private void Txt1_MouseUp(object sender, MouseButtonEventArgs e)
        {
          //  System.Windows.Forms.MessageBox.Show("Test");
            if (fucked)
            {
                fucked = false;
                return;
            }
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

            //清除长按计时器
            if (timer.Enabled)
            {
                timer.Stop();
            }
           
        }
        int getxq(int n )
        {
            if (n == 7) return 0;
            if (n == 0) return -1;
                return n;
        }
        private void ScrollViewer_ManipulationBoundaryFeedback(object sender, ManipulationBoundaryFeedbackEventArgs e)
        {
            e.Handled = true;
        }

        private void Rectangle_MouseUp(object sender, MouseButtonEventArgs e)
        {
       

        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Txt1_MouseUp(null, null);
            TImer t = new TImer();
            t.Show();
        

        }

        private void txt1_MouseDown(object sender, MouseButtonEventArgs e)
        {
            //   lasttick = getTimeTicks();
            //   isTitlePressed = true;
          //  System.Windows.Forms.MessageBox.Show("Test");
            if (timer.Enabled==false)
            {
                timer.Start();
            }
            else
            {
             //   timer.Stop();
                timer.Start();
            }
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
           // Thuhelper.WindowHelpser.EnBlur(this.grd1);
        }
    }
}
