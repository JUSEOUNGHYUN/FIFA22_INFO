using Npgsql;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
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

        private void SelectFunc(string sTeamName)
        {
            NpgsqlConnection conn = null;

            try
            {
                conn = new NpgsqlConnection(MainWindow.mConnString);
                conn.Open();

                string sql = "select champions_league_winner_cnt, europa_league_winner_cnt , conference_league_winner_cnt , super_cup_winner_cnt  from total where team_name = '" + sTeamName+ "';";

                NpgsqlCommand cmd = new NpgsqlCommand(sql, conn);
                NpgsqlDataReader reader = cmd.ExecuteReader();

                while(reader.Read())
                {
                    ChampionsCNT_textBox.Text = reader[0].ToString().Trim();
                    EuropaCNT_textBox.Text = reader[1].ToString().Trim();   
                    ConferenceCNT_textBox.Text = reader[2].ToString().Trim();
                    SuperCupCNT_textBox.Text = reader[3].ToString().Trim();
                }

                reader.Close();

                JudgeWinner(sTeamName);

            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            finally
            {
                if (conn != null && conn.State != ConnectionState.Closed)
                {
                    conn.Close();
                    conn.Dispose();
                }
            }
        }

        private void JudgeWinner(string sTeamName)
        {
            List<int> ints = new List<int>();

            string Presql = "select count(*) from premier_league  where champions = '"+ sTeamName + "';";
            string Francesql = "select count(*) from ligue1_uber_eats where champions = '" + sTeamName + "';";
            string Bundessql = "select count(*) from bundesliga where champions = '" + sTeamName + "';";
            string Seriesql = "select count(*) from serie_a where champions = '" + sTeamName + "';";
            string EREsql = "select count(*) from eredivisie where champions = '" + sTeamName + "';";
            string porsql = "select count(*) from liga_portugal where champions = '" + sTeamName + "';";
            string laligasql = "select count(*) from laliga_santander where champions = '" + sTeamName + "';";

            NpgsqlConnection conn = null;

            try
            {
                conn = new NpgsqlConnection(MainWindow.mConnString);
                conn.Open();

                NpgsqlCommand Precmd = new NpgsqlCommand(Presql, conn);
                NpgsqlDataReader Prereader = Precmd.ExecuteReader();

                while (Prereader.Read())
                {
                    ints.Add(int.Parse(Prereader[0].ToString().Trim()));
                }

                Prereader.Close();

                NpgsqlCommand Francecmd = new NpgsqlCommand(Francesql, conn);
                NpgsqlDataReader Francereader = Francecmd.ExecuteReader();

                while(Francereader.Read())
                {
                    ints.Add(int.Parse(Francereader[0].ToString().Trim()));
                }

                Francereader.Close();

                NpgsqlCommand Buncmd = new NpgsqlCommand(Bundessql, conn);
                NpgsqlDataReader Bunreader = Buncmd.ExecuteReader();

                while (Bunreader.Read())
                {
                    ints.Add(int.Parse(Bunreader[0].ToString().Trim()));
                }

                Bunreader.Close();

                NpgsqlCommand Seriecmd = new NpgsqlCommand(Seriesql, conn);
                NpgsqlDataReader Sereader = Seriecmd.ExecuteReader();

                while (Sereader.Read())
                {
                    ints.Add(int.Parse(Sereader[0].ToString().Trim()));
                }

                Sereader.Close();

                NpgsqlCommand Erecmd = new NpgsqlCommand(EREsql, conn);
                NpgsqlDataReader Erereader = Erecmd.ExecuteReader();

                while (Erereader.Read())
                {
                    ints.Add(int.Parse(Erereader[0].ToString().Trim()));
                }

                Erereader.Close();

                NpgsqlCommand porcmd = new NpgsqlCommand(porsql, conn);
                NpgsqlDataReader porreader = porcmd.ExecuteReader();

                while (porreader.Read())
                {
                    ints.Add(int.Parse(porreader[0].ToString().Trim()));
                }

                porreader.Close();


                NpgsqlCommand Sancmd = new NpgsqlCommand(laligasql, conn);
                NpgsqlDataReader Sanreader = Sancmd.ExecuteReader();

                while (Sanreader.Read())
                {
                    ints.Add(int.Parse(Sanreader[0].ToString().Trim()));
                }

                Sanreader.Close();

                // 리스트에 있는 값이 다 똑같으면 allEqual == True
                bool allEqual = ints.Distinct().Count() == 1;

                if(allEqual)
                {
                    LeagueCNT_textBox.Text = "0";
                }
                else
                {
                    for (int i = 0; i < ints.Count; i++)
                    {
                        if (ints[i]!=0)
                        {
                            LeagueCNT_textBox.Text = ints[i].ToString();
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            finally
            {
                if (conn != null && conn.State != ConnectionState.Closed)
                {
                    conn.Close();
                    conn.Dispose();
                }
            }
        }

        private void KeyEvent(object sender, KeyEventArgs e)
        {
            if(e.Key == Key.Escape)
            {
                this.Close();
            }
            else if(e.Key == Key.D)
            {
                AllTeam at = new AllTeam();
                at.DataPassProdCd += new AllTeam.DataPassProdCdEventHandler(TeamNameReceive);
                at.WindowStartupLocation = WindowStartupLocation.CenterScreen;
                at.ShowDialog();

                ImageData image = new ImageData();

                BitmapImage bitmap = new BitmapImage(new Uri("Resources/" + m_sTeamName.Trim() + ".png", UriKind.Relative));
                ImageBrush brush = new ImageBrush(bitmap);
                Run_imageRec.Fill = brush;

                SelectFunc(Search_TeamName_textBox.Text);
            }
        }

        private void Team_Search_button_Click(object sender, RoutedEventArgs e)
        {
            AllTeam at = new AllTeam();
            at.DataPassProdCd += new AllTeam.DataPassProdCdEventHandler(TeamNameReceive);
            at.WindowStartupLocation = WindowStartupLocation.CenterScreen;
            at.ShowDialog();

            BitmapImage bitmap = new BitmapImage(new Uri("Resources/" + m_sTeamName.Trim() + ".png", UriKind.Relative));
            ImageBrush brush = new ImageBrush(bitmap);
            Run_imageRec.Fill = brush;

            SelectFunc(Search_TeamName_textBox.Text);
        }


        private void TeamNameReceive(string sTeamName)
        {
            Search_TeamName_textBox.Text = sTeamName;
            m_sTeamName = sTeamName;
        }

        private void Search_TeamName_textBox_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            AllTeam at = new AllTeam();
            at.DataPassProdCd += new AllTeam.DataPassProdCdEventHandler(TeamNameReceive);
            at.WindowStartupLocation = WindowStartupLocation.CenterScreen;
            at.ShowDialog();

            BitmapImage bitmap = new BitmapImage(new Uri("Resources/" + m_sTeamName.Trim() + ".png", UriKind.Relative));
            ImageBrush brush = new ImageBrush(bitmap);
            Run_imageRec.Fill = brush;

            SelectFunc(Search_TeamName_textBox.Text);
        }
        private void Insert_TeamName_textBox_KeyDown(object sender, KeyEventArgs e)
        {
            if(e.Key == Key.Enter)
            {
                if(Insert_TeamName_textBox.Text != string.Empty)
                {
                    BitmapImage bitmap = new BitmapImage(new Uri("Resources/" + Insert_TeamName_textBox.Text.Trim() + ".png", UriKind.Relative));
                    ImageBrush brush = new ImageBrush(bitmap);
                    Run_imageRec.Fill = brush;

                    SelectFunc(Insert_TeamName_textBox.Text);
                }
                else
                {
                    MessageBox.Show("팀 이름을 입력하세요", "Error", MessageBoxButton.OK, MessageBoxImage.Information);
                }
            }
        }

        private void TeamName_TextChanged(object sender, TextChangedEventArgs e)
        {
            TextBox textBox = sender as TextBox;
            textBox.Text = textBox.Text.ToUpper();
            textBox.CaretIndex = textBox.Text.Length;
        }

        private void TeamName_textBox_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            Regex regex = new Regex("[^a-zA-Z0-9\\s]+");
            if (regex.IsMatch(e.Text))
            {
                //e.Handled = !((e.Text[0] >= 'a' && e.Text[0] <= 'z') || (e.Text[0] >= 'A' && e.Text[0] <= 'Z'));
                e.Handled = true;
            }
        }
    }
}
