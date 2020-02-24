using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
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
    /// Snapshotbutton.xaml 的交互逻辑
    /// </summary>
    public partial class Snapshotbutton : Window
    {
        System.Windows.Forms.Timer timer = new System.Windows.Forms.Timer() { Interval = 1000 };
        double x, y;
        bool fucked = false;
        //explorer.exe shell:appsFolder\Microsoft.ScreenSketch_8wekyb3d8bbwe!App
        public Snapshotbutton()
        {
            InitializeComponent();
            timer.Tick += (a, b) =>
            {
                
                if (this.Left==x&&this.Top==y)
                {
                    this.Close();
                    fucked = true;   
                }
                timer.Stop();
            };
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            this.Left = System.Windows.SystemParameters.PrimaryScreenWidth - this.ActualWidth - 10;
            this.Top = 40;
            this.Topmost = true;
        }

        private void Ellipse_MouseDown(object sender, MouseButtonEventArgs e)
        {
            //e.Handled = true;
            if (e.RightButton == MouseButtonState.Pressed)
            {
                //直接关闭
                this.Close();
                fucked = true;
            }
            if (e.LeftButton == MouseButtonState.Pressed)
            {
                x = this.Left;
                y = this.Top;
                if (!timer.Enabled)
                {
                    timer.Start();
                }
                this.DragMove();
                if (timer.Enabled)
                {
                    //停止长按关闭检测
                    timer.Stop();
                }
                if (this.Left == x && this.Top == y&&!fucked)
                {
                    //没有拖动且没有按死
                    Process.Start(@"explorer.exe", @"shell:appsFolder\Microsoft.ScreenSketch_8wekyb3d8bbwe!App");
                }
                
                //System.Windows.Forms.MessageBox.Show("Test");
            }
            

        }

        private void Ellipse_MouseUp(object sender, MouseButtonEventArgs e)
        {
           
           
            //长按则已经关闭，只考虑截图的情况
            //
        }

        private void Ellipse_DragOver(object sender, DragEventArgs e)
        {
          
        }
    }
}
