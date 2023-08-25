#define WORKING_HOME

using Npgsql;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
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
    public class PREMIER
    {
        public string LLeague_Year;
        public string LChampions;
        public string LSecond_Place;
        public string LRemark;

        public PREMIER(string lLeague_Year, string lChampions, string lSecond_Place, string lRemark)
        {
            LLeague_Year = lLeague_Year;
            LChampions = lChampions;
            LSecond_Place = lSecond_Place;
            LRemark = lRemark;
        }
    }

    public class PREMIER_RANKING
    {
        public string LRanking;
        public string LTeam_Name;
        public string LChampions_CNT;
        public string LRunnerUp_CNT;

        public PREMIER_RANKING(string lRanking, string lTeam_Name, string lChampions_CNT, string lRunnerUp_CNT)
        {
            LRanking = lRanking;
            LTeam_Name = lTeam_Name;
            LChampions_CNT = lChampions_CNT;
            LRunnerUp_CNT = lRunnerUp_CNT;
        }
    }

    /// <summary>
    /// Premier_League.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class Premier_League : Window
    {
        public static List<PREMIER> mPREMEIRList = new List<PREMIER>();
        public static List<PREMIER_RANKING> mPRERANKINGList = new List<PREMIER_RANKING>();

        public string m_sTeamName = string.Empty;
        public string m_sOption = string.Empty;

        public Premier_League()
        {
            InitializeComponent();
        }

        private void ToMiniButton_Click(object sender, RoutedEventArgs e)
        {
            this.WindowState = System.Windows.WindowState.Minimized;
        }

        private void CloseButton_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void Rectangle_MouseDown(object sender, MouseButtonEventArgs e)
        {
            this.DragMove();
        }

        private void SetDataGridUsingQueryResultList()
        {
            List<Premier_League_Data> uiList = new List<Premier_League_Data>();

            for(int i=0; i< mPREMEIRList.Count; i++)
            {
                PREMIER pr = mPREMEIRList[i];

                uiList.Add(new Premier_League_Data()
                {
                    League_Year = mPREMEIRList[i].LLeague_Year.Trim(),
                    ChampionsLOGO = mPREMEIRList[i].LChampions.Trim(),
                    Champions = mPREMEIRList[i].LChampions.Trim(),
                    Second_PlaceLOGO = mPREMEIRList[i].LSecond_Place.Trim(),
                    Second_Place = mPREMEIRList[i].LSecond_Place.Trim(),
                    Remark = mPREMEIRList[i].LRemark.Trim()  
                });
            }

            grdEmployee.ItemsSource = uiList;
            grdEmployee.ScrollIntoView(grdEmployee.Items[grdEmployee.Items.Count - 1]);
        }


        // Set DataGrid Ranking UI
        private void SetDataGridRankingUI()
        {
            List<EnglandTotalData> uList = new List<EnglandTotalData>();

            for (int i = 0; i < mPRERANKINGList.Count; i++)
            {
                PREMIER_RANKING far = mPRERANKINGList[i];

                uList.Add(new EnglandTotalData()
                {
                    Ranking = int.Parse(mPRERANKINGList[i].LRanking.Trim()),
                    Team_Logo = mPRERANKINGList[i].LTeam_Name.Trim(),
                    Team_Name = mPRERANKINGList[i].LTeam_Name.Trim(),
                    Champions_CNT = mPRERANKINGList[i].LChampions_CNT.Trim(),
                    Second_Place_CNT = mPRERANKINGList[i].LRunnerUp_CNT.Trim()
                });
            }

            Ranking_DataGrid.ItemsSource = uList;
        }

        private void Search_Click(object sender, RoutedEventArgs e)
        {
            SearchFunc();
        }

        private void Row_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            int seelctedIndex = ((DataGrid)sender).SelectedIndex;

            PREMIER fa = mPREMEIRList[seelctedIndex];

            League_Year_textBox.Text = fa.LLeague_Year.ToString();
            League_Year2_textBox.Text = fa.LLeague_Year.ToLower();
            Champion_Name_Textbox.Text = fa.LChampions.ToString();
            Runner_UP_Textbox.Text = fa.LSecond_Place.ToString();
            Remark_Textbox.Text = fa.LRemark.ToString();

            NpgsqlConnection conn = null;
            try
            {
                conn = new NpgsqlConnection(MainWindow.mConnString);
                conn.Open();

                string sql = "select Count(*) from Premier_League t where t.champions = '" + Champion_Name_Textbox.Text + "' and t.league_year between '2021/22' and '" + League_Year_textBox.Text + "';";

                NpgsqlCommand RankCmd = new NpgsqlCommand(sql, conn);
                NpgsqlDataReader reader = RankCmd.ExecuteReader();

                string strWInsCount = string.Empty;

                while (reader.Read())
                {
                    strWInsCount = reader[0].ToString();
                }

                NumOfWins_Textbox.Text = strWInsCount;

                reader.Close();

                string Secsql = "select COUNT(*) from Premier_League T where T.second_place = '" + Runner_UP_Textbox.Text + "' and t.league_year between '2021/22' and '" + League_Year2_textBox.Text + "';";

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

                BitmapImage bitmap = new BitmapImage(new Uri("Image/England_Team/" + image.TEAMNAME.ToString() + ".png", UriKind.Relative));
                ImageBrush brush = new ImageBrush(bitmap);
                imageRec.Fill = brush;

                BitmapImage runbit = new BitmapImage(new Uri("Image/England_Team/" + image.SECOND_TEAMNAME.ToLower() + ".png", UriKind.Relative));
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
            List<string> list = new List<string>();

            string sql = "select * from premier_league ORDER BY LEAGUE_YEAR;";

            NpgsqlConnection conn = new NpgsqlConnection(MainWindow.mConnString);

            conn.Open();

            NpgsqlCommand cmd = new NpgsqlCommand(sql, conn);
            NpgsqlDataReader reader = cmd.ExecuteReader();

            mPREMEIRList.Clear();

            while (reader.Read())
            {
                PREMIER pr = new PREMIER(reader[0].ToString().Trim(),
                    reader[1].ToString().Trim(),
                    reader[2].ToString().Trim(),
                    reader[3].ToString().Trim());

                mPREMEIRList.Add(pr);
            }

            SetDataGridUsingQueryResultList();

            reader.Close();
            // 보류 2023/08/14
            /*

            string Teamsql = "select distinct(champions) from "+ sOption + " union select distinct(SECOND_PLACE) from "+ sOption + " order by champions;";

            NpgsqlCommand TeamCmd = new NpgsqlCommand(Teamsql, conn);
            NpgsqlDataReader Teamreader = TeamCmd.ExecuteReader();

            List<string> TeamList = new List<string>();

            while (Teamreader.Read())
            {
                TeamList.Add(Teamreader[0].ToString().Trim());
            }

            for(int i=0; i<TeamList.Count; i++)
            {
                string str = "select (select count(*) from " + sOption + " where champions = '" + TeamList[i] + "')," +
                   "(select count(*) from " + sOption + " where second_place  = '" + TeamList[i] + "')";
            }
             */

            // DataGrid에 있는 최선 정보 txt파일에 저장
            string sOption = "PREMIER_LEAGUE";

            string path = AppDomain.CurrentDomain.BaseDirectory + "\\Scripts\\" + sOption + ".txt";

            FileStream stream = File.Create(path);
            stream.Close();

            if (File.Exists(path))
            {
                for (int i = 0; i < mPREMEIRList.Count; i++)
                {
                    File.AppendAllText(path, mPREMEIRList[i].LLeague_Year.Trim() + "\n");
                    File.AppendAllText(path, mPREMEIRList[i].LChampions.Trim() + "\n");
                    File.AppendAllText(path, mPREMEIRList[i].LSecond_Place.Trim() + "\n");
                    File.AppendAllText(path, mPREMEIRList[i].LRemark.Trim() + "\n");
                }
            }

            string Ranksql = "select rank() over( order by t.premier_league_winner_cnt desc) Ranking, t.team_name, t.premier_league_winner_cnt , t.premier_league_run_cnt  " +
                "from england_total t " +
                "where t.premier_league_run_cnt != 0 OR t.premier_league_winner_cnt != 0 " +
                "order by t.premier_league_winner_cnt desc, " +
                "t.premier_league_run_cnt desc;";

            NpgsqlCommand RankCmd = new NpgsqlCommand(Ranksql, conn);
            NpgsqlDataReader Rankreader = RankCmd.ExecuteReader();

            mPRERANKINGList.Clear();

            while (Rankreader.Read())
            {
                PREMIER_RANKING far = new PREMIER_RANKING(Rankreader[0].ToString(),
                    Rankreader[1].ToString(),
                    Rankreader[2].ToString(),
                    Rankreader[3].ToString());

                mPRERANKINGList.Add(far);
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
            else if(e.Key == Key.C)
            {
                this.Close();
                EMIRATES_FA_CUP fa = new EMIRATES_FA_CUP();
                fa.WindowStartupLocation = WindowStartupLocation.CenterScreen;
                fa.Show();
            }
            else if(e.Key == Key.X)
            {
                this.Close();
                CARABAO_CUP cc = new CARABAO_CUP();
                cc.WindowStartupLocation = WindowStartupLocation.CenterScreen;
                cc.Show();
            }
        }

        private void Ranking_DataGrid_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            int selectedIndex = Ranking_DataGrid.SelectedIndex;
            PREMIER_RANKING ccr = mPRERANKINGList[selectedIndex];

            string teamName = ccr.LTeam_Name.Trim();
            string sOption = "PREMIER_LEAGUE";

            //DataPassProdCd(teamName);
            TeamCareer tc = new TeamCareer();
            tc.WindowStartupLocation = WindowStartupLocation.CenterScreen;
            tc.Show();
            tc.GetTeam(teamName, sOption);
        }

        private void Ranking_DataGrid_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            int selectedIndex = Ranking_DataGrid.SelectedIndex;
            PREMIER_RANKING ccr = mPRERANKINGList[selectedIndex];

            string teamName = ccr.LTeam_Name.Trim();
            string sOption = "PREMIER_LEAGUE";

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
