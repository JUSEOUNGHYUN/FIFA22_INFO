using Npgsql;
using System;
using System.Collections.Generic;
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
            
            //List<OptionNameColor> cs = new List<OptionNameColor>();
            OptionNameColor cs = new OptionNameColor();
            cs.OptionName = Option_textBox.Text;
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
            string championssql = "select * from "+ Option_textBox .Text+ " where "+ sRanking + " = '" + teamname + "' order by league_year;";
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
            else if(e.Key == Key.C)
            {
                DBSelect("Champions");
            }
            else if(e.Key == Key.R)
            {
                DBSelect("second_place");
            }
        }

    }
}
