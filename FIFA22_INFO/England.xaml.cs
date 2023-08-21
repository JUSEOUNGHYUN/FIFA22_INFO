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
    /// England.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class England : Window
    {
        public England()
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

        private void PREMIER_LEAGUE_Click(object sender, RoutedEventArgs e)
        {
            Premier_League pl = new Premier_League();
            pl.WindowStartupLocation = WindowStartupLocation.CenterScreen;
            pl.Show();
        }

        private void EMIRATES_FA_CUP_Click(object sender, RoutedEventArgs e)
        {
            EMIRATES_FA_CUP fa = new EMIRATES_FA_CUP();
            fa.WindowStartupLocation = WindowStartupLocation.CenterScreen;
            fa.Show();
        }

        private void CARABAO_CUP_Click(object sender, RoutedEventArgs e)
        {
            CARABAO_CUP cc = new CARABAO_CUP();
            cc.WindowStartupLocation = WindowStartupLocation.CenterScreen;
            cc.Show();
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
