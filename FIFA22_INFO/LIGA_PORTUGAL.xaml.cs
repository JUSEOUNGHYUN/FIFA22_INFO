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
    /// LIGA_PORTUGAL.xaml에 대한 상호 작용 논리
    /// </summary>
    /// 
    public class LIGA_PORTUGAL_LEAGUE
    {
        public string PLeague_Year;
        public string PChampions;
        public string PSecond_Place;
        public string PThird_Place;
        public string PFourth_Place;
        public string PRemark;

        public LIGA_PORTUGAL_LEAGUE(string pLeague_Year, string pChampions, string pSecond_Place, string pThird_Place, string pFourth_Place, string pRemark)
        {
            PLeague_Year = pLeague_Year;
            PChampions = pChampions;
            PSecond_Place = pSecond_Place;
            PThird_Place = pThird_Place;
            PFourth_Place = pFourth_Place;
            PRemark = pRemark;
        }
    }

    public class LIGA_PORTUGAL_RANKING
    {
        public string PRRanking;
        public string PRTeam_Name;
        public string PRChampions_CNT;
        public string PRRunnerUp_CNT;
        public string PRThird_CNT;
        public string PRFourth_CNT;

        public LIGA_PORTUGAL_RANKING(string pRRanking, string pRTeam_Name, string pRChampions_CNT, string pRRunnerUp_CNT, string pRThird_CNT, string pRFourth_CNT)
        {
            PRRanking = pRRanking;
            PRTeam_Name = pRTeam_Name;
            PRChampions_CNT = pRChampions_CNT;
            PRRunnerUp_CNT = pRRunnerUp_CNT;
            PRThird_CNT = pRThird_CNT;
            PRFourth_CNT = pRFourth_CNT;
        }

    }
    public partial class LIGA_PORTUGAL : Window
    {
        public static List<LIGA_PORTUGAL_LEAGUE> mPORLEAGUEList = new List<LIGA_PORTUGAL_LEAGUE>();
        public static List<LIGA_PORTUGAL_RANKING> mPORRANKINGList = new List<LIGA_PORTUGAL_RANKING>();

        public delegate void DataPassProdCdEventHandler(string strTeamName);
        public event DataPassProdCdEventHandler DataPassProdCd;

        public string m_sTeamName = string.Empty;
        public string m_sOption = string.Empty;
        public LIGA_PORTUGAL()
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

            for (int i = 0; i < mPORLEAGUEList.Count; i++)
            {
                uiList.Add(new Other_League()
                {
                    League_Year = mPORLEAGUEList[i].PLeague_Year.Trim(),
                    ChampionsLOGO = mPORLEAGUEList[i].PChampions.Trim(),
                    Champions = mPORLEAGUEList[i].PChampions.Trim(),
                    Second_PlaceLOGO = mPORLEAGUEList[i].PSecond_Place.Trim(),
                    Second_Place = mPORLEAGUEList[i].PSecond_Place.Trim(),
                    Third_PlaceLOGO = mPORLEAGUEList[i].PThird_Place.Trim(),
                    Third_Place = mPORLEAGUEList[i].PThird_Place.Trim(),
                    Fourth_PlaceLOGO = mPORLEAGUEList[i].PFourth_Place.Trim(),
                    Fourth_Place = mPORLEAGUEList[i].PFourth_Place.Trim(),
                    Remark = mPORLEAGUEList[i].PRemark.Trim(),
                });
            }
            grdEmployee.ItemsSource = uiList;
            grdEmployee.ScrollIntoView(grdEmployee.Items[grdEmployee.Items.Count - 1]);
        }

        private void SetDataGridRankingUI()
        {
            List<Other_League_Ranking_Data> uList = new List<Other_League_Ranking_Data>();

            for (int i = 0; i < mPORRANKINGList.Count; i++)
            {
                LIGA_PORTUGAL_RANKING lur = mPORRANKINGList[i];

                uList.Add(new Other_League_Ranking_Data()
                {
                    Ranking = int.Parse(mPORRANKINGList[i].PRRanking.Trim()),
                    Team_Logo = mPORRANKINGList[i].PRTeam_Name.Trim(),
                    Team_Name = mPORRANKINGList[i].PRTeam_Name.Trim(),
                    Champions_CNT = mPORRANKINGList[i].PRChampions_CNT.Trim(),
                    Second_Place_CNT = mPORRANKINGList[i].PRRunnerUp_CNT.Trim(),
                    Third_Place_CNT = mPORRANKINGList[i].PRThird_CNT.Trim(),
                    Fourth_Place_CNT = mPORRANKINGList[i].PRFourth_CNT.Trim()
                });
            }
            Ranking_DataGrid.ItemsSource = uList;

        }

        private void Row_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            int seelctedIndex = ((DataGrid)sender).SelectedIndex;

            LIGA_PORTUGAL_LEAGUE cc = mPORLEAGUEList[seelctedIndex];
            League_Year_textBox.Text = cc.PLeague_Year.ToString();
            League_Year2_textBox.Text = cc.PLeague_Year.ToLower();
            Champion_Name_Textbox.Text = cc.PChampions.ToString();
            Runner_UP_Textbox.Text = cc.PSecond_Place.ToString();
            Remark_Textbox.Text = cc.PRemark.ToString();

            NpgsqlConnection conn = null;

            try
            {
                conn = new NpgsqlConnection(MainWindow.mConnString);
                conn.Open();

                string sql = "select Count(*) from LIGA_PORTUGAL t where t.champions = '" + Champion_Name_Textbox.Text + "' and t.league_year between '2021/22' and '" + League_Year_textBox.Text + "';";

                NpgsqlCommand RankCmd = new NpgsqlCommand(sql, conn);
                NpgsqlDataReader reader = RankCmd.ExecuteReader();

                string strWInsCount = string.Empty;

                while (reader.Read())
                {
                    strWInsCount = reader[0].ToString();
                }

                NumOfWins_Textbox.Text = strWInsCount;

                reader.Close();

                string Secsql = "select COUNT(*) from LIGA_PORTUGAL T where T.second_place = '" + Runner_UP_Textbox.Text + "' and t.league_year between '2021/22' and '" + League_Year2_textBox.Text + "';";

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

                BitmapImage bitmap = new BitmapImage(new Uri("Image/LIGA_PORTUGAL_Team/" + image.TEAMNAME.ToString() + ".png", UriKind.Relative));
                ImageBrush brush = new ImageBrush(bitmap);
                imageRec.Fill = brush;

                BitmapImage runbit = new BitmapImage(new Uri("Image/LIGA_PORTUGAL_Team/" + image.SECOND_TEAMNAME.ToLower() + ".png", UriKind.Relative));
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

            string sql = "select * from LIGA_PORTUGAL lue order by league_year";

            NpgsqlCommand cmd = new NpgsqlCommand(sql, conn);
            NpgsqlDataReader reader = cmd.ExecuteReader();

            mPORLEAGUEList.Clear();

            while (reader.Read())
            {
                LIGA_PORTUGAL_LEAGUE lu = new LIGA_PORTUGAL_LEAGUE(reader[0].ToString(),
                    reader[1].ToString(),
                    reader[2].ToString(),
                    reader[3].ToString(),
                    reader[4].ToString(),
                    reader[5].ToString());

                mPORLEAGUEList.Add(lu);
            }

            SetDataGridUsingQueryResultList();

            reader.Close();

            // DataGrid에 있는 최선 정보 txt파일에 저장
            string sOption = "LIGA_PORTUGAL";

            string path = AppDomain.CurrentDomain.BaseDirectory + "\\Scripts\\" + sOption + ".txt";

            FileStream stream = File.Create(path);
            stream.Close();

            if (File.Exists(path))
            {
                for (int i = 0; i < mPORLEAGUEList.Count; i++)
                {
                    File.AppendAllText(path, mPORLEAGUEList[i].PLeague_Year.Trim() + "\n");
                    File.AppendAllText(path, mPORLEAGUEList[i].PChampions.Trim() + "\n");
                    File.AppendAllText(path, mPORLEAGUEList[i].PSecond_Place.Trim() + "\n");
                    File.AppendAllText(path, mPORLEAGUEList[i].PThird_Place.Trim() + "\n");
                    File.AppendAllText(path, mPORLEAGUEList[i].PFourth_Place.Trim() + "\n");
                    File.AppendAllText(path, mPORLEAGUEList[i].PRemark.Trim() + "\n");
                }
            }

            string Ranksql = "select rank() over( order by t.CHAMPIONS_CNT desc) Ranking, t.team_name, t.CHAMPIONS_CNT , t.SECOND_PLACE_CNT,  " +
                "t.THIRD_PLACE_CNT, t.FOURTH_PLACE_CNT from LIGA_PORTUGAL_RANKING t " +
                "where t.THIRD_PLACE_CNT != 0 or t.FOURTH_PLACE_CNT != 0 or t.SECOND_PLACE_CNT != 0 OR t.CHAMPIONS_CNT != 0 " +
                "order by t.CHAMPIONS_CNT desc, " +
                "t.SECOND_PLACE_CNT desc, t.THIRD_PLACE_CNT desc, t.FOURTH_PLACE_CNT  desc";

            NpgsqlCommand RankCmd = new NpgsqlCommand(Ranksql, conn);
            NpgsqlDataReader Rankreader = RankCmd.ExecuteReader();

            mPORRANKINGList.Clear();

            while (Rankreader.Read())
            {
                LIGA_PORTUGAL_RANKING ccr = new LIGA_PORTUGAL_RANKING(Rankreader[0].ToString(),
                    Rankreader[1].ToString(),
                    Rankreader[2].ToString(),
                    Rankreader[3].ToString(),
                    Rankreader[4].ToString(),
                    Rankreader[5].ToString());

                mPORRANKINGList.Add(ccr);
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
                LALIGA_SANTANDER b = new LALIGA_SANTANDER();
                b.WindowStartupLocation = WindowStartupLocation.CenterScreen;
                b.Show();
            }
            else if (e.Key == Key.X)
            {
                this.Close();
                SERIE_A ls = new SERIE_A();
                ls.WindowStartupLocation = WindowStartupLocation.CenterScreen;
                ls.Show();
            }
        }

        private void Ranking_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            int selectedIndex = Ranking_DataGrid.SelectedIndex;
            LIGA_PORTUGAL_RANKING ccr = mPORRANKINGList[selectedIndex];

            m_sTeamName = ccr.PRTeam_Name.Trim();
            m_sOption = "LIGA_PORTUGAL";

            //DataPassProdCd(teamName);
            TeamCareer tc = new TeamCareer();
            tc.WindowStartupLocation = WindowStartupLocation.CenterScreen;
            tc.Show();
            tc.GetTeam(m_sTeamName, m_sOption);
        }

        private void Ranking_DataGrid_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            int selectedIndex = Ranking_DataGrid.SelectedIndex;
            LIGA_PORTUGAL_RANKING ccr = mPORRANKINGList[selectedIndex];

            m_sTeamName = ccr.PRTeam_Name.Trim();
            m_sOption = "LIGA_PORTUGAL";

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
