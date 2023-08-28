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
    /// Search.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class Search : Window
    {
        public Search()
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

        private void keyDown_Event(object sender, KeyEventArgs e)
        {
            if(e.Key == Key.Escape)
            {
                this.Close();
            }
        }

        private void TeamSearch_Click(object sender, RoutedEventArgs e)
        {
            Team_Info ti = new Team_Info();
            ti.WindowStartupLocation = WindowStartupLocation.CenterScreen;
            ti.Show();
        }

        private void YearSearch_Click(object sender, RoutedEventArgs e)
        {
            Year y = new Year();
            y.WindowStartupLocation = WindowStartupLocation.CenterScreen;
            y.Show();
        }
    }
}
