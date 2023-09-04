using Npgsql;
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

namespace FIFA22_INFO
{
    /// <summary>
    /// EREDIVISIE.xaml에 대한 상호 작용 논리
    /// </summary>
    /// 
    public class EREDIVISIE_LEAGUE
    {
        public string ELeague_Year;
        public string EChampions;
        public string ESecond_Place;
        public string EThird_Place;
        public string EFourth_Place;
        public string ERemark;

        public EREDIVISIE_LEAGUE(string eLeague_Year, string eChampions, string eSecond_Place, string eThird_Place, string eFourth_Place, string eRemark)
        {
            ELeague_Year = eLeague_Year;
            EChampions = eChampions;
            ESecond_Place = eSecond_Place;
            EThird_Place = eThird_Place;
            EFourth_Place = eFourth_Place;
            ERemark = eRemark;
        }
    }

    public class EREDIVISIE_RANKING
    {
        public string ERRanking;
        public string ERTeam_Name;
        public string ERChampions_CNT;
        public string ERRunnerUp_CNT;
        public string ERThird_CNT;
        public string ERFourth_CNT;

        public EREDIVISIE_RANKING(string eRRanking, string eRTeam_Name, string eRChampions_CNT, string eRRunnerUp_CNT, string eRThird_CNT, string eRFourth_CNT)
        {
            ERRanking = eRRanking;
            ERTeam_Name = eRTeam_Name;
            ERChampions_CNT = eRChampions_CNT;
            ERRunnerUp_CNT = eRRunnerUp_CNT;
            ERThird_CNT = eRThird_CNT;
            ERFourth_CNT = eRFourth_CNT;
        }

    }


    public partial class EREDIVISIE : Window
    {
        public static List<EREDIVISIE_LEAGUE> mERELEAGUEList = new List<EREDIVISIE_LEAGUE>();
        public static List<EREDIVISIE_RANKING> mERERANKINGList = new List<EREDIVISIE_RANKING>();


        public string m_sTeamName = string.Empty;
        public string m_sOption = string.Empty;

        public EREDIVISIE()
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

            for (int i = 0; i < mERELEAGUEList.Count; i++)
            {
                uiList.Add(new Other_League()
                {
                    League_Year = mERELEAGUEList[i].ELeague_Year.Trim(),
                    ChampionsLOGO = mERELEAGUEList[i].EChampions.Trim(),
                    Champions = mERELEAGUEList[i].EChampions.Trim(),
                    Second_PlaceLOGO = mERELEAGUEList[i].ESecond_Place.Trim(),
                    Second_Place = mERELEAGUEList[i].ESecond_Place.Trim(),
                    Third_PlaceLOGO = mERELEAGUEList[i].EThird_Place.Trim(),
                    Third_Place = mERELEAGUEList[i].EThird_Place.Trim(),
                    Fourth_PlaceLOGO = mERELEAGUEList[i].EFourth_Place.Trim(),
                    Fourth_Place = mERELEAGUEList[i].EFourth_Place.Trim(),
                    Remark = mERELEAGUEList[i].ERemark.Trim(),
                });
            }
            grdEmployee.ItemsSource = uiList;
            grdEmployee.ScrollIntoView(grdEmployee.Items[grdEmployee.Items.Count - 1]);
        }

        private void SetDataGridRankingUI()
        {
            List<Other_League_Ranking_Data> uList = new List<Other_League_Ranking_Data>();

            for (int i = 0; i < mERERANKINGList.Count; i++)
            {
                EREDIVISIE_RANKING lur = mERERANKINGList[i];

                uList.Add(new Other_League_Ranking_Data()
                {
                    Ranking = int.Parse(mERERANKINGList[i].ERRanking.Trim()),
                    Team_Logo = mERERANKINGList[i].ERTeam_Name.Trim(),
                    Team_Name = mERERANKINGList[i].ERTeam_Name.Trim(),
                    Champions_CNT = mERERANKINGList[i].ERChampions_CNT.Trim(),
                    Second_Place_CNT = mERERANKINGList[i].ERRunnerUp_CNT.Trim(),
                    Third_Place_CNT = mERERANKINGList[i].ERThird_CNT.Trim(),
                    Fourth_Place_CNT = mERERANKINGList[i].ERFourth_CNT.Trim()
                });
            }
            Ranking_DataGrid.ItemsSource = uList;

        }

        private void Row_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            int seelctedIndex = ((DataGrid)sender).SelectedIndex;

            EREDIVISIE_LEAGUE cc = mERELEAGUEList[seelctedIndex];
            League_Year_textBox.Text = cc.ELeague_Year.ToString();
            League_Year2_textBox.Text = cc.ELeague_Year.ToLower();
            Champion_Name_Textbox.Text = cc.EChampions.ToString();
            Runner_UP_Textbox.Text = cc.ESecond_Place.ToString();
            Remark_Textbox.Text = cc.ERemark.ToString();

            NpgsqlConnection conn = null;

