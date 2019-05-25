using CSDeskBand;
using QTranser;
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

namespace QTestsWindow
{
    /// <summary>
    /// ShowBand.xaml 的交互逻辑
    /// </summary>
    public partial class ShowBand : Window
    {
        public ShowBand()
        {
            InitializeComponent();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            BandOperate.ShowBand(typeof(QTranse));
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            BandOperate.HideBand(typeof(QTranse));
        }
    }
}
