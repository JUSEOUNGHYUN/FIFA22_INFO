using Npgsql;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
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
    public class TEAM_CAREER
    {
        public string League_Year;
        public string Champions;
        public string Second_Place;
        public string Remark;

        public TEAM_CAREER(string league_Year, string champions, string second_Place, string remark)
        {
            League_Year = league_Year;
            Champions = champions;
            Second_Place = second_Place;
            Remark = remark;
        }
    }

    /// <summary>
    /// TeamCareer.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class TeamCareer : Window
    {

        List<TEAM_CAREER> mTC = new List<TEAM_CAREER>();

        public TeamCareer()
        {
            InitializeComponent();
        }

        public void GetTeam(string teamName , string sOption)
        {
            TEAMNAME_textBox.Text = teamName;

            //BitmapImage bitmap = new BitmapImage(new Uri("Image/Champions_League_Team/" + TEAMNAME_textBox.Text + ".png", UriKind.Relative));
            BitmapImage bitmap = new BitmapImage(new Uri("Resources/" + TEAMNAME_textBox.Text + ".png", UriKind.Relative));
            ImageBrush brush = new ImageBrush(bitmap);
            imageRec.Fill = brush;

            Option_textBox.Text = sOption;

            // 04048c
            if (Option_textBox.Text == "CHAMPIONS_LEAGUE")
            {
                Option_textBox.Foreground = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#04048c"));
            }
            else if (Option_textBox.Text == "EUROPA_LEAGUE")
            {
                Option_textBox.Foreground = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#fb6b04"));
            }
            else if(Option_textBox.Text == "CONFERENCE_LEAGUE")
            {
                Option_textBox.Foreground = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#04bb14"));
            }
            else if( Option_textBox.Text == "SUPER_CUP")
            {
                Option_textBox.Foreground = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#040434"));
            }
            else if (Option_textBox.Text == "PREMIER_LEAGUE")
            {
                Option_textBox.Foreground = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#44044c"));
            }
            else if (Option_textBox.Text == "EMIRATES_FA_CUP")
            {
                Option_textBox.Foreground = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#d41c24"));
            }
            else if (Option_textBox.Text == "CARABAO_CUP")
            {
                Option_textBox.Foreground = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#058C5C"));
            }

            /*
            //List<OptionNameColor> cs = new List<OptionNameColor>();
            OptionNameColor cs = new OptionNameColor();
            cs.OptionName = Option_textBox.Text;
             */
        }

        private void champions_Click(object sender, RoutedEventArgs e)
        {
            DBSelect("Champions");
        }

        private void Runnerup_Click(object sender, RoutedEventArgs e)
        {
            DBSelect("second_place");
        }

        private void DBSelect(string sRanking)
        {
            string teamname = TEAMNAME_textBox.Text;
            string championssql = "select league_year , champions , second_place , remark from " + Option_textBox .Text+ " where "+ sRanking + " = '" + teamname + "' order by league_year;";
            NpgsqlConnection conn = new NpgsqlConnection(MainWindow.mConnString);

            try
            {
                conn.Open();

                NpgsqlCommand cmd = new NpgsqlCommand(championssql, conn);
                NpgsqlDataReader reader = cmd.ExecuteReader();
                mTC.Clear();

                while (reader.Read())
                {
                    TEAM_CAREER tc = new TEAM_CAREER(
                        reader[0].ToString().Trim(),
                        reader[1].ToString().Trim(),
                        reader[2].ToString().Trim(),
                        reader[3].ToString().Trim()
                        );
                    mTC.Add(tc);
                }

                SetDataGridUsingQueryResultList();
             
                reader.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            finally
            {
                if (conn != null)
                {
                    conn.Close();
                    conn.Dispose();
                }
            }
        }

        private void SetDataGridUsingQueryResultList()
        {
            List<Team_Career> uiList = new List<Team_Career>();

            for (int i = 0; i < mTC.Count; i++)
            {
                TEAM_CAREER tc1 = mTC[i];

                uiList.Add(new Team_Career()
                {
                    Index = i + 1,
                    League_Year = mTC[i].League_Year.Trim(),
                    ChampionsLOGO = mTC[i].Champions.Trim(),
                    Champions = mTC[i].Champions.Trim(),
                    Second_PlaceLOGO = mTC[i].Second_Place.Trim(),
                    Second_Place = mTC[i].Second_Place.Trim(),
                    Remark = mTC[i].Remark.Trim()
                });
            }

            Ranking_DataGrid.ItemsSource = uiList;
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
            if(e.Key == Key.C)
            {
                DBSelect("Champions");
            }
            if(e.Key == Key.R)
            {
                DBSelect("second_place");
            }
            if (e.KeyboardDevice.Modifiers == ModifierKeys.Control && e.Key == Key.S)
            {
                SaveFunc();
            }
            if(e.Key == Key.D)
            {
                AllTeam at = new AllTeam();
                at.DataPassProdCd += new AllTeam.DataPassProdCdEventHandler(TeamNameReceive);
                at.WindowStartupLocation = WindowStartupLocation.CenterScreen;
                at.ShowDialog();

                BitmapImage bitmap = new BitmapImage(new Uri("Resources/" + TEAMNAME_textBox.Text + ".png", UriKind.Relative));
                ImageBrush brush = new ImageBrush(bitmap);
                imageRec.Fill = brush;
            }
        }

        private void TeamName_textBox_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            AllTeam at = new AllTeam();
            at.DataPassProdCd += new AllTeam.DataPassProdCdEventHandler(TeamNameReceive);
            at.WindowStartupLocation = WindowStartupLocation.CenterScreen;
            at.ShowDialog();

            BitmapImage bitmap = new BitmapImage(new Uri("Resources/" + TEAMNAME_textBox.Text + ".png", UriKind.Relative));
            ImageBrush brush = new ImageBrush(bitmap);
            imageRec.Fill = brush;
        }

        private void TeamNameReceive(string sTeamName)
        {
            TEAMNAME_textBox.Text = sTeamName;
        }

        private void Ranking_DataGrid_KeyDown(object sender, KeyEventArgs e)
        {
            /*
            if(e.Key == Key.U)
            {
                int selectedIndex = Ranking_DataGrid.SelectedIndex;

                if(selectedIndex < 0)
                {
                    MessageBox.Show("선택하지 않았습니다. 선택하세요", "선택", MessageBoxButton.OK, MessageBoxImage.Information);
                    return;
                }
                else
                {
                    TEAM_CAREER ci = mTC[selectedIndex];

                    DB_Insert di = new DB_Insert();
                    di.WindowStartupLocation = WindowStartupLocation.CenterScreen;
                    di.Show();
                    di.GetSelectedData(Option_textBox.Text, ci.League_Year, ci.Remark);
                    // Option_textBox.text
                    // LeagueYear
                    // Remark

                }
            }
             */
        }

        private void SaveFunc()
        {
            if (MessageBox.Show("데이터를 저장하시 겠습니까??", "데이터 저장", MessageBoxButton.YesNo, MessageBoxImage.Information) == MessageBoxResult.Yes)
            {
                List<Team_Career> list = Ranking_DataGrid.ItemsSource as List<Team_Career>;
                int n = mTC.Count;

                List<string> updateList = new List<string>();

                for (int i = 0; i < list.Count; i++)
                {
                    TEAM_CAREER pr = mTC[i];
                    Team_Career other = list[i];

                    if (pr.Champions!= other.Champions)
                    {
                        updateList.Add(Premier_League.UpdateSqlFunc(Option_textBox.Text, "champions", other.Champions, other.League_Year));
                    }
                    if (pr.Second_Place != other.Second_Place)
                    {
                        updateList.Add(Premier_League.UpdateSqlFunc(Option_textBox.Text, "Second_Place", other.Second_Place, other.League_Year));
                    }
                    if (pr.Remark != other.Remark)
                    {
                        updateList.Add(Premier_League.UpdateSqlFunc(Option_textBox.Text, "Remark", other.Remark, other.League_Year));
                    }
                }

                NpgsqlConnection conn = null;
                try
                {
                    conn = new NpgsqlConnection(MainWindow.mConnString);
                    conn.Open();

                    for (int i = 0; i < updateList.Count; i++)
                    {
                        NpgsqlCommand UpdateCommand = new NpgsqlCommand(updateList[i], conn);
                        UpdateCommand.ExecuteNonQuery();
                    }

                    MessageBox.Show("저장을 완료했습니다.", "데이터 업데이트", MessageBoxButton.OK, MessageBoxImage.Information);
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                }
                finally
                {
                    if (conn != null)
                    {
                        if (conn.State != ConnectionState.Closed)
                        {
                            conn.Close();
                            conn.Dispose();
                        }
                    }
                }
            }
        }


    }
}
