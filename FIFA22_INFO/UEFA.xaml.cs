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
    /// UEFA.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class UEFA : Window
    {
        public UEFA()
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

        private void Champions_League_Click(object sender, RoutedEventArgs e)
        {
            CHAMPIONS_LEAGUE cl = new CHAMPIONS_LEAGUE();
            cl.WindowStartupLocation = WindowStartupLocation.CenterScreen;
            cl.Show();
        }

        private void Europa_League_Click(object sender, RoutedEventArgs e)
        {
            EUROPA_LEAGUE ep = new EUROPA_LEAGUE();
            ep.WindowStartupLocation= WindowStartupLocation.CenterScreen;
            ep.Show();
        }

        private void Conference_League_Click(object sender, RoutedEventArgs e)
        {
            CONFERENCE_LEAGUE cl = new CONFERENCE_LEAGUE();
            cl.WindowStartupLocation = WindowStartupLocation.CenterScreen;
            cl.Show();
        }

        private void Super_Cup_Click(object sender, RoutedEventArgs e)
        {
            SUPER_CUP sc = new SUPER_CUP();
            sc.WindowStartupLocation = WindowStartupLocation.CenterScreen;
            sc.Show();
        }

        private void keyDown_Event(object sender, KeyEventArgs e)
        {
            if(e.Key == Key.Escape)
            {
                this.Close();
            }
        }
    }
}
