using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Media;
using System.Windows.Input;
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace Thuhelper
{
    /// <summary>
    /// TImer.xaml 的交互逻辑
    /// </summary>
    public partial class TImer : Window
    {
        public TImer()
        {
            InitializeComponent();
        }
        int getfuhao(int a)
        {
            if (a == 0) return 0;
            return a / Math.Abs(a);
        }
        int ttimes = 0;
        long lasttime = -1;
        int lastx = 0;
        int goqushi = 1;
   
      
        private void Rectangle_MouseMove(object sender, MouseEventArgs e)
        {
           
            if (e.LeftButton == MouseButtonState.Pressed)
            {
                
                long ttime = DateTime.Now.Ticks / 10000;
                if (lasttime == -1) { ttimes = 0; lasttime = ttime; lastx = (int)this.PointToScreen(new Point(0, 0)).X; return; }
                if (ttime - lasttime > 400) { lasttime = -1; return; }
                if (getfuhao((int)this.PointToScreen(new Point(0, 0)).X - lastx) == -goqushi) { goqushi = -goqushi; ttimes++; }
                //  tbPause.Text = ttimes.ToString();
                if (ttimes > 5)
                {
                 
                    DoubleAnimation d = new DoubleAnimation(1, 0, new Duration(TimeSpan.FromMilliseconds(1000)));
                    d.Completed += D_Completed;
                    grd1.BeginAnimation(Grid.OpacityProperty, d);

                    ; return;
                }
                lastx = (int)this.PointToScreen(new Point(0, 0)).X;
                //  tbPause.Text = lastx.ToString();
                lasttime = ttime;
                this.DragMove();

            }
        }


        private void D_Completed(object sender, EventArgs e)
        {
            this.Close();
        }

        private void Grd1_Loaded(object sender, RoutedEventArgs e)
        {
            WindowHelpser.EnBlur(this.grd1);
            Inputbox i = new Inputbox();
            i.ShowDialog();
            double ret = Convert.ToDouble(i.Ret);
            full =(int)( ret * 36000);
        }
        byte[,] tgtcolor = new byte[5, 4] { { 0, 0xe8, 0xf8, 0x6b }, { 0, 0x09, 0x96, 0xe0 }, { 0, 0xf1, 0xf1, 0x1a }, { 1, 0xb2, 0x60, 0x06 }, { 0, 0, 0, 0 } };
        int full; 
        void dorgb(int val)
        {



               //数据总数
            val = val % full;
            byte[] ncol = new byte[4];
            int pieces = full / 3;
            for (int i = 1; i <= 3; i++)
            {
                if (val >= pieces * (i - 1) && val <= pieces * i)
                {
                    val -= pieces * (i - 1);
                    for (int j = 1; j <= 3; j++)
                    {
                        ncol[j] = (byte)(tgtcolor[i - 1, j] + (tgtcolor[i, j] - tgtcolor[i - 1, j]) * val / pieces);
                    }
                    rtBg.Fill = new SolidColorBrush(Color.FromRgb(ncol[1], ncol[2], ncol[3]));
                    break;

                }

            }


        }
        bool isstarted = false;
        private void TextBlock_MouseUp(object sender, MouseButtonEventArgs e)
        {
            if (isstarted)
            {
                //终止
                tbStart.Text = "启动";
                tbStart.Foreground = Brushes.Black;
                tbPause.Foreground = Brushes.Gray;
                ispauseed = false;
                tbPause.Text = "暂停";
                t.Stop();
                time = 0;
                time2 = 0;
                isstarted = false;
                return;
            }

            t = new System.Windows.Forms.Timer();
            t.Interval = 100;
            t.Tick += (a, b) =>
            {

                time2++;
                time = time2 / 10;
                dorgb(time2);
                if (time > 3600)
                    ssss.Text = $"{(time / 3600).ToString("D2")}:{((time % 3600) / 60).ToString("D2")}:{(time % 60).ToString("D2")}";
                else
                    ssss.Text = $"{((time % 3600) / 60).ToString("D2")}:{(time % 60).ToString("D2")}";
            };
            t.Start();
            tbStart.Text = "终止";
            tbStart.Foreground = Brushes.Red;
            tbPause.Foreground = Brushes.Black;
            ssss.Text = "00:00";
            isstarted = true;

        }


        int time = 0;
        int time2 = 0;
        System.Windows.Forms.Timer t;
        bool ispauseed = false;
        private void TextBlock_MouseUp_1(object sender, MouseButtonEventArgs e)
        {
            if (!isstarted) return;
            if (!ispauseed) { t.Stop(); ispauseed = true; tbPause.Text = "继续"; }
            else { t.Start(); tbPause.Text = "暂停"; ispauseed = false; }

        }
    }
}
