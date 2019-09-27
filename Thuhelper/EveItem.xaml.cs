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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Thuhelper
{
    /// <summary>
    /// EveItem.xaml 的交互逻辑
    /// </summary>
    public partial class EveItem : UserControl
    {
        public EveItem()
        {
            InitializeComponent();
        }
        public string Eventname
        {
            get { return Clsnm.Text; }


            set { Clsnm.Text = value; }
        }
        public string Eventtime
        {
            get { return Clstime.Text; }


            set { Clstime.Text = value; }
        }
        public string Classroom
        {
            get { return Clsroom.Text; }


            set { Clsroom.Text = value; }
        }
    }

}
