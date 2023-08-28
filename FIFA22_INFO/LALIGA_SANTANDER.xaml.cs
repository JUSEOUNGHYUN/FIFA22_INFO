using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
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
using Npgsql;

namespace FIFA22_INFO
{
    /// <summary>
    /// LALIGA_SANTANDER.xaml에 대한 상호 작용 논리
    /// </summary>
    /// 
    public class LALIGA_SANTANDER_LEAGUE
    {
        public string SLeague_Year;
        public string SChampions;
        public string SSecond_Place;
        public string SThird_Place;
        public string SFourth_Place;
        public string SRemark;

        public LALIGA_SANTANDER_LEAGUE(string sLeague_Year, string sChampions, string sSecond_Place, string sThird_Place, string sFourth_Place, string sRemark)
        {
            SLeague_Year = sLeague_Year;
            SChampions = sChampions;
            SSecond_Place = sSecond_Place;
            SThird_Place = sThird_Place;
            SFourth_Place = sFourth_Place;
            SRemark = sRemark;
        }
    }

    public class LALIGA_SANTANDER_RANKING
    {
        public string SRRanking;
        public string SRTeam_Name;
        public string SRChampions_CNT;
        public string SRRunnerUp_CNT;
        public string SRThird_CNT;
        public string SRFourth_CNT;

        public LALIGA_SANTANDER_RANKING(string sLeague_Year, string sChampions, string sSecond_Place, string sThird_Place, string sFourth_Place, string sRemark)
        {
            SRRanking = sLeague_Year;
            SRTeam_Name = sChampions;
            SRChampions_CNT = sSecond_Place;
            SRRunnerUp_CNT = sThird_Place;
            SRThird_CNT = sFourth_Place;
            SRFourth_CNT = sRemark;
        }
    }

    public partial class LALIGA_SANTANDER : Window
    {
        public static List<LALIGA_SANTANDER_LEAGUE> mSANLEAGUEList = new List<LALIGA_SANTANDER_LEAGUE>();
        public static List<LALIGA_SANTANDER_RANKING> mSANRANKINGList = new List<LALIGA_SANTANDER_RANKING>();

        public delegate void DataPassProdCdEventHandler(string strTeamName);
        public event DataPassProdCdEventHandler DataPassProdCd;

        public string m_sTeamName = string.Empty;
        public string m_sOption = string.Empty;
        public LALIGA_SANTANDER()
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

        private void Search_Click(object sender, RoutedEventArgs e)
        {
            SearchFunc();
        }

        private void SetDataGridUsingQueryResultList()
        {
            List<Other_League> uiList = new List<Other_League>();

            for (int i = 0; i < mSANLEAGUEList.Count; i++)
            {
                uiList.Add(new Other_League()
                {
                    League_Year = mSANLEAGUEList[i].SLeague_Year.Trim(),
                    ChampionsLOGO = mSANLEAGUEList[i].SChampions.Trim(),
                    Champions = mSANLEAGUEList[i].SChampions.Trim(),
                    Second_PlaceLOGO = mSANLEAGUEList[i].SSecond_Place.Trim(),
                    Second_Place = mSANLEAGUEList[i].SSecond_Place.Trim(),
                    Third_PlaceLOGO = mSANLEAGUEList[i].SThird_Place.Trim(),
                    Third_Place = mSANLEAGUEList[i].SThird_Place.Trim(),
                    Fourth_PlaceLOGO = mSANLEAGUEList[i].SFourth_Place.Trim(),
                    Fourth_Place = mSANLEAGUEList[i].SFourth_Place.Trim(),
                    Remark = mSANLEAGUEList[i].SRemark.Trim(),
                });
            }
            grdEmployee.ItemsSource = uiList;
            grdEmployee.ScrollIntoView(grdEmployee.Items[grdEmployee.Items.Count - 1]);
        }

        private void SetDataGridRankingUI()
        {
            List<Other_League_Ranking_Data> uList = new List<Other_League_Ranking_Data>();

            for (int i = 0; i < mSANRANKINGList.Count; i++)
            {
                LALIGA_SANTANDER_RANKING lur = mSANRANKINGList[i];

                uList.Add(new Other_League_Ranking_Data()
                {
                    Ranking = int.Parse(mSANRANKINGList[i].SRRanking.Trim()),
                    Team_Logo = mSANRANKINGList[i].SRTeam_Name.Trim(),
                    Team_Name = mSANRANKINGList[i].SRTeam_Name.Trim(),
                    Champions_CNT = mSANRANKINGList[i].SRChampions_CNT.Trim(),
                    Second_Place_CNT = mSANRANKINGList[i].SRRunnerUp_CNT.Trim(),
                    Third_Place_CNT = mSANRANKINGList[i].SRThird_CNT.Trim(),
                    Fourth_Place_CNT = mSANRANKINGList[i].SRFourth_CNT.Trim()
                });
            }
            Ranking_DataGrid.ItemsSource = uList;

        }

