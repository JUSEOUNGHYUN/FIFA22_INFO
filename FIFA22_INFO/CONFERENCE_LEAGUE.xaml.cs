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
    /// CONFERENCE_LEAGUE.xaml에 대한 상호 작용 논리
    /// </summary>
    /// 
    public class CONFERENCE
    {
        public string CFLeague_Year;
        public string CFChampions;
        public string CFSecond_Place;
        public string CFRemark;

        public CONFERENCE(string cFLeague_Year, string cFChampions, string cFSecond_Place, string cFRemark)
        {
            CFLeague_Year = cFLeague_Year;
            CFChampions = cFChampions;
            CFSecond_Place = cFSecond_Place;
            CFRemark = cFRemark;
        }
    }

    public class CONFERENCE_RANKING
    {
        public string CFRRanking;
        public string CFRTeam_Name;
        public string CFRChampions_CNT;
        public string CFRRunnerUp_CNT;

        public CONFERENCE_RANKING(string cFRRanking, string cFRTeam_Name, string cFRChampions_CNT, string cFRRunnerUp_CNT)
        {
            CFRRanking = cFRRanking;
            CFRTeam_Name = cFRTeam_Name;
            CFRChampions_CNT = cFRChampions_CNT;
            CFRRunnerUp_CNT = cFRRunnerUp_CNT;
        }
    }

    public partial class CONFERENCE_LEAGUE : Window
    {
        public static List<CONFERENCE> mCONFERENCEList = new List<CONFERENCE>();
        public static List<CONFERENCE_RANKING> mCONFERENCERANKINGList = new List<CONFERENCE_RANKING>();

        public string m_sTeamName = string.Empty;
        public string m_sOption = string.Empty;

        public CONFERENCE_LEAGUE()
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

        private void SearchFunc()
        {
            NpgsqlConnection conn = new NpgsqlConnection(MainWindow.mConnString);
            conn.Open();
            List<string> list = new List<string>();

            string sql = "select * from CONFERENCE_LEAGUE t order by league_year";

            NpgsqlCommand cmd = new NpgsqlCommand(sql, conn);
            NpgsqlDataReader reader = cmd.ExecuteReader();

            mCONFERENCEList.Clear();

            while (reader.Read())
            {
                CONFERENCE cc = new CONFERENCE(reader[0].ToString(),
                    reader[1].ToString(),
                    reader[2].ToString(),
                    reader[3].ToString());

                mCONFERENCEList.Add(cc);
            }

            SetDataGridUsingQueryResultList();

            reader.Close();


            // DataGrid에 있는 최선 정보 txt파일에 저장
            string sOption = "CONFERENCE_LEAGUE";

            string path = AppDomain.CurrentDomain.BaseDirectory + "\\Scripts\\" + sOption + ".txt";

            try
            {
                FileStream stream = File.Create(path);
                stream.Close();

                if (File.Exists(path))
                {
                    for (int i = 0; i < mCONFERENCEList.Count; i++)
                    {
                        File.AppendAllText(path, mCONFERENCEList[i].CFLeague_Year.Trim() + "\n");
                        File.AppendAllText(path, mCONFERENCEList[i].CFChampions.Trim() + "\n");
                        File.AppendAllText(path, mCONFERENCEList[i].CFSecond_Place.Trim() + "\n");
                        File.AppendAllText(path, mCONFERENCEList[i].CFRemark.Trim() + "\n");
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);

            }

            string Ranksql = "select rank() over( order by t.CONFERENCE_LEAGUE_WINNER_CNT desc) Ranking, t.team_name, t.CONFERENCE_LEAGUE_WINNER_CNT , t.CONFERENCE_LEAGUE_RUN_CNT  " +
              "from TOTAL t " +
              "where t.CONFERENCE_LEAGUE_RUN_CNT != 0 OR t.CONFERENCE_LEAGUE_WINNER_CNT != 0 " +
              "order by t.CONFERENCE_LEAGUE_WINNER_CNT desc, " +
              "t.CONFERENCE_LEAGUE_RUN_CNT desc;";

            NpgsqlCommand RankCmd = new NpgsqlCommand(Ranksql, conn);
            NpgsqlDataReader Rankreader = RankCmd.ExecuteReader();

            mCONFERENCERANKINGList.Clear();

            while (Rankreader.Read())
            {
                CONFERENCE_RANKING ccr = new CONFERENCE_RANKING(Rankreader[0].ToString(),
                    Rankreader[1].ToString(),
                    Rankreader[2].ToString(),
                    Rankreader[3].ToString());

                mCONFERENCERANKINGList.Add(ccr);
            }

            SetDataGridRankingUI();

            Rankreader.Close();
        }

        private void SetDataGridUsingQueryResultList()
        {
            List<Premier_League_Data> uiList = new List<Premier_League_Data>();

            for (int i = 0; i < mCONFERENCEList.Count; i++)
            {
                CONFERENCE cl = mCONFERENCEList[i];

                uiList.Add(new Premier_League_Data()
                {
                    League_Year = mCONFERENCEList[i].CFLeague_Year.Trim(),
                    ChampionsLOGO = mCONFERENCEList[i].CFChampions.Trim(),
                    Champions = mCONFERENCEList[i].CFChampions.Trim(),
                    Second_PlaceLOGO = mCONFERENCEList[i].CFSecond_Place.Trim(),
                    Second_Place = mCONFERENCEList[i].CFSecond_Place.Trim(),
                    Remark = mCONFERENCEList[i].CFRemark.Trim()
                });
            }
            grdEmployee.ItemsSource = uiList;
            grdEmployee.ScrollIntoView(grdEmployee.Items[grdEmployee.Items.Count - 1]);

        }

        private void SetDataGridRankingUI()
        {
            List<EnglandTotalData> uList = new List<EnglandTotalData>();

            for (int i = 0; i < mCONFERENCERANKINGList.Count; i++)
            {
                CONFERENCE_RANKING clr = mCONFERENCERANKINGList[i];

                uList.Add(new EnglandTotalData()
                {
                    Ranking = int.Parse(mCONFERENCERANKINGList[i].CFRRanking.Trim()),
                    Team_Logo = mCONFERENCERANKINGList[i].CFRTeam_Name.Trim(),
                    Team_Name = mCONFERENCERANKINGList[i].CFRTeam_Name.Trim(),
                    Champions_CNT = mCONFERENCERANKINGList[i].CFRChampions_CNT.Trim(),
                    Second_Place_CNT = mCONFERENCERANKINGList[i].CFRRunnerUp_CNT.Trim()
                });
            }
            Ranking_DataGrid.ItemsSource = uList;
        }

        private void Row_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            int seelctedIndex = ((DataGrid)sender).SelectedIndex;

            CONFERENCE cc = mCONFERENCEList[seelctedIndex];
            League_Year_textBox.Text = cc.CFLeague_Year.ToString();
            League_Year2_textBox.Text = cc.CFLeague_Year.ToLower();
            Champion_Name_Textbox.Text = cc.CFChampions.ToString();
            Runner_UP_Textbox.Text = cc.CFSecond_Place.ToString();
            Remark_Textbox.Text = cc.CFRemark.ToString();

            NpgsqlConnection conn = null;

            try
            {
                conn = new NpgsqlConnection(MainWindow.mConnString);
                conn.Open();

                string sql = "select Count(*) from CONFERENCE_LEAGUE t where t.champions = '" + Champion_Name_Textbox.Text + "' and t.league_year between '2021/22' and '" + League_Year_textBox.Text + "';";

                NpgsqlCommand RankCmd = new NpgsqlCommand(sql, conn);
                NpgsqlDataReader reader = RankCmd.ExecuteReader();

                string strWInsCount = string.Empty;

                while (reader.Read())
                {
                    strWInsCount = reader[0].ToString();
                }

                NumOfWins_Textbox.Text = strWInsCount;

                reader.Close();

                string Secsql = "select COUNT(*) from CONFERENCE_LEAGUE T where T.second_place = '" + Runner_UP_Textbox.Text + "' and t.league_year between '2021/22' and '" + League_Year2_textBox.Text + "';";

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

                BitmapImage bitmap = new BitmapImage(new Uri("Resources/" + image.TEAMNAME.ToString() + ".png", UriKind.Relative));
                ImageBrush brush = new ImageBrush(bitmap);
                imageRec.Fill = brush;

                BitmapImage runbit = new BitmapImage(new Uri("Resources/" + image.SECOND_TEAMNAME.ToLower() + ".png", UriKind.Relative));
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

        private void keyDown_Event(object sender, KeyEventArgs e)
        {
            if(e.Key == Key.Escape)
            {
                this.Close();
            }
            if(e.Key == Key.S)
            {
                SearchFunc();
            }
            else if(e.Key == Key.C)
            {
                this.Close();
                SUPER_CUP cl = new SUPER_CUP();
                cl.WindowStartupLocation = WindowStartupLocation.CenterScreen;
                cl.Show();
            }
            else if(e.Key == Key.X)
            {
                this.Close();
                EUROPA_LEAGUE fa = new EUROPA_LEAGUE();
                fa.WindowStartupLocation = WindowStartupLocation.CenterScreen;
                fa.Show();
            }

        }

        private void Ranking_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            int selectedIndex = Ranking_DataGrid.SelectedIndex;
            CONFERENCE_RANKING ccr = mCONFERENCERANKINGList[selectedIndex];

            string teamName = ccr.CFRTeam_Name.Trim();
            string sOption = "CONFERENCE_LEAGUE";

            //DataPassProdCd(teamName);
            TeamCareer tc = new TeamCareer();
            tc.WindowStartupLocation = WindowStartupLocation.CenterScreen;
            tc.Show();
            tc.GetTeam(teamName, sOption);
        }

        private void Ranking_DataGrid_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            int selectedIndex = Ranking_DataGrid.SelectedIndex;
            CONFERENCE_RANKING ccr = mCONFERENCERANKINGList[selectedIndex];

            string teamName = ccr.CFRTeam_Name.Trim();
            string sOption = "CONFERENCE_LEAGUE";

            if(e.Key == Key.Enter)
            {
                //DataPassProdCd(teamName);
                TeamCareer tc = new TeamCareer();
                tc.WindowStartupLocation = WindowStartupLocation.CenterScreen;
                tc.Show();
                tc.GetTeam(teamName, sOption);
            }
        }
    }
}
