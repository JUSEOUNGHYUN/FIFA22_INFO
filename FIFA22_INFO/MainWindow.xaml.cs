using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Data;
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
using Npgsql;

namespace FIFA22_INFO
{
    /// <summary>
    /// MainWindow.xaml에 대한 상호 작용 논리
    /// </summary>
    /// 

    public partial class MainWindow : Window
    {

#if WORKING_HOME

        public static string mConnString = "HOST=localhost;PORT=5432;USERNAME=postgres;PASSWORD=1234;DATABASE=postgres;";
#else
        public static string mConnString = "HOST=localhost;PORT=5432;USERNAME=postgres;PASSWORD=1234;DATABASE=FIFA22;";
#endif

        private string mDBConnectionInfo = string.Empty;

        public static List<PREMIER> mPREMEIRList = new List<PREMIER>();

        public MainWindow()
        {
            InitializeComponent();

            /*
            mDBConnectionInfo = GetConfigDBConnectionInfo();

            bool bIsConnectDB = false;
            SqlConnection conn = new SqlConnection(mDBConnectionInfo);
            try
            {
                // DB 연결
                conn.Open();

                // 연결여부에 따라 다른 메시지를 보여준다
                if (conn.State != ConnectionState.Open)
                {
                    MessageBox.Show("DB 서버에 연결할수없습니다. 프로그램을 종료합니다. ");
                }
                else
                {
                    bIsConnectDB = true;
                }
            }
            catch (SqlException sqlex)
            {
                MessageBox.Show("DB 서버에 연결할수없습니다. 프로그램을 종료합니다.");
            }
            finally
            {
                if (conn != null)
                {
                    conn.Close();
                    conn.Dispose();
                }

            }
             */

            LeagueOption_comboBox.Items.Add("PREMIER_LEAGUE");
            LeagueOption_comboBox.Items.Add("EMIRATES_FA_CUP");
            LeagueOption_comboBox.Items.Add("CARABAO_CUP");
            LeagueOption_comboBox.Items.Add("CHAMPIONS_LEAGUE");
            LeagueOption_comboBox.Items.Add("EUROPA_LEAGUE");
            LeagueOption_comboBox.Items.Add("CONFERENCE_LEAGUE");
            LeagueOption_comboBox.Items.Add("SUPER_CUP");
            LeagueOption_comboBox.Items.Add("LIGUE1_UBER_EATS");
            LeagueOption_comboBox.Items.Add("BUNDESLIGA");
            LeagueOption_comboBox.Items.Add("SERIE_A");
            LeagueOption_comboBox.Items.Add("EREDIVISIE");
            LeagueOption_comboBox.Items.Add("LALIG_PROTUGAL");
            LeagueOption_comboBox.Items.Add("LALIGA_SANTANDER");

            LeagueOption_comboBox.SelectedIndex = 0;

        }

        // DB 연결
        private string GetConfigDBConnectionInfo()
        {
            AppSettingsReader ar = new AppSettingsReader();
            string strConString = string.Empty;
            try
            {
                strConString = (string)ar.GetValue("ConnectionString", typeof(string));
            }
            catch (Exception ex)
            {
                MessageBox.Show("DB정보가 없습니다..", "DB 연결", MessageBoxButton.OK, MessageBoxImage.Exclamation);
                this.Close();
            }

            List<string> ConDataList = ParseConDataConfig(strConString);
            string ConString = "HOST=" + ConDataList[0] + ";PORT=" + ConDataList[1] + ";USERNAME=" + ConDataList[2] + ";PASSWORD=" + ConDataList[3] + ";DATABASE=" + ConDataList[4] + ";";
            ConString += "Connection Timeout=5;";

            return ConString;
        }

        public static List<string> ParseConDataConfig(string strConString)
        {
            List<string> list = new List<string>();

            list = strConString.Split(';').ToList();
            list.RemoveAt(list.Count() - 1);

            List<string> ConDataList = new List<string>();

            for (int i = 0; i < list.Count(); i++)
            {
                string str1 = list[i].Substring(list[i].IndexOf('=') + 1).Trim();
                ConDataList.Add(str1);
            }

            return ConDataList;
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

        private void Premier_League_Click(object sender, RoutedEventArgs e)
        {
            LIGUE1 l1 = new LIGUE1();
            l1.WindowStartupLocation = WindowStartupLocation.CenterScreen;
            l1.Show();
        }

        // 실제 DB에 우승팀 etc insert 하는 창 띄우는 버튼

        private void Update_Click(object sender, RoutedEventArgs e)
        {
            DB_Insert insert = new DB_Insert();
            insert.WindowStartupLocation = WindowStartupLocation.CenterScreen;
            insert.Show();
        }

        private void INSERT_button_Click(object sender, RoutedEventArgs e)
        {

        }

        private void England_Click(object sender, RoutedEventArgs e)
        {
            England en = new England();
            en.WindowStartupLocation = WindowStartupLocation.CenterScreen;
            en.Show();
        }

        private void UEFA_button_Click(object sender, RoutedEventArgs e)
        {
            UEFA ue = new UEFA();
            ue.WindowStartupLocation = WindowStartupLocation.CenterScreen;
            ue.Show();
        }

        private void Italia_button_Click(object sender, RoutedEventArgs e)
        {
            SERIE_A se = new SERIE_A();
            se.WindowStartupLocation = WindowStartupLocation.CenterScreen;
            se.Show();
            /*
            Premier_League pl = new Premier_League();
            pl.WindowStartupLocation = WindowStartupLocation.CenterScreen;
            pl.Show();
             */
        }

        private void BUNDESLIGA_Click(object sender, RoutedEventArgs e)
        {
            BUNDESLIGA b = new BUNDESLIGA();
            b.WindowStartupLocation = WindowStartupLocation.CenterScreen;
            b.Show();
        }

        private void EREDIVISIE_Click(object sender, RoutedEventArgs e)
        {
            EREDIVISIE ere = new EREDIVISIE();
            ere.WindowStartupLocation = WindowStartupLocation.CenterScreen;
            ere.Show();
        }

        private void LIGA_PORTUGAL_Click(object sender, RoutedEventArgs e)
        {
            LIGA_PORTUGAL lp = new LIGA_PORTUGAL();
            lp.WindowStartupLocation = WindowStartupLocation.CenterScreen;
            lp.Show();
        }

        private void LALIGA_SANTANDER_Click(object sender, RoutedEventArgs e)
        {
            LALIGA_SANTANDER ls = new LALIGA_SANTANDER();
            ls.WindowStartupLocation = WindowStartupLocation.CenterScreen;
            ls.Show();
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
