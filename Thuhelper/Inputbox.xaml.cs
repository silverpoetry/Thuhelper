using System;
using System.Collections.Generic;
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
    /// Inputbox.xaml 的交互逻辑
    /// </summary>
    public partial class Inputbox : Window
    {
        public Inputbox()
        {
            InitializeComponent();
        }

        private void Rectangle_MouseMove(object sender, MouseEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed) this.DragMove();

        }
        public string Ret { get { return textBox.Text; } }

        private void Q_MouseUp(object sender, MouseButtonEventArgs e)
        {
            this.Close();
        }

        private void TextBox_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter) Q_MouseUp(null, null);
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            textBox.Focus();
            textBox.SelectAll();
        }
    }
}