        private void Row_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            int seelctedIndex = ((DataGrid)sender).SelectedIndex;

            LALIGA_SANTANDER_LEAGUE cc = mSANLEAGUEList[seelctedIndex];
            League_Year_textBox.Text = cc.SLeague_Year.ToString();
            League_Year2_textBox.Text = cc.SLeague_Year.ToLower();
            Champion_Name_Textbox.Text = cc.SChampions.ToString();
            Runner_UP_Textbox.Text = cc.SSecond_Place.ToString();
            Remark_Textbox.Text = cc.SRemark.ToString();

            NpgsqlConnection conn = null;

            try
            {
                conn = new NpgsqlConnection(MainWindow.mConnString);
                conn.Open();

                string sql = "select Count(*) from LALIGA_SANTANDER t where t.champions = '" + Champion_Name_Textbox.Text + "' and t.league_year between '2021/22' and '" + League_Year_textBox.Text + "';";

                NpgsqlCommand RankCmd = new NpgsqlCommand(sql, conn);
                NpgsqlDataReader reader = RankCmd.ExecuteReader();

                string strWInsCount = string.Empty;

                while (reader.Read())
                {
                    strWInsCount = reader[0].ToString();
                }

                NumOfWins_Textbox.Text = strWInsCount;

                reader.Close();

                string Secsql = "select COUNT(*) from LALIGA_SANTANDER T where T.second_place = '" + Runner_UP_Textbox.Text + "' and t.league_year between '2021/22' and '" + League_Year2_textBox.Text + "';";

                NpgsqlCommand SecCmd = new NpgsqlCommand(Secsql, conn);
                NpgsqlDataReader secreader = SecCmd.ExecuteReader();

                string strSecCount = string.Empty;

                while (secreader.Read())
                {
                    strSecCount = secreader[0].ToString();
                }

                NumOfRunner_Up_Textbox_Copy.Text = strSecCount;

                secreader.Close();

                ImageData image = new ImageData();

                image.SECOND_TEAMNAME = Runner_UP_Textbox.Text.Trim();
                image.COUNT = strWInsCount;
                image.TEAMNAME = Champion_Name_Textbox.Text.Trim();

                BitmapImage bitmap = new BitmapImage(new Uri("Image/LALIGA_SANTANDER_Team/" + image.TEAMNAME.ToString() + ".png", UriKind.Relative));
                ImageBrush brush = new ImageBrush(bitmap);
                imageRec.Fill = brush;

                BitmapImage runbit = new BitmapImage(new Uri("Image/LALIGA_SANTANDER_Team/" + image.SECOND_TEAMNAME.ToLower() + ".png", UriKind.Relative));
                ImageBrush runbrush = new ImageBrush(runbit);
                Run_imageRec.Fill = runbrush;

                int nCount = int.Parse(image.COUNT);

                if (nCount >= 10 && nCount % 10 == 0)
                {
                    BitmapImage bit = new BitmapImage(new Uri("Image/Star/" + image.COUNT.ToString() + ".png", UriKind.Relative));
                    ImageBrush br = new ImageBrush(bit);
                    CountStarRec.Fill = br;
                }
                else
                {
                    BitmapImage bit = new BitmapImage(new Uri("Image/Star/Empty.png", UriKind.Relative));
                    ImageBrush br = new ImageBrush(bit);
                    CountStarRec.Fill = br;
                }

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

        private void SearchFunc()
        {
            NpgsqlConnection conn = new NpgsqlConnection(MainWindow.mConnString);
            conn.Open();
            List<string> list = new List<string>();

            string sql = "select * from LALIGA_SANTANDER lue order by league_year";

            NpgsqlCommand cmd = new NpgsqlCommand(sql, conn);
            NpgsqlDataReader reader = cmd.ExecuteReader();

            mSANLEAGUEList.Clear();

            while (reader.Read())
            {
                LALIGA_SANTANDER_LEAGUE lu = new LALIGA_SANTANDER_LEAGUE(reader[0].ToString(),
                    reader[1].ToString(),
                    reader[2].ToString(),
                    reader[3].ToString(),
                    reader[4].ToString(),
                    reader[5].ToString());

                mSANLEAGUEList.Add(lu);
            }

            SetDataGridUsingQueryResultList();

            reader.Close();

            // DataGrid에 있는 최선 정보 txt파일에 저장
            string sOption = "LALIGA_SANTANDER";

            string path = AppDomain.CurrentDomain.BaseDirectory + "\\Scripts\\" + sOption + ".txt";

            FileStream stream = File.Create(path);
            stream.Close();

            if (File.Exists(path))
            {
                for (int i = 0; i < mSANLEAGUEList.Count; i++)
                {
                    File.AppendAllText(path, mSANLEAGUEList[i].SLeague_Year.Trim() + "\n");
                    File.AppendAllText(path, mSANLEAGUEList[i].SChampions.Trim() + "\n");
                    File.AppendAllText(path, mSANLEAGUEList[i].SSecond_Place.Trim() + "\n");
                    File.AppendAllText(path, mSANLEAGUEList[i].SThird_Place.Trim() + "\n");
                    File.AppendAllText(path, mSANLEAGUEList[i].SFourth_Place.Trim() + "\n");
                    File.AppendAllText(path, mSANLEAGUEList[i].SRemark.Trim() + "\n");
                }
            }

            string Ranksql = "select rank() over( order by t.CHAMPIONS_CNT desc) Ranking, t.team_name, t.CHAMPIONS_CNT , t.SECOND_PLACE_CNT,  " +
                "t.THIRD_PLACE_CNT, t.FOURTH_PLACE_CNT from LALIGA_SANTANDER_RANKING t " +
                "where t.THIRD_PLACE_CNT != 0 or t.FOURTH_PLACE_CNT != 0 or t.SECOND_PLACE_CNT != 0 OR t.CHAMPIONS_CNT != 0 " +
                "order by t.CHAMPIONS_CNT desc, " +
                "t.SECOND_PLACE_CNT desc, t.THIRD_PLACE_CNT desc, t.FOURTH_PLACE_CNT  desc";

            NpgsqlCommand RankCmd = new NpgsqlCommand(Ranksql, conn);
            NpgsqlDataReader Rankreader = RankCmd.ExecuteReader();

            mSANRANKINGList.Clear();

            while (Rankreader.Read())
            {
                LALIGA_SANTANDER_RANKING ccr = new LALIGA_SANTANDER_RANKING(Rankreader[0].ToString(),
                    Rankreader[1].ToString(),
                    Rankreader[2].ToString(),
                    Rankreader[3].ToString(),
                    Rankreader[4].ToString(),
                    Rankreader[5].ToString());

                mSANRANKINGList.Add(ccr);
            }

            SetDataGridRankingUI();

            Rankreader.Close();
        }

        private void keyDown_Event(object sender, KeyEventArgs e)
        {
            if(e.Key == Key.Escape)
            {
                this.Close();
            }
            else if(e.Key == Key.S)
            {
                SearchFunc();
            }
            else if (e.Key == Key.C)
            {
                this.Close();
                LIGUE1 b = new LIGUE1();
                b.WindowStartupLocation = WindowStartupLocation.CenterScreen;
                b.Show();
            }
            else if (e.Key == Key.X)
            {
                this.Close();
                LIGA_PORTUGAL ls = new LIGA_PORTUGAL();
                ls.WindowStartupLocation = WindowStartupLocation.CenterScreen;
                ls.Show();
            }
        }

        private void Ranking_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            int selectedIndex = Ranking_DataGrid.SelectedIndex;
            LALIGA_SANTANDER_RANKING ccr = mSANRANKINGList[selectedIndex];

            m_sTeamName = ccr.SRTeam_Name.Trim();
            m_sOption = "LALIGA_SANTANDER";

            //DataPassProdCd(teamName);
            TeamCareer tc = new TeamCareer();
            tc.WindowStartupLocation = WindowStartupLocation.CenterScreen;
            tc.Show();
            tc.GetTeam(m_sTeamName, m_sOption);
        }

        private void Ranking_DataGrid_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            int selectedIndex = Ranking_DataGrid.SelectedIndex;
            LALIGA_SANTANDER_RANKING ccr = mSANRANKINGList[selectedIndex];

            m_sTeamName = ccr.SRTeam_Name.Trim();
            m_sOption = "LALIGA_SANTANDER";

            if(e.Key == Key.Enter)
            {
                //DataPassProdCd(teamName);
                TeamCareer tc = new TeamCareer();
                tc.WindowStartupLocation = WindowStartupLocation.CenterScreen;
                tc.Show();
                tc.GetTeam(m_sTeamName, m_sOption);
            }
        }
    }
}
