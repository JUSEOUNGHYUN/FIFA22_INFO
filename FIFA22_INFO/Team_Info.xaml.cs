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
    /// Team_Info.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class Team_Info : Window
    {
        string m_sTeamName = string.Empty;
        public Team_Info()
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

        private void KeyEvent(object sender, KeyEventArgs e)
        {
            if(e.Key == Key.Escape)
            {
                this.Close();
            }
        }

        private void Team_Search_button_Click(object sender, RoutedEventArgs e)
        {
            AllTeam at = new AllTeam();
            at.DataPassProdCd += new AllTeam.DataPassProdCdEventHandler(TeamNameReceive);
            at.WindowStartupLocation = WindowStartupLocation.CenterScreen;
            at.ShowDialog();

            ImageData image = new ImageData();

            BitmapImage bitmap = new BitmapImage(new Uri("Resources/" + m_sTeamName + ".png", UriKind.Relative));
            ImageBrush brush = new ImageBrush(bitmap);
            Run_imageRec.Fill = brush;
        }

        private void TeamName_DoubleClick(object sender, MouseButtonEventArgs e)
        {
            AllTeam at = new AllTeam();
            at.DataPassProdCd += new AllTeam.DataPassProdCdEventHandler(TeamNameReceive);
            at.WindowStartupLocation = WindowStartupLocation.CenterScreen;
            at.Show();
        }

        private void TeamNameReceive(string sTeamName)
        {
            TeamName_textBox.Text = sTeamName;
            m_sTeamName = sTeamName;
        }
    }
}
