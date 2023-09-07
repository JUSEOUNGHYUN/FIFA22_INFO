using Npgsql;
using System;
using System.Collections.Generic;
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
using System.Windows.Shapes;

namespace FIFA22_INFO
{
    /// <summary>
    /// Year.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class Year : Window
    {
        public Year()
        {
            InitializeComponent();
            GetLeagueYear();
        }

        private void GetLeagueYear()
        {
            NpgsqlConnection conn = null;

            try
            {
                conn = new NpgsqlConnection(MainWindow.mConnString);
                conn.Open();

                string sql = "select league_year from champions_league order by league_year;";

                NpgsqlCommand cmd = new NpgsqlCommand(sql, conn);
                NpgsqlDataReader reader = cmd.ExecuteReader();

                List<string> yearList = new List<string>();

                while (reader.Read())
                {
                    yearList.Add(reader[0].ToString().Trim());
                }

                reader.Close();

                for (int i = 0; i < yearList.Count; i++)
                {
                    League_Year_comboBox.Items.Add(yearList[i]);
                }
                League_Year_comboBox.SelectedIndex = 0;

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

        private void LeagueYear_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            LeagueYear_textbox.GetLeagueYear(League_Year_comboBox.SelectedItem.ToString());
        }

        private void button_Click(object sender, RoutedEventArgs e)
        {
            SelectFunc();
        }

        private void SelectFunc()
        {
            string sYear = LeagueYear_textbox.SetLeagueYear();

            if (sYear != null)
            {
                NpgsqlConnection conn = null;

                try
                {
                    conn = new NpgsqlConnection(MainWindow.mConnString);
                    conn.Open();

                    string sChapions = string.Empty;
                    string championsSql = "select champions from champions_league where league_year = '" + sYear + "';";

                    NpgsqlCommand cmd = new NpgsqlCommand(championsSql, conn);
                    NpgsqlDataReader reader = cmd.ExecuteReader();

                    while (reader.Read())
                    {
                        sChapions = reader[0].ToString().Trim();
                    }

                    reader.Close();

                    BitmapImage bitmap = new BitmapImage(new Uri("Resources/" + sChapions + ".png", UriKind.Relative));
                    ImageBrush brush = new ImageBrush(bitmap);
                    ChampionsLeague_image.Fill = brush;
                    Champions_League_textBox.Text = sChapions;

                    string sEuropa = string.Empty;
                    string EuropaSql = "select champions from Europa_League where league_year = '" + sYear + "';";

                    NpgsqlCommand cmd1 = new NpgsqlCommand(EuropaSql, conn);
                    NpgsqlDataReader reader1 = cmd1.ExecuteReader();

                    while (reader1.Read())
                    {
                        sEuropa = reader1[0].ToString().Trim();
                    }

                    reader1.Close();

                    BitmapImage bitmap1 = new BitmapImage(new Uri("Resources/" + sEuropa + ".png", UriKind.Relative));
                    ImageBrush brush1 = new ImageBrush(bitmap1);
                    EuropaLeague_image.Fill = brush1;
                    Europa_League_textBox.Text = sEuropa;

                    string sConference = string.Empty;
                    string ConferenceSql = "select champions from Conference_League where league_year = '" + sYear + "';";

                    NpgsqlCommand cmd2 = new NpgsqlCommand(ConferenceSql, conn);
                    NpgsqlDataReader reader2 = cmd2.ExecuteReader();

                    while (reader2.Read())
                    {
                        sConference = reader2[0].ToString().Trim();
                    }

                    reader2.Close();

                    BitmapImage bitmap2 = new BitmapImage(new Uri("Resources/" + sConference + ".png", UriKind.Relative));
                    ImageBrush brush2 = new ImageBrush(bitmap2);
                    ConferenceLeague_image.Fill = brush2;
                    Conference_League_textBox.Text = sConference;

                    string sSuperCup = string.Empty;
                    string SuperCupSql = "select champions from Super_cup where league_year = '" + sYear + "';";

                    NpgsqlCommand cmd3 = new NpgsqlCommand(SuperCupSql, conn);
                    NpgsqlDataReader reader3 = cmd3.ExecuteReader();

                    while (reader3.Read())
                    {
                        sSuperCup = reader3[0].ToString().Trim();
                    }

                    reader3.Close();

                    BitmapImage bitmap3 = new BitmapImage(new Uri("Resources/" + sSuperCup + ".png", UriKind.Relative));
                    ImageBrush brush3 = new ImageBrush(bitmap3);
                    SuperCup_image.Fill = brush3;
                    SuperCup_textBox.Text = sSuperCup;


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

        private void LeagueYear_KeyEvent(object sender, KeyEventArgs e)
        {
            if(e.Key == Key.Enter)
            {
                SelectFunc();
            }
        }
    }
}
