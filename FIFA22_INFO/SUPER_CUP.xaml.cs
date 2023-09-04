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
    /// SUPER_CUP.xaml에 대한 상호 작용 논리
    /// </summary>
    /// 
    public class SUPERCUP
    {
        public string SLeague_Year;
        public string SChampions;
        public string SSecond_Place;
        public string SRemark;

        public SUPERCUP(string sLeague_Year, string sChampions, string sSecond_Place, string sRemark)
        {
            SLeague_Year = sLeague_Year;
            SChampions = sChampions;
            SSecond_Place = sSecond_Place;
            SRemark = sRemark;
        }
    }

    public class SUPERCUP_RANKING
    {
        public string SRRanking;
        public string SRTeam_Name;
        public string SRChampions_CNT;
        public string SRRunnerUp_CNT;

        public SUPERCUP_RANKING(string sRRanking, string sRTeam_Name, string sRChampions_CNT, string sRRunnerUp_CNT)
        {
            SRRanking = sRRanking;
            SRTeam_Name = sRTeam_Name;
            SRChampions_CNT = sRChampions_CNT;
            SRRunnerUp_CNT = sRRunnerUp_CNT;
        }
    }

    public partial class SUPER_CUP : Window
    {
        public static List<SUPERCUP> mSUPERCUPList = new List<SUPERCUP>();
        public static List<SUPERCUP_RANKING> mSUPERCUPRANKINGList = new List<SUPERCUP_RANKING>();

        public string m_sTeamName = string.Empty;
        public string m_sOption = string.Empty;

        public SUPER_CUP()
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
            List<Premier_League_Data> uiList = new List<Premier_League_Data>();

            for (int i = 0; i < mSUPERCUPList.Count; i++)
            {
                SUPERCUP cl = mSUPERCUPList[i];

                uiList.Add(new Premier_League_Data()
                {
                    League_Year = mSUPERCUPList[i].SLeague_Year.Trim(),
                    ChampionsLOGO = mSUPERCUPList[i].SChampions.Trim(),
                    Champions = mSUPERCUPList[i].SChampions.Trim(),
                    Second_PlaceLOGO = mSUPERCUPList[i].SSecond_Place.Trim(),
                    Second_Place = mSUPERCUPList[i].SSecond_Place.Trim(),
                    Remark = mSUPERCUPList[i].SRemark.Trim()
                });
            }
            grdEmployee.ItemsSource = uiList;
            grdEmployee.ScrollIntoView(grdEmployee.Items[grdEmployee.Items.Count - 1]);
        }

        private void SetDataGridRankingUI()

        {
            List<EnglandTotalData> uList = new List<EnglandTotalData>();

            for (int i = 0; i < mSUPERCUPRANKINGList.Count; i++)
            {
                SUPERCUP_RANKING clr = mSUPERCUPRANKINGList[i];

                uList.Add(new EnglandTotalData()
                {
                    Ranking = int.Parse(mSUPERCUPRANKINGList[i].SRRanking.Trim()),
                    Team_Logo = mSUPERCUPRANKINGList[i].SRTeam_Name.Trim(),
                    Team_Name = mSUPERCUPRANKINGList[i].SRTeam_Name.Trim(),
                    Champions_CNT = mSUPERCUPRANKINGList[i].SRChampions_CNT.Trim(),
                    Second_Place_CNT = mSUPERCUPRANKINGList[i].SRRunnerUp_CNT.Trim()
                });
            }
            Ranking_DataGrid.ItemsSource = uList;

        }

        private void Row_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            int seelctedIndex = ((DataGrid)sender).SelectedIndex;

            SUPERCUP cc = mSUPERCUPList[seelctedIndex];
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

                string sql = "select Count(*) from SUPER_CUP t where t.champions = '" + Champion_Name_Textbox.Text + "' and t.league_year between '2021/22' and '" + League_Year_textBox.Text + "';";

                NpgsqlCommand RankCmd = new NpgsqlCommand(sql, conn);

                NpgsqlDataReader reader = RankCmd.ExecuteReader();

                string strWInsCount = string.Empty;

                while (reader.Read())
                {
                    strWInsCount = reader[0].ToString();
                }

                NumOfWins_Textbox.Text = strWInsCount;

                reader.Close();

                string Secsql = "select COUNT(*) from SUPER_CUP T where T.second_place = '" + Runner_UP_Textbox.Text + "' and t.league_year between '2021/22' and '" + League_Year2_textBox.Text + "';";

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

        private void SearchFunc()
        {
            NpgsqlConnection conn = new NpgsqlConnection(MainWindow.mConnString);
            conn.Open();
            List<string> list = new List<string>();

            string sql = "select * from super_cup t order by league_year";

            NpgsqlCommand cmd = new NpgsqlCommand(sql, conn);
            NpgsqlDataReader reader = cmd.ExecuteReader();

            mSUPERCUPList.Clear();

            while (reader.Read())
            {
                SUPERCUP cc = new SUPERCUP(reader[0].ToString(),
                    reader[1].ToString(),
                    reader[2].ToString(),
                    reader[3].ToString());

                mSUPERCUPList.Add(cc);
            }

            SetDataGridUsingQueryResultList();

            reader.Close();

            // DataGrid에 있는 최선 정보 txt파일에 저장
            string sOption = "SUPER_CUP";

            string path = AppDomain.CurrentDomain.BaseDirectory + "\\Scripts\\" + sOption + ".txt";

            try
            {
                FileStream stream = File.Create(path);
                stream.Close();

                if (File.Exists(path))
                {
                    for (int i = 0; i < mSUPERCUPList.Count; i++)
                    {
                        File.AppendAllText(path, mSUPERCUPList[i].SLeague_Year.Trim() + "\n");
                        File.AppendAllText(path, mSUPERCUPList[i].SChampions.Trim() + "\n");
                        File.AppendAllText(path, mSUPERCUPList[i].SSecond_Place.Trim() + "\n");
                        File.AppendAllText(path, mSUPERCUPList[i].SRemark.Trim() + "\n");
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);

            }

            string Ranksql = "select rank() over( order by t.SUPER_CUP_WINNER_CNT desc) Ranking, t.team_name, t.SUPER_CUP_WINNER_CNT , t.SUPER_CUP_RUN_CNT  " +
               "from TOTAL t " +
               "where t.SUPER_CUP_RUN_CNT != 0 OR t.SUPER_CUP_WINNER_CNT != 0 " +
               "order by t.SUPER_CUP_WINNER_CNT desc, " +
               "t.SUPER_CUP_RUN_CNT desc;";

            NpgsqlCommand RankCmd = new NpgsqlCommand(Ranksql, conn);
            NpgsqlDataReader Rankreader = RankCmd.ExecuteReader();

            mSUPERCUPRANKINGList.Clear();

            while (Rankreader.Read())
            {
                SUPERCUP_RANKING ccr = new SUPERCUP_RANKING(Rankreader[0].ToString(),
                    Rankreader[1].ToString(),
                    Rankreader[2].ToString(),
                    Rankreader[3].ToString());

                mSUPERCUPRANKINGList.Add(ccr);
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
                CHAMPIONS_LEAGUE cl = new CHAMPIONS_LEAGUE();
                cl.WindowStartupLocation = WindowStartupLocation.CenterScreen;
                cl.Show();
            }
            else if (e.Key == Key.X)
            {
                this.Close();
                CONFERENCE_LEAGUE con = new CONFERENCE_LEAGUE();
                con.WindowStartupLocation = WindowStartupLocation.CenterScreen;
                con.Show();
            }
        }

        private void Ranking_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            int selectedIndex = Ranking_DataGrid.SelectedIndex;
            SUPERCUP_RANKING ccr = mSUPERCUPRANKINGList[selectedIndex];

            m_sTeamName = ccr.SRTeam_Name.Trim();
            m_sOption = "SUPER_CUP";

            //DataPassProdCd(teamName);
            TeamCareer tc = new TeamCareer();
            tc.WindowStartupLocation = WindowStartupLocation.CenterScreen;
            tc.Show();
            tc.GetTeam(m_sTeamName, m_sOption);
        }

        private void Ranking_DataGrid_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            int selectedIndex = Ranking_DataGrid.SelectedIndex;
            SUPERCUP_RANKING ccr = mSUPERCUPRANKINGList[selectedIndex];

            m_sTeamName = ccr.SRTeam_Name.Trim();
            m_sOption = "SUPER_CUP";

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
