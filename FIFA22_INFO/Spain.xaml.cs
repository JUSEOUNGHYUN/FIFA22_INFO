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

namespace FIFA22_INFO
{
    /// <summary>
    /// Spain.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class Spain : Window
    {
        public Spain()
        {
            InitializeComponent();
        }

        private void Rectangle_MouseDown(object sender, MouseButtonEventArgs e)
        {
            this.DragMove();
        }

        private void ToMiniButton_Click(object sender, RoutedEventArgs e)
        {
            this.WindowState = System.Windows.WindowState.Minimized;
        }

        private void CloseButton_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void Santander_Click(object sender, RoutedEventArgs e)
        {
            LALIGA_SANTANDER ls = new LALIGA_SANTANDER();
            ls.WindowStartupLocation = WindowStartupLocation.CenterScreen;
            ls.Show();
        }

        private void SamrtBank_Click(object sender, RoutedEventArgs e)
        {
            LALIGA_SMARTBANK ls = new LALIGA_SMARTBANK();
            ls.WindowStartupLocation = WindowStartupLocation.CenterScreen;
            ls.Show();
        }

        private void keyEvent(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Escape)
            {
                this.Close();
            }
        }
    }
}