            try
            {
                conn = new NpgsqlConnection(MainWindow.mConnString);
                conn.Open();

                string sql = "select Count(*) from EREDIVISIE t where t.champions = '" + Champion_Name_Textbox.Text + "' and t.league_year between '2021/22' and '" + League_Year_textBox.Text + "';";

                NpgsqlCommand RankCmd = new NpgsqlCommand(sql, conn);
                NpgsqlDataReader reader = RankCmd.ExecuteReader();

                string strWInsCount = string.Empty;

                while (reader.Read())
                {
                    strWInsCount = reader[0].ToString();
                }

                NumOfWins_Textbox.Text = strWInsCount;

                reader.Close();

                string Secsql = "select COUNT(*) from EREDIVISIE T where T.second_place = '" + Runner_UP_Textbox.Text + "' and t.league_year between '2021/22' and '" + League_Year2_textBox.Text + "';";

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

                BitmapImage bitmap = new BitmapImage(new Uri("Image/EREDIVISIE_Team/" + image.TEAMNAME.ToString() + ".png", UriKind.Relative));
                ImageBrush brush = new ImageBrush(bitmap);
                imageRec.Fill = brush;

                BitmapImage runbit = new BitmapImage(new Uri("Image/EREDIVISIE_Team/" + image.SECOND_TEAMNAME.ToLower() + ".png", UriKind.Relative));
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

            string sql = "select * from EREDIVISIE lue order by league_year";

            NpgsqlCommand cmd = new NpgsqlCommand(sql, conn);
            NpgsqlDataReader reader = cmd.ExecuteReader();

            mERELEAGUEList.Clear();

            while (reader.Read())
            {
                EREDIVISIE_LEAGUE lu = new EREDIVISIE_LEAGUE(reader[0].ToString(),
                    reader[1].ToString(),
                    reader[2].ToString(),
                    reader[3].ToString(),
                    reader[4].ToString(),
                    reader[5].ToString());

                mERELEAGUEList.Add(lu);
            }

            SetDataGridUsingQueryResultList();

            reader.Close();

            // DataGrid에 있는 최선 정보 txt파일에 저장
            string sOption = "EREDIVISIE";

            string path = AppDomain.CurrentDomain.BaseDirectory + "\\Scripts\\" + sOption + ".txt";

            FileStream stream = File.Create(path);
            stream.Close();

            if (File.Exists(path))
            {
                for (int i = 0; i < mERELEAGUEList.Count; i++)
                {
                    File.AppendAllText(path, mERELEAGUEList[i].ELeague_Year.Trim() + "\n");
                    File.AppendAllText(path, mERELEAGUEList[i].EChampions.Trim() + "\n");
                    File.AppendAllText(path, mERELEAGUEList[i].ESecond_Place.Trim() + "\n");
                    File.AppendAllText(path, mERELEAGUEList[i].EThird_Place.Trim() + "\n");
                    File.AppendAllText(path, mERELEAGUEList[i].EFourth_Place.Trim() + "\n");
                    File.AppendAllText(path, mERELEAGUEList[i].ERemark.Trim() + "\n");
                }
            }

            string Ranksql = "select rank() over( order by t.CHAMPIONS_CNT desc) Ranking, t.team_name, t.CHAMPIONS_CNT , t.SECOND_PLACE_CNT,  " +
                "t.THIRD_PLACE_CNT, t.FOURTH_PLACE_CNT from EREDIVISIE_RANKING t " +
                "where t.THIRD_PLACE_CNT != 0 or t.FOURTH_PLACE_CNT != 0 or t.SECOND_PLACE_CNT != 0 OR t.CHAMPIONS_CNT != 0 " +
                "order by t.CHAMPIONS_CNT desc, " +
                "t.SECOND_PLACE_CNT desc, t.THIRD_PLACE_CNT desc, t.FOURTH_PLACE_CNT  desc";

            NpgsqlCommand RankCmd = new NpgsqlCommand(Ranksql, conn);
            NpgsqlDataReader Rankreader = RankCmd.ExecuteReader();

            mERERANKINGList.Clear();

            while (Rankreader.Read())
            {
                EREDIVISIE_RANKING ccr = new EREDIVISIE_RANKING(Rankreader[0].ToString(),
                    Rankreader[1].ToString(),
                    Rankreader[2].ToString(),
                    Rankreader[3].ToString(),
                    Rankreader[4].ToString(),
                    Rankreader[5].ToString());

                mERERANKINGList.Add(ccr);
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
                LIGA_PORTUGAL b = new LIGA_PORTUGAL();
                b.WindowStartupLocation = WindowStartupLocation.CenterScreen;
                b.Show();
            }
            else if (e.Key == Key.X)
            {
                SERIE_A ls = new SERIE_A();
                ls.WindowStartupLocation = WindowStartupLocation.CenterScreen;
                ls.Show();
            }
        }

        private void Ranking_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            int selectedIndex = Ranking_DataGrid.SelectedIndex;
            EREDIVISIE_RANKING ccr = mERERANKINGList[selectedIndex];

            m_sTeamName = ccr.ERTeam_Name.Trim();
            m_sOption = "EREDIVISIE";

            //DataPassProdCd(teamName);
            TeamCareer tc = new TeamCareer();
            tc.WindowStartupLocation = WindowStartupLocation.CenterScreen;
            tc.Show();
            tc.GetTeam(m_sTeamName, m_sOption);
        }

        private void Ranking_DataGrid_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            int selectedIndex = Ranking_DataGrid.SelectedIndex;
            EREDIVISIE_RANKING ccr = mERERANKINGList[selectedIndex];

            m_sTeamName = ccr.ERTeam_Name.Trim();
            m_sOption = "EREDIVISIE";

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
