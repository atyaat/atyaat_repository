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

namespace SPI_Show
{
    /// <summary>
    /// Login.xaml 的交互逻辑
    /// </summary>
    public partial class Login : Window
    {
        public Login()
        {
            InitializeComponent();
        }

        private void Loging_Click(object sender, RoutedEventArgs e)
        {
            if (combox1.Text == "SPI_数据上传监控看板")
            {
                MainWindow window1 = new MainWindow();
                window1.Show();
                this.Close();
            }
            else if (combox1.Text == "SPI_fail数展示")
            {
                SPI_FAIL window1 = new SPI_FAIL();
                window1.Show();
                this.Close();
            }
          
        }

        private void quit(object sender, MouseButtonEventArgs e)
        {
            Application.Current.Shutdown();
        }
    }
}
