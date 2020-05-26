using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Forms;
using System.Windows.Input;
using System.Windows.Interop;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace Thuhelper
{


    internal class HotKeyWinApi
    {
        public const int WmHotKey = 0x0312;
        [DllImport("user32.dll", SetLastError = true)] public static extern bool RegisterHotKey(IntPtr hWnd, int id, ModifierKeys fsModifiers, Keys vk);
        [DllImport("user32.dll", SetLastError = true)] public static extern bool UnregisterHotKey(IntPtr hWnd, int id);
    }



    public sealed class HotKey : IDisposable { public event Action<HotKey> HotKeyPressed; private readonly int _id; private bool _isKeyRegistered; readonly IntPtr _handle; public HotKey(ModifierKeys modifierKeys, Keys key, Window window) : this(modifierKeys, key, new WindowInteropHelper(window)) { Contract.Requires(window != null); } public HotKey(ModifierKeys modifierKeys, Keys key, WindowInteropHelper window) : this(modifierKeys, key, window.Handle) { Contract.Requires(window != null); } public HotKey(ModifierKeys modifierKeys, Keys key, IntPtr windowHandle) { Contract.Requires(modifierKeys != ModifierKeys.None || key != Keys.None); Contract.Requires(windowHandle != IntPtr.Zero); Key = key; KeyModifier = modifierKeys; _id = GetHashCode(); _handle = windowHandle; RegisterHotKey(); ComponentDispatcher.ThreadPreprocessMessage += ThreadPreprocessMessageMethod; } ~HotKey() { Dispose(); } public Keys Key { get; private set; } public ModifierKeys KeyModifier { get; private set; } public void RegisterHotKey() { if (Key == Keys.None) return; if (_isKeyRegistered) UnregisterHotKey(); _isKeyRegistered = HotKeyWinApi.RegisterHotKey(_handle, _id, KeyModifier, Key); if (!_isKeyRegistered) ; } public void UnregisterHotKey() { _isKeyRegistered = !HotKeyWinApi.UnregisterHotKey(_handle, _id); } public void Dispose() { ComponentDispatcher.ThreadPreprocessMessage -= ThreadPreprocessMessageMethod; UnregisterHotKey(); } private void ThreadPreprocessMessageMethod(ref MSG msg, ref bool handled) { if (!handled) { if (msg.message == HotKeyWinApi.WmHotKey && (int)(msg.wParam) == _id) { OnHotKeyPressed(); handled = true; } } } private void OnHotKeyPressed() { if (HotKeyPressed != null) HotKeyPressed(this); } }

    /// <summary>
    /// ScreenCapture.xaml 的交互逻辑
    /// </summary>
    public partial class ScreenCapture : Window
    {
        float rate;
        public ScreenCapture()
        {
            InitializeComponent();
           // System.Drawing.Rectangle rc = SystemInformation.VirtualScreen;
             rate = Graphics.FromHwnd(IntPtr.Zero).DpiX / 96;
            this.Width = System.Windows.Forms.Screen.PrimaryScreen.Bounds.Width / rate;
            this.Height = System.Windows.Forms.Screen.PrimaryScreen.Bounds.Height / rate;
            screenscource = ToBitmapSource(GetScreenSnapshot());
            asd.Source = screenscource;
            asd.Width = System.Windows.Forms.Screen.PrimaryScreen.Bounds.Width/rate ;
            //  System.Windows.Forms.MessageBox.Show();
            
            //asd.Height = this.Height;
            // this.Background =new ImageBrush( ToBitmapSource(GetScreenSnapshot()));
        }

        Bitmap GetScreenSnapshot()
        {
            System.Drawing.Rectangle rc = SystemInformation.VirtualScreen;
            var bitmap = new Bitmap(rc.Width, rc.Height);
            using (Graphics g = Graphics.FromImage(bitmap))
            {
                g.CopyFromScreen(0, 0, 0, 0, rc.Size, CopyPixelOperation.SourceCopy);
            }
            return bitmap;
        }
        BitmapSource ToBitmapSource(Bitmap bmp)
        {
            BitmapSource returnSource;
            try
            {
                returnSource = Imaging.CreateBitmapSourceFromHBitmap(bmp.GetHbitmap(), IntPtr.Zero, Int32Rect.Empty, BitmapSizeOptions.FromEmptyOptions());
            }
            catch
            {
                returnSource = null;
            }
            return returnSource;
        }
        BitmapSource screenscource;
        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            this.Topmost = true;
        }
        System.Windows.Point p;
        bool startdrag = false;
        private void Canvas_MouseDown(object sender, MouseButtonEventArgs e)
        {
            p = e.GetPosition((IFrameworkInputElement)sender);
            Canvas.SetLeft(bd, p.X);
            Canvas.SetTop(bd, p.Y);
            startdrag = true;
        }
        CroppedBitmap c;
        private void Canvas_MouseUp(object sender, MouseButtonEventArgs e)
        {
            // System.Windows.Forms.MessageBox.Show("end!!");
           

        }

        private void Canvas_MouseMove(object sender, System.Windows.Input.MouseEventArgs e)
        {
            try
            {
                if (e.MouseDevice.LeftButton == MouseButtonState.Pressed)
                {
                    bd.Width = e.GetPosition((IFrameworkInputElement)sender).X - p.X;
                    bd.Height = e.GetPosition((IFrameworkInputElement)sender).Y - p.Y;

                    var p2 = e.GetPosition((IFrameworkInputElement)sender);
                    System.Windows.Controls.Image i = new System.Windows.Controls.Image();
                    BitmapSource b = screenscource.Clone();
                    i.Source = b;

                    i.Clip = new RectangleGeometry();
                    c = new CroppedBitmap(screenscource, new Int32Rect((int)(p.X * rate), (int)(p.Y * rate), (int)(((int)p2.X - (int)p.X) * rate),(int)( ((int)p2.Y - (int)p.Y) * rate)));
                    bd.Background = new ImageBrush(c);
                }
            }
            catch (Exception)
            {

                
            }
          
        }

        private void Window_MouseUp(object sender, MouseButtonEventArgs e)
        {

            //if (startdrag)
            //{
            //    startdrag = false;
            //    System.Windows.Clipboard.SetImage(c);
            //    this.Close();
            //}

        }
        private void Window_KeyUp(object sender, System.Windows.Input.KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                if (startdrag)
                {
                    startdrag = false;
                    System.Windows.Clipboard.SetImage(c);
                    this.Close();
                }

            }
            if (e.Key == Key.Escape)
            {
                startdrag = false;
                bd.Width = 0;
            }
        }
    }
}
